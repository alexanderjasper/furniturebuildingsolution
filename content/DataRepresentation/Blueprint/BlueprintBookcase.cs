using System;
using System.Collections.Generic;
using System.Linq;
using FurnitureBuildingSolution.Entities;
using FurnitureBuildingSolution.Helpers;
using static FurnitureBuildingSolution.Helpers.Enums;

namespace FurnitureBuildingSolution.Blueprint
{
    public class BlueprintBookcase
    {
        public int BookcaseId { get; set; }
        public List<BlueprintPlate> Plates { get; set; }
        private readonly Bookcase _bookcase;
        private double _centerOffsetShift => (double)3.5;
        private double _doorEdgeGap => 3;
        private double _hingeEdgeOffset => 68;
        private double _hingeShift => 8;
        private double _hingeMountEdgeOffset => 37;
        private double _hingeMountCC => 32;
        private double _doorMountHoleDiameter => 5;
        private double _doorStopHoleDiameter => 10;
        private double _rearCabineoGap => 1.5;
        private double _cabineoMountHoleDiameter => 5;
        private double _skirtingBoardHeightMargin = 10;
        private double _skirtingBoardDepthMargin = 5;
        private double _yPositionOffset { get; set; }
        private double _baseFrontShiftBack = 20;

        public BlueprintBookcase(Bookcase bookcase, bool includeFrame, bool includeDoors)
        {
            _bookcase = bookcase;
            if (_bookcase.Id == null)
            {
                throw new ArgumentException("Cannot create BlueprintBookcase for a Bookcase without an Id");
            }
            else
            {
                _yPositionOffset = _bookcase.PlateThickness / 2;
                BookcaseId = _bookcase.Id.Value;
                Plates = new List<BlueprintPlate>();

                if (includeFrame)
                {
                    foreach (var plate in _bookcase.Plates)
                    {
                        AddPlate(plate);
                    }
                    if (_bookcase.BaseHeight != null && _bookcase.BaseHeight > 0)
                    {
                        AddBase();
                    }
                }

                foreach (var compartment in _bookcase.Compartments)
                {
                    if (includeFrame)
                    {
                        if (compartment.ForceSupport == ForceSupport.WallMount)
                        {
                            AddWallMount(compartment);
                        }
                        else if (compartment.HasSupport)
                        {
                            AddSupport(compartment, _bookcase.SupportHeight, SupportType.Default);
                        }
                        else if (compartment.BackPlatePosition != null)
                        {
                            AddBackPlate(compartment);
                        }
                    }
                    //AddDoor should be run even when not including doors, in order to specify hinge mounting holes.
                    AddDoor(compartment, includeFrame, includeDoors);
                }
            }
        }

        private void AddBase()
        {
            var dividingPlates = _bookcase.Plates
                .Where(plate => !plate.IsHorizontal && plate.Start.y == _bookcase.BottomBound && !plate.InnerPlateStart)
                .OrderBy(plate => plate.Start.x)
                .ToList();

            for (var i = 0; i < dividingPlates.Count() - 1; i++)
            {
                if (BottomCompartmentExists(dividingPlates[i]))
                {
                    AddBaseSection(dividingPlates[i], dividingPlates[i + 1]);
                }
            }
        }

