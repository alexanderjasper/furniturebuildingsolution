class Entity {
  constructor(id) {
    this.id = id;
  }
}
class Material extends Entity {
  constructor(material) {
    super(material.id);
    this.name = material.name;
    this.thickness = material.thickness;
    this.sideTextureLocation = material.sideTextureLocation;
    this.surfaceTextureLocation = material.surfaceTextureLocation;
    this.pricePerSquareMeter = material.pricePerSquareMeter;
  }
}
class Corner extends Entity {
  constructor(id, x, y) {
    super(id);
    this.x = Math.round(x);
    this.y = Math.round(y);
  }
}
class Plate extends Entity {
  constructor(
    bookcase,
    id,
    startCorner,
    endCorner,
    innerPlateStart,
    innerPlateEnd,
    draggable,
    parentCompartment,
    specialDepth
  ) {
    super(id);
    this.start = startCorner;
    this.end = endCorner;
    this.innerPlateStart = innerPlateStart;
    this.innerPlateEnd = innerPlateEnd;
    this.draggable = draggable;
    this.intermediateCorners = [];
    this.isSelected = false;
    this.displacement = 0;
    this.bookcase = bookcase;
    this.parentCompartment = parentCompartment;
    this.specialDepth = specialDepth;
  }

  get getJson() {
    function replacer(key, value) {
      if (key == "bookcase") return undefined;
      else return value;
    }
    return JSON.stringify(this, replacer);
  }

  get isHorizontal() {
    return this.start.x !== this.end.x;
  }
  get allCorners() {
    return [this.start, ...this.intermediateCorners, this.end];
  }

  get thickness() {
    return this.bookcase.materialThickness;
  }

  get isBottomPlate() {
    if (this.isHorizontal && this.start.y === this.bookcase.borders.bottom) {
      return true;
    }
    return false;
  }

  get cabineoSide() {
    if (this.isHorizontal) {
      if (this.isHorizontal && this.start.y === this.bookcase.borders.bottom) {
        return "top";
      }
      return "bottom";
    } else {
      var compartmentsLeft = this.bookcase.compartments.filter(
        compartment =>
          compartment.right === this &&
          compartment.bottom.start.y === this.start.y
      );
      if (compartmentsLeft.length === 0) {
        return "right";
      }
      var compartmentsRight = this.bookcase.compartments.filter(
        compartment =>
          compartment.left === this &&
          compartment.bottom.start.y === this.start.y
      );
      if (compartmentsRight.length === 0) {
        return "left";
      }

      if (this.start.x < this.bookcase.center.x) {
        return "right";
      } else {
        return "left";
      }
    }
  }

  cornerIsIntermediateAndMissing(corner) {
    if (this.isHorizontal) {
      if (
        corner.y === this.start.y &&
        corner.x > this.start.x &&
        corner.x < this.end.x &&
        !this.intermediateCorners.includes(corner)
      ) {
        return true;
      } else {
        return false;
      }
    } else {
      if (
        corner.x === this.start.x &&
        corner.y > this.start.y &&
        corner.y < this.end.y &&
        !this.intermediateCorners.includes(corner)
      ) {
        return true;
      } else {
        return false;
      }
    }
  }

  get length() {
    var coordinateLength;
    if (this.isHorizontal) {
      coordinateLength = this.end.x - this.start.x;
    } else {
      coordinateLength = this.end.y - this.start.y;
    }
    var halfPlateWidth = this.thickness / 2;
    if (this.innerPlateStart) {
      coordinateLength -= halfPlateWidth;
    } else {
      coordinateLength += halfPlateWidth;
    }
    if (this.innerPlateEnd) {
      coordinateLength -= halfPlateWidth;
    } else {
      coordinateLength += halfPlateWidth;
    }
    return coordinateLength;
  }
}
class Compartment extends Entity {
  constructor(
    bookcase,
    id,
    topPlate,
    bottomPlate,
    leftPlate,
    rightPlate,
    hasDoor,
    hasDrawer,
    model,
    doorPosition,
    forceSupport,
    backPlatePosition
  ) {
    super(id);
    this.bookcase = bookcase;
    this.top = topPlate;
    this.bottom = bottomPlate;
    this.left = leftPlate;
    this.right = rightPlate;
    this.hasDoor = hasDoor;
    this.hasDrawer = hasDrawer;
    this.model = model;
    this.doorPosition = doorPosition;
    this.forceSupport = forceSupport;
    this.backPlatePosition = backPlatePosition;
    this.visualizeWalls = 0;
    this.visualizeShelves = 0;
  }

  get getJson() {
    function replacer(key, value) {
      if (key == "bookcase") return undefined;
      else return value;
    }
    return JSON.stringify(this, replacer);
  }

