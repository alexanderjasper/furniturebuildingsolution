<template>
  <span>
    <!--Standard materials-->
    <vgl-mesh-lambert-material name="std"></vgl-mesh-lambert-material>
    <vgl-mesh-lambert-material name="stdGrey" color="#dfdfdf"></vgl-mesh-lambert-material>
    <vgl-mesh-lambert-material name="stdDarkGrey" color="#bfbfbf"></vgl-mesh-lambert-material>
    <vgl-mesh-lambert-material name="stdOffWhite" color="#f9f9f9"></vgl-mesh-lambert-material>
    <vgl-mesh-lambert-material name="cabineoWhiteMaterial" color="#f9f9f9"></vgl-mesh-lambert-material>
    <vgl-mesh-lambert-material name="cabineoLightBrownMaterial" color="#e6d2b5"></vgl-mesh-lambert-material>
    <vgl-mesh-lambert-material name="cabineoBlackMaterial" color="#0e0e10"></vgl-mesh-lambert-material>
    <vgl-mesh-lambert-material name="greyMdf" color="#505050"></vgl-mesh-lambert-material>
    <vgl-mesh-lambert-material name="greyMdfBack" color="#505050" side="back"></vgl-mesh-lambert-material>
    <vgl-mesh-lambert-material name="highlightMaterial" color="#2dac86"></vgl-mesh-lambert-material>
    <vgl-mesh-basic-material name="highlightMaterialBasic" color="#2dac86"></vgl-mesh-basic-material>
    <vgl-mesh-basic-material name="warningMaterial" color="#ff0000"></vgl-mesh-basic-material>
    <vgl-mesh-basic-material name="whiteMaterial" color="#fff"></vgl-mesh-basic-material>
    <vgl-mesh-basic-material name="blackMaterial" color="#000"></vgl-mesh-basic-material>
    <!--Standard materials-->
    <!--Used for plate drag handles-->
    <vgl-plane-geometry name="dragHandle" :width="scaledNumber(10)" :height="scaledNumber(47)"></vgl-plane-geometry>
    <vgl-plane-geometry
      name="dragHandleInvisiblePart"
      :width="scaledNumber(160)"
      :height="scaledNumber(140)"
    ></vgl-plane-geometry>
    <vgl-circle-geometry
      name="dragHandleInvisiblePartSmall"
      :radius="scaledNumber(50)"
      segments="8"
    ></vgl-circle-geometry>
    <vgl-circle-geometry name="compartmentSelectButton" :radius="scaledNumber(40)" segments="32"></vgl-circle-geometry>
    <vgl-circle-geometry
      name="compartmentSelectButtonMiddlePart"
      :radius="scaledNumber(30)"
      segments="32"
    ></vgl-circle-geometry>
    <vgl-mesh-basic-material name="invisibleMaterial"></vgl-mesh-basic-material>
    <!--Used for plate drag handles-->
    <!--Used for bin symbol-->
    <shape-geometry name="binGeometry" :data="binData"></shape-geometry>
    <!--Used for bin symbol-->
    <!--Used for door handles-->
    <shape-geometry name="handleGeometry" :data="doorHandleData" :depth="32"></shape-geometry>
    <vgl-cylinder-geometry
      name="innerHandleGeometry"
      :radius-top="scaledNumber(14.9)"
      :radius-bottom="scaledNumber(14.9)"
      :height="scaledNumber(16)"
      :open-ended="true"
      :radial-segments="32"
    ></vgl-cylinder-geometry>
    <!--Used for bin symbol-->
    <!--Used for invisible compartment measurement buttons-->
    <vgl-plane-geometry
      name="measurementButtonGeometry"
      :width="scaledNumber(120)"
      :height="scaledNumber(120)"
    ></vgl-plane-geometry>
    <shape-geometry name="widthBoxGeometry" :data="widthBoxData"></shape-geometry>
    <shape-geometry name="heightBoxGeometry" :data="heightBoxData"></shape-geometry>
    <shape-geometry name="cabineoGeometry" :data="cabineoData" :depth="2"></shape-geometry>
    <!-- <vgl-plane-geometry
      name="measurementBoxGeometry"
      :width="scaledNumber(130)"
      :height="scaledNumber(70)"
    ></vgl-plane-geometry>-->
    <!--Used for invisible compartment measurement buttons-->
    <!--Used for compartment edit buttons-->
    <vgl-plane-geometry name="pencilBody" :width="scaledNumber(20)" :height="scaledNumber(55)"></vgl-plane-geometry>
    <vgl-plane-geometry
      name="pencilTip"
      :width="scaledNumber(20/Math.sqrt(2))"
      :height="scaledNumber(20/Math.sqrt(2))"
    ></vgl-plane-geometry>
    <vgl-plane-geometry name="pencilEraser" :width="scaledNumber(20)" :height="scaledNumber(20)"></vgl-plane-geometry>
    <!--Used for compartment edit buttons-->
    <!--Used for measurements-->
    <vgl-text-geometry
      name="number0"
      font="../../resources/fonts/helvetiker_regular.typeface.json"
      text="0"
      :size="scaledNumber(textSize)"
      :height="textHeight"
    ></vgl-text-geometry>
    <vgl-text-geometry
      name="number1"
      font="../../resources/fonts/helvetiker_regular.typeface.json"
      text="1"
      :size="scaledNumber(textSize)"
      :height="textHeight"
    ></vgl-text-geometry>
    <vgl-text-geometry
      name="number2"
      font="../../resources/fonts/helvetiker_regular.typeface.json"
      text="2"
      :size="scaledNumber(textSize)"
      :height="textHeight"
    ></vgl-text-geometry>
    <vgl-text-geometry
      name="number3"
      font="../../resources/fonts/helvetiker_regular.typeface.json"
      text="3"
      :size="scaledNumber(textSize)"
      :height="textHeight"
    ></vgl-text-geometry>
    <vgl-text-geometry
      name="number4"
      font="../../resources/fonts/helvetiker_regular.typeface.json"
      text="4"
      :size="scaledNumber(textSize)"
      :height="textHeight"
    ></vgl-text-geometry>
    <vgl-text-geometry
      name="number5"
      font="../../resources/fonts/helvetiker_regular.typeface.json"
      text="5"
      :size="scaledNumber(textSize)"
      :height="textHeight"
    ></vgl-text-geometry>
    <vgl-text-geometry
      name="number6"
      font="../../resources/fonts/helvetiker_regular.typeface.json"
      text="6"
      :size="scaledNumber(textSize)"
      :height="textHeight"
    ></vgl-text-geometry>
    <vgl-text-geometry
      name="number7"
      font="../../resources/fonts/helvetiker_regular.typeface.json"
      text="7"
      :size="scaledNumber(textSize)"
      :height="textHeight"
    ></vgl-text-geometry>
    <vgl-text-geometry
      name="number8"
      font="../../resources/fonts/helvetiker_regular.typeface.json"
      text="8"
      :size="scaledNumber(textSize)"
      :height="textHeight"
    ></vgl-text-geometry>
    <vgl-text-geometry
      name="number9"
      font="../../resources/fonts/helvetiker_regular.typeface.json"
      text="9"
      :size="scaledNumber(textSize)"
      :height="textHeight"
    ></vgl-text-geometry>
    <vgl-text-geometry
      name="commaText"
      font="../../resources/fonts/helvetiker_regular.typeface.json"
      text=","
      :size="scaledNumber(textSize)"
      :height="textHeight"
    ></vgl-text-geometry>
    <!--Used for measurements-->
  </span>
