using System;

namespace FurnitureBuildingSolution.ViewModels
{
    public class BaseViewModel
    {
        public int? Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Deleted { get; set; }
    }
}