using System;
using FurnitureBuildingSolution.Entities;
using FurnitureBuildingSolution.Database;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FurnitureBuildingSolution.Dtos.Bookcase;

namespace FurnitureBuildingSolution.Helpers
{
    public interface IBookcaseMapper
    {
        Bookcase MapToEntity(DbBookcase dbBookcase);
        DbBookcase MapToDb(Bookcase bookcase);
        BookcaseDto MapToDto(Bookcase bookcase);
        Bookcase MapToEntity(BookcaseDto bookcaseDto);
    }

    public class BookcaseMapper : IBookcaseMapper
    {
        private readonly IMapper _autoMapper;
        public BookcaseMapper(IMapper mapper)
        {
            _autoMapper = mapper;
        }

        public Bookcase MapToEntity(DbBookcase dbBookcase)
        {
            if (dbBookcase == null)
            {
                return null;
            }
            var corners = new List<Corner>();
            var plates = new List<Plate>();
            var compartments = new List<Compartment>();
            var material = _autoMapper.Map<Material>(dbBookcase.Material);
            var user = _autoMapper.Map<User>(dbBookcase.User);

            var bookcase = new Bookcase(material)
            {
                Id = dbBookcase.Id,
                Created = dbBookcase.Created,
                Modified = dbBookcase.Modified,
                Deleted = dbBookcase.Deleted,
                User = user,
                Name = dbBookcase.Name,
                Corners = corners,
                Plates = plates,
                Compartments = compartments,
                LockedForEditing = dbBookcase.LockedForEditing,
                PlateDepth = dbBookcase.PlateDepth,
                IsWallSuspended = dbBookcase.IsWallSuspended,
                SkirtingBoardDepth = dbBookcase.SkirtingBoardDepth,
                SkirtingBoardHeight = dbBookcase.SkirtingBoardHeight,
                BaseHeight = dbBookcase.BaseHeight,
                StartingPrice = dbBookcase.StartingPrice,
                PriceIndex = dbBookcase.PriceIndex,
                DoorMaterial = dbBookcase.DoorMaterial
            };

            if (dbBookcase.Plates != null)
            {
                foreach (var plate in dbBookcase.Plates)
                {
                    bookcase.Plates.Add(new Plate(bookcase)
                    {
                        Start = AddGetCorner(plate.StartXCoordinate, plate.StartYCoordinate, ref corners),
                        End = AddGetCorner(plate.EndXCoordinate, plate.EndYCoordinate, ref corners),
                        Id = plate.Id,
                        InnerPlateStart = plate.InnerPlateStart,
                        InnerPlateEnd = plate.InnerPlateEnd,
                        Draggable = plate.Draggable,
                        ParentCompartment = null,
                        SpecialDepth = plate.SpecialDepth
                    });
                }
            }

            if (dbBookcase.Compartments != null)
            {
                foreach (var compartment in dbBookcase.Compartments)
                {
                    compartments.Add(new Compartment(bookcase)
                    {
                        Top = plates.Single(plate => plate.Id == compartment.Top.Id),
                        Bottom = plates.Single(plate => plate.Id == compartment.Bottom.Id),
                        Left = plates.Single(plate => plate.Id == compartment.Left.Id),
                        Right = plates.Single(plate => plate.Id == compartment.Right.Id),
                        Id = compartment.Id,
                        HasDoor = compartment.HasDoor,
                        DoorPosition = compartment.DoorPosition,
                        HasDrawer = compartment.HasDrawer,
                        Model = compartment.Model,
                        ForceSupport = compartment.ForceSupport,
                        BackPlatePosition = compartment.BackPlatePosition
                    });
                }
                foreach (var plate in bookcase.Plates)
                {
                    var dbPlate = dbBookcase.Plates.Single(dbP => dbP.Id == plate.Id);
                    if (dbPlate.ParentCompartment != null)
                    {
                        plate.ParentCompartment = compartments.Single(compartment => compartment.Id == dbPlate.ParentCompartment.Id);
                    }
                }
            }
            AddIntermediateCorners(bookcase);

            return bookcase;
        }

