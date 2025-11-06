using System.Collections.Generic;
using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Dtos.Bookcase
{
    public class BookcaseDto : BaseDto
    {
        public string Name { get; set; }
        public double? PlateThickness { get; set; }
        public double? DoorPlateThickness { get; set; }
        public List<CompartmentDto> Compartments { get; set; }
        public List<PlateDto> Plates { get; set; }
        public List<CornerDto> Corners { get; set; }
        public MaterialDto Material { get; set; }
        public bool? LockedForEditing { get; set; }
        public int? SupportHeight { get; set; }
        public int? PlateDepth { get; set; }
        public bool IsWallSuspended { get; set; }
        public double? SkirtingBoardDepth { get; set; }
        public double? SkirtingBoardHeight { get; set; }
        public double? BaseHeight { get; set; }
        public double? StartingPrice { get; set; }
        public double? PriceIndex { get; set; }
        public DoorMaterial DoorMaterial { get; set; }
        public double? MaxLengthWithoutSupport { get; set; }
        public int? SalesPrice { get; set; }
    }
}
