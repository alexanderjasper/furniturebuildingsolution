using AutoMapper;
using FurnitureBuildingSolution.Blueprint;
using FurnitureBuildingSolution.Database;
using FurnitureBuildingSolution.Dtos.Bookcase;
using FurnitureBuildingSolution.Entities;
using FurnitureBuildingSolution.Helpers;
using FurnitureBuildingSolution.Repositories;
using FurnitureBuildingSolution.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FurnitureBuildingUnitTests
{
    public class ConfiguratorJsonExportTests
    {
        private BlueprintBookcase _blueprint;

        [Fact]
        public void CorrectNumberOfPlates()
        {
            var regularPlates = _blueprint.Plates.Count(p => p.PlateId != null);
            var supports = _blueprint.Plates.Count(p => p.CompartmentId != null);
            var none = _blueprint.Plates.Count(p => p.PlateId == null && p.CompartmentId == null);
            var both = _blueprint.Plates.Count(p => p.PlateId != null && p.CompartmentId != null);

            Assert.Equal(15, regularPlates);
            Assert.Equal(10, supports);
            Assert.Equal(0, none);
            Assert.Equal(0, both);
        }

        [Fact]
        public void CorrectSupports()
        {
            var supports = _blueprint.Plates.Where(p => p.CompartmentId != null).OrderBy(p => p.CompartmentId);
            Assert.Collection(
                supports,
                p => Xunit.Assert.Equal(0, p.CompartmentId),
                p => Xunit.Assert.Equal(1, p.CompartmentId),
                p => Xunit.Assert.Equal(2, p.CompartmentId),
                p => Xunit.Assert.Equal(3, p.CompartmentId),
                p => Xunit.Assert.Equal(4, p.CompartmentId),
                p => Xunit.Assert.Equal(5, p.CompartmentId),
                p => Xunit.Assert.Equal(6, p.CompartmentId),
                p => Xunit.Assert.Equal(7, p.CompartmentId),
                p => Xunit.Assert.Equal(8, p.CompartmentId),
                p => Xunit.Assert.Equal(11, p.CompartmentId)
            );
        }

        [Fact]
        public void CorrectSupportLengths()
        {
            var supports = _blueprint.Plates.Where(p => p.CompartmentId != null).OrderBy(p => p.CompartmentId);
            Assert.Collection(
                supports,
                p => Xunit.Assert.Equal(435, p.Length),
                p => Xunit.Assert.Equal(885, p.Length),
                p => Xunit.Assert.Equal(435, p.Length),
                p => Xunit.Assert.Equal(435, p.Length),
                p => Xunit.Assert.Equal(435, p.Length),
                p => Xunit.Assert.Equal(435, p.Length),
                p => Xunit.Assert.Equal(435, p.Length),
                p => Xunit.Assert.Equal(435, p.Length),
                p => Xunit.Assert.Equal(435, p.Length),
                p => Xunit.Assert.Equal(435, p.Length)
            );
        }

        [Fact]
        public void CorrectSocketShifts()
        {
            var plate2Sockets = GetBlueprintByPlateId(2).Sockets.OrderBy(s => s.X).ThenBy(s => s.Y).ToArray();
            var plate3Sockets = GetBlueprintByPlateId(3).Sockets.OrderBy(s => s.X).ThenBy(s => s.Y).ToArray();
            var plate5Sockets = GetBlueprintByPlateId(5).Sockets.OrderBy(s => s.X).ThenBy(s => s.Y).ToArray();

            //Don't shift left since there is no adjacent plate.
            Assert.Equal(-100, plate2Sockets[0].Y);
            Assert.Equal(100, plate2Sockets[1].Y);

            //Don't shift right since there is no adjacent plate.
            Assert.Equal(-100, plate5Sockets[2].Y);
            Assert.Equal(100, plate5Sockets[3].Y);

            //Shift right inwards since there is an adjacent plate.
            Assert.Equal(-96.5, plate2Sockets[2].Y);
            Assert.Equal(96.5, plate2Sockets[3].Y);

            //Shift left outwards since there is an adjacent plate.
            Assert.Equal(-103.5, plate3Sockets[0].Y);
            Assert.Equal(103.5, plate3Sockets[1].Y);
        }

        public ConfiguratorJsonExportTests()
        {
            _blueprint = GetStandardBlueprint();
        }

        private BlueprintPlate GetBlueprintByPlateId(int id)
        {
            return _blueprint.Plates.Single(p => p.PlateId == id);
        }

        private BlueprintPlate GetBlueprintByCompartmentId(int id)
        {
            return _blueprint.Plates.Single(p => p.CompartmentId == id);
        }

        private BlueprintBookcase GetStandardBlueprint()
        {
            var material = new Material
            {
                Id = 1,
                Name = "Test Material",
                Thickness = 18,
                PricePerSquareMeter = 100
            };
            var bookcase = new Bookcase(material)
            {
                Id = 0,
                Name = "Test",
                PlateDepth = 300
            };
            bookcase.Corners = new List<Corner>(){
                new Corner(-200, -200, 0),
                new Corner(250, -200, 1),
                new Corner(1150, -200, 2),
                new Corner(-200, 300, 3),
                new Corner(250, 300, 4),
                new Corner(700, 300, 5),
                new Corner(1150, 300, 6),
                new Corner(250, 600, 7),
                new Corner(700, 600, 8),
                new Corner(1150,600,9),
                new Corner(-200,900,10),
                new Corner(250,900,11),
                new Corner(500,900,12),
                new Corner(700,900,13),
                new Corner(-200,1200,14),
                new Corner(250,1200,15),
                new Corner(700,1200,16),
                new Corner(1150,1200,17),
                new Corner(-200,1500,18),
                new Corner(250,1500,19),
                new Corner(500,1500,20),
                new Corner(700,1500,21),
                new Corner(1150,1500,22),
            };
            bookcase.Plates = new List<Plate>()
            {
                new Plate(bookcase) {
                    Start = bookcase.Corners[0],
                    End = bookcase.Corners[2],
                    InnerPlateStart = false,
                    InnerPlateEnd = false,
                    Id = 0
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[3],
                    End = bookcase.Corners[6],
                    InnerPlateStart = true,
                    InnerPlateEnd = true,
                    Id = 1
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[7],
                    End = bookcase.Corners[8],
                    InnerPlateStart = true,
                    InnerPlateEnd = true,
                    Id = 2
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[8],
                    End = bookcase.Corners[9],
                    InnerPlateStart = true,
                    InnerPlateEnd = true,
                    Id = 3
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[10],
                    End = bookcase.Corners[11],
                    InnerPlateStart = true,
                    InnerPlateEnd = true,
                    Id = 4
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[11],
                    End = bookcase.Corners[13],
                    InnerPlateStart = true,
                    InnerPlateEnd = true,
                    Id = 5
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[14],
                    End = bookcase.Corners[15],
                    InnerPlateStart = true,
                    InnerPlateEnd = true,
                    Id = 6
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[16],
                    End = bookcase.Corners[17],
                    InnerPlateStart = true,
                    InnerPlateEnd = true,
                    Id = 7
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[18],
                    End = bookcase.Corners[22],
                    InnerPlateStart = false,
                    InnerPlateEnd = false,
                    Id = 8
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[0],
                    End = bookcase.Corners[18],
                    InnerPlateStart = true,
                    InnerPlateEnd = true,
                    Id = 9
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[1],
                    End = bookcase.Corners[4],
                    InnerPlateStart = true,
                    InnerPlateEnd = true,
                    Id = 10
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[4],
                    End = bookcase.Corners[19],
                    InnerPlateStart = true,
                    InnerPlateEnd = true,
                    Id = 11
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[12],
                    End = bookcase.Corners[20],
                    InnerPlateStart = true,
                    InnerPlateEnd = true,
                    Id = 12
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[5],
                    End = bookcase.Corners[21],
                    InnerPlateStart = true,
                    InnerPlateEnd = true,
                    Id = 13
                },
                new Plate(bookcase) {
                    Start = bookcase.Corners[2],
                    End = bookcase.Corners[22],
                    InnerPlateStart = true,
                    InnerPlateEnd = true,
                    Id = 14
                }
            };
            bookcase.Compartments = new List<Compartment>(){
                new Compartment(bookcase){
                    Id = 0,
                    Top = bookcase.Plates[1],
                    Bottom = bookcase.Plates[0],
                    Left = bookcase.Plates[9],
                    Right = bookcase.Plates[10]
                },
                new Compartment(bookcase){
                    Id = 1,
                    Top = bookcase.Plates[1],
                    Bottom = bookcase.Plates[0],
                    Left = bookcase.Plates[10],
                    Right = bookcase.Plates[14]
                },
                new Compartment(bookcase){
                    Id = 2,
                    Top = bookcase.Plates[4],
                    Bottom = bookcase.Plates[1],
                    Left = bookcase.Plates[9],
                    Right = bookcase.Plates[11]
                },
                new Compartment(bookcase){
                    Id = 3,
                    Top = bookcase.Plates[2],
                    Bottom = bookcase.Plates[1],
                    Left = bookcase.Plates[11],
                    Right = bookcase.Plates[13]
                },
                new Compartment(bookcase){
                    Id = 4,
                    Top = bookcase.Plates[5],
                    Bottom = bookcase.Plates[2],
                    Left = bookcase.Plates[11],
                    Right = bookcase.Plates[13]
                },
                new Compartment(bookcase){
                    Id = 5,
                    Top = bookcase.Plates[3],
                    Bottom = bookcase.Plates[1],
                    Left = bookcase.Plates[13],
                    Right = bookcase.Plates[14]
                },
                new Compartment(bookcase){
                    Id = 6,
                    Top = bookcase.Plates[7],
                    Bottom = bookcase.Plates[3],
                    Left = bookcase.Plates[13],
                    Right = bookcase.Plates[14]
                },
                new Compartment(bookcase){
                    Id = 7,
                    Top = bookcase.Plates[6],
                    Bottom = bookcase.Plates[4],
                    Left = bookcase.Plates[9],
                    Right = bookcase.Plates[11]
                },
                new Compartment(bookcase){
                    Id = 8,
                    Top = bookcase.Plates[8],
                    Bottom = bookcase.Plates[6],
                    Left = bookcase.Plates[9],
                    Right = bookcase.Plates[11]
                },
                new Compartment(bookcase){
                    Id = 9,
                    Top = bookcase.Plates[8],
                    Bottom = bookcase.Plates[5],
                    Left = bookcase.Plates[11],
                    Right = bookcase.Plates[12]
                },
                new Compartment(bookcase){
                    Id = 10,
                    Top = bookcase.Plates[8],
                    Bottom = bookcase.Plates[5],
                    Left = bookcase.Plates[12],
                    Right = bookcase.Plates[13]
                },
                new Compartment(bookcase){
                    Id = 11,
                    Top = bookcase.Plates[8],
                    Bottom = bookcase.Plates[7],
                    Left = bookcase.Plates[13],
                    Right = bookcase.Plates[14]
                }
            };

            foreach (var plate in bookcase.Plates)
            {
                plate.IntermediateCorners = bookcase.GetIntermediateCorners(plate);
            }

            var mapperProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            var mapper = new Mapper(configuration);

            var mockEmailService = new Mock<IEmailService>();
            var bookcaseService = new BookcaseService(new Mock<BookcaseRepository>(new Mock<DataContext>(new DbContextOptions<DataContext>()).Object, new BookcaseMapper(mapper)).Object, new BookcaseMapper(mapper), mapper, mockEmailService.Object);

            var blueprintRequest = new BlueprintRequestDto
            {
                Id = bookcase.Id,
                PlateThickness = bookcase.PlateThickness,
                DoorPlateThickness = bookcase.DoorPlateThickness,
                IncludeSingleSided = true,
                IncludeDoubleSided = true
            };

            return bookcaseService.GetBlueprint(blueprintRequest);
        }
    }
}
