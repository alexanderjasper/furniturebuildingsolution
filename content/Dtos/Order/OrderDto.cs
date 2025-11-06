using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Dtos.Order
{
    [Table("Orders")]
    public class OrderDto : BaseDto
    {
        public int OrderNumber { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public AddressDto Address { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string VatNumber { get; set; }
        public string EmailAddress { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public DateTime OrderTime { get; set; }
    }
}