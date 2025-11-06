<template>
  <div class="control-panel">
    <div class="control-panel-content full-size">
      <div class="options-toggle-button-container show-on-small-screens">
        <button v-on:click="showSettings= !showSettings" class="btn-invisible">
          <icon v-if="showSettings" fixed-width icon="chevron-down" />
          <icon v-if="!showSettings" fixed-width icon="chevron-up" />
        </button>
      </div>
      <div class="measurements-section">
        <div class="third right measurement-margin">
          <icon icon="arrows-alt-v"></icon>
          {{(bookcase.height/10).toFixed(1).replace('.', ',')}} cm
        </div>
        <div class="third left measurement-margin">
          <icon icon="arrows-alt-h"></icon>
          {{(bookcase.width/10).toFixed(1).replace('.', ',')}} cm
        </div>
        <div class="third right center">
          <bookcase-price :price="bookcase.price"></bookcase-price>
        </div>
      </div>
      <div v-if="bookcase.lockedForEditing">
        <button
          v-on:click="copyBookcase()"
          type="button"
          class="btn"
          style="width: 120px; float:right"
        >
          <icon fixed-width icon="plus" class="fa-lg" />Kopiér
        </button>
        Reolen er allerede bestilt og er derfor skrivebeskyttet.
      </div>
      <div v-if="showSettings">
        <div class="control-panel-section">
          <input
            id="bookcaseNameField"
            type="text"
            placeholder="Reolens navn"
            v-model="bookcase.name"
            class="bookcase-name-field"
          />
        </div>
        <div class="control-panel-section">
          <div class="container">
            <div class="row">
              <div
                class="col-sm-6 col-xs-12"
                style="font-weight: bold; text-align: center"
                v-for="material in availableMaterials"
                :key="material.id"
              >
                <div
                  :style="getBackgroundAttribute(material.surfaceTextureLocation)"
                  :class="selectedMaterialId===material.id ? 'material-image selected' : 'material-image'"
                  v-on:click="setMaterial(material.id)"
                >
                  <div
                    :class="material.name ==='Sort' ? 'material-image-text white-text' : 'material-image-text'"
                  >{{material.name}}</div>
                </div>
                <div v-if="material.type === 0">
                  <div class="material-tooltip" id="varnish-tooltip">
                    <icon icon="question-circle" class="fa-2x" />
                  </div>
                  <b-tooltip target="varnish-tooltip">Naturligt smuk, lakeret birkekrydsfiner.</b-tooltip>
                </div>
                <div v-if="material.type === 4">
                  <div class="material-tooltip" id="laminate-tooltip">
                    <icon icon="question-circle" class="fa-2x" />
                  </div>
                  <b-tooltip target="laminate-tooltip">Birkerydsfiner belagt med hvid laminat</b-tooltip>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div v-if="showDoorMaterialOption">
          <div>Låger</div>
          <label class="b-contain" style="display: inline">
            <span>Grå</span>
            <input type="radio" v-model="bookcase.doorMaterial" :value="0" />
            <span class="b-input"></span>
          </label>
          <label class="b-contain" style="display: inline; margin-left: 10px">
            <span>Hvid (grå kant)</span>
            <input type="radio" v-model="bookcase.doorMaterial" :value="1" />
            <span class="b-input"></span>
          </label>
          <label class="b-contain" style="display: inline; margin-left: 10px">
            <span>Hvid</span>
            <input type="radio" v-model="bookcase.doorMaterial" :value="2" />
            <span class="b-input"></span>
          </label>
        </div>
        <div class="control-panel-section">
          <div>Dybde</div>
          <div class="row">
            <div class="col-2">
              <input
                type="number"
                style="width: 70px"
                v-model.number="selectedDepth"
                :min="depthMin"
                :max="depthMax"
                step="0.1"
                @blur="correctDepth"
              />
            </div>
            <div class="col-10">
              <div class="slider-container">
                <input
                  type="range"
                  :min="depthMin"
                  :max="depthMax"
                  v-model.number="selectedDepth"
                  class="slider plate-depth-slider"
                  id="plateDepthSlider"
                  step="0.1"
                  @mouseup="correctDepth"
                  @touchend="correctDepth"
                />
                <div class="sliderticks">
                  <p>15&nbsp;cm</p>
                  <p>30&nbsp;cm</p>
                  <p>45&nbsp;cm</p>
                  <p>60&nbsp;cm</p>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="control-panel-section">
          <label class="b-contain">
            <span>Vis redigeringssymboler</span>
            <input type="checkbox" v-model="showButtons" />
            <span class="b-input"></span>
          </label>
          <div>
            <button v-on:click="showBookcaseOptions" class="btn">
              <icon fixed-width icon="cog" />Tilvalg
            </button>
          </div>
        </div>
      </div>
      <div class="options-toggle-button-container show-on-big-screens">
        <button v-on:click="showSettings= !showSettings" class="tiny-btn btn-invisible">
          <icon v-if="showSettings" fixed-width icon="chevron-up" />
          <icon v-if="!showSettings" fixed-width icon="chevron-down" />
        </button>
      </div>
    </div>
  </div>
