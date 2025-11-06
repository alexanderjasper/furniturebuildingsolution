<template>
  <div class="main-nav" id="topNavigationBar">
    <!-- Section Header Start -->
    <header>
      <!-- Top navigation - Useful links -->
      <!-- <div class="header__top">
        <div class="container">
          <nav class="header__useful-links">
            <ul>
              <li>
                <a href="#">Hvordan virker det?</a>
              </li>
              <li>
                <router-link to="/moed-shelfer">Mød Shelfer</router-link>
              </li>
            </ul>
          </nav>
        </div>
      </div>-->

      <!-- Main header brand bar -->
      <div class="container">
        <div class="header__content">
          <router-link to="/">
            <img class="logo" src="/resources/images/logo--dark.png" alt="Shelfer" />
          </router-link>

          <div class="navigation__backdrop" v-on:click="toggleBurger()"></div>
          <nav class="header__navigation header__navigation--mobile">
            <ul v-on:click="toggleBurger()">
              <li>
                <router-link to="/reoldesigner/vaelg">Design din reol</router-link>
              </li>
              <li>
                <router-link to="/indkoebskurv">
                  <icon fixed-width icon="shopping-cart" />Indkøbskurv
                  <span class="cart-number-mobile">{{numberOfCartItems}}</span>
                </router-link>
              </li>
              <li v-for="route in mainNavRoutes" :key="route.name">
                <router-link :to="route.path">{{route.display}}</router-link>
              </li>

              <li v-for="route in userMenuRoutes" :key="route.name">
                <router-link :to="route.path">{{route.display}}</router-link>
              </li>

              <li v-if="isLoggedIn">
                <router-link to="log-ind">Log ud</router-link>
              </li>
            </ul>
          </nav>
          <nav class="header__navigation header__navigation--desktop">
            <ul>
              <li class="nav-item">
                <router-link to="/reoldesigner/vaelg">Design din reol</router-link>
              </li>
              <li class="nav-item" v-for="route in mainNavRoutes" :key="route.name">
                <router-link :to="route.path">{{route.display}}</router-link>
              </li>
            </ul>
          </nav>
          <nav class="header__user" v-click-outside="closeUserMenu">
            <ul v-if="isLoggedIn">
              <li>
                <div class="user-menu" v-on:click="toggleUserMenu()">
                  <div class="user-menu__user">
                    <icon fixed-width icon="user" />
                    {{userName}}
                    <span class="drop-icon"></span>
                  </div>
                  <transition name="fold">
                    <ul v-if="showUserMenu" class="menu-item__user">
                      <li v-for="route in userMenuRoutes" :key="route.name">
                        <router-link :to="route.path">{{route.display}}</router-link>
                      </li>
                      <li v-if="isLoggedIn">
                        <router-link to="log-ind">Log ud</router-link>
                      </li>
                    </ul>
                  </transition>
                </div>
              </li>
              <li class="menu-item__cart">
                <router-link to="/indkoebskurv">
                  <icon fixed-width icon="shopping-cart" />
                  <span class="cart-number-wrapper">
                    <div :class="cartJump ? 'cart-number jump' : 'cart-number'">
                      <div>{{numberOfCartItems}}</div>
                    </div>
                  </span>
                </router-link>
              </li>
            </ul>

            <ul v-else>
              <li v-for="route in userMenuRoutes" :key="route.name">
                <router-link :to="route.path">{{route.display}}</router-link>
              </li>
              <li class="menu-item__cart">
                <router-link to="/indkoebskurv">
                  <icon fixed-width icon="shopping-cart" />
                  <span class="cart-number-wrapper">
                    <div :class="cartJump ? 'cart-number jump' : 'cart-number'">
                      <div>{{numberOfCartItems}}</div>
                    </div>
                  </span>
                </router-link>
              </li>
            </ul>
          </nav>
          <div class="header__burger" v-on:click="toggleBurger()">
            <span></span>
            <span></span>
            <span></span>
            <span></span>
          </div>
        </div>
      </div>
    </header>
    <!-- Section Header End -->
  </div>
</template>

