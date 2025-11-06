using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Database
{
    [Table("Compartments")]
    public class DbCompartment : BaseDbEntity
    {
        public DbPlate Top { get; set; }
        public DbPlate Bottom { get; set; }
        public DbPlate Left { get; set; }
        public DbPlate Right { get; set; }
        public bool HasDoor { get; set; }
        public DoorPosition DoorPosition { get; set; }
        public bool HasDrawer { get; set; }
        public string Model { get; set; }
        public virtual List<DbPlate> InternalPlates { get; set; }
        public ForceSupport ForceSupport { get; set; }
        public double? BackPlatePosition { get; set; }
    }
}