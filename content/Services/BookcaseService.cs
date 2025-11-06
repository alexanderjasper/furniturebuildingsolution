using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FurnitureBuildingSolution.Blueprint;
using FurnitureBuildingSolution.DataRepresentation;
using FurnitureBuildingSolution.Dtos.Bookcase;
using FurnitureBuildingSolution.Entities;
using FurnitureBuildingSolution.Helpers;
using FurnitureBuildingSolution.Repositories;

namespace FurnitureBuildingSolution.Services
{
    public interface IBookcaseService
    {
        BookcaseDto Save(BookcaseDto bookcase, int userId);
        List<BookcaseDto> GetAll(int userId);
        IEnumerable<BookcaseListData> GetAll();
        BookcaseDto Get(int bookcaseId, int userId);
        BookcaseDto GetPublic(int bookcaseId);
        BookcaseDto Get(int bookcaseId);
        List<BookcaseDto> DeleteBookcase(int bookcaseId, int userId);
        BlueprintBookcase GetBlueprint(BlueprintRequestDto blueprintRequest);
        BlueprintBookcase GetDoorsBlueprint(int bookcaseId, double? plateThickness, double? doorPlateThickness);
        List<MaterialDto> GetMaterials();
    }

    public class BookcaseService : IBookcaseService
    {
        private IBookcaseRepository _bookcaseRepository;
        private readonly IMapper _autoMapper;
        private readonly IBookcaseMapper _bookcaseMapper;
        private readonly IEmailService _emailService;

        public BookcaseService(IBookcaseRepository bookcaseRepository, IBookcaseMapper bookcaseMapper, IMapper mapper, IEmailService emailService)
        {
            _bookcaseRepository = bookcaseRepository;
            _bookcaseMapper = bookcaseMapper;
            _autoMapper = mapper;
            _emailService = emailService;
        }

        public BookcaseDto Save(BookcaseDto bookcaseDto, int userId)
        {
            var bookcase = _bookcaseMapper.MapToEntity(bookcaseDto);
            if (bookcase.Id != null)
            {
                var returnBookcase = _bookcaseRepository.Update(bookcase, userId);
                return _bookcaseMapper.MapToDto(returnBookcase);
            }
            else
            {
                var returnBookcase = _bookcaseRepository.Save(bookcase, userId);
                SendBookcaseAddedNotificationEmail(userId);
                return _bookcaseMapper.MapToDto(returnBookcase);
            }
        }

        private Email SendBookcaseAddedNotificationEmail(int userId)
        {
            var email = new Email()
            {
                TextBody = $"Reol oprettet{Environment.NewLine}{Environment.NewLine}Bruger-id: {userId}",
                Subject = $"Reol oprettet{Environment.NewLine}{Environment.NewLine}Bruger-id: {userId}",
                Receiver = "info@shelfer.dk"
            };
            return _emailService.EnqueueEmail(email);
        }

        public List<BookcaseDto> GetAll(int userId)
        {
            var bookcases = _bookcaseRepository.GetAll(userId);
            return bookcases.Select(bookcase => _bookcaseMapper.MapToDto(bookcase)).ToList();
        }

        public IEnumerable<BookcaseListData> GetAll()
        {
            var dbBookcases = _bookcaseRepository.GetAll();
            return dbBookcases.Select(bookcase => new BookcaseListData()
            {
                Id = bookcase.Id,
                Created = bookcase.Created,
                Modified = bookcase.Modified,
                Deleted = bookcase.Deleted,
                UserId = bookcase.User.Id,
                UserFirstName = bookcase.User.FirstName,
                UserLastName = bookcase.User.LastName
            });
        }

        public BookcaseDto Get(int bookcaseId, int userId)
        {
            var bookcase = _bookcaseRepository.Get(bookcaseId, userId);
            return _bookcaseMapper.MapToDto(bookcase);
        }

        public BookcaseDto GetPublic(int bookcaseId)
        {
            if (_bookcaseRepository.IsStandardModel(bookcaseId))
            {
                return _autoMapper.Map<BookcaseDto>(_bookcaseRepository.Get(bookcaseId));
            }
            else
            {
                throw new Exception("No standard model exists with bookcase id " + bookcaseId + ".");
            }
        }

        public BookcaseDto Get(int bookcaseId)
        {
            var bookcase = _bookcaseRepository.Get(bookcaseId);
            return _bookcaseMapper.MapToDto(bookcase);
        }

        public List<BookcaseDto> DeleteBookcase(int bookcaseId, int userId)
        {
            var bookcases = _bookcaseRepository.Delete(bookcaseId, userId);
            return bookcases.Select(bookcase => _bookcaseMapper.MapToDto(bookcase)).ToList();
        }

        public BlueprintBookcase GetBlueprint(BlueprintRequestDto blueprintRequest)
        {
            var bookcase = _bookcaseRepository.GetForSpecifications(blueprintRequest.Id.Value, blueprintRequest.PlateThickness, blueprintRequest.DoorPlateThickness);
            var blueprint = new BlueprintBookcase(bookcase, true, false);
            if (!blueprintRequest.IncludeSingleSided)
            {
                blueprint.Plates = blueprint.Plates.Where(p => p.Mounts.Any(m => m.Type == "bottom")).ToList();
            }
            if (!blueprintRequest.IncludeDoubleSided)
            {
                blueprint.Plates = blueprint.Plates.Where(p => p.Mounts.All(m => m.Type != "bottom")).ToList();
            }
            return blueprint;
        }

        public BlueprintBookcase GetDoorsBlueprint(int bookcaseId, double? plateThickness, double? doorPlateThickness)
        {
            var bookcase = _bookcaseRepository.GetForSpecifications(bookcaseId, plateThickness, doorPlateThickness);
            return new BlueprintBookcase(bookcase, false, true);
        }

        private bool CornerIsOnPlate(Corner corner, Plate plate)
        {
            if (plate.IsHorizontal && corner.y == plate.Start.y && corner.x >= plate.Start.x && corner.x <= plate.End.x)
            {
                return true;
            }
            else if (!plate.IsHorizontal && corner.x == plate.Start.x && corner.y >= plate.Start.y && corner.y <= plate.End.y)
            {
                return true;
            }
            return false;
        }

        private int GetPositionOnPlate(Plate plate, Corner corner)
        {
            int position;
            if (plate.IsHorizontal)
            {
                position = corner.x - plate.Start.x;
            }
            else
            {
                position = corner.y - plate.Start.y;
            }
            //TODO: use variable for plate thickness.
            // if (plate.InnerPlateStart)
            // {
            //     position -= 9;
            // }
            // else
            // {
            //     position += 9;
            // }
            return position;
        }

        public List<MaterialDto> GetMaterials()
        {
            var materials = _bookcaseRepository.GetMaterials().OrderBy(material => material.ListPosition);
            return materials.Select(material => _autoMapper.Map<MaterialDto>(material)).ToList();
        }
    }
}
