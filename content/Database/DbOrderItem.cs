using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureBuildingSolution.Database
{
    [Table("OrderItems")]
    public class DbOrderItem : BaseDbEntity
    {
        public DbBookcase Bookcase { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }
}