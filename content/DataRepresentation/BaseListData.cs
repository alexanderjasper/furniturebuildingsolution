using System;

namespace FurnitureBuildingSolution.DataRepresentation
{
    public class BaseListData
    {
        public int? Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}