using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.ViewModels.Bookcase
{
    public class CompartmentViewModel : BaseViewModel
    {
        public PlateViewModel Top { get; set; }
        public PlateViewModel Bottom { get; set; }
        public PlateViewModel Left { get; set; }
        public PlateViewModel Right { get; set; }
        public bool HasDoor { get; set; }
        public DoorPosition DoorPosition { get; set; }
        public bool HasDrawer { get; set; }
        public string Model { get; set; }
    }
}