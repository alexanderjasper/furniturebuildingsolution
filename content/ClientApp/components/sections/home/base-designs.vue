<template>
  <!-- Section Base Designs Start -->
  <div class="base-designs">
    <div class="container">
      <div class="row">
        <div class="col-sm-6">
          <router-link class="base-designs__link" to="/reoldesigner/simpelt">
            <div class="design">
              <div class="design__image">
                <img src="/resources/images/base-basic.png" alt="Simpelt grunddesign" />
              </div>
              <div class="design__content">
                <h3>
                  Grunddesign
                  <span class="text--highlight">Simpelt</span>
                </h3>
                <p>Rammen er sat – gå på opdagelse og lav din reol eller dit skab præcist som du vil.</p>
                <div class="design__link">
                  <router-link class="text-link" to="/reoldesigner/simpelt">Design nu</router-link>
                </div>
              </div>
            </div>
          </router-link>
        </div>

        <div class="col-sm-6">
          <router-link class="base-designs__link" to="/reoldesigner/doeraabning">
            <div class="design">
              <div class="design__image">
                <img src="/resources/images/base-door.png" alt="Simpelt grunddesign" />
              </div>
              <div class="design__content">
                <h3>
                  Grunddesign
                  <span class="text--highlight">Døråbning</span>
                </h3>
                <p>Udnyt pladsen optimalt ved at bygge en reol omkring din døråbning.</p>
                <div class="design__link">
                  <router-link class="text-link" to="/reoldesigner/doeraabning">Design nu</router-link>
                </div>
              </div>
            </div>
          </router-link>
        </div>
      </div>
    </div>
  </div>
  <!-- Section Base Designs End -->
</template>

<script>
export default {
  name: "base-designs",
  components: {},
  data() {
    return {};
  },
  computed: {},
  methods: {}
};
</script>

<style>
/*--------------------------------------------------------------
# Base designs section
--------------------------------------------------------------*/

.base-designs {
  margin-bottom: -60px;
  margin-top: 100px;
}
.design {
  margin-top: 30px;
}

@media only screen and (min-width: 576px) {
  .base-designs {
    margin-bottom: 0;
  }
  .design {
    margin-top: 0;
  }
}
@media only screen and (min-width: 768px) {
  .base-designs {
    margin-top: 70px;
    padding-top: 40px;
  }
}

.base-designs__link:hover {
  text-decoration: none;
}

.design {
  background: #cbe6df;
  padding: 0 20px 20px 20px;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: vertical;
  -webkit-box-direction: normal;
  -ms-flex-direction: column;
  flex-direction: column;
  margin-bottom: 60px;
}
@media only screen and (min-width: 576px) {
  .design {
    margin-bottom: 0;
  }
}
@media only screen and (min-width: 1200px) {
  .design {
    -webkit-box-orient: horizontal;
    -webkit-box-direction: normal;
    -ms-flex-direction: row;
    flex-direction: row;
    padding: 0 20px 20px 20px;
  }
}
@media only screen and (min-width: 1300px) {
  .design {
    padding: 0 50px 50px 50px;
  }
}

.design__content {
  padding-top: 35px;
}
@media only screen and (min-width: 1200px) {
  .design__content {
    padding-left: 25px;
  }
}
@media only screen and (min-width: 1300px) {
  .design__content {
    padding-left: 35px;
  }
}
.design__content p {
  font-size: 15px;
  margin-bottom: 0;
}

.design__image {
  max-width: 75%;
  margin: auto;
  margin-top: -40px;
}
@media only screen and (min-width: 1200px) {
  .design__image {
    margin-top: -40px;
    max-width: 40%;
  }
}

.design__link {
  margin-top: 30px;
  text-align: right;
}
</style>