        private void AddBaseSection(Plate leftBoundingPlate, Plate rightBoundingPlate)
        {
            var depth = Math.Min(leftBoundingPlate.Depth, rightBoundingPlate.Depth);
            var leftBound = leftBoundingPlate.Start.x;
            var rightBound = rightBoundingPlate.Start.x;
            var leftBlueprintPlate = Plates.Single(p => p.PlateId == leftBoundingPlate.Id);
            var rightBlueprintPlate = Plates.Single(p => p.PlateId == rightBoundingPlate.Id);

            var plate = new BlueprintPlate(_bookcase);
            var plateYCenter = _bookcase.BottomBound - (_bookcase.BaseHeight.Value + _bookcase.PlateThickness) / 2;
            plate.SetPosition((leftBound + rightBound) / 2, plateYCenter, -depth + _baseFrontShiftBack + _bookcase.PlateThickness / 2);
            plate.Rotation = new BlueprintRotation()
            {
                X = (double)-0.5,
                Y = 0,
                Z = 0
            };
            plate.Length = rightBound - leftBound - _bookcase.PlateThickness;
            plate.Depth = _bookcase.BaseHeight.Value;
            plate.Thickness = _bookcase.PlateThickness;

            var socketXShift = plate.Length / 2;
            var socketLeftYShift = plate.Depth / 3 - _centerOffsetShift;
            var socketRightYShift = plate.Depth / 3 + _centerOffsetShift;
            var mountDepthPosition = -depth + _baseFrontShiftBack + _bookcase.PlateThickness - 5;

            plate.Sockets.Add(new BlueprintSocket()
            {
                X = -plate.Length / 2,
                Y = socketLeftYShift,
                Direction = Direction.Increasing,
                Rotate = false,
                FlushCap = false
            });
            leftBlueprintPlate.AddMount(0, plateYCenter + socketLeftYShift, mountDepthPosition, _cabineoMountHoleDiameter, Side.Right);
            plate.Sockets.Add(new BlueprintSocket()
            {
                X = -plate.Length / 2,
                Y = -socketLeftYShift,
                Direction = Direction.Increasing,
                Rotate = false,
                FlushCap = false
            });
            leftBlueprintPlate.AddMount(0, plateYCenter - socketLeftYShift, mountDepthPosition, _cabineoMountHoleDiameter, Side.Right);

            plate.Sockets.Add(new BlueprintSocket()
            {
                X = plate.Length / 2,
                Y = socketRightYShift,
                Direction = Direction.Decreasing,
                Rotate = false,
                FlushCap = false
            });
            rightBlueprintPlate.AddMount(0, plateYCenter + socketRightYShift, mountDepthPosition, _cabineoMountHoleDiameter, Side.Left);
            plate.Sockets.Add(new BlueprintSocket()
            {
                X = plate.Length / 2,
                Y = -socketRightYShift,
                Direction = Direction.Decreasing,
                Rotate = false,
                FlushCap = false
            });
            rightBlueprintPlate.AddMount(0, plateYCenter - socketRightYShift, mountDepthPosition, _cabineoMountHoleDiameter, Side.Left);

            Plates.Add(plate);

            var rearDepthPosition = (_bookcase.SkirtingBoardDepth != null ? -_bookcase.SkirtingBoardDepth.Value - _skirtingBoardDepthMargin : 0) - _rearCabineoGap - _bookcase.PlateThickness / 2;
            var rearMountDepthPosition = rearDepthPosition + _bookcase.PlateThickness / 2 - 5;
            var rearPlate = new BlueprintPlate(_bookcase);
            rearPlate.SetPosition((leftBound + rightBound) / 2, plateYCenter, rearDepthPosition);
            rearPlate.Rotation = new BlueprintRotation()
            {
                X = (double)-0.5,
                Y = 0,
                Z = 0
            };
            rearPlate.Length = rightBound - leftBound - _bookcase.PlateThickness;
            rearPlate.Depth = _bookcase.BaseHeight.Value;
            rearPlate.Thickness = _bookcase.PlateThickness;

            plate.Sockets.Add(new BlueprintSocket()
            {
                X = -rearPlate.Length / 2,
                Y = socketLeftYShift,
                Direction = Direction.Increasing,
                Rotate = false,
                FlushCap = false
            });
            leftBlueprintPlate.AddMount(0, plateYCenter + socketLeftYShift, rearMountDepthPosition, _cabineoMountHoleDiameter, Side.Right);
            rearPlate.Sockets.Add(new BlueprintSocket()
            {
                X = -rearPlate.Length / 2,
                Y = -socketLeftYShift,
                Direction = Direction.Increasing,
                Rotate = false,
                FlushCap = false
            });
            leftBlueprintPlate.AddMount(0, plateYCenter - socketLeftYShift, rearMountDepthPosition, _cabineoMountHoleDiameter, Side.Right);

            rearPlate.Sockets.Add(new BlueprintSocket()
            {
                X = rearPlate.Length / 2,
                Y = socketRightYShift,
                Direction = Direction.Decreasing,
                Rotate = false,
                FlushCap = false
            });
            rightBlueprintPlate.AddMount(0, plateYCenter + socketRightYShift, rearMountDepthPosition, _cabineoMountHoleDiameter, Side.Left);
            rearPlate.Sockets.Add(new BlueprintSocket()
            {
                X = rearPlate.Length / 2,
                Y = -socketRightYShift,
                Direction = Direction.Decreasing,
                Rotate = false,
                FlushCap = false
            });
            rightBlueprintPlate.AddMount(0, plateYCenter - socketRightYShift, rearMountDepthPosition, _cabineoMountHoleDiameter, Side.Left);

            Plates.Add(rearPlate);
        }

        private bool BottomCompartmentExists(Plate leftPlate)
        {
            return _bookcase.Compartments.Any(compartment =>
                compartment.Left.Id == leftPlate.Id && compartment.LowerBound == _bookcase.BottomBound
            );
        }

        private void AddWallMount(Compartment compartment)
        {
            var bookcaseMountHeight = 60;
            var wallMountHeight = 80;
            var wallMountWidthMargin = 100;
            double coverPlateHeight = bookcaseMountHeight + wallMountHeight + 20;
            if (compartment.Height - coverPlateHeight < 100)
            {
                coverPlateHeight = (double)compartment.Height - _bookcase.PlateThickness;
            }

            AddSupport(compartment, bookcaseMountHeight, SupportType.Default);
            var holeDistance = 50;
            var numberOfHoles = Math.Floor((double)((compartment.Width - wallMountWidthMargin) / holeDistance));

            var wallMountedPlate = new BlueprintPlate(_bookcase)
            {
                CompartmentId = compartment.Id,
                Length = compartment.Width - 100,
                Depth = wallMountHeight,
                Thickness = _bookcase.PlateThickness,
                Type = "wallmount",
                Rotation = new BlueprintRotation()
                {
                    X = (double)-0.5,
                    Y = 0,
                    Z = 0
                }
            };
            wallMountedPlate.SetPosition(compartment.xCenter, compartment.Top.Start.y - ((double)(wallMountHeight) / 2) - bookcaseMountHeight - 10, -_bookcase.PlateThickness / 2);

            var firstHoleOffset = (wallMountedPlate.Length - holeDistance * (numberOfHoles - 1)) / 2;
            var holeXPosition = (-wallMountedPlate.Length) / 2 + firstHoleOffset;
            for (var i = 0; i < numberOfHoles; i++)
            {
                wallMountedPlate.Mounts.Add(new BlueprintMount(holeXPosition, -5, "through", 6));
                wallMountedPlate.Mounts.Add(new BlueprintMount(holeXPosition, 25, "through", 6));
                holeXPosition += holeDistance;
            }

            Plates.Add(wallMountedPlate);
            AddSupport(compartment, coverPlateHeight, SupportType.CoverPlate);
        }

