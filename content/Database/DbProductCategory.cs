using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureBuildingSolution.Database
{
    [Table("ProductCategories")]
    public class DbProductCategory : BaseDbEntity
    {
        public string Name { get; set; }
    }
}