<template>
  <span>
    <vgl-box-geometry
      :name="'invisibleGeo' + plate.id"
      :width="invisibleWidth"
      :height="invisibleHeight"
      :depth="plateDepth/scale"
    ></vgl-box-geometry>

    <vgl-texture
      :src="bookcase.material.surfaceTextureLocation"
      :name="'plywoodSurfaceTexture' + plate.id"
      wrap-s="repeat"
      :repeat="plate.length/400 + ' 1'"
    ></vgl-texture>
    <vgl-mesh-lambert-material
      :name="'plywoodSurfaceMaterial' + plate.id"
      :map="'plywoodSurfaceTexture' + plate.id"
    ></vgl-mesh-lambert-material>

    <vgl-mesh
      :name="'1plate' + plate.id"
      :geometry="'invisibleGeo' + plate.id"
      material="invisibleMaterial"
      :position="`${position.x} ${position.y} ${position.z}`"
    ></vgl-mesh>
    <vgl-plane-geometry :name="'plateFrontGeo' + plate.id" :width="thickness" :height="length"></vgl-plane-geometry>
    <vgl-plane-geometry :name="'plateEndGeo' + plate.id" :width="thickness" :height="depth"></vgl-plane-geometry>
    <vgl-plane-geometry :name="'plateSurfaceGeo' + plate.id" :width="length" :height="depth"></vgl-plane-geometry>
    <!--Front-->
    <vgl-mesh
      :name="'0plate' + plate.id"
      :geometry="'plateFrontGeo' + plate.id"
      :material="shouldHighlight ? 'highlightMaterial' : 'sideMaterial'"
      :position="frontPosition"
      :rotation="frontRotation"
    ></vgl-mesh>
    <vgl-mesh
      v-if="!plate.innerPlateStart"
      :name="'1plate' + plate.id"
      :geometry="'plateEndGeo' + plate.id"
      :material="shouldHighlight ? 'highlightMaterial' : 'sideMaterial'"
      :position="end1Position"
      :rotation="end1Rotation"
    ></vgl-mesh>
    <vgl-mesh
      v-if="!plate.innerPlateEnd"
      :name="'2plate' + plate.id"
      :geometry="'plateEndGeo' + plate.id"
      :material="shouldHighlight ? 'highlightMaterial' : 'sideMaterial'"
      :position="end2Position"
      :rotation="end2Rotation"
    ></vgl-mesh>
    <vgl-mesh
      :name="'3plate' + plate.id"
      :geometry="'plateSurfaceGeo' + plate.id"
      :material="shouldHighlight ? 'highlightMaterial' : ('plywoodSurfaceMaterial' + plate.id)"
      :position="surface1Position"
      :rotation="surface1Rotation"
    ></vgl-mesh>
    <vgl-mesh
      :name="'4plate' + plate.id"
      :geometry="'plateSurfaceGeo' + plate.id"
      :material="shouldHighlight ? 'highlightMaterial' : ('plywoodSurfaceMaterial' + plate.id)"
      :position="surface2Position"
      :rotation="surface2Rotation"
    ></vgl-mesh>
    <cabineo-joints
      :plate="plate"
      :bookcase="bookcase"
      :startX="plate.start.x"
      :startY="plate.start.y"
      :endX="plate.end.x"
      :endY="plate.end.y"
    ></cabineo-joints>
    <drag-controls
      v-if="plate.isSelected || (plate.parentCompartment && plate.parentCompartment.id === selectedCompartmentId)"
      :cmr="camera"
      :plate="plate"
      :position="position"
      :horizontal="plate.isHorizontal"
      :bookcase="bookcase"
      :editable="editable"
      :showMeasurements="true"
      :showDeleteButton="true"
      @draggingPlate="draggingPlate"
      @stoppedDraggingPlate="stoppedDraggingPlate"
    ></drag-controls>

    <bookcase-support
      v-if="showRearSupport"
      :start="plate.start"
      :end="plate.end"
      zPlacement="rear"
      :id="plate.id + 'plateRear'"
      :bookcase="bookcase"
    ></bookcase-support>
    <bookcase-support
      v-if="showFrontSupport"
      :start="plate.start"
      :end="plate.end"
      zPlacement="front"
      :id="plate.id + 'plateFront'"
      :bookcase="bookcase"
    ></bookcase-support>
  </span>
