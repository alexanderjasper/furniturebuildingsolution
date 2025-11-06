<template>
  <div class="custom-modal">
    <div class="modal-content" style="max-width: 400px" v-click-outside="close">
      <h3>Bekræft sletning</h3>
      <div>Er du sikker på, at du ønsker at slette reolen?</div>
      <div class="modal-button-container">
        <button v-on:click="close()" class="btn btn-secondary">Annuller</button>
        <button v-on:click="confirm()" class="btn">Slet</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "confirm-delete-bookcase-modal",
  methods: {
    confirm: function() {
      this.$emit("confirmDelete");
    },
    close: function() {
      this.$emit("close");
    }
  }
};
</script>