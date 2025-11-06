<template>
  <div class="custom-modal">
    <div class="modal-content" style="max-width: 350px" v-click-outside="close">
      <h3>Bekræft bestilling</h3>
      <div
        style="text-align: center"
      >Ønsker du at bestille {{numberOfItems}} vare{{numberOfItems > 1 ? "r" : ""}} til en samlet pris af {{totalPrice}} kr.?</div>
      <p style="text-align: right; margin-top: 10px">
        <button v-on:click="close()" class="btn btn-secondary">Annuller</button>
        <button v-on:click="submit()" class="btn">Bestil</button>
      </p>
    </div>
  </div>
</template>

<script>
export default {
  name: "order-confirmation-modal",
  data() {
    return {};
  },
  created() {},
  methods: {
    close: function() {
      this.$emit("close");
    },
    submit: function() {
      this.$emit("submit");
      this.$emit("close");
    }
  },
  computed: {
    numberOfItems: function() {
      return this.$store.state.numberOfCartItems;
    },
    totalPrice: function() {
      return this.$store.state.shoppingCartTotalPrice;
    }
  }
};
</script>

<style>
</style>