</template>
<script>
import * as VueGL from "vue-gl";
import dataTools from "./tools/data-tools";
import dragControls from "./controls/drag-controls";
import cabineoJoints from "./cabineo-joints";
import bookcaseSupport from "./compartment/bookcase-support";
export default {
  name: "bookcase-plate",
  inject: ["vglNamespace"],
  mixins: [dataTools],
  components: {
    "drag-controls": dragControls,
    "cabineo-joints": cabineoJoints,
    "bookcase-support": bookcaseSupport,
    //VueGL
    "vgl-box-geometry": VueGL.VglBoxGeometry,
    "vgl-texture": VueGL.VglTexture,
    "vgl-mesh-lambert-material": VueGL.VglMeshLambertMaterial,
    "vgl-mesh": VueGL.VglMesh,
    "vgl-plane-geometry": VueGL.VglPlaneGeometry
  },
  props: {
    camera: String,
    plate: Object,
    bookcase: Object,
    editable: Boolean,
    compartmentIsSelected: Boolean,
    selectedCompartmentId: Number
  },
  data() {
    return {
      invisiblePlateWidth: 80,
      gap: 0
    };
  },
  computed: {
    plateWidth: function() {
      return this.bookcase.material.thickness;
    },
    plateDepth: function() {
      return this.bookcase.plateDepth;
    },
    shouldHighlight: function() {
      return (
        this.editable && (this.plate.isSelected || this.compartmentIsSelected)
      );
    },
    invisibleWidth: function() {
      if (this.plate.isHorizontal) {
        return this.width;
      } else {
        return this.scaledNumber(this.invisiblePlateWidth);
      }
    },
    invisibleHeight: function() {
      if (this.plate.isHorizontal) {
        return this.scaledNumber(this.invisiblePlateWidth);
      } else {
        return this.height;
      }
    },
    width: function() {
      if (this.plate.isHorizontal) {
        return this.scaledNumber(this.plate.length);
      } else {
        return this.scaledNumber(this.plateWidth);
      }
    },
    height: function() {
      if (this.plate.isHorizontal) {
        return this.scaledNumber(this.plateWidth);
      } else {
        return this.scaledNumber(this.plate.length);
      }
    },
    length: function() {
      return this.scaledNumber(this.plate.length);
    },
    depth: function() {
      return this.scaledNumber(
        this.plateDepth - (this.plate.parentCompartment ? 16 : 0)
      );
    },
    thickness: function() {
      return this.scaledNumber(this.plateWidth);
    },
    frontPosition: function() {
      var plate = this.plate;
      return this.getVectorString({
        x: this.position.x,
        y: this.position.y,
        z: this.position.z + this.depth / 2
      });
    },
    position: function() {
      var plate = this.plate;
      var shiftIncrement = 0;
      if (plate.innerPlateStart) shiftIncrement += 1;
      else shiftIncrement -= 1;
      if (plate.innerPlateEnd) shiftIncrement -= 1;
      else shiftIncrement += 1;

      var shift = this.scaledNumber((shiftIncrement * this.plateWidth) / 4);
      var position = {
        x: this.scaledNumber((plate.start.x + plate.end.x) / 2),
        y: this.scaledNumber((plate.start.y + plate.end.y) / 2),
        z: this.scaledNumber(
          this.plateDepth / 2 - (this.plate.parentCompartment ? 16 : 0)
        )
      };
      if (plate.isHorizontal) {
        position.x += shift;
      } else {
        position.y += shift;
      }
      return position;
    },
    frontRotation: function() {
      var rotation = { x: 0, y: 0, z: 0 };
      if (this.plate.isHorizontal) {
        rotation.z = Math.PI / 2;
      }
      return this.getVectorString(rotation);
    },
    end1Position: function() {
      var plate = this.plate;
      if (plate.isHorizontal) {
        return this.getVectorString({
          x: this.position.x - this.scaledNumber(plate.length / 2),
          y: this.position.y,
          z: this.position.z
        });
      } else {
        return this.getVectorString({
          x: this.position.x,
          y: this.position.y - this.scaledNumber(plate.length / 2),
          z: this.position.z
        });
      }
    },
    end1Rotation: function() {
      var rotation = { x: Math.PI / 2, y: 0, z: 0 };
      if (this.plate.isHorizontal) {
        rotation.y = -Math.PI / 2;
      } else {
        rotation.x = -Math.PI / 2;
        rotation.y = Math.PI;
      }
      return this.getVectorString(rotation);
    },
    end2Position: function() {
      var plate = this.plate;
      if (plate.isHorizontal) {
        return this.getVectorString({
          x: this.position.x + this.scaledNumber(plate.length / 2),
          y: this.position.y,
          z: this.position.z
        });
      } else {
        return this.getVectorString({
          x: this.position.x,
          y: this.position.y + this.scaledNumber(plate.length / 2),
          z: this.position.z
        });
      }
    },
    end2Rotation: function() {
      var rotation = { x: Math.PI / 2, y: 0, z: 0 };
      if (this.plate.isHorizontal) {
        rotation.y = Math.PI / 2;
      } else {
        rotation.x = Math.PI / 2;
        rotation.y = -Math.PI;
      }
      return this.getVectorString(rotation);
    },
    surface1Position: function() {
      var plate = this.plate;
      if (plate.isHorizontal) {
        return this.getVectorString({
          x: this.position.x,
          y: this.position.y + this.scaledNumber(this.plateWidth / 2),
          z: this.position.z
        });
      } else {
        return this.getVectorString({
          x: this.position.x + this.scaledNumber(this.plateWidth / 2),
          y: this.position.y,
          z: this.position.z
        });
      }
    },
    surface1Rotation: function() {
      // TODO: Make proper random funtion
      // var rotate = Math.random() < 0.5;
      var rotate = false;
      var rotation = { x: 0, y: 0, z: 0 };
      if (this.plate.isHorizontal) {
        rotation.x = -Math.PI / 2;
        rotation.z = rotate ? Math.PI : 0;
      } else {
        rotation.x = rotate ? Math.PI / 2 : -Math.PI / 2;
        rotation.y = Math.PI / 2;
      }
      return this.getVectorString(rotation);
    },
    surface2Position: function() {
      var plate = this.plate;
      if (plate.isHorizontal) {
        return this.getVectorString({
          x: this.position.x,
          y: this.position.y - this.scaledNumber(this.plateWidth / 2),
          z: this.scaledNumber(this.plateDepth / 2)
        });
      } else {
        return this.getVectorString({
          x: this.position.x - this.scaledNumber(this.plateWidth / 2),
          y: this.position.y,
          z: this.scaledNumber(this.plateDepth / 2)
        });
      }
    },
    surface2Rotation: function() {
      var rotation = { x: 0, y: 0, z: 0 };
      if (this.plate.isHorizontal) {
        rotation.x = Math.PI / 2;
      } else {
        rotation.x = Math.PI / 2;
        rotation.y = -Math.PI / 2;
      }
      return this.getVectorString(rotation);
    },
    showRearSupport: function() {
      return (
        this.plate.parentCompartment &&
        this.plate.end.x - this.plate.start.x >
          this.bookcase.maxLengthWithoutSupport
      );
    },
    showFrontSupport: function() {
      //TODO: Hardcoded value
      return (
        this.plate.parentCompartment &&
        this.plate.end.x - this.plate.start.x > 815
      );
    }
  },
  methods: {
    draggingPlate: function() {
      this.$emit("draggingPlate");
    },
    stoppedDraggingPlate: function(coordinateChanged) {
      this.$emit("stoppedDraggingPlate", coordinateChanged);
    }
  }
};
</script>
