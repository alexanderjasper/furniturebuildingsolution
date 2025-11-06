<template>
  <div>
    <div class="standard-box align-center">
      <h1>Tillykke!</h1>
      <div
        class="text-section"
      >Din ordre er gennemført. Ordrebekræftelsen er sendt til dig via e-mail. Ordrebekræftelsen kan desuden hentes under menupunktet "Ordrer" eller her:</div>
      <div class="text-section">
        <button class="btn" v-on:click="openOrderConfirmation()">Åbn ordrebekræftelse</button>
      </div>
      <div
        class="text-section"
        style="margin-top: 10px"
      >Du vil inden længe modtage en faktura for dit køb. Fakturaen kan betales med kreditkort eller MobilePay.</div>
      <h2 style="margin-top: 30px">Ordredetaljer</h2>
      <div>
        Ordrenummer:
        <span id="conf-order-numer">{{orderConfirmation.orderNumber}}</span>
      </div>
      <div>{{orderConfirmation.firstName}} {{orderConfirmation.lastName}}</div>
      <div>
        <span v-if="orderConfirmation.companyName">
          {{orderConfirmation.companyName}}
          <span v-if="orderConfirmation.vatNumber">-</span>
        </span>
        <span v-if="orderConfirmation.vatNumber">CVR: {{orderConfirmation.vatNumber}}</span>
      </div>
      <div>Adresse: {{orderConfirmation.address.addressString}}</div>
      <div
        v-if="orderConfirmation.shippingAddress"
      >Forsendelsesadresse: {{orderConfirmation.shippingAddress.addressString}}</div>

      <div class="table-responsive small-table-container">
        <table class="table">
          <tbody>
            <tr>
              <th>Vare</th>
              <th>Pris</th>
              <th>Antal</th>
              <th>I alt</th>
            </tr>
            <tr v-for="(orderItem) in orderConfirmation.orderItems" :key="orderItem.bookcaseId">
              <td>
                <b>Reol:</b>
                <span id="conf-bookcase-name">{{orderItem.bookcase.name}}</span>
                <br />
                <span style="font-size: 12px">{{orderItem.bookcase.material.name}}</span>
              </td>
              <td id="conf-unit-price">{{orderItem.unitPrice}} kr.</td>
              <td id="conf-quantity">{{orderItem.quantity}}</td>
              <td id="conf-line-total">{{orderItem.unitPrice * orderItem.quantity}} kr.</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <newsletter></newsletter>
    <page-footer></page-footer>
  </div>
</template>

<script>
import { localstorageService, orderService } from "../../_services";
import pageFooter from "../page-footer";
import newsletter from "../sections/products/newsletter";

export default {
  components: {
    "page-footer": pageFooter,
    newsletter: newsletter,
  },
  name: "order-confirmation-page",
  data() {
    return {
      orderConfirmation: null,
    };
  },
  created() {
    this.orderConfirmation = localstorageService.getOrderConfirmation();
  },
  methods: {
    openOrderConfirmation: function () {
      window.open("/ordrebekraeftelse", "_blank");
    },
  },
};
</script>

<style></style>
