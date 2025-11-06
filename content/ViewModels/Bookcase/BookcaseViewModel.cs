using System;
using System.Collections.Generic;

namespace FurnitureBuildingSolution.ViewModels.Bookcase
{
    public class BookcaseViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public List<CompartmentViewModel> Compartments { get; set; }
        public List<PlateViewModel> Plates { get; set; }
        public List<CornerViewModel> Corners { get; set; }
        public bool LockedForEditing { get; set; }
    }
}