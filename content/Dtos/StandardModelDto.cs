using FurnitureBuildingSolution.Dtos.Bookcase;

namespace FurnitureBuildingSolution.Dtos
{
    public class StandardModelDto : BaseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Material { get; set; }
        public decimal Price { get; set; }
        public string ImageFilename { get; set; }
        public int ProductCategoryId { get; set; }
        public int BookcaseId { get; set; }
    }
}