  get bounds() {
    return {
      left: Math.max(this.top.start.x, this.left.start.x),
      right: Math.min(this.top.end.x, this.right.start.x),
      lower: Math.max(this.left.start.y, this.bottom.start.y),
      upper: Math.min(this.left.end.y, this.top.start.y)
    };
  }
  get corners() {
    return {
      topLeft: this.bookcase.addGetCorner(this.bounds.left, this.bounds.upper),
      topRight: this.bookcase.addGetCorner(
        this.bounds.right,
        this.bounds.upper
      ),
      bottomLeft: this.bookcase.addGetCorner(
        this.bounds.left,
        this.bounds.lower
      ),
      bottomRight: this.bookcase.addGetCorner(
        this.bounds.right,
        this.bounds.lower
      )
    };
  }
  get dimensions() {
    return {
      height: this.bounds.upper - this.bounds.lower,
      width: this.bounds.right - this.bounds.left
    };
  }
  get center() {
    return {
      x: (this.bounds.left + this.bounds.right) / 2,
      y: (this.bounds.lower + this.bounds.upper) / 2
    };
  }
  get maxNumberOfWalls() {
    return Math.floor(
      this.dimensions.width / 117 - // TODO this.bookcase.minimumCompartmentSize
        1
    );
  }
  get maxNumberOfShelves() {
    for (var i = 0; i < this.bookcase.plates.length; i++) {
      if (this.bookcase.plates[i].parentCompartment === this) {
        return 0;
      }
    }
    return Math.floor(
      this.dimensions.height / 117 - // TODO this.bookcase.minimumCompartmentSize
        1
    );
  }
  addModel(modelName) {
    this.model = modelName;
  }
  removeModel() {
    this.model = null;
  }
  addDrawer() {
    this.hasDrawer = true;
  }
  removeDrawer() {
    this.hasDrawer = false;
  }
  addDoor() {
    this.hasDoor = true;
  }
  removeDoor() {
    this.hasDoor = false;
  }
}
class Bookcase extends Entity {
  constructor() {
    super(null);
    this.corners = [];
    this.plates = [];
    this.compartments = [];
    this.minimumCompartmentSize = 117;
    this.maxPlateLength = 2370;
    this.maxWidthWithDoor = 800;
    this.minHeightWithDoor = 200;
    this.pricePerPiece = 100;
    this.defaultStartingPrice = 500;
    this.extraPricePerSquareMeter = 260;
    this.material = null;
    this.supportHeight = 60;
    this.plateDepth = null;
    this.isWallSuspended = false;
    this.skirtingBoardDepth = null;
    this.skirtingBoardHeight = null;
    this.baseHeight = null;
    this.startingPrice = null;
    this.priceIndex = null;
    this.doorMaterial = 0;
    this.maxLengthWithoutSupport = null;
    this.salesPrice = null;
    this.lockedForEditing = false;
  }

  get getJson() {
    function replacer(key, value) {
      if (key == "bookcase") return undefined;
      else return value;
    }
    return JSON.stringify(this, replacer);
  }

  getById(array, id) {
    var objects = array.filter(function(item) {
      return item.id === id;
    });
    if (objects.length > 0) {
      return objects[0];
    }
    return null;
  }
  cornerById(id) {
    return this.getById(this.corners, id);
  }
  plateById(id) {
    return this.getById(this.plates, id);
  }
  compartmentById(id) {
    return this.getById(this.compartments, id);
  }

  getCorners(compartment) {
    var bounds = compartment.bounds;
    return {
      topLeft: this.addGetCorner(bounds.left, bounds.upper),
      topRight: this.addGetCorner(bounds.right, bounds.upper),
      bottomLeft: this.addGetCorner(bounds.left, bounds.lower),
      bottomRight: this.addGetCorner(bounds.right, bounds.lower)
    };
  }
  get borders() {
    var left = Infinity;
    var right = -Infinity;
    var top = -Infinity;
    var bottom = Infinity;
    for (var i = 0; i < this.corners.length; i++) {
      var corner = this.corners[i];
      left = Math.min(left, corner.x);
      right = Math.max(right, corner.x);
      bottom = Math.min(bottom, corner.y);
      top = Math.max(top, corner.y);
    }
    return { left: left, right: right, bottom: bottom, top: top };
  }
  get center() {
    var borders = this.borders;
    if (borders.bottom === Infinity) {
      return { x: 0, y: 500 };
    }
    return {
      x: (borders.left + borders.right) / 2,
      y: (borders.bottom + borders.top) / 2
    };
  }
  get width() {
    var borders = this.borders;
    if (borders.bottom === Infinity) {
      return 2500;
    }
    return borders.right - borders.left + this.material.thickness;
  }
  get height() {
    var borders = this.borders;
    if (borders.bottom === Infinity) {
      return 2500;
    }
    var baseHeight = this.baseHeight ? this.baseHeight : 0;
    return borders.top - borders.bottom + this.material.thickness + baseHeight;
  }
  get materialThickness() {
    return this.material.thickness;
  }

