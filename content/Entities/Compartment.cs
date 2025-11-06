using System;
using System.Collections.Generic;
using System.Linq;
using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Entities
{
    public class Compartment : BaseEntity
    {
        private Bookcase _bookcase;
        public Compartment(Bookcase bookcase)
        {
            _bookcase = bookcase;
        }

        public Plate Top { get; set; }
        public Plate Bottom { get; set; }
        public Plate Left { get; set; }
        public Plate Right { get; set; }
        public bool HasDoor { get; set; }
        public DoorPosition DoorPosition { get; set; }
        public bool HasDrawer { get; set; }
        public string Model { get; set; }
        public ForceSupport ForceSupport { get; set; }
        public double? BackPlatePosition { get; set; }
        // TODO: Do this smarter than having 3 properties
        public List<Plate> InnerPlates => _bookcase.Plates.Where(plate => plate.ParentCompartment != null && plate.ParentCompartment.Id == Id).ToList();
        public bool HasSupport => (Width > _bookcase.MaxLengthWithoutSupport || ForceSupport == ForceSupport.ForceStandard || ForceSupport == ForceSupport.Top) && BackPlatePosition == null && ForceSupport != ForceSupport.None;
        public bool HasDoubleSupport => Width > _bookcase.MaxLengthWithoutDoubleSupport && BackPlatePosition == null;
        public double xCenter => (double)(Math.Min(Top.End.x, Right.End.x) + Math.Max(Top.Start.x, Left.End.x)) / 2;
        public double yCenter => (double)(Math.Max(Left.Start.y, Bottom.Start.y) + Math.Min(Left.End.y, Top.Start.y)) / 2;
        public int LeftBound => Math.Max(Top.Start.x, Left.Start.x);
        public int RightBound => Math.Min(Top.End.x, Right.Start.x);
        public int LowerBound => Math.Max(Left.Start.y, Bottom.Start.y);
        public int UpperBound => Math.Min(Left.End.y, Top.Start.y);
        public int Width => RightBound - LeftBound;
        public int Height => UpperBound - LowerBound;
        public bool IsFloatingCompartment => Bottom.Start.y > 0 && !_bookcase.Compartments.Any(c => c.Top.Id == Bottom.Id);
    }
}
