<template>
  <svg :viewBox="viewBox" v-if="initialized" :width="size + 'px'" :height="size + 'px'">
    <rect :width="size" :height="size" :fill="fillColor" :rx="size / 40" :ry="size / 40" />
    <line
      v-for="plate in bookcase.plates"
      :x1="plateX1(plate)"
      :y1="plateY1(plate)"
      :x2="plateX2(plate)"
      :y2="plateY2(plate)"
      :key="'plate' + plate.id"
      :stroke="getStrokeColor(plate)"
      :stroke-width="getStrokeWidth(plate)"
      v-on:click="selectPlate(plate.id)"
    />
    <rect
      v-for="compartment in compartments"
      :x="compartmentX(compartment)"
      :y="compartmentY(compartment)"
      :width="compartmentWidth(compartment)"
      :height="compartmentHeight(compartment)"
      :key="'compartment' + compartment.id"
      fill="grey"
    />
    <text
      v-for="plate in horizontalPlates"
      :x="(plateX2(plate)+plateX1(plate))/2"
      :y="plateY1(plate) -0.05*svgScale"
      text-anchor="middle"
      :key="'plateId' + plate.id"
      :id="'plateId' + plate.id"
      :stroke="'#2dac86'"
      :stroke-width="isHighlighted(plate) ? 0.4 : 0.2"
      v-on:click="selectPlate(plate.id)"
      :font-size="isHighlighted(plate) ? '8px' : '3.5px'"
    >{{plate.id}}</text>
    <text
      v-for="plate in verticalPlates"
      :x="plateX1(plate) - (0.05)*svgScale"
      :y="(plateY2(plate)+plateY1(plate))/2 + (isHighlighted(plate) ? 0.13 : 0.06) * svgScale"
      text-anchor="end"
      :key="'plateId' + plate.id"
      :id="'plateId' + plate.id"
      :stroke="'red'"
      :stroke-width="isHighlighted(plate) ? 0.4 : 0.2"
      v-on:click="selectPlate(plate.id)"
      :font-size="isHighlighted(plate) ? '8px' : '3.5px'"
    >{{plate.id}}</text>
    <rect
      v-if="selectedCompartment"
      :x="compartmentX(selectedCompartment)"
      :y="compartmentY(selectedCompartment)"
      :width="compartmentWidth(selectedCompartment)"
      :height="compartmentHeight(selectedCompartment)"
      :key="'compartment' + selectedCompartment.id"
      fill="#2dac86"
    />
    <text
      v-for="compartment in compartmentsToShow"
      :x="compartmentCenterX(compartment)"
      :y="compartmentCenterY(compartment)+ 0.05*svgScale"
      v-on:click="selectCompartment(compartment.id)"
      text-anchor="middle"
      :key="'compartmentId' + compartment.id"
      font-size="3.5px"
    >{{compartment.id}}</text>
    <use v-bind="{'xlink:href':'#plateId'+plateToHighlight}" />
  </svg>
</template>

