<template>
  <div class="custom-modal">
    <div class="modal-content" style="text-align: center; max-height: 80%; max-width: 700px">
      <h3>Bekræft afslutning</h3>
      <div style="margin-bottom: 10px">Vil du gemme reolen, inden du afslutter?</div>
      <div>
        <button v-on:click="exit(false)" class="btn btn-secondary">Luk uden at gemme</button>
        <button v-on:click="exit(true)" class="btn">Gem</button>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "exit-save-modal",
  data() {
    return {};
  },
  computed: {},
  methods: {
    exit: function(shouldSave) {
      this.$emit("exit", shouldSave);
    }
  }
};
</script>
