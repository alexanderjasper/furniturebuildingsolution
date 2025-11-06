<template>
  <div>
    <div class="standard-box align-center">
      <div class="stepwise-form-container">
        <div class="stepwise-form-step">
          <div class="stepwise-form-header">Indkøbskurv</div>
          <div class="stepwise-form-content" v-if="!showDeliveryInformation">
            <shopping-cart
              :editable="!orderConfirmed"
              @setValid="setShoppingCartValid"
              @proceedToNextStep="proceedToDeliveryInformation"
            ></shopping-cart>
          </div>
        </div>
        <div class="stepwise-form-step">
          <div class="stepwise-form-header">Forsendelsesoplysninger</div>
          <div class="stepwise-form-content" v-show="showDeliveryInformation">
            <delivery-information
              :editable="!orderConfirmed"
              @setValid="setDeliveryInformationValid"
              @setData="setDeliveryInformationData"
              @proceedToNextStep="showOrderConfirmationModal"
            ></delivery-information>
            <div>
              <label class="b-contain ml-auto">
                <span>
                  Jeg accepterer
                  <router-link
                    target="_blank"
                    to="/handelsbetingelser-og-persondatapolitik"
                  >Shelfers handelsbetingelser og persondatapolitik</router-link>.
                </span>
                <input type="checkbox" v-model="termsAccepted" />
                <div class="b-input"></div>
              </label>
            </div>
            <div style="text-align:right">
              <button v-on:click="backToShoppingCart()" class="btn btn-secondary">Tilbage</button>
              <button
                type="button"
                :disabled="!deliveryInformationValid || showTermsConditionsModal || !termsAccepted || processing"
                class="btn"
                v-on:click="showOrderConfirmationModal()"
              >
                Bekræft bestilling
                <icon fixed-width icon="check" class="fa-lg" />
              </button>
              <payment-info></payment-info>
            </div>
          </div>
        </div>
      </div>
      <order-confirmation-modal
        v-if="orderConfirmationModalShouldShow"
        @close="hideOrderConfirmationModal"
        @submit="confirmOrder"
      ></order-confirmation-modal>
      <!-- <terms-conditions-modal v-if="showTermsConditionsModal" @close="hideTermsConditionsModal"></terms-conditions-modal> -->
    </div>
    <newsletter></newsletter>
    <page-footer></page-footer>
  </div>
</template>

<script>
import { localstorageService, orderService } from "../../_services";
import shoppingCart from "./shopping-cart";
import deliveryInformation from "./delivery-information";
import orderConfirmationModal from "./order-confirmation-modal";
import pageFooter from "../page-footer";
import paymentInfo from "../payment-info";
import newsletter from "../sections/products/newsletter";
// import termsConditionsModal from "./terms-conditions-modal";
export default {
  name: "stepwise-checkout-page",
  components: {
    "shopping-cart": shoppingCart,
    "payment-info": paymentInfo,
    "delivery-information": deliveryInformation,
    "order-confirmation-modal": orderConfirmationModal,
    "page-footer": pageFooter,
    newsletter: newsletter,
  },
  data() {
    return {
      showDeliveryInformation: false,
      shoppingCartValid: false,
      deliveryInformationValid: false,
      orderConfirmationModalShouldShow: false,
      showTermsConditionsModal: false,
      orderConfirmed: false,
      deliveryInformation: null,
      termsAccepted: false,
      processing: false,
    };
  },
  created() {},
  methods: {
    setShoppingCartValid: function (value) {
      this.shoppingCartValid = value;
    },
    setDeliveryInformationValid: function (value) {
      this.deliveryInformationValid = value;
    },
    setDeliveryInformationData: function (data) {
      this.deliveryInformation = data;
    },
    proceedToDeliveryInformation: function () {
      this.showDeliveryInformation = true;
    },
    backToShoppingCart: function () {
      this.showDeliveryInformation = false;
    },
    showOrderConfirmationModal: function () {
      this.orderConfirmationModalShouldShow = true;
    },
    hideOrderConfirmationModal: function () {
      this.orderConfirmationModalShouldShow = false;
    },
    hideTermsConditionsModal: function () {
      this.showTermsConditionsModal = false;
    },
    confirmOrder: function () {
      this.processing = true;
      var orderData = {
        orderItems: this.getOrderItems(),
        firstName: this.deliveryInformation.firstName,
        lastName: this.deliveryInformation.lastName,
        companyName: this.deliveryInformation.companyName,
        vatNumber: this.deliveryInformation.vatNumber,
        emailAddress: this.deliveryInformation.emailAddress,
        address: this.deliveryInformation.address,
        shippingAddress: this.deliveryInformation.shippingAddress,
      };
      orderService.placeOrder(orderData).then(this.finishOrder);
    },
    finishOrder: function (order) {
      localstorageService.storeOrderConfirmation(order);
      localstorageService.resetShoppingCart();
      this.$store.commit("setNumberOfCartItems", 0);
      this.$router.push("/ordre-bekraeftet");
      this.addDataLayer(order);
    },
    addDataLayer: function (order) {
      function reducer(previousValue, currentValue) {
        return previousValue + currentValue.quantity * currentValue.unitPrice;
      }
      var amount = order.orderItems.reduce(reducer, 0);

      window.dataLayer = window.dataLayer || [];
      window.dataLayer.push({
        event: "purchase",
        ecommerce: {
          purchase: {
            actionField: {
              id: order.orderNumber, //transaktions id som dynamisk skal indsættes
              revenue: amount,
            },
            products: order.orderItems.map((item) => {
              return {
                name: "Reol " + item.bookcase.id,
                id: item.bookcase.id,
                price: item.unitPrice,
                quantity: item.quantity,
              };
            }),
          },
        },
      });
    },
    getOrderItems: function () {
      var shoppingCart = localstorageService.getCart();
      var orderItems = [];
      Object.keys(shoppingCart).forEach((key) =>
        orderItems.push({
          bookcase: { id: key },
          quantity: shoppingCart[key].quantity,
        })
      );
      return orderItems;
    },
  },
  computed: {},
};
</script>

<style>
.stepwise-form-header {
  background-color: #cbe6df;
  font-size: 20px;
  padding: 10px;
}
.stepwise-form-container {
  max-width: 1200px;
  margin: 0 auto 0 auto;
}
.stepwise-form-step {
  border: solid 1px darkgrey;
  margin: -1px;
  padding: 30px;
}
.stepwise-form-content.hidden {
  display: none;
}
@media (max-width: 800px) {
  .stepwise-form-header {
    font-size: 16px;
  }
  .stepwise-form-container {
    font-size: 16px;
  }
  .stepwise-form-step {
    border: none;
    margin: 0;
    padding: 30px 0;
  }
}
</style>
