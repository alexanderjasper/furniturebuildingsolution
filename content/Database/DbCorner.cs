using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureBuildingSolution.Database
{
    [Table("Corners")]
    public class DbCorner : BaseDbEntity
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}