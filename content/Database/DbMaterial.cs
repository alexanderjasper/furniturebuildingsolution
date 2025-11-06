using System.ComponentModel.DataAnnotations;
using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Database
{
    public class DbMaterial : BaseDbEntity
    {
        [Required]
        public string Name { get; set; }
        public MaterialType Type { get; set; }
        public double Thickness { get; set; }
        [Required]
        public string SideTextureLocation { get; set; }
        [Required]
        public string SurfaceTextureLocation { get; set; }
        public decimal PricePerSquareMeter { get; set; }
        public int ListPosition { get; set; }
    }
}