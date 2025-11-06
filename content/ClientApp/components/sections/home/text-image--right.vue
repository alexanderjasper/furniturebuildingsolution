<template>
  <!-- Section Text Image Start -->
  <div class="text-image">
    <div class="container">
      <div class="row">
        <div class="col-md-6 offset-md-1">
          <div class="text-image__image">
            <img src="/resources/images/design.jpg" alt="Event" />
          </div>
        </div>
        <div class="col-md-5">
          <div class="text-image__text">
            <span class="hashtag hashtag--yellow">#designselv</span>
            <h2 class="text-image__title">Designet af... dig!</h2>
            <p class="text-image__paragraph">
              Med vores designværktøj kan du nemt konfigurere din reol eller dit skab med de helt rigtige mål og din helt egen stil. Der er fuld frihed!
              <br />
              <br />Du kan selv måle op, og du er velkommen til at kontakte os, hvis du er i tvivl om noget. Vi tilbyder også opmålingsservice (200 kr. + 20 kr./km fra vores adresse i København SV). Så kommer vi ud og måler op, og vi garanterer, at målene er lige i skabet!
            </p>
            <div>
              <router-link class="btn" to="/produkter">Se vores tidligere projekter</router-link>
              <router-link class="btn" to="/produkter">Design selv nu</router-link>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- Section Text Image End -->
</template>

<script>
export default {
  name: "text-image--right",
  components: {},
  data() {
    return {};
  },
  computed: {},
  methods: {}
};
</script>
