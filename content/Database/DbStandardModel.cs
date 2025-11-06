using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureBuildingSolution.Database
{
    [Table("StandardModels")]
    public class DbStandardModel : BaseDbEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Material { get; set; }
        public string ImageFilename { get; set; }
        [ForeignKey("ProductCategory")]
        public int ProductCategoryId { get; set; }
        public DbProductCategory ProductCategory { get; set; }
        [ForeignKey("Bookcase")]
        public int BookcaseId { get; set; }
        public DbBookcase Bookcase { get; set; }
    }
}