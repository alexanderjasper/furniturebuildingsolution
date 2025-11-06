using System;

namespace FurnitureBuildingSolution.Database
{
    public class BaseDbEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}