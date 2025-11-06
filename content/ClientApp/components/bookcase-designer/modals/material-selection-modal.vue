<template>
  <div class="custom-modal">
    <div
      class="modal-content material-image-container"
      style="display:inline-block; text-align: center; max-height: 80%; overflow: auto; max-width: 800px"
      v-click-outside="close"
    >
      <div class="modal-close-button">
        <button v-on:click="close()" class="s-btn s-btn-transparent">
          <icon icon="times" class="fa-2x"></icon>
        </button>
      </div>
      <p>Alle Shelfers reoler fremstilles af birkekrydsfiner, et utrolig stærkt materiale, som også er smukt at se på.</p>
      <h3>Ubehandlet birkekrydsfiner, 15 mm</h3>
      <div>Naturligt smuk, ubehandlet krydsfiner af birketræ.</div>
      <div
        v-for="material in untreatedMaterials"
        :key="material.id"
        :style="getBackgroundAttribute(material.id)"
        class="material-image"
        v-on:click="submit(material.id)"
      >
        <div class="material-image-text">{{material.name}}</div>
      </div>
      <!-- <h3>Malet krydsfiner, 15 mm</h3>
      <div
        v-for="material in paintedMaterials"
        :key="material.id"
        :style="getBackgroundAttribute(material.id)"
        class="material-image"
        v-on:click="submit(material.id)"
      >
        <div class="material-image-text">{{material.name}}</div>
      </div>-->
      <h3>Krydsfiner med plasticbelægning (Multiwall), 15 mm</h3>
      <div>Få et let udtryk med hvid birkekrydsfiner. Krydsfineren er belagt med Multiwall, et yderst slidstærkt, vandafvisende materiale med en let nopret overflade, som er nem at tørre af. Krydsfinerens kanter er blottede, så man kan se alle lagene, hvilket giver et helt særligt, moderne udtryk.</div>
      <div
        v-for="material in multiwallMaterials"
        :key="material.id"
        :style="getBackgroundAttribute(material.id)"
        class="material-image"
        v-on:click="submit(material.id)"
      >
        <div class="material-image-text">{{material.name}}</div>
      </div>
      <!-- <h3>Krydsfiner m. laminat, 17 mm</h3>
      <div
        v-for="material in laminateMaterials"
        :key="material.id"
        :style="getBackgroundAttribute(material.id)"
        class="material-image"
        v-on:click="submit(material.id)"
      >
        <div class="material-image-text">{{material.name}}</div>
      </div>-->
    </div>
  </div>
</template>
<script>
import dataTools from "../tools/data-tools";
export default {
  name: "material-selection-modal",
  mixins: [dataTools],
  props: {
    materials: Array
  },
  data() {
    return {};
  },
  computed: {
    untreatedMaterials: function() {
      return this.materials.filter(material => material.type === 0);
    },
    paintedMaterials: function() {
      return this.materials.filter(material => material.type === 1);
    },
    multiwallMaterials: function() {
      return this.materials.filter(material => material.type === 2);
    },
    laminateMaterials: function() {
      return this.materials.filter(material => material.type === 3);
    }
  },
  methods: {
    getBackgroundAttribute: function(materialId) {
      return (
        "background-image: url('" +
        this.getById(this.materials, materialId).surfaceTextureLocation.substr(
          2
        ) +
        "');"
      );
    },
    submit: function(id) {
      this.$emit("setMaterial", id);
    },
    close: function() {
      this.$emit("close");
    }
  }
};
</script>
<style>
.material-image {
  width: 200px;
  height: 200px;
  display: inline-block;
  cursor: pointer;
  border: 1px solid;
  border-radius: 10px;
  margin: 20px;
}
/* .material-image-container::-webkit-scrollbar {
  display: none;
} */
.material-image-text {
  position: relative;
  top: 50%;
  transform: translateY(-50%);
  color: white;
  mix-blend-mode: difference;
  margin: auto;
}
</style>
