<template>
  <div class="custom-modal">
    <div class="modal-content" style="max-width: 900px" v-click-outside="close">
      <h3>{{data.title}}</h3>
      <div class="product-container">
        <div class="location" v-if="data.location">{{data.location}}</div>
        <div class="price" v-if="data.price">
          <pre>{{data.price}}</pre>
        </div>
        <div class="story" v-if="data.story">
          <icon class="fa-xs" icon="quote-left" />
          {{data.story}}
          <icon class="fa-xs" icon="quote-right" />
        </div>
        <div class="description" v-if="data.description">{{data.description}}</div>
        <div class="image-container" v-if="data.largeImage">
          <img :src="data.largeImage" />
        </div>
      </div>

      <div>
        <button v-on:click="close()" class="btn modal-bottom-close-button">Luk</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "see-more-modal",
  props: {
    data: Object
  },
  methods: {
    close: function() {
      this.$emit("close");
    }
  }
};
</script>
<style scoped>
.product-container .location {
  font-size: 14px;
}
.product-container .story {
  font-style: italic;
  font-size: 13px;
  margin: 15px 0;
}
.product-container .image-container {
  max-width: 80%;
  margin: 30px auto;
}
.product-container pre {
  font-size: inherit;
}
</style>
