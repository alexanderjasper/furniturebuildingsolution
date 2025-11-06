<template>
  <div>
    <div>
      <h3 v-if="showContinueOption">Eller vælg grunddesign</h3>
      <h3 v-if="!showContinueOption">Vælg grunddesign</h3>
      <div>
        <div class="base-design-image-container">
          <div :style="getBackgroundAttribute(0)" class="base-design-image" v-on:click="submit(0)"></div>
        </div>
        <div class="base-design-image-container">
          <div :style="getBackgroundAttribute(2)" class="base-design-image" v-on:click="submit(2)"></div>
        </div>
      </div>
      <!-- <div>
        <div class="base-design-image-container">
          <div :style="getBackgroundAttribute(1)" class="base-design-image" v-on:click="submit(1)"></div>
        </div>
        <div class="base-design-image-container">
          <div :style="getBackgroundAttribute(3)" class="base-design-image" v-on:click="submit(3)"></div>
        </div>
      </div>-->
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {};
  },
  props: {
    showContinueOption: Boolean
  },
  methods: {
    submit: function(index) {
      this.$emit("selectBaseDesign", index);
    },
    getBackgroundAttribute: function(index) {
      if (index === 0) {
        return "background-image: url('/resources/base-designs/square-empty-white.PNG');";
      } else if (index === 1) {
        return "background-image: url('/resources/base-designs/square-filled.PNG');";
      } else if (index === 2) {
        return "background-image: url('/resources/base-designs/door-empty-white.PNG');";
      } else {
        return "background-image: url('/resources/base-designs/door-filled.PNG');";
      }
    }
  }
};
</script>

<style>
.base-design-image {
  width: 100%;
  padding-top: 100%;
  display: inline-block;
  cursor: pointer;
  background-size: cover;
  border-radius: 5px;
}
.base-design-image-container {
  display: inline-block;
  margin: 15px;
  width: 300px;
  max-width: calc(50% - 40px);
}
</style>