        private Compartment MapToEntity(DbCompartment dbCompartment, Bookcase bookcase)
        {
            if (dbCompartment != null)
            {
                return new Compartment(bookcase)
                {
                    Id = dbCompartment.Id,
                    Top = bookcase.Plates.Single(plate => plate.TemporaryId == dbCompartment.Top.Id),
                    Bottom = bookcase.Plates.Single(plate => plate.TemporaryId == dbCompartment.Bottom.Id),
                    Left = bookcase.Plates.Single(plate => plate.TemporaryId == dbCompartment.Left.Id),
                    Right = bookcase.Plates.Single(plate => plate.TemporaryId == dbCompartment.Right.Id),
                    HasDoor = dbCompartment.HasDoor,
                    DoorPosition = dbCompartment.DoorPosition,
                    HasDrawer = dbCompartment.HasDrawer,
                    Model = dbCompartment.Model,
                    ForceSupport = dbCompartment.ForceSupport,
                    BackPlatePosition = dbCompartment.BackPlatePosition
                };
            }
            return null;
        }

        private void AddIntermediateCorners(Bookcase bookcase)
        {
            foreach (var plate in bookcase.Plates)
            {
                plate.IntermediateCorners = bookcase.GetIntermediateCorners(plate);
            }
        }

        public DbBookcase MapToDb(Bookcase bookcase)
        {
            var plates = new List<DbPlate>();
            var compartments = new List<DbCompartment>();

            if (bookcase.Plates != null)
            {
                foreach (var plate in bookcase.Plates)
                {
                    plates.Add(new DbPlate
                    {
                        StartXCoordinate = plate.Start.x,
                        StartYCoordinate = plate.Start.y,
                        EndXCoordinate = plate.End.x,
                        EndYCoordinate = plate.End.y,
                        Id = plate.Id.Value,
                        InnerPlateStart = plate.InnerPlateStart,
                        InnerPlateEnd = plate.InnerPlateEnd,
                        Draggable = plate.Draggable,
                        ParentCompartment = _autoMapper.Map<DbCompartment>(plate.ParentCompartment),
                        SpecialDepth = plate.SpecialDepth
                    });
                }
            }

            if (bookcase.Compartments != null)
            {
                foreach (var compartment in bookcase.Compartments)
                {
                    compartments.Add(new DbCompartment
                    {
                        Top = plates.Single(plate => plate.Id == compartment.Top.Id),
                        Bottom = plates.Single(plate => plate.Id == compartment.Bottom.Id),
                        Left = plates.Single(plate => plate.Id == compartment.Left.Id),
                        Right = plates.Single(plate => plate.Id == compartment.Right.Id),
                        Id = compartment.Id.Value,
                        HasDoor = compartment.HasDoor,
                        DoorPosition = compartment.DoorPosition,
                        HasDrawer = compartment.HasDrawer,
                        Model = compartment.Model,
                        InternalPlates = plates.Where(dbPlate => dbPlate.ParentCompartment.Id == compartment.Id.Value).ToList()
                    });
                }
            }

            return new DbBookcase
            {
                Id = bookcase.Id.Value,
                Created = bookcase.Created ?? DateTime.MinValue,
                Modified = bookcase.Modified ?? DateTime.MinValue,
                Deleted = bookcase.Deleted,
                Name = bookcase.Name,
                Plates = plates,
                Compartments = compartments,
                Material = _autoMapper.Map<DbMaterial>(bookcase.Material),
                LockedForEditing = bookcase.LockedForEditing,
                PlateDepth = bookcase.PlateDepth,
                IsWallSuspended = bookcase.IsWallSuspended,
                SkirtingBoardDepth = bookcase.SkirtingBoardDepth,
                SkirtingBoardHeight = bookcase.SkirtingBoardHeight,
                BaseHeight = bookcase.BaseHeight,
                StartingPrice = bookcase.StartingPrice,
                PriceIndex = bookcase.PriceIndex,
                DoorMaterial = bookcase.DoorMaterial
            };
        }