<script>
import { routes } from "../../router/routes";
import { localstorageService } from "../../_services";

export default {
  name: "nav-menu",
  data() {
    return {
      routes,
      collapsed: true,
      cartJump: false,
      showUserMenu: false,
    };
  },
  computed: {
    numberOfCartItems: function () {
      var cartItems = this.$store.state.numberOfCartItems;
      if (cartItems) {
        return cartItems;
      } else {
        return 0;
      }
    },
    isLoggedIn: function () {
      return this.$store.state.account.status.loggedIn;
    },
    isAdmin: function () {
      var user = this.$store.state.account.user;
      if (user && user.role === 1) {
        return true;
      }
      return false;
    },
    mainNavRoutes: function () {
      var filteredRoutes;
      if (this.isLoggedIn) {
        filteredRoutes = routes.filter(
          (route) => route.type === "main-nav" && route.showWhenLoggedIn
        );
      } else {
        filteredRoutes = routes.filter(
          (route) => route.type === "main-nav" && route.showWhenLoggedOut
        );
      }

      if (!this.isAdmin) {
        filteredRoutes = filteredRoutes.filter((route) => !route.adminOnly);
      }

      return filteredRoutes;
    },
    userMenuRoutes: function () {
      var filteredRoutes;
      if (this.isLoggedIn) {
        filteredRoutes = routes.filter(
          (route) => route.type === "user-menu" && route.showWhenLoggedIn
        );
      } else {
        filteredRoutes = routes.filter(
          (route) => route.type === "user-menu" && route.showWhenLoggedOut
        );
      }

      if (!this.isAdmin) {
        filteredRoutes = filteredRoutes.filter((route) => !route.adminOnly);
      }

      return filteredRoutes;
    },
    userName: function () {
      var firstName = this.$store.state.account.user.firstName;
      var lastName = this.$store.state.account.user.lastName;
      if (!firstName) firstName = "";
      if (!lastName) lastName = "";

      var firstNameTruncated = firstName.substring(0, 1);
      if (firstName.length > 1) {
        firstNameTruncated += ".";
      }
      var lastNameTruncated = lastName.substring(0, 8);
      if (lastName.length > 8) {
        lastNameTruncated += ".";
      }
      return firstNameTruncated + " " + lastNameTruncated;
    },
    userMenuItem: function () {
      return document.querySelector(".menu-item__user");
    },
  },
  methods: {
    setNumberOfCartItems: function () {
      var fromState = this.$store.state.numberOfCartItems;
      if (fromState) {
        return;
      } else {
        var numberOfCartItems = localstorageService.getNumberOfCartItems();
        this.$store.commit("setNumberOfCartItems", numberOfCartItems);
      }
    },
    setCartJumpFalse: function () {
      this.cartJump = false;
    },
    toggleBurger: function () {
      if (window.burger_active) {
        document.querySelector("body").classList.remove("overflow--hide");
        document
          .querySelector(".header__burger")
          .classList.remove("header__burger--close");
        document
          .querySelector(".header__navigation--mobile")
          .classList.remove("toggle-menu");
        document.querySelector(".navigation__backdrop").style.display = "none";
      } else {
        document.querySelector("body").classList.add("overflow--hide");
        document
          .querySelector(".header__burger")
          .classList.add("header__burger--close");
        document
          .querySelector(".header__navigation--mobile")
          .classList.add("toggle-menu");
        document.querySelector(".navigation__backdrop").style.display = "block";
      }

      window.burger_active = !window.burger_active;
    },
    toggleUserMenu: function () {
      this.showUserMenu = !this.showUserMenu;
    },
    closeUserMenu: function () {
      this.openCloseUserMenu(false);
    },
    openCloseUserMenu: function (shouldOpen) {
      if (shouldOpen) {
        this.showUserMenu = true;
      } else {
        this.showUserMenu = false;
      }
    },
  },
  mounted() {
    this.setNumberOfCartItems();
  },
  watch: {
    numberOfCartItems: function (newValue, oldValue) {
      if (newValue > oldValue) {
        this.cartJump = true;
        setTimeout(this.setCartJumpFalse, 1400);
      }
    },
  },
};
</script>

