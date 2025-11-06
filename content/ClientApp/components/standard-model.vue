<template>
  <div>
    <div class="standard-box align-center">
      <div v-if="standardModel">
        <h1 id="title">{{standardModel.title}}</h1>
        <div class="container">
          <div class="row">
            <div class="col-12 col-md-6">
              <img
                :src="'/resources/images/standard-models/' + standardModel.imageFilename + '.jpg'"
              />
            </div>
            <div class="col-12 col-md-6 my-auto">
              <div id="id" style="display: none">{{standardModel.bookcaseId}}</div>
              <div id="description" class="text-section large-text">{{standardModel.description}}</div>

              <div id="material" class="text-section">Materiale: {{standardModel.material}}</div>

              <div id="availability">
                Lagerstatus: Klar til produktion
                <icon icon="check" class="brand-color" />
                <br />(fremstilles på ordre)
              </div>
              <br />
              <div id="delivery-time">Leveringstid: 1-4 uger</div>

              <div id="mpn">Varenummer: {{standardModel.bookcaseId}}</div>

              <div id="price" class="price-tag">{{standardModel.price}},-</div>

              <div>
                <router-link class="btn" to="/standardmodeller">
                  <icon fixed-width icon="arrow-left" />Tilbage
                </router-link>
                <button v-on:click="addToCart(standardModel)" class="btn btn-brand">
                  Køb nu
                  <icon fixed-width icon="shopping-cart" />
                </button>
              </div>
              <payment-info></payment-info>
            </div>
          </div>
        </div>
      </div>
    </div>
    <newsletter></newsletter>
    <page-footer></page-footer>
  </div>
</template>

<script>
import newsletter from "./sections/products/newsletter";
import pageFooter from "./page-footer";
import paymentInfo from "./payment-info";
import { productService, localstorageService } from "../_services";
import { mapActions } from "vuex";

export default {
  name: "standard-model",
  props: {
    modelId: String,
  },
  components: {
    newsletter: newsletter,
    "payment-info": paymentInfo,
    "page-footer": pageFooter,
  },
  data() {
    return {
      standardModel: null,
    };
  },
  created() {
    productService.getStandardModel(this.modelId).then((standardModelData) => {
      this.standardModel = standardModelData;
    });
  },
  methods: {
    ...mapActions("alert", ["success"]),
    addToCart: function (product) {
      localstorageService.addToCart(product.bookcaseId, product.id);
      var numberOfCartItems = localstorageService.getNumberOfCartItems();
      this.$store.commit("setNumberOfCartItems", numberOfCartItems);
      this.success("Føjet til indkøbskurv");
    },
  },
};
</script>

<style>
</style>
