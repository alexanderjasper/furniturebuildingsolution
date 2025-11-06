<template>
  <!-- Section Hero Start -->
  <div class="hero">
    <div class="container">
      <div class="row">
        <div class="col-md-6">
          <div class="hero__content">
            <h1>Reoler og skabe – designet af dig</h1>
            <p>Med vores banebrydende online designværktøj kan du designe din reol eller dit skab i ro og mag. Start fra bunden, eller lad værktøjet hjælpe dig på vej. Dit møbel kan tilpasses præcis, som du vil have det!</p>
            <router-link class="btn" to="/reoldesigner">Design selv nu</router-link>
            <div style="margin-top: 35px">
              <!-- TrustBox widget - Starter -->
              <div
                class="trustpilot-widget"
                data-locale="da-DK"
                data-template-id="5613c9cde69ddc09340c6beb"
                data-businessunit-id="5e7b1baa4936170001c1199f"
                data-style-height="100%"
                data-style-width="100%"
                data-theme="light"
                id="trustbox"
              >
                <a
                  href="https://dk.trustpilot.com/review/shelfer.dk"
                  target="_blank"
                  rel="noopener"
                ></a>
              </div>
              <!-- End TrustBox widget -->
            </div>
          </div>
        </div>
        <div class="col-md-5 offset-md-1 hero__image">
          <img class src="/resources/images/hero-bookcase.png" alt />
        </div>
      </div>
    </div>
  </div>
  <!-- Section Hero End -->
</template>

<script>
export default {
  name: "hero",
  components: {},
  data() {
    return {};
  },
  computed: {},
  methods: {},
  mounted() {
    var aScript = document.createElement("script");
    aScript.type = "text/javascript";
    aScript.src =
      "//widget.trustpilot.com/bootstrap/v5/tp.widget.bootstrap.min.js";
    aScript.async = "true";
    document.head.appendChild(aScript);
    aScript.onload = function () {
      var trustbox = document.getElementById("trustbox");
      window.Trustpilot.loadFromElement(trustbox);
    };
  },
};
</script>

<style scoped>
/*--------------------------------------------------------------
# Hero styling
--------------------------------------------------------------*/
.hero {
  width: 100%;
  position: relative;
  background: #cbe6df;
}

.hero__content {
  z-index: 2;
  padding: 45px 0;
  position: relative;
}

.hero__image {
  padding: 0 30px;
}

@media only screen and (min-width: 992px) {
  .hero__image {
    padding: 0;
  }
  .hero__content {
    padding: 80px 0;
  }
  .hero {
    margin-top: 120px;
    margin-bottom: 150px;
  }
  .hero__image img {
    -o-object-fit: contain;
    object-fit: contain;
    max-height: 606px;
    margin-top: -75px;
    margin-bottom: -75px;
    box-shadow: 0 0 8px 0 #888888;
  }
}
.hero__content p {
  margin-bottom: 0;
}
.hero__content .btn {
  margin-top: 30px;
}
</style>
