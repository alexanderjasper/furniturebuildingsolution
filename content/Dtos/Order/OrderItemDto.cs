using FurnitureBuildingSolution.Dtos.Bookcase;

namespace FurnitureBuildingSolution.Dtos.Order
{
    public class OrderItemDto : BaseDto
    {
        public BookcaseDto Bookcase { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }
}