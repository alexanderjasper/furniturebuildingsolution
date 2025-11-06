<template>
  <span>
    <span v-if="plate.innerPlateStart">
      <vgl-mesh
        :name="'1Cabineo' + plate.id"
        geometry="cabineoGeometry"
        :material="cabineoMaterial"
        :position="joint1Position"
        :rotation="joint1Rotation"
        :scale="cabineoScale"
      ></vgl-mesh>
      <vgl-mesh
        :name="'2Cabineo' + plate.id"
        geometry="cabineoGeometry"
        :material="cabineoMaterial"
        :position="joint2Position"
        :rotation="joint2Rotation"
        :scale="cabineoScale"
      ></vgl-mesh>
    </span>
    <span v-if="plate.innerPlateEnd">
      <vgl-mesh
        :name="'3Cabineo' + plate.id"
        geometry="cabineoGeometry"
        :material="cabineoMaterial"
        :position="joint3Position"
        :rotation="joint3Rotation"
        :scale="cabineoScale"
      ></vgl-mesh>
      <vgl-mesh
        :name="'4Cabineo' + plate.id"
        geometry="cabineoGeometry"
        :material="cabineoMaterial"
        :position="joint4Position"
        :rotation="joint4Rotation"
        :scale="cabineoScale"
      ></vgl-mesh>
    </span>
  </span>