  get price() {
    var price = this.startingPrice
      ? this.startingPrice
      : this.defaultStartingPrice;
    for (var i = 0; i < this.plates.length; i++) {
      price += this.platePrice(this.plates[i]);
    }
    for (var i = 0; i < this.compartments.length; i++) {
      price += this.supportAndDoorPrice(this.compartments[i]);
    }
    if (this.isWallSuspended) {
      price *= 1.3;
    } else if (this.baseHeight) {
      price += this.basePrice;
    }
    if (this.skirtingBoardDepth && this.skirtingBoardHeight) {
      price *= 1.08;
    }
    if (this.priceIndex) {
      price *= this.priceIndex;
    }
    return Math.round(price * 1.25);
  }

  get basePrice() {
    if (!this.baseHeight) {
      return 0;
    }
    var numberOfBottomPlates = 0;
    var totalBaseLength = 0;
    for (var i = 0; i < this.plates.length; i++) {
      if (this.plates[i].start.y == 0 && this.plates[i].end.y == 0) {
        numberOfBottomPlates += 1;
        totalBaseLength += this.plates[i].length;
      }
    }
    return (
      400 +
      this.pricePerPiece * numberOfBottomPlates +
      (this.baseHeight * totalBaseLength * this.platePricePerSquareMeter) /
        1000000
    );
  }

  get platePricePerSquareMeter() {
    return (
      1.3 * this.material.pricePerSquareMeter + this.extraPricePerSquareMeter
    );
  }
  get platePricePerMillimeter() {
    return (this.platePricePerSquareMeter * this.plateDepth) / 1000000;
  }
  get supportPricePerMillimeter() {
    return (this.platePricePerSquareMeter * this.supportHeight) / 1000000;
  }

  platePrice(plate) {
    var price = 0;

    if (plate.parentCompartment) {
      price += this.supportPrice(plate.parentCompartment.dimensions.width);
    }
    price += this.pricePerPiece + plate.length * this.platePricePerMillimeter;

    return price;
  }
  supportAndDoorPrice(compartment) {
    var price = 0;
    var dimensions = compartment.dimensions;

    price += this.supportPrice(dimensions.width);

    if (compartment.hasDoor) {
      price += this.doorPrice(dimensions);
    }
    return price;
  }
  supportPrice(width) {
    var price = 0;
    if (width > this.maxLengthWithoutSupport) {
      var extraSupportPrice =
        this.pricePerPiece +
        (width - this.material.thickness) * this.supportPricePerMillimeter;
      price += extraSupportPrice;
      if (width > this.maxLengthWithoutSupport + 500) {
        //TODO: Define this value centrally. Also used in bookcase-support.vue.
        price += extraSupportPrice;
      }
    }
    return price;
  }
  doorPrice(dimensions) {
    //TODO: Store door material in DB
    var areaSquareMeters = (dimensions.width * dimensions.height) / 1000000;
    //TODO: Calculated square meter price
    var squareMeterPrice, pricePerDoor;
    squareMeterPrice = 350 * 1.3 + this.extraPricePerSquareMeter;
    pricePerDoor = 200;
    return areaSquareMeters * squareMeterPrice + pricePerDoor;
  }

  removeFromArray(item, array) {
    var index = array.indexOf(item);
    if (index !== -1) {
      array.splice(index, 1);
    }
  }
  removeCompartment(compartment) {
    this.removeFromArray(compartment, this.compartments);
  }
  removePlate(plate) {
    this.removeFromArray(plate, this.plates);
  }
  removeCorner(corner) {
    this.removeFromArray(corner, this.corners);
    for (var i = 0; i < this.plates.length; i++) {
      this.removeFromArray(corner, this.plates[i].intermediateCorners);
    }
  }

