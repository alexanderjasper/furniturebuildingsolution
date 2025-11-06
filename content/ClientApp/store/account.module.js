import { userService } from "../_services";
import { default as router } from "../router";

const user = JSON.parse(localStorage.getItem("shelferUser"));
const state = user
  ? { status: { loggedIn: true }, user }
  : { status: {}, user: null };

const actions = {
  login({ dispatch, commit }, { emailAddress, password }) {
    commit("loginRequest", { emailAddress });

    userService.login(emailAddress, password).then(
      user => {
        commit("loginSuccess", user);
        router.push("/");
        setTimeout(() => {
          // display success message after route change completes
          dispatch("alert/success", "Du er nu logget ind.", { root: true });
        });
      },
      error => {
        commit("loginFailure", error);
        dispatch("alert/error", error, { root: true });
      }
    );
  },
  logout({ commit }) {
    userService.logout();
    commit("logout");
  },
  register({ dispatch, commit }, user) {
    commit("registerRequest", user);

    userService.register(user).then(
      registerResult => {
        commit("registerSuccess", registerResult);

        userService.login(user.emailAddress, user.password).then(
          loginResult => {
            commit("loginSuccess", loginResult);
            router.push("/");
            setTimeout(() => {
              // display success message after route change completes
              dispatch("alert/success", "Du er nu logget ind.", { root: true });
            });
          },
          error => {
            commit("loginFailure", error);
            dispatch("alert/error", error, { root: true });
          }
        );
      },
      error => {
        commit("registerFailure", error);
        dispatch("alert/error", error, { root: true });
      }
    );
  }
};

const mutations = {
  loginRequest(state, user) {
    state.status = { loggingIn: true };
    state.user = user;
  },
  loginSuccess(state, user) {
    state.status = { loggedIn: true };
    state.user = user;
  },
  loginFailure(state) {
    state.status = {};
    state.user = null;
  },
  logout(state) {
    state.status = {};
    state.user = null;
  },
  registerRequest(state, user) {
    state.status = { registering: true };
  },
  registerSuccess(state, user) {
    state.status = {};
  },
  registerFailure(state, error) {
    state.status = {};
  }
};

export const account = {
  namespaced: true,
  state,
  actions,
  mutations
};
