<template>
  <div>
    <full-page-spinner v-if="loading"></full-page-spinner>
    <div v-if="!showBookcaseDesigner && !loading">
      <div class="standard-box align-center">
        <div
          id="open-bookcase-button-container"
          v-if="showContinueOption && !showBookcaseDesigner && !loading"
          style="margin-top: 10px; margin-bottom: 20px"
        >
          <h3>Fortsæt hvor du slap</h3>
          <a v-on:click="openLocallyStoredBookcase()">
            <open-bookcase-button
              v-if="!showBookcaseDesigner"
              :size="svgSize"
              :bookcaseData="bookcase"
              :margin="0.25"
              fillColor="#f2f2f2"
            ></open-bookcase-button>
          </a>
        </div>
        <div v-if="showContinueOption">
          <h3>Eller vælg grunddesign</h3>
        </div>
        <div v-else>
          <h3>Vælg grunddesign</h3>
        </div>
        <base-designs></base-designs>
      </div>
      <page-footer></page-footer>
    </div>
    <bookcase-designer
      v-if="showBookcaseDesigner"
      :initialData="bookcase"
      @exit="exitBookcaseDesigner"
      @showHideNavMenu="showHideNavMenu"
      @setLoading="setLoading"
      :baseDesign="baseDesign"
      :isLoggedIn="isLoggedIn"
    ></bookcase-designer>
    <div></div>
  </div>
</template>

<script>
import baseDesigns from "./sections/home/base-designs";
import fullPageSpinner from "./full-page-spinner";
import openBookcaseButton from "./bookcase-designer/graphic-buttons/open-bookcase-button";
import bookcaseDesigner from "./bookcase-designer/bookcase-designer";
import { bookcaseService, localstorageService } from "../_services";
import pageFooter from "./page-footer";
export default {
  components: {
    "open-bookcase-button": openBookcaseButton,
    "bookcase-designer": bookcaseDesigner,
    "base-designs": baseDesigns,
    "full-page-spinner": fullPageSpinner,
    "page-footer": pageFooter,
  },
  props: {
    baseDesignChoice: String,
  },
  data() {
    return {
      loading: true,
      showContinueOption: false,
      showBookcaseDesigner: false,
      bookcase: null,
      baseDesign: null,
    };
  },
  mounted() {
    setTimeout(this.initialLoad);
  },
  computed: {
    svgSize: function () {
      var deviceSize = Math.round(
        window.innerWidth > 0 ? window.innerWidth : screen.width
      );
      return Math.min(deviceSize / 2 - 20, 300);
      //TODO: Merge with identical function in bookcase-template-chooser.
    },
    isLoggedIn: function () {
      return this.$store.state.account.status.loggedIn;
    },
  },
  watch: {
    baseDesignChoice: function (newValue, oldValue) {
      if (newValue !== oldValue) {
        this.initialLoad();
      }
    },
  },
  methods: {
    initialLoad: function () {
      if (this.baseDesignChoice == "simpelt") {
        this.selectBaseDesign(0);
      } else if (this.baseDesignChoice == "doeraabning") {
        this.selectBaseDesign(2);
      } else if (localstorageService.bookcaseExists()) {
        this.bookcase = localstorageService.getBookcase();
        if (localstorageService.getOpenDirectly()) {
          this.showHideBookcaseDesigner(true);
        } else {
          this.showContinueOption = true;
        }
      }
      this.loading = false;
    },
    setLoading: function (value) {
      this.loading = value;
    },
    selectBaseDesign: function (index) {
      this.bookcase = null;
      this.baseDesign = index;
      this.showHideBookcaseDesigner(true);
    },
    showHideBookcaseDesigner: function (shouldShow) {
      if (shouldShow) {
        this.showBookcaseDesigner = true;
      } else {
        this.showBookcaseDesigner = false;
      }
      localstorageService.setOpenDirectly(false);
    },
    showHideNavMenu: function (shouldShow) {
      if (shouldShow) {
        this.$emit("showHideNavMenu", true);
      } else {
        this.$emit("showHideNavMenu", false);
      }
    },
    hideContinueOption: function () {
      this.showContinueOption = false;
    },
    openLocallyStoredBookcase: function () {
      this.showContinueOption = false;
      this.showHideBookcaseDesigner(true);
    },
    openNewBookcase: function (bookcase) {
      this.bookcase = bookcase;
      this.showHideBookcaseDesigner(true);
    },
    openBookcase: function (bookcaseId) {
      this.loading = true;
      bookcaseService.get(bookcaseId).then((bookcase) => {
        this.loading = false;
        this.bookcase = bookcase;
        this.showHideBookcaseDesigner(true);
      });
    },
    exitBookcaseDesigner: function () {
      if (this.isLoggedIn) {
        this.$router.push("/mine-reoler");
      } else {
        this.$router.push("/");
        if (localstorageService.bookcaseExists()) {
          this.bookcase = localstorageService.getBookcase();
        }
        this.showHideBookcaseDesigner(false);
        this.showContinueOption = true;
      }
    },
  },
};
</script>

<style>
</style>
