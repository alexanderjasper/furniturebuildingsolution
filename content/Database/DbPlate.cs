using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureBuildingSolution.Database
{
    [Table("Plates")]
    public class DbPlate : BaseDbEntity
    {
        public int StartXCoordinate { get; set; }
        public int StartYCoordinate { get; set; }
        public int EndXCoordinate { get; set; }
        public int EndYCoordinate { get; set; }
        public bool InnerPlateStart { get; set; }
        public bool InnerPlateEnd { get; set; }
        public bool Draggable { get; set; }
        [ForeignKey("ParentCompartmentId")]
        public virtual DbCompartment ParentCompartment { get; set; }
        public int? SpecialDepth { get; set; }
    }
}