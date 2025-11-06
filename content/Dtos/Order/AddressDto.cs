using System;

namespace FurnitureBuildingSolution.Dtos.Order
{
    public class AddressDto : BaseDto
    {
        public string AddressString { get; set; }
        public Guid DawaId { get; set; }
    }
}