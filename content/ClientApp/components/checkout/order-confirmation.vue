<template>
  <div>
    <full-page-spinner v-if="loading"></full-page-spinner>
    <div v-if="!loading" class="a4-page">
      <div>
        <img src="/resources/images/logo--dark.png" class="center-logo" />
      </div>
      <h1>Ordrebekræftelse</h1>
      <h4>Ordrenummer: {{orderData.orderNumber}}</h4>
      <div class="section">
        <div>
          <b>Kunde:</b>
        </div>
        <div>{{orderData.firstName}} {{orderData.lastName}}</div>
        <div>
          <span v-if="orderData.companyName">
            {{orderData.companyName}}
            <span v-if="orderData.vatNumber">-</span>
          </span>
          <span v-if="orderData.vatNumber">CVR: {{orderData.vatNumber}}</span>
        </div>
        <div>{{orderData.address.addressString}}</div>
        <div
          v-if="orderData.shippingAddress"
        >Forsendelsesadresse: {{orderData.shippingAddress.addressString}}</div>
      </div>

      <div>
        <table class="table">
          <tbody>
            <tr>
              <th width="20%">Vare</th>
              <th width="20%">Design</th>
              <th width="20%">Pris</th>
              <th width="20%">Antal</th>
              <th width="20%">I alt</th>
            </tr>
            <tr v-for="(orderItem) in orderData.orderItems" :key="orderItem.bookcaseId">
              <td>
                <b>Reol:</b>
                <span id="conf-bookcase-name">{{orderItem.bookcase.name}}</span>
                <br />
                <span style="font-size: 12px">{{orderItem.bookcase.material.name}}</span>
              </td>
              <td>
                <open-bookcase-button
                  height="120px"
                  :size="200"
                  :bookcaseData="orderItem.bookcase"
                  :margin="0.25"
                  fillColor="transparent"
                ></open-bookcase-button>
              </td>
              <td id="conf-unit-price">{{orderItem.unitPrice}} kr.</td>
              <td id="conf-quantity">{{orderItem.quantity}}</td>
              <td id="conf-line-total">{{orderItem.unitPrice * orderItem.quantity}} kr.</td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="subtotal">
        <b>
          Subtotal (
          <span id="conf-number-of-items">{{numberOfItems}}</span> varer):
        </b>
        <span id="conf-total-incl-vat">{{subTotal}}</span> kr.
        <br />
        <b>Heraf moms:</b>
        <span id="conf-vat">{{vatAmount}}</span> kr.
      </div>
    </div>
  </div>
</template>

<script>
import { localstorageService } from "../../_services";
import fullPageSpinner from "../full-page-spinner";
import openBookcaseButton from "../bookcase-designer/graphic-buttons/open-bookcase-button";
export default {
  name: "order-confirmation-page",
  components: {
    "full-page-spinner": fullPageSpinner,
    "open-bookcase-button": openBookcaseButton,
  },
  data() {
    return {
      loading: true,
      orderData: null,
    };
  },
  computed: {
    numberOfItems: function () {
      var number = 0;
      for (var i = 0; i < this.orderData.orderItems.length; i++) {
        number += this.orderData.orderItems[i].quantity;
      }
      return number;
    },
    subTotal: function () {
      var number = 0;
      for (var i = 0; i < this.orderData.orderItems.length; i++) {
        number +=
          this.orderData.orderItems[i].unitPrice *
          this.orderData.orderItems[i].quantity;
      }
      return number;
    },
    vatAmount: function () {
      var amount = Math.round(this.subTotal * 0.2 * 100) / 100;
      return amount.toFixed(2).replace(".", ",");
    },
  },
  created() {
    this.$emit("showHideNavMenu", false);
    var orderConfirmation = localstorageService.getOrderConfirmation();
    if (orderConfirmation.orderNumber) {
      this.orderData = orderConfirmation;
      this.loading = false;
    } else {
      this.$router.push("/");
    }
  },
};
</script>

<style media="print">
@page {
  size: auto; /* auto is the initial value */
  margin: 0; /* this affects the margin in the printer settings */
}
.a4-page {
  text-align: center;
  padding: 80px;
}
.center-logo {
  height: 140px;
  margin-bottom: 40px;
}
.section {
  margin-bottom: 20px;
  text-align: left;
}
.subtotal {
  text-align: right;
  padding-right: 30px;
}
</style>
