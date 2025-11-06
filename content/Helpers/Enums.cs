namespace FurnitureBuildingSolution.Helpers
{
    public class Enums
    {
        public enum Role
        {
            Customer = 0,
            Admin = 1
        }
        public enum OrderStatus
        {
            New = 0
        }
        public enum Direction
        {
            Increasing = 1,
            Decreasing = -1
        }

        public enum PlateEnd
        {
            Beginning,
            End
        }

        public enum CabineoSide
        {
            Top,
            Bottom
        }

        public enum PlateSide
        {
            Before,
            After
        }

        public enum Side
        {
            Top,
            Right,
            Bottom,
            Left,
            Through
        }

        public enum MaterialType
        {
            UntreadedPlywood = 0,
            PaintedPlywood = 1,
            MultiwallPlywood = 2,
            LaminatePlywood = 3,
            MelaminPlywood = 4,
            FormworkPlywood = 5
        }

        public enum DoorMaterial
        {
            ValchromatLightGrey = 0,
            ValchromatLightGreyWhiteFront = 1,
            MdfWhite = 2
        }

        public enum DoorPosition
        {
            Left = 0,
            Right = 1,
            Double = 2
        }

        public enum SupportType
        {
            Default,
            BackPlate,
            CoverPlate
        }

        public enum ForceSupport
        {
            Default = 0,
            ForceStandard = 1,
            None = 2,
            Top = 3,
            Bottom = 4,
            WallMount = 5
        }
    }
}
