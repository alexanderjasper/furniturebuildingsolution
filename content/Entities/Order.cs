using System;
using System.Collections.Generic;
using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Entities
{
    public class Order : BaseEntity
    {
        public User User { get; set; }
        public int OrderNumber { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Address Address { get; set; }
        public Address ShippingAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string VatNumber { get; set; }
        public string EmailAddress { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public DateTime OrderTime { get; set; }
    }
}