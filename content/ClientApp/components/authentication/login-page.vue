<template>
  <div>
    <div class="standard-box">
      <div class="text-container align-center">
        <h2>Log ind</h2>
        <form @submit.prevent="handleSubmit">
          <div class="form-group">
            <label for="emailAddress">E-mailadresse</label>
            <input
              type="text"
              v-model="emailAddress"
              name="emailAddress"
              class="form-control small-input"
              :class="{ 'is-invalid': submitted && !emailAddress }"
            />
            <div v-show="submitted && !emailAddress" class="invalid-feedback">Brugernavn er påkrævet</div>
          </div>
          <div class="form-group">
            <label for="password">Adgangskode</label>
            <input
              type="password"
              v-model="password"
              name="password"
              class="form-control small-input"
              :class="{ 'is-invalid': submitted && !password }"
            />
            <div v-show="submitted && !password" class="invalid-feedback">Adgangskode er påkrævet</div>
          </div>
          <div class="form-group">
            <button class="btn" :disabled="status.loggingIn">Log ind</button>
            <img
              v-show="status.loggingIn"
              src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA=="
            />
            <!-- <router-link to="opret-konto" class="btn btn-link">Register</router-link> -->
          </div>
          <div>
            <router-link class="text-link" to="opret-konto" exact-active-class="active">Opret konto</router-link>
          </div>
          <div>
            <router-link
              class="text-link"
              to="glemt-adgangskode"
              exact-active-class="active"
            >Glemt adgangskode</router-link>
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
      emailAddress: "",
      password: "",
      submitted: false,
    };
  },
  computed: {
    ...mapState("account", ["status"]),
  },
  created() {
    if (this.$store.state.account.status.loggedIn) {
      // reset login status
      this.logout();
      //TODO: Find some smarter way to reset this
      this.$store.commit("setNumberOfCartItems", 0);
    }
  },
  methods: {
    ...mapActions("account", ["login", "logout"]),
    handleSubmit(e) {
      this.submitted = true;
      const { emailAddress, password } = this;
      if (emailAddress && password) {
        this.login({ emailAddress, password });
      }
    },
  },
};
</script>