        private double CenterOffset(Plate plate)
        {
            if (_bookcase.SkirtingBoardHeight != null && _bookcase.SkirtingBoardDepth != null & plate.Start.y <= _bookcase.SkirtingBoardHeight + _bookcase.PlateThickness / 2)
            {
                return Math.Min(plate.Depth / 3, plate.Depth / 2 - _bookcase.SkirtingBoardDepth.Value - _skirtingBoardDepthMargin - 30);
            }
            return (double)(plate.Depth / 3);
        }


        private void AddBackPlate(Compartment compartment)
        {
            if (compartment.BackPlatePosition == null) { return; }
            var cutouts = new List<BlueprintCutout>();
            cutouts.Add(new BlueprintCutout()
            {
                Type = "rectangle",
                X = 0,
                Y = compartment.Height / 2 - Math.Max(150, compartment.Height / 3),
                XSize = compartment.Width / 4,
                YSize = 100
            });
            AddSupport(compartment, compartment.Height - _bookcase.PlateThickness, SupportType.BackPlate, null, cutouts);
        }

        private void AddDoor(Compartment compartment, bool includeFrame, bool includeDoors)
        {
            if (!compartment.HasDoor)
            {
                return;
            }

            if (includeFrame)
            {
                AddDoorMounts(compartment);
            }

            if (includeDoors)
            {
                AddDoorPlate(compartment);
            }
        }

        private void AddDoorMounts(Compartment compartment)
        {
            if (GetHingeSide(compartment) == "left")
            {
                if (compartment.InnerPlates.Count > 0)
                {
                    AddDoorHingeMounts(compartment.Left, compartment);
                }
                else
                {
                    AddDoorHingeMounts(compartment.Left, compartment, compartment.Right);
                }
            }
            else if (GetHingeSide(compartment) == "right")
            {
                if (compartment.InnerPlates.Count > 0)
                {
                    AddDoorHingeMounts(compartment.Right, compartment);
                }
                else
                {
                    AddDoorHingeMounts(compartment.Right, compartment, compartment.Left);
                }
            }
            else if (GetHingeSide(compartment) == "double")
            {
                AddDoorHingeMounts(compartment.Left, compartment);
                AddDoorHingeMounts(compartment.Right, compartment);
            }
        }

        private void AddDoorHingeMounts(Plate hingeInputPlate, Compartment compartment, Plate doorStopInputPlate = null)
        {
            var compartmentBottomY = (double)compartment.Bottom.Start.y;
            var compartmentTopY = (double)compartment.Top.Start.y;

            // TODO: This does not prevent collision of hinge mounts. Only makes it less likely.
            var shift = hingeInputPlate == compartment.Left ? _hingeShift : 0;
            var hinge1YPosition = compartmentBottomY + _bookcase.PlateThickness / 2 + _doorEdgeGap + _hingeEdgeOffset - shift;
            var hinge2YPosition = compartmentTopY - _bookcase.PlateThickness / 2 - _doorEdgeGap - _hingeEdgeOffset + shift;

            var hingeBlueprintPlate = Plates.First(Plate => Plate.PlateId == hingeInputPlate.Id);

            var hinge1YPositionOnPlate = hinge1YPosition - hingeBlueprintPlate.Position.Z + _yPositionOffset;
            var hinge2YPositionOnPlate = hinge2YPosition - hingeBlueprintPlate.Position.Z + _yPositionOffset;

            bool fromTop;
            double hingeMountZPosition;

            if (hingeInputPlate == compartment.Left)
            {
                fromTop = hingeBlueprintPlate.Rotation.X == 1;
                hingeMountZPosition = fromTop ? hingeBlueprintPlate.Depth / 2 - _hingeMountEdgeOffset - _bookcase.DoorPlateThickness : -hingeBlueprintPlate.Depth / 2 + _hingeMountEdgeOffset + _bookcase.DoorPlateThickness;
            }
            else
            {
                fromTop = hingeBlueprintPlate.Rotation.X == 0;
                hingeMountZPosition = !fromTop ? hingeBlueprintPlate.Depth / 2 - _hingeMountEdgeOffset - _bookcase.DoorPlateThickness : -hingeBlueprintPlate.Depth / 2 + _hingeMountEdgeOffset + _bookcase.DoorPlateThickness;
            }

            // Bottom hinge
            hingeBlueprintPlate.Mounts.Add(new BlueprintMount(hinge1YPositionOnPlate + _hingeMountCC / 2, hingeMountZPosition, "top", _doorMountHoleDiameter));
            hingeBlueprintPlate.Mounts.Add(new BlueprintMount(hinge1YPositionOnPlate - _hingeMountCC / 2, hingeMountZPosition, "top", _doorMountHoleDiameter));

            // Top hinge
            hingeBlueprintPlate.Mounts.Add(new BlueprintMount(hinge2YPositionOnPlate + _hingeMountCC / 2, hingeMountZPosition, "top", _doorMountHoleDiameter));
            hingeBlueprintPlate.Mounts.Add(new BlueprintMount(hinge2YPositionOnPlate - _hingeMountCC / 2, hingeMountZPosition, "top", _doorMountHoleDiameter));

            // TODO: Duplicated code
            if (doorStopInputPlate != null)
            {
                var doorStopBlueprintPlate = Plates.First(Plate => Plate.PlateId == doorStopInputPlate.Id);

                var doorStop1YPositionOnPlate = hinge1YPosition - doorStopBlueprintPlate.Position.Z + _yPositionOffset - shift;
                var doorStop2YPositionOnPlate = hinge2YPosition - doorStopBlueprintPlate.Position.Z + _yPositionOffset + shift;


                bool doorStopFromTop;
                double doorStopZPosition;

                if (doorStopInputPlate == compartment.Left)
                {
                    doorStopFromTop = doorStopBlueprintPlate.Rotation.X == 1;
                    doorStopZPosition = doorStopFromTop ? doorStopBlueprintPlate.Depth / 2 - _bookcase.DoorPlateThickness - _doorStopHoleDiameter / 2 : -doorStopBlueprintPlate.Depth / 2 + _bookcase.DoorPlateThickness + _doorStopHoleDiameter / 2;
                }
                else
                {
                    doorStopFromTop = doorStopBlueprintPlate.Rotation.X == 0;
                    doorStopZPosition = !doorStopFromTop ? doorStopBlueprintPlate.Depth / 2 - _bookcase.DoorPlateThickness - _doorStopHoleDiameter / 2 : -doorStopBlueprintPlate.Depth / 2 + _bookcase.DoorPlateThickness + _doorStopHoleDiameter / 2;
                }

                doorStopBlueprintPlate.Mounts.Add(new BlueprintMount(doorStop1YPositionOnPlate, doorStopZPosition, doorStopFromTop ? "top" : "bottom", _doorStopHoleDiameter));
                doorStopBlueprintPlate.Mounts.Add(new BlueprintMount(doorStop2YPositionOnPlate, doorStopZPosition, doorStopFromTop ? "top" : "bottom", _doorStopHoleDiameter));
            }


        }

