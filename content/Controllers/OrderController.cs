using System;
using Microsoft.AspNetCore.Mvc;
using FurnitureBuildingSolution.Helpers;
using Microsoft.AspNetCore.Authorization;
using FurnitureBuildingSolution.Services;
using FurnitureBuildingSolution.Dtos.Order;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureBuildingSolution.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        IUserService _userService;
        IOrderService _orderService;
        IOrderMapper _orderMapper;

        public OrderController(IUserService userService, IOrderService orderService, IOrderMapper orderMapper)
        {
            _userService = userService;
            _orderService = orderService;
            _orderMapper = orderMapper;
        }

        [HttpPost("placeOrder")]
        public async Task<IActionResult> PlaceOrderAsync([FromBody] OrderDto orderDto)
        {
            try
            {
                int? userId = null;
                if (User.Identity.Name != null)
                {
                    userId = int.Parse(User.Identity.Name);
                }
                var placedOrder = await _orderService.PlaceOrderAsync(orderDto, userId);
                return Ok(placedOrder);
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
                var user = _userService.GetById(userId);
                var orderList = _orderService.GetAll(userId);
                return Ok(orderList);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("getOrderConfirmation")]
        public IActionResult GetOrderConfirmation(OrderDto order)
        {
            try
            {
                var userId = int.Parse(User.Identity.Name);
                var user = _userService.GetById(userId);
                var orderData = _orderService.GetOrderConfirmation(order.OrderNumber, userId);
                return Ok(orderData);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
