import config from "config";
import { authHeader } from "../_helpers";

export const userService = {
  login,
  logout,
  register,
  forgotPassword,
  changePassword,
  getAll
};

function login(emailAddress, password) {
  const requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ emailAddress, password })
  };

  return fetch(`${config.apiUrl}/users/authenticate`, requestOptions)
    .then(handleResponse)
    .then(user => {
      // login successful if there's a jwt token in the response
      if (user.token) {
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        localStorage.setItem("shelferUser", JSON.stringify(user));
      }

      return user;
    });
}

function logout() {
  // remove login data as well as other temporary data
  removeLocalStorageData();
}

function register(user) {
  const requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(user)
  };

  return fetch(`${config.apiUrl}/users/register`, requestOptions).then(
    handleResponse
  );
}

function forgotPassword(emailAddress) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify({ emailAddress })
  };

  return fetch(`${config.apiUrl}/users/forgotPassword`, requestOptions).then(
    handleResponse
  );
}

function changePassword(newPassword, recoveryKey) {
  const requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ newPassword: newPassword, recoveryKey: recoveryKey })
  };

  return fetch(`${config.apiUrl}/users/changePassword`, requestOptions).then(
    handleResponse
  );
}

function getAll() {
  const requestOptions = {
    method: "GET",
    headers: { ...authHeader(), "Content-Type": "application/json" }
  };

  return fetch(`${config.apiUrl}/users/getAll`, requestOptions).then(
    handleResponse
  );
}

function handleResponse(response) {
  return response.text().then(text => {
    const data = text && JSON.parse(text);
    if (!response.ok) {
      if (response.status === 401) {
        // auto logout if 401 response returned from api
        logout();
        location.reload(true);
      }

      const error = (data && data.message) || response.statusText;
      return Promise.reject(error);
    }

    return data;
  });
}

function removeLocalStorageData() {
  localStorage.removeItem("shelferUser");
  localStorage.removeItem("openBookcaseDirectly");
  localStorage.removeItem("shelferShoppingCart");
  localStorage.removeItem("orderConfirmation");
  localStorage.removeItem("helpShown");
}
