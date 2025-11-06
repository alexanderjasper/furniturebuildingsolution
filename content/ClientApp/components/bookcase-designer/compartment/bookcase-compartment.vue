<template>
  <span>
    <compartment-buttons
      v-if="editable && showControls"
      :isEditable="isEditable"
      :compartment="compartment"
      :bookcase="bookcaseData"
      :camera="camera"
      :showButtons="showButtons"
    ></compartment-buttons>
    <compartment-door
      v-if="compartment.hasDoor"
      :compartment="compartment"
      :bookcase="bookcaseData"
      :compartmentSelected="isSelected"
    ></compartment-door>
    <!-- <compartment-drawer
      v-if="compartment.hasDrawer"
      :compartment="compartment"
      :bookcase="bookcaseData"
    ></compartment-drawer>-->
    <!-- <tv-model
      v-if="compartment.model"
      scene="scn"
      :name="'tv' + compartment.id"
      :maxWidth="compartment.dimensions.width"
      :maxHeight="compartment.dimensions.height"
      :maxDepth="plateDepth"
      :centerOfBase="centerOfBase"
      :bookcase="bookcaseData"
    ></tv-model>-->
    <bookcase-support
      v-if="showTopRearSupport"
      :start="compartment.corners.topLeft"
      :end="compartment.corners.topRight"
      zPlacement="rear"
      :id="compartment.id + 'rear'"
      :bookcase="bookcaseData"
    ></bookcase-support>
    <bookcase-support
      v-if="showTopFrontSupport"
      :start="compartment.corners.topLeft"
      :end="compartment.corners.topRight"
      zPlacement="front"
      :id="compartment.id + 'front'"
      :bookcase="bookcaseData"
    ></bookcase-support>
    <bookcase-plate-visualization
      v-if="compartment.visualizeWalls > 0 || compartment.visualizeShelves > 0"
      :bookcase="bookcaseData"
      :compartment="compartment"
    ></bookcase-plate-visualization>
    <!-- TODO: Only include template once -->
    <span v-if="isSelected">
      <drag-controls
        :cmr="camera"
        :plate="compartment.top"
        :position="topPosition"
        :horizontal="compartment.top.isHorizontal"
        :bookcase="bookcaseData"
        :editable="editable"
        :showMeasurements="false"
        :showDeleteButton="false"
        @draggingPlate="draggingPlate"
        @stoppedDraggingPlate="stoppedDraggingPlate"
      ></drag-controls>
      <drag-controls
        :cmr="camera"
        :plate="compartment.bottom"
        :position="bottomPosition"
        :horizontal="compartment.bottom.isHorizontal"
        :bookcase="bookcaseData"
        :editable="editable"
        :showMeasurements="false"
        :showDeleteButton="false"
        @draggingPlate="draggingPlate"
        @stoppedDraggingPlate="stoppedDraggingPlate"
      ></drag-controls>
      <drag-controls
        :cmr="camera"
        :plate="compartment.left"
        :position="leftPosition"
        :horizontal="compartment.left.isHorizontal"
        :bookcase="bookcaseData"
        :editable="editable"
        :showMeasurements="false"
        :showDeleteButton="false"
        @draggingPlate="draggingPlate"
        @stoppedDraggingPlate="stoppedDraggingPlate"
      ></drag-controls>
      <drag-controls
        :cmr="camera"
        :plate="compartment.right"
        :position="rightPosition"
        :horizontal="compartment.right.isHorizontal"
        :bookcase="bookcaseData"
        :editable="editable"
        :showMeasurements="false"
        :showDeleteButton="false"
        @draggingPlate="draggingPlate"
        @stoppedDraggingPlate="stoppedDraggingPlate"
      ></drag-controls>

      <compartment-measurements
        :key="'compartmentWidthMeasurement' + compartment.id"
        :id="compartment.id"
        measurementType="width"
        :measurement="compartment.dimensions.width - plateWidth"
        :isBefore="false"
        :position="widthPosition"
        :bookcase="bookcaseData"
      ></compartment-measurements>

      <compartment-measurements
        :key="'compartmentHeightMeasurement' + compartment.id"
        :id="compartment.id"
        measurementType="height"
        :measurement="compartment.dimensions.height - plateWidth"
        :isBefore="true"
        :position="heightPosition"
        :bookcase="bookcaseData"
      ></compartment-measurements>

      <compartment-measurements
        :key="'compartmentInnerPlateMeasurement' + measurement.id"
        v-for="measurement in innerPlateMeasurements"
        :id="measurement.id"
        measurementType="height"
        :measurement="measurement.measurement"
        :isBefore="true"
        :position="measurement.position"
        :bookcase="bookcaseData"
      ></compartment-measurements>
    </span>
  </span>
