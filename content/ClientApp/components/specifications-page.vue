<template>
  <div class="centered-box">
    <h1>Hent specifikationer</h1>
    <full-page-spinner v-if="loading"></full-page-spinner>
    <div class="container">
      <div class="row">
        <div class="col-12">
          <div>
            Id:
            <input type="text" v-model="enteredId" style="width: 60px" />
            Pladetk.:
            <input type="text" v-model="plateThickness" style="width: 60px" />
            Lågetk.:
            <input type="text" v-model="doorPlateThickness" style="width: 60px" />
          </div>
          <div>
            <span style="display: inline-block">
              <label class="b-contain">
                <span>Inkluder enkeltsidet</span>
                <input type="checkbox" v-model="includeSingleSided" />
                <span class="b-input"></span>
              </label>
            </span>
            <span style="display: inline-block">
              <label class="b-contain">
                <span>Inkluder dobbeltsidet</span>
                <input type="checkbox" v-model="includeDoubleSided" />
                <span class="b-input"></span>
              </label>
            </span>
          </div>

          <button type="button" class="btn" v-on:click="getData()">Hent spec.</button>
          <button type="button" class="btn" v-on:click="getBlueprint()">Hent reol</button>
          <button type="button" class="btn" v-on:click="getDoorsBlueprint()">Hent låger</button>
        </div>

        <div v-if="errorMessage" style="margin-top: 20px; color: red">{{errorMessage}}</div>
        <div class="col-12">
          <div class="bookcase-overview">
            <open-bookcase-button
              v-if="bookcase"
              width="100%"
              height="100%"
              :size="200"
              :bookcaseData="bookcase"
              :margin="0.25"
              :dataToShow="dataToShow"
              fillColor="transparent"
              :plateToHighlight="selectedPlateId"
              :compartmentToHighlight="selectedCompartmentId"
              :isSpecifications="true"
              @highlightPlate="selectPlate"
              @highlightCompartment="selectCompartment"
            ></open-bookcase-button>
          </div>
        </div>
      </div>
      <div class="row" v-if="bookcase">
        <div class="col-12">
          <label class="b-contain" style="display: inline">
            <span>Ingen</span>
            <input type="radio" v-model="dataToShow" value />
            <span class="b-input"></span>
          </label>
          <label class="b-contain" style="display: inline">
            <span>Plader</span>
            <input type="radio" v-model="dataToShow" value="plates" />
            <span class="b-input"></span>
          </label>
          <label class="b-contain" style="display: inline">
            <span>Rum</span>
            <input type="radio" v-model="dataToShow" value="compartments" />
            <span class="b-input"></span>
          </label>
        </div>
      </div>
      <div class="row">
        <div class="col-12" v-if="bookcase">
          <div>Pris: {{bookcase.salesPrice}}</div>

          <div class="table-responsive" v-if="dataToShow === 'plates'">
            <div>
              <button type="button" class="btn" v-on:click="setEditPlate(null)">Tilføj plade</button>
            </div>
            <table class="bookcase-specifications">
              <tbody>
                <tr>
                  <th>Id</th>
                  <th>Længde</th>
                  <th>Start</th>
                  <th>Slut</th>
                  <th>Beslag</th>
                  <th>Flytbar</th>
                  <th></th>
                </tr>
                <tr
                  v-for="(plate, index) in bookcase.plates"
                  v-on:click="selectPlate(plate.id)"
                  :key="index"
                  :class="plate.id == selectedPlateId ? 'highlight-background' : ''"
                >
                  <td>{{plate.id}}</td>
                  <td>{{plate.length}}</td>
                  <td>{{plate.start.x}}, {{plate.start.y}}</td>
                  <td>{{plate.end.x}}, {{plate.end.y}}</td>
                  <td>
                    <input type="checkbox" v-model="plate.innerPlateStart" disabled />
                    <input type="checkbox" v-model="plate.innerPlateEnd" disabled />
                  </td>
                  <td>
                    <input type="checkbox" v-model="plate.draggable" disabled />
                  </td>
                  <td v-on:click="setEditPlate(plate)">
                    <icon fixed-width icon="pen" class="fa-lg" />
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div class="table-responsive" v-if="bookcase && dataToShow === 'compartments'">
            <div>
              <button type="button" class="btn" v-on:click="setEditCompartment(null)">Tilføj rum</button>
            </div>
            <table class="bookcase-specifications">
              <tbody>
                <tr>
                  <th>Id</th>
                  <th>Bredde</th>
                  <th>Højde</th>
                  <th>Låge</th>
                  <th>Bagplade</th>
                  <th></th>
                </tr>
                <tr
                  v-for="(compartment, index) in bookcase.compartments"
                  v-on:click="selectCompartment(compartment.id)"
                  :key="index"
                  :class="compartment.id == selectedCompartmentId ? 'highlight-background' : ''"
                >
                  <td>{{compartment.id}}</td>
                  <td>{{compartment.dimensions.width}}</td>
                  <td>{{compartment.dimensions.height}}</td>
                  <td>
                    <input type="checkbox" v-model="compartment.hasDoor" disabled />
                  </td>
                  <td>{{compartment.backPlatePosition}}</td>

                  <td v-on:click="setEditCompartment(compartment)">
                    <icon fixed-width icon="pen" class="fa-lg" />
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
    <specifications-edit-plate-modal
      v-if="editPlate"
      :inputPlate="plateToEdit"
      :bookcase="bookcase"
      @close="exitEditModal"
    ></specifications-edit-plate-modal>
    <specifications-edit-compartment-modal
      v-if="editCompartment"
      :inputCompartment="compartmentToEdit"
      :bookcase="bookcase"
      @close="exitEditModal"
    ></specifications-edit-compartment-modal>
    <a id="downloadAnchorElem" style="display:none"></a>
  </div>
