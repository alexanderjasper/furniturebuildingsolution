using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Blueprint
{
    public class BlueprintSocket
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Direction Direction { get; set; }
        public bool Rotate { get; set; }
        public bool FlushCap { get; set; }
    }
}