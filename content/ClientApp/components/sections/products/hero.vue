<template>
  <!-- Section Hero Start -->
  <div class="hero hero--product">
    <div class="container">
      <div class="row">
        <div class="col-md-4">
          <div class="hero__content">
            <h1>Få inspiration fra vores glade kunder</h1>
            <p>Se vores kunders smukke designs, og få inspiration til at lave dit eget design.</p>
            <div>
              <router-link class="btn" to="/reoldesigner/vaelg">Design din egen</router-link>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="hero__image col-md-7">
      <div class="image-wrapper">
        <img style="width: 100%" src="/resources/images/white-bookcase.jpg" alt />
      </div>
      <div class="image__gradient"></div>
      <!-- Disabled so far since we will ask our customers personally for now
      <div class="hero__share-design">
        <h2>Del dit design</h2>
        <p>Har du designet en reol som du gerne vil dele med os og vores kunder? - Få din lagt op på vores side.</p>
        <a href="#" class="btn">Del dit design med os</a>
      </div>-->
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
  methods: {}
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
@media only screen and (min-width: 768px) {
  .hero__content {
    padding: 105px 0;
  }
}
.hero__content p {
  margin-bottom: 0;
}
.hero__content .btn {
  margin-top: 30px;
}

.hero__image {
  position: relative;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  z-index: 1;
  background: #ffffff;
}
@media only screen and (min-width: 768px) {
  .hero__image {
    position: absolute;
    top: 0;
    right: 0;
    height: 100%;
  }
  .hero__image img {
    position: absolute;
    height: 100%;
    left: 0;
    -o-object-fit: contain;
    object-fit: contain;
  }
}

.hero--product .hero__image {
  display: block;
}
@media only screen and (min-width: 768px) {
  .hero--product .hero__image {
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
  }
}
.hero--product .hero__image .image-wrapper {
  margin-left: -15px;
  margin-right: -15px;
}
.hero--product .hero__image .image-wrapper img {
  -o-object-fit: cover;
  object-fit: cover;
}

.hero--product .image__gradient {
  position: absolute;
  display: none;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  z-index: 1;
  background: -webkit-linear-gradient(
    180deg,
    rgba(0, 0, 0, 0) 0%,
    rgba(0, 0, 0, 0.25) 100%
  );
  background: linear-gradient(
    270deg,
    rgba(0, 0, 0, 0) 0%,
    rgba(0, 0, 0, 0.25) 100%
  );
}
@media only screen and (min-width: 768px) {
  .hero--product .image__gradient {
    display: block;
  }
}

.hero--product .hero__share-design {
  bottom: 0;
  right: 0;
  width: 100%;
  background-color: #ffffff;
  z-index: 2;
  padding: 15px 0 0 0;
}
@media only screen and (min-width: 768px) {
  .hero--product .hero__share-design {
    position: absolute;
    padding: 30px 50px 0 50px;
  }
}
@media only screen and (min-width: 992px) {
  .hero--product .hero__share-design {
    width: 65%;
  }
}
</style>
