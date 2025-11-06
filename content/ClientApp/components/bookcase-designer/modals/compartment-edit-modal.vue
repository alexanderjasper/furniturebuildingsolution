<script>
import dataTools from "../tools/data-tools";
export default {
  name: "compartment-edit-modal",
  mixins: [dataTools],
  props: {
    compartment: Object,
    type: String,
    bookcase: Object
  },
  data() {
    return {
      addDoor: false,
      addDrawer: false,
      numberOfWalls: 0,
      numberOfShelves: 0,
      model: this.compartment.model,
      closeWithoutEdit: false,
      hadDoorOriginally: this.compartment.hasDoor,
      originalDoorPosition: this.compartment.doorPosition
    };
  },
  watch: {
    numberOfWalls: function(newVal) {
      this.compartment.visualizeWalls = newVal;
    },
    numberOfShelves: function(newVal) {
      this.compartment.visualizeShelves = newVal;
    },
    maxNumberOfWalls: function(newVal) {
      if (
        this.compartment.visualizeWalls > newVal ||
        this.numberOfWalls > newVal
      ) {
        this.compartment.visualizeWalls = newVal;
        this.numberOfWalls = newVal;
      }
    },
    maxNumberOfShelves: function(newVal) {
      if (
        this.compartment.visualizeShelves > newVal ||
        this.numberOfShelves > newVal
      ) {
        this.compartment.visualizeShelves = newVal;
        this.numberOfShelves = newVal;
      }
    },
    showDoorOption: function(shouldShow) {
      if (!shouldShow) {
        this.compartment.hasDoor = false;
      }
    },
    hasDoor: function(value) {
      if (!value) {
        var innerPlates = this.bookcase.plates.filter(
          plate => plate.parentCompartment === this.compartment
        );
        for (var i = 0; i < innerPlates.length; i++) {
          this.bookcase.deletePlate(innerPlates[i]);
        }
      }
    }
  },
  computed: {
    // showDrawerOption: function() {
    //   return (
    //     this.compartment.dimensions.width <= 800 &&
    //     this.compartment.dimensions.height <= 400 &&
    //     !(this.compartment.hasDrawer || this.compartment.hasDoor)
    //   );
    // },
    showWidthInfo: function() {
      return (
        this.compartment.dimensions.width >=
        this.bookcase.maxWidthWithDoor + this.bookcase.material.thickness
      );
    },
    showHeightInfo: function() {
      return (
        this.compartment.dimensions.height <=
        this.bookcase.minHeightWithDoor + this.bookcase.material.thickness
      );
    },
    showDoorOption: function() {
      return (
        this.compartment.dimensions.width <=
          this.bookcase.maxWidthWithDoor + this.bookcase.material.thickness &&
        this.compartment.dimensions.height >=
          this.bookcase.minHeightWithDoor + this.bookcase.material.thickness &&
        this.numberOfWalls == 0
      );
    },
    maxNumberOfWalls: function() {
      if (this.compartment.hasDoor) {
        return 0;
      }
      return this.compartment.maxNumberOfWalls;
    },
    maxNumberOfShelves: function() {
      return this.compartment.maxNumberOfShelves;
    },
    iconSize: function() {
      return 100;
    },
    hasLeftDoor: function() {
      return this.compartment.hasDoor && this.compartment.doorPosition === 0;
    },
    hasRightDoor: function() {
      return this.compartment.hasDoor && this.compartment.doorPosition === 1;
    },
    hasDoor: function() {
      return this.hasLeftDoor || this.hasRightDoor;
    }
  },
  methods: {
    selectDoor(hasDoor, doorPosition) {
      this.compartment.hasDoor = hasDoor;
      this.compartment.doorPosition = doorPosition;
    },
    close(closeWithoutEdit) {
      this.closeWithoutEdit = closeWithoutEdit;
      this.$emit("close");
    },
    getReturnArguments: function() {
      return {
        id: this.compartment.id,
        addWalls: this.numberOfWalls > 0,
        addShelves: this.numberOfShelves > 0,
        addDoor: this.addDoor,
        addDrawer: this.addDrawer,
        numberOfWalls: this.numberOfWalls,
        numberOfShelves: this.numberOfShelves,
        model: this.model
      };
    },
    submit(ev, el) {
      if (
        this.compartment.hasDoor !== this.hadDoorOriginally ||
        this.compartment.doorPosition !== this.originalDoorPosition
      ) {
        this.$emit("triggerHistoryVersion", true);
      }
      this.$emit("editCompartment", this.getReturnArguments());
    }
  },
  destroyed() {
    if (this.closeWithoutEdit) {
      this.compartment.hasDoor = this.hadDoorOriginally;
      this.compartment.doorPosition = this.originalDoorPosition;
    } else {
      this.submit();
    }
  }
};
</script>

<template>
  <div class="control-panel">
    <div class="control-panel-content">
      <h3>Rediger rum</h3>
      <div v-if="!model" class="row">
        <div v-if="maxNumberOfWalls > 0" class="col-6">
          <div>Antal vægge: {{numberOfWalls}}</div>
          <div>
            <input
              type="range"
              min="0"
              :max="maxNumberOfWalls"
              v-model.number="numberOfWalls"
              class="slider"
              id="wallsSlider"
            />
            <div class="sliderticks">
              <p>0</p>
              <p>{{maxNumberOfWalls}}</p>
            </div>
          </div>
        </div>
        <div v-if="maxNumberOfShelves > 0" class="col-6">
          <div>Antal hylder: {{numberOfShelves}}</div>
          <div>
            <input
              type="range"
              min="0"
              :max="maxNumberOfShelves"
              v-model.number="numberOfShelves"
              class="slider"
              id="shelvesSlider"
            />
            <div class="sliderticks">
              <p>0</p>
              <p>{{maxNumberOfShelves}}</p>
            </div>
          </div>
        </div>
      </div>
      <div class="row" style="margin-top: 15px; text-align: center" v-if="showDoorOption">
        <div class="col-12">
          <img
            src="/resources/icons/CompartmentNoDoor.png"
            :class="!hasDoor ? 'door-select-image selected' : 'door-select-image'"
            v-on:click="selectDoor(false,0)"
          />
          <img
            src="/resources/icons/CompartmentLeftHingedDoor.png"
            :class="hasLeftDoor ? 'door-select-image selected' : 'door-select-image'"
            v-on:click="selectDoor(true,0)"
          />
          <img
            src="/resources/icons/CompartmentRightHingedDoor.png"
            :class="hasRightDoor ? 'door-select-image selected' : 'door-select-image'"
            v-on:click="selectDoor(true,1)"
          />
        </div>
      </div>
      <div class="row" style="margin-top: 10px" v-if="showHeightInfo">
        <div class="col-12">Min. højde ved låge: 20 cm</div>
      </div>
      <div class="row" style="margin-top: 10px" v-if="showWidthInfo">
        <div class="col-12">Maks. bredde ved låge: 80 cm</div>
      </div>
      <div class="button-container">
        <button v-on:click="close(true)" class="btn btn-secondary">Annuller</button>
        <button v-on:click="close(false)" class="btn btn-brand">Vælg</button>
      </div>
    </div>
  </div>
</template>
<style>
.door-select-image {
  width: calc(30%);
  border: 10px solid transparent;
}
.door-select-image.selected {
  border: 10px solid #2dac86;
}
</style>