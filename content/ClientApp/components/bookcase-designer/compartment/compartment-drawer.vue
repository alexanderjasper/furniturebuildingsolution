<template>
  <span>
    <!--Left and right-->
    <vgl-box-geometry
      :name="'drawerLeftRight' + compartment.id"
      :width="scaledNumber(plateWidth)"
      :height="getHeight()"
      :depth="scaledNumber(plateDepth)"
    ></vgl-box-geometry>
    <vgl-mesh
      :geometry="'drawerLeftRight' + compartment.id"
      material="std"
      :position="`${getLeft().x} ${getLeft().y} ${0 + drawerPulloutOffset}`"
    ></vgl-mesh>
    <vgl-mesh
      :geometry="'drawerLeftRight' + compartment.id"
      material="std"
      :position="`${getRight().x} ${getRight().y} ${0 + drawerPulloutOffset}`"
    ></vgl-mesh>
    <!--Left and right-->
    <!--Bottom-->
    <vgl-box-geometry
      :name="'drawerBottom' + compartment.id"
      :width="getWidth()"
      :height="scaledNumber(plateWidth)"
      :depth="scaledNumber(plateDepth)"
    ></vgl-box-geometry>
    <vgl-mesh
      :geometry="'drawerBottom' + compartment.id"
      material="std"
      :position="`${getBottom().x} ${getBottom().y} ${0 + drawerPulloutOffset}`"
    ></vgl-mesh>
    <!--Bottom-->
    <!--Front and rear-->
    <vgl-box-geometry
      :name="'drawerFrontRear' + compartment.id"
      :width="getWidth()"
      :height="getHeight()"
      :depth="scaledNumber(plateWidth)"
    ></vgl-box-geometry>
    <vgl-mesh
      :geometry="'drawerFrontRear' + compartment.id"
      material="std"
      ,
      :position="`${scaledNumber(compartment.center.x)} ${scaledNumber(compartment.center.y)} ${scaledNumber((plateDepth-plateWidth)/2) + drawerPulloutOffset}`"
    ></vgl-mesh>
    <vgl-mesh
      :geometry="'drawerFrontRear' + compartment.id"
      material="std"
      ,
      :position="`${scaledNumber(compartment.center.x)} ${scaledNumber(compartment.center.y)} ${scaledNumber((plateWidth-plateDepth)/2) + drawerPulloutOffset}`"
    ></vgl-mesh>
    <!--Front-->
  </span>
</template>
<script>
import * as VueGL from "vue-gl";
import dataTools from "../tools/data-tools";
import * as TWEEN from "@tweenjs/tween.js";
export default {
  name: "compartment-drawer",
  inject: ["vglNamespace"],
  components: {
    "vgl-box-geometry": VueGL.VglBoxGeometry,
    "vgl-mesh": VueGL.VglMesh
  },
  mixins: [dataTools],
  props: {
    compartment: Object,
    bookcase: Object
  },
  data() {
    return {
      drawerClearance: 5,
      handlePosition: { x: 0, y: 0.2 },
      handleSize: { width: 100, depth: 35, thickness: 15 },
      drawerPulloutOffset: 0,
      animationFinished: false,
      tween: null,
      tweenBack: null
    };
  },
  mounted() {
    if (!this.compartment.animationFinished) {
      this.tween = new TWEEN.Tween(this)
        .to({ drawerPulloutOffset: this.scaledNumber(200) }, 1000)
        .easing(TWEEN.Easing.Quadratic.InOut);
      this.tweenBack = new TWEEN.Tween(this)
        .to({ drawerPulloutOffset: 0 }, 1000)
        .easing(TWEEN.Easing.Quadratic.InOut);
      this.tween.chain(this.tweenBack);
      this.tween.start();
      this.animate();
    }
  },
  computed: {
    plateDepth: function() {
      return this.bookcaseData.plateDepth;
    },
    plateWidth: function() {
      return this.bookcaseData.material.thickness;
    }
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
    getLeft: function() {
      return {
        x: this.scaledNumber(
          this.compartment.left.start.x + this.plateWidth + this.drawerClearance
        ),
        y: this.scaledNumber(this.compartment.center.y)
      };
    },
    getRight: function() {
      return {
        x: this.scaledNumber(
          this.compartment.right.start.x -
            this.plateWidth -
            this.drawerClearance
        ),
        y: this.scaledNumber(this.compartment.center.y)
      };
    },
    getWidth: function() {
      return this.scaledNumber(
        this.compartment.dimensions.width -
          this.plateWidth -
          2 * this.drawerClearance
      );
    },
    getHeight: function() {
      return this.scaledNumber(
        this.compartment.dimensions.height -
          this.plateWidth -
          2 * this.drawerClearance
      );
    },
    getBottom: function() {
      return {
        x: this.scaledNumber(this.compartment.center.x),
        y: this.scaledNumber(
          this.compartment.bottom.start.y +
            this.plateWidth +
            this.drawerClearance
        )
      };
    }
  }
};
</script>
