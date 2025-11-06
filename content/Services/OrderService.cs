using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FurnitureBuildingSolution.Dtos.Order;
using FurnitureBuildingSolution.Entities;
using FurnitureBuildingSolution.Helpers;
using FurnitureBuildingSolution.Repositories;
using RazorLight;

namespace FurnitureBuildingSolution.Services
{
    public interface IOrderService
    {
        Task<OrderDto> PlaceOrderAsync(OrderDto orderDto, int? userId = null);
        List<OrderDto> GetAll(int userId);
        OrderDto GetOrderConfirmation(int orderNumber, int userId);
    }

    public class OrderService : IOrderService
    {
        private IMapper _autoMapper;
        private IOrderRepository _orderRepository;
        private IBookcaseRepository _bookcaseRepository;
        private IEmailService _emailService;
        private readonly IRazorLightEngine _razorEngine;
        public OrderService(IOrderRepository orderRepository, IMapper autoMapper, IBookcaseRepository bookcaseRepository, IEmailService emailService, IRazorLightEngine razorEngine)
        {
            _bookcaseRepository = bookcaseRepository;
            _orderRepository = orderRepository;
            _autoMapper = autoMapper;
            _emailService = emailService;
            _razorEngine = razorEngine;
        }

        public List<OrderDto> GetAll(int userId)
        {
            var orders = _orderRepository.GetAll(userId);
            return orders.Select(order => _autoMapper.Map<OrderDto>(order)).ToList();
        }

        public OrderDto GetOrderConfirmation(int orderNumber, int userId)
        {
            var order = _orderRepository.GetByOrderNumber(orderNumber, userId);
            return _autoMapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> PlaceOrderAsync(OrderDto orderDto, int? userId = null)
        {
            var order = _autoMapper.Map<Order>(orderDto);
            FillUnitPrices(order, userId);
            if (userId != null)
            {
                LockBookcases(orderDto, userId.Value);
            }
            else if (!OrderConsistsOfStandardModels(orderDto))
            {
                throw new AccessViolationException("User must be logged in to order bookcase which is not a standard model.");
            }
            var addedOrder = _orderRepository.Add(order, userId);
            await SendOrderConfirmationEmailAsync(addedOrder);
            SendOrderConfirmationNotificationEmail(addedOrder);
            return _autoMapper.Map<OrderDto>(addedOrder);
        }

        private bool OrderConsistsOfStandardModels(OrderDto orderDto)
        {
            foreach (var bookcase in orderDto.OrderItems.Select(oi => oi.Bookcase))
            {
                if (bookcase.Id != null && !_bookcaseRepository.IsStandardModel(bookcase.Id.Value))
                {
                    return false;
                }
            }
            return true;
        }

        private Email SendOrderConfirmationNotificationEmail(Order addedOrder)
        {
            var email = new Email()
            {
                TextBody = $"Ordre oprettet{Environment.NewLine}{Environment.NewLine}Ordrenummer: {addedOrder.OrderNumber}{Environment.NewLine}Bestilt af: {addedOrder.FirstName} {addedOrder.LastName}",
                Subject = $"Ordre oprettet. Ordrenummer {addedOrder.OrderNumber}",
                Receiver = "info@shelfer.dk"
            };
            return _emailService.EnqueueEmail(email);
        }

        private async Task<Email> SendOrderConfirmationEmailAsync(Order addedOrder)
        {
            string emailAddress;
            if (addedOrder.User != null)
            {
                emailAddress = addedOrder.User.EmailAddress;
            }
            else if (addedOrder.EmailAddress != null)
            {
                emailAddress = addedOrder.EmailAddress;
            }
            else
            {
                throw new AppException("No user found on order");
            }

            var email = new Email()
            {
                HtmlBody = await GetOrderConfirmationEmailHtmlAsync(addedOrder),
                TextBody = $"Denne e-mail indeholder din Shelfer-ordrebekræftelse med ordrenummeret {addedOrder.OrderNumber}. Kan du ikke se ordrebekræftelsen her, kan du logge ind på shelfer.dk og hente den under menupunktet \"Ordrer\"",
                Subject = $"Shelfer ordrebekræftelse: Ordrenummer {addedOrder.OrderNumber}",
                Receiver = emailAddress
            };
            return _emailService.EnqueueEmail(email);
        }

        private async Task<string> GetOrderConfirmationEmailHtmlAsync(Order addedOrder)
        {
            var templatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), $"Views\\Emails\\OrderConfirmation.cshtml");
            string razorTemplate = System.IO.File.ReadAllText(templatePath);
            var orderDto = _autoMapper.Map<OrderDto>(addedOrder);
            return await _razorEngine.CompileRenderAsync("orderConfirmation", razorTemplate, orderDto);
        }

        private void LockBookcases(OrderDto orderDto, int userId)
        {
            var bookcaseIdsToLock = orderDto.OrderItems.Select(orderItem => orderItem.Bookcase.Id.Value).ToList();
            _bookcaseRepository.LockForEditing(bookcaseIdsToLock, userId);
        }

        private void FillUnitPrices(Order order, int? userId = null)
        {
            foreach (var orderItem in order.OrderItems)
            {
                if (_bookcaseRepository.IsStandardModel(orderItem.Bookcase.Id.Value))
                {
                    var bookcase = _bookcaseRepository.Get(orderItem.Bookcase.Id.Value);
                    orderItem.UnitPrice = bookcase.SalesPrice;
                }
                else
                {
                    var bookcase = _bookcaseRepository.Get(orderItem.Bookcase.Id.Value, userId);
                    orderItem.UnitPrice = bookcase.SalesPrice;
                }
            }
        }
    }
}