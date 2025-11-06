<template>
  <div class="custom-modal">
    <div class="modal-content" style="max-width: 400px">
      <h3>Konfigurer reol</h3>
      <div>
        <label class="b-contain">
          <span>Væghængt</span>
          <input type="checkbox" v-model="isWallSuspended" />
          <span class="b-input"></span>
        </label>
      </div>
      <div
        class="checklist-sub-content small-text"
      >Vi bygger reolen, så den kan vægmonteres, og monterer den på væggen.</div>
      <div>
        <div>
          <label class="b-contain">
            <span>Udskæring til fodliste</span>
            <input type="checkbox" v-model="hasSkirtingBoard" />
            <span class="b-input"></span>
          </label>
        </div>
        <div
          class="checklist-sub-content small-text"
        >Vi laver en udskæring på reolens bagside, så reolen kan sættes helt op ad bagvæggen, uden om fodlisten.</div>
        <div class="checklist-sub-content" v-if="hasSkirtingBoard">
          <div>Maks. tykkelse af fodliste</div>
          <div
            class="small-text"
          >Tykkelsen af fodlisten på det tykkeste sted. Måles med målebånd eller tommestok ved at kigge ovenfra.</div>
          <div>
            <input
              type="number"
              v-model.number="skirtingBoardDepth"
              class="number-input-field"
              step="0.1"
            /> cm
          </div>
          <div>Maks. højde af fodliste</div>
          <div
            class="small-text"
          >Højden af fodlisten på det højeste sted. Måles med målebånd eller tommestok ved at kigge forfra.</div>
          <div>
            <input
              type="number"
              v-model.number="skirtingBoardHeight"
              class="number-input-field"
              step="0.1"
            /> cm
          </div>
        </div>
      </div>
      <div>
        <div>
          <label class="b-contain">
            <span>Sokkel</span>
            <input type="checkbox" v-model="hasBase" />
            <span class="b-input"></span>
          </label>
        </div>
        <div
          class="checklist-sub-content small-text"
        >Reolen forsynes med en sokkel, så reolens nederste hylde hæves 8 cm over gulvet. Bemærk, at soklen ikke vises her i konfiguratoren.</div>
      </div>
      <div>
        <button v-on:click="close(false)" class="btn" style="float:right">OK</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "bookcase-options",
  data() {
    return {
      hasSkirtingBoard: false,
      skirtingBoardDepth: null,
      skirtingBoardHeight: null,
      hasBase: false,
      baseHeight: null,
      isWallSuspended: false
    };
  },
  props: {
    bookcase: Object
  },
  watch: {
    hasSkirtingBoard: function(value) {
      if (value) {
        this.isWallSuspended = false;
        if (!this.skirtingBoardDepth) {
          this.skirtingBoardDepth = 3;
        }
        if (!this.skirtingBoardHeight) {
          this.skirtingBoardHeight = 12;
        }
      } else {
        this.skirtingBoardDepth = null;
        this.skirtingBoardHeight = null;
      }
    },
    skirtingBoardDepth: function(value) {
      if (typeof value == "number" && value) {
        if (value < 0.1) {
          this.skirtingBoardDepth = 0.1;
          return;
        }
        if (value > 10) {
          this.skirtingBoardDepth = 10;
        }
        this.bookcase.skirtingBoardDepth = value * 10;
      } else {
        this.bookcase.skirtingBoardDepth = null;
      }
    },
    skirtingBoardHeight: function(value) {
      if (typeof value == "number" && value) {
        if (value < 0.1) {
          this.skirtingBoardHeight = 0.1;
          return;
        }
        if (value > 50) {
          this.skirtingBoardHeight = 50;
        }
        this.bookcase.skirtingBoardHeight = value * 10;
      } else {
        this.bookcase.skirtingBoardHeight = null;
      }
    },
    isWallSuspended: function(value) {
      this.bookcase.isWallSuspended = value;
      if (value) {
        this.hasSkirtingBoard = false;
        this.hasBase = false;
      }
    },
    hasBase: function(value) {
      if (value) {
        this.isWallSuspended = false;
        if (!this.bookcase.baseHeight) {
          this.baseHeight = 8;
        }
      } else {
        this.baseHeight = null;
      }
    },
    baseHeight: function(value) {
      if (value) {
        this.bookcase.baseHeight = value * 10;
      } else {
        this.bookcase.baseHeight = null;
      }
    }
  },
  methods: {
    close: function() {
      this.$emit("close");
    }
  },
  mounted() {
    if (this.bookcase.skirtingBoardDepth) {
      this.hasSkirtingBoard = true;
      this.skirtingBoardDepth = this.bookcase.skirtingBoardDepth / 10;
    }
    if (this.bookcase.skirtingBoardHeight) {
      this.hasSkirtingBoard = true;
      this.skirtingBoardHeight = this.bookcase.skirtingBoardHeight / 10;
    }
    if (this.bookcase.baseHeight) {
      this.hasBase = true;
      this.baseHeight = this.bookcase.baseHeight / 10;
    }
    this.isWallSuspended = this.bookcase.isWallSuspended;
  }
};
</script>
<style>
.number-input-field {
  width: 70px;
  margin: 5px 0;
}
.checklist-sub-content {
  margin: -5px 0 8px 45px;
}
.small-text {
  font-size: 12px;
}
</style>