  splitCompartment(id, splitType, divisionsString) {
    var divisions = parseInt(divisionsString);
    var compartment = this.compartmentById(id);
    var corners = this.getCorners(compartment);
    var returnCompartments = [];

    if (splitType === "horizontal") {
      var newBottomCorners = this.splitPlate(
        compartment.bottom,
        divisions,
        compartment.bounds,
        corners.bottomLeft
      );
      var newTopCorners = this.splitPlate(
        compartment.top,
        divisions,
        compartment.bounds,
        corners.topLeft
      );

      var previousLeft = compartment.left;
      var newPlate;
      for (var i = 0; i < newBottomCorners.length; i++) {
        newPlate = this.addGetPlate(
          null,
          newBottomCorners[i],
          newTopCorners[i],
          true,
          true,
          true,
          null,
          null
        );
        returnCompartments.push(
          this.addGetCompartment(
            null,
            compartment.top,
            compartment.bottom,
            previousLeft,
            newPlate,
            false,
            false,
            null,
            0,
            0,
            null
          )
        );
        previousLeft = newPlate;
      }

      returnCompartments.push(
        this.addGetCompartment(
          null,
          compartment.top,
          compartment.bottom,
          newPlate,
          compartment.right,
          false,
          false,
          null,
          0,
          0,
          null
        )
      );

      this.removeCompartment(compartment);
    } else if (splitType === "vertical") {
      var newLeftCorners = this.splitPlate(
        compartment.left,
        divisions,
        compartment.bounds,
        corners.bottomLeft
      );
      var newRightCorners = this.splitPlate(
        compartment.right,
        divisions,
        compartment.bounds,
        corners.bottomRight
      );

      var previousBottom = compartment.bottom;
      var newPlate = null;
      for (var i = 0; i < newLeftCorners.length; i++) {
        newPlate = this.addGetPlate(
          null,
          newLeftCorners[i],
          newRightCorners[i],
          true,
          true,
          true,
          null,
          null
        );
        if (compartment.hasDoor) {
          newPlate.parentCompartment = compartment;
        } else {
          returnCompartments.push(
            this.addGetCompartment(
              null,
              newPlate,
              previousBottom,
              compartment.left,
              compartment.right,
              false,
              false,
              null,
              0,
              0,
              null
            )
          );
        }
        previousBottom = newPlate;
      }

      if (!compartment.hasDoor) {
        returnCompartments.push(
          this.addGetCompartment(
            null,
            compartment.top,
            newPlate,
            compartment.left,
            compartment.right,
            false,
            false,
            null,
            0,
            0,
            null
          )
        );
        this.removeCompartment(compartment);
      }
    } else {
      alert("split type " + splitType + " not recognized");
    }
    return returnCompartments;
  }
  splitPlate(plate, divisions, bounds, firstSplitCorner) {
    var xDistance = plate.isHorizontal
      ? (bounds.right - bounds.left) / (divisions + 1)
      : 0;
    var yDistance = plate.isHorizontal
      ? 0
      : (bounds.upper - bounds.lower) / (divisions + 1);
    var newCorners = [];

    var previousCorner = firstSplitCorner;
    for (var i = 0; i < divisions; i++) {
      var newPoint = {
        x: previousCorner.x + xDistance,
        y: previousCorner.y + yDistance
      };
      var newCorner = this.addGetCorner(newPoint.x, newPoint.y);
      newCorners.push(newCorner);
      if (plate.intermediateCorners.indexOf(newCorner) === -1) {
        plate.intermediateCorners.push(newCorner);
      }
      previousCorner = newCorner;
    }

    return newCorners;
  }

  addGetCorner(x, y) {
    var corners = this.cornersWithCoordinates(x, y);
    if (corners.length > 0) {
      return corners[0];
    } else {
      var newCorner = new Corner(this.nextId(this.corners), x, y);
      this.corners.push(newCorner);
      return newCorner;
    }
  }
  addNewPlate(
    startX,
    startY,
    endX,
    endY,
    innerPlateStart,
    innerPlateEnd,
    draggable,
    specialDepth
  ) {
    var start = this.addGetCorner(startX, startY);
    var end = this.addGetCorner(endX, endY);
    this.addGetPlate(
      null,
      start,
      end,
      innerPlateStart,
      innerPlateEnd,
      draggable,
      null,
      specialDepth
    );
  }

