import Vue from "vue";
import Vuex from "vuex";

import { alert } from "./alert.module";
import { account } from "./account.module";
import { users } from "./users.module";

Vue.use(Vuex);

// TYPES
const MAIN_SET_COUNTER = "MAIN_SET_COUNTER";

// STATE
const state = {
  counter: 1,
  numberOfCartItems: null
};

// MUTATIONS
const mutations = {
  [MAIN_SET_COUNTER](state, obj) {
    state.counter = obj.counter;
  },
  setNumberOfCartItems(state, numberOfCartItems) {
    state.numberOfCartItems = numberOfCartItems;
  },
  setShoppingCartTotalPrice(state, price) {
    state.shoppingCartTotalPrice = price;
  }
};

// ACTIONS
const actions = {
  setCounter({ commit }, obj) {
    commit(MAIN_SET_COUNTER, obj);
  }
};

export default new Vuex.Store({
  state,
  mutations,
  actions,
  modules: {
    alert,
    account,
    users
  }
});
