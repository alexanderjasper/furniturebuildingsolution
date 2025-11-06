<template>
  <div id="app" class="container-fluid">
    <closedown-modal
      v-if="showInfoModal"
      @close="closeInfoModal"
    ></closedown-modal>

    <div class="row">
      <nav-menu v-if="showNavMenu" params="route: route"></nav-menu>
      <alert-popup
        :route="$route.name"
        :type="alert.type"
        :message="alert.message"
      ></alert-popup>
      <div id="mainView">
        <router-view @showHideNavMenu="showHideNavMenu"></router-view>
        <cookie-law theme="dark-lime">
          >
          <div slot-scope="props" style="text-align: center; width: 100%">
            <div>
              Klik for at acceptere vores brug af cookies og tracking på
              shelfer.dk.
              <router-link target="_blank" to="/cookiepolitik">
                <span>Læs mere her</span>
              </router-link>

              <button
                class="btn btn-secondary"
                @click="props.accept"
                style="margin-left: 20px"
                v-on:click="cookieAccepted()"
              >
                <span>Accepter</span>
              </button>
            </div>
          </div>
        </cookie-law>
      </div>
    </div>
    <span v-if="trackingActive">
      <noscript>
        <iframe
          src="https://www.googletagmanager.com/ns.html?id=GTM-PSG9638"
          height="0"
          width="0"
          style="display: none; visibility: hidden"
        ></iframe>
      </noscript>
    </span>
  </div>
</template>

<script>
import NavMenu from "./nav/nav-menu";
import AlertPopup from "./alert-popup";
import { mapState, mapActions } from "vuex";
import CookieLaw from "vue-cookie-law";
import { localstorageService } from "../_services";
import closedownModal from "./modals/closedown-modal";

export default {
  components: {
    "nav-menu": NavMenu,
    "alert-popup": AlertPopup,
    "cookie-law": CookieLaw,
    "closedown-modal": closedownModal,
  },
  data() {
    return {
      showNavMenu: true,
      trackingActive: false,
      showInfoModal: true,
      closeDownMessage: `r`,
      showInfoModal: true,
    };
  },
  computed: {
    ...mapState({
      alert: (state) => state.alert,
    }),
  },
  created() {
    if (localstorageService.cookieAccepted()) {
      this.initializeTracking();
    }
  },
  methods: {
    ...mapActions({
      clearAlert: "alert/clear",
    }),
    showHideNavMenu: function (shouldShow) {
      this.showNavMenu = shouldShow;
      var messengerWidget = document.getElementsByClassName(
        "fb_customer_chat_bubble_animated_no_badge"
      )[0];
      if (messengerWidget) {
        if (shouldShow) {
          messengerWidget.style.display = "block";
        } else {
          messengerWidget.style.display = "none";
        }
      }
    },
    closeInfoModal: function () {
      this.showInfoModal = false;
    },
    cookieAccepted: function () {
      if (
        document.location.hostname.search("shelfer.dk") === 0 &&
        document.location.hostname.search("test") === -1
      ) {
        this.initializeTracking();
      }
    },
    initializeTracking: function () {
      if (
        document.location.hostname.search("shelfer.dk") === 0 &&
        document.location.hostname.search("test") === -1
      ) {
        this.trackingActive = true;
        this.initializeGTM();
        this.initializeHotjar();
      }
    },
    initializeGTM: function () {
      (function (w, d, s, l, i) {
        w[l] = w[l] || [];
        w[l].push({ "gtm.start": new Date().getTime(), event: "gtm.js" });
        var f = d.getElementsByTagName(s)[0],
          j = d.createElement(s),
          dl = l != "dataLayer" ? "&l=" + l : "";
        j.async = true;
        j.src = "https://www.googletagmanager.com/gtm.js?id=" + i + dl;
        f.parentNode.insertBefore(j, f);
      })(window, document, "script", "dataLayer", "GTM-PSG9638");
    },
    initializeHotjar: function () {
      (function (h, o, t, j, a, r) {
        h.hj =
          h.hj ||
          function () {
            (h.hj.q = h.hj.q || []).push(arguments);
          };
        h._hjSettings = { hjid: 1813041, hjsv: 6 };
        a = o.getElementsByTagName("head")[0];
        r = o.createElement("script");
        r.async = 1;
        r.src = t + h._hjSettings.hjid + j + h._hjSettings.hjsv;
        a.appendChild(r);
      })(window, document, "https://static.hotjar.com/c/hotjar-", ".js?sv=");
    },
  },
  watch: {
    $route(to, from) {
      // clear alert on location change
      this.clearAlert();
    },
  },
};
</script>

<style>
.Cookie--mytheme {
  background: rgba(50, 50, 50);
}
</style>
