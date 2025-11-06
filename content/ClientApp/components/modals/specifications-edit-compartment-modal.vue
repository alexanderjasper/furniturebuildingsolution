<template>
  <div class="custom-modal">
    <div class="modal-content" style="max-width: 400px" v-click-outside="close">
      <h3 v-if="inputCompartment">Rediger rum id {{inputCompartment.id}}</h3>
      <h3 v-else>Tilføj rum</h3>
      <div>
        <div>
          Venstre:
          <select v-model="compartment.left">
            <option
              v-for="(plate,index) in bookcase.plates"
              :key="index"
              :value="plate"
            >{{plate.id}}</option>
          </select>
        </div>
        <div>
          Højre:
          <select v-model="compartment.right">
            <option
              v-for="(plate,index) in bookcase.plates"
              :key="index"
              :value="plate"
            >{{plate.id}}</option>
          </select>
        </div>
        <div>
          Top:
          <select v-model="compartment.top">
            <option
              v-for="(plate,index) in bookcase.plates"
              :key="index"
              :value="plate"
            >{{plate.id}}</option>
          </select>
        </div>
        <div>
          Bund:
          <select v-model="compartment.bottom">
            <option
              v-for="(plate,index) in bookcase.plates"
              :key="index"
              :value="plate"
            >{{plate.id}}</option>
          </select>
        </div>
        <div>
          <label class="b-contain">
            <span>Låge</span>
            <input type="checkbox" v-model="compartment.hasDoor" />
            <span class="b-input"></span>
          </label>
        </div>
        <div v-if="compartment.hasDoor">
          Lågeplacering:
          <select v-model="compartment.doorPosition">
            <option value="0">Venstre</option>
            <option value="1">Højre</option>
            <option value="2">Dobbelt</option>
          </select>
        </div>
        <div>
          Understøttelse type:
          <select v-model="compartment.forceSupport">
            <option value="0">Auto</option>
            <option value="2">Ingen</option>
            <option value="3">Top</option>
            <option value="4">Bund</option>
            <option value="5">Vægophæng</option>
          </select>
        </div>
        <div>
          Bagplade (mm fra front):
          <input
            class="coordinate-field"
            type="number"
            v-model="compartment.backPlatePosition"
          />
        </div>
      </div>
      <div class="modal-button-container">
        <button v-on:click="close()" class="btn btn-secondary">Luk</button>
        <button v-on:click="save()" class="btn">Gem</button>
      </div>
    </div>
  </div>
</template>

<script>
import { bookcaseService } from "../../_services/bookcase.service";
export default {
  name: "specifications-edit-compartment-modal",
  data() {
    return {
      compartment: {
        top: null,
        bottom: null,
        left: null,
        right: null,
        hasDoor: false,
        doorPosition: 0,
        forceSupport: 0,
        backPlatePosition: null
      }
    };
  },
  props: {
    inputCompartment: Object,
    bookcase: Object
  },
  created() {
    if (this.inputCompartment) {
      this.compartment = this.inputCompartment;
    }
  },
  methods: {
    close: function() {
      this.$emit("close");
    },
    validate: function() {
      return (
        this.compartment.top &&
        this.compartment.bottom &&
        this.compartment.left &&
        this.compartment.right
      );
    },
    save: function() {
      if (this.validate()) {
        if (!this.inputCompartment) {
          this.bookcase.addGetCompartment(
            null,
            this.compartment.top,
            this.compartment.bottom,
            this.compartment.left,
            this.compartment.right,
            this.compartment.hasDoor,
            false,
            null,
            this.compartment.doorPosition,
            this.compartment.forceSupport,
            this.compartment.backPlatePosition
          );
        }
        bookcaseService.save(this.bookcase).then(
          () => {
            this.close();
          },
          error => {
            alert(error);
          }
        );
      }
    }
  }
};
</script>

<style scoped>
.coordinate-field {
  width: 60px;
}
</style>