        public BookcaseDto MapToDto(Bookcase bookcase)
        {
            if (bookcase == null)
            {
                return null;
            }
            var corners = bookcase.Corners.Select(corner => Map(corner)).ToList();
            var plates = bookcase.Plates.Select(
                plate => new PlateDto
                {
                    Id = plate.Id,
                    Start = corners.Single(corner => corner.Id == plate.Start.Id),
                    End = corners.Single(corner => corner.Id == plate.End.Id),
                    InnerPlateStart = plate.InnerPlateStart,
                    InnerPlateEnd = plate.InnerPlateEnd,
                    Draggable = plate.Draggable,
                    ParentCompartment = _autoMapper.Map<CompartmentDto>(plate.ParentCompartment),
                    SpecialDepth = plate.SpecialDepth
                }
            ).ToList();
            var compartments = bookcase.Compartments.Select(
                compartment => new CompartmentDto
                {
                    Id = compartment.Id,
                    Top = plates.Single(plate => plate.Id == compartment.Top.Id),
                    Bottom = plates.Single(plate => plate.Id == compartment.Bottom.Id),
                    Left = plates.Single(plate => plate.Id == compartment.Left.Id),
                    Right = plates.Single(plate => plate.Id == compartment.Right.Id),
                    HasDoor = compartment.HasDoor,
                    DoorPosition = compartment.DoorPosition,
                    HasDrawer = compartment.HasDrawer,
                    Model = compartment.Model,
                    ForceSupport = compartment.ForceSupport,
                    BackPlatePosition = compartment.BackPlatePosition
                }
            ).ToList();
            return new BookcaseDto
            {
                Id = bookcase.Id,
                Created = bookcase.Created,
                Modified = bookcase.Modified,
                Deleted = bookcase.Deleted,
                Name = bookcase.Name,
                Corners = corners,
                Plates = plates,
                Compartments = compartments,
                Material = _autoMapper.Map<MaterialDto>(bookcase.Material),
                SupportHeight = bookcase.SupportHeight,
                PlateDepth = bookcase.PlateDepth,
                IsWallSuspended = bookcase.IsWallSuspended,
                SkirtingBoardDepth = bookcase.SkirtingBoardDepth,
                SkirtingBoardHeight = bookcase.SkirtingBoardHeight,
                BaseHeight = bookcase.BaseHeight,
                StartingPrice = bookcase.StartingPrice,
                PriceIndex = bookcase.PriceIndex,
                DoorMaterial = bookcase.DoorMaterial,
                MaxLengthWithoutSupport = bookcase.MaxLengthWithoutSupport,
                LockedForEditing = bookcase.LockedForEditing,
                SalesPrice = bookcase.SalesPrice
            };
        }

