using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FurnitureBuildingSolution.Database;
using FurnitureBuildingSolution.Entities;
using FurnitureBuildingSolution.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FurnitureBuildingSolution.Repositories
{
    public interface IBookcaseRepository
    {
        Bookcase Save(Bookcase bookcase, int userId);
        Bookcase Update(Bookcase bookcase, int userId);
        List<Bookcase> GetAll(int userId);
        IEnumerable<DbBookcase> GetAll();
        Bookcase Get(int bookcaseId, int? userId = null);
        List<Bookcase> Delete(int bookcaseId, int userId);
        Bookcase GetForSpecifications(int bookcaseId, double? plateThickness = null, double? doorPlateThickness = null);
        void LockForEditing(List<int> bookcaseIdsToLock, int userId);
        List<Material> GetMaterials();
        bool IsStandardModel(int bookcaseId);
    }

    public class BookcaseRepository : IBookcaseRepository
    {
        private DataContext _context;
        private IBookcaseMapper _bookcaseMapper;
        private readonly IMapper _autoMapper;

        public BookcaseRepository(DataContext context, IBookcaseMapper bookcaseMapper, IMapper mapper)
        {
            _context = context;
            _bookcaseMapper = bookcaseMapper;
            _autoMapper = mapper;
        }

        public Bookcase Save(Bookcase bookcase, int userId)
        {
            var plateIdByTemporaryId = new Dictionary<int, int>();
            var compartmentIdByTemporaryId = new Dictionary<int, int>();
            var plates = new List<DbPlate>();
            var compartments = new List<DbCompartment>();
            var material = _context.Materials.Single(mat => mat.Id == bookcase.Material.Id);

            foreach (var plate in bookcase.Plates)
            {
                AddPlate(plate, ref plateIdByTemporaryId, ref plates);
            }
            foreach (var compartment in bookcase.Compartments)
            {
                AddCompartment(compartment, plateIdByTemporaryId, ref plates, ref compartments, ref compartmentIdByTemporaryId);
            }
            foreach (var plate in bookcase.Plates)
            {
                if (plate.ParentCompartment != null)
                {
                    var dbPlate = plates.Single(p => p.Id == plateIdByTemporaryId[plate.TemporaryId.Value]);
                    dbPlate.ParentCompartment = compartments.Single(c => c.Id == compartmentIdByTemporaryId[plate.ParentCompartment.TemporaryId.Value]);
                }
            }
            var savedBookcase = SaveBookcaseToDatabase(compartments, plates, bookcase.Name, userId, material, bookcase);

            return _bookcaseMapper.MapToEntity(savedBookcase);
        }

        public Bookcase Update(Bookcase bookcase, int userId)
        {
            var user = _context.Users.Single(dbUser => dbUser.Id == userId);
            var existingBookcase = _context.Bookcases
            .Include(dbBookcase => dbBookcase.Compartments)
            .Include(dbBookcase => dbBookcase.Plates)
            .Include(dbBookcase => dbBookcase.Material)
            .Include(dbBookcase => dbBookcase.User)
            .Single(dbBookcase => dbBookcase.Id == bookcase.Id);

            if (user.Role != Enums.Role.Admin && existingBookcase.User.Id != userId)
            {
                throw new Exception("User not authorized to edit this bookcase.");
            }

            if (existingBookcase.LockedForEditing)
            {
                throw new AppException("Attempted to edit bookcase which was locked for editing.");
            }

            var plateIdByTemporaryId = new Dictionary<int, int>();
            var compartmentIdByTemporaryId = new Dictionary<int, int>();
            var plates = new List<DbPlate>();
            var compartments = new List<DbCompartment>();
            var material = _context.Materials.Single(mat => mat.Id == bookcase.Material.Id);

            foreach (var plate in bookcase.Plates)
            {
                AddOrUpdatePlate(plate, ref plates, existingBookcase, ref plateIdByTemporaryId);
            }
            foreach (var compartment in bookcase.Compartments)
            {
                AddOrUpdateCompartment(compartment, ref plates, ref compartments, existingBookcase, plateIdByTemporaryId, ref compartmentIdByTemporaryId);
            }
            foreach (var plate in bookcase.Plates)
            {
                if (plate.ParentCompartment != null)
                {
                    var dbPlate = plates.Single(p => p.Id == plateIdByTemporaryId[plate.TemporaryId.Value]);
                    dbPlate.ParentCompartment = compartments.Single(c => c.Id == compartmentIdByTemporaryId[plate.ParentCompartment.TemporaryId.Value]);
                }
            }
            var updatedBookcase = UpdateBookcaseInDatabase(existingBookcase, compartments, plates, bookcase.Name, material, bookcase);

            return _bookcaseMapper.MapToEntity(updatedBookcase);
        }

        public List<Bookcase> GetAll(int userId)
        {
            return _context.Bookcases.Where(dbBookcase => dbBookcase.User.Id == userId)
                .Include(dbBookcase => dbBookcase.Plates)
                .Include(dbBookcase => dbBookcase.Compartments)
                .Include(dbBookcase => dbBookcase.Material)
                .Include(dbBookcase => dbBookcase.User)
                .OrderByDescending(dbBookcase => dbBookcase.Modified)
                .Select(dbBookcase => _bookcaseMapper.MapToEntity(dbBookcase))
                .ToList();
        }

        public IEnumerable<DbBookcase> GetAll()
        {
            return _context.Bookcases
                 .Include(dbBookcase => dbBookcase.User);
        }

        public Bookcase Get(int bookcaseId, int? userId = null)
        {
            if (userId != null)
            {
                var user = _context.Users.Single(dbUser => dbUser.Id == userId);
                if (user.Role == Enums.Role.Admin)
                {
                    return Get(bookcaseId);
                }
                else
                {
                    return _bookcaseMapper.MapToEntity(
                        _context.Bookcases
                        .Include(dbBookcase => dbBookcase.Compartments)
                        .Include(dbBookcase => dbBookcase.Plates)
                        .Include(dbBookcase => dbBookcase.Material)
                        .Include(dbBookcase => dbBookcase.User)
                        .Single(dbBookcase => dbBookcase.Id == bookcaseId && dbBookcase.User.Id == userId)
                    );
                }
            }
            else
            {
                return _bookcaseMapper.MapToEntity(
                    _context.Bookcases
                    .Include(dbBookcase => dbBookcase.Compartments)
                    .Include(dbBookcase => dbBookcase.Plates)
                    .Include(dbBookcase => dbBookcase.Material)
                    .Include(dbBookcase => dbBookcase.User)
                    .Single(dbBookcase => dbBookcase.Id == bookcaseId)
                );
            }
        }

        public Bookcase GetForSpecifications(int bookcaseId, double? plateThickness = null, double? doorPlateThickness = null)
        {
            var bookcase = _bookcaseMapper.MapToEntity(
                _context.Bookcases
                .Include(dbBookcase => dbBookcase.Compartments)
                .Include(dbBookcase => dbBookcase.Plates)
                .Include(dbBookcase => dbBookcase.Material)
                .Include(dbBookcase => dbBookcase.User)
                .Single(dbBookcase => dbBookcase.Id == bookcaseId)
            );
            if (plateThickness != null)
            {
                bookcase.PlateThickness = plateThickness.Value;
            }
            if (doorPlateThickness != null)
            {
                bookcase.DoorPlateThickness = doorPlateThickness.Value;
            }
            return bookcase;
        }

        public List<Bookcase> Delete(int bookcaseId, int userId)
        {
            var bookcase = _context.Bookcases.Single(dbBookcase => dbBookcase.Id == bookcaseId && dbBookcase.User.Id == userId);
            _context.Remove(bookcase);
            _context.SaveChanges();
            return GetAll(userId);
        }

        public bool IsStandardModel(int bookcaseId)
        {
            return _context.StandardModels.Any(sm => sm.BookcaseId == bookcaseId);
        }

        public void LockForEditing(List<int> bookcaseIdsToLock, int userId)
        {
            var bookcases = _context.Bookcases.Where(bookcase => bookcaseIdsToLock.Any(bookcaseId => bookcaseId == bookcase.Id)).Include(bookcase => bookcase.User);
            bookcases.ToList().ForEach(bookcase =>
            {
                if (!IsStandardModel(bookcase.Id))
                {
                    if (bookcase.User.Id != userId)
                    {
                        throw new AccessViolationException("Attempted to lock other user's bookcase(s) for editing");

                    }
                    bookcase.LockedForEditing = true;
                }
            });
            _context.SaveChanges();
        }

        private DbBookcase UpdateBookcaseInDatabase(DbBookcase existingBookcase, List<DbCompartment> compartments, List<DbPlate> plates, String bookcaseName, DbMaterial material, Bookcase newBookcase)
        {
            var platesToRemove = existingBookcase.Plates
                .Where(dbPlate => !plates.Select(plate => plate.Id).Contains(dbPlate.Id));
            var compartmentsToRemove = existingBookcase.Compartments
                .Where(dbCompartment => !compartments.Select(compartment => compartment.Id).Contains(dbCompartment.Id));

            _context.RemoveRange(platesToRemove);
            _context.RemoveRange(compartmentsToRemove);

            existingBookcase.Compartments = compartments;
            existingBookcase.Plates = plates;
            existingBookcase.Name = bookcaseName;
            existingBookcase.Material = material;
            existingBookcase.PlateDepth = newBookcase.PlateDepth;
            existingBookcase.IsWallSuspended = newBookcase.IsWallSuspended;
            existingBookcase.SkirtingBoardDepth = newBookcase.SkirtingBoardDepth;
            existingBookcase.SkirtingBoardHeight = newBookcase.SkirtingBoardHeight;
            existingBookcase.BaseHeight = newBookcase.BaseHeight;
            existingBookcase.StartingPrice = newBookcase.StartingPrice;
            existingBookcase.PriceIndex = newBookcase.PriceIndex;
            existingBookcase.DoorMaterial = newBookcase.DoorMaterial;

            var updatedBookcase = _context.Update(existingBookcase).Entity;

            _context.SaveChanges();

            return updatedBookcase;
        }

        private void AddOrUpdateCompartment(Compartment compartment, ref List<DbPlate> plates, ref List<DbCompartment> compartments, DbBookcase bookcase, Dictionary<int, int> plateIdByTemporaryId, ref Dictionary<int, int> compartmentIdByTemporaryId)
        {
            if (compartment.TemporaryId == null)
                throw new AppException("Received compartment had no TemporaryId. Cannot save to database.");

            var existingCompartment = _context.Compartments.SingleOrDefault(
                dbCompartment => dbCompartment.Id == compartment.TemporaryId && bookcase.Compartments.Contains(dbCompartment)
            );
            if (existingCompartment != null)
            {
                existingCompartment.Top = plates.Single(plate => plate.Id == plateIdByTemporaryId[compartment.Top.TemporaryId.Value]);
                existingCompartment.Bottom = plates.Single(plate => plate.Id == plateIdByTemporaryId[compartment.Bottom.TemporaryId.Value]);
                existingCompartment.Left = plates.Single(plate => plate.Id == plateIdByTemporaryId[compartment.Left.TemporaryId.Value]);
                existingCompartment.Right = plates.Single(plate => plate.Id == plateIdByTemporaryId[compartment.Right.TemporaryId.Value]);
                existingCompartment.HasDoor = compartment.HasDoor;
                existingCompartment.DoorPosition = compartment.DoorPosition;
                existingCompartment.HasDrawer = compartment.HasDrawer;
                existingCompartment.Model = compartment.Model;
                var updatedCompartment = _context.Update(existingCompartment).Entity;
                compartmentIdByTemporaryId.Add(updatedCompartment.Id, updatedCompartment.Id);
                compartments.Add(updatedCompartment);
            }
            else
            {
                var newCompartment = new DbCompartment
                {
                    Top = plates.Single(plate => plate.Id == plateIdByTemporaryId[compartment.Top.TemporaryId.Value]),
                    Bottom = plates.Single(plate => plate.Id == plateIdByTemporaryId[compartment.Bottom.TemporaryId.Value]),
                    Left = plates.Single(plate => plate.Id == plateIdByTemporaryId[compartment.Left.TemporaryId.Value]),
                    Right = plates.Single(plate => plate.Id == plateIdByTemporaryId[compartment.Right.TemporaryId.Value]),
                    HasDoor = compartment.HasDoor,
                    DoorPosition = compartment.DoorPosition,
                    HasDrawer = compartment.HasDrawer,
                    Model = compartment.Model
                };
                var addedCompartment = _context.Add(newCompartment).Entity;
                compartmentIdByTemporaryId.Add(compartment.TemporaryId.Value, addedCompartment.Id);
                compartments.Add(addedCompartment);
            }
        }

        private void AddOrUpdatePlate(Plate plate, ref List<DbPlate> plates, DbBookcase bookcase, ref Dictionary<int, int> plateIdByTemporaryId)
        {
            if (plate.TemporaryId == null)
                throw new AppException("Received plate had no TemporaryId. Cannot save to database.");

            var existingPlate = _context.Plates.SingleOrDefault(
                dbPlate => dbPlate.Id == plate.TemporaryId && bookcase.Plates.Contains(dbPlate)
            );
            if (existingPlate != null)
            {
                existingPlate.StartXCoordinate = plate.Start.x;
                existingPlate.StartYCoordinate = plate.Start.y;
                existingPlate.EndXCoordinate = plate.End.x;
                existingPlate.EndYCoordinate = plate.End.y;
                existingPlate.InnerPlateStart = plate.InnerPlateStart;
                existingPlate.InnerPlateEnd = plate.InnerPlateEnd;
                existingPlate.Draggable = plate.Draggable;
                existingPlate.ParentCompartment = _context.Compartments.SingleOrDefault(compartment => plate.ParentCompartment != null && compartment.Id == plate.ParentCompartment.Id);
                var updatedPlate = _context.Update(existingPlate).Entity;
                plates.Add(updatedPlate);
                plateIdByTemporaryId.Add(updatedPlate.Id, updatedPlate.Id);
            }
            else
            {
                AddPlateToContext(plate, ref plates, ref plateIdByTemporaryId);
            }
        }

        private DbBookcase SaveBookcaseToDatabase(List<DbCompartment> compartments, List<DbPlate> plates, string name, int userId, DbMaterial material, Bookcase newBookcase)
        {
            var bookcaseToAdd = new DbBookcase
            {
                Compartments = compartments,
                Plates = plates,
                Name = name,
                User = _context.Users.Single(dbUser => dbUser.Id == userId),
                Material = material,
                PlateDepth = newBookcase.PlateDepth,
                IsWallSuspended = newBookcase.IsWallSuspended,
                SkirtingBoardDepth = newBookcase.SkirtingBoardDepth,
                SkirtingBoardHeight = newBookcase.SkirtingBoardHeight,
                BaseHeight = newBookcase.BaseHeight,
                StartingPrice = newBookcase.StartingPrice,
                PriceIndex = newBookcase.PriceIndex,
                DoorMaterial = newBookcase.DoorMaterial
            };

            var savedBookcase = _context.Add(bookcaseToAdd).Entity;

            _context.SaveChanges();

            return savedBookcase;
        }

        private void AddCompartment(Compartment compartment, Dictionary<int, int> plateIdByTemporaryId, ref List<DbPlate> plates, ref List<DbCompartment> compartments, ref Dictionary<int, int> compartmentIdByTemporaryId)
        {
            if (compartment.Top.TemporaryId == null || compartment.Bottom.TemporaryId == null
                    || compartment.Left.TemporaryId == null || compartment.Right.TemporaryId == null)
                throw new AppException("Received plate in compartment had no TemporaryId. Cannot save to database.");

            AddCompartmentToContext(compartment, plates, compartments, plateIdByTemporaryId, ref compartmentIdByTemporaryId);
        }

        private void AddCompartmentToContext(Compartment compartment, List<DbPlate> plates, List<DbCompartment> compartments, Dictionary<int, int> plateIdByTemporaryId, ref Dictionary<int, int> compartmentIdByTemporaryId)
        {
            var topPlate = plates.Single(plate => plate.Id == plateIdByTemporaryId[compartment.Top.TemporaryId.Value]);
            var bottomPlate = plates.Single(plate => plate.Id == plateIdByTemporaryId[compartment.Bottom.TemporaryId.Value]);
            var leftPlate = plates.Single(plate => plate.Id == plateIdByTemporaryId[compartment.Left.TemporaryId.Value]);
            var rightPlate = plates.Single(plate => plate.Id == plateIdByTemporaryId[compartment.Right.TemporaryId.Value]);

            var compartmentToAdd = new DbCompartment
            {
                Top = topPlate,
                Bottom = bottomPlate,
                Left = leftPlate,
                Right = rightPlate,
                HasDoor = compartment.HasDoor,
                DoorPosition = compartment.DoorPosition,
                HasDrawer = compartment.HasDrawer,
                Model = compartment.Model
            };

            var addedCompartment = _context.Add(compartmentToAdd).Entity;
            compartmentIdByTemporaryId.Add(compartment.TemporaryId.Value, addedCompartment.Id);

            compartments.Add(addedCompartment);
        }

        private void AddPlate(Plate plate, ref Dictionary<int, int> plateIdByTemporaryId, ref List<DbPlate> plates)
        {
            if (plate.TemporaryId == null)
                throw new AppException("Received plate had no TemporaryId. Cannot save to database.");

            AddPlateToContext(plate, ref plates, ref plateIdByTemporaryId);
        }

        private void AddPlateToContext(Plate plate, ref List<DbPlate> plates, ref Dictionary<int, int> plateIdByTemporaryId)
        {
            var dbPlate = new DbPlate
            {
                StartXCoordinate = plate.Start.x,
                StartYCoordinate = plate.Start.y,
                EndXCoordinate = plate.End.x,
                EndYCoordinate = plate.End.y,
                InnerPlateStart = plate.InnerPlateStart,
                InnerPlateEnd = plate.InnerPlateEnd,
                Draggable = plate.Draggable
            };
            var addedPlate = _context.Add(dbPlate).Entity;
            plateIdByTemporaryId.Add(plate.TemporaryId.Value, addedPlate.Id);
            plates.Add(addedPlate);
        }

        public List<Material> GetMaterials()
        {
            var dbMaterials = _context.Materials.ToList();
            var materials = dbMaterials.Select(dbMaterial => _autoMapper.Map<Material>(dbMaterial)).ToList();
            return materials;
        }
    }
}
