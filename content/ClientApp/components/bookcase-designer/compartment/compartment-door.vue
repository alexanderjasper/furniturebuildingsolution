<template>
  <span>
    <vgl-box-geometry
      :name="'0doorGeometry' + compartment.id"
      :width="scaledNumber(compartment.dimensions.width - plateWidth - 2*doorClearance)"
      :height="scaledNumber((compartment.dimensions.height -plateWidth)/2 - handleBoxPartRadius - doorClearance)"
      :depth="scaledNumber(16)"
    ></vgl-box-geometry>
    <vgl-box-geometry
      :name="'1doorGeometry' + compartment.id"
      :width="scaledNumber(compartment.dimensions.width -plateWidth - 2*doorClearance - 2*handleBoxPartRadius)"
      :height="scaledNumber(2*handleBoxPartRadius)"
      :depth="scaledNumber(doorThickness)"
    ></vgl-box-geometry>
    <vgl-mesh
      :name="'0door' + compartment.id"
      geometry="handleGeometry"
      :material="frontMaterial"
      :position="handleBoxPartPosition"
      :scale="doorScale"
      :rotation="doorRotation"
    ></vgl-mesh>
    <vgl-mesh
      :name="'1door' + compartment.id"
      :geometry="'0doorGeometry' + compartment.id"
      :material="frontMaterial"
      :position="topDoorPartPosition"
      :rotation="doorRotation"
    ></vgl-mesh>
    <vgl-mesh
      :name="'2door' + compartment.id"
      :geometry="'0doorGeometry' + compartment.id"
      :material="frontMaterial"
      :position="bottomDoorPartPosition"
      :rotation="doorRotation"
    ></vgl-mesh>
    <vgl-mesh
      :name="'3door' + compartment.id"
      :geometry="'1doorGeometry' + compartment.id"
      :material="frontMaterial"
      :position="sideDoorPartPosition"
      :rotation="doorRotation"
    ></vgl-mesh>

    <span v-if="coloredEdges">
      <vgl-mesh
        :name="'4door' + compartment.id"
        geometry="innerHandleGeometry"
        material="greyMdfBack"
        :position="handleBoxPartPosition"
        :rotation="handleInnerPartRotation"
      ></vgl-mesh>

      <vgl-plane-geometry
        :name="'2doorGeometry' + compartment.id"
        :width="scaledNumber(16)"
        :height="scaledNumber(compartment.dimensions.height -plateWidth - doorClearance*2 + 0.2)"
      ></vgl-plane-geometry>
      <vgl-plane-geometry
        :name="'3doorGeometry' + compartment.id"
        :width="scaledNumber(16)"
        :height="scaledNumber(compartment.dimensions.width -plateWidth - doorClearance*2 + 0.2)"
      ></vgl-plane-geometry>
      <vgl-mesh
        :name="'5door' + compartment.id"
        :geometry="'2doorGeometry' + compartment.id"
        :material="hingeSide === 'left' ? 'greyMdfBack' : 'greyMdf'"
        :position="getScaledVectorString(doorOpeningPosition(hingeSide,compartment.dimensions.width-plateWidth/2-doorClearance+0.1,compartment.center.y))"
        :rotation="leftRotation"
      ></vgl-mesh>
      <vgl-mesh
        :name="'6door' + compartment.id"
        :geometry="'2doorGeometry' + compartment.id"
        :material="hingeSide === 'left' ? 'greyMdf' : 'greyMdfBack'"
        :position="getScaledVectorString(doorOpeningPosition(hingeSide,plateWidth/2+doorClearance-0.1,compartment.center.y))"
        :rotation="leftRotation"
      ></vgl-mesh>
      <vgl-mesh
        :name="'7door' + compartment.id"
        :geometry="'3doorGeometry' + compartment.id"
        material="greyMdfBack"
        :position="getScaledVectorString(doorOpeningPosition(hingeSide,compartment.dimensions.width/2,compartment.center.y+compartment.dimensions.height/2-plateWidth/2-doorClearance+0.1))"
        :rotation="topRotation"
      ></vgl-mesh>
      <vgl-mesh
        :name="'7door' + compartment.id"
        :geometry="'3doorGeometry' + compartment.id"
        material="greyMdf"
        :position="getScaledVectorString(doorOpeningPosition(hingeSide,compartment.dimensions.width/2,compartment.center.y-compartment.dimensions.height/2+plateWidth/2+doorClearance-0.1))"
        :rotation="topRotation"
      ></vgl-mesh>
    </span>
  </span>
