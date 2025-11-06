<template>
  <span>
    <vgl-texture
      :src="bookcase.material.surfaceTextureLocation"
      :name="'plywoodSupportSurfaceTexture' + this.id"
      wrap-s="repeat"
      :repeat="supportWidth/400 + ' 1'"
    ></vgl-texture>
    <vgl-mesh-lambert-material
      :name="'plywoodSupportSurfaceMaterial' + this.id"
      :map="'plywoodSupportSurfaceTexture' + this.id"
    ></vgl-mesh-lambert-material>

    <vgl-plane-geometry
      :name="'supportFrontGeo' + this.id"
      :width="scaledNumber(supportWidth)"
      :height="scaledNumber(supportHeight)"
    ></vgl-plane-geometry>
    <vgl-plane-geometry
      :name="'supportSideGeo' + this.id"
      :height="scaledNumber(supportWidth)"
      :width="scaledNumber(thickness)"
    ></vgl-plane-geometry>

    <vgl-mesh
      :name="'supportSide' + this.id"
      :geometry="'supportSideGeo' + this.id"
      material="sideMaterial"
      :rotation="`${Math.PI/2} 0 ${Math.PI/2}`"
      :position="supportSidePosition"
    ></vgl-mesh>
    <vgl-mesh
      :name="'supportFront' + this.id"
      :geometry="'supportFrontGeo' + this.id"
      :material="'plywoodSupportSurfaceMaterial' + this.id"
      :position="supportFrontPosition"
    ></vgl-mesh>
  </span>
</template>
<script>
import * as VueGL from "vue-gl";
import dataTools from "../tools/data-tools";
export default {
  name: "bookcase-support",
  mixins: [dataTools],
  components: {
    "vgl-texture": VueGL.VglTexture,
    "vgl-mesh-lambert-material": VueGL.VglMeshLambertMaterial,
    "vgl-plane-geometry": VueGL.VglPlaneGeometry,
    "vgl-mesh": VueGL.VglMesh
  },
  props: {
    start: Object,
    end: Object,
    zPlacement: String,
    id: String,
    bookcase: Object
  },
  data() {
    return {
      centerSupportThreshold: 250,
      doubleSupportThreshold: 500
    };
  },
  computed: {
    supportWidth: function() {
      return this.end.x - this.start.x - this.thickness;
    },
    thickness: function() {
      return this.bookcase.material.thickness;
    },
    center: function() {
      var zPosition;
      if (this.zPlacement === "rear") {
        zPosition = this.thickness / 2;
      } else if (this.zPlacement === "front") {
        zPosition = this.bookcase.plateDepth - this.thickness / 2;
      }
      return {
        x: (this.start.x + this.end.x) / 2,
        y: (this.start.y + this.end.y) / 2,
        z: zPosition
      };
    },
    supportFrontPosition: function() {
      return this.getScaledVectorString({
        x: this.center.x,
        y: this.center.y - this.thickness / 2 - this.supportHeight / 2,
        z: this.center.z + this.thickness / 2
      });
    },
    supportSidePosition: function() {
      return this.getScaledVectorString({
        x: this.center.x,
        y: this.center.y - this.thickness / 2 - this.supportHeight,
        z: this.center.z
      });
    }
  }
};
</script>
