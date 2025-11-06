namespace FurnitureBuildingSolution.ViewModels.Bookcase
{
    public class PlateViewModel : BaseViewModel
    {
        public CornerViewModel Start { get; set; }
        public CornerViewModel End { get; set; }
        public bool InnerPlateStart { get; set; }
        public bool InnerPlateEnd { get; set; }
        public bool Draggable { get; set; }
        public double Displacement { get; set; }
    }
}