        private void AddDoorPlate(Compartment compartment)
        {
            var doorHeight = compartment.Height - 2 * _doorEdgeGap - _bookcase.PlateThickness;
            if (compartment.DoorPosition == DoorPosition.Left || compartment.DoorPosition == DoorPosition.Right)
            {
                var plate = new BlueprintPlate(_bookcase)
                {
                    CompartmentId = compartment.Id,
                    Length = compartment.Width - 2 * _doorEdgeGap - _bookcase.PlateThickness,
                    Depth = doorHeight,
                    Type = GetHingeSide(compartment) + "Door",
                    Thickness = _bookcase.DoorPlateThickness,
                    Mounts = new List<BlueprintMount>() { },
                    Sockets = new List<BlueprintSocket>() { },
                    HingeCups = new List<BlueprintHingeCup>() {
                        new BlueprintHingeCup() {
                            HingeType = "36Snap",
                            HingeSide = GetHingeSide(compartment),
                            HingeEdgeOffset = GetHingeSide(compartment) == "left" ? _hingeEdgeOffset - _hingeShift: _hingeEdgeOffset
                        }
                    },
                    Cutouts = new List<BlueprintCutout>() { },
                    Rotation = new BlueprintRotation()
                    {
                        X = (double)-0.5,
                        Y = 0,
                        Z = 0
                    },
                    CabineoSide = CabineoSide.Bottom
                };
                plate.SetPosition(compartment.xCenter, compartment.yCenter, (double)-compartment.Top.Depth + _bookcase.DoorPlateThickness / 2);
                Plates.Add(plate);
            }
            if (compartment.DoorPosition == DoorPosition.Double)
            {
                var doorWidth = (compartment.Width - 2 * _doorEdgeGap - _bookcase.PlateThickness) / 2 - _doorEdgeGap / 2;
                var leftPlate = new BlueprintPlate(_bookcase)
                {
                    CompartmentId = compartment.Id,
                    Length = doorWidth,
                    Depth = doorHeight,
                    Type = "leftDoor",
                    Thickness = _bookcase.DoorPlateThickness,
                    Mounts = new List<BlueprintMount>() { },
                    Sockets = new List<BlueprintSocket>() { },
                    HingeCups = new List<BlueprintHingeCup>() {
                        new BlueprintHingeCup() {
                            HingeType = "36Snap",
                            HingeSide = "left",
                            HingeEdgeOffset = _hingeEdgeOffset - _hingeShift
                        }
                    },
                    Cutouts = new List<BlueprintCutout>() { },
                    Rotation = new BlueprintRotation()
                    {
                        X = (double)-0.5,
                        Y = 0,
                        Z = 0
                    },
                    CabineoSide = CabineoSide.Bottom
                };
                leftPlate.SetPosition(compartment.LeftBound + _bookcase.PlateThickness / 2 + _doorEdgeGap + doorWidth / 2, compartment.yCenter, (double)(-_bookcase.PlateDepth + _bookcase.DoorPlateThickness) / 2);
                var rightPlate = new BlueprintPlate(_bookcase)
                {
                    CompartmentId = compartment.Id,
                    Length = doorWidth,
                    Depth = doorHeight,
                    Type = "rightDoor",
                    Thickness = _bookcase.DoorPlateThickness,
                    Mounts = new List<BlueprintMount>() { },
                    Sockets = new List<BlueprintSocket>() { },
                    HingeCups = new List<BlueprintHingeCup>() {
                        new BlueprintHingeCup() {
                            HingeType = "36Snap",
                            HingeSide = "right",
                            HingeEdgeOffset = _hingeEdgeOffset
                        }
                    },
                    Cutouts = new List<BlueprintCutout>() { },
                    Rotation = new BlueprintRotation()
                    {
                        X = (double)-0.5,
                        Y = 0,
                        Z = 0
                    },
                    CabineoSide = CabineoSide.Bottom
                };
                rightPlate.SetPosition(compartment.RightBound - _bookcase.PlateThickness / 2 - _doorEdgeGap - doorWidth / 2, compartment.yCenter, (double)(-_bookcase.PlateDepth + _bookcase.DoorPlateThickness) / 2);
                Plates.Add(leftPlate);
                Plates.Add(rightPlate);
            }
        }

