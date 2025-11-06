<template>
  <div id="bookcaseDesignerContainer">
    <vgl-renderer
      v-if="bookcase"
      scene="scn"
      camera="c"
      antialias
      alpha
      disable-stencil
      power-preference="high-performance"
      style="height: 100vh; overflow: hidden;"
      id="rendererContainer"
    >
      <vgl-scene name="scn" background="#ffffff">
        <shared-objects :scaledNumber="scaledNumber"></shared-objects>
        <vgl-texture
          :src="bookcase.material.sideTextureLocation"
          name="sideTexture"
          wrap-s="repeat"
        ></vgl-texture>
        <vgl-mesh-lambert-material name="sideMaterial" map="sideTexture"></vgl-mesh-lambert-material>
        <bookcase-plate
          v-for="plate in bookcase.plates"
          :key="'plate' + plate.id"
          :plate="plate"
          :bookcase="bookcase"
          :editable="editable"
          :compartmentIsSelected="plate.isSelected || activePlates.includes(plate.id)"
          :selectedCompartmentId="editCompartmentModalId"
          camera="c"
          @draggingPlate="draggingPlate"
          @stoppedDraggingPlate="stoppedDraggingPlate"
        ></bookcase-plate>
        <bookcase-compartment
          v-for="compartment in bookcase.compartments"
          :key="'compartment' + compartment.id"
          :compartment="compartment"
          :bookcaseData="bookcase"
          :showButtons="showCompartmentButtons"
          :editable="editable"
          :showControls="!selectedPlate && !showEditComapartmentModal && activeBaseDesign === null"
          :isSelected="editCompartmentModalId === compartment.id"
          camera="c"
          @draggingPlate="draggingPlate"
          @stoppedDraggingPlate="stoppedDraggingPlate"
        ></bookcase-compartment>
        <!--Background-->
        <vgl-plane-geometry
          name="backgroundPlane"
          :width="scaledNumber(roomWidth)"
          :height="scaledNumber(roomHeight)"
        ></vgl-plane-geometry>
        <vgl-plane-geometry
          name="sidePlane"
          :width="scaledNumber(roomDepth)"
          :height="scaledNumber(roomHeight)"
        ></vgl-plane-geometry>
        <vgl-mesh
          geometry="backgroundPlane"
          material="stdDarkGrey"
          :position="
            scaledNumber(xCenter) +
              ' ' +
              scaledNumber(roomHeight / 2 - plateWidth / 2) +
              ' ' +
              scaledNumber(0)
          "
        ></vgl-mesh>
        <!--Left-->
        <vgl-mesh
          geometry="sidePlane"
          material="stdGrey"
          :position="
            scaledNumber(xCenter + roomWidth / 2) +
              ' ' +
              scaledNumber(roomHeight / 2 - plateWidth / 2) +
              ' ' +
              scaledNumber(roomDepth / 2)
          "
          :rotation="'0 ' + -Math.PI / 2 + ' 0'"
        ></vgl-mesh>
        <!--Right-->
        <vgl-mesh
          geometry="sidePlane"
          material="stdGrey"
          :position="
            scaledNumber(xCenter - roomWidth / 2) +
              ' ' +
              scaledNumber(roomHeight / 2 - plateWidth / 2) +
              ' ' +
              scaledNumber(roomDepth / 2)
          "
          :rotation="'0 ' + Math.PI / 2 + ' 0'"
        ></vgl-mesh>
        <!--Ceiling-->
        <vgl-mesh
          geometry="floorPlane"
          material="stdOffWhite"
          :position="
            scaledNumber(xCenter) +
              ' ' +
              scaledNumber(roomHeight - plateWidth / 2) +
              ' ' +
              scaledNumber(roomDepth / 2)
          "
          :rotation="Math.PI / 2 + ' 0 0'"
        ></vgl-mesh>
        <!--Background-->
        <!--Floor-->
        <vgl-plane-geometry
          name="floorPlane"
          :width="scaledNumber(roomWidth)"
          :height="scaledNumber(roomDepth)"
        ></vgl-plane-geometry>
        <vgl-mesh
          geometry="floorPlane"
          material="stdOffWhite"
          :position="
            scaledNumber(xCenter) +
              ' ' +
              -scaledNumber(plateWidth) / 2 +
              ' ' +
              scaledNumber(roomDepth / 2)
          "
          :rotation="-Math.PI / 2 + ' 0 0'"
        ></vgl-mesh>
        <!--Floor-->
        <!--Lights-->
        <vgl-ambient-light color="#ffffff" intensity="0.95"></vgl-ambient-light>
        <vgl-directional-light position="0 1.8 10" intensity="0.25"></vgl-directional-light>
        <!--Lights-->
        <measurement-guide
          v-if="measurementGuide"
          :coordinates="measurementGuide"
          :bookcaseDepth="bookcase.plateDepth"
          :plateWidth="bookcase.material.thickness"
        ></measurement-guide>
      </vgl-scene>
      <vgl-perspective-camera name="c" near="0.03" far="5"></vgl-perspective-camera>
      <orbit-controls
        v-if="cameraPosition"
        :noRotate="dragging"
        :enabled="controlsEnabled && !disableOrbitControls"
        :position="cameraPosition"
        :bookcase="bookcase"
        camera="c"
      ></orbit-controls>
      <raycaster
        v-if="
          mounted &&
            !showMeasurementEditModal &&
            !showUndoRedoHelp &&
            activeBaseDesign === null
        "
        camera="c"
        @click="raycasterClicked"
        :dragging="dragging"
      ></raycaster>
    </vgl-renderer>
    <measurement-edit-modal
      v-if="showMeasurementEditModal && editable"
      :bookcase="bookcase"
      :compartment="measurementEditCompartment"
      :plate="measurementEditPlate"
      @close="hideMeasurementEditModal"
      @triggerHistoryVersion="triggerHistoryVersion"
    ></measurement-edit-modal>
    <bookcase-control-panel
      v-if="activeBaseDesign === null"
      v-show="!showEditComapartmentModal"
      :editable="editable"
      :isLoggedIn="isLoggedIn"
      :currentState="currentState"
      :bookcase="bookcase"
      :showCompartmentButtons="showCompartmentButtons"
      @setOrbitControlsDisabled="setOrbitControlsDisabled"
      @showHideCompartmentButtons="showHideCompartmentButtons"
      @showBookcaseOptions="showHideBookcaseOptions(true)"
    ></bookcase-control-panel>
    <base-design-configurator
      v-if="activeBaseDesign !== null"
      :design="activeBaseDesign"
      @confirm="setBookcaseFromBaseDesign"
      @submit="confirmBaseDesign"
      @setMeasurementGuide="setMeasurementGuide"
    ></base-design-configurator>
    <compartment-edit-modal
      v-if="showEditComapartmentModal && editable"
      :compartment="bookcase.compartmentById(editCompartmentModalId)"
      :bookcase="bookcase"
      @editCompartment="editCompartment"
      @close="hideEditCompartmentModal"
      @triggerHistoryVersion="triggerHistoryVersion"
    ></compartment-edit-modal>
    <require-login-modal
      v-if="showRequireLoginModal"
      @close="showRequireLoginModal = false"
      promptText="Du skal være logget ind for at gemme din reol eller føje den til indkøbskurven."
    ></require-login-modal>
    <exit-save-modal v-if="showExitSaveModal" @exit="confirmExit"></exit-save-modal>
    <bookcase-options
      :bookcase="bookcase"
      v-if="showBookcaseOptions"
      @close="showHideBookcaseOptions(false)"
    ></bookcase-options>
    <undo-redo-help
      @undo="undo"
      @redo="redo"
      @showHide="showHideUndoRedoHelp"
      @exitBookcaseDesigner="exit(false)"
      @saveBookcase="save"
      @buyBookcase="buyBookcase"
      :currentState="currentState"
      :showHelp="showUndoRedoHelp"
      :showUndo="undoAvailable"
      :showRedo="historyVersion < history.length - 1"
      :editable="editable"
    ></undo-redo-help>
  </div>
