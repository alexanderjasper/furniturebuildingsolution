<template>
  <span>
    <vgl-mesh
      geometry="measurementButtonGeometry"
      material="invisibleMaterial"
      :name="'0MeasurementButton' + id"
      :position="buttonPosition"
    ></vgl-mesh>
    <vgl-mesh
      :geometry="geometry"
      material="highlightMaterialBasic"
      :name="'0MeasurementButton' + id"
      :position="boxPosition"
      scale="0.0002 0.0002 0.0002"
    ></vgl-mesh>
    <vgl-mesh
      v-for="(digit, index) in digitsArray"
      :key="'topDigit' + index"
      :geometry="'number' + digit"
      material="whiteMaterial"
      :position="digitPosition(index)"
      :name="(index+1) + 'MeasurementButton' + id"
    ></vgl-mesh>
    <vgl-mesh
      geometry="commaText"
      material="whiteMaterial"
      :position="commaPosition"
      :name="'5MeasurementButton' + id"
    ></vgl-mesh>
  </span>
</template>
<script>
import * as VueGL from "vue-gl";
import dataTools from "../tools/data-tools";
export default {
  name: "compartment-measurements",
  mixins: [dataTools],
  components: {
    "vgl-mesh": VueGL.VglMesh
  },
  props: {
    id: Number,
    measurementType: String,
    measurement: Number,
    isBefore: Boolean,
    position: Object,
    bookcase: Object
  },
  data() {
    return {
      digitSpacing: 28,
      commaSpacing: 13,
      beforeSideHorizontalOffset: 12,
      beforeSideVerticalOffset: 32
    };
  },
  methods: {
    digitPosition: function(index) {
      var numberOfDigits = Math.round(this.measurement).toString().length;
      var fourDigitOffset = 0;
      var fourDigitCommaOffset = 0;
      if (numberOfDigits === 4) {
        fourDigitOffset = -17;
        fourDigitCommaOffset = 9;
      }
      // TODO: Make this much more elegant
      var digitPosition = 0;
      if (index === -1) {
        digitPosition =
          fourDigitCommaOffset +
          this.horizontalOffset +
          3 * this.digitSpacing -
          23;
      } else if (index === 0) {
        digitPosition = fourDigitOffset + this.horizontalOffset;
      } else if (index === 1) {
        digitPosition =
          fourDigitOffset + this.horizontalOffset + this.digitSpacing;
      } else if (index === 2) {
        digitPosition =
          fourDigitOffset + this.horizontalOffset + 2 * this.digitSpacing;
        if (numberOfDigits === 3) {
          digitPosition += this.commaSpacing;
        }
      } else {
        digitPosition =
          fourDigitOffset +
          this.horizontalOffset +
          3 * this.digitSpacing +
          this.commaSpacing;
      }

      var coordinate = {
        x: this.position.x + this.scaledNumber(digitPosition),
        y: this.position.y + this.scaledNumber(this.verticalOffset),
        z: this.position.z + this.scaledNumber(2)
      };
      return this.getVectorString(coordinate);
    }
  },
  computed: {
    geometry: function() {
      if (this.measurementType === "width") {
        return "widthBoxGeometry";
      } else {
        return "heightBoxGeometry";
      }
    },
    commaPosition: function() {
      return this.digitPosition(-1);
    },
    horizontalOffset: function() {
      if (this.measurementType === "width") {
        var offset = 20;
        if (this.isBefore) {
          return offset + this.beforeSideHorizontalOffset;
        } else {
          return offset - this.beforeSideHorizontalOffset;
        }
        return offset;
      } else {
        return 19;
      }
    },
    verticalOffset: function() {
      if (this.measurementType === "height") {
        var offset = 15;
        if (this.isBefore) {
          return offset + this.beforeSideVerticalOffset;
        } else {
          return offset - this.beforeSideVerticalOffset;
        }
      } else {
        return 16;
      }
    },
    buttonPosition: function() {
      return (
        this.position.x +
        " " +
        this.position.y +
        " " +
        (this.position.z + this.scaledNumber(3))
      );
    },
    boxPosition: function() {
      if (this.measurementType === "width") {
        if (this.isBefore) {
          return (
            this.position.x +
            this.scaledNumber(this.beforeSideHorizontalOffset) +
            " " +
            this.position.y +
            " " +
            (this.position.z + this.scaledNumber(1))
          );
        } else {
          return (
            this.position.x -
            this.scaledNumber(this.beforeSideHorizontalOffset) +
            " " +
            this.position.y +
            " " +
            (this.position.z + this.scaledNumber(1))
          );
        }
      } else {
        if (this.isBefore) {
          return (
            this.position.x +
            " " +
            (this.position.y +
              this.scaledNumber(this.beforeSideVerticalOffset)) +
            " " +
            (this.position.z + this.scaledNumber(1))
          );
        } else {
          return (
            this.position.x +
            " " +
            (this.position.y -
              this.scaledNumber(this.beforeSideVerticalOffset)) +
            " " +
            (this.position.z + this.scaledNumber(1))
          );
        }
      }
    },
    digitsArray: function() {
      var measurementArray = [];
      var number = Math.round(this.measurement);
      while (number) {
        measurementArray.push(number % 10);
        number = Math.floor(number / 10);
      }
      return measurementArray.reverse();
    }
  }
};
</script>
