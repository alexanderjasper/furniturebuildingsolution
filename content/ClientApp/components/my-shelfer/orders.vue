<template>
  <div>
    <div class="standard-box">
      <h1 class="page-header">Ordrer</h1>
      <full-page-spinner v-if="loading"></full-page-spinner>
      <div v-if="!loading" class="table-responsive medium-table-container">
        <table class="table">
          <tbody>
            <tr>
              <th>Ordrenummer</th>
              <th>Ordredato</th>
              <th>Status</th>
              <th>Samlet pris</th>
              <th>Ordrebekræftelse</th>
            </tr>
            <tr v-for="(order, index) in orderList" :key="index">
              <td>{{order.orderNumber}}</td>
              <td>{{getDisplayDate(order.orderTime)}}</td>
              <td>{{getStatus(order.orderStatus)}}</td>
              <td>{{getTotalPrice(order)}} kr.</td>
              <td>
                <button class="btn" v-on:click="openOrderConfirmation(order.orderNumber)">Hent</button>
              </td>
            </tr>
            <tr v-if="!orderList.length > 0">
              <td
                colspan="6"
              >Du har endnu ingen ordrer. Gå til "Mine reoler" for at designe og bestille en reol.</td>
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
import { orderService, localstorageService } from "../../_services";
import fullPageSpinner from "../full-page-spinner";
import pageFooter from "../page-footer";
import newsletter from "../sections/products/newsletter";
export default {
  components: {
    "full-page-spinner": fullPageSpinner,
    "page-footer": pageFooter,
    newsletter: newsletter,
  },
  data() {
    return {
      loading: true,
      orderList: [],
    };
  },
  props: {},
  created() {
    this.getOrders();
  },
  computed: {},
  methods: {
    getOrders: function () {
      orderService.getAll().then((orderList) => {
        this.orderList = orderList;
        this.loading = false;
      });
    },
    getDisplayDate: function (dateString) {
      var date = new Date(dateString);
      return date.toLocaleDateString("da-DK", {
        day: "numeric",
        month: "long",
        year: "numeric",
      });
    },
    getStatus(orderStatusEnum) {
      if (orderStatusEnum === 0) return "Modtaget";
    },
    getTotalPrice: function (order) {
      var price = 0;
      order.orderItems.forEach(
        (orderItem) => (price += orderItem.quantity * orderItem.unitPrice)
      );
      return price;
    },
    openOrderConfirmation: function (orderNumber) {
      this.orderData = orderService
        .getOrderConfirmation(orderNumber)
        .then((orderData) => {
          localstorageService.storeOrderConfirmation(orderData);
          this.$router.push("/ordrebekraeftelse");
        });
    },
  },
};
</script>

<style>
.table > tbody > tr > td {
  vertical-align: middle;
}
.table > tbody > tr > th {
  vertical-align: middle;
}
.date-column {
  min-width: 115px;
}
@media only screen and (max-width: 700px) {
  .hide-on-mobile {
    display: none;
  }
}
</style>
