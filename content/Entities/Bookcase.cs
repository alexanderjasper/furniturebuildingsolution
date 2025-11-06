using System;
using System.Collections.Generic;
using System.Linq;
using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Entities
{
    public class Bookcase : BaseEntity
    {
        public User User { get; set; }
        public string Name { get; set; }
        public List<Compartment> Compartments { get; set; }
        public List<Plate> Plates { get; set; }
        public List<Corner> Corners { get; set; }
        public Material Material { get; set; }
        public int SupportHeight => 60;
        public double PlateThickness { get; set; }
        public double DoorPlateThickness { get; set; }
        public int PlateDepth { get; set; }
        public bool IsWallSuspended { get; set; }
        public double? SkirtingBoardDepth { get; set; }
        public double? SkirtingBoardHeight { get; set; }
        public double? BaseHeight { get; set; }
        public double? StartingPrice { get; set; }
        public double? PriceIndex { get; set; }
        public DoorMaterial DoorMaterial { get; set; }
        // TODO: Avoid hard coded values
        public double MaxLengthWithoutSupport => 500 + Math.Round(PlateThickness);
        public double MaxLengthWithoutDoubleSupport => MaxLengthWithoutSupport + 300;
        public bool LockedForEditing { get; set; }
        public int BottomBound => Corners.Min(c => c.y);
        public int TopBound => Corners.Max(c => c.y);
        public int LeftBound => Corners.Min(c => c.x);
        public int RightBound => Corners.Max(c => c.x);
        public double xCenter => (LeftBound + RightBound) / 2;

        public Bookcase(Material material)
        {
            Material = material;
            PlateThickness = Material.Thickness;
        }

        public Bookcase(Material material, double plateThickness)
        {
            PlateThickness = plateThickness;
        }

        public List<Corner> GetIntermediateCorners(Plate plate)
        {
            var intermediateCorners = new List<Corner>();
            if (plate.IsHorizontal)
            {
                intermediateCorners = Corners
                    .Where(corner => corner.y == plate.Start.y && corner.x > plate.Start.x && corner.x < plate.End.x)
                    .OrderBy(corner => corner.x).ToList();
            }
            else
            {
                intermediateCorners = Corners
                    .Where(corner => corner.x == plate.Start.x && corner.y > plate.Start.y && corner.y < plate.End.y)
                    .OrderBy(corner => corner.y).ToList();
            }
            return intermediateCorners;
        }

        //TODO: These should be stored in DB for every bookcase:
        private double PricePerPiece = 100;
        private double DefaultStartingPrice = 500;
        private decimal ExtraPricePerSquareMeter = 260;
        private double VATRate => 1.25;
        private decimal PricePerSquareMeter => ((decimal)1.3 * Material.PricePerSquareMeter + ExtraPricePerSquareMeter);

        private double PlatePricePerMillimeter => ((double)PricePerSquareMeter * (double)PlateDepth) / 1000000;
        private double SupportPricePerMillimeter => ((double)PricePerSquareMeter * (double)SupportHeight) / 1000000;

        public int SalesPrice
        {
            get
            {
                double price = StartingPrice != null ? StartingPrice.Value : DefaultStartingPrice;
                foreach (var plate in Plates)
                {
                    price += PlatePrice(plate);
                }
                foreach (var compartment in Compartments)
                {
                    price += SupportPrice(compartment.Width);
                    if (compartment.HasDoor)
                    {
                        price += DoorPrice(compartment);
                    }
                }
                if (IsWallSuspended)
                {
                    price *= 1.30;
                }
                else if (BaseHeight != null && BaseHeight > 0)
                {
                    price += BasePrice();
                }
                if (SkirtingBoardDepth != null && SkirtingBoardHeight != null)
                {
                    price *= 1.08;
                }
                if (PriceIndex != null)
                {
                    price *= PriceIndex.Value;
                }
                //Price including VAT:
                return (int)Math.Round(price * VATRate);
            }

        }

        private double BasePrice()
        {
            if (this.BaseHeight == null || BaseHeight == 0)
            {
                return 0;
            }

            double numberOfBottomPlates = 0;
            double totalBaseLength = 0;

            foreach (var plate in Plates)
            {
                if (plate.Start.y == 0 && plate.End.y == 0)
                {
                    numberOfBottomPlates += 1;
                    totalBaseLength += plate.Length;
                }
            }
            return 400 + this.PricePerPiece * numberOfBottomPlates + BaseHeight.Value * totalBaseLength * (double)PricePerSquareMeter / 1000000;
        }

        private double DoorPrice(Compartment compartment)
        {
            var areaSquareMeters = (double)(compartment.Width * compartment.Height) / 1000000;
            //TODO: Calculated square meter price
            double squareMeterPrice, pricePerDoor;
            squareMeterPrice = 350 * 1.3 + (double)ExtraPricePerSquareMeter;
            pricePerDoor = 200;
            return areaSquareMeters * squareMeterPrice + pricePerDoor;
        }

        private double SupportPrice(int width)
        {
            double extraSupportPrice = 0;
            if (width > MaxLengthWithoutSupport)
            {
                extraSupportPrice += PricePerPiece + (width - PlateThickness) * SupportPricePerMillimeter;
            }
            if (width > MaxLengthWithoutSupport + 500)
            {
                //TODO: Define this value centrally. Also used in bookcase-support.vue.
                extraSupportPrice += extraSupportPrice;
            }
            return extraSupportPrice;
        }

        private double PlatePrice(Plate plate)
        {
            double price = 0;

            if (plate.ParentCompartment != null)
            {
                price += SupportPrice(plate.ParentCompartment.Width);
            }
            price += PricePerPiece + plate.Length * PlatePricePerMillimeter;

            return price;
        }
    }
}
