namespace FurnitureBuildingSolution.Entities
{
    public class StandardModel : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Material { get; set; }
        public string ImageFilename { get; set; }
        public int ProductCategoryId { get; set; }
        public int BookcaseId { get; set; }
    }
}