using System;

namespace FurnitureBuildingSolution.Dtos
{
    public class BaseDto
    {
        public int? Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}