<template>
  <div>
    <div class="control-panel">
      <div class="control-panel-content">
        <div>
          <div v-if="design === 0 || design === 1" class="row">
            <div class="col-12" v-if="activeStep === 0">
              <div>
                <span class="base-design-header">Bredde</span>
                <div class="base-design-tooltip" id="box-width">
                  <icon icon="question-circle" class="base-design-tooltip-icon" />
                </div>
                <b-tooltip
                  target="box-width"
                  :placement="tooltipPlacement"
                >Reolens ydre bredde. Skal reolen stå imellem to genstande/vægge, anbefaler vi at måle afstanden og trække mindst 4 cm fra, da f.eks. vægge godt kan være skæve, især i ældre bygninger.</b-tooltip>
              </div>

              <div class="base-design-measurement-container">
                <div class="base-design-measurement">
                  <input
                    type="number"
                    class="base-design-input"
                    v-model.number="boxMeasurements.width"
                    :min="boxLimits.width.min"
                    :max="boxLimits.width.max"
                    @blur="recompute"
                    step="0.1"
                    lang="da-DK"
                  />
                </div>
                <div class="base-design-slider">
                  <input
                    type="range"
                    :min="boxLimits.width.min"
                    :max="boxLimits.width.max"
                    v-model.number="boxMeasurements.width"
                    class="slider"
                    step="0.1"
                    @mouseup="recompute"
                    @touchend="recompute"
                  />
                  <div class="sliderticks">
                    <p>{{boxLimits.width.min}} cm</p>
                    <p>{{ boxLimits.width.max }} cm</p>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-12" v-if="activeStep === 1">
              <div>
                <span class="base-design-header">Højde</span>
                <div class="base-design-tooltip" id="box-height">
                  <icon icon="question-circle" class="base-design-tooltip-icon" />
                </div>
                <b-tooltip
                  target="box-height"
                  :placement="tooltipPlacement"
                >Reolens ydre højde. Skal reolen gå op til en genstand/loftet, anbefaler vi at måle afstanden og trække mindst 4 cm fra, da gulve og lofter godt kan være skæve, især i ældre bygninger.</b-tooltip>
              </div>

              <div class="base-design-measurement-container">
                <div class="base-design-measurement">
                  <input
                    type="number"
                    class="base-design-input"
                    v-model.number="boxMeasurements.height"
                    :min="boxLimits.height.min"
                    :max="boxLimits.height.max"
                    @blur="recompute"
                    step="0.1"
                    lang="da-DK"
                  />
                </div>
                <div class="base-design-slider">
                  <input
                    type="range"
                    :min="boxLimits.height.min"
                    :max="boxLimits.height.max"
                    v-model.number="boxMeasurements.height"
                    class="slider"
                    step="0.1"
                    @mouseup="recompute"
                    @touchend="recompute"
                  />
                  <div class="sliderticks">
                    <p>{{boxLimits.height.min}} cm</p>
                    <p>{{ boxLimits.height.max }} cm</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div v-if="design === 2 || design === 3" class="row">
            <div class="col-12" v-if="activeStep === 0">
              <div>
                <span class="base-design-header">Højde</span>
                <div class="base-design-tooltip" id="ceiling-height">
                  <icon icon="question-circle" class="base-design-tooltip-icon" />
                </div>
                <b-tooltip
                  target="ceiling-height"
                  :placement="tooltipPlacement"
                >Reolens ydre højde. Skal reolen gå op til en genstand/loftet, anbefaler vi at måle afstanden og trække mindst 4 cm fra, da gulve og lofter godt kan være skæve, især i ældre bygninger.</b-tooltip>
              </div>

              <div class="base-design-measurement-container">
                <div class="base-design-measurement">
                  <input
                    type="number"
                    class="base-design-input"
                    v-model.number="doorMeasurements.ceilingHeight"
                    :min="doorLimits.ceilingHeight.min"
                    :max="doorLimits.ceilingHeight.max"
                    @blur="recompute"
                    step="0.1"
                    lang="da-DK"
                  />
                </div>
                <div class="base-design-slider">
                  <input
                    type="range"
                    :min="doorLimits.ceilingHeight.min"
                    :max="doorLimits.ceilingHeight.max"
                    v-model.number="doorMeasurements.ceilingHeight"
                    class="slider"
                    step="0.1"
                    @mouseup="recompute"
                    @touchend="recompute"
                  />
                  <div class="sliderticks">
                    <p>{{ doorLimits.ceilingHeight.min }} cm</p>
                    <p>{{doorLimits.ceilingHeight.max}} cm</p>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-12" v-if="activeStep === 1">
              <div>
                <span class="base-design-header">Højde, åbning</span>
                <div class="base-design-tooltip" id="door-height">
                  <icon icon="question-circle" class="base-design-tooltip-icon" />
                </div>
                <b-tooltip
                  target="door-height"
                  :placement="tooltipPlacement"
                >Frihøjde over gulvet i reolens åbning i midten. Hvis reolen står over en døråbning, kan denne f.eks. sættes til højden af døren inkl. dørkarmen plus 2 cm for at tage højde for evt. skævhed i gulvet og dørkarmen.</b-tooltip>
              </div>
              <div class="base-design-measurement-container">
                <div class="base-design-measurement">
                  <input
                    type="number"
                    class="base-design-input"
                    v-model.number="doorMeasurements.doorHeight"
                    :min="doorLimits.doorHeight.min"
                    :max="doorLimits.doorHeight.max"
                    @blur="recompute"
                    step="0.1"
                    lang="da-DK"
                  />
                </div>
                <div class="base-design-slider">
                  <input
                    type="range"
                    :min="doorLimits.doorHeight.min"
                    :max="doorLimits.doorHeight.max"
                    v-model.number="doorMeasurements.doorHeight"
                    class="slider"
                    step="0.1"
                    @mouseup="recompute"
                    @touchend="recompute"
                  />
                  <div class="sliderticks">
                    <p>{{doorLimits.doorHeight.min}} cm</p>
                    <p>{{doorLimits.doorHeight.max}} cm</p>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-12" v-if="activeStep === 2">
              <div>
                <span class="base-design-header">Bredde, åbning</span>
                <div class="base-design-tooltip" id="door-width">
                  <icon icon="question-circle" class="base-design-tooltip-icon" />
                </div>
                <b-tooltip
                  target="door-width"
                  :placement="tooltipPlacement"
                >Bredde af reolens åbning i midten. Hvis reolen står over en døråbning, kan denne f.eks. sættes til bredden af døren og dørkarmene plus 4 cm for at tage højde for evt. skævhed i dørkarmen.</b-tooltip>
              </div>

              <div class="base-design-measurement-container">
                <div class="base-design-measurement">
                  <input
                    type="number"
                    class="base-design-input"
                    v-model.number="doorMeasurements.doorWidth"
                    :min="doorLimits.doorWidth.min"
                    :max="doorLimits.doorWidth.max"
                    @blur="recompute"
                    step="0.1"
                    lang="da-DK"
                  />
                </div>
                <div class="base-design-slider">
                  <input
                    type="range"
                    :min="doorLimits.doorWidth.min"
                    :max="doorLimits.doorWidth.max"
                    v-model.number="doorMeasurements.doorWidth"
                    class="slider"
                    step="0.1"
                    @mouseup="recompute"
                    @touchend="recompute"
                  />
                  <div class="sliderticks">
                    <p>{{doorLimits.doorWidth.min}} cm</p>
                    <p>{{doorLimits.doorWidth.max}} cm</p>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-12" v-if="activeStep === 3">
              <div>
                <span class="base-design-header">Bredde, venstre del</span>
                <div class="base-design-tooltip" id="left-width">
                  <icon icon="question-circle" class="base-design-tooltip-icon" />
                </div>
                <b-tooltip
                  target="left-width"
                  :placement="tooltipPlacement"
                >Bredde af reolens venstre del. Hvis reolen står over en døråbning, kan denne f.eks. sættes til mængden af fri plads til venstre for dørkarmen minus 4 cm for at tage højde for evt. skævhed i dørkarm/væg.</b-tooltip>
              </div>
              <div class="base-design-measurement-container">
                <div class="base-design-measurement">
                  <input
                    type="number"
                    class="base-design-input"
                    v-model.number="doorMeasurements.leftWidth"
                    :min="doorLimits.leftWidth.min"
                    :max="doorLimits.leftWidth.max"
                    @blur="recompute"
                    step="0.1"
                    lang="da-DK"
                  />
                </div>
                <div class="base-design-slider">
                  <input
                    type="range"
                    :min="doorLimits.leftWidth.min"
                    :max="doorLimits.leftWidth.max"
                    v-model.number="doorMeasurements.leftWidth"
                    class="slider"
                    step="0.1"
                    @mouseup="recompute"
                    @touchend="recompute"
                  />
                  <div class="sliderticks">
                    <p>{{doorLimits.leftWidth.min}} cm</p>
                    <p>{{doorLimits.leftWidth.max}} cm</p>
                  </div>
                </div>
              </div>
            </div>
            <div class="col-12" v-if="activeStep === 4">
              <div>
                <span class="base-design-header">Bredde, højre del</span>
                <div class="base-design-tooltip" id="right-width">
                  <icon icon="question-circle" class="base-design-tooltip-icon" />
                </div>
                <b-tooltip
                  target="right-width"
                  :placement="tooltipPlacement"
                >Bredde af reolens højre del. Hvis reolen står over en døråbning, kan denne f.eks. sættes til mængden af fri plads til højre for dørkarmen minus 4 cm for at tage højde for evt. skævhed i dørkarm/væg.</b-tooltip>
              </div>
              <div class="base-design-measurement-container">
                <div class="base-design-measurement">
                  <input
                    type="number"
                    class="base-design-input"
                    v-model.number="doorMeasurements.rightWidth"
                    :min="doorLimits.rightWidth.min"
                    :max="doorLimits.rightWidth.max"
                    @blur="recompute"
                    step="0.1"
                    lang="da-DK"
                  />
                </div>
                <div class="base-design-slider">
                  <input
                    type="range"
                    :min="doorLimits.rightWidth.min"
                    :max="doorLimits.rightWidth.max"
                    v-model.number="doorMeasurements.rightWidth"
                    class="slider"
                    step="0.1"
                    @mouseup="recompute"
                    @touchend="recompute"
                  />
                  <div class="sliderticks">
                    <p>{{doorLimits.rightWidth.min}} cm</p>
                    <p>{{doorLimits.rightWidth.max}} cm</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="col-12" v-if="activeStep === numberOfSteps-2">
            <div>
              <span class="base-design-header">Antal rum</span>
              <div class="base-design-tooltip" id="density">
                <icon icon="question-circle" class="base-design-tooltip-icon" />
              </div>
              <b-tooltip
                target="density"
                :placement="tooltipPlacement"
              >Mængden af vægge og hylder i det automatisk genererede design.</b-tooltip>
            </div>
            <div class="base-design-measurement-container clearfix">
              <div class="base-design-slider" style="float: left">
                <input
                  type="range"
                  min="0"
                  max="100"
                  v-model.number="density"
                  class="slider"
                  step="1"
                  @mouseup="recompute"
                  @touchend="recompute"
                />
                <div class="sliderticks">
                  <p>Min.</p>
                  <p>Maks.</p>
                </div>
              </div>
              <div
                class="base-design-measurement"
                style="margin-left: 25px; position: relative; bottom: 2px"
              >
                <button v-on:click="recompute()" class="btn btn-small">
                  <icon fixed-width icon="redo" class="fa-lg" />
                </button>
              </div>
            </div>
          </div>
          <div class="col-12" v-if="activeStep === numberOfSteps-1">
            <div>
              <span class="base-design-header">Antal låger</span>
              <div class="base-design-tooltip" id="numberOfDoors">
                <icon icon="question-circle" class="base-design-tooltip-icon" />
              </div>
              <b-tooltip
                target="numberOfDoors"
                :placement="tooltipPlacement"
              >Antallet af låger i det automatisk genererede design.</b-tooltip>
            </div>
            <div class="base-design-measurement-container clearfix">
              <div class="base-design-slider" style="float: left">
                <input
                  type="range"
                  min="0"
                  max="1"
                  v-model.number="numberOfDoors"
                  class="slider"
                  step="0.01"
                  @mouseup="recompute"
                  @touchend="recompute"
                />
                <div class="sliderticks">
                  <p>Min.</p>
                  <p>Maks.</p>
                </div>
              </div>
              <div
                class="base-design-measurement"
                style="margin-left: 25px; position: relative; bottom: 2px"
              >
                <button v-on:click="recompute()" class="btn btn-small">
                  <icon fixed-width icon="redo" class="fa-lg" />
                </button>
              </div>
            </div>
          </div>
          <div>
            <div class="align-center"></div>
          </div>
          <div class="steps-buttons-container">
            <div>
              <span style="margin-right: 10px">
                <bookcase-price v-if="bookcase" :price="bookcase.price"></bookcase-price>
              </span>

              <button
                :style="activeStep === 0 ? 'visibility: hidden': ''"
                v-on:click="previousStep()"
                class="btn btn-small btn-secondary"
              >
                <icon fixed-width icon="arrow-left" />Forrige
              </button>
              <button
                v-if="activeStep < numberOfSteps - 1"
                v-on:click="nextStep()"
                class="btn btn-small"
              >
                Næste
                <icon fixed-width icon="arrow-right" />
              </button>
              <button
                v-if="activeStep === numberOfSteps - 1"
                v-on:click="submit()"
                class="btn btn-small"
              >
                Bekræft
                <icon fixed-width icon="check" />
              </button>
            </div>
          </div>
        </div>
      </div>
      <full-page-spinner v-if="calculating"></full-page-spinner>
    </div>
  </div>