</template>
<script>
import * as VueGL from "vue-gl";
import dataTools from "./tools/data-tools";
export default {
  name: "cabineo-joints",
  mixins: [dataTools],
  components: {
    "vgl-mesh": VueGL.VglMesh
  },
  props: {
    plate: Object,
    bookcase: Object,
    startX: Number,
    startY: Number,
    endX: Number,
    endY: Number
  },
  data() {
    return {
      side: ""
    };
  },
  computed: {
    plateWidth: function() {
      return this.bookcase.material.thickness;
    },
    plateDepth: function() {
      return this.bookcase.plateDepth;
    },
    isInLeftPart: function() {
      if (this.plate.isHorizontal) {
        return true;
      }
      if (this.startX < this.bookcase.center.x) {
        return true;
      }
      return false;
    },
    cabineoScale: function() {
      var scale = 33.5 / 19 / 5000;
      return this.getVectorString({
        x: scale,
        y: scale,
        z: scale
      });
    },
    cabineoMaterial: function() {
      if (this.bookcase.material.name === "Sort") {
        //TODO: Make this not hardcoded.
        return "cabineoBlackMaterial";
      } else if (this.bookcase.material.name === "Lakeret") {
        return "cabineoLightBrownMaterial";
      }
      return "cabineoWhiteMaterial";
    },
    joint1Position: function() {
      if (this.side === "bottom") {
        return this.getVectorString({
          x: this.scaledNumber(this.startX + this.plateWidth / 2),
          y: this.scaledNumber(this.startY - this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 5) / 6)
        });
      } else if (this.side === "top") {
        return this.getVectorString({
          x: this.scaledNumber(this.startX + this.plateWidth / 2),
          y: this.scaledNumber(this.startY + this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 5) / 6)
        });
      } else if (this.side === "left") {
        return this.getVectorString({
          x: this.scaledNumber(this.startX - this.plateWidth / 2),
          y: this.scaledNumber(this.startY + this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 5) / 6)
        });
      } else {
        return this.getVectorString({
          x: this.scaledNumber(this.startX + this.plateWidth / 2),
          y: this.scaledNumber(this.startY + this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 5) / 6)
        });
      }
    },
    joint1Rotation: function() {
      if (this.side === "bottom") {
        return this.getVectorString({
          x: Math.PI / 2,
          y: 0,
          z: 0
        });
      } else if (this.side === "top") {
        return this.getVectorString({
          x: -Math.PI / 2,
          y: 0,
          z: 0
        });
      } else if (this.side === "left") {
        return this.getVectorString({
          x: -Math.PI / 2,
          y: -Math.PI / 2,
          z: 0
        });
      } else {
        return this.getVectorString({
          x: Math.PI / 2,
          y: Math.PI / 2,
          z: 0
        });
      }
    },
    joint2Position: function() {
      if (this.side === "bottom") {
        return this.getVectorString({
          x: this.scaledNumber(this.startX + this.plateWidth / 2),
          y: this.scaledNumber(this.startY - this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 1) / 6)
        });
      } else if (this.side === "top") {
        return this.getVectorString({
          x: this.scaledNumber(this.startX + this.plateWidth / 2),
          y: this.scaledNumber(this.startY + this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 1) / 6)
        });
      } else if (this.side === "left") {
        return this.getVectorString({
          x: this.scaledNumber(this.startX - this.plateWidth / 2),
          y: this.scaledNumber(this.startY + this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 1) / 6)
        });
      } else {
        return this.getVectorString({
          x: this.scaledNumber(this.startX + this.plateWidth / 2),
          y: this.scaledNumber(this.startY + this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 1) / 6)
        });
      }
    },
    joint2Rotation: function() {
      return this.joint1Rotation;
    },
    joint3Position: function() {
      if (this.side === "bottom") {
        return this.getVectorString({
          x: this.scaledNumber(this.endX - this.plateWidth / 2),
          y: this.scaledNumber(this.startY - this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 1) / 6)
        });
      } else if (this.side === "top") {
        return this.getVectorString({
          x: this.scaledNumber(this.endX - this.plateWidth / 2),
          y: this.scaledNumber(this.startY + this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 1) / 6)
        });
      } else if (this.side === "left") {
        return this.getVectorString({
          x: this.scaledNumber(this.startX - this.plateWidth / 2),
          y: this.scaledNumber(this.endY - this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 5) / 6)
        });
      } else {
        return this.getVectorString({
          x: this.scaledNumber(this.startX + this.plateWidth / 2),
          y: this.scaledNumber(this.endY - this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 5) / 6)
        });
      }
    },
    joint3Rotation: function() {
      if (this.side === "bottom") {
        return this.getVectorString({
          x: -Math.PI / 2,
          y: Math.PI,
          z: 0
        });
      } else if (this.side === "top") {
        return this.getVectorString({
          x: Math.PI / 2,
          y: Math.PI,
          z: 0
        });
      } else if (this.side === "left") {
        return this.getVectorString({
          x: Math.PI / 2,
          y: -Math.PI / 2,
          z: 0
        });
      } else {
        return this.getVectorString({
          x: -Math.PI / 2,
          y: Math.PI / 2,
          z: 0
        });
      }
    },
    joint4Position: function() {
      if (this.side === "bottom") {
        return this.getVectorString({
          x: this.scaledNumber(this.endX - this.plateWidth / 2),
          y: this.scaledNumber(this.startY - this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 5) / 6)
        });
      } else if (this.side === "top") {
        return this.getVectorString({
          x: this.scaledNumber(this.endX - this.plateWidth / 2),
          y: this.scaledNumber(this.startY + this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 5) / 6)
        });
      } else if (this.side === "left") {
        return this.getVectorString({
          x: this.scaledNumber(this.startX - this.plateWidth / 2),
          y: this.scaledNumber(this.endY - this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 1) / 6)
        });
      } else {
        return this.getVectorString({
          x: this.scaledNumber(this.startX + this.plateWidth / 2),
          y: this.scaledNumber(this.endY - this.plateWidth / 2),
          z: this.scaledNumber((this.plateDepth * 1) / 6)
        });
      }
    },
    joint4Rotation: function() {
      return this.joint3Rotation;
    }
  },
  watch: {
    startX(newVal, oldVal) {
      if (newVal !== oldVal) {
        this.setCabineoSide();
      }
    },
    startY(newVal, oldVal) {
      if (newVal !== oldVal) {
        this.setCabineoSide();
      }
    }
  },
  created() {
    this.setCabineoSide();
  },
  mounted() {},
  methods: {
    setCabineoSide: function() {
      this.side = this.plate.cabineoSide;
    }
  }
};
</script>
