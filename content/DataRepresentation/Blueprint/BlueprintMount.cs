namespace FurnitureBuildingSolution.Blueprint
{
    public class BlueprintMount
    {
        public BlueprintMount(double x, double y, string type, double diameter)
        {
            X = x;
            Y = y;
            Type = type;
            Diameter = diameter;
        }
        public double X { get; set; }
        public double Y { get; set; }
        public string Type { get; set; }
        public double Diameter { get; set; }
    }
}