  addGetPlate(
    id,
    startCorner,
    endCorner,
    innerPlateStart,
    innerPlateEnd,
    draggable,
    parentCompartment,
    specialDepth
  ) {
    if (!id) {
      id = this.nextId(this.plates);
    }
    var plates = this.platesWithCorners(startCorner, endCorner);
    if (plates.length > 0) {
      return plates[0];
    } else {
      var newPlate = new Plate(
        this,
        id,
        startCorner,
        endCorner,
        innerPlateStart,
        innerPlateEnd,
        draggable,
        parentCompartment,
        specialDepth
      );
      this.plates.push(newPlate);
      return newPlate;
    }
  }
  addGetCompartment(
    id,
    topPlate,
    bottomPlate,
    leftPlate,
    rightPlate,
    hasDoor,
    hasDrawer,
    model,
    doorPosition,
    forceSupport,
    backPlatePosition
  ) {
    if (!id) {
      id = this.nextId(this.compartments);
    }
    var compartments = this.compartmentsWithPlates(
      topPlate,
      bottomPlate,
      leftPlate,
      rightPlate
    );
    if (compartments.length > 0) {
      return compartments[0];
    } else {
      var newCompartment = new Compartment(
        this,
        id,
        topPlate,
        bottomPlate,
        leftPlate,
        rightPlate,
        hasDoor,
        hasDrawer,
        model,
        doorPosition,
        forceSupport,
        backPlatePosition
      );
      this.compartments.push(newCompartment);
      return newCompartment;
    }
  }
  cornersWithCoordinates(x, y) {
    return this.corners.filter(corner => corner.x === x && corner.y === y);
  }
  platesWithCorners(startCorner, endCorner) {
    return this.plates.filter(
      plate => plate.start === startCorner && plate.end === endCorner
    );
  }
  compartmentsWithPlates(topPlate, bottomPlate, leftPlate, rightPlate) {
    return this.compartments.filter(
      compartment =>
        compartment.top === topPlate &&
        compartment.bottom === bottomPlate &&
        compartment.left === leftPlate &&
        compartment.right === rightPlate
    );
  }
  nextId(array) {
    var maxId = Math.max(
      1,
      Math.max.apply(
        Math,
        array.map(function(element) {
          return element.id;
        })
      )
    );
    return maxId + 1;
  }
  isDeleteable(plate) {
    if (plate.intermediateCorners.length > 0) {
      return false;
    }
    if (plate.parentCompartment) {
      return true;
    }
    if (plate.isHorizontal) {
      var compartmentsContainingPlate = this.compartments.filter(
        compartment => compartment.top === plate || compartment.bottom === plate
      );
      if (compartmentsContainingPlate.length === 2) {
        if (
          compartmentsContainingPlate[0].left !==
            compartmentsContainingPlate[1].left ||
          compartmentsContainingPlate[0].right !==
            compartmentsContainingPlate[1].right
        ) {
          return false;
        }
      } else {
        return false;
      }
    } else {
      var compartmentsContainingPlate = this.compartments.filter(
        compartment => compartment.left === plate || compartment.right === plate
      );
      if (compartmentsContainingPlate.length === 2) {
        if (
          compartmentsContainingPlate[0].bottom !==
            compartmentsContainingPlate[1].bottom ||
          compartmentsContainingPlate[0].top !==
            compartmentsContainingPlate[1].top
        ) {
          return false;
        }
      } else {
        return false;
      }
    }
    return true;
  }
  compartmentsBefore(plate) {
    if (plate.isHorizontal) {
      var compartments = this.compartments.filter(
        compartment => compartment.top === plate
      );
      return compartments.length > 0 ? compartments : null;
    } else {
      var compartments = this.compartments.filter(
        compartment => compartment.right === plate
      );
      return compartments.length > 0 ? compartments : null;
    }
  }
  compartmentsAfter(plate) {
    if (plate.isHorizontal) {
      var compartments = this.compartments.filter(
        compartment => compartment.bottom === plate
      );
      return compartments.length > 0 ? compartments : null;
    } else {
      var compartments = this.compartments.filter(
        compartment => compartment.left === plate
      );
      return compartments.length > 0 ? compartments : null;
    }
  }
  deletePlate(plate) {
    if (!this.isDeleteable(plate)) {
      console.log("Error: tried to delete non-deleteable plate.");
      return;
    }
    if (!plate.parentCompartment) {
      if (plate.isHorizontal) {
        var compartmentAbove = this.compartmentsAfter(plate)[0];
        var compartmentBelow = this.compartmentsBefore(plate)[0];
        compartmentAbove.bottom = compartmentBelow.bottom;
        this.removeCompartment(compartmentBelow);
      } else {
        var compartmentRight = this.compartmentsAfter(plate)[0];
        var compartmentLeft = this.compartmentsBefore(plate)[0];
        compartmentRight.left = compartmentLeft.left;
        this.removeCompartment(compartmentLeft);
      }
    }
    this.removePlate(plate);
    this.removeCornersIfUnused([plate.start, plate.end]);
  }
  platesBeforeMin(plate) {
    var platesBefore = this.plates.filter(function(bookcasePlate) {
      return (
        bookcasePlate.isHorizontal != plate.isHorizontal &&
        plate.allCorners.includes(bookcasePlate.end)
      );
    });
    if (platesBefore.length === 0) {
      return Infinity;
    }
    if (plate.isHorizontal) {
      return Math.min(
        ...platesBefore.map(function(plateBefore) {
          return plateBefore.start.y;
        })
      );
    } else {
      return Math.min(
        ...platesBefore.map(function(plateBefore) {
          return plateBefore.start.x;
        })
      );
    }
  }
  platesAfterMax(plate) {
    var platesAfter = this.plates.filter(function(bookcasePlate) {
      return (
        bookcasePlate.isHorizontal != plate.isHorizontal &&
        plate.allCorners.includes(bookcasePlate.start)
      );
    });
    if (platesAfter.length === 0) {
      return -Infinity;
    }
    if (plate.isHorizontal) {
      return Math.max(
        ...platesAfter.map(function(plateAfter) {
          return plateAfter.end.y;
        })
      );
    } else {
      return Math.min(
        ...platesAfter.map(function(plateAfter) {
          return plateAfter.end.x;
        })
      );
    }
  }
  getRightBound(plate) {
    var bound = Math.min(
      3745,
      this.platesBeforeMin(plate) +
        this.maxPlateLength -
        this.material.thickness
    );
    for (var i = 0; i < this.compartments.length; i++) {
      var compartment = this.compartments[i];
      if (compartment.left === plate) {
        bound = Math.min(
          bound,
          compartment.right.start.x - this.minimumCompartmentSize
        );
      }
      if (compartment.right === plate && compartment.hasDoor) {
        bound = Math.min(
          bound,
          compartment.left.start.x +
            this.maxWidthWithDoor +
            this.material.thickness
        );
      }
    }
    return bound;
  }
  getLeftBound(plate) {
    var bound = Math.max(
      -3745,
      this.platesAfterMax(plate) - this.maxPlateLength + this.material.thickness
    );
    for (var i = 0; i < this.compartments.length; i++) {
      var compartment = this.compartments[i];
      if (compartment.right === plate) {
        bound = Math.max(
          bound,
          compartment.left.start.x + this.minimumCompartmentSize
        );
      }
      if (compartment.left === plate && compartment.hasDoor) {
        bound = Math.max(
          bound,
          compartment.right.start.x -
            this.maxWidthWithDoor -
            this.material.thickness
        );
      }
    }
    return bound;
  }
  getUpperBound(horizontalPlate) {
    var platesAbove = this.plates.filter(function(otherPlate) {
      return (
        otherPlate.isHorizontal &&
        otherPlate.start.y > horizontalPlate.start.y &&
        otherPlate.start.x < horizontalPlate.end.x &&
        otherPlate.end.x > horizontalPlate.start.x
      );
    });

    var minOfPlatesAbove = Math.min.apply(
      null,
      platesAbove.map(plate => plate.start.y)
    );

    var doorMax = Infinity;
    for (var i = 0; i < this.compartments.length; i++) {
      var compartment = this.compartments[i];
      if (compartment.hasDoor && compartment.bottom === horizontalPlate) {
        doorMax =
          compartment.top.start.y -
          this.minHeightWithDoor -
          this.material.thickness;
      }
    }

    //TODO: hardcoded values
    return Math.min(
      2985,
      minOfPlatesAbove - this.minimumCompartmentSize,
      this.platesBeforeMin(horizontalPlate) +
        this.maxPlateLength -
        this.material.thickness,
      doorMax
    );
  }
  getLowerBound(horizontalPlate) {
    var platesBelow = this.plates.filter(function(otherPlate) {
      return (
        otherPlate.isHorizontal &&
        otherPlate.start.y < horizontalPlate.start.y &&
        otherPlate.start.x < horizontalPlate.end.x &&
        otherPlate.end.x > horizontalPlate.start.x
      );
    });

    var maxOfPlatesBelow = Math.max.apply(
      null,
      platesBelow.map(plate => plate.start.y)
    );

    var doorMin = -Infinity;
    for (var i = 0; i < this.compartments.length; i++) {
      var compartment = this.compartments[i];
      if (compartment.hasDoor && compartment.top === horizontalPlate) {
        doorMin =
          compartment.bottom.start.y +
          this.minHeightWithDoor +
          this.material.thickness;
      }
    }

    //TODO: hardcoded values
    return Math.max(
      0,
      maxOfPlatesBelow + this.minimumCompartmentSize,
      this.platesAfterMax(horizontalPlate) -
        this.maxPlateLength +
        this.material.thickness,
      doorMin
    );
  }
  snapOrAvoid(plate, newValue, overrideSnapping) {
    var snapThreshold = 20;
    let closeCornersStart = this.corners.filter(
      corner =>
        corner !== plate.start &&
        ((plate.isHorizontal & (corner.x === plate.start.x) &&
          Math.abs(corner.y - newValue) < snapThreshold) ||
          (!plate.isHorizontal &&
            corner.y === plate.start.y &&
            Math.abs(corner.x - newValue) < snapThreshold))
    );
    let closeCornersEnd = this.corners.filter(
      corner =>
        corner !== plate.end &&
        ((plate.isHorizontal & (corner.x === plate.end.x) &&
          Math.abs(corner.y - newValue) < snapThreshold) ||
          (!plate.isHorizontal &&
            corner.y === plate.end.y &&
            Math.abs(corner.x - newValue) < snapThreshold))
    );

    let closeValues = [];
    if (closeCornersStart.length > 0) {
      let closeCorner = closeCornersStart[0];
      if (
        this.plates.filter(
          otherPlate =>
            otherPlate.end === closeCorner &&
            (!otherPlate.innerPlateEnd || !plate.innerPlateStart) &&
            otherPlate.isHorizontal === plate.isHorizontal
        ).length > 0
      ) {
        closeValues.push(plate.isHorizontal ? closeCorner.y : closeCorner.x);
      }
    }
    if (closeCornersEnd.length > 0) {
      let closeCorner = closeCornersEnd[0];
      if (
        this.plates.filter(
          otherPlate =>
            otherPlate.start === closeCorner &&
            (!otherPlate.innerPlateStart || !plate.innerPlateEnd) &&
            otherPlate.isHorizontal === plate.isHorizontal
        ).length > 0
      ) {
        closeValues.push(plate.isHorizontal ? closeCorner.y : closeCorner.x);
      }
    }

    if (closeValues.length > 0) {
      let min = Math.min(...closeValues);
      let max = Math.max(...closeValues);
      let avg = 0;
      closeValues.forEach(value => (avg += value));

      let smallBound = plate.isHorizontal
        ? this.getLowerBound(plate)
        : this.getLeftBound(plate);
      let largeBound = plate.isHorizontal
        ? this.getUpperBound(plate)
        : this.getLowerBound(plate);

      if (avg < newValue) {
        if (largeBound >= max + snapThreshold) {
          return this.snapOrAvoid(plate, max + snapThreshold, overrideSnapping);
        } else {
          return this.snapOrAvoid(plate, min - snapThreshold, overrideSnapping);
        }
      } else {
        if (smallBound <= min - snapThreshold) {
          return this.snapOrAvoid(plate, min - snapThreshold, overrideSnapping);
        } else {
          return this.snapOrAvoid(plate, max + snapThreshold, overrideSnapping);
        }
      }
    }

    if (!overrideSnapping) {
      for (var i = 0; i < this.corners.length; i++) {
        var corner = this.corners[i];
        if (plate.isHorizontal) {
          if (
            (corner !== plate.start &&
              corner.x === plate.start.x &&
              Math.abs(corner.y - newValue) < snapThreshold) ||
            (corner !== plate.end &&
              corner.x === plate.end.x &&
              Math.abs(corner.y - newValue) < snapThreshold)
          ) {
            return corner.y;
          }
        } else {
          if (
            (corner !== plate.start &&
              corner.y === plate.start.y &&
              Math.abs(corner.x - newValue) < snapThreshold) ||
            (corner !== plate.end &&
              corner.y === plate.end.y &&
              Math.abs(corner.x - newValue) < snapThreshold)
          ) {
            return corner.x;
          }
        }
      }
    }
    return newValue;
  }
  movePlate(plate, newValue, overrideSnapping) {
    var isHorizontal = plate.isHorizontal;
    if (isHorizontal) {
      var lowerBound = this.getLowerBound(plate);
      var upperBound = this.getUpperBound(plate);
      newValue = Math.round(
        Math.min(Math.max(newValue, lowerBound), upperBound)
      );
    } else {
      let rightBound = this.getRightBound(plate);
      let leftBound = this.getLeftBound(plate);
      newValue = Math.round(
        Math.max(Math.min(newValue, rightBound), leftBound)
      );
    }

    var snappedValue = this.snapOrAvoid(plate, newValue, overrideSnapping);

    var startIsInUse = this.cornerIsInUse(plate.start, plate);
    var endIsInUse = this.cornerIsInUse(plate.end, plate);
    if (isHorizontal) {
      if (startIsInUse) {
        plate.start = this.addGetCorner(plate.start.x, snappedValue);
      } else {
        plate.start.y = snappedValue;
      }
      if (endIsInUse) {
        plate.end = this.addGetCorner(plate.end.x, snappedValue);
      } else {
        plate.end.y = snappedValue;
      }
      for (var i = 0; i < plate.intermediateCorners.length; i++) {
        plate.intermediateCorners[i].y = snappedValue;
      }
    } else {
      if (startIsInUse) {
        plate.start = this.addGetCorner(snappedValue, plate.start.y);
      } else {
        plate.start.x = snappedValue;
      }
      if (endIsInUse) {
        plate.end = this.addGetCorner(snappedValue, plate.end.y);
      } else {
        plate.end.x = snappedValue;
      }
      for (var i = 0; i < plate.intermediateCorners.length; i++) {
        plate.intermediateCorners[i].x = snappedValue;
      }
    }
    this.addintermediateCorners(); //TODO: This is highly inefficient. Should be changed.
    this.removeCornersIfUnused(this.corners); //TODO: This is highly inefficient. Should be changed.
  }
  removeCornersIfUnused(arrayOfCorners) {
    for (var i = 0; i < arrayOfCorners.length; i++) {
      if (!this.cornerIsInUse(arrayOfCorners[i])) {
        this.removeCorner(arrayOfCorners[i]);
      }
    }
  }
  cornerIsInUse(corner, plateToExclude = null) {
    for (var i = 0; i < this.plates.length; i++) {
      if (this.plates[i].start === corner || this.plates[i].end === corner) {
        if (
          !plateToExclude ||
          (plateToExclude !== this.plates[i] &&
            plateToExclude.isHorizontal === this.plates[i].isHorizontal)
        ) {
          return true;
        }
      }
    }
    return false;
  }
  populateDisplacements(displacements) {
    for (var i = 0; i < this.plates.length; i++) {
      var plate = this.plates[i];
      var displacement = displacements[plate.id];
      if (!!displacement) {
        plate.displacement = displacement;
      } else {
        plate.displacement = 0;
      }
    }
    var x = 1;
    return;
  }

