<template>
  <span>
    <vgl-plane-geometry
      name="measurementEnd"
      :width="scaledNumber(endWidth)"
      :height="scaledNumber(endHeight)"
    ></vgl-plane-geometry>
    <vgl-plane-geometry
      name="measurementBar"
      :width="scaledNumber(barLength)"
      :height="scaledNumber(barWidth)"
    ></vgl-plane-geometry>

    <vgl-mesh
      geometry="measurementEnd"
      material="highlightMaterialBasic"
      :position="startPosition"
      :rotation="rotation"
    ></vgl-mesh>
    <vgl-mesh
      geometry="measurementEnd"
      material="highlightMaterialBasic"
      :position="endPosition"
      :rotation="rotation"
    ></vgl-mesh>
    <vgl-mesh
      geometry="measurementBar"
      material="highlightMaterialBasic"
      :position="centerPosition"
      :rotation="rotation"
    ></vgl-mesh>
  </span>
</template>
<script>
import * as VueGL from "vue-gl";
import dataTools from "./tools/data-tools";
export default {
  name: "measurement-guide",
  mixins: [dataTools],
  components: {
    //VueGL
    "vgl-mesh": VueGL.VglMesh,
    "vgl-plane-geometry": VueGL.VglPlaneGeometry
  },
  props: {
    coordinates: Object,
    bookcaseDepth: Number,
    plateWidth: Number
  },
  data() {
    return {
      endWidth: 30,
      endHeight: 100,
      barWidth: 30
    };
  },
  computed: {
    startPosition() {
      if (this.isHorizontal) {
        return this.getScaledVectorString({
          x: this.coordinates.start.x + this.endWidth / 2,
          y: this.coordinates.start.y + this.yOffset,
          z: this.depthOffset
        });
      } else {
        return this.getScaledVectorString({
          x: this.coordinates.start.x,
          y: this.coordinates.start.y + this.endWidth / 2 + this.yOffset,
          z: this.depthOffset
        });
      }
    },
    endPosition() {
      if (this.isHorizontal) {
        return this.getScaledVectorString({
          x: this.coordinates.end.x - this.endWidth / 2,
          y: this.coordinates.end.y + this.yOffset,
          z: this.depthOffset
        });
      } else {
        return this.getScaledVectorString({
          x: this.coordinates.end.x,
          y: this.coordinates.end.y - this.endWidth / 2 + this.yOffset,
          z: this.depthOffset
        });
      }
    },
    centerPosition() {
      return this.getScaledVectorString({
        x: (this.coordinates.end.x + this.coordinates.start.x) / 2,
        y:
          (this.coordinates.end.y + this.coordinates.start.y) / 2 +
          this.yOffset,
        z: this.depthOffset
      });
    },
    barLength() {
      var baseLength = this.isHorizontal
        ? this.coordinates.end.x - this.coordinates.start.x
        : this.coordinates.end.y - this.coordinates.start.y;
      return baseLength - 2 * this.endWidth;
    },
    isHorizontal() {
      return this.coordinates.start.y === this.coordinates.end.y;
    },
    rotation() {
      return this.getVectorString({
        x: 0,
        y: 0,
        z: this.isHorizontal ? 0 : Math.PI / 2
      });
    },
    depthOffset() {
      return this.bookcaseDepth + 1;
    },
    yOffset() {
      return -this.plateWidth / 2;
    }
  }
};
</script>
