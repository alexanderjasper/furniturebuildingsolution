<template>
  <!-- Section Footer Start -->
  <div class="footer">
    <div class="container">
      <div class="footer__menu">
        <div class="row">
          <div class="col-md-3">
            <span>Kontakt os</span>
            <ul>
              <li>
                <a href="tel:+4542803076">+45 4280 3076</a>
              </li>
              <li>
                <a href="mailto:info@shelfer.dk">info@shelfer.dk</a>
              </li>
              <li>
                Bådehavnsgade 42P, port 9
                <br />2450 København SV
              </li>
            </ul>
          </div>
          <div class="col-md-3">
            <span>Information</span>
            <ul>
              <li>
                <router-link to="/produkter">Oplev produktet</router-link>
              </li>
              <li>
                <router-link to="/standardmodeller">Standardmodeller</router-link>
              </li>
              <li>
                <router-link to="/kontakt-os">Kontakt os</router-link>
              </li>
              <li>
                <router-link to="/ofte-stillede-sporgsmaal">Ofte stillede spørgsmål</router-link>
              </li>
            </ul>
          </div>
          <div class="col-md-3">
            <span>Betingelser</span>
            <ul>
              <li>
                <router-link
                  to="/handelsbetingelser-og-persondatapolitik"
                >Handelsbetingelser og persondatapolitik</router-link>
              </li>
            </ul>
          </div>
          <div class="col-md-3">
            <div class="footer__logo">
              <router-link to="/">
                <img src="/resources/images/logo--white.png" alt="Shelfer Logo" />
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="footer__bottom">
      <div class="container">
        <div class="row">
          <div class="col-md-6">© Copyright {{currentYear}} Shelfer</div>
          <div class="col-md-6 footer__so-me">
            <a href="https://www.facebook.com/shelfer.dk" target="_blank" class="text-link">Facebook</a> |
            <a
              href="https://www.instagram.com/shelfer.dk/"
              target="_blank"
              class="text-link"
            >Instagram</a>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- Section Footer End -->
</template>

<script>
export default {
  name: "page-footer",
  components: {},
  data() {
    return {};
  },
  computed: {
    currentYear: function () {
      var today = new Date();
      return today.getFullYear();
    },
  },
  methods: {},
};
</script>

<style>
/*--------------------------------------------------------------
# Footer
--------------------------------------------------------------*/
.footer__inspiration {
  background-color: #cbe6df;
}
.footer__inspiration .footer__wrapper {
  padding: 45px 50px;
}
.footer__inspiration .footer__title {
  font-size: 30px;
  line-height: 39px;
  margin-bottom: 25px;
}
.footer__inspiration .footer__title b {
  font-weight: 700;
}
.footer__inspiration .footer__image {
  position: relative;
  display: none;
  height: -webkit-calc(100% + 70px);
  height: calc(100% + 70px);
  margin-top: -70px;
  max-width: 100%;
}
@media only screen and (min-width: 768px) {
  .footer__inspiration .footer__image {
    display: block;
  }
}
.footer__inspiration .footer__image img {
  position: absolute;
  top: 12px;
  width: 85%;
  height: 300px;
  object-fit: cover;
  object-position: top;
}
.footer__menu {
  position: relative;
  padding: 50px;
  color: #ffffff;
  background-color: #2dac86;
}
.footer__logo {
  width: 80%;
  margin: 0 auto;
}
@media only screen and (max-width: 768px) {
  .footer__menu {
    text-align: center;
  }
}
.footer__menu .footer__wide {
  -webkit-box-flex: 2;
  -ms-flex: 2;
  flex: 2;
}
.footer__menu .footer__wide img {
  margin-top: 30px;
  margin-bottom: 30px;
}
@media only screen and (min-width: 768px) {
  .footer__menu .footer__wide img {
    margin-top: 0;
  }
}
.footer__menu span {
  display: block;
  font-weight: 700;
  margin-top: 20px;
  margin-bottom: 15px;
}
.footer__menu ul {
  padding: 0;
  list-style-type: none;
}
.footer__menu ul li {
  margin-bottom: 10px;
}
.footer__menu ul li a {
  color: #ffffff;
}
.footer__bottom {
  text-align: center;
  margin-top: -110px;
  padding-top: 140px;
  padding-bottom: 30px;
  background-color: #f5f4f5;
}
@media only screen and (min-width: 768px) {
  .footer__bottom {
    text-align: left;
  }
}
@media only screen and (min-width: 768px) {
  .footer__bottom .footer__so-me {
    text-align: right;
  }
}
.footer__bottom .footer__so-me .text-link {
  border-bottom: none;
  font-weight: 600;
  margin: 0 10px;
}
.footer__bottom .footer__so-me .text-link:last-child {
  margin-right: 0;
}
</style>

