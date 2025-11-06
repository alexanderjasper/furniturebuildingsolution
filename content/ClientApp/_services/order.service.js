import config from "config";
import { authHeader } from "../_helpers";
import { userService } from "./user.service";

export const orderService = {
  placeOrder,
  getAll,
  getOrderConfirmation
};

function placeOrder(orderData) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify(orderData)
  };

  return fetch(`${config.apiUrl}/order/placeOrder`, requestOptions).then(
    handleResponse
  );
}

function getAll() {
  const requestOptions = {
    method: "GET",
    headers: { ...authHeader(), "Content-Type": "application/json" }
  };

  return fetch(`${config.apiUrl}/order/getAll`, requestOptions).then(
    handleResponse
  );
}

function getOrderConfirmation(orderNumber) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify({ orderNumber: orderNumber })
  };

  return fetch(
    `${config.apiUrl}/order/getOrderConfirmation`,
    requestOptions
  ).then(handleResponse);
}

function handleResponse(response) {
  return response.text().then(text => {
    const data = text && JSON.parse(text);
    if (!response.ok) {
      if (response.status === 401) {
        // auto logout if 401 response returned from api
        userService.logout();
        location.reload(true);
      }

      const error = (data && data.message) || response.statusText;
      return Promise.reject(error);
    }

    return data;
  });
}
