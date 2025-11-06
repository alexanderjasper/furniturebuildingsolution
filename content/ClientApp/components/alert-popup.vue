<template>
  <div :class="containerClassAttribute">
    <div v-if="type && message" :class="popupClassAttribute">
      <div class="alert-text-part">{{message}}</div>
    </div>
  </div>
</template>

<script>
import { mapActions } from "vuex";
export default {
  components: {},
  props: {
    type: String,
    message: String,
    route: String
  },
  computed: {
    popupClassAttribute: function() {
      var base = "alert-popup";
      if (this.type === "alert-success") {
        base += " success";
      } else if (this.type === "alert-danger") {
        base += " danger";
      }
      if (this.route === "bookcase-designer" && this.type && this.message) {
        base += " alert-popup-top-margin";
      }
      return base;
    },
    containerClassAttribute: function() {
      var base = "alert-popup-container";
      if (this.route !== "bookcase-designer" && this.type && this.message) {
        base += " alert-popup-top-margin";
      }
      return base;
    }
  },
  methods: {
    ...mapActions("alert", ["clear"]),
    close: function() {
      this.clear();
    }
  },
  watch: {
    message: function(message) {
      if (message) {
        setTimeout(this.close, 3000);
      }
    }
  },
  data() {
    return {};
  }
};
</script>

<style>
.alert-popup {
  padding: 10px 20px;
  border-width: 1px;
  border-style: solid;
  border-radius: 5px;
  margin-bottom: -40px;
  position: fixed;
  display: inline-block;
  transform: translateX(-50%);
  z-index: 9001;
  line-height: 28px;
}
.alert-popup-top-margin {
  margin-top: 110px;
}
.alert-popup-container {
  width: 100%;
  text-align: center;
  position: absolute;
}
.success {
  background: #2dac86;
  border: none;
}
.danger {
  background: red;
  border-color: rgb(214, 0, 0);
}
.alert-text-part {
  color: white;
  font-weight: bold;
}
.alert-button-part {
  width: 20px;
  float: right;
  margin-left: 10px;
}
</style>