<style scoped>
.menu-item__cart {
  position: relative;
  background-color: #f5f4f5;
  box-shadow: inset 7px 0 9px -7px rgba(0, 0, 0, 0.1);
}
.cart-number-wrapper {
  position: absolute;
  top: 0;
  right: 0;
  height: 30px;
  width: 30px;
  transform: translate(35%, -35%);
}
.cart-number {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
  width: 100%;
  background: #47c19c;
  color: #ffffff;
  font-size: 11px;
  font-weight: 700;
  font-family: serif;
  border-radius: 50%;
  border: 4px solid #ffffff;
  margin: 0;
}
.cart-number-mobile {
  padding: 5px;
  background-color: #cbe6df;
  border-radius: 20px;
}

.header__user ul {
  display: flex;
}

.user-menu {
  position: relative;
  min-width: 125px;
}

.user-menu__user {
  padding: 13px 57px 13px 20px;
  background-color: #f5f4f5;
  font-weight: 600;
  text-transform: uppercase;
  user-select: none;
  cursor: pointer;
}

.user-menu .drop-icon {
  position: absolute;
  top: calc(50% - 6px);
  right: 32px;
  height: 8px;
  width: 8px;
  transform: rotate(225deg);
}

.fold-enter-active,
.fold-leave-active {
  transition: all 0.5s;
  transform: scaleY(1);
}
.fold-enter,
.fold-leave-to {
  transform: scaleY(0);
}

.drop-icon::before,
.drop-icon::after {
  content: "";
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 2px;
  border-radius: 1px;
  background-color: #000000;
}

.drop-icon::after {
  transform: rotate(90deg);
  transform-origin: left;
}

ul .menu-item__user {
  position: absolute;
  flex-direction: column;
  left: 0;
  top: 100%;
  width: 100%;
  background-color: #f5f4f5;
  text-transform: initial;
  transition: all 0.3s ease-out;
  transform-origin: top;
}

ul .menu-item__user li {
  transition: all 0.2s ease-out;
}

ul .menu-item__user li:last-child {
  color: #ff5755;
}

ul .menu-item__user--active {
  display: flex;
}

.jump {
  animation-name: angry-animation;
  animation-duration: 1.4s;
  animation-timing-function: ease;
  animation-delay: 0s;
  animation-iteration-count: 1;
  animation-direction: normal;
}
@keyframes angry-animation {
  0% {
    -webkit-transform: translateY(0);
    transform: translateY(0);
  }
  20% {
    -webkit-transform: translateY(0);
    transform: translateY(0);
  }
  40% {
    -webkit-transform: translateY(-30px);
    transform: translateY(-30px);
  }
  50% {
    -webkit-transform: translateY(0);
    transform: translateY(0);
  }
  60% {
    -webkit-transform: translateY(-15px);
    transform: translateY(-15px);
  }
  80% {
    -webkit-transform: translateY(0);
    transform: translateY(0);
  }
  100% {
    -webkit-transform: translateY(0);
    transform: translateY(0);
  }
}

.slide-enter-active,
.slide-leave-active {
  transition: max-height 0.35s;
}
.slide-enter,
.slide-leave-to {
  max-height: 0px;
}

.slide-enter-to,
.slide-leave {
  max-height: 20em;
}
.navbar-logo {
  max-height: 45px;
  margin-left: 5px;
  margin-top: -15px;
  margin-bottom: -10px;
}
</style>

<style>
/*--------------------------------------------------------------
# Hero styling
--------------------------------------------------------------*/
header {
  border-bottom: 1px solid #f5f4f5;
}
header a {
  color: inherit;
}
header a:hover {
  color: #2dac86;
  text-decoration: none;
}

/*--------------------------------------------------------------
# Navigation styling
--------------------------------------------------------------*/
nav ul {
  margin: 0;
  padding: 0;
}
nav ul li {
  display: inline-block;
}
nav ul li a {
  display: block;
  padding: 13px 20px;
}

