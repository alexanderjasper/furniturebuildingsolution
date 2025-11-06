using System;

namespace FurnitureBuildingSolution.Entities
{
    public class Address : BaseEntity
    {
        public string AddressString { get; set; }
        public Guid DawaId { get; set; }
    }
}