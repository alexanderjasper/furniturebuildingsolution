using System;

namespace FurnitureBuildingSolution.Entities
{
    public class BaseEntity
    {
        public int? Id { get; set; }
        public int? TemporaryId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}