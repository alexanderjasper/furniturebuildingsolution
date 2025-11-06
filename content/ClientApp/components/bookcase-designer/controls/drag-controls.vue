<template>
  <span>
    <span v-if="bookcase.compartmentsBefore(plate) && showMeasurements">
      <compartment-measurements
        v-for="compartment in bookcase.compartmentsBefore(plate)"
        :key="'compartmentMeasurement' + compartment.id"
        :id="compartment.id"
        :measurementType="measurementType"
        :measurement="getMeasurement(compartment)"
        :isBefore="true"
        :position="measurementBeforePosition(compartment)"
        :bookcase="bookcase"
      ></compartment-measurements>
    </span>
    <span v-if="bookcase.compartmentsAfter(plate) && showMeasurements">
      <compartment-measurements
        v-for="compartment in bookcase.compartmentsAfter(plate)"
        :key="'compartmentMeasurement' + compartment.id"
        :id="compartment.id"
        :measurementType="measurementType"
        :measurement="getMeasurement(compartment)"
        :isBefore="false"
        :position="measurementAfterPosition(compartment)"
        :bookcase="bookcase"
      ></compartment-measurements>
    </span>
    <span v-if="plate.draggable && editable">
      <vgl-mesh
        :name="'handle' + plate.id"
        geometry="dragHandle"
        material="blackMaterial"
        :position="topLeft"
        :rotation="'0 0 ' + (-Math.PI/4)"
      ></vgl-mesh>
      <vgl-mesh
        :name="'handle' + plate.id"
        geometry="dragHandle"
        material="blackMaterial"
        :position="topRight"
        :rotation="'0 0 ' + Math.PI/4"
      ></vgl-mesh>
      <vgl-mesh
        :name="'handle' + plate.id"
        geometry="dragHandle"
        material="blackMaterial"
        :position="bottomLeft"
        :rotation="'0 0 ' + Math.PI/4"
      ></vgl-mesh>
      <vgl-mesh
        :name="'handle' + plate.id"
        geometry="dragHandle"
        material="blackMaterial"
        :position="bottomRight"
        :rotation="'0 0 ' + (-Math.PI/4)"
      ></vgl-mesh>
      <vgl-mesh
        :name="'handle' + plate.id"
        geometry="dragHandleInvisiblePart"
        material="invisibleMaterial"
        :position="middle"
      ></vgl-mesh>
      <span v-if="showDeleteButton && bookcase.isDeleteable(plate)">
        <vgl-mesh
          :name="'0DeleteButton' + plate.id"
          geometry="binGeometry"
          material="blackMaterial"
          :position="deleteButtonPosition(false)"
          scale="0.0005 0.0005 0.0005"
        ></vgl-mesh>
        <vgl-mesh
          :name="'1DeleteButton' + plate.id"
          geometry="dragHandleInvisiblePart"
          material="invisibleMaterial"
          :position="deleteButtonPosition(true)"
        ></vgl-mesh>
      </span>
    </span>
  </span>
