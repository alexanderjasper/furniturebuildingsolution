<template>
  <span>
    <div class="undo-redo-help-container">
      <div class="undo-redo-help">
        <button class="tiny-btn btn-invisible" :style="undoButtonStyle" v-on:click="undo()">
          <icon fixed-width icon="undo" class="undo-redo-help-icon" />
        </button>
        <button class="tiny-btn btn-invisible" :style="redoButtonStyle" v-on:click="redo()">
          <icon fixed-width icon="redo" class="undo-redo-help-icon" />
        </button>
        <button
          class="btn btn-secondary help-icon-button button--designer-help"
          v-on:click="showHideHelp(true)"
        >
          <icon fixed-width icon="question" class="undo-redo-help-icon" />
        </button>
        <span v-if="editable" v-on:click="buy()">
          <button class="btn btn-brand help-icon-button button--designer-buy">
            <span v-if="!currentState.addingToCart">
              <icon fixed-width icon="shopping-cart" class="undo-redo-help-icon" />
            </span>
            <!-- TODO: huge hack styling: -->
            <span
              v-if="currentState.addingToCart"
              class="spinner-border spinner-border-sm undo-redo-help-icon"
              role="status"
              aria-hidden="true"
              style="width: 1em; height: 1em; margin-left: 0.125em; margin-right: 0.125em;"
            ></span>
          </button>
        </span>
        <span v-if="editable" v-on:click="saveBookcase()">
          <button class="btn btn-brand help-icon-button button--designer-save">
            <span v-if="!currentState.isSaving">
              <icon fixed-width icon="save" class="undo-redo-help-icon" />
            </span>
            <!-- TODO: huge hack styling: -->
            <span
              v-if="currentState.isSaving"
              class="spinner-border spinner-border-sm undo-redo-help-icon"
              role="status"
              aria-hidden="true"
              style="width: 1em; height: 1em; margin-left: 0.125em; margin-right: 0.125em;"
            ></span>
          </button>
        </span>
        <button
          class="btn btn-negative help-icon-button button--designer-close"
          v-on:click="exitBookcaseDesigner()"
        >
          <icon fixed-width icon="times" class="undo-redo-help-icon" />
        </button>
      </div>
    </div>
    <div class="custom-modal" v-if="showHelp">
      <div class="modal-content" style="max-width: 500px" v-click-outside="hideHelp">
        <div class="help-content">
          <!-- <div v-if="helpIndex === 0">
            <h3>Angiv mål:</h3>
            <ul>
              <li>Angiv de ønskede mål til din reol.</li>
              <li>Klik på "Dan ny tilfældig reol" for at få et nyt tilfældigt genereret design.</li>
              <li>Klik på OK. Derefter kan du justere reolen helt efter eget ønske.</li>
            </ul>
            <div class="align-center">
              <img class="help-image" src="/resources/images/Help5.png" />
            </div>
          </div>-->
          <div v-if="helpIndex === 0">
            <h3>Roter/flyt:</h3>
            <ul>
              <li>Klik og træk for at rotere skærmbillet.</li>
              <li>Højreklik og træk for at flytte skærmbilledet.</li>
              <li>Scroll for at zoome.</li>
            </ul>
            <div class="align-center">
              <video autoplay loop muted playsinline class="help-image">
                <source src="/resources/images/Help1.mp4" type="video/mp4" />
              </video>
            </div>
          </div>
          <div v-if="helpIndex === 1">
            <h3>Rediger rum</h3>
            <ul>
              <li>Tryk på blyantsikonet i et rum for at redigere rummet.</li>
            </ul>
            <div class="align-center">
              <img class="help-image" src="/resources/images/Help2-1.PNG" />
            </div>
            <ul>
              <li>Du kan nu inddele rummet ved at tilføje vægge og hylder.</li>
            </ul>
            <div class="align-center">
              <img class="help-image" src="/resources/images/Help2-2.PNG" />
            </div>
          </div>
          <div v-if="helpIndex === 2">
            <h3>Rediger plade</h3>
            <ul>
              <li>Tryk på en plade for at markere den.</li>
              <li>Du kan nu flytte eller slette pladen.</li>
            </ul>
            <div class="align-center">
              <img class="help-image" src="/resources/images/Help3.PNG" />
            </div>
          </div>
          <div v-if="helpIndex === 3">
            <h3>Rediger reol</h3>Brug kontrolpanelet til at:
            <ul>
              <li>Navngive reolen</li>
              <li>Ændre materiale</li>
              <li>Ændre reolens dybde</li>
              <li>Vise eller skjule redigeringssymbolerne (blyantsikoner)</li>
              <li>Vælge tilvalg</li>
            </ul>
            <div class="align-center">
              <img class="help-image" src="/resources/images/Help4.PNG" />
            </div>
          </div>
        </div>
        <div class="help-change-buttons">
          <button v-on:click="previousHelp()" class="btn btn-small">
            <icon fixed-width icon="arrow-left" />
          </button>
          <span style="margin: 0 10px">{{(this.helpIndex + 1) + "/" + (this.maxHelpIndex +1)}}</span>
          <button v-on:click="nextHelp()" class="btn btn-small">
            <icon fixed-width icon="arrow-right" />
          </button>
        </div>
        <div>
          <button
            v-on:click="showHideHelp(false)"
            class="btn btn-small"
            style="float:right; position: relative; bottom: -10px;"
          >OK</button>
        </div>
      </div>
    </div>
  </span>
