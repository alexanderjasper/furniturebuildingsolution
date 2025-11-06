using System.Collections.Generic;
using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Dtos.Bookcase
{
    public class CompartmentDto : BaseDto
    {
        public PlateDto Top { get; set; }
        public PlateDto Bottom { get; set; }
        public PlateDto Left { get; set; }
        public PlateDto Right { get; set; }
        public bool HasDoor { get; set; }
        public DoorPosition DoorPosition { get; set; }
        public bool HasDrawer { get; set; }
        public string Model { get; set; }
        public ForceSupport ForceSupport { get; set; }
        public double? BackPlatePosition { get; set; }
    }
}