  initialize(bookcaseData) {
    this.id = bookcaseData.id;
    this.name = bookcaseData.name;
    this.lockedForEditing = bookcaseData.lockedForEditing;
    this.material = new Material(bookcaseData.material);
    this.supportHeight = bookcaseData.supportHeight;
    this.plateDepth = bookcaseData.plateDepth;
    this.isWallSuspended = bookcaseData.isWallSuspended;
    this.skirtingBoardDepth = bookcaseData.skirtingBoardDepth;
    this.skirtingBoardHeight = bookcaseData.skirtingBoardHeight;
    this.baseHeight = bookcaseData.baseHeight;
    this.startingPrice = bookcaseData.startingPrice;
    this.priceIndex = bookcaseData.priceIndex;
    this.doorMaterial = bookcaseData.doorMaterial;
    this.maxLengthWithoutSupport = bookcaseData.maxLengthWithoutSupport;
    this.salesPrice = bookcaseData.salesPrice;

    for (var i = 0; i < bookcaseData.plates.length; i++) {
      this.addGetPlateFromData(bookcaseData.plates[i]);
    }
    for (var i = 0; i < bookcaseData.compartments.length; i++) {
      var top = this.plateById(bookcaseData.compartments[i].top.id);
      var bottom = this.plateById(bookcaseData.compartments[i].bottom.id);
      var left = this.plateById(bookcaseData.compartments[i].left.id);
      var right = this.plateById(bookcaseData.compartments[i].right.id);
      this.addGetCompartment(
        bookcaseData.compartments[i].id,
        top,
        bottom,
        left,
        right,
        bookcaseData.compartments[i].hasDoor,
        bookcaseData.compartments[i].hasDrawer,
        bookcaseData.compartments[i].model,
        bookcaseData.compartments[i].doorPosition,
        bookcaseData.compartments[i].forceSupport,
        bookcaseData.compartments[i].backPlatePosition
      );
    }
    for (var i = 0; i < this.plates.length; i++) {
      if (this.plates[i].parentCompartment) {
        this.plates[i].parentCompartment = this.compartmentById(
          this.plates[i].parentCompartment.id
        );
      }
    }

    this.addintermediateCorners();
  }

  addGetPlateFromData(plate) {
    this.addGetPlate(
      plate.id,
      this.addGetCorner(plate.start.x, plate.start.y),
      this.addGetCorner(plate.end.x, plate.end.y),
      plate.innerPlateStart,
      plate.innerPlateEnd,
      plate.draggable,
      plate.parentCompartment,
      plate.specialDepth
    );
  }

  addGetPlatesFromData(platesList) {
    var returnArray = [];
    for (var i = 0; i < platesList.length; i++) {
      returnArray.push(this.addGetPlateFromData(platesList[i]));
    }
    return returnArray;
  }

  addintermediateCorners() {
    for (var i = 0; i < this.plates.length; i++) {
      var plate = this.plates[i];
      for (var j = 0; j < this.corners.length; j++) {
        var corner = this.corners[j];

        if (plate.cornerIsIntermediateAndMissing(corner)) {
          plate.intermediateCorners.push(corner);
        }
      }
    }
  }
}
export default Bookcase;