</template>
<script>
export default {
  name: "undo-redo-help",
  props: {
    currentState: Object,
    editable: Boolean,
    showUndo: Boolean,
    showRedo: Boolean,
    showHelp: Boolean,
  },
  data() {
    return { maxHelpIndex: 3, helpIndex: 0 };
  },
  computed: {
    undoButtonStyle: function () {
      if (!this.showUndo) {
        return "display: none;";
      } else {
        return "";
      }
    },
    redoButtonStyle: function () {
      if (!this.showUndo && !this.showRedo) {
        return "display: none;";
      } else if (this.showUndo && !this.showRedo) {
        return "visibility: hidden;";
      } else {
        return "";
      }
    },
  },
  methods: {
    previousHelp: function () {
      if (this.helpIndex === 0) {
        this.helpIndex = this.maxHelpIndex;
      } else {
        this.helpIndex -= 1;
      }
    },
    nextHelp: function () {
      if (this.helpIndex === this.maxHelpIndex) {
        this.helpIndex = 0;
      } else {
        this.helpIndex += 1;
      }
    },
    hideHelp: function () {
      this.showHideHelp(false);
    },
    showHideHelp: function (shouldShow) {
      this.$emit("showHide", shouldShow);
    },
    exitBookcaseDesigner: function () {
      this.$emit("exitBookcaseDesigner");
    },
    saveBookcase: function () {
      this.$emit("saveBookcase");
    },
    buy: function () {
      this.$emit("buyBookcase");
    },
    undo: function () {
      this.$emit("undo");
    },
    redo: function () {
      this.$emit("redo");
    },
  },
};
</script>
<style>
.help-content {
  margin-bottom: 40px;
  text-align: left;
  height: calc(90vh);
  max-height: 522px;
  overflow: auto;
}
.help-image {
  max-width: 80%;
  margin: 0 0 15px 0;
  border-radius: 3px;
}
.undo-redo-help-container {
  overflow: auto; /* Enable scroll if needed */
  background-color: rgb(0, 0, 0); /* Fallback color */
  background-color: transparent;
}

.undo-redo-help {
  padding: 10px 0 0 0;
  margin: 6px 0 0 0;
}

.help-icon-button {
  padding: 14px 10px;
  margin-right: 15px;
}

.help-change-buttons {
  position: fixed;
  bottom: 20px;
  left: 50%;
  margin-left: -78px;
}

.undo-redo-help-container {
  position: fixed; /* Stay in place */
  top: 0;
  right: 0;
}

@media (max-width: 400px) {
  .help-change-buttons {
    margin-left: -106px;
  }
}

@media (max-width: 850px) {
  .undo-redo-help-icon {
    font-size: 1.3333333333em;
    line-height: 0.75em;
    vertical-align: -0.0667em;
  }
  .help-icon-button {
    padding: 4px 4px 2px 4px;
    margin-right: 4px;
  }
}
@media (min-width: 851px) {
  .undo-redo-help-icon {
    font-size: 2em;
  }
  .undo-redo-help-container {
    position: fixed; /* Stay in place */
    right: 0;
  }
}
</style>
