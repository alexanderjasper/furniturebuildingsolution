using FurnitureBuildingSolution.Dtos.Bookcase;
using FurnitureBuildingSolution.Entities;
using FurnitureBuildingSolution.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static FurnitureBuildingSolution.Helpers.SkyCivJsonInputObject;
using Material = FurnitureBuildingSolution.Helpers.SkyCivJsonInputObject.Material;

namespace FurnitureBuildingSolution.Services
{
    public interface ISkycivService
    {
        Dictionary<int, double> GetDisplacements(BookcaseDto bookcase);
    }

    public class SkycivService : ISkycivService
    {
        private readonly AppSettings _appSettings;
        private readonly IBookcaseMapper _bookcaseMapper;

        public SkycivService(IOptions<AppSettings> appSettings, IBookcaseMapper bookcaseMapper)
        {
            _appSettings = appSettings.Value;
            _bookcaseMapper = bookcaseMapper;
        }

        public Dictionary<int, double> GetDisplacements(BookcaseDto bookcaseDto)
        {
            var bookcase = _bookcaseMapper.MapToEntity(bookcaseDto);
            var inputObject = new Rootobject();
            FillConfiguration(ref inputObject);
            var memberIdDictionary = FillDataAndReturnMemberIdDictionary(ref inputObject, bookcase);
            AddSupports(ref inputObject, bookcase);
            var inputJson = JsonConvert.SerializeObject(inputObject);

            var apiResponse = CallSkycivAPIV2Async(inputObject).Result.ToString();
            var returnDictionary = new Dictionary<int, double>();

            dynamic responseObject = JsonConvert.DeserializeObject(apiResponse);
            if (responseObject.results == null)
            {
                throw new AppException("Could not get displacements from external service.");
            }
            JObject displacementsResponse = responseObject.results.First.member_minimums.displacement_y;

            foreach (var entry in displacementsResponse)
            {
                var plate = bookcase.Plates.Single(bookcasePlate => bookcasePlate.Id == memberIdDictionary[entry.Key]);
                var displacementValue = (double)entry.Value;
                if (returnDictionary.ContainsKey(memberIdDictionary[entry.Key]) && returnDictionary[memberIdDictionary[entry.Key]] <= displacementValue)
                {
                    continue;
                }
                else if (plate.IsHorizontal)
                {
                    returnDictionary[memberIdDictionary[entry.Key]] = displacementValue;
                }
            }

            return returnDictionary;
        }

        private void AddDistributedLoad(ref Rootobject inputObject, int memberId)
        {
            var distributedLoad = new Distributed_Load()
            {
                member = memberId,
                x_mag_A = 0,
                y_mag_A = (double)-0.5,
                z_mag_A = 0,
                x_mag_B = 0,
                y_mag_B = (double)-0.5,
                z_mag_B = 0,
                position_A = 0,
                position_B = 100,
                load_group = 1,
                axes = "global"
            };
            inputObject.distributed_loads.TryAdd(memberId.ToString(), distributedLoad);
        }

        private async Task<object> CallSkycivAPIV2Async(Rootobject inputJson)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://solver.skyciv.com/structural/v2");
            var response = await client.PostAsJsonAsync(
                "", inputJson);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        private void AddSupports(ref Rootobject inputObject, Bookcase bookcase)
        {
            inputObject.supports = new System.Dynamic.ExpandoObject();
            var nextSupportId = 0;
            foreach (var corner in bookcase.Corners.Where(corner => corner.y == 0))
            {
                inputObject.supports.TryAdd(nextSupportId.ToString(), new Support()
                {
                    node = corner.Id.Value,
                    restraint_code = "FFFFFR",
                    tx = 0,
                    ty = 0,
                    tz = 0,
                    rx = 0,
                    ry = 0,
                    rz = 0
                });
                nextSupportId++;
            }
        }

        private Dictionary<string, int> FillDataAndReturnMemberIdDictionary(ref Rootobject inputObject, Bookcase bookcase)
        {
            inputObject.nodes = new System.Dynamic.ExpandoObject();
            inputObject.members = new System.Dynamic.ExpandoObject();
            inputObject.distributed_loads = new System.Dynamic.ExpandoObject();
            foreach (var corner in bookcase.Corners)
            {
                inputObject.nodes.TryAdd(corner.Id.ToString(), new Node()
                {
                    x = ((double)corner.x) / 1000,
                    y = ((double)corner.y) / 1000,
                    z = 0
                });
            }
            var memberIdToPlateId = new Dictionary<string, int>();
            var nextMemberId = 0;
            foreach (var plate in bookcase.Plates)
            {
                var intermediateCorners = bookcase.GetIntermediateCorners(plate);
                var numberOfPlates = intermediateCorners.Count() + 1;

                for (int i = 0; i < numberOfPlates; i++)
                {
                    var isFirstPlate = i == 0;
                    var isLastPlate = i == numberOfPlates - 1;
                    var nextCornerId = isLastPlate ? plate.End.Id.Value : intermediateCorners[i].Id.Value;

                    var member = new Member()
                    {
                        type = "normal",
                        node_A = isFirstPlate ? plate.Start.Id.Value : intermediateCorners[i - 1].Id.Value,
                        node_B = isLastPlate ? plate.End.Id.Value : intermediateCorners[i].Id.Value,
                        section_id = 1,
                        rotation_angle = 0,
                        fixity_A = isFirstPlate && plate.IsHorizontal ? "FFFFFR" : "FFFFFF",
                        fixity_B = isLastPlate && plate.IsHorizontal ? "FFFFFR" : "FFFFFF",
                        offset_Ax = 0,
                        offset_Ay = 0,
                        offset_Az = 0,
                        offset_Bx = 0,
                        offset_By = 0,
                        offset_Bz = 0
                    };
                    inputObject.members.TryAdd(nextMemberId.ToString(), member);
                    AddDistributedLoad(ref inputObject, nextMemberId);
                    memberIdToPlateId.Add(nextMemberId.ToString(), plate.Id.Value);
                    nextMemberId++;
                }
            }
            return memberIdToPlateId;
        }

        private void FillConfiguration(ref Rootobject inputObject)
        {
            inputObject.auth = new Auth()
            {
                username = _appSettings.SkyCivSettings.Username,
                key = _appSettings.SkyCivSettings.APIKey
            };
            inputObject.file_management = new File_Management()
            {
                save_file = true
            };
            inputObject.settings = new Settings()
            {
                analysis_type = 0,
                accurate_buckling_shape = false,
                units = "metric",
                precision = 0,
                precision_values = 5,
                evaluation_points = 5,
                non_linear_tolerance = 4,
                auto_stabilize_model = false,
                analysis_report = false
            };
            inputObject.materials = new System.Dynamic.ExpandoObject();
            inputObject.materials.TryAdd("1", new Material()
            {
                name = "melamin19",
                density = 900,
                elasticity_modulus = 2000,
                poissons_ratio = 0.3F,
            });
            inputObject.sections = new System.Dynamic.ExpandoObject();
            inputObject.sections.TryAdd("1", new Section()
            {
                name = "melamin19",
                area = 5700,
                Iy = 42750000,
                Iz = 145800,
                J = 658110,
                material_id = 1
            });
            inputObject.self_weight = new Self_Weight()
            {
                enabled = true,
                x = 0,
                y = -1,
                z = 0
            };
        }
    }
}
