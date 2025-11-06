using AutoMapper;
using FurnitureBuildingSolution.Database;
using FurnitureBuildingSolution.DataRepresentation;
using FurnitureBuildingSolution.Dtos;
using FurnitureBuildingSolution.Dtos.Bookcase;
using FurnitureBuildingSolution.Dtos.Order;
using FurnitureBuildingSolution.Entities;
using FurnitureBuildingSolution.ViewModels.Bookcase;

namespace FurnitureBuildingSolution.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //DbEntity -> Entity
            CreateMap<DbAddress, Address>();
            CreateMap<DbUser, User>();
            CreateMap<DbEmail, Email>();
            CreateMap<DbMaterial, Material>();
            CreateMap<DbProductCategory, ProductCategory>();
            CreateMap<DbStandardModel, StandardModel>();

            //Entity -> DbEntity
            CreateMap<Address, DbAddress>();
            CreateMap<User, DbUser>();
            CreateMap<Email, DbEmail>();
            CreateMap<Material, DbMaterial>();
            CreateMap<ProductCategory, DbProductCategory>();
            CreateMap<StandardModel, DbStandardModel>();

            //Entity -> Dto
            CreateMap<Address, AddressDto>();
            CreateMap<Bookcase, BookcaseDto>();
            CreateMap<Compartment, CompartmentDto>();
            CreateMap<Corner, CornerDto>();
            CreateMap<Material, MaterialDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<Plate, PlateDto>();
            CreateMap<User, UserDto>();
            CreateMap<ProductCategory, ProductCategoryDto>();
            CreateMap<StandardModel, StandardModelDto>();

            //Dto -> Entity
            CreateMap<AddressDto, Address>();
            CreateMap<BookcaseDto, Bookcase>();
            CreateMap<CompartmentDto, Compartment>();
            CreateMap<CornerDto, Corner>();
            CreateMap<MaterialDto, Material>();
            CreateMap<OrderDto, Order>();
            CreateMap<OrderItemDto, OrderItem>();
            CreateMap<PlateDto, Plate>();
            CreateMap<UserDto, User>();
            CreateMap<StandardModelDto, StandardModel>();
            CreateMap<ProductCategoryDto, ProductCategory>();

            //Dto -> ViewModel
            CreateMap<BookcaseDto, BookcaseViewModel>();
            CreateMap<CompartmentDto, CompartmentViewModel>();
            CreateMap<CornerDto, CornerViewModel>();
            CreateMap<PlateDto, PlateViewModel>();

            //ViewModel -> Dto
            CreateMap<BookcaseViewModel, BookcaseDto>();
            CreateMap<CompartmentViewModel, CompartmentDto>();
            CreateMap<CornerViewModel, CornerDto>();
            CreateMap<PlateViewModel, PlateDto>();

            //Entity -> DataViewModel
            CreateMap<User, UserListData>();
        }
    }
}