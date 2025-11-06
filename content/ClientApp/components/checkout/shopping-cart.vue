<template>
  <div>
    <div class="table-responsive">
      <table class="table shopping-cart-table">
        <tbody>
          <tr v-if="orderItems.length > 0">
            <th class="bookcase-design-column"></th>
            <th>Vare</th>
            <th>Pris</th>
            <th>Antal</th>
            <th>I alt</th>
            <th></th>
          </tr>
          <tr v-for="(orderItem) in orderItems" :key="orderItem.bookcase.id">
            <td class="bookcase-design-column" id="cart-design">
              <open-bookcase-button
                :size="100"
                :bookcaseData="orderItem.bookcase"
                :margin="0.25"
                fillColor="transparent"
              ></open-bookcase-button>
            </td>
            <td id="cart-bookcase-name">Reol: {{orderItem.bookcase.name}}</td>
            <td id="cart-unit-price">{{orderItem.salesPrice}} kr.</td>
            <td>
              <icon
                v-if="editable"
                v-on:click="addItem(orderItem)"
                fixed-width
                icon="plus"
                class="fa-lg"
              />
              <span id="cart-quantity">{{orderItem.numberOfItems}}</span>
              <icon
                v-if="editable"
                v-on:click="removeItem(orderItem)"
                fixed-width
                icon="minus"
                class="fa-lg"
              />
            </td>
            <td id="cart-line-total">{{orderItem.salesPrice * orderItem.numberOfItems}} kr.</td>
            <td id="cart-remove-line">
              <icon
                v-if="editable"
                v-on:click="removeLine(orderItem)"
                fixed-width
                icon="trash-alt"
                class="fa-lg"
              />
            </td>
          </tr>
          <tr v-if="!orderItems.length > 0">
            <td colspan="6">
              Der er ingen varer i indkøbskurven. Gå til
              <router-link to="/reoldesigner">Reoldesigneren</router-link>
              <span>for at designe og bestille en reol.</span>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div class="cart-subtotal">
      <table class="table-no-borders ml-auto">
        <tbody>
          <tr>
            <td>
              <b>
                Subtotal (
                <span id="cart-number-of-items">{{numberOfCartItems}}</span> varer):
              </b>
            </td>
            <td id="cart-total-incl-vat">{{totalPrice.toFixed(2).replace(".", ",")}} kr.</td>
          </tr>
          <tr>
            <td>
              <b>Heraf moms:</b>
            </td>
            <td id="cart-vat">{{vatAmount}} kr.</td>
          </tr>
        </tbody>
      </table>
      <div
        style="margin-top: 20px"
      >+ Levering: 20 kr. pr. km fra vores værksted i Bådehavnsgade 42, 2450 København SV</div>
      <div>Forventet leveringstid: 1-4 uger</div>
      <div>
        <button
          type="button"
          v-if="numberOfCartItems > 0"
          class="btn"
          style="margin-top: 10px"
          v-on:click="proceedToNextStep()"
        >
          Fortsæt til bestilling
          <icon fixed-width icon="cash-register" class="fa-lg" />
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { bookcaseService, localstorageService } from "../../_services";
import openBookcaseButton from "../bookcase-designer/graphic-buttons/open-bookcase-button";
import requireLoginModal from "../bookcase-designer/modals/require-login-modal";
import fullPageSpinner from "../full-page-spinner";
import dataTools from "../bookcase-designer/tools/data-tools";
import BookcaseClass from "../../extensions/bookcaseClasses.js";
export default {
  name: "shopping-cart",
  components: {
    "open-bookcase-button": openBookcaseButton,
    "full-page-spinner": fullPageSpinner,
    "require-login-modal": requireLoginModal,
  },
  mixins: [dataTools],
  props: {
    editable: Boolean,
  },
  data() {
    return {
      loading: true,
      shoppingCart: {},
      orderItems: [],
      activeSection: 0,
    };
  },
  created() {
    this.shoppingCart = localstorageService.getCart();
    Object.keys(this.shoppingCart).forEach(this.addOrderItemFromKey);
    this.orderItems.sort(function (a, b) {
      return a.bookcase.id - b.bookcase.id;
    });
    this.loading = false;
  },
  methods: {
    addOrderItemFromKey: function (key) {
      this.addOrderItem(key, this.shoppingCart[key]);
    },
    addOrderItem: function (bookcaseId, cartItem) {
      var bookcaseData;
      if (cartItem.standardModelId) {
        bookcaseService.getPublic(bookcaseId).then(
          (bookcaseDto) => {
            this.addOrderItemFromBookcase(bookcaseDto, cartItem);
          },
          (error) => {
            localstorageService.removeFromCart(bookcaseId);
          }
        );
      } else {
        bookcaseService.get(bookcaseId).then(
          (bookcaseDto) => {
            this.addOrderItemFromBookcase(bookcaseDto, cartItem);
          },
          (error) => {
            localstorageService.removeFromCart(bookcaseId);
          }
        );
      }
    },
    addOrderItemFromBookcase(bookcaseDto, cartItem) {
      var bookcase = new BookcaseClass(null);
      bookcase.initialize(bookcaseDto);
      this.orderItems.push({
        bookcase: bookcase,
        salesPrice: bookcase.salesPrice,
        numberOfItems: cartItem.quantity,
      });
    },
    addItem: function (orderItem) {
      orderItem.numberOfItems += 1;
      localstorageService.setNumberOfItems(
        orderItem.bookcase.id,
        orderItem.numberOfItems
      );
      this.refreshTotalNumberOfItems();
    },
    removeItem: function (orderItem) {
      if (orderItem.numberOfItems > 1) {
        orderItem.numberOfItems -= 1;
        localstorageService.setNumberOfItems(
          orderItem.bookcase.id,
          orderItem.numberOfItems
        );
      } else {
        var index = this.orderItems.indexOf(orderItem);
        this.orderItems.splice(index, 1);
        localstorageService.removeFromCart(orderItem.bookcase.id);
      }
      this.refreshTotalNumberOfItems();
    },
    removeLine: function (orderItem) {
      var index = this.orderItems.indexOf(orderItem);
      this.orderItems.splice(index, 1);
      localstorageService.removeFromCart(orderItem.bookcase.id);
      this.refreshTotalNumberOfItems();
    },
    refreshTotalNumberOfItems: function () {
      var numberOfCartItems = localstorageService.getNumberOfCartItems();
      this.$store.commit("setNumberOfCartItems", numberOfCartItems);
    },
    proceedToNextStep: function () {
      this.$emit("proceedToNextStep");
    },
  },
  computed: {
    totalPrice: function () {
      var price = 0;
      this.orderItems.forEach(
        (orderItem) => (price += orderItem.salesPrice * orderItem.numberOfItems)
      );
      this.$store.commit("setShoppingCartTotalPrice", price);
      return price;
    },
    numberOfCartItems: function () {
      return this.$store.state.numberOfCartItems;
    },
    vatAmount: function () {
      var amount = Math.round(this.totalPrice * 0.2 * 100) / 100;
      return amount.toFixed(2).replace(".", ",");
    },
  },
  watch: {
    orderItems: function (value) {
      if (value.length > 0) {
        this.$emit("setValid", true);
      } else {
        this.$emit("setValid", false);
      }
    },
  },
};
</script>

<style>
.cart-subtotal {
  text-align: right;
  padding-right: 5px;
}
@media (max-width: 800px) {
  .bookcase-design-column {
    display: none;
  }
}
</style>