</template>
<script>
import dataTools from "./tools/data-tools";
import bookcasePrice from "./bookcase-price";
import { bookcaseService } from "../../_services";
import { BTooltip } from "bootstrap-vue/esm/components/tooltip/tooltip";
export default {
  name: "bookcase-control-panel",
  components: {
    "bookcase-price": bookcasePrice,
    "b-tooltip": BTooltip
  },
  mixins: [dataTools],
  props: {
    currentState: Object,
    bookcase: Object,
    isLoggedIn: Boolean,
    editable: Boolean,
    showCompartmentButtons: Boolean
  },
  data() {
    return {
      materials: [],
      selectedMaterialId: null,
      selectedDepth: this.bookcase.plateDepth / 10,
      depthMin: 15,
      depthMax: 60,
      showMaterialsModal: false,
      showSettings: false,
      showButtons: this.showCompartmentButtons
    };
  },
  created() {
    bookcaseService.getMaterials().then(materials => {
      this.materials = materials;
      this.selectedMaterialId = this.bookcase.material.id;
    });
    if (window.innerWidth > 850) {
      this.showSettings = true;
    }
  },
  computed: {
    availableMaterials: function() {
      return this.materials.filter(
        material => material.type === 0 || material.type === 4
        // || material.type === 5
      );
    },
    showDoorMaterialOption: function() {
      return (
        this.bookcase.compartments.filter(compartment => compartment.hasDoor)
          .length > 0
      );
    }
  },
  methods: {
    showHideCompartmentButtons: function(event) {
      // event.preventDefault();
    },
    copyBookcase: function() {
      this.bookcase.id = null;
      this.bookcase.lockedForEditing = false;
      this.bookcase.name = null;
    },
    showHideMaterialsModal: function(shouldShow) {
      this.showMaterialsModal = shouldShow;
      this.$emit("setOrbitControlsDisabled", shouldShow);
    },
    correctDepth: function() {
      var depth = this.selectedDepth;
      if (depth < this.depthMin) {
        this.selectedDepth = this.depthMin;
        this.bookcase.plateDepth = this.depthMin * 10;
      } else if (depth > this.depthMax) {
        this.selectedDepth = this.depthMax;
        this.bookcase.plateDepth = this.depthMax * 10;
      } else {
        var floored = Math.floor(depth * 10) / 10;
        this.selectedDepth = floored;
        this.bookcase.plateDepth = floored * 10;
      }
    },
    setMaterial: function(id) {
      this.selectedMaterialId = id;
      this.showHideMaterialsModal(false);
    },
    getBackgroundAttribute: function(surfaceTextureLocation) {
      return (
        "background-image: url('" + surfaceTextureLocation.substr(2) + "');"
      );
    },
    showBookcaseOptions: function() {
      this.$emit("showBookcaseOptions");
    }
  },
  watch: {
    selectedMaterialId: function(newVal) {
      var material = this.getById(this.materials, newVal);
      this.bookcase.material = material;
    },
    enabled: function(newVal) {
      this.controls.enabled = newVal;
    },
    showButtons: function(newVal) {
      this.$emit("showHideCompartmentButtons", newVal);
    }
  }
};
</script>
<style>
.plate-depth-slider {
  width: calc(100%);
}

.logo-image {
  margin: 14px 0 0 14px;
}

.bookcase-name-field {
  width: 100%;
  height: 38px;
  padding: 0 5px;
}

.material-dropdown {
  margin-top: 10px;
  width: 100%;
}

/* The Close Button */
.close {
  color: #aaa;
  float: right;
  font-size: 28px;
  font-weight: bold;
}

.close:hover,
.close:focus {
  color: black;
  text-decoration: none;
  cursor: pointer;
}

.measurements-section {
  margin-top: 6px;
  text-align: center;
  font-size: 17px;
  overflow: auto;
}

.third {
  width: 33.3%;
}
.left {
  float: left;
}
.right {
  float: right;
}
.center {
  text-align: center;
}
.measurement-margin {
  margin-top: 2.5px;
}
.material-image {
  width: calc(100% - 35px);
  float: left;
  height: 40px;
  border: 2px solid lightgray;
  cursor: pointer;
  border-radius: 4px;
  margin: 5px;
}

.material-image.selected {
  border: 4px solid #2dac86;
}

.material-image-text {
  position: relative;
  top: 50%;
  transform: translateY(-50%);
  margin: auto;
}

.material-tooltip {
  float: right;
  margin-top: 9px;
  margin-right: -10px;
}
</style>
