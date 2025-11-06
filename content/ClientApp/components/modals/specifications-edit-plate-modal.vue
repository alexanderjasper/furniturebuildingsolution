<template>
  <div class="custom-modal">
    <div class="modal-content" style="max-width: 400px" v-click-outside="close">
      <h3 v-if="plate.id">Rediger plade id {{plate.id}}</h3>
      <h3 v-else>Opret plade</h3>
      <div>
        <div>Start</div>
        <div>
          X:
          <input class="coordinate-field" type="number" v-model="plate.start.x" /> Y:
          <input class="coordinate-field" type="number" v-model="plate.start.y" />
        </div>
        <div>Slut</div>
        <div>
          X:
          <input class="coordinate-field" type="number" v-model="plate.end.x" /> Y:
          <input class="coordinate-field" type="number" v-model="plate.end.y" />
        </div>
        <div>
          <label class="b-contain">
            <span>Beslag start</span>
            <input type="checkbox" v-model="plate.innerPlateStart" />
            <span class="b-input"></span>
          </label>
          <label class="b-contain">
            <span>Beslag slut</span>
            <input type="checkbox" v-model="plate.innerPlateEnd" />
            <span class="b-input"></span>
          </label>
          <label class="b-contain">
            <span>Flytbar</span>
            <input type="checkbox" v-model="plate.draggable" />
            <span class="b-input"></span>
          </label>
        </div>
        <div>
          Særlig dybde (mm):
          <input
            class="coordinate-field"
            type="number"
            v-model="plate.specialDepth"
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
  name: "specifications-edit-plate-modal",
  data() {
    return {
      plate: {
        start: {
          x: null,
          y: null
        },
        end: {
          x: null,
          y: null
        },
        innerPlateStart: true,
        innerPlateEnd: true,
        draggable: true,
        specialDepth: null
      }
    };
  },
  props: {
    bookcase: Object,
    inputPlate: Object
  },
  created() {
    if (this.inputPlate) {
      this.plate = this.inputPlate;
    }
  },
  methods: {
    close: function() {
      this.$emit("close");
    },
    validate: function() {
      if (
        this.plate.start.x !== this.plate.end.x &&
        this.plate.start.y !== this.plate.end.y
      ) {
        alert("Pladen skal være vandret eller lodret.");
        return false;
      } else {
        return true;
      }
    },
    save: function() {
      if (this.validate()) {
        if (!this.inputPlate) {
          this.bookcase.addNewPlate(
            this.plate.start.x,
            this.plate.start.y,
            this.plate.end.x,
            this.plate.end.y,
            this.plate.innerPlateStart,
            this.plate.innerPlateEnd,
            this.plate.draggable,
            this.plate.specialDepth
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