        public Bookcase MapToEntity(BookcaseDto bookcaseDto)
        {
            var material = _autoMapper.Map<Material>(bookcaseDto.Material);
            if (bookcaseDto.LockedForEditing == null)
            {
                throw new ArgumentException("Cannot map BookcaseDto to Bookcase. LockedForEditing is null.");
            }
            if (bookcaseDto.PlateDepth == null)
            {
                throw new ArgumentException("Cannot map BookcaseDto to Bookcase. PlateDepth is null.");
            }
            var bookcase = new Bookcase(material)
            {
                Id = bookcaseDto.Id,
                Created = bookcaseDto.Created,
                Modified = bookcaseDto.Modified,
                Deleted = bookcaseDto.Deleted,
                Name = bookcaseDto.Name,
                LockedForEditing = bookcaseDto.LockedForEditing.Value,
                PlateDepth = bookcaseDto.PlateDepth.Value,
                IsWallSuspended = bookcaseDto.IsWallSuspended,
                SkirtingBoardDepth = bookcaseDto.SkirtingBoardDepth,
                SkirtingBoardHeight = bookcaseDto.SkirtingBoardHeight,
                BaseHeight = bookcaseDto.BaseHeight,
                StartingPrice = bookcaseDto.StartingPrice,
                PriceIndex = bookcaseDto.PriceIndex,
                DoorMaterial = bookcaseDto.DoorMaterial
            };

            bookcase.Corners = bookcaseDto.Corners != null ? bookcaseDto.Corners.Select(cornerDto => Map(cornerDto)).ToList() : new List<Corner>();
            bookcase.Plates = bookcaseDto.Plates != null ? bookcaseDto.Plates.Select(
                plateDto => new Plate(bookcase)
                {
                    TemporaryId = plateDto.Id,
                    Start = bookcase.Corners.Single(corner => corner.TemporaryId == plateDto.Start.Id),
                    End = bookcase.Corners.Single(corner => corner.TemporaryId == plateDto.End.Id),
                    InnerPlateStart = plateDto.InnerPlateStart,
                    InnerPlateEnd = plateDto.InnerPlateEnd,
                    Draggable = plateDto.Draggable,
                    ParentCompartment = null,
                    SpecialDepth = plateDto.SpecialDepth
                }
            ).ToList() : new List<Plate>();
            bookcase.Compartments = bookcaseDto.Compartments != null ? bookcaseDto.Compartments.Select(
                compartmentDto => new Compartment(bookcase)
                {
                    TemporaryId = compartmentDto.Id,
                    Top = bookcase.Plates.Single(plate => plate.TemporaryId == compartmentDto.Top.Id),
                    Bottom = bookcase.Plates.Single(plate => plate.TemporaryId == compartmentDto.Bottom.Id),
                    Left = bookcase.Plates.Single(plate => plate.TemporaryId == compartmentDto.Left.Id),
                    Right = bookcase.Plates.Single(plate => plate.TemporaryId == compartmentDto.Right.Id),
                    HasDoor = compartmentDto.HasDoor,
                    DoorPosition = compartmentDto.DoorPosition,
                    HasDrawer = compartmentDto.HasDrawer,
                    Model = compartmentDto.Model,
                    ForceSupport = compartmentDto.ForceSupport,
                    BackPlatePosition = compartmentDto.BackPlatePosition
                }
            ).ToList() : new List<Compartment>();
            foreach (var plate in bookcase.Plates)
            {
                var plateDto = bookcaseDto.Plates.First(refPlateDto => refPlateDto.Id == plate.TemporaryId);
                if (plateDto.ParentCompartment != null)
                {
                    plate.ParentCompartment = bookcase.Compartments.First(compartment => compartment.TemporaryId == plateDto.ParentCompartment.Id);
                }
            }
            return bookcase;
        }

        private Corner Map(CornerDto cornerDto)
        {
            return new Corner
            {
                TemporaryId = cornerDto.Id,
                x = cornerDto.x,
                y = cornerDto.y
            };
        }

        private CornerDto Map(Corner corner)
        {
            return new CornerDto
            {
                Id = corner.Id,
                x = corner.x,
                y = corner.y
            };
        }

        private Corner AddGetCorner(int xCoordinate, int yCoordinate, ref List<Corner> corners)
        {
            var existingCorner = corners.SingleOrDefault(corner => corner.x == xCoordinate && corner.y == yCoordinate);

            if (existingCorner != null)
            {
                return existingCorner;
            }
            else
            {
                int nextId;
                if (corners.Any())
                {
                    nextId = corners.Max(corner => corner.Id).Value + 1;
                }
                else
                {
                    nextId = 1;
                }
                var newCorner = new Corner()
                {
                    x = xCoordinate,
                    y = yCoordinate,
                    Id = nextId
                };
                corners.Add(newCorner);
                return newCorner;
            }
        }
    }
}
