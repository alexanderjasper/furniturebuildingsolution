using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Database
{
    [Table("Orders")]
    public class DbOrder : BaseDbEntity
    {
        public DbUser User { get; set; }
        public int OrderNumber { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DbAddress Address { get; set; }
        public DbAddress ShippingAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string VatNumber { get; set; }
        public string EmailAddress { get; set; }
        public List<DbOrderItem> OrderItems { get; set; }
        public DateTime OrderTime { get; set; }
    }
}