</template>

<script>
import * as VueGL from "vue-gl";
import shapeGeometry from "./shape-geometry";
export default {
  name: "shared-objects",
  inject: ["vglNamespace"],
  components: {
    "shape-geometry": shapeGeometry,
    "vgl-mesh-lambert-material": VueGL.VglMeshLambertMaterial,
    "vgl-mesh-basic-material": VueGL.VglMeshBasicMaterial,
    "vgl-plane-geometry": VueGL.VglPlaneGeometry,
    "vgl-circle-geometry": VueGL.VglCircleGeometry,
    "vgl-text-geometry": VueGL.VglTextGeometry,
    "vgl-cylinder-geometry": VueGL.VglCylinderGeometry
  },
  props: {
    scaledNumber: Function
  },
  mounted() {
    // TODO: This sets the proper graphics resolution. Not related to raycaster, but placed here so we don't have to create another component to access the renderer.
    this.$parent.vglNamespace.renderers[0].inst.setPixelRatio(
      window.devicePixelRatio
    );
    this.$parent.vglNamespace.materials["invisibleMaterial"].visible = false;
  },
  data() {
    return {
      textSize: 40,
      textHeight: "0",
      binData: {
        components: [
          {
            shape: [
              [-10, -11, [-10, -13]],
              [-10, 11, false],
              [-8, 13, [-10, 13]],
              [8, 13, false],
              [10, 11, [10, 13]],
              [10, -11, false],
              [8, -13, [10, -13]],
              [-8, -13, false]
            ],
            holes: [
              [
                [-6, -7, [-6, -8]],
                [-6, 7, false],
                [-5, 8, [-6, 8]],
                [-4, 7, [-4, 8]],
                [-4, -7, false],
                [-5, -8, [-4, -8]]
              ],
              [
                [-1, -7, [-1, -8]],
                [-1, 7, false],
                [0, 8, [-1, 8]],
                [1, 7, [1, 8]],
                [1, -7, false],
                [0, -8, [1, -8]]
              ],
              [
                [4, -7, [4, -8]],
                [4, 7, false],
                [5, 8, [4, 8]],
                [6, 7, [6, 8]],
                [6, -7, false],
                [5, -8, [6, -8]]
              ]
            ]
          },
          {
            shape: [
              [-10, 15, [-10, 14]],
              [-10, 16, false],
              [-9, 17, [-10, 17]],
              [9, 17, false],
              [10, 16, [10, 17]],
              [10, 15, false],
              [9, 14, [10, 14]],
              [-9, 14, false]
            ],
            holes: []
          }
        ]
      },
      widthBoxData: {
        components: [
          {
            shape: [
              [0, 15, [0, 0]],
              [-25, 35, false],
              [0, 55, false],
              [15, 70, [0, 70]],
              [115, 70, false],
              [130, 55, [130, 70]],
              [155, 35, false],
              [130, 15, false],
              [115, 0, [130, 0]],
              [15, 0, false]
            ],
            holes: []
          }
        ]
      },
      heightBoxData: {
        components: [
          {
            shape: [
              [0, 15, [0, 0]],
              [0, 55, false],
              [15, 70, [0, 70]],
              [45, 70, false],
              [65, 95, false],
              [85, 70, false],
              [115, 70, false],
              [130, 55, [130, 70]],
              [130, 15, false],
              [115, 0, [130, 0]],
              [85, 0, false],
              [65, -25, false],
              [45, 0, false],
              [15, 0, false]
            ],
            holes: []
          }
        ]
      },
      cabineoData: {
        components: [
          {
            shape: [
              [0, -5, false],
              [0, 5, false],
              [14, 5, false],
              [19, 0, [19, 5]],
              [14, -5, [19, -5]]
            ],
            holes: []
          }
        ]
      }
    };
  },
  computed: {
    doorHandleData: function() {
      var handleRadius = 15;
      var extraOffset = 5;
      var radiusPlusOffset = handleRadius + extraOffset;
      return {
        components: [
          {
            shape: [
              [radiusPlusOffset, -radiusPlusOffset, false],
              [radiusPlusOffset, radiusPlusOffset, false],
              [-radiusPlusOffset, radiusPlusOffset, false],
              [-radiusPlusOffset, -radiusPlusOffset, false],
              [radiusPlusOffset, -radiusPlusOffset, false]
            ],
            holes: [
              [
                [handleRadius, 0, false],
                [0, handleRadius, [handleRadius, handleRadius]],
                [-handleRadius, 0, [-handleRadius, handleRadius]],
                [0, -handleRadius, [-handleRadius, -handleRadius]],
                [handleRadius, 0, [handleRadius, -handleRadius]]
              ]
            ]
          }
        ]
      };
    }
  }
};
</script>
