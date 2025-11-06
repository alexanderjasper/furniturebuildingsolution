namespace FurnitureBuildingSolution.Entities
{
    public class OrderItem : BaseEntity
    {
        public Bookcase Bookcase { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }
}