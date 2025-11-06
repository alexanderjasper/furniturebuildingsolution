namespace FurnitureBuildingSolution.Entities
{
    public class Corner : BaseEntity
    {
        public Corner()
        {
        }

        public Corner(int xCoordinate, int yCoordinate, int id)
        {
            x = xCoordinate;
            y = yCoordinate;
            Id = id;
        }
        public int x { get; set; }
        public int y { get; set; }
    }
}