        private string GetHingeSide(Compartment compartment)
        {
            if (compartment.DoorPosition == DoorPosition.Left)
            {
                return "left";
            }
            else if (compartment.DoorPosition == DoorPosition.Right)
            {
                return "right";
            }
            else if (compartment.DoorPosition == DoorPosition.Double)
            {
                return "double";
            }
            throw new Exception("GetHingeSide cannot provide a meaningful result for a compartment without a DoorPosition");
        }

        private void AddSupport(Compartment compartment, double supportHeight, SupportType supportType, List<BlueprintMount> mounts = null, List<BlueprintCutout> cutouts = null)
        {
            var supportLeftExists = _bookcase.Compartments.Any(c => c.HasSupport && c.UpperBound == compartment.UpperBound && c.RightBound == compartment.LeftBound);
            var supportRightExists = _bookcase.Compartments.Any(c => c.HasSupport && c.UpperBound == compartment.UpperBound && c.LeftBound == compartment.RightBound);

            var supportCenterOffset = supportHeight / 4;


            var supportLength = compartment.Width - _bookcase.PlateThickness;
            var socketXShift = (double)supportLength / 2;

            var sockets = new List<BlueprintSocket>();
            var rotation = new BlueprintRotation()
            {
                X = (double)-0.5,
                Y = 0,
                Z = 0
            };
            var supportPosition = GetSupportPosition(compartment, supportHeight);

            var leftPlate = Plates.First(p => p.PlateId == compartment.Left.Id);
            var rightPlate = Plates.First(p => p.PlateId == compartment.Right.Id);
            var bottomPlate = Plates.First(p => p.PlateId == compartment.Bottom.Id);
            var topPlate = Plates.First(p => p.PlateId == compartment.Top.Id);

            double mountZPlacement = 0;

            if (supportType == SupportType.Default)
            {
                mountZPlacement = -5 - _rearCabineoGap;

                sockets.Add(new BlueprintSocket()
                {
                    X = -socketXShift,
                    Y = _centerOffsetShift,
                    Direction = Direction.Increasing,
                    Rotate = false,
                    FlushCap = false
                });
                leftPlate.AddMount(0, GetSupportPosition(compartment, supportHeight).Y - _centerOffsetShift, mountZPlacement, 5, supportLeftExists ? Side.Through : Side.Right);

                sockets.Add(new BlueprintSocket()
                {
                    X = socketXShift,
                    Y = -_centerOffsetShift,
                    Direction = Direction.Decreasing,
                    Rotate = false,
                    FlushCap = false
                });
                rightPlate.AddMount(0, GetSupportPosition(compartment, supportHeight).Y + _centerOffsetShift, mountZPlacement, 5, supportRightExists ? Side.Through : Side.Left);


                if (compartment.IsFloatingCompartment)
                {
                    sockets.Add(new BlueprintSocket()
                    {
                        X = 0,
                        Y = supportHeight / 2,
                        Direction = Direction.Decreasing,
                        Rotate = true,
                        FlushCap = false
                    });
                    bottomPlate.AddMount(GetSupportPosition(compartment, supportHeight).X, 0, mountZPlacement, 5, Side.Top);
                }
                else
                {
                    sockets.Add(new BlueprintSocket()
                    {
                        X = 0,
                        Y = -supportHeight / 2,
                        Direction = Direction.Increasing,
                        Rotate = true,
                        FlushCap = false
                    });
                    topPlate.AddMount(GetSupportPosition(compartment, supportHeight).X, 0, mountZPlacement, 5, Side.Bottom);
                }

                if (compartment.HasDoubleSupport)
                {
                    var frontSupportZPlacement = -_bookcase.PlateDepth + _bookcase.PlateThickness - 5;
                    leftPlate.AddMount(0, GetSupportPosition(compartment, supportHeight).Y - _centerOffsetShift, frontSupportZPlacement, 5, supportLeftExists ? Side.Through : Side.Right);
                    rightPlate.AddMount(0, GetSupportPosition(compartment, supportHeight).Y + _centerOffsetShift, frontSupportZPlacement, 5, supportRightExists ? Side.Through : Side.Left);

                    if (compartment.IsFloatingCompartment)
                    {
                        bottomPlate.AddMount(GetSupportPosition(compartment, supportHeight).X, 0, frontSupportZPlacement, 5, Side.Top);
                    }
                    else
                    {
                        topPlate.AddMount(GetSupportPosition(compartment, supportHeight).X, 0, frontSupportZPlacement, 5, Side.Bottom);
                    }
                    var extraSupportPlate = new BlueprintPlate(_bookcase)
                    {
                        CompartmentId = compartment.Id,
                        Length = supportLength,
                        Depth = supportHeight,
                        Type = "support",
                        Thickness = _bookcase.PlateThickness,
                        Sockets = sockets,
                        Mounts = new List<BlueprintMount>(),
                        HingeCups = new List<BlueprintHingeCup>(),
                        Cutouts = new List<BlueprintCutout>() { },
                        Rotation = rotation,
                        CabineoSide = CabineoSide.Top
                    };
                    extraSupportPlate.SetPosition(compartment.xCenter, compartment.Top.Start.y - ((double)(_bookcase.PlateThickness + supportHeight) / 2), (double)(-_bookcase.PlateDepth + _bookcase.PlateThickness) / 2);
                    Plates.Add(extraSupportPlate);
                }
            }
            else if (supportType == SupportType.BackPlate || supportType == SupportType.CoverPlate)
            {
                var socketLeftYShift = supportCenterOffset + _centerOffsetShift;
                var socketRightYShift = supportCenterOffset - _centerOffsetShift;

                sockets.Add(new BlueprintSocket()
                {
                    X = -socketXShift,
                    Y = -socketLeftYShift,
                    Direction = Direction.Increasing,
                    Rotate = false,
                    FlushCap = false
                });
                sockets.Add(new BlueprintSocket()
                {
                    X = -socketXShift,
                    Y = socketLeftYShift,
                    Direction = Direction.Increasing,
                    Rotate = false,
                    FlushCap = false
                });
                sockets.Add(new BlueprintSocket()
                {
                    X = socketXShift,
                    Y = -socketRightYShift,
                    Direction = Direction.Decreasing,
                    Rotate = false,
                    FlushCap = false
                });
                sockets.Add(new BlueprintSocket()
                {
                    X = socketXShift,
                    Y = socketRightYShift,
                    Direction = Direction.Decreasing,
                    Rotate = false,
                    FlushCap = false
                });

                if (supportType == SupportType.BackPlate)
                {
                    supportPosition.Z = -compartment.Top.Depth + _bookcase.PlateThickness / 2 + compartment.BackPlatePosition.Value;
                }
                else
                //Coverplate:
                {
                    supportPosition.Z = -_bookcase.PlateThickness * 2;
                }
                mountZPlacement = supportPosition.Z + _bookcase.PlateThickness / 2 - 5;

                leftPlate.AddMount(0, GetSupportPosition(compartment, supportHeight).Y - socketLeftYShift, mountZPlacement, 5, supportLeftExists ? Side.Through : Side.Right);
                leftPlate.AddMount(0, GetSupportPosition(compartment, supportHeight).Y + socketLeftYShift, mountZPlacement, 5, supportLeftExists ? Side.Through : Side.Right);
                rightPlate.AddMount(0, GetSupportPosition(compartment, supportHeight).Y - socketRightYShift, mountZPlacement, 5, supportRightExists ? Side.Through : Side.Left);
                rightPlate.AddMount(0, GetSupportPosition(compartment, supportHeight).Y + socketRightYShift, mountZPlacement, 5, supportRightExists ? Side.Through : Side.Left);

                //TODO hard coded value
                if (supportType == SupportType.BackPlate && compartment.Width > 400)
                {
                    var bottomSocketXPosition = compartment.xCenter;
                    var minimumDistanceToVerticalPlate = 30;
                    var adjacentPlate = _bookcase.Plates.FirstOrDefault(plate =>
                        !plate.IsHorizontal &&
                        plate.End.y == compartment.LowerBound &&
                        Math.Abs(plate.Start.x - bottomSocketXPosition) < minimumDistanceToVerticalPlate);
                    if (adjacentPlate != null)
                    {
                        bottomSocketXPosition -= (minimumDistanceToVerticalPlate - Math.Abs(adjacentPlate.Start.x - bottomSocketXPosition)) * Math.Sign(adjacentPlate.Start.x - bottomSocketXPosition);
                    }
                    var mountBottomBookcasePlate = _bookcase.Plates.First(plate =>
                        plate.IsHorizontal &&
                        plate.Start.y == compartment.LowerBound &&
                        plate.Start.x < bottomSocketXPosition &&
                        plate.End.x > bottomSocketXPosition
                    );
                    var mountBottomPlate = Plates.First(plate => plate.PlateId == mountBottomBookcasePlate.Id);
                    sockets.Add(new BlueprintSocket()
                    {
                        X = bottomSocketXPosition - supportPosition.X,
                        Y = supportHeight / 2,
                        Direction = Direction.Decreasing,
                        Rotate = true,
                        FlushCap = false
                    });
                    mountBottomPlate.AddMount(bottomSocketXPosition, 0, mountZPlacement, 5, Side.Top);
                }
            }

            var supportPlate = new BlueprintPlate(_bookcase)
            {
                CompartmentId = compartment.Id,
                Length = supportLength,
                Depth = supportHeight,
                Type = "support",
                Thickness = _bookcase.PlateThickness,
                Sockets = sockets,
                Mounts = mounts ?? new List<BlueprintMount>(),
                HingeCups = new List<BlueprintHingeCup>(),
                Cutouts = cutouts ?? new List<BlueprintCutout>() { },
                Rotation = rotation,
                CabineoSide = CabineoSide.Top
            };
            supportPlate.SetPosition(supportPosition.X, supportPosition.Y, supportPosition.Z);
            Plates.Add(supportPlate);
        }

