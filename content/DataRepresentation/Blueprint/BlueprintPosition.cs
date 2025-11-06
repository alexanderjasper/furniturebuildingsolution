namespace FurnitureBuildingSolution.Blueprint
{
    public class BlueprintPosition
    {
        public BlueprintPosition() { }
        public BlueprintPosition(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}