<template>
  <div>
    <div class="standard-box align-center">
      <h2>Glemt adgangskode</h2>
      <div class="form-group">
        <label for="emailAddress">E-mailadresse</label>
        <input
          type="text"
          v-model="emailAddress"
          name="emailAddress"
          class="form-control small-input"
        />
      </div>
      <div class="form-group">
        <button
          v-on:click="submit()"
          class="btn"
          :disabled="!isValid"
        >Send link til ændring af adgangskode</button>
      </div>
    </div>
    <newsletter></newsletter>
    <page-footer></page-footer>
  </div>
</template>

<script>
import { mapActions } from "vuex";
import { userService } from "../../_services";
import pageFooter from "../page-footer";
import newsletter from "../sections/products/newsletter";

export default {
  components: {
    "page-footer": pageFooter,
    newsletter: newsletter,
  },
  data() {
    return {
      emailAddress: null,
    };
  },
  computed: {
    isValid: function () {
      return !!this.emailAddress;
    },
  },
  methods: {
    ...mapActions("alert", ["success", "error"]),
    submit() {
      if (this.isValid) {
        userService.forgotPassword(this.emailAddress).then(
          (response) => {
            this.$router.push("/log-ind");
            setTimeout(() => {
              // display success message after route change completes
              this.success(
                "Hvis der eksisterer en konto med den angivne e-mailadresse, modtager du om et øjeblik en e-mail med et link til ændring af adgangskoden."
              );
            });
          },
          (error) => {
            this.error("Der skete en fejl. Forsøg igen, eller kontakt os.");
          }
        );
      }
    },
  },
};
</script>