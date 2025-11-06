<template>
  <span>
    <span v-if="compartment.visualizeWalls > 0">
      <vgl-box-geometry
        name="wallVisualizationGeo"
        :width="wallWidth"
        :height="wallHeight"
        :depth="scaledNumber(plateDepth)"
      ></vgl-box-geometry>
    </span>
    <span v-if="compartment.visualizeShelves > 0">
      <vgl-box-geometry
        name="shelfVisualizationGeo"
        :width="shelfWidth"
        :height="shelfHeight"
        :depth="scaledNumber(plateDepth)"
      ></vgl-box-geometry>
    </span>
    <vgl-mesh
      v-for="(wallPosition, index) in wallPositions"
      :key="'wallVisualization' + index"
      :name="'wallVisualization' + index"
      geometry="wallVisualizationGeo"
      material="highlightMaterial"
      :position="wallPosition"
    ></vgl-mesh>
    <vgl-mesh
      v-for="(shelfPosition, index) in shelfPositions"
      :key="'shelfVisualization' + index"
      :name="'shelfVisualization' + index"
      geometry="shelfVisualizationGeo"
      material="highlightMaterial"
      :position="shelfPosition"
    ></vgl-mesh>
  </span>
</template>
<script>
import * as VueGL from "vue-gl";
import dataTools from "./tools/data-tools";
export default {
  name: "bookcase-plate-visualization",
  inject: ["vglNamespace"],
  mixins: [dataTools],
  components: {
    "vgl-box-geometry": VueGL.VglBoxGeometry,
    "vgl-mesh": VueGL.VglMesh
  },
  props: {
    bookcase: Object,
    compartment: Object
  },
  data() {
    return {};
  },
  computed: {
    plateWidth: function() {
      return this.bookcase.material.thickness;
    },
    plateDepth: function() {
      return this.bookcase.plateDepth;
    },
    wallWidth: function() {
      return this.scaledNumber(this.plateWidth);
    },
    wallHeight: function() {
      return this.scaledNumber(
        this.compartment.dimensions.height - this.plateWidth
      );
    },
    shelfWidth: function() {
      return this.scaledNumber(
        this.compartment.dimensions.width - this.plateWidth
      );
    },
    shelfHeight: function() {
      return this.scaledNumber(this.plateWidth);
    },
    wallPositions: function() {
      var array = [];
      var divisions = this.compartment.visualizeWalls + 1;
      var increment = this.compartment.dimensions.width / divisions;
      for (var i = 0; i < this.compartment.visualizeWalls; i++) {
        array.push(
          this.getScaledVectorString({
            x: this.compartment.bounds.left + (i + 1) * increment,
            y: this.compartment.center.y,
            z: this.plateDepth / 2
          })
        );
      }
      return array;
    },
    shelfPositions: function() {
      var array = [];
      var divisions = this.compartment.visualizeShelves + 1;
      var increment = this.compartment.dimensions.height / divisions;
      for (var i = 0; i < this.compartment.visualizeShelves; i++) {
        array.push(
          this.getScaledVectorString({
            x: this.compartment.center.x,
            y: this.compartment.bounds.lower + (i + 1) * increment,
            z: this.plateDepth / 2
          })
        );
      }
      return array;
    }
  }
};
</script>
