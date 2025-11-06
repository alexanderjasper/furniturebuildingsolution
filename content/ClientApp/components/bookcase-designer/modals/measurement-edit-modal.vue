<template>
  <div class="custom-modal">
    <div class="modal-content" style="max-width: 500px" v-on:keyup.enter="submit()">
      <h3>Indstil {{editTypeDisplayText}}</h3>
      <div class="row">
        <div class="col-2">
          <input
            type="number"
            v-model.number="selectedMeasurement"
            :min="minMeasurement"
            :max="maxMeasurement"
            @blur="correctMeasurement"
            step="0.1"
            lang="da-DK"
            v-focus
          />
        </div>
        <div class="col-10">
          <div style="padding: 0 10px">
            <input
              type="range"
              :min="minMeasurement"
              :max="maxMeasurement"
              v-model="selectedMeasurement"
              class="slider"
              id="shelvesSlider"
              step="0.1"
            />
            <div class="sliderticks">
              <p>{{minMeasurement}}&nbsp;cm</p>
              <p>{{maxMeasurement}}&nbsp;cm</p>
            </div>
          </div>
        </div>
      </div>
      <p class="button-container">
        <button v-on:click="close()" class="btn btn-secondary">Annuller</button>
        <button v-on:click="submit()" class="btn" :disabled="!submittable">Vælg</button>
      </p>
    </div>
  </div>
</template>
<script>
import dataTools from "../tools/data-tools";
export default {
  name: "measurement-edit-modal",
  mixins: [dataTools],
  props: {
    bookcase: Object,
    compartment: Object,
    plate: Object
  },
  data() {
    return {
      selectedMeasurement: null
    };
  },
  computed: {
    plateWidth: function() {
      return this.bookcase.material.thickness;
    },
    submittable: function() {
      return (
        this.selectedMeasurement <= this.maxMeasurement &&
        this.selectedMeasurement >= this.minMeasurement
      );
    },
    editTypeText: function() {
      if (this.plateType === "left" || this.plateType === "right") {
        return "width";
      } else {
        return "height";
      }
    },
    editTypeDisplayText: function() {
      if (this.editTypeText === "width") {
        return "bredde";
      } else {
        return "højde";
      }
    },
    plateType: function() {
      if (this.plate === this.compartment.top) {
        return "top";
      } else if (this.plate === this.compartment.bottom) {
        return "bottom";
      } else if (this.plate === this.compartment.left) {
        return "left";
      } else if (this.plate === this.compartment.right) {
        return "right";
      }
    },
    minMeasurement: function() {
      var difference = null;
      if (this.plateType === "top") {
        var lowerBound = this.bookcase.getLowerBound(this.plate);
        difference = lowerBound - this.compartment.bounds.lower;
      } else if (this.plateType === "bottom") {
        var upperBound = this.bookcase.getUpperBound(this.plate);
        difference = this.compartment.bounds.upper - upperBound;
      } else if (this.plateType === "left") {
        var rightBound = this.bookcase.getRightBound(this.plate);
        difference = this.compartment.bounds.right - rightBound;
      } else if (this.plateType === "right") {
        var leftBound = this.bookcase.getLeftBound(this.plate);
        difference = leftBound - this.compartment.bounds.left;
      }
      return (difference - this.plateWidth) / 10;
    },
    maxMeasurement: function() {
      var difference = null;
      if (this.plateType === "top") {
        var upperBound = this.bookcase.getUpperBound(this.plate);
        difference = upperBound - this.compartment.bounds.lower;
      } else if (this.plateType === "bottom") {
        var lowerBound = this.bookcase.getLowerBound(this.plate);
        difference = this.compartment.bounds.upper - lowerBound;
      } else if (this.plateType === "left") {
        var leftBound = this.bookcase.getLeftBound(this.plate);
        difference = this.compartment.bounds.right - leftBound;
      } else if (this.plateType === "right") {
        var rightBound = this.bookcase.getRightBound(this.plate);
        difference = rightBound - this.compartment.bounds.left;
      }
      return (difference - this.plateWidth) / 10;
    }
  },
  mounted() {
    if (this.editTypeText === "width") {
      this.selectedMeasurement =
        (this.compartment.dimensions.width - this.plateWidth) / 10;
    } else {
      this.selectedMeasurement =
        (this.compartment.dimensions.height - this.plateWidth) / 10;
    }
  },
  methods: {
    correctMeasurement: function() {
      var measurement = this.selectedMeasurement;
      if (measurement < this.minMeasurement) {
        this.selectedMeasurement = this.minMeasurement;
      } else if (measurement > this.maxMeasurement) {
        this.selectedMeasurement = this.maxMeasurement;
      } else {
        this.selectedMeasurement = Math.floor(measurement * 10) / 10;
      }
    },
    close() {
      this.$emit("close");
    },
    submit() {
      var millimeterMeasurement = this.selectedMeasurement * 10;
      if (this.plateType === "top") {
        var newY =
          this.compartment.bounds.lower +
          this.plateWidth +
          millimeterMeasurement;
        this.bookcase.movePlate(this.compartment.top, newY, true);
      } else if (this.plateType === "bottom") {
        var newY =
          this.compartment.bounds.upper -
          this.plateWidth -
          millimeterMeasurement;
        this.bookcase.movePlate(this.compartment.bottom, newY, true);
      } else if (this.plateType === "left") {
        var newX =
          this.compartment.bounds.right -
          this.plateWidth -
          millimeterMeasurement;
        this.bookcase.movePlate(this.compartment.left, newX, true);
      } else if (this.plateType === "right") {
        var newX =
          this.compartment.bounds.left +
          this.plateWidth +
          millimeterMeasurement;
        this.bookcase.movePlate(this.compartment.right, newX, true);
      }
      this.$emit("triggerHistoryVersion");
      this.close();
    }
  }
};
</script>
