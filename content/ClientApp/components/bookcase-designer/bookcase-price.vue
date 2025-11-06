<template>
  <span class="sales-price-text">{{price}} kr.</span>
</template>
<script>
import dataTools from "./tools/data-tools";
export default {
  name: "bookcase-price",
  props: {
    price: Number
  }
};
</script>
<style>
.sales-price-text {
  border-bottom: solid 1px gray;
}
</style>
