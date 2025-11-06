using System;
using System.Linq;
using AutoMapper;
using FurnitureBuildingSolution.Database;
using FurnitureBuildingSolution.Entities;

namespace FurnitureBuildingSolution.Helpers
{
    public interface IOrderMapper
    {
        Order MapToEntity(DbOrder order);
        DbOrder MapToDb(Order order);
    }

    public class OrderMapper : IOrderMapper
    {
        private readonly IMapper _autoMapper;
        private readonly IBookcaseMapper _bookcaseMapper;

        public OrderMapper(IMapper mapper, IBookcaseMapper bookcaseMapper)
        {
            _autoMapper = mapper;
            _bookcaseMapper = bookcaseMapper;
        }

        public Order MapToEntity(DbOrder order)
        {
            return new Order
            {
                User = _autoMapper.Map<User>(order.User),
                Created = order.Created,
                Modified = order.Modified,
                Deleted = order.Deleted,
                OrderNumber = order.OrderNumber,
                OrderStatus = order.OrderStatus,
                Address = _autoMapper.Map<Address>(order.Address),
                ShippingAddress = _autoMapper.Map<Address>(order.ShippingAddress),
                FirstName = order.FirstName,
                LastName = order.LastName,
                CompanyName = order.CompanyName,
                VatNumber = order.VatNumber,
                EmailAddress = order.EmailAddress,
                OrderItems = order.OrderItems.Select(orderItem => Map(orderItem)).ToList(),
                OrderTime = order.OrderTime
            };
        }

        public DbOrder MapToDb(Order order)
        {
            return new DbOrder
            {
                OrderNumber = order.OrderNumber,
                OrderStatus = order.OrderStatus,
                Address = _autoMapper.Map<DbAddress>(order.Address),
                ShippingAddress = _autoMapper.Map<DbAddress>(order.ShippingAddress),
                FirstName = order.FirstName,
                LastName = order.LastName,
                CompanyName = order.CompanyName,
                VatNumber = order.VatNumber,
                EmailAddress = order.EmailAddress,
                OrderItems = order.OrderItems.Select(orderItem => Map(orderItem)).ToList(),
                OrderTime = order.OrderTime
            };
        }

        private OrderItem Map(DbOrderItem orderItem)
        {
            return new OrderItem
            {
                Created = orderItem.Created,
                Modified = orderItem.Modified,
                Deleted = orderItem.Deleted,
                Bookcase = _bookcaseMapper.MapToEntity(orderItem.Bookcase),
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice
            };
        }

        private DbOrderItem Map(OrderItem orderItem)
        {
            return new DbOrderItem
            {
                Bookcase = _bookcaseMapper.MapToDb(orderItem.Bookcase),
                Quantity = orderItem.Quantity,
                UnitPrice = orderItem.UnitPrice
            };
        }
    }
}