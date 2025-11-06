using System;
using System.Collections.Generic;
using FurnitureBuildingSolution.Entities;
using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Blueprint
{
    public class BlueprintPlate
    {
        public BlueprintPlate(Plate inputPlate, CabineoSide cabineoSide, Bookcase bookcase)
        {
            _bookcase = bookcase;

            Length = inputPlate.Length;
            Position = new BlueprintPosition()
            {
                X = (double)(inputPlate.Start.x + inputPlate.End.x) / 2,
                Y = -inputPlate.Depth / 2,
                Z = (double)(inputPlate.Start.y + inputPlate.End.y) / 2 + VerticalOffset
            };

            if (bookcase.BaseHeight != null && !inputPlate.InnerPlateStart && !inputPlate.IsHorizontal && inputPlate.Start.y == bookcase.BottomBound)
            {
                Length += bookcase.BaseHeight.Value;
                Position.Z -= bookcase.BaseHeight.Value / 2;
            }

            PlateId = inputPlate.Id;
            Type = "plate";
            Depth = inputPlate.Depth;
            Thickness = inputPlate.Thickness;
            Sockets = new List<BlueprintSocket>();
            Mounts = new List<BlueprintMount>();
            HingeCups = new List<BlueprintHingeCup>();
            Cutouts = new List<BlueprintCutout>();
            

            double shift = 0;
            double shiftIncrement = inputPlate.Thickness / 4;
            if (inputPlate.InnerPlateStart)
                shift += shiftIncrement;
            else
                shift -= shiftIncrement;
            if (inputPlate.InnerPlateEnd)
                shift -= shiftIncrement;
            else
                shift += shiftIncrement;
            if (inputPlate.IsHorizontal)
                Position.X += shift;
            else
                Position.Z += shift;

            Rotation = new BlueprintRotation()
            {
                X = cabineoSide == CabineoSide.Top ? 0 : 1,
                Y = inputPlate.IsHorizontal ? 0 : (double)0.5,
                Z = 0
            };
            CabineoSide = cabineoSide;
        }
        public BlueprintPlate(Bookcase bookcase) {
            _bookcase = bookcase;
            Sockets = new List<BlueprintSocket>();
            Mounts = new List<BlueprintMount>();
            HingeCups = new List<BlueprintHingeCup>();
            Cutouts = new List<BlueprintCutout>();
        }

        private Bookcase _bookcase { get; set; }
        private double VerticalOffset => _bookcase.PlateThickness / 2;
        private bool IsHorizontal => Rotation.Y == 0;

        public int? PlateId { get; set; }
        public int? CompartmentId { get; set; }
        public double Length { get; set; }
        public double Depth { get; set; }
        public double Thickness { get; set; }
        public List<BlueprintSocket> Sockets { get; set; }
        public List<BlueprintMount> Mounts { get; set; }
        public List<BlueprintHingeCup> HingeCups { get; set; }
        public List<BlueprintCutout> Cutouts { get; set; }
        public BlueprintPosition Position { get; private set; }
        public BlueprintRotation Rotation { get; set; }
        public CabineoSide CabineoSide { get; set; }
        public string Type { get; set; }
        public void AddMount(double worldX, double worldY, double worldZ, double diameter, Side side)
        {
            var adjustedZ = worldZ - Position.Y;

            if (IsHorizontal)
            {
                Mounts.Add(new BlueprintMount(worldX - Position.X, Rotation.X == 0 ? adjustedZ : -adjustedZ, GetMountType(side), diameter));
            } else
            {
                Mounts.Add(new BlueprintMount(worldY - Position.Z + VerticalOffset, Rotation.X == 0 ? adjustedZ : -adjustedZ, GetMountType(side), diameter));
            }
        }
        public void SetPosition(double worldX, double worldY, double worldZ)
        {
            Position = new BlueprintPosition()
            {
                X = worldX,
                Y = worldZ,
                Z = worldY + VerticalOffset
            };
        }
        private string GetMountType(Side side)
        {
            if (IsHorizontal)
            {
                if (side == Side.Top)
                {
                    if (Rotation.X == 0)
                    {
                        return "top";
                    }
                    else
                    {
                        return "bottom";
                    }
                }
                else if (side == Side.Bottom)
                {
                    if (Rotation.X == 0)
                    {
                        return "bottom";
                    }
                    else
                    {
                        return "top";
                    }
                }
                else if (side == Side.Through)
                {
                    return "through";
                }
            }
            else
            {
                if (side == Side.Right)
                {
                    if (Rotation.X == 1)
                    {
                        return "top";
                    }
                    else
                    {
                        return "bottom";
                    }
                }
                else if (side == Side.Left)
                {
                    if (Rotation.X == 1)
                    {
                        return "bottom";
                    }
                    else
                    {
                        return "top";
                    }
                }
                else if (side == Side.Through)
                {
                    return "through";
                }
            }
            throw new Exception("Mount added to plate was not of valid type.");
        }
    }
}