        private BlueprintPosition GetSupportPosition(Compartment compartment, double supportHeight)
        {
            double y = 0;
            double z = 0;
            if (compartment.BackPlatePosition == null)
            {
                z = (double)(-_bookcase.PlateThickness) / 2 - _rearCabineoGap;
            }
            else
            {
                z = (double)(_bookcase.PlateThickness / 2 - compartment.Top.Depth + compartment.BackPlatePosition.Value);
            }
            if (compartment.IsFloatingCompartment && !(compartment.ForceSupport == ForceSupport.Top))
            {
                y = compartment.Bottom.Start.y + ((double)(_bookcase.PlateThickness + supportHeight) / 2);
            }
            else
            {
                y = compartment.Top.Start.y - ((double)(_bookcase.PlateThickness + supportHeight) / 2);
            }

            return new BlueprintPosition()
            {
                X = compartment.xCenter,
                Y = y,
                Z = z
            };
        }

        private void AddPlate(Plate inputPlate)
        {
            var cabineoSide = GetCabineoSide(inputPlate);
            var plate = new BlueprintPlate(inputPlate, cabineoSide, _bookcase);
            AddSocketsToPlate(plate, inputPlate);
            AddMounts(plate, inputPlate);
            AddCutouts(plate, inputPlate);

            Plates.Add(plate);
        }