</template>
<script>
import compartmentButtons from "./compartment-buttons";
// import compartmentDrawer from "./compartment-drawer";
import compartmentDoor from "./compartment-door";
import bookcaseSupport from "./bookcase-support";
import bookcasePlateVisualization from "../bookcase-plate-visualization";
// import tvModel from "../models/tv-model";
import dataTools from "../tools/data-tools";
import dragControls from "../controls/drag-controls";
import compartmentMeasurements from "../compartment/compartment-measurements";
export default {
  name: "bookcase-compartment",
  inject: ["vglNamespace"],
  components: {
    "compartment-buttons": compartmentButtons,
    // "compartment-drawer": compartmentDrawer,
    "compartment-door": compartmentDoor,
    // "tv-model": tvModel,
    "bookcase-support": bookcaseSupport,
    "bookcase-plate-visualization": bookcasePlateVisualization,
    "drag-controls": dragControls,
    "compartment-measurements": compartmentMeasurements
  },
  mixins: [dataTools],
  props: {
    compartment: Object,
    bookcaseData: Object,
    camera: String,
    showButtons: Boolean,
    editable: Boolean,
    showControls: Boolean,
    showCompartmentButtons: Boolean,
    isSelected: Boolean
  },
  computed: {
    plateWidth: function() {
      return this.bookcaseData.material.thickness;
    },
    plateDepth: function() {
      return this.bookcaseData.plateDepth;
    },
    isEditable: function() {
      return (
        this.compartment.dimensions.width >= 200 &&
        this.compartment.dimensions.height >= 200
      );
    },
    centerOfBase: function() {
      let bounds = this.compartment.bounds;
      return {
        x: this.scaledNumber((bounds.right + bounds.left) / 2),
        y: this.scaledNumber(this.compartment.bottom.start.y + this.plateWidth),
        z: 0
      };
    },
    topPosition: function() {
      return {
        x: this.scaledNumber(this.compartment.center.x),
        y: this.scaledNumber(this.compartment.bounds.upper),
        z: 0
      };
    },
    bottomPosition: function() {
      return {
        x: this.scaledNumber(this.compartment.center.x),
        y: this.scaledNumber(this.compartment.bounds.lower),
        z: 0
      };
    },
    leftPosition: function() {
      return {
        x: this.scaledNumber(this.compartment.bounds.left),
        y: this.scaledNumber(this.compartment.center.y),
        z: 0
      };
    },
    rightPosition: function() {
      return {
        x: this.scaledNumber(this.compartment.bounds.right),
        y: this.scaledNumber(this.compartment.center.y),
        z: 0
      };
    },
    widthPosition: function() {
      return {
        x: this.scaledNumber(this.compartment.center.x - 55),
        y: this.scaledNumber(this.compartment.bounds.upper + 80),
        z: this.scaledNumber(this.bookcaseData.plateDepth)
      };
    },
    heightPosition: function() {
      return {
        x: this.scaledNumber(this.compartment.bounds.left - 210),
        y: this.scaledNumber(this.compartment.center.y - 65),
        z: this.scaledNumber(this.bookcaseData.plateDepth)
      };
    },
    showTopRearSupport: function() {
      return (
        this.compartment.dimensions.width >
        this.bookcaseData.maxLengthWithoutSupport
      );
    },
    showTopFrontSupport: function() {
      //TODO: Hardcoded value
      return this.compartment.dimensions.width > 817;
    },
    innerPlateMeasurements: function() {
      var measurementPlates = this.bookcaseData.plates
        .filter(
          plate =>
            plate.parentCompartment === this.compartment ||
            plate === this.compartment.top
        )
        .sort(function(a, b) {
          if (a.start.y > b.start.y) {
            return 1;
          }
          if (b.start.y > a.start.y) {
            return -1;
          }
          return 0;
        });

      var measurements = [];
      if (measurementPlates.length < 2) {
        return measurements;
      }

      var previousBottomY = this.compartment.bounds.lower;
      var xPosition =
        this.compartment.doorPosition === 0
          ? this.compartment.center.x
          : this.compartment.center.x - 150;
      for (var i = 0; i < measurementPlates.length; i++) {
        measurements.push({
          id: measurementPlates[i].id,
          measurement:
            measurementPlates[i].start.y - previousBottomY - this.plateWidth,
          position: {
            x: this.scaledNumber(xPosition),
            y: this.scaledNumber(
              (measurementPlates[i].start.y + previousBottomY) / 2 - 68
            ),
            z: this.scaledNumber(this.plateDepth)
          }
        });
        previousBottomY = measurementPlates[i].start.y;
      }
      return measurements;
    }
  },
  methods: {
    draggingPlate: function() {
      this.$emit("draggingPlate");
    },
    stoppedDraggingPlate: function(coordinateChanged) {
      this.$emit("stoppedDraggingPlate", coordinateChanged);
    }
  }
};
</script>
