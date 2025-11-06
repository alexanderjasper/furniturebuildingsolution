<template>
  <!-- Section Newsletter Start -->
  <div class="newsletter">
    <div class="container">
      <div class="footer__inspiration">
        <div class="row">
          <div class="col-lg-8 offset-lg-2 col-xl-6 offset-xl-3 footer__newsletter">
            <h2>Tilmeld dig vores nyhedsbrev</h2>
            <form action="javascript:void(0);">
              <div class="form__group">
                <div class="field">
                  <input type="email" name="email" id="email" placeholder=" " v-model="email" />
                  <label for="email">E-mailadresse</label>
                </div>
                <div class="field field--min">
                  <input
                    type="submit"
                    value="Tilmeld"
                    class="btn"
                    v-on:click="showEmailSubscribeModal=true"
                  />
                </div>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
    <email-subscribe-modal
      :enteredEmail="email"
      v-if="showEmailSubscribeModal"
      @close="showEmailSubscribeModal=false"
      @resetEmail="email = ''"
    ></email-subscribe-modal>
  </div>
  <!-- Section Newsletter End -->
</template>

<script>
import emailSubscribeModal from "../../email-subscribe-modal";
export default {
  name: "newsletter",
  components: {
    "email-subscribe-modal": emailSubscribeModal
  },
  data() {
    return {
      showEmailSubscribeModal: false,
      email: ""
    };
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
/*--------------------------------------------------------------
# Newsletter
--------------------------------------------------------------*/
.footer__newsletter {
  padding: 50px 30px 65px 30px;
  text-align: center;
}
.footer__newsletter p {
  width: 80%;
  margin: 0 auto 30px auto;
}
</style>
