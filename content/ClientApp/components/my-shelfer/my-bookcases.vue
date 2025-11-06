<template>
  <div>
    <full-page-spinner v-if="loading"></full-page-spinner>
    <div v-else>
      <div class="standard-box">
        <div v-if="!loading">
          <h1 class="page-header">Mine reoler</h1>
          <div class="table-responsive medium-table-container">
            <table class="table">
              <tbody>
                <tr>
                  <th>
                    <button v-on:click="addNewBookcase()" type="button" class="btn">
                      <icon fixed-width icon="plus" class="fa-lg" />
                      <span class="show-on-big-screens">Opret ny</span>
                    </button>
                  </th>
                  <th>Reolens navn</th>
                  <th class="date-column hide-on-mobile">Oprettet</th>
                  <th class="date-column hide-on-mobile">Senest ændret</th>
                  <th></th>
                  <th></th>
                </tr>
                <tr v-for="(bookcase) in bookcaseList" :key="bookcase.id">
                  <td :title="'Id: ' + bookcase.id">
                    <a v-on:click="openBookcase(bookcase.id, false)">
                      <open-bookcase-button
                        :size="symbolWidth"
                        :bookcaseData="bookcase"
                        :fillColor="bookcase.lockedForEditing ? '#c0c0c0' : '#f2f2f2'"
                        :width="symbolWidth +'px'"
                        :height="symbolWidth+'px'"
                        :margin="0.25"
                      ></open-bookcase-button>
                    </a>
                  </td>
                  <td>{{bookcase.name}}</td>
                  <td class="hide-on-mobile">{{displayedDate(bookcase.created)}}</td>
                  <td class="hide-on-mobile">{{displayedDate(bookcase.modified)}}</td>
                  <td>
                    <button
                      v-on:click="addToCart(bookcase.id, null)"
                      type="button"
                      class="btn btn-brand"
                    >
                      <icon fixed-width icon="shopping-cart" class="fa-lg" />
                      <icon fixed-width icon="plus" />
                    </button>
                  </td>
                  <td>
                    <icon
                      v-if="!bookcase.lockedForEditing"
                      v-on:click="showDeleteBookcaseModal(bookcase.id)"
                      fixed-width
                      icon="trash-alt"
                      class="mr-2 fa-lg"
                    />
                  </td>
                </tr>
                <tr v-if="!bookcaseList.length > 0">
                  <td colspan="7">
                    Du har endnu ikke designet en reol. Gå til
                    <router-link to="/reoldesigner">Reoldesigneren</router-link>
                    <span>for at designe og bestille en reol.</span>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>

        <confirm-delete-bookcase-modal
          v-if="bookcaseToDelete !== null"
          @confirmDelete="deleteBookcase"
          @close="hideDeleteBookcaseModal"
        ></confirm-delete-bookcase-modal>
      </div>
      <newsletter></newsletter>
      <page-footer></page-footer>
    </div>
  </div>
</template>

<script>
import { bookcaseService, localstorageService } from "../../_services";
import openBookcaseButton from "../bookcase-designer/graphic-buttons/open-bookcase-button";
import bigAddButton from "../bookcase-designer/graphic-buttons/big-add-button";
import confirmDeleteBookcaseModal from "../bookcase-designer/modals/confirm-delete-bookcase-modal";
import fullPageSpinner from "../full-page-spinner";
import pageFooter from "../page-footer";
import newsletter from "../sections/products/newsletter";
import { mapActions } from "vuex";
export default {
  components: {
    "open-bookcase-button": openBookcaseButton,
    "big-add-button": bigAddButton,
    "confirm-delete-bookcase-modal": confirmDeleteBookcaseModal,
    "full-page-spinner": fullPageSpinner,
    "page-footer": pageFooter,
    newsletter: newsletter,
  },
  data() {
    return {
      bookcaseToDelete: null,
      bookcaseList: [],
      loading: true,
    };
  },
  created() {
    this.getAllBookcases();
  },
  computed: {
    symbolWidth: function () {
      if (window.outerWidth && window.outerWidth < 600) {
        return 50;
      } else {
        return 100;
      }
    },
  },
  methods: {
    ...mapActions("alert", ["success"]),
    getAllBookcases: function () {
      this.loading = true;
      bookcaseService.getAll().then((bookcaseList) => {
        this.bookcaseList = bookcaseList;
        this.loading = false;
      });
    },
    displayedDate: function (dateAsString) {
      return dateAsString.substring(0, 10);
    },
    openBookcase: function (bookcaseId, openCopy) {
      this.$emit("loading");
      bookcaseService.get(bookcaseId).then((bookcase) => {
        if (openCopy) {
          bookcase.id = null;
          bookcase.lockedForEditing = false;
        }
        localstorageService.storeBookcase(bookcase);
        localstorageService.setOpenDirectly(true);
        this.$router.push("/reoldesigner");
      });
    },
    showDeleteBookcaseModal: function (bookcaseId) {
      this.bookcaseToDelete = bookcaseId;
    },
    hideDeleteBookcaseModal: function () {
      this.bookcaseToDelete = null;
    },
    deleteBookcase: function (bookcaseId) {
      bookcaseService.deleteBookcase(this.bookcaseToDelete).then((response) => {
        this.removeFromList(this.bookcaseToDelete, this.bookcaseList);
        this.hideDeleteBookcaseModal();
        this.success("Reolen er slettet.");
      });
    },
    removeFromList: function (bookcaseToDeleteId, bookcaseList) {
      this.bookcaseList = this.bookcaseList.filter(
        (bookcase) => bookcase.id != bookcaseToDeleteId
      );
    },
    addNewBookcase: function () {
      this.loading = true;
      localstorageService.setOpenDirectly(false);
      this.$router.push("/reoldesigner");
    },
    addToCart: function (bookcaseId) {
      var shoppingCart = localstorageService.addToCart(bookcaseId, null);
      var numberOfCartItems = localstorageService.getNumberOfCartItems();
      this.$store.commit("setNumberOfCartItems", numberOfCartItems);
      this.success("Føjet til indkøbskurv");
    },
  },
};
</script>

<style>
.table > tbody > tr > td,
.table > tbody > tr > th {
  vertical-align: middle;
  border-left: none;
  border-right: none;
}
.date-column {
  min-width: 115px;
}
@media only screen and (max-width: 700px) {
  .hide-on-mobile {
    display: none;
  }
}
</style>
