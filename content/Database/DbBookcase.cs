using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Database
{
    [Table("Bookcases")]
    public class DbBookcase : BaseDbEntity
    {
        public string Name { get; set; }
        public DbUser User { get; set; }
        public List<DbCompartment> Compartments { get; set; }
        public List<DbPlate> Plates { get; set; }
        public DbMaterial Material { get; set; }
        public bool LockedForEditing { get; set; }
        public int PlateDepth { get; set; }
        public bool IsWallSuspended { get; set; }
        public double? SkirtingBoardDepth { get; set; }
        public double? SkirtingBoardHeight { get; set; }
        public double? BaseHeight { get; set; }
        public double? StartingPrice { get; set; }
        public double? PriceIndex { get; set; }
        public DoorMaterial DoorMaterial { get; set; }
    }
}
