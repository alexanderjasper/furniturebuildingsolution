using System;
using System.Collections.Generic;
using System.Linq;

namespace FurnitureBuildingSolution.Entities
{
    public class Plate : BaseEntity
    {
        private Bookcase _bookcase;
        public Plate(Bookcase bookcase)
        {
            _bookcase = bookcase;
        }
        public Corner Start { get; set; }
        public Corner End { get; set; }
        public List<Corner> IntermediateCorners { get; set; }
        public bool InnerPlateStart { get; set; }
        public bool InnerPlateEnd { get; set; }
        public bool Draggable { get; set; }
        public Compartment ParentCompartment { get; set; }
        public int? SpecialDepth { get; set; }
        public bool IsHorizontal => Start.y == End.y;
        public double Length => GetLength();
        public double Depth
        {
            get
            {
                var plateDepth = SpecialDepth ?? _bookcase.PlateDepth;
                return ParentCompartment != null ? plateDepth - _bookcase.DoorPlateThickness : plateDepth;
            }
        }
        public double Thickness => _bookcase.PlateThickness;
        public double DoorThickness => _bookcase.DoorPlateThickness;
        public List<Corner> AllCorners
        {
            get
            {
                var corners = new List<Corner>() { Start };
                corners.AddRange(IntermediateCorners.OrderBy(c => c.x).ThenBy(c => c.y));
                corners.Add(End);
                return corners;
            }
        }

        private double GetLength()
        {
            double length = 0;
            if (IsHorizontal)
            {
                length = End.x - Start.x;
            }
            else
            {
                length = End.y - Start.y;
            }
            var lengthIncrement = _bookcase.PlateThickness / 2;

            if (InnerPlateStart)
                length -= lengthIncrement;
            else
                length += lengthIncrement;

            if (InnerPlateEnd)
                length -= lengthIncrement;
            else
                length += lengthIncrement;

            return length;
        }
    }
}