</template>
<script>
import Vue from "vue";
import BookcaseClass from "../../extensions/bookcaseClasses.js";
import dataTools from "./tools/data-tools";
import bookcasePrice from "./bookcase-price";
import fullPageSpinner from "../full-page-spinner";
import { bookcaseService } from "../../_services";
import "bootstrap-vue/dist/bootstrap-vue.css";
import { BTooltip } from "bootstrap-vue/esm/components/tooltip/tooltip";
export default {
  name: "base-design-configurator",
  components: {
    "bookcase-price": bookcasePrice,
    "full-page-spinner": fullPageSpinner,
    "b-tooltip": BTooltip
  },
  mixins: [dataTools],
  props: {
    design: Number
  },
  data() {
    return {
      tooltipPlacement: "bottomright",
      calculating: false,
      bookcase: null,
      material: null,
      maxPlateLength: 2370,
      density: 0,
      numberOfDoors: 0,
      activeStep: 0,
      doorMeasurements: {
        doorHeight: 210,
        ceilingHeight: 260,
        doorWidth: 100,
        leftWidth: 100,
        rightWidth: 100
      },
      boxMeasurements: {
        width: 200,
        height: 200
      }
    };
  },
  mounted() {
    this.handleResize();
    window.addEventListener("resize", this.handleResize);
    bookcaseService.getMaterials().then(materials => {
      this.material = materials[0];
      this.recompute();
    });
    if (this.design === 1 || this.design === 3) {
      this.density = 75;
    }
    this.$emit("setMeasurementGuide", this.measurementGuide);
  },
  destroyed() {
    window.removeEventListener("resize", this.handleResize);
  },
  computed: {
    boxLimits() {
      return {
        width: {
          min: 20,
          max: Math.floor(
            (this.maxPlateLength * 3 + this.materialThickness * 4) / 10
          )
        },
        height: {
          min: 20,
          max: 300
        }
      };
    },
    doorLimits() {
      return {
        doorHeight: {
          min: 60,
          max: Math.floor(
            this.doorMeasurements.ceilingHeight -
              (this.minimumCompartmentSize + this.materialThickness) / 10
          )
        },
        ceilingHeight: {
          min: 100,
          max: 300
        },
        doorWidth: {
          min: 50,
          max: Math.floor(this.maxPlateLength / 10)
        },
        leftWidth: {
          min: 20,
          max: Math.floor(this.maxPlateLength / 10)
        },
        rightWidth: {
          min: 20,
          max: Math.floor(this.maxPlateLength / 10)
        }
      };
    },
    rootDensity: function() {
      //Third root
      return Math.pow(this.density / 100, 1 / 3) * 100;
    },
    numberOfSteps: function() {
      if (this.design === 0 || this.design === 1) {
        return 4;
      } else if (this.design === 2 || this.design === 3) {
        return 7;
      } else {
        return 0;
      }
    },
    minimumCompartmentSize: function() {
      if (this.bookcase) {
        return this.bookcase.minimumCompartmentSize;
      } else {
        //TODO: get rid of hardcoded value
        return 117;
      }
    },
    materialThickness: function() {
      if (this.material) {
        return this.material.thickness;
      } else {
        return 17;
      }
    },
    measurementGuide: function() {
      let offset = 60;
      if (this.design === 0 || this.design === 1) {
        if (this.activeStep === 0) {
          return {
            start: {
              x: (-this.boxMeasurements.width * 10) / 2,
              y: this.boxMeasurements.height * 10 + offset
            },
            end: {
              x: (this.boxMeasurements.width * 10) / 2,
              y: this.boxMeasurements.height * 10 + offset
            }
          };
        } else if (this.activeStep === 1) {
          return {
            start: {
              x: (-this.boxMeasurements.width * 10) / 2 - offset,
              y: 0
            },
            end: {
              x: (-this.boxMeasurements.width * 10) / 2 - offset,
              y: this.boxMeasurements.height * 10
            }
          };
        }
      } else if (this.design === 2 || this.design === 3) {
        //TODO: correct this hardcoding
        let leftSide =
          (-(
            this.doorMeasurements.doorWidth +
            this.doorMeasurements.leftWidth +
            this.doorMeasurements.rightWidth
          ) /
            2) *
          10;
        if (this.activeStep === 0) {
          let x = leftSide - offset - this.materialThickness;
          return {
            start: {
              x: x,
              y: 0
            },
            end: {
              x: x,
              y: this.doorMeasurements.ceilingHeight * 10
            }
          };
        } else if (this.activeStep === 1) {
          let x =
            leftSide +
            (this.doorMeasurements.leftWidth +
              this.doorMeasurements.doorWidth / 2) *
              10;
          return {
            start: {
              x: x,
              y: 0
            },
            end: {
              x: x,
              y: this.doorMeasurements.doorHeight * 10
            }
          };
        } else if (this.activeStep === 2) {
          return {
            start: {
              x: leftSide + this.doorMeasurements.leftWidth * 10,
              y: (this.doorMeasurements.doorHeight * 10) / 2
            },
            end: {
              x:
                leftSide +
                (this.doorMeasurements.leftWidth +
                  this.doorMeasurements.doorWidth) *
                  10,
              y: (this.doorMeasurements.doorHeight * 10) / 2
            }
          };
        } else if (this.activeStep === 3) {
          return {
            start: {
              x: leftSide,
              y: (this.doorMeasurements.doorHeight * 10) / 2 + 100
            },
            end: {
              x: leftSide + this.doorMeasurements.leftWidth * 10,
              y: (this.doorMeasurements.doorHeight * 10) / 2 + 100
            }
          };
        } else if (this.activeStep === 4) {
          return {
            start: {
              x:
                leftSide +
                (this.doorMeasurements.leftWidth +
                  this.doorMeasurements.doorWidth) *
                  10,
              y: (this.doorMeasurements.doorHeight * 10) / 2 + 100
            },
            end: {
              x:
                leftSide +
                (this.doorMeasurements.leftWidth +
                  this.doorMeasurements.doorWidth +
                  this.doorMeasurements.rightWidth) *
                  10,
              y: (this.doorMeasurements.doorHeight * 10) / 2 + 100
            }
          };
        }
      }
    }
  },
  methods: {
    handleResize: function() {
      if (
        Math.max(document.documentElement.clientWidth, window.innerWidth || 0) >
        850
      ) {
        this.tooltipPlacement = "bottomright";
      } else {
        this.tooltipPlacement = "topright";
      }
    },
    previousStep: function() {
      if (this.activeStep > 0) {
        this.activeStep -= 1;
      }
      this.$emit("setMeasurementGuide", this.measurementGuide);
    },
    nextStep: function() {
      if (this.activeStep < this.numberOfSteps - 1) {
        this.activeStep += 1;
      }
      this.$emit("setMeasurementGuide", this.measurementGuide);
    },
    submit: function() {
      this.$emit("submit");
    },
    recompute: function() {
      this.calculating = true;
      setTimeout(this.performRecompute);
    },
    performRecompute: function() {
      if (this.design === 0 || this.design === 1) {
        if (this.boxMeasurements.width > this.boxLimits.width.max) {
          this.boxMeasurements.width = this.boxLimits.width.max;
        } else if (this.boxMeasurements.width < this.boxLimits.width.min) {
          this.boxMeasurements.width = this.boxLimits.width.min;
        }
        if (this.boxMeasurements.height > this.boxLimits.height.max) {
          this.boxMeasurements.height = this.boxLimits.height.max;
        } else if (this.boxMeasurements.height < this.boxLimits.height.min) {
          this.boxMeasurements.height = this.boxLimits.height.min;
        }
        this.getBoxDesign();
      } else if (this.design === 2 || this.design === 3) {
        if (
          this.doorMeasurements.ceilingHeight <
          this.doorLimits.ceilingHeight.min
        ) {
          this.doorMeasurements.ceilingHeight = this.doorLimits.ceilingHeight.min;
        }
        if (
          this.doorMeasurements.ceilingHeight >
          this.doorLimits.ceilingHeight.max
        ) {
          this.doorMeasurements.ceilingHeight = this.doorLimits.ceilingHeight.max;
        }

        if (this.doorMeasurements.doorHeight < this.doorLimits.doorHeight.min) {
          this.doorMeasurements.doorHeight = this.doorLimits.doorHeight.min;
        }
        if (this.doorMeasurements.doorHeight > this.doorLimits.doorHeight.max) {
          this.doorMeasurements.doorHeight = this.doorLimits.doorHeight.max;
        }

        if (this.doorMeasurements.doorWidth < this.doorLimits.doorWidth.min) {
          this.doorMeasurements.doorWidth = this.doorLimits.doorWidth.min;
        }
        if (this.doorMeasurements.doorWidth > this.doorLimits.doorWidth.max) {
          this.doorMeasurements.doorWidth = this.doorLimits.doorWidth.max;
        }

        if (this.doorMeasurements.leftWidth < this.doorLimits.leftWidth.min) {
          this.doorMeasurements.leftWidth = this.doorLimits.leftWidth.min;
        }
        if (this.doorMeasurements.leftWidth > this.doorLimits.leftWidth.max) {
          this.doorMeasurements.leftWidth = this.doorLimits.leftWidth.max;
        }

        if (this.doorMeasurements.rightWidth < this.doorLimits.rightWidth.min) {
          this.doorMeasurements.rightWidth = this.doorLimits.rightWidth.min;
        }
        if (this.doorMeasurements.rightWidth > this.doorLimits.rightWidth.max) {
          this.doorMeasurements.rightWidth = this.doorLimits.rightWidth.max;
        }

        this.getDoorDesign();
      }
      this.$emit("setMeasurementGuide", this.measurementGuide);
      this.calculating = false;
    },
    getBoxDesign: function() {
      var bookcase = this.getEmptyBox();
      this.fillBookcase(bookcase);
      this.bookcase = bookcase;
      this.$emit("confirm", bookcase);
    },
    getDoorDesign: function() {
      var bookcase = this.getDoorTemplate(
        this.doorMeasurements.doorHeight * 10,
        this.doorMeasurements.ceilingHeight * 10,
        this.doorMeasurements.doorWidth * 10,
        this.doorMeasurements.leftWidth * 10,
        this.doorMeasurements.rightWidth * 10
      );
      this.fillBookcase(bookcase);
      this.bookcase = bookcase;
      this.$emit("confirm", bookcase);
    },
    bookcaseHasLargeCompartments: function(bookcase) {
      for (var i = 0; i < bookcase.compartments.length; i++) {
        var compartment = bookcase.compartments[i];
        if (
          this.compartmentIsWide(compartment, bookcase) ||
          this.compartmentIsTall(compartment, bookcase)
        ) {
          return true;
        }
      }
      return false;
    },
    fillBookcase: function(bookcase) {
      if (this.density > 0) {
        while (this.bookcaseHasLargeCompartments(bookcase)) {
          for (var i = 0; i < bookcase.compartments.length; i++) {
            var compartment = bookcase.compartments[i];
            var randomNumber = Math.random();
            if (
              randomNumber < 1 / 6 &&
              this.compartmentIsWide(compartment, bookcase)
            ) {
              var newCompartments = bookcase.splitCompartment(
                compartment.id,
                "horizontal",
                1
              );
              var newPlate = newCompartments[0].right;
              var leftBound = bookcase.getLeftBound(newPlate);
              var rightBound = bookcase.getRightBound(newPlate);
              var newPosition = Math.floor(
                Math.random() * (rightBound - leftBound) + leftBound
              );
              bookcase.movePlate(newPlate, newPosition, false);
            } else if (
              randomNumber > 5 / 6 &&
              this.compartmentIsTall(compartment, bookcase)
            ) {
              var newCompartments = bookcase.splitCompartment(
                compartment.id,
                "vertical",
                1
              );
              var newPlate = newCompartments[0].top;
              var lowerBound = bookcase.getLowerBound(newPlate);
              var upperBound = bookcase.getUpperBound(newPlate);
              var newPosition = Math.floor(
                Math.random() * (upperBound - lowerBound) + lowerBound
              );
              bookcase.movePlate(newPlate, newPosition, false);
            }
          }
        }
      }

      if (this.numberOfDoors > 0) {
        var validCompartments = bookcase.compartments.filter(compartment =>
          this.compartmentCanHaveDoor(compartment, bookcase)
        );

        bookcase.compartments = this.shuffleArray(bookcase.compartments);
        var i = 0,
          j = 0;
        while (j < validCompartments.length * this.numberOfDoors) {
          if (this.compartmentCanHaveDoor(bookcase.compartments[i], bookcase)) {
            bookcase.compartments[i].hasDoor = true;
            bookcase.compartments[i].doorPosition = Math.random() > 0.5 ? 0 : 1;
            j++;
          }
          i++;
        }
      }
    },
    compartmentCanHaveDoor: function(compartment, bookcase) {
      //TODO: Duplicated code in compartment-edit-modal
      return (
        compartment.dimensions.width <=
          this.bookcase.maxWidthWithDoor + this.materialThickness &&
        compartment.dimensions.height >=
          this.bookcase.minHeightWithDoor + this.materialThickness
      );
    },
    compartmentIsWide: function(compartment, bookcase) {
      var maxWidth =
        this.maxPlateLength -
        (this.rootDensity *
          (this.maxPlateLength - 2 * bookcase.minimumCompartmentSize)) /
          100;
      if (compartment.dimensions.width > maxWidth) {
        return true;
      }
      return false;
    },
    compartmentIsTall: function(compartment, bookcase) {
      var maxHeight =
        this.maxPlateLength -
        (this.rootDensity *
          (this.maxPlateLength - 2 * bookcase.minimumCompartmentSize)) /
          100;
      if (compartment.dimensions.height > maxHeight) {
        return true;
      }
      return false;
    },
    getEmptyBox: function() {
      var width = this.boxMeasurements.width * 10;
      var coordinateWidth = width - this.materialThickness;
      var height = this.boxMeasurements.height * 10;
      var coordinateHeight = height - this.materialThickness;
      var plates, plateNumbersForCompartments;
      if (width <= this.maxPlateLength) {
        if (height <= this.maxPlateLength + 2 * this.materialThickness) {
          plates = [
            [[0, 0], [0, coordinateHeight], true, true, true], //Left
            [
              [0, coordinateHeight],
              [coordinateWidth, coordinateHeight],
              false,
              false,
              true
            ], //Top
            [
              [coordinateWidth, 0],
              [coordinateWidth, coordinateHeight],
              true,
              true,
              true
            ], //Right
            [[0, 0], [coordinateWidth, 0], false, false, false] //Bottom
          ];
          plateNumbersForCompartments = [[1, 3, 0, 2]];
        } else {
          var mid = Math.floor(coordinateHeight / 2);
          plates = [
            [[0, 0], [0, mid], true, true, false], //0Left lower
            [[0, mid], [0, coordinateHeight], true, true, false], //1Left upper
            [[coordinateWidth, 0], [coordinateWidth, mid], true, true, false], //2Right lower
            [
              [coordinateWidth, mid],
              [coordinateWidth, coordinateHeight],
              true,
              true,
              false
            ], //3Right upper
            [[0, 0], [coordinateWidth, 0], false, false, false], //4Bottom
            [[0, mid], [coordinateWidth, mid], false, false, true], //5Middle
            [
              [0, coordinateHeight],
              [coordinateWidth, coordinateHeight],
              false,
              false,
              true
            ] //6Top
          ];
          plateNumbersForCompartments = [
            [5, 4, 0, 2],
            [6, 5, 1, 3]
          ];
        }
      } else if (width <= 2 * this.maxPlateLength + this.materialThickness) {
        if (height <= this.maxPlateLength + 2 * this.materialThickness) {
          var mid = Math.floor(coordinateWidth / 2);
          plates = [
            [[0, 0], [0, coordinateHeight], false, false, true], //0Left
            [[0, 0], [mid, 0], true, true, false], //1Left lower
            [[0, coordinateHeight], [mid, coordinateHeight], true, true, false], //2Left upper
            [[mid, 0], [mid, coordinateHeight], false, false, true], //3Middle
            [[mid, 0], [coordinateWidth, 0], true, true, false], //4Right lower
            [
              [mid, coordinateHeight],
              [coordinateWidth, coordinateHeight],
              true,
              true,
              false
            ], //5Right upper
            [
              [coordinateWidth, 0],
              [coordinateWidth, coordinateHeight],
              false,
              false,
              true
            ] //6Right
          ];
          plateNumbersForCompartments = [
            [2, 1, 0, 3],
            [5, 4, 3, 6]
          ];
        } else {
          var third = Math.floor(coordinateWidth / 3);
          var midWidth = Math.floor(coordinateWidth / 2);
          var twoThirds = Math.floor((2 * coordinateWidth) / 3);
          var midHeight = Math.floor(coordinateHeight / 2);
          plates = [
            [[0, 0], [midWidth, 0], false, true, false], // 0 Bottom left
            [[0, 0], [0, midHeight], true, true, false], // 1 Lower left
            [[0, midHeight], [midWidth, midHeight], false, true, false], //2 Middle left
            [[0, midHeight], [0, coordinateHeight], true, true, false], // 3 Upper left
            [
              [0, coordinateHeight],
              [third, coordinateHeight],
              false,
              true,
              false
            ], // 4 Top left
            [[third, midHeight], [third, coordinateHeight], true, false, true], // 5 Top third
            [
              [third, coordinateHeight],
              [twoThirds, coordinateHeight],
              true,
              true,
              false
            ], // 6 Top middle
            [[midWidth, 0], [midWidth, midHeight], false, false, true], // 7 Lower middle
            [[midWidth, 0], [coordinateWidth, 0], true, false, false], // 8 Bottom right
            [
              [midWidth, midHeight],
              [coordinateWidth, midHeight],
              true,
              false,
              false
            ], //9 Middle right
            [
              [twoThirds, midHeight],
              [twoThirds, coordinateHeight],
              true,
              false,
              true
            ], // 10 Top two thirds
            [
              [twoThirds, coordinateHeight],
              [coordinateWidth, coordinateHeight],
              true,
              false,
              false
            ], // 11 Top right
            [
              [coordinateWidth, 0],
              [coordinateWidth, midHeight],
              true,
              true,
              false
            ], // 12 Lower right
            [
              [coordinateWidth, midHeight],
              [coordinateWidth, coordinateHeight],
              true,
              true,
              false
            ] // 13 Upper right
          ];
          plateNumbersForCompartments = [
            [2, 0, 1, 7],
            [4, 2, 3, 5],
            [6, 2, 5, 10], //TODO: Problem with compartment data model. This compartment actually has 2 bottom compartments.
            [9, 8, 7, 12],
            [11, 9, 10, 13]
          ];
        }
      } else {
        if (height <= this.maxPlateLength + 2 * this.materialThickness) {
          var midLeft = Math.floor(coordinateWidth / 3);
          var midRight = Math.floor((2 * coordinateWidth) / 3);
          plates = [
            [[0, 0], [0, coordinateHeight], false, false, true], //0Left
            [[0, 0], [midLeft, 0], true, true, false], //1Left lower
            [
              [0, coordinateHeight],
              [midLeft, coordinateHeight],
              true,
              true,
              false
            ], //2Left upper
            [[midLeft, 0], [midLeft, coordinateHeight], false, false, true], //3Middle left
            [[midLeft, 0], [midRight, 0], true, true, false], //4Middle lower
            [
              [midLeft, coordinateHeight],
              [midRight, coordinateHeight],
              true,
              true,
              false
            ], //5Middle upper
            [[midRight, 0], [midRight, coordinateHeight], false, false, true], //6Middle Right
            [[midRight, 0], [coordinateWidth, 0], true, true, false], //7Right lower
            [
              [midRight, coordinateHeight],
              [coordinateWidth, coordinateHeight],
              true,
              true,
              false
            ], //8Right upper
            [
              [coordinateWidth, 0],
              [coordinateWidth, coordinateHeight],
              false,
              false,
              true
            ] //9Right
          ];
          plateNumbersForCompartments = [
            [2, 1, 0, 3],
            [5, 4, 3, 6],
            [8, 7, 6, 9]
          ];
        } else {
          var fourth = Math.floor(coordinateWidth / 4);
          var third = Math.floor(coordinateWidth / 3);
          var midWidth = Math.floor(coordinateWidth / 2);
          var twoThirds = Math.floor((2 * coordinateWidth) / 3);
          var threeFourths = Math.floor((3 * coordinateWidth) / 4);
          var midHeight = Math.floor(coordinateHeight / 2);
          plates = [
            [[0, 0], [third, 0], false, true, false], // 0 Bottom left
            [[0, 0], [0, midHeight], true, true, false], // 1 Lower left
            [[0, midHeight], [third, midHeight], false, true, false], //2 Middle left
            [[0, midHeight], [0, coordinateHeight], true, true, false], // 3 Upper left
            [
              [0, coordinateHeight],
              [fourth, coordinateHeight],
              false,
              true,
              false
            ], // 4 Top left
            [
              [fourth, midHeight],
              [fourth, coordinateHeight],
              true,
              false,
              true
            ], // 5 Upper fourth
            [
              [fourth, coordinateHeight],
              [midWidth, coordinateHeight],
              true,
              true,
              false
            ], // 6 Top two fourths
            [[third, 0], [third, midHeight], false, false, true], // 7 Lower third

            [[third, 0], [twoThirds, 0], true, true, false], // 8 Bottom middle
            [[third, midHeight], [twoThirds, midHeight], true, true, false], //9 Middle middle
            [
              [midWidth, midHeight],
              [midWidth, coordinateHeight],
              true,
              false,
              true
            ], // 10 Upper middle
            [
              [midWidth, coordinateHeight],
              [threeFourths, coordinateHeight],
              true,
              true,
              false
            ], // 11 Top three fourths
            [[twoThirds, 0], [twoThirds, midHeight], false, false, true], // 12 Lower two thirds
            [[twoThirds, 0], [coordinateWidth, 0], true, false, false], // 13 Bottom right
            [
              [twoThirds, midHeight],
              [coordinateWidth, midHeight],
              true,
              false,
              false
            ], // 14 Middle right
            [
              [threeFourths, midHeight],
              [threeFourths, coordinateHeight],
              true,
              false,
              true
            ], // 15 Upper three fourths
            [
              [threeFourths, coordinateHeight],
              [coordinateWidth, coordinateHeight],
              true,
              false,
              false
            ], // 16 Top right
            [
              [coordinateWidth, 0],
              [coordinateWidth, midHeight],
              true,
              true,
              false
            ], // 17 Lower right
            [
              [coordinateWidth, midHeight],
              [coordinateWidth, coordinateHeight],
              true,
              true,
              false
            ] // 18 Upper right
          ];
          plateNumbersForCompartments = [
            [2, 0, 1, 7], //TODO: Problem with compartment data model. This compartment actually has 2 bottom compartments.
            [4, 2, 3, 5],
            [6, 2, 5, 10], //TODO: Problem with compartment data model. This compartment actually has 2 bottom compartments.
            [9, 8, 7, 12], //TODO: Problem with compartment data model. This compartment actually has 2 bottom compartments.
            [11, 9, 10, 15], //TODO: Problem with compartment data model. This compartment actually has 2 bottom compartments.
            [14, 13, 12, 17], //TODO: Problem with compartment data model. This compartment actually has 2 bottom compartments.
            [16, 14, 15, 18]
          ];
        }
      }
      var bookcase = this.getBookcase(plates, plateNumbersForCompartments);
      return bookcase;
    },
    getDoorTemplate: function(
      doorHeight,
      ceilingHeight,
      doorWidth,
      leftWidth,
      rightWidth
    ) {
      var left = 0;
      var innerLeft = leftWidth - this.materialThickness;
      var innerRight = innerLeft + doorWidth + this.materialThickness;
      var right = innerRight + rightWidth - this.materialThickness;
      var bottom = 0;
      var innerTop = doorHeight;
      var top = ceilingHeight - this.materialThickness;
      var minimumCompartmentSize = 117;
      var lowerInnerTop = Math.floor(innerTop / 2);
      var plates = [
        [[left, bottom], [left, lowerInnerTop], true, true, false], //0Left lower outer
        [[left, lowerInnerTop], [left, top], true, false, false], //1Left upper outer
        [[left, bottom], [innerLeft, bottom], false, false, false], //2Bottom left
        [[innerLeft, bottom], [innerLeft, lowerInnerTop], true, true, false], //3Left lower inner
        [[innerLeft, lowerInnerTop], [innerLeft, top], true, false, false], //4Left upper inner
        [[left, top], [innerLeft, top], true, true, false], //5Top left
        [[innerLeft, top], [innerRight, top], true, true, false], //6Top middle
        [[innerRight, top], [right, top], true, true, false], //7Top right
        [[innerLeft, innerTop], [innerRight, innerTop], true, true, false], //8Base of top compartment
        [[innerRight, bottom], [innerRight, lowerInnerTop], true, true, false], //9Right lower inner
        [[right, bottom], [right, lowerInnerTop], true, true, false], //10Right lower outer
        [[innerRight, lowerInnerTop], [innerRight, top], true, false, false], //11Right upper inner
        [[right, lowerInnerTop], [right, top], true, false, false], //12Right upper outer
        [[innerRight, bottom], [right, bottom], false, false, false], //13Bottom right
        [[left, lowerInnerTop], [innerLeft, lowerInnerTop], false, false, true], //14Left middle lower horizontal
        [
          [innerRight, lowerInnerTop],
          [right, lowerInnerTop],
          false,
          false,
          true
        ], //15Right middle lower horizontal
        [[left, innerTop], [innerLeft, innerTop], true, true, true], //16Left middle upper horizontal
        [[innerRight, innerTop], [right, innerTop], true, true, true] //17Right middle upper horizontal
      ];
      var plateNumbersForCompartments = [
        [14, 2, 0, 3],
        [16, 14, 1, 4],
        [5, 16, 1, 4],
        [6, 8, 4, 11],
        [15, 13, 9, 10],
        [17, 15, 11, 12],
        [7, 17, 11, 12]
      ];
      var bookcase = this.getBookcase(plates, plateNumbersForCompartments);
      bookcase.startingPrice = 2000;
      return bookcase;
    },
    getBookcase: function(plates, plateNumbersForCompartments) {
      var bookcase = new BookcaseClass(null);
      bookcase.material = this.material;
      bookcase.maxLengthWithoutSupport = 500 + this.materialThickness;
      bookcase.plateDepth = 300;
      bookcase.lockedForEditing = false;
      bookcase.supportHeight = 60; //TODO: Make these values come from backend.
      for (var i = 0; i < plates.length; i++) {
        bookcase.addGetPlate(
          null,
          bookcase.addGetCorner(plates[i][0][0], plates[i][0][1]),
          bookcase.addGetCorner(plates[i][1][0], plates[i][1][1]),
          plates[i][2],
          plates[i][3],
          plates[i][4],
          null
        );
      }
      for (var i = 0; i < plateNumbersForCompartments.length; i++) {
        var plateNumbersForCompartment = plateNumbersForCompartments[i];
        bookcase.addGetCompartment(
          null,
          bookcase.plates[plateNumbersForCompartment[0]],
          bookcase.plates[plateNumbersForCompartment[1]],
          bookcase.plates[plateNumbersForCompartment[2]],
          bookcase.plates[plateNumbersForCompartment[3]],
          false,
          false,
          null,
          0
        );
      }
      return this.centerBookcase(bookcase);
    },
    centerBookcase: function(bookcase) {
      var xCoordinates = bookcase.corners.map(corner => corner.x);
      var left = Math.min(...xCoordinates);
      var right = Math.max(...xCoordinates);
      var center = Math.floor((left + right) / 2);
      for (var i = 0; i < bookcase.corners.length; i++) {
        bookcase.corners[i].x -= center;
      }
      return bookcase;
    },
    shuffleArray: function(array) {
      var currentIndex = array.length,
        temporaryValue,
        randomIndex;

      // While there remain elements to shuffle...
      while (0 !== currentIndex) {
        // Pick a remaining element...
        randomIndex = Math.floor(Math.random() * currentIndex);
        currentIndex -= 1;

        // And swap it with the current element.
        temporaryValue = array[currentIndex];
        array[currentIndex] = array[randomIndex];
        array[randomIndex] = temporaryValue;
      }

      return array;
    }
  }
};
</script>
<style>
.base-design-slider {
  width: calc(100% - 90px);
  float: right;
}
.base-design-slider.large {
  width: 100%;
}
.base-design-tooltip {
  display: inline;
}
.base-design-header {
  font-size: 20px;
  position: relative;
  bottom: 5px;
  margin-right: 10px;
}

.base-design-tooltip-icon {
  font-size: 2em;
  color: #2dac86;
}

@media (max-width: 850px) {
  .base-design-header {
    font-size: 16px;
  }
  .base-design-tooltip-icon {
    font-size: 1.5em;
  }
}
.base-design-input {
  width: 70px;
}

.base-design-measurement {
  display: inline-block;
}

.base-design-measurement-container {
  margin-top: 6px;
  text-align: center;
}

.steps-buttons-container {
  text-align: center;
  margin-top: 15px;
}
.tooltip .arrow {
  display: none !important;
}
</style>