</template>

<script>
import * as VueGL from "vue-gl";
import BookcaseClass from "../../extensions/bookcaseClasses.js";
import orbitControls from "./controls/orbit-controls";
import raycaster from "./controls/raycaster";
import bookcaseCompartment from "./compartment/bookcase-compartment";
import bookcasePlate from "./bookcase-plate";
import measurementGuideComp from "./measurement-guide";
import bookcaseControlPanel from "./bookcase-control-panel";
import undoRedoHelp from "./undo-redo-help";
import baseDesignConfigurator from "./base-design-configurator";
import compartmentEditModal from "./modals/compartment-edit-modal";
import measurementEditModal from "./modals/measurement-edit-modal";
import sharedObjects from "./tools/shared-objects";
import dataTools from "./tools/data-tools";
import requireLoginModal from "./modals/require-login-modal";
import exitSaveModal from "./modals/exit-save-modal";
import bookcaseOptions from "./modals/bookcase-options";
import { bookcaseService, localstorageService } from "../../_services";
import { mapActions } from "vuex";
export default {
  name: "bookcase-designer",
  components: {
    "orbit-controls": orbitControls,
    raycaster: raycaster,
    "bookcase-compartment": bookcaseCompartment,
    "bookcase-plate": bookcasePlate,
    "measurement-guide": measurementGuideComp,
    "bookcase-control-panel": bookcaseControlPanel,
    "undo-redo-help": undoRedoHelp,
    "base-design-configurator": baseDesignConfigurator,
    "compartment-edit-modal": compartmentEditModal,
    "measurement-edit-modal": measurementEditModal,
    "shared-objects": sharedObjects,
    "require-login-modal": requireLoginModal,
    "exit-save-modal": exitSaveModal,
    "bookcase-options": bookcaseOptions,
    //VueGL
    "vgl-renderer": VueGL.VglRenderer,
    "vgl-scene": VueGL.VglScene,
    "vgl-texture": VueGL.VglTexture,
    "vgl-mesh-lambert-material": VueGL.VglMeshLambertMaterial,
    "vgl-plane-geometry": VueGL.VglPlaneGeometry,
    "vgl-mesh": VueGL.VglMesh,
    "vgl-ambient-light": VueGL.VglAmbientLight,
    "vgl-directional-light": VueGL.VglDirectionalLight,
    "vgl-perspective-camera": VueGL.VglPerspectiveCamera,
  },
  mixins: [dataTools],
  props: {
    initialData: Object,
    isLoggedIn: Boolean,
    baseDesign: Number,
  },
  data() {
    return {
      roomWidth: 7500,
      defaultRoomHeight: 2650,
      roomDepth: 5000,
      bookcase: null,
      currentState: {
        isSaving: false,
        addingToCart: false,
      },
      mounted: false,
      editCompartmentModalId: null,
      selectedPlate: null,
      dragging: false,
      measurementEditCompartment: null,
      measurementEditPlate: null,
      disableOrbitControls: false,
      activeBaseDesign: this.baseDesign,
      history: [],
      historyVersion: -1,
      showRequireLoginModal: false,
      showUndoRedoHelp: false,
      measurementGuide: null,
      cameraPosition: null,
      showCompartmentButtons: true,
      isDirty: false,
      showExitSaveModal: false,
      showBookcaseOptions: false,
    };
  },
  computed: {
    isActive: function () {
      return (
        !this.currentState.isSaving &&
        !this.currentState.addingToCart &&
        !this.editCompartmentModalId &&
        !this.measurementEditCompartment &&
        !this.measurementEditPlate &&
        !this.showRequireLoginModal &&
        !this.showUndoRedoHelp &&
        !this.showExitSaveModal &&
        !this.showBookcaseOptions
      );
    },
    roomHeight: function () {
      var minRoofGap = 20;
      if (
        this.bookcase &&
        this.bookcase.height &&
        this.bookcase.height > this.defaultRoomHeight - minRoofGap
      ) {
        return this.bookcase.height + minRoofGap;
      }
      return this.defaultRoomHeight;
    },
    undoAvailable: function () {
      return this.historyVersion > 0;
    },
    plateWidth: function () {
      return this.bookcase.material.thickness;
    },
    xCenter: function () {
      return 0;
    },
    showEditComapartmentModal: function () {
      return this.editCompartmentModalId != null;
    },
    showMeasurementEditModal: function () {
      return this.measurementEditCompartment && this.measurementEditPlate;
    },
    controlsEnabled: function () {
      return !this.showMeasurementEditModal && !this.showUndoRedoHelp;
    },

    orbitTarget: function () {
      var center = this.bookcase.center;
      return (
        this.scaledNumber(center.x) + " " + this.scaledNumber(center.y) + " 0"
      );
    },
    editable: function () {
      return this.bookcase && !this.bookcase.lockedForEditing;
    },
    activePlates: function () {
      if (!this.editCompartmentModalId) {
        return [];
      } else {
        let compartment = this.bookcase.compartmentById(
          this.editCompartmentModalId
        );
        return [
          compartment.top.id,
          compartment.bottom.id,
          compartment.left.id,
          compartment.right.id,
        ];
      }
    },
  },
  methods: {
    ...mapActions("alert", ["success"]),
    showHideBookcaseOptions(shouldShow) {
      this.showBookcaseOptions = shouldShow;
    },
    setMeasurementGuide(coordinates) {
      this.measurementGuide = coordinates;
    },
    showHideCompartmentButtons: function (value) {
      this.showCompartmentButtons = value;
    },
    setCameraPosition: function () {
      var center = this.bookcase.center;
      var width = this.bookcase.width + 200;
      var height = this.bookcase.height + 200;
      var cameraZ = Math.max(
        (width * 2500) / (window.innerWidth * window.devicePixelRatio),
        (height * 2500) / (window.innerHeight * window.devicePixelRatio)
      );
      if (window.innerWidth > 850) {
        this.cameraPosition = {
          x: this.scaledNumber(center.x),
          y: this.scaledNumber(center.y),
          z: this.scaledNumber(cameraZ),
        };
      } else {
        this.cameraPosition = {
          x: this.scaledNumber(center.x),
          y: this.scaledNumber(center.y * 0.8),
          z: this.scaledNumber(cameraZ),
        };
      }
    },
    setBookcaseFromBaseDesign(bookcase) {
      this.bookcase = new BookcaseClass(bookcase.material);
      this.bookcase.initialize(bookcase);
      this.triggerHistoryVersion(true);
      this.setCameraPosition();
    },
    confirmBaseDesign: function () {
      this.activeBaseDesign = null;
      this.measurementGuide = null;
      this.handleShowHelp();
    },
    draggingPlate: function () {
      this.dragging = true;
    },
    stoppedDraggingPlate: function (coordinateChanged) {
      this.dragging = false;
      if (coordinateChanged) {
        this.triggerHistoryVersion(true);
      }
    },
    triggerHistoryVersion: function (setDirty) {
      this.history = this.history.slice(0, this.historyVersion + 1);
      var bookcaseJson = this.bookcase.getJson;
      if (bookcaseJson && bookcaseJson != "null") {
        this.history.push(bookcaseJson);
        this.historyVersion = this.history.length - 1;
        if (setDirty) {
          this.isDirty = true;
        }
      }
    },
    undo: function () {
      if (this.undoAvailable) {
        this.pickHistoryVersion(this.historyVersion - 1);
      }
    },
    redo: function () {
      if (this.historyVersion < this.history.length - 1) {
        this.pickHistoryVersion(this.historyVersion + 1);
      }
    },
    pickHistoryVersion: function (versionToPick) {
      this.hideEditCompartmentModal();
      this.$emit("setLoading", true);
      setTimeout(this.performPickHistoryVersion, 0, versionToPick);
    },
    performPickHistoryVersion: function (versionToPick) {
      var bookcaseData = JSON.parse(this.history[versionToPick]);
      this.bookcase = new BookcaseClass(null);
      this.bookcase.initialize(bookcaseData);
      this.historyVersion = versionToPick;
      this.$emit("setLoading", false);
      this.isDirty = true;
    },
    save: function (shouldExit) {
      if (this.currentState.isSaving) {
        return;
      }
      this.editCompartmentModalId = null;
      this.selectedPlate = null;

      localstorageService.storeBookcase(this.bookcase);
      if (this.$store.state.account.status.loggedIn) {
        this.currentState.isSaving = true;
        bookcaseService.save(this.bookcase).then((bookcase) => {
          if (shouldExit) {
            this.exit(true);
          } else {
            this.bookcase = new BookcaseClass(null);
            this.bookcase.initialize(bookcase);
            this.getDisplacements();
            this.setSavingToFalse();
            this.isDirty = false;
          }
          this.success("Reolen er gemt");
        });
      } else {
        this.showExitSaveModal = false;
        this.showRequireLoginModal = true;
      }
    },
    buyBookcase: function () {
      if (this.currentState.addingToCart) {
        return;
      }
      this.editCompartmentModalId = null;
      this.selectedPlate = null;

      localstorageService.storeBookcase(this.bookcase);
      if (this.$store.state.account.status.loggedIn) {
        this.currentState.addingToCart = true;
        bookcaseService.save(this.bookcase).then((bookcase) => {
          var shoppingCart = localstorageService.addToCart(bookcase.id, null);
          var numberOfCartItems = localstorageService.getNumberOfCartItems();
          this.$store.commit("setNumberOfCartItems", numberOfCartItems);
          this.$router.push("/indkoebskurv");
        });
      } else {
        this.showRequireLoginModal = true;
      }
    },
    setSavingToFalse: function () {
      this.currentState.isSaving = false;
    },
    raycasterClicked: function (message) {
      if (message.substr(0, 10) === "EditButton") {
        var parseInt = Number.parseInt(message.replace("EditButton", ""));
        if (!isNaN(parseInt)) {
          this.editCompartmentModalId = parseInt;
          return;
        }
      } else if (message.substr(0, 4) === "door") {
        var parseInt = Number.parseInt(message.replace("door", ""));
        if (!isNaN(parseInt)) {
          if (this.selectedPlate) {
            var alreadySelectedPlate = this.bookcase.plateById(
              this.selectedPlate
            );
            if (alreadySelectedPlate) {
              alreadySelectedPlate.isSelected = false;
            }
            this.selectedPlate = null;
          } else if (!this.editCompartmentModalId) {
            this.editCompartmentModalId = parseInt;
          } else {
            this.editCompartmentModalId = null;
          }
          return;
        }
      } else if (
        message.substr(0, 5) === "plate" &&
        !this.showEditComapartmentModal
      ) {
        var parseInt = Number.parseInt(message.replace("plate", ""));
        if (!isNaN(parseInt)) {
          if (this.selectedPlate) {
            var alreadySelectedPlate = this.bookcase.plateById(
              this.selectedPlate
            );
            if (alreadySelectedPlate) {
              alreadySelectedPlate.isSelected = false;
            }
            this.selectedPlate = null;
          } else {
            var plate = this.bookcase.plateById(parseInt);
            if (
              plate.parentCompartment &&
              this.editCompartmentModalId !== plate.parentCompartment.id
            ) {
              this.editCompartmentModalId = plate.parentCompartment.id;
            } else {
              plate.isSelected = true;
              this.selectedPlate = parseInt;
            }
          }
          return;
        }
      } else if (message.substr(0, 12) === "DeleteButton") {
        var parseInt = Number.parseInt(message.replace("DeleteButton", ""));
        if (!isNaN(parseInt)) {
          var plate = this.bookcase.plateById(parseInt);
          if (this.bookcase.isDeleteable(plate)) {
            this.bookcase.deletePlate(plate);
            this.selectedPlate = null;
            this.triggerHistoryVersion(true);
          }
        }
      } else if (message.substr(0, 17) === "MeasurementButton") {
        var parseInt = Number.parseInt(
          message.replace("MeasurementButton", "")
        );
        if (!isNaN(parseInt) && this.selectedPlate) {
          this.measurementEditCompartment = this.bookcase.compartmentById(
            parseInt
          );
          var plate = this.bookcase.plateById(this.selectedPlate);
          if (plate.draggable) {
            this.measurementEditPlate = plate;
          }
        }
      } else if (message.substr(0, 6) !== "handle") {
        if (this.selectedPlate) {
          var plate = this.bookcase.plateById(this.selectedPlate);
          plate.isSelected = false;
          this.selectedPlate = null;
        }
        this.hideEditCompartmentModal();
      }
    },
    hideEditCompartmentModal: function () {
      if (this.editCompartmentModalId) {
        let compartment = this.bookcase.compartmentById(
          this.editCompartmentModalId
        );
        compartment.visualizeWalls = 0;
        compartment.visualizeShelves = 0;
        this.editCompartmentModalId = null;
      }
    },
    hideMeasurementEditModal: function () {
      this.measurementEditCompartment = null;
      this.measurementEditPlate = null;
    },
    editCompartment: function (editArguments) {
      if (this.dragging) return;

      this.hideEditCompartmentModal();

      if (editArguments.addWalls) {
        var newCompartments = this.bookcase.splitCompartment(
          editArguments.id,
          "horizontal",
          editArguments.numberOfWalls
        );
        if (editArguments.addShelves) {
          for (var i = 0; i < newCompartments.length; i++) {
            this.bookcase.splitCompartment(
              newCompartments[i].id,
              "vertical",
              editArguments.numberOfShelves
            );
          }
        }
      } else if (editArguments.addShelves) {
        this.bookcase.splitCompartment(
          editArguments.id,
          "vertical",
          editArguments.numberOfShelves
        );
      } else if (editArguments.addDoor) {
        this.bookcase.compartmentById(editArguments.id).addDoor();
      } else if (editArguments.addDrawer) {
        this.bookcase.compartmentById(editArguments.id).addDrawer();
      } else if (editArguments.model) {
        this.bookcase
          .compartmentById(editArguments.id)
          .addModel(editArguments.model);
      } else {
        return;
      }
      this.triggerHistoryVersion(true);
    },
    setOrbitControlsDisabled: function (shouldDisable) {
      this.disableOrbitControls = shouldDisable;
    },
    showHideUndoRedoHelp: function (shouldShow) {
      this.showUndoRedoHelp = shouldShow;
    },
    confirmExit: function (shouldSave) {
      if (shouldSave) {
        this.save(true);
      } else {
        this.exit(true);
      }
    },
    exit: function (force) {
      if (force || (!this.isDirty && this.bookcase.id)) {
        this.$emit("setLoading", true);
        localstorageService.storeBookcase(this.bookcase);
        this.$emit("exit");
      } else {
        this.showExitSaveModal = true;
      }
    },
    loadInitialData: function () {
      if (this.initialData != null) {
        this.bookcase = new BookcaseClass(this.initialData.material);
        this.bookcase.initialize(this.initialData);
        if (this.$store.state.account.status.loggedIn) {
          this.getDisplacements();
        }
        this.triggerHistoryVersion(false);
        this.setCameraPosition();
      } else {
        bookcaseService.getMaterials().then((materials) => {
          var material = materials[0];
          var bookcase = new BookcaseClass(null);
          bookcase.material = material;
          bookcase.maxLengthWithoutSupport = 500 + material.thickness;
          bookcase.plateDepth = 300;
          bookcase.lockedForEditing = false;
          this.bookcase = bookcase;
          this.setCameraPosition();
        });
      }
    },
    getDisplacements: function () {
      // Disabled for now:
      // var bookcaseId = this.bookcase.id;
      // bookcaseService.getDisplacements(bookcaseId).then(displacements => {
      //   this.bookcase.populateDisplacements(displacements);
      // });
    },
    handleShowHelp: function () {
      if (localstorageService.helpHasBeenShown()) {
        return;
      } else {
        this.showUndoRedoHelp = true;
        localstorageService.setHelpShown(true);
      }
    },
    addRemoveListeners: function (shouldAdd) {
      if (shouldAdd) {
        document.addEventListener(
          "keyup",
          (event) => {
            const keyName = event.key;
            if (keyName === "Delete" || keyName === "Backspace") {
              this.attemptDeletePlate();
            }
          },
          false
        );
      }
    },
    attemptDeletePlate: function () {
      if (this.isActive && this.selectedPlate) {
        var plate = this.bookcase.plateById(this.selectedPlate);
        if (this.bookcase.isDeleteable(plate)) {
          this.bookcase.deletePlate(plate);
          this.selectedPlate = null;
          this.triggerHistoryVersion(true);
        }
      }
    },
  },
  created() {
    this.loadInitialData();
    this.addRemoveListeners(true);
  },
  mounted() {
    this.mounted = true;
    this.$emit("showHideNavMenu", false);
  },
  destroyed() {
    this.$emit("showHideNavMenu", true);
    this.addRemoveListeners(false);
  },
};
</script>
