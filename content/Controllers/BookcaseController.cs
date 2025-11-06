using System;
using Microsoft.AspNetCore.Mvc;
using FurnitureBuildingSolution.Helpers;
using Microsoft.AspNetCore.Authorization;
using FurnitureBuildingSolution.Services;
using FurnitureBuildingSolution.Dtos.Bookcase;
using AutoMapper;
using System.Linq;

namespace FurnitureBuildingSolution.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookcaseController : ControllerBase
    {
        private readonly IBookcaseService _bookcaseService;
        private readonly IUserService _userService;
        private readonly ISkycivService _skycivService;
        private readonly IBookcaseMapper _bookcaseMapper;
        private readonly IMapper _autoMapper;

        public BookcaseController(IBookcaseService bookcaseService, ISkycivService skycivService, IBookcaseMapper bookcaseMapper, IMapper autoMapper, IUserService userService)
        {
            _bookcaseService = bookcaseService;
            _skycivService = skycivService;
            _bookcaseMapper = bookcaseMapper;
            _autoMapper = autoMapper;
            _userService = userService;
        }

        [Authorize]
        [HttpPost("save")]
        public IActionResult Save([FromBody] BookcaseDto bookcaseDto)
        {

            try
            {
                var userId = int.Parse(User.Identity.Name);

                var savedBookcase = _bookcaseService.Save(bookcaseDto, userId);

                return Ok(savedBookcase);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var listOfBookcases = _bookcaseService.GetAll(userId);
                return Ok(listOfBookcases);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("getData")]
        public IActionResult GetData()
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var user = _userService.GetById(userId);
                if (user.Role != Enums.Role.Admin)
                {
                    return Unauthorized(new { message = "Access denied. GetData is only allowed for admins." });
                }

                var bookcaseListData = _bookcaseService.GetAll().OrderByDescending(b => b.Id).ToList();
                return Ok(bookcaseListData);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("getMaterials")]
        public IActionResult GetMaterials()
        {
            try
            {
                var listOfMaterials = _bookcaseService.GetMaterials();
                return Ok(listOfMaterials);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("getBlueprint")]
        public IActionResult GetBlueprint(BlueprintRequestDto blueprintRequest)
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var user = _userService.GetById(userId);
                if (user.Role != Enums.Role.Admin)
                {
                    return Unauthorized(new { message = "Access denied. GetBlueprint is only allowed for admins." });
                }
                if (blueprintRequest.PlateThickness == null)
                {
                    throw new ArgumentException("PlateThickness not specified for blueprintRequest.");
                }
                if (blueprintRequest.DoorPlateThickness == null)
                {
                    throw new ArgumentException("DoorPlateThickness not specified for blueprintRequest.");
                }
                var blueprint = _bookcaseService.GetBlueprint(blueprintRequest);
                return Ok(blueprint);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("getDoorsBlueprint")]
        public IActionResult GetDoorsBlueprint(BlueprintRequestDto blueprintRequest)
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var user = _userService.GetById(userId);
                if (user.Role != Enums.Role.Admin)
                {
                    return Unauthorized(new { message = "Access denied. GetBlueprint is only allowed for admins." });
                }
                if (blueprintRequest.PlateThickness == null)
                {
                    throw new ArgumentException("PlateThickness not specified for blueprintRequest.");
                }
                if (blueprintRequest.DoorPlateThickness == null)
                {
                    throw new ArgumentException("DoorPlateThickness not specified for blueprintRequest.");
                }
                var blueprint = _bookcaseService.GetDoorsBlueprint(blueprintRequest.Id.Value, blueprintRequest.PlateThickness, blueprintRequest.DoorPlateThickness.Value);
                return Ok(blueprint);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("get")]
        public IActionResult Get(BookcaseDto bookcaseDto)
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var bookcaseFromDb = _bookcaseService.Get(bookcaseDto.Id.Value, userId);
                return Ok(bookcaseFromDb);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("getPublic")]
        public IActionResult GetPublic(BookcaseDto bookcaseDto)
        {
            try
            {
                var bookcaseFromDb = _bookcaseService.GetPublic(bookcaseDto.Id.Value);
                return Ok(bookcaseFromDb);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("getDisplacements")]
        public IActionResult GetDisplacements(BookcaseDto bookcaseDto)
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var bookcaseFromDb = _bookcaseService.Get(bookcaseDto.Id.Value, userId);
                var displacements = _skycivService.GetDisplacements(bookcaseFromDb);
                return Ok(displacements);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("deleteBookcase")]
        public IActionResult DeleteBookcase(BookcaseDto bookcaseDto)
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var listOfBookcases = _bookcaseService.DeleteBookcase(bookcaseDto.Id.Value, userId);
                return Ok(listOfBookcases);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
