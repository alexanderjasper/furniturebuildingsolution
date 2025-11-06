using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Dtos.Bookcase
{
    public class MaterialDto : BaseDto
    {
        public string Name { get; set; }
        public MaterialType Type { get; set; }
        public int? Thickness { get; set; }
        public string SideTextureLocation { get; set; }
        public string SurfaceTextureLocation { get; set; }
        public decimal? PricePerSquareMeter { get; set; }
        public int ListPosition { get; set; }
    }
}