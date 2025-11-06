<template>
  <div class="custom-modal">
    <div
      class="modal-content"
      style="text-align: center; max-height: 80%; max-width: 700px"
      v-click-outside="close"
    >
      <div class="modal-close-button">
        <button v-on:click="close()" class="s-btn s-btn-transparent">
          <icon icon="times" class="fa-2x"></icon>
        </button>
      </div>
      <h3>{{promptText}}</h3>
      <div style="margin-top: 20px">
        <router-link to="/log-ind" class="btn" exact-active-class="active">Log ind</router-link>
        <router-link to="/opret-konto" class="btn" exact-active-class="active">Opret konto</router-link>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "require-login-modal",
  data() {
    return {};
  },
  props: {
    promptText: String,
  },
  computed: {},
  methods: {
    close: function () {
      this.$emit("close");
    },
  },
};
</script>
