<template>
  <div>
    <hero></hero>

    <product-overview></product-overview>

    <newsletter></newsletter>

    <page-footer></page-footer>
  </div>
</template>

<script>
import emailSubscribeModal from "./email-subscribe-modal";

import hero from "./sections/products/hero";
import productOverview from "./sections/products/product-overview";
import newsletter from "./sections/products/newsletter";
import pageFooter from "./page-footer";

export default {
  name: "products",
  components: {
    "email-subscribe-modal": emailSubscribeModal,

    hero: hero,
    "product-overview": productOverview,
    newsletter: newsletter,
    "page-footer": pageFooter,
  },
  data() {
    return {
      emailAddress: "",
      showEmailSubscribeModal: false,
    };
  },
  methods: {
    showHideEmailSubscribeModal: function (shouldShow) {
      this.showEmailSubscribeModal = shouldShow;
    },
    resetEmail: function () {
      this.emailAddress = "";
    },
  },
};
</script>

<style>
</style>
