using System.Collections.Generic;
using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Dtos.Bookcase
{
    public class BlueprintRequestDto : BaseDto
    {
        public double? PlateThickness { get; set; }
        public double? DoorPlateThickness { get; set; }
        public bool IncludeSingleSided { get; set; }
        public bool IncludeDoubleSided { get; set; }
    }
}
