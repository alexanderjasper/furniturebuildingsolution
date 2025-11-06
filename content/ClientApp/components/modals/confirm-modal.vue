<template>
  <div class="custom-modal">
    <div class="modal-content" style="max-width: 400px" v-click-outside="close">
      <h3>{{title}}</h3>
      <div>{{body}}</div>
      <div class="modal-button-container">
        <button v-on:click="close()" class="btn btn-secondary">{{cancelText}}</button>
        <button v-on:click="confirm()" class="btn">{{confirmText}}</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "confirm-modal",
  props: {
    title: String,
    body: String,
    confirmText: String,
    cancelText: String
  },
  methods: {
    confirm: function() {
      this.$emit("confirm");
    },
    close: function() {
      this.$emit("cancel");
    }
  }
};
</script>