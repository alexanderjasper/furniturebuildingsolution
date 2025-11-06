<template>
  <div>
    <div class="standard-box">
      <div class="text-container align-center">
        <h2>Opret konto</h2>
        <form @submit.prevent="handleSubmit">
          <div class="form-group">
            <label for="firstName">Fornavn</label>
            <input
              type="text"
              v-model="user.firstName"
              name="firstName"
              class="form-control small-input"
              :class="{ 'is-invalid': submitted && errors.firstName }"
            />
            <div
              v-if="submitted && errors.firstName"
              class="invalid-feedback"
            >{{ errors.firstName }}</div>
          </div>
          <div class="form-group">
            <label for="lastName">Efternavn</label>
            <input
              type="text"
              v-model="user.lastName"
              name="lastName"
              class="form-control small-input"
              :class="{ 'is-invalid': submitted && errors.lastName }"
            />
            <div v-if="submitted && errors.lastName" class="invalid-feedback">{{ errors.lastName }}</div>
          </div>
          <div class="form-group">
            <label for="emailAddress">E-mailadresse</label>
            <input
              type="text"
              v-model="user.emailAddress"
              name="emailAddress"
              class="form-control small-input"
              :class="{ 'is-invalid': submitted && errors.emailAddress }"
            />
            <div
              v-if="submitted && errors.emailAddress"
              class="invalid-feedback"
            >{{ errors.emailAddress }}</div>
          </div>
          <div class="form-group">
            <label for="password">Adgangskode</label>
            <input
              type="password"
              v-model="user.password"
              name="password"
              class="form-control small-input"
              :class="{ 'is-invalid': submitted && errors.password }"
            />
            <div v-if="submitted && errors.password" class="invalid-feedback">{{ errors.password }}</div>
          </div>
          <div class="form-group">
            <label for="password">Bekræft adgangskode</label>
            <input
              type="password"
              v-model="user.passwordConfirm"
              name="passwordConfirm"
              class="form-control small-input"
              :class="{ 'is-invalid': submitted && errors.passwordConfirm }"
            />
            <div
              v-if="submitted && errors.passwordConfirm"
              class="invalid-feedback"
            >{{ errors.passwordConfirm }}</div>
          </div>
          <div class="form-group">
            <button class="btn" :disabled="status.registering">Opret konto</button>
            <img
              v-show="status.registering"
              src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA=="
            />
            <router-link to="log-ind" class="btn btn-secondary">Annuller</router-link>
          </div>
        </form>
      </div>
    </div>
    <newsletter></newsletter>
    <page-footer></page-footer>
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";
import pageFooter from "../page-footer";
import newsletter from "../sections/products/newsletter";

export default {
  components: {
    "page-footer": pageFooter,
    newsletter: newsletter,
  },
  data() {
    return {
      user: {
        firstName: "",
        lastName: "",
        emailAddress: "",
        password: "",
        passwordConfirm: "",
      },
      submitted: false,
      errors: {},
    };
  },
  computed: {
    ...mapState("account", ["status"]),
  },
  methods: {
    ...mapActions("account", ["register", "login"]),
    handleSubmit(e) {
      this.submitted = true;
      this.errors = {};
      if (!this.user.firstName) {
        this.errors.firstName = "Fornavn skal udfyldes.";
      }
      if (!this.user.lastName) {
        this.errors.lastName = "Efternavn skal udfyldes.";
      }
      if (!this.user.emailAddress) {
        this.errors.emailAddress = "E-mailadresse skal udfyldes.";
      }
      if (!this.user.password) {
        this.errors.password = "Adgangskode skal udfyldes.";
      } else if (this.user.password.length < 6) {
        this.errors.password = "Adgangskode skal indeholde mindst 6 tegn.";
      } else if (this.user.passwordConfirm !== this.user.password) {
        this.errors.passwordConfirm = "Adgangskoderne stemmer ikke overens.";
      }
      if (Object.keys(this.errors).length === 0) {
        this.register(this.user).then((success) => {});
      }
    },
  },
};
</script>