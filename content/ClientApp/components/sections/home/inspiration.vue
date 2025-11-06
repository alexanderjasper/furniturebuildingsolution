<template>
  <!-- Section Footer Start -->
  <div class="inspiration">
    <div class="container">
      <div class="footer__inspiration">
        <div class="row">
          <div class="col-md-5 footer__wrapper">
            <span class="hashtag hashtag--yellow">#inspiration</span>
            <h3 class="footer__title">
              Tænk at få det
              <b>præcis som du vil have det</b>
            </h3>
            <router-link class="text-link" to="/produkter">Se eksempler fra vores glade kunder</router-link>
          </div>
          <div class="col-md-6 offset-md-1">
            <div class="footer__image">
              <img src="/resources/images/hero-bookcase.png" alt="Reolsystem" />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- Section Footer End -->
</template>

<script>
export default {
  name: "inspiration",
  components: {},
  data() {
    return {};
  },
  computed: {
    currentYear: function() {
      var today = new Date();
      return today.getFullYear();
    }
  },
  methods: {}
};
</script>

<style>
.inspiration {
  margin-top: 70px;
}
@media only screen and (min-width: 768px) {
  .inspiration {
    padding-top: 70px;
  }
}
</style>

