<template>
  <div>
    <div class="standard-box align-center">
      <h2>Skift adgangskode</h2>
      <div class="form-group">
        <label for="newPassword">Ny adgangskode</label>
        <input
          type="password"
          v-model="newPassword"
          name="newPassword"
          class="form-control small-input"
        />
      </div>
      <div class="form-group">
        <label for="confirmPassword">Bekræft ny adgangskode</label>
        <input
          type="password"
          v-model="confirmPassword"
          name="confirmPassword"
          class="form-control small-input"
        />
      </div>
      <div class="form-group">
        <button v-on:click="submit()" class="btn" :disabled="!isValid">Skift adgangskode</button>
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
      newPassword: null,
      confirmPassword: null,
    };
  },
  props: {
    recoveryKey: String,
  },
  computed: {
    isValid: function () {
      return (
        !!this.newPassword &&
        !!this.confirmPassword &&
        this.newPassword === this.confirmPassword
      );
    },
  },
  methods: {
    ...mapActions("alert", ["success", "error", "clear"]),
    submit() {
      if (this.isValid) {
        userService.changePassword(this.newPassword, this.recoveryKey).then(
          (response) => {
            this.$router.push("/log-ind");
            setTimeout(() => {
              // display success message after route change completes
              this.success("Adgangskoden er ændret.");
            });
          },
          (error) => {
            this.error(
              "Der skete en fejl. Adgangskoden er ikke ændret. Forsøg igen, eller kontakt os."
            );
          }
        );
      }
    },
  },
};
</script>