</template>

<script>
import { bookcaseService } from "../_services";
import BookcaseClass from "../extensions/bookcaseClasses";
import fullPageSpinner from "./full-page-spinner";
import specificationsEditPlateModal from "./modals/specifications-edit-plate-modal";
import specificationsEditCompartmentModal from "./modals/specifications-edit-compartment-modal";
import openBookcaseButton from "./bookcase-designer/graphic-buttons/open-bookcase-button";
export default {
  components: {
    "full-page-spinner": fullPageSpinner,
    "open-bookcase-button": openBookcaseButton,
    "specifications-edit-plate-modal": specificationsEditPlateModal,
    "specifications-edit-compartment-modal": specificationsEditCompartmentModal
  },
  data() {
    return {
      enteredId: null,
      plateThickness: null,
      doorPlateThickness: null,
      includeSingleSided: true,
      includeDoubleSided: true,
      bookcase: null,
      loading: false,
      errorMessage: null,
      bookcase: null,
      selectedPlateId: null,
      selectedCompartmentId: null,
      dataToShow: "",
      plateToEdit: null,
      compartmentToEdit: null,
      editPlate: false,
      editCompartment: false
    };
  },
  computed: {
    selectedPlate: function() {
      return this.bookcase.plateById(this.selectedPlateId);
    },
    overviewSize: function() {
      var windowWidth = window.innerWidth;
      if (windowWidth < 1100) {
        return 300;
      } else {
        return 300 + (windowWidth - 1100);
      }
    }
  },
  methods: {
    exitEditModal() {
      this.plateToEdit = null;
      this.editPlate = false;
      this.compartmentToEdit = null;
      this.editCompartment = false;
      this.getData();
    },
    setEditPlate(plate) {
      this.editPlate = true;
      this.plateToEdit = plate;
    },
    setEditCompartment(compartment) {
      this.editCompartment = true;
      this.compartmentToEdit = compartment;
    },
    selectPlate(plateId) {
      this.selectedPlateId = plateId;
    },
    selectCompartment(compartmentId) {
      this.selectedCompartmentId = compartmentId;
    },
    getData() {
      this.loading = true;
      this.bookcase = null;
      this.errorMessage = null;
      bookcaseService.get(this.enteredId).then(
        bookcaseData => {
          this.loading = false;
          this.bookcase = new BookcaseClass(null);
          this.bookcase.initialize(bookcaseData);
        },
        error => {
          this.loading = false;
          this.errorMessage = "Error: " + error;
          this.bookcase = null;
        }
      );
    },
    getBlueprint() {
      bookcaseService
        .getBlueprint(
          this.enteredId,
          this.plateThickness,
          this.doorPlateThickness,
          this.includeSingleSided,
          this.includeDoubleSided
        )
        .then(
          blueprint => {
            var dataStr =
              "data:text/json;charset=utf-8," +
              encodeURIComponent(JSON.stringify(blueprint));
            var dlAnchorElem = document.getElementById("downloadAnchorElem");
            dlAnchorElem.setAttribute("href", dataStr);
            dlAnchorElem.setAttribute(
              "download",
              "shelferBookase" + this.enteredId + ".json"
            );
            dlAnchorElem.click();
          },
          error => {
            this.loading = false;
            this.errorMessage = "Error: " + error;
            this.bookcase = null;
          }
        );
    },
    getDoorsBlueprint() {
      bookcaseService
        .getDoorsBlueprint(
          this.enteredId,
          this.plateThickness,
          this.doorPlateThickness
        )
        .then(
          blueprint => {
            var dataStr =
              "data:text/json;charset=utf-8," +
              encodeURIComponent(JSON.stringify(blueprint));
            var dlAnchorElem = document.getElementById("downloadAnchorElem");
            dlAnchorElem.setAttribute("href", dataStr);
            dlAnchorElem.setAttribute(
              "download",
              "shelferDoors" + this.enteredId + ".json"
            );
            dlAnchorElem.click();
          },
          error => {
            this.loading = false;
            this.errorMessage = "Error: " + error;
            this.bookcase = null;
          }
        );
    }
  }
};
</script>

<style>
.highlight-background {
  background-color: #2dac86;
}
.bookcase-overview {
  width: (100%);
  margin: 0 auto;
}
.bookcase-specifications {
  margin: 0 auto 40px auto;
}
th,
td {
  border: 1px solid lightgrey;
  padding: 6px;
}
</style>