        private void AddCutouts(BlueprintPlate plate, Plate inputPlate)
        {
            if (!ShouldAddCutouts(inputPlate))
            {
                return;
            }

            var yValue = (plate.Depth - _bookcase.SkirtingBoardDepth.Value - _skirtingBoardDepthMargin) / 2;

            if (inputPlate.IsHorizontal)
            {
                if (plate.Rotation.X == 1)
                {
                    yValue *= -1;
                }
                plate.Cutouts.Add(new BlueprintCutout()
                {
                    Type = "rectangle",
                    X = 0,
                    Y = yValue,
                    XSize = plate.Length + 1,
                    YSize = _bookcase.SkirtingBoardDepth.Value + _skirtingBoardDepthMargin
                });
            }
            else
            {
                if (plate.Rotation.X == 1)
                {
                    yValue *= -1;
                }
                plate.Cutouts.Add(new BlueprintCutout()
                {
                    Type = "rectangle",
                    X = (_bookcase.SkirtingBoardHeight.Value + _skirtingBoardHeightMargin) / 2 - plate.Position.Z,
                    Y = yValue,
                    XSize = _bookcase.SkirtingBoardHeight.Value + _skirtingBoardHeightMargin,
                    YSize = _bookcase.SkirtingBoardDepth.Value + _skirtingBoardDepthMargin
                });
            }
        }

        private bool ShouldAddCutouts(Plate inputPlate)
        {
            return _bookcase.SkirtingBoardDepth != null && _bookcase.SkirtingBoardHeight != null && inputPlate.Start.y <= _bookcase.SkirtingBoardHeight + _bookcase.PlateThickness / 2;
        }

        private CabineoSide GetCabineoSide(Plate inputPlate)
        {
            if (inputPlate.IsHorizontal)
            {
                return (_bookcase.BaseHeight == null && inputPlate.Start.y == _bookcase.BottomBound) ? CabineoSide.Top : CabineoSide.Bottom;
            }
            else
            {
                if (!CompartmentsExistLeft(inputPlate))
                {
                    return CabineoSide.Bottom;
                }
                if (!CompartmentsExistRight(inputPlate))
                {
                    return CabineoSide.Top;
                }

                var centerX = (double)(_bookcase.LeftBound + _bookcase.RightBound) / 2;
                return inputPlate.Start.x < centerX ? CabineoSide.Bottom : CabineoSide.Top;
            }
        }