<script>
import BookcaseClass from "../../../extensions/bookcaseClasses.js";
import dataTools from "../tools/data-tools";
export default {
  mixins: [dataTools],
  props: {
    size: Number,
    bookcaseData: Object,
    fillColor: String,
    plateToHighlight: Number,
    compartmentToHighlight: Number,
    margin: Number,
    dataToShow: String,
    isSpecifications: Boolean
  },
  data() {
    return {
      bounds: {
        left: Infinity,
        right: -Infinity,
        upper: -Infinity,
        lower: Infinity
      },
      width: 0,
      height: 0,
      bookcase: new BookcaseClass(null),
      initialized: false
    };
  },
  computed: {
    viewBox: function() {
      if (this.isSpecifications) {
        var cutoff = (1 - this.height / this.width) / (1 - this.margin + 0.2);
        if (cutoff > 0) {
          return (
            "0 " +
            (this.size * cutoff) / 2 +
            " " +
            this.size +
            " " +
            this.size * (1 - cutoff)
          );
        }
      }
      return "0 0 " + this.size + " " + this.size;
    },
    compartmentsToShow: function() {
      if (this.dataToShow === "compartments") {
        return this.bookcase.compartments;
      }
      return null;
    },
    selectedCompartment: function() {
      if (this.compartmentToHighlight && this.dataToShow == "compartments") {
        return this.bookcase.compartmentById(this.compartmentToHighlight);
      } else {
        return null;
      }
    },
    strokeWidth: function() {
      return this.dataToShow ? 1 : 2;
    },
    offset: function() {
      return (this.svgScale * this.margin * this.size) / 2;
    },
    svgScale: function() {
      return (
        Math.max(
          this.bounds.right - this.bounds.left,
          this.bounds.upper - this.bounds.lower
        ) /
        (this.size - this.size * this.margin)
      );
    },
    compartments: function() {
      return this.bookcase.compartments.filter(
        compartment => compartment.hasDoor || compartment.hasDrawer
      );
    },
    horizontalPlates: function() {
      return this.dataToShow == "plates"
        ? this.bookcase.plates.filter(plate => plate.isHorizontal)
        : null;
    },
    verticalPlates: function() {
      return this.dataToShow == "plates"
        ? this.bookcase.plates.filter(plate => !plate.isHorizontal)
        : null;
    }
  },
  methods: {
    isHighlighted: function(plate) {
      return this.plateToHighlight === plate.id;
    },
    selectPlate: function(plateId) {
      this.$emit("highlightPlate", plateId);
    },
    selectCompartment: function(compartmentId) {
      this.$emit("highlightCompartment", compartmentId);
    },
    plateX1: function(plate) {
      return (plate.start.x - this.bounds.left + this.offset) / this.svgScale;
    },
    plateY1: function(plate) {
      return (
        this.size -
        (plate.start.y - this.bounds.lower + this.offset) / this.svgScale
      );
    },
    plateX2: function(plate) {
      return (plate.end.x - this.bounds.left + this.offset) / this.svgScale;
    },
    plateY2: function(plate) {
      return (
        this.size -
        (plate.end.y - this.bounds.lower + this.offset) / this.svgScale
      );
    },
    compartmentCenterX: function(compartment) {
      return (
        (compartment.center.x - this.bounds.left + this.offset) / this.svgScale
      );
    },
    compartmentCenterY: function(compartment) {
      return (
        this.size -
        (compartment.center.y - this.bounds.lower + this.offset) / this.svgScale
      );
    },
    compartmentX: function(compartment) {
      return (
        (compartment.bounds.left - this.bounds.left + this.offset) /
          this.svgScale +
        this.strokeWidth / 2
      );
    },
    compartmentY: function(compartment) {
      return (
        this.size -
        (compartment.bounds.upper - this.bounds.lower + this.offset) /
          this.svgScale +
        this.strokeWidth / 2
      );
    },
    compartmentWidth: function(compartment) {
      return (
        (compartment.bounds.right - compartment.bounds.left) / this.svgScale -
        this.strokeWidth
      );
    },
    compartmentHeight: function(compartment) {
      return (
        (compartment.bounds.upper - compartment.bounds.lower) / this.svgScale -
        this.strokeWidth
      );
    },
    getStrokeColor: function(plate) {
      if (plate.id === this.plateToHighlight && this.dataToShow === "plates") {
        return "#2dac86";
      } else {
        return "black";
      }
    },
    getStrokeWidth: function(plate) {
      if (plate.id === this.plateToHighlight && this.dataToShow === "plates") {
        return this.strokeWidth * 2;
      } else {
        return this.strokeWidth;
      }
    }
  },
  mounted() {
    this.bookcase.initialize(this.bookcaseData);

    for (var i = 0; i < this.bookcase.corners.length; i++) {
      var corner = this.bookcase.corners[i];
      if (corner.x < this.bounds.left) {
        this.bounds.left = corner.x;
      }
      if (corner.x > this.bounds.right) {
        this.bounds.right = corner.x;
      }
      if (corner.y < this.bounds.lower) {
        this.bounds.lower = corner.y;
      }
      if (corner.y > this.bounds.upper) {
        this.bounds.upper = corner.y;
      }
    }

    this.width = this.bounds.right - this.bounds.left;
    this.height = this.bounds.upper - this.bounds.lower;
    let difference = Math.abs(this.width - this.height);
    if (this.width > this.height) {
      this.bounds.lower -= difference / 2;
      this.bounds.upper += difference / 2;
    } else {
      this.bounds.left -= difference / 2;
      this.bounds.right += difference / 2;
    }

    this.initialized = true;
  }
};
</script>