/*--------------------------------------------------------------
# Header Top
--------------------------------------------------------------*/
.header__top {
  position: relative;
  display: none;
  width: 100%;
  background: #f5f4f5;
}
@media only screen and (min-width: 992px) {
  .header__top {
    display: block;
  }
}

.header__useful-links {
  display: none;
  -webkit-box-pack: end;
  -ms-flex-pack: end;
  justify-content: flex-end;
  color: #4e4e4e;
  font-size: 14px;
}
@media only screen and (min-width: 992px) {
  .header__useful-links {
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
  }
}
.header__useful-links ul li:last-of-type a {
  padding-right: 0;
}

/*--------------------------------------------------------------
# Header
--------------------------------------------------------------*/
.header__content {
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-pack: justify;
  -ms-flex-pack: justify;
  justify-content: space-between;
  -webkit-box-align: center;
  -ms-flex-align: center;
  align-items: center;
  padding: 15px 0;
  color: #000000;
  font-family: "Assistant", sans-serif;
  font-size: 18px;
}
@media only screen and (min-width: 992px) {
  .header__content {
    padding: 25px 0;
  }
}
.header__content .navigation__backdrop {
  position: fixed;
  display: none;
  top: 81px;
  right: 0;
  bottom: 0;
  left: 0;
  background-color: rgba(40, 40, 40, 0.3);
  z-index: 500;
}
.header__content .header__navigation {
  display: block;
}
.header__content .header__navigation--mobile {
  position: fixed;
  display: block;
  top: 81px;
  right: 0;
  bottom: 0;
  left: 25%;
  background-color: #ffffff;
  z-index: 1000;
  -webkit-transform: translateX(100%);
  -ms-transform: translateX(100%);
  transform: translateX(100%);
  -webkit-transition: all 0.3s ease-out;
  transition: all 0.3s ease-out;
}
.header__content .header__navigation--mobile ul {
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: vertical;
  -webkit-box-direction: normal;
  -ms-flex-direction: column;
  flex-direction: column;
}
.header__content .header__navigation--mobile ul li {
  border-bottom: 1px solid #f5f4f5;
}
.header__content .header__navigation--desktop {
  display: none;
}
@media only screen and (min-width: 992px) {
  .header__content .header__navigation--mobile {
    display: none;
  }
  .header__content .header__navigation--desktop {
    display: block;
  }
}
.header__content .toggle-menu {
  -webkit-transform: translateX(0);
  -ms-transform: translateX(0);
  transform: translateX(0);
}
.header__content .header__user {
  display: none;
}
@media only screen and (min-width: 992px) {
  .header__content .header__user {
    display: block;
  }
}
.header__content .header__burger {
  position: relative;
  height: 18px;
  width: 24px;
  cursor: pointer;
}
@media only screen and (min-width: 992px) {
  .header__content .header__burger {
    display: none;
  }
}
.header__content .header__burger span {
  position: absolute;
  top: -webkit-calc(50% - 1px);
  top: calc(50% - 1px);
  left: 0;
  width: 100%;
  height: 2px;
  background-color: #282828;
  -webkit-transition: all 0.3s ease-out;
  transition: all 0.3s ease-out;
  -webkit-transform-origin: center center;
  -ms-transform-origin: center center;
  transform-origin: center center;
}
.header__content .header__burger span:first-child {
  top: 0;
}
.header__content .header__burger span:last-child {
  top: -webkit-calc(100% - 2px);
  top: calc(100% - 2px);
}
.header__content .header__burger--close span {
  -webkit-transform: rotate(45deg);
  -ms-transform: rotate(45deg);
  transform: rotate(45deg);
}
.header__content .header__burger--close span:first-child,
.header__content .header__burger--close span:last-child {
  -webkit-transform: rotate(0);
  -ms-transform: rotate(0);
  transform: rotate(0);
  opacity: 0;
}
.header__content .header__burger--close span:nth-child(2) {
  -webkit-transform: rotate(-45deg);
  -ms-transform: rotate(-45deg);
  transform: rotate(-45deg);
}

.logo {
  height: 50px;
}
@media only screen and (min-width: 992px) {
  .logo {
    height: 60px;
  }
}
</style>
