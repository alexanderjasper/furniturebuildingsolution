namespace FurnitureBuildingSolution.Dtos.Bookcase
{
    public class PlateDto : BaseDto
    {
        public CornerDto Start { get; set; }
        public CornerDto End { get; set; }
        public bool InnerPlateStart { get; set; }
        public bool InnerPlateEnd { get; set; }
        public bool Draggable { get; set; }
        public double Displacement { get; set; }
        public CompartmentDto ParentCompartment { get; set; }
        public int? SpecialDepth { get; set; }
    }
}