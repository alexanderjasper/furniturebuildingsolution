<template>
  <div>
    <div class="delivery-information-form">
      <div class="row delivery-information-row">
        <div class="col-md-6 col-xs-12">
          <div class="delivery-information-label required">Fornavn</div>
          <div>
            <input
              :disabled="!editable"
              class="delivery-information-input"
              type="text"
              v-model="formData.firstName"
              id="firstNameField"
              v-focus
            />
          </div>
        </div>
        <div class="col-md-6 col-xs-12">
          <div class="delivery-information-label required">Efternavn</div>
          <div>
            <input
              :disabled="!editable"
              class="delivery-information-input"
              type="text"
              v-model="formData.lastName"
            />
          </div>
        </div>
      </div>
      <div class="row delivery-information-row">
        <div class="col-md-6 col-xs-12">
          <div class="delivery-information-label">Firmanavn</div>
          <div>
            <input
              :disabled="!editable"
              class="delivery-information-input"
              type="text"
              v-model="formData.companyName"
            />
          </div>
        </div>
        <div class="col-md-6 col-xs-12">
          <div class="delivery-information-label">CVR-nummer</div>
          <div>
            <input
              :disabled="!editable"
              class="delivery-information-input"
              type="text"
              v-model="formData.vatNumber"
            />
          </div>
        </div>
      </div>
      <div v-if="!isLoggedIn" class="row delivery-information-row">
        <div class="col-md-6 col-xs-12">
          <div class="delivery-information-label required">E-mailadresse</div>
          <div>
            <input
              :disabled="!editable"
              class="delivery-information-input"
              type="text"
              v-model="formData.emailAddress"
            />
          </div>
        </div>
      </div>
      <div class="row delivery-information-row">
        <div class="col-12">
          <div class="delivery-information-label required">Adresse</div>
          <div class="autocomplete-container">
            <input
              :disabled="!editable"
              class="delivery-information-input"
              type="search"
              v-model="formData.address.addressString"
              id="dawa-autocomplete-address"
            />
          </div>
        </div>
      </div>
      <div class="row delivery-information-row">
        <div class="col-12">
          <div class="delivery-information-label">Forsendelsesadresse (hvis forskellig fra adresse)</div>
          <div class="autocomplete-container">
            <input
              :disabled="!editable"
              class="delivery-information-input"
              type="search"
              v-model="formData.shippingAddress.addressString"
              id="dawa-autocomplete-shipping-address"
            />
          </div>
        </div>
      </div>
      <div style="color: red" class="delivery-information-label required">Påkrævede felter</div>
    </div>
  </div>
</template>

<script>
var dawaAutocomplete2 = require("dawa-autocomplete2");
export default {
  name: "checkout-page",
  props: {
    editable: Boolean,
  },
  data() {
    return {
      formData: {
        firstName: "",
        lastName: "",
        companyName: "",
        vatNumber: "",
        emailAddress: "",
        address: {
          addressString: "",
          dawaId: null,
        },
        shippingAddress: {
          addressString: "",
          dawaId: null,
        },
      },
    };
  },
  methods: {
    onAddressSelected: function (selectedAddress) {
      this.formData.address.addressString = selectedAddress.tekst;
      this.formData.address.dawaId = selectedAddress.data.id;
    },
    onShippingAddressSelected: function (selectedAddress) {
      this.formData.shippingAddress.addressString = selectedAddress.tekst;
      this.formData.shippingAddress.dawaId = selectedAddress.data.id;
    },
  },
  mounted() {
    var addressElm = document.getElementById("dawa-autocomplete-address");
    dawaAutocomplete2.dawaAutocomplete(addressElm, {
      select: this.onAddressSelected,
    });
    var shippingAddressElm = document.getElementById(
      "dawa-autocomplete-shipping-address"
    );
    dawaAutocomplete2.dawaAutocomplete(shippingAddressElm, {
      select: this.onShippingAddressSelected,
    });
  },
  computed: {
    isValid: function () {
      return (
        !!this.formData.firstName &&
        !!this.formData.lastName &&
        !!this.formData.address.dawaId &&
        (this.isLoggedIn || this.formData.emailAddress)
      );
    },
    isLoggedIn: function () {
      return this.$store.state.account.status.loggedIn;
    },
  },
  watch: {
    isValid: function (value) {
      if (value) {
        this.$emit("setValid", true);
      } else {
        this.$emit("setValid", false);
      }
    },
    formData: {
      handler: function (data) {
        var returnObject = {
          firstName: data.firstName,
          lastName: data.lastName,
          companyName: data.companyName,
          vatNumber: data.vatNumber,
          emailAddress: data.emailAddress,
          address: data.address.dawaId
            ? {
                addressString: data.address.addressString,
                dawaId: data.address.dawaId,
              }
            : null,
          shippingAddress: data.shippingAddress.dawaId
            ? {
                addressString: data.shippingAddress.addressString,
                dawaId: data.shippingAddress.dawaId,
              }
            : null,
        };
        this.$emit("setData", returnObject);
      },
      deep: true,
    },
  },
};
</script>

<style>
.delivery-information-form {
  text-align: left;
  max-width: 630px;
  padding: 0px 15px;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 15px;
}
.delivery-information-input {
  width: 100%;
  padding: 5px 7px;
  border-radius: 0.3125em;
}
.delivery-information-label {
  margin-top: 15px;
}
.delivery-information-label.required::after {
  content: " *";
  color: red;
  font-size: 20px;
}

/* DAWA stuff: */
.autocomplete-container {
  /* relative position for at de absolut positionerede forslag får korrekt placering.*/
  position: relative;
  width: 100%;
}

.dawa-autocomplete-suggestions {
  margin: 0.3em 0 0 0;
  padding: 0;
  text-align: left;
  border-radius: 0.3125em;
  background: #fcfcfc;
  box-shadow: 0 0.0625em 0.15625em rgba(0, 0, 0, 0.15);
  position: absolute;
  left: 0;
  right: 0;
  z-index: 9999;
  overflow-y: auto;
  box-sizing: border-box;
}

.dawa-autocomplete-suggestions .dawa-autocomplete-suggestion {
  margin: 0;
  list-style: none;
  cursor: pointer;
  padding: 0.4em 0.6em;
  color: #333;
  border: 0.0625em solid #ddd;
  border-bottom-width: 0;
}

.dawa-autocomplete-suggestions .dawa-autocomplete-suggestion:first-child {
  border-top-left-radius: inherit;
  border-top-right-radius: inherit;
}

.dawa-autocomplete-suggestions .dawa-autocomplete-suggestion:last-child {
  border-bottom-left-radius: inherit;
  border-bottom-right-radius: inherit;
  border-bottom-width: 0.0625em;
}

.dawa-autocomplete-suggestions .dawa-autocomplete-suggestion.dawa-selected,
.dawa-autocomplete-suggestions .dawa-autocomplete-suggestion:hover {
  background: #f0f0f0;
}
</style>
