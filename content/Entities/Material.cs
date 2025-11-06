using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Entities
{
    public class Material : BaseEntity
    {
        public string Name { get; set; }
        public MaterialType Type { get; set; }
        public double Thickness { get; set; }
        public string SideTextureLocation { get; set; }
        public string SurfaceTextureLocation { get; set; }
        public decimal PricePerSquareMeter { get; set; }
        public int ListPosition { get; set; }
    }
}