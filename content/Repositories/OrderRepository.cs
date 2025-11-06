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
    public interface IOrderRepository
    {
        Order Add(Order order, int? userId = null);
        List<Order> GetAll(int userId);
        Order GetByOrderNumber(int orderNumber, int userId);
    }

    public class OrderRepository : IOrderRepository
    {
        private DataContext _context;
        private IOrderMapper _orderMapper;
        private IMapper _autoMapper;
        public OrderRepository(DataContext context, IOrderMapper orderMapper, IMapper autoMapper)
        {
            _context = context;
            _orderMapper = orderMapper;
            _autoMapper = autoMapper;
        }

        public Order Add(Order order, int? userId = null)
        {
            var dbOrder = _orderMapper.MapToDb(order);
            dbOrder.OrderTime = DateTime.Now;
            dbOrder.OrderNumber = GetNewOrderNumber();
            dbOrder.OrderStatus = Enums.OrderStatus.New;
            if (userId != null)
            {
                dbOrder.User = _context.Users.Single(dbUser => dbUser.Id == userId.Value);
            }
            var savedOrder = _context.Orders.Add(dbOrder);
            _context.SaveChanges();
            var returnOrder = _orderMapper.MapToEntity(savedOrder.Entity);
            return returnOrder;
        }

        public List<Order> GetAll(int userId)
        {
            return _context.Orders
            .Include(order => order.Address)
            .Include(order => order.OrderItems)
            .Include(order => order.ShippingAddress)
            .Include(order => order.User)
            .Where(order => order.User != null && order.User.Id == userId)
            .Select(order => _orderMapper.MapToEntity(order)).ToList();
        }

        public Order GetByOrderNumber(int orderNumber, int userId)
        {
            var dbOrder = _context.Orders
            .Include(order => order.Address)
            .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.Bookcase)
                .ThenInclude(bookcase => bookcase.Plates)
            .Include(order => order.OrderItems)
                .ThenInclude(orderItem => orderItem.Bookcase)
                .ThenInclude(bookcase => bookcase.Material)
            .Include(order => order.ShippingAddress)
            .Include(order => order.User)
            .Single(order => order.User != null && order.User.Id == userId && order.OrderNumber == orderNumber);
            return _orderMapper.MapToEntity(dbOrder);
        }

        private int GetNewOrderNumber()
        {
            var latestOrder = _context.Orders.OrderByDescending(order => order.OrderNumber).FirstOrDefault();
            if (latestOrder == null)
            {
                return 1;
            }
            else
            {
                return latestOrder.OrderNumber + 1;
            }
        }
    }
}