        private bool CompartmentsExistRight(Plate inputPlate)
        {
            foreach (var compartment in _bookcase.Compartments)
            {
                if (CompartmentCoversRight(compartment, inputPlate.Start))
                {
                    foreach (var otherCompartment in _bookcase.Compartments)
                    {
                        if (CompartmentCoversRight(otherCompartment, inputPlate.End))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool CompartmentsExistLeft(Plate inputPlate)
        {
            foreach (var compartment in _bookcase.Compartments)
            {
                if (CompartmentCoversLeft(compartment, inputPlate.Start))
                {
                    foreach (var otherCompartment in _bookcase.Compartments)
                    {
                        if (CompartmentCoversLeft(otherCompartment, inputPlate.End))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool CompartmentCoversRight(Compartment compartment, Corner corner)
        {
            return compartment.LeftBound == corner.x && compartment.LowerBound <= corner.y && compartment.UpperBound >= corner.y;
        }

        private bool CompartmentCoversLeft(Compartment compartment, Corner corner)
        {
            return compartment.RightBound == corner.x && compartment.LowerBound <= corner.y && compartment.UpperBound >= corner.y;
        }

        private void AddMounts(BlueprintPlate plate, Plate inputPlate)
        {
            foreach (var corner in inputPlate.AllCorners)
            {
                AddMountsForCorner(corner, plate, inputPlate);
            }
        }

        private void AddMountsForCorner(Corner corner, BlueprintPlate plate, Plate inputPlate)
        {
            var plateBefore = _bookcase.Plates.FirstOrDefault(p => p.IsHorizontal == !inputPlate.IsHorizontal && p.End == corner && p.InnerPlateEnd);
            var plateAfter = _bookcase.Plates.FirstOrDefault(p => p.IsHorizontal == !inputPlate.IsHorizontal && p.Start == corner && p.InnerPlateStart);
            // var shouldShiftPosition = plateBefore && plateAfter;
            var shouldShiftPosition = true;
            var shouldSwapSides = plate.CabineoSide == CabineoSide.Bottom;

            var xPosition = inputPlate.IsHorizontal ? corner.x - plate.Position.X : corner.y - plate.Position.Z;

            var x = xPosition + GetCabineoMountOffset(corner, inputPlate);
            if (!inputPlate.IsHorizontal)
            {
                x += _yPositionOffset;
            }

            if (plateBefore != null)
            {
                var yShift = shouldShiftPosition ?
                    CenterOffset(plateBefore) - _centerOffsetShift :
                    CenterOffset(plateBefore);
                //TODO Use AddMount method
                var type = "";
                if (plateAfter != null && plateAfter.Depth > Math.Abs(yShift) + inputPlate.Depth / 2 + 3)
                {
                    type = "through";
                }
                else
                {
                    var fromTop = inputPlate.IsHorizontal ? shouldSwapSides : !shouldSwapSides;
                    type = fromTop ? "top" : "bottom";
                }
                plate.Mounts.Add(new BlueprintMount(x, yShift + GetYOffset(plateBefore, null, inputPlate), type, _cabineoMountHoleDiameter));
                plate.Mounts.Add(new BlueprintMount(x, -yShift + GetYOffset(plateBefore, null, inputPlate), type, _cabineoMountHoleDiameter));
            }
            if (plateAfter != null)
            {
                var yShift = shouldShiftPosition ?
                    CenterOffset(plateAfter) + _centerOffsetShift :
                    CenterOffset(plateAfter);

                var type = "";
                if (plateBefore != null && plateBefore.Depth > Math.Abs(yShift) + inputPlate.Depth / 2 + 3)
                {
                    type = "through";
                }
                else
                {
                    var fromTop = inputPlate.IsHorizontal ? !shouldSwapSides : shouldSwapSides;
                    type = fromTop ? "top" : "bottom";
                }
                plate.Mounts.Add(new BlueprintMount(x, yShift + GetYOffset(null, plateAfter, inputPlate), type, _cabineoMountHoleDiameter));
                plate.Mounts.Add(new BlueprintMount(x, -yShift + GetYOffset(null, plateAfter, inputPlate), type, _cabineoMountHoleDiameter));
            }
        }

        private double GetYOffset(Plate plateBefore, Plate plateAfter, Plate inputPlate)
        {
            var otherPlateDepth = plateBefore != null ? plateBefore.Depth : plateAfter.Depth;
            var offset = (inputPlate.Depth - otherPlateDepth) / 2;
            if (GetCabineoSide(inputPlate) == CabineoSide.Bottom)
            {
                offset = -offset;
            }
            return offset;
        }

        private double GetCabineoMountOffset(Corner corner, Plate inputPlate)
        {
            var offset = (inputPlate.Thickness / 2) - 5;
            if (inputPlate.IsHorizontal)
            {
                if (!CompartmentsExistLeft(corner))
                {
                    return offset;
                }
                if (!CompartmentsExistRight(corner))
                {
                    return -offset;
                }

                var centerX = (double)(_bookcase.LeftBound + _bookcase.RightBound) / 2;
                var correctedOffset = corner.x < centerX ? offset : -offset;
                return correctedOffset;
            }
            else
            {
                return corner.y == _bookcase.BottomBound ? offset : -offset;
            }
        }

        private bool CompartmentsExistRight(Corner corner)
        {
            foreach (var compartment in _bookcase.Compartments)
            {
                if (CompartmentCoversRight(compartment, corner))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CompartmentsExistLeft(Corner corner)
        {
            foreach (var compartment in _bookcase.Compartments)
            {
                if (CompartmentCoversLeft(compartment, corner))
                {
                    return true;
                }
            }
            return false;
        }

        private void AddSocketsToPlate(BlueprintPlate plate, Plate inputPlate)
        {
            if (inputPlate.InnerPlateStart)
            {
                AddSocketsToPlateEnd(plate, PlateEnd.Beginning, true, inputPlate);
            }

            if (inputPlate.InnerPlateEnd)
            {
                AddSocketsToPlateEnd(plate, PlateEnd.End, true, inputPlate);
            }
        }

        private void AddSocketsToPlateEnd(BlueprintPlate plate, PlateEnd plateEnd, bool shouldShiftPosition, Plate inputPlate)
        {
            var x = plateEnd == PlateEnd.Beginning ? -(double)plate.Length / 2 : (double)plate.Length / 2;
            var y = shouldShiftPosition ?
                plateEnd == PlateEnd.Beginning ?
                    CenterOffset(inputPlate) + _centerOffsetShift :
                    CenterOffset(inputPlate) - _centerOffsetShift
                : CenterOffset(inputPlate);
            var direction = plateEnd == PlateEnd.Beginning ? Direction.Increasing : Direction.Decreasing;

            plate.Sockets.Add(new BlueprintSocket()
            {
                X = x,
                Y = inputPlate.ParentCompartment != null ? y + _bookcase.DoorPlateThickness / 2 : y,
                Direction = direction,
                Rotate = false,
                FlushCap = false
            });
            plate.Sockets.Add(new BlueprintSocket()
            {
                X = x,
                Y = inputPlate.ParentCompartment != null ? -y + _bookcase.DoorPlateThickness / 2 : -y,
                Direction = direction,
                Rotate = false,
                FlushCap = false
            });
        }
    }
}