</template>
<script>
import * as VueGL from "vue-gl";
import dataTools from "../tools/data-tools";
import shapeGeometry from "../tools/shape-geometry";
import * as TWEEN from "@tweenjs/tween.js";
export default {
  name: "compartment-door",
  inject: ["vglNamespace"],
  mixins: [dataTools],
  components: {
    "shape-geometry": shapeGeometry,
    "vgl-box-geometry": VueGL.VglBoxGeometry,
    "vgl-plane-geometry": VueGL.VglPlaneGeometry,
    "vgl-mesh": VueGL.VglMesh
  },
  props: {
    compartment: Object,
    bookcase: Object,
    compartmentSelected: Boolean
  },
  data() {
    return {
      doorClearance: 3,
      doorThickness: 16,
      handlePosition: { x: -0.35, y: 0 },
      handleRadius: 15,
      handleBoxPartRadius: 20,
      handleOffset: 30,
      openingAngle: this.compartmentSelected ? Math.PI / 2 : 0,
      animationFinished: false,
      tweenOpen: new TWEEN.Tween(this)
        .to({ openingAngle: Math.PI / 2 }, 1000)
        .easing(TWEEN.Easing.Quadratic.InOut),
      tweenClose: new TWEEN.Tween(this)
        .to({ openingAngle: 0 }, 1000)
        .easing(TWEEN.Easing.Quadratic.InOut)
    };
  },
  methods: {
    animate: function(time) {
      var id = requestAnimationFrame(this.animate);

      var result = TWEEN.default.update(time);
      this.vglNamespace.update();
      if (!result) {
        cancelAnimationFrame(id);
        this.compartment.animationFinished = true;
      }
    },
    doorOpeningPosition: function(
      hingeSide,
      centerDistanceFromAxis,
      yPosition
    ) {
      var axisX =
        hingeSide === "left"
          ? this.compartment.left.start.x
          : this.compartment.right.start.x;
      var axisZ = this.zPosition + this.plateWidth / 2;
      var direction = hingeSide === "left" ? 1 : -1;
      return {
        x:
          axisX +
          direction * centerDistanceFromAxis * Math.cos(this.openingAngle),
        y: yPosition,
        z:
          axisZ - centerDistanceFromAxis * Math.sin(this.openingAngle + Math.PI)
      };
    }
  },
  watch: {
    compartmentSelected(newVal) {
      if (newVal) {
        this.tweenOpen.start();
      } else {
        this.tweenClose.start();
      }
      this.animate();
    },
    hingeSide() {
      this.vglNamespace.update();
    }
  },
  computed: {
    frontMaterial: function() {
      if (this.bookcase.doorMaterial == 1 || this.bookcase.doorMaterial == 2) {
        return "stdOffWhite";
      } else {
        return "greyMdf";
      }
    },
    coloredEdges: function() {
      if (this.bookcase.doorMaterial == 1) {
        return true;
      } else {
        return false;
      }
    },
    plateWidth: function() {
      return this.bookcase.material.thickness;
    },
    zPosition: function() {
      return this.bookcase.plateDepth - this.doorThickness;
    },
    width: function() {
      return (
        this.compartment.dimensions.width -
        this.plateWidth -
        2 * this.doorClearance
      );
    },
    height: function() {
      return (
        this.compartment.dimensions.height -
        this.plateWidth -
        2 * this.doorClearance
      );
    },
    hingeSide: function() {
      return this.compartment.doorPosition === 0 ? "left" : "right";
    },
    leftHingedDoorPosition: function() {},
    doorCenterPosition: function() {
      return this.doorOpeningPosition(
        this.hingeSide,
        this.compartment.dimensions.width / 2,
        this.compartment.center.y
      );
    },
    doorRotation: function() {
      var direction = this.hingeSide === "left" ? -1 : 1;
      return this.getVectorString({
        x: 0,
        y: this.openingAngle * direction,
        z: 0
      });
    },
    leftRotation: function() {
      var direction = this.hingeSide === "left" ? -1 : 1;
      return this.getVectorString({
        x: 0,
        y: this.openingAngle * direction - Math.PI / 2,
        z: 0
      });
    },
    topRotation: function() {
      var direction = this.hingeSide === "left" ? -1 : 1;
      return this.getVectorString({
        x: Math.PI / 2,
        y: 0,
        z: -this.openingAngle * direction - Math.PI / 2
      });
    },
    handleInnerPartRotation: function() {
      var direction = this.hingeSide === "left" ? -1 : 1;
      return this.getVectorString({
        x: Math.PI / 2,
        y: 0,
        z: -this.openingAngle * direction
      });
    },
    handleBoxPartPosition: function() {
      return this.getScaledVectorString(
        this.doorOpeningPosition(
          this.hingeSide,
          this.compartment.dimensions.width -
            this.plateWidth / 2 -
            this.doorClearance -
            this.handleBoxPartRadius,
          this.compartment.center.y
        )
      );
    },
    doorScale: function() {
      var scale = 1 / this.scale;
      return this.getVectorString({
        x: scale,
        y: scale,
        z: scale
      });
    },
    doorPartYOffset: function() {
      return (
        this.handleBoxPartRadius / 2 +
        (this.compartment.dimensions.height -
          this.plateWidth -
          2 * this.doorClearance) /
          4
      );
    },
    topDoorPartPosition: function() {
      return this.getScaledVectorString(
        this.doorOpeningPosition(
          this.hingeSide,
          this.compartment.dimensions.width / 2,
          this.compartment.center.y + this.doorPartYOffset
        )
      );
    },
    bottomDoorPartPosition: function() {
      return this.getScaledVectorString(
        this.doorOpeningPosition(
          this.hingeSide,
          this.compartment.dimensions.width / 2,
          this.compartment.center.y - this.doorPartYOffset
        )
      );
    },
    sideDoorPartPosition: function() {
      return this.getScaledVectorString(
        this.doorOpeningPosition(
          this.hingeSide,
          this.compartment.dimensions.width / 2 - this.handleBoxPartRadius,
          this.compartment.center.y
        )
      );
    }
  }
};
</script>
