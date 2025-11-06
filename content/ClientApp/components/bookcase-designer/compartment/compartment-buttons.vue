<template>
  <span>
    <vgl-plane-geometry
      :name="'invisibleCompartmentButton' + compartment.id"
      :width="scaledNumber(compartment.dimensions.width)"
      :height="scaledNumber(compartment.dimensions.height)"
    ></vgl-plane-geometry>
    <vgl-mesh
      :name="'0EditButton' + compartment.id"
      :geometry="'invisibleCompartmentButton' + compartment.id"
      material="invisibleMaterial"
      :position="outerSquarePosition"
    ></vgl-mesh>
    <span v-if="showButtons">
      <vgl-mesh
        :name="'2EditButton' + compartment.id"
        geometry="pencilBody"
        material="blackMaterial"
        :position="pencilBodyPosition"
        :rotation="`0 0 ${-Math.PI/4}`"
      ></vgl-mesh>
      <vgl-mesh
        :name="'3EditButton' + compartment.id"
        geometry="pencilTip"
        material="blackMaterial"
        :position="pencilTipPosition"
      ></vgl-mesh>
      <vgl-mesh
        :name="'4EditButton' + compartment.id"
        geometry="pencilEraser"
        material="blackMaterial"
        :position="pencilEraserPosition"
        :rotation="`0 0 ${-Math.PI/4}`"
      ></vgl-mesh>
    </span>
  </span>
</template>
<script>
import * as VueGL from "vue-gl";
import dragControls from "../controls/drag-controls";
import dataTools from "../tools/data-tools";
export default {
  name: "compartment-buttons",
  mixins: [dataTools],
  components: {
    "drag-controls": dragControls,
    "vgl-plane-geometry": VueGL.VglPlaneGeometry,
    "vgl-mesh": VueGL.VglMesh
  },
  props: {
    compartment: Object,
    isEditable: Boolean,
    camera: String,
    bookcase: Object,
    showButtons: Boolean
  },
  data() {
    return {
      pencilOffset: -5
    };
  },
  methods: {
    controlPositions: function(plateType) {
      var bounds = this.compartment.bounds;
      if (plateType === "top") {
        return {
          x: this.scaledNumber((bounds.right + bounds.left) / 2),
          y: this.scaledNumber(bounds.upper),
          z: this.zPosition + this.scaledNumber(1)
        };
      } else if (plateType === "bottom") {
        return {
          x: this.scaledNumber((bounds.right + bounds.left) / 2),
          y: this.scaledNumber(bounds.lower),
          z: this.zPosition + this.scaledNumber(1)
        };
      } else if (plateType === "left") {
        return {
          x: this.scaledNumber(bounds.left),
          y: this.scaledNumber((bounds.upper + bounds.lower) / 2),
          z: this.zPosition + this.scaledNumber(1)
        };
      } else if (plateType === "right") {
        return {
          x: this.scaledNumber(bounds.right),
          y: this.scaledNumber((bounds.upper + bounds.lower) / 2),
          z: this.zPosition + this.scaledNumber(1)
        };
      }
    }
  },
  computed: {
    plateDepth: function() {
      return this.bookcase.plateDepth;
    },
    outerSquarePosition: function() {
      let center = this.compartment.center;
      return (
        this.scaledNumber(center.x) +
        " " +
        this.scaledNumber(center.y) +
        " " +
        this.scaledNumber(50)
      );
    },
    innerSquarePosition: function() {
      let center = this.compartment.center;
      return (
        this.scaledNumber(center.x) +
        " " +
        this.scaledNumber(center.y) +
        " " +
        (this.zPosition + this.scaledNumber(2))
      );
    },
    pencilBodyPosition: function() {
      let center = this.compartment.center;
      return (
        this.scaledNumber(center.x + this.pencilOffset) +
        " " +
        this.scaledNumber(center.y + this.pencilOffset) +
        " " +
        (this.zPosition + this.scaledNumber(5))
      );
    },
    pencilTipPosition: function() {
      let center = this.compartment.center;
      return (
        this.scaledNumber(center.x - 20 + this.pencilOffset) +
        " " +
        this.scaledNumber(center.y - 20 + this.pencilOffset) +
        " " +
        (this.zPosition + this.scaledNumber(3))
      );
    },
    pencilEraserPosition: function() {
      let center = this.compartment.center;
      return (
        this.scaledNumber(center.x + 30 + this.pencilOffset) +
        " " +
        this.scaledNumber(center.y + 30 + this.pencilOffset) +
        " " +
        (this.zPosition + this.scaledNumber(4))
      );
    },
    zPosition: function() {
      return this.scaledNumber(this.plateDepth + 1);
    }
  }
};
</script>