</template>
<script>
import * as VueGL from "vue-gl";
import dataTools from "../tools/data-tools";
import compartmentMeasurements from "../compartment/compartment-measurements";
import * as THREE from "three";
export default {
  inject: ["vglNamespace"],
  mixins: [dataTools],
  components: {
    "compartment-measurements": compartmentMeasurements,
    "vgl-mesh": VueGL.VglMesh,
  },
  props: {
    cmr: String,
    plate: Object,
    position: Object,
    horizontal: Boolean,
    bookcase: Object,
    editable: Boolean,
    showMeasurements: Boolean,
    showDeleteButton: Boolean,
  },
  data() {
    return {
      plane: new THREE.Plane(
        new THREE.Vector3(0, 0, 1),
        this.plateDepth / (this.scale * 2)
      ),
      domElement: null,
      camera: this.vglNamespace.object3ds[this.cmr],
      raycaster: new THREE.Raycaster(),
      mouse: new THREE.Vector2(),
      offset: new THREE.Vector3(),
      intersection: new THREE.Vector3(),
      worldPosition: new THREE.Vector3(),
      inverseMatrix: new THREE.Matrix4(),
      selected: null,
      hovered: null,
      arrowOffset: 40,
      dragStartCoordinate: null,
    };
  },
  computed: {
    plateDepth: function () {
      return this.bookcase.plateDepth;
    },
    measurementType: function () {
      return this.plate.isHorizontal ? "height" : "width";
    },
    objects: function () {
      return [this.vglNamespace.object3ds["handle" + this.plate.id]];
    },
    xDivide: function () {
      return this.horizontal ? 3 : 1;
    },
    yDivide: function () {
      return this.horizontal ? 1 : 3;
    },
    zOffset: function () {
      return this.scaledNumber(this.plateDepth + 2);
    },
    middle: function () {
      return this.position.x + " " + this.position.y + " " + this.zOffset;
    },
    topLeft: function () {
      return (
        this.position.x -
        this.scaledNumber(this.arrowOffset / this.xDivide) +
        " " +
        (this.position.y + this.scaledNumber(this.arrowOffset / this.yDivide)) +
        " " +
        this.zOffset
      );
    },
    topRight: function () {
      return (
        this.position.x +
        this.scaledNumber(this.arrowOffset / this.xDivide) +
        " " +
        (this.position.y + this.scaledNumber(this.arrowOffset / this.yDivide)) +
        " " +
        this.zOffset
      );
    },
    bottomLeft: function () {
      return (
        this.position.x -
        this.scaledNumber(this.arrowOffset / this.xDivide) +
        " " +
        (this.position.y - this.scaledNumber(this.arrowOffset / this.yDivide)) +
        " " +
        this.zOffset
      );
    },
    bottomRight: function () {
      return (
        this.position.x +
        this.scaledNumber(this.arrowOffset / this.xDivide) +
        " " +
        (this.position.y - this.scaledNumber(this.arrowOffset / this.yDivide)) +
        " " +
        this.zOffset
      );
    },
    dragPosition: function () {
      if (this.plate.isHorizontal) {
        return this.position.y;
      } else {
        return this.position.x;
      }
    },
  },
  methods: {
    getMeasurement: function (compartment) {
      return (
        (this.plate.isHorizontal
          ? compartment.dimensions.height
          : compartment.dimensions.width) - this.bookcase.material.thickness
      );
    },
    deleteButtonPosition: function (invisiblePart) {
      if (this.plate.isHorizontal) {
        return (
          this.position.x +
          this.scaledNumber(100) +
          (invisiblePart ? this.scaledNumber(30) : 0) +
          " " +
          this.position.y +
          " " +
          this.zOffset
        );
      } else {
        return (
          this.position.x +
          " " +
          (this.position.y -
            this.scaledNumber(110) +
            (invisiblePart ? this.scaledNumber(-10) : 0)) +
          " " +
          this.zOffset
        );
      }
    },
    measurementBeforePosition: function (compartment) {
      if (this.plate.isHorizontal) {
        return {
          x: this.scaledNumber(compartment.center.x - 65),
          y: this.position.y - this.scaledNumber(198),
          z: this.zOffset,
        };
      } else {
        return {
          x: this.position.x - this.scaledNumber(240),
          y: this.scaledNumber(compartment.center.y - 35),
          z: this.zOffset,
        };
      }
    },
    measurementAfterPosition: function (compartment) {
      if (this.plate.isHorizontal) {
        return {
          x: this.scaledNumber(compartment.center.x - 65),
          y: this.position.y + this.scaledNumber(130),
          z: this.zOffset,
        };
      } else {
        return {
          x: this.position.x + this.scaledNumber(110),
          y: this.scaledNumber(compartment.center.y - 35),
          z: this.zOffset,
        };
      }
    },
    movePlate: function (newPosition) {
      if (this.plate.isHorizontal) {
        this.bookcase.movePlate(this.plate, newPosition.y * this.scale, false);
      } else {
        this.bookcase.movePlate(this.plate, newPosition.x * this.scale, false);
      }
      this.vglNamespace.update();
    },
    activate: function () {
      this.domElement.addEventListener(
        "mousemove",
        this.onDocumentMouseMove,
        false
      );
      this.domElement.addEventListener(
        "mousedown",
        this.onDocumentMouseDown,
        false
      );
      this.domElement.addEventListener(
        "mouseup",
        this.onDocumentMouseCancel,
        false
      );
      this.domElement.addEventListener(
        "mouseleave",
        this.onDocumentMouseCancel,
        false
      );
      this.domElement.addEventListener(
        "touchmove",
        this.onDocumentTouchMove,
        false
      );
      this.domElement.addEventListener(
        "touchstart",
        this.onDocumentTouchStart,
        false
      );
      this.domElement.addEventListener(
        "touchend",
        this.onDocumentTouchEnd,
        false
      );
    },
    deactivate: function () {
      this.domElement.removeEventListener(
        "mousemove",
        this.onDocumentMouseMove,
        false
      );
      this.domElement.removeEventListener(
        "mousedown",
        this.onDocumentMouseDown,
        false
      );
      this.domElement.removeEventListener(
        "mouseup",
        this.onDocumentMouseCancel,
        false
      );
      this.domElement.removeEventListener(
        "mouseleave",
        this.onDocumentMouseCancel,
        false
      );
      this.domElement.removeEventListener(
        "touchmove",
        this.onDocumentTouchMove,
        false
      );
      this.domElement.removeEventListener(
        "touchstart",
        this.onDocumentTouchStart,
        false
      );
      this.domElement.removeEventListener(
        "touchend",
        this.onDocumentTouchEnd,
        false
      );
    },
    onDocumentMouseMove: function (event) {
      var rect = this.domElement.getBoundingClientRect();

      this.mouse.x = ((event.clientX - rect.left) / rect.width) * 2 - 1;
      this.mouse.y = -((event.clientY - rect.top) / rect.height) * 2 + 1;

      this.raycaster.setFromCamera(this.mouse, this.camera);

      if (this.selected) {
        event.preventDefault();
        if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
          this.movePlate(
            this.intersection.sub(this.offset).applyMatrix4(this.inverseMatrix)
          );
        }
        // scope.dispatchEvent({ type: "drag", object: this.selected });
        return;
      }

      this.raycaster.setFromCamera(this.mouse, this.camera);

      var intersects = this.raycaster.intersectObjects(this.objects);

      if (intersects.length > 0) {
        var object = intersects[0].object;
        this.plane.setFromNormalAndCoplanarPoint(
          this.camera.getWorldDirection(this.plane.normal),
          this.worldPosition.setFromMatrixPosition(object.matrixWorld)
        );
        if (this.hovered !== object) {
          // this.$emit("setSelected", true);
          // scope.dispatchEvent({ type: "hoveron", object: object });
          this.domElement.style.cursor = "pointer";
          this.hovered = object;
        }
      } else {
        if (this.hovered !== null) {
          // this.$emit("setSelected", false);
          // scope.dispatchEvent({ type: "hoveroff", object: this.hovered });

          this.domElement.style.cursor = "auto";
          this.hovered = null;
        }
      }
    },
    onDocumentMouseDown: function (event) {
      if (
        event.target.closest(".control-panel-content") ||
        event.target.closest(".modal-content")
      ) {
        return;
      }
      this.raycaster.setFromCamera(this.mouse, this.camera);
      var intersects = this.raycaster.intersectObjects(this.objects);
      if (intersects.length > 0) {
        this.startedDragging();
        this.selected = intersects[0].object;
        if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
          this.inverseMatrix.getInverse(this.selected.parent.matrixWorld);
          this.offset
            .copy(this.intersection)
            .sub(
              this.worldPosition.setFromMatrixPosition(
                this.selected.matrixWorld
              )
            );
        }
        this.domElement.style.cursor = "move";
        // scope.dispatchEvent({ type: "dragstart", object: this.selected });
      }
    },
    onDocumentMouseCancel: function (event) {
      // event.preventDefault();
      this.stoppedDragging();
      if (this.selected) {
        // scope.dispatchEvent({ type: "dragend", object: this.selected });
        this.selected = null;
      }
      this.domElement.style.cursor = this.hovered ? "pointer" : "auto";
    },
    onDocumentTouchMove: function (inputEvent) {
      var event = inputEvent.changedTouches[0];
      var rect = this.domElement.getBoundingClientRect();
      this.mouse.x = ((event.clientX - rect.left) / rect.width) * 2 - 1;
      this.mouse.y = -((event.clientY - rect.top) / rect.height) * 2 + 1;
      this.raycaster.setFromCamera(this.mouse, this.camera);
      if (this.selected) {
        inputEvent.preventDefault();
        if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
          this.movePlate(
            this.intersection.sub(this.offset).applyMatrix4(this.inverseMatrix)
          );
        }
        // scope.dispatchEvent({ type: "drag", object: this.selected });
        return;
      }
    },
    onDocumentTouchStart: function (event) {
      if (
        event.target.closest(".control-panel-content") ||
        event.target.closest(".modal-content")
      ) {
        return;
      }
      event = event.changedTouches[0];
      var rect = this.domElement.getBoundingClientRect();
      this.mouse.x = ((event.clientX - rect.left) / rect.width) * 2 - 1;
      this.mouse.y = -((event.clientY - rect.top) / rect.height) * 2 + 1;
      this.raycaster.setFromCamera(this.mouse, this.camera);
      var intersects = this.raycaster.intersectObjects(this.objects);
      if (intersects.length > 0) {
        this.startedDragging();
        this.selected = intersects[0].object;
        this.plane.setFromNormalAndCoplanarPoint(
          this.camera.getWorldDirection(this.plane.normal),
          this.worldPosition.setFromMatrixPosition(this.selected.matrixWorld)
        );
        if (this.raycaster.ray.intersectPlane(this.plane, this.intersection)) {
          this.inverseMatrix.getInverse(this.selected.parent.matrixWorld);
          this.offset
            .copy(this.intersection)
            .sub(
              this.worldPosition.setFromMatrixPosition(
                this.selected.matrixWorld
              )
            );
        }
        this.domElement.style.cursor = "move";
        // scope.dispatchEvent({ type: "dragstart", object: this.selected });
      }
    },
    onDocumentTouchEnd: function (event) {
      // event.preventDefault();
      this.stoppedDragging();
      if (this.selected) {
        // scope.dispatchEvent({ type: "dragend", object: this.selected });
        this.selected = null;
      }
      this.domElement.style.cursor = "auto";
    },
    stoppedDragging: function () {
      this.$emit(
        "stoppedDraggingPlate",
        this.dragStartCoordinate &&
          this.dragPosition !== this.dragStartCoordinate
      );
      this.dragStartCoordinate = this.dragPosition;
    },
    startedDragging: function () {
      this.$emit("draggingPlate");
      this.dragStartCoordinate = this.dragPosition;
    },
  },
  mounted() {
    this.domElement = document.getElementById("bookcaseDesignerContainer");
    if (this.plate.draggable) {
      this.activate();
    }
  },
  destroyed() {
    this.deactivate();
  },
};
</script>