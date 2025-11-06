<template>
  <div class="standard-box">
    <h1>Hent data</h1>
    <div v-if="errorMessage" style="margin-top: 20px; color: red">{{errorMessage}}</div>
    <full-page-spinner v-if="loading"></full-page-spinner>
    <div v-else class="container">
      <div class="row">
        <div class="col-12 col-md-6">
          <div class="table-responsive">
            <h2>Brugere</h2>
            <table>
              <tbody>
                <tr>
                  <th>Id</th>
                  <th>Fornavn</th>
                  <th>Efternavn</th>
                  <th>Mail</th>
                </tr>
                <tr v-for="(user, index) in users" :key="index">
                  <td>{{user.id}}</td>
                  <td>{{user.firstName}}</td>
                  <td>{{user.lastName}}</td>
                  <td>{{user.emailAddress}}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
        <div class="col-12 col-md-6">
          <div class="table-responsive">
            <h2>Reoler</h2>
            <table>
              <tbody>
                <tr>
                  <th>Id</th>
                  <th>Bruger-id</th>
                  <th>Fornavn</th>
                  <th>Efternavn</th>
                </tr>
                <tr v-for="(bookcase, index) in bookcases" :key="index">
                  <td>{{bookcase.id}}</td>
                  <td>{{bookcase.userId}}</td>
                  <td>{{bookcase.userFirstName}}</td>
                  <td>{{bookcase.userLastName}}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { userService, bookcaseService } from "../_services";
import fullPageSpinner from "./full-page-spinner";
export default {
  components: {
    "full-page-spinner": fullPageSpinner,
  },
  data() {
    return {
      errorMessage: null,
      loading: true,
      users: [],
      bookcases: [],
    };
  },
  mounted() {
    this.getUsers();
    this.getBookcases();
  },
  methods: {
    getUsers() {
      userService.getAll().then(
        (usersData) => {
          this.loading = false;
          this.users = usersData;
        },
        (error) => {
          this.loading = false;
          this.errorMessage = "Error: " + error;
        }
      );
    },
    getBookcases() {
      bookcaseService.getData().then(
        (bookcaseData) => {
          this.loading = false;
          this.bookcases = bookcaseData;
        },
        (error) => {
          this.loading = false;
          this.errorMessage = "Error: " + error;
        }
      );
    },
  },
};
</script>

<style>
th,
td {
  border: 1px solid lightgrey;
  padding: 6px;
}
</style>