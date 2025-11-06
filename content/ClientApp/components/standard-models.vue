<template>
  <div>
    <div v-if="pageLoaded">
      <div class="standard-box">
        <div class="container">
          <div class="row" style="text-align: center">
            <div class="col-12">
              <h1>Standardmodeller{{selectedCategory ? (' – ' + selectedCategory.name):''}}</h1>
            </div>
          </div>
          <div class="row">
            <div class="col-md-3" style="margin-bottom: 30px">
              <div class="standard-model-category-container">
                <div
                  :class="'standard-model-category ' + (selectedCategoryId ? '' : 'selected-category')"
                  v-on:click="selectedCategoryId = null"
                >Alle</div>
                <div
                  :class="'standard-model-category ' + (selectedCategoryId===category.id ? 'selected-category': '')"
                  v-for="category in categories"
                  :key="category.id"
                  v-on:click="selectedCategoryId = category.id"
                >{{category.name}}</div>
              </div>
            </div>
            <div class="col-md-9">
              <div class="row">
                <div
                  class="product-container col-md-4 col-12"
                  v-for="(product,index) in products"
                  :key="index"
                >
                  <h2 class="product-title">{{product.title}}</h2>
                  <router-link :to="'/standardmodeller/' + product.id">
                    <img
                      :src="'/resources/images/standard-models/' + product.imageFilename + '_mini.jpg'"
                    />
                  </router-link>
                  <div id="availability">
                    Lagerstatus: Klar til produktion 
                    <icon icon="check" class="brand-color" />
                    </br>(fremstilles på ordre)
                  </div>
                  <div class="price-tag">{{product.price}},-</div>
                  <router-link class="btn" :to="'/standardmodeller/' + product.id">Vis</router-link>
                  <button v-on:click="addToCart(product)" class="btn btn-brand">
                    Køb nu
                    <icon fixed-width icon="shopping-cart" />
                  </button>
                  <payment-info></payment-info>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <inspiration></inspiration>
      <page-footer></page-footer>
    </div>
    <full-page-spinner v-else></full-page-spinner>
  </div>
</template>

<script>
import { productService, localstorageService } from "../_services";
import inspiration from "./sections/home/inspiration";
import paymentInfo from "./payment-info";
import pageFooter from "./page-footer";
import { mapActions } from "vuex";
import fullPageSpinner from "./full-page-spinner";

export default {
  name: "standard-models",
  components: {
    inspiration: inspiration,
    "payment-info": paymentInfo,
    "page-footer": pageFooter,
    "full-page-spinner": fullPageSpinner,
  },
  data() {
    return {
      selectedCategoryId: 0,
      categories: null,
      products: null,
      categoriesLoaded: false,
      standardModelsLoaded: false,
    };
  },
  created() {
    productService.getCategories().then((categoriesData) => {
      this.categories = categoriesData;
      this.categoriesLoaded = true;
    });
    productService.getStandardModels().then((standardModelsData) => {
      this.products = standardModelsData;
      this.standardModelsLoaded = true;
    });
  },
  updated() {
    this.setTitlesSameHeight();
  },
  methods: {
    ...mapActions("alert", ["success"]),
    addToCart: function (product) {
      localstorageService.addToCart(product.bookcaseId, product.id);
      var numberOfCartItems = localstorageService.getNumberOfCartItems();
      this.$store.commit("setNumberOfCartItems", numberOfCartItems);
      this.success("Føjet til indkøbskurv");
    },
    setTitlesSameHeight: function () {
      var maxHeight = 0;

      var elements = Array.from(
        document.getElementsByClassName("product-title")
      );

      elements.forEach((elt) => {
        var height = parseInt(elt.clientHeight, 10);
        if (height > maxHeight) {
          maxHeight = height;
        }
      });

      elements.forEach((elt) => {
        elt.style.height = maxHeight + "px";
      });
    },
  },
  computed: {
    selectedCategory: function () {
      if (this.categories) {
        return this.categories.find((c) => c.id === this.selectedCategoryId);
      } else {
        return null;
      }
    },
    pageLoaded: function () {
      return this.categoriesLoaded && this.standardModelsLoaded;
    },
  },
  watch: {
    selectedCategoryId: function (categoryId) {
      this.standardModelsLoaded = false;
      productService
        .getStandardModels(categoryId)
        .then((standardModelsData) => {
          this.products = standardModelsData;
          this.standardModelsLoaded = true;
        });
    },
  },
};
</script>

<style scoped>
.standard-model-category-container {
  margin: 0 auto;
  display: table;
}
.standard-model-category {
  font-size: 20px;
}
.product-container {
  text-align: center;
  margin-bottom: 30px;
  margin-top: 30px;
}
.selected-category {
  font-weight: bold;
}
.product-title {
  font-weight: 100;
  font-size: 25px;
}
</style>
