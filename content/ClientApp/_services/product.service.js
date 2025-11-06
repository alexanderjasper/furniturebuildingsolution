import config from "config";

export const productService = {
  getCategories,
  getStandardModels,
  getStandardModel
};

function getCategories() {
  const requestOptions = {
    method: "GET",
    headers: { "Content-Type": "application/json" }
  };

  return fetch(`${config.apiUrl}/product/getCategories`, requestOptions).then(
    handleResponse
  );
}

function getStandardModels(categoryId) {
  const requestOptions = {
    method: "GET",
    headers: { "Content-Type": "application/json" }
  };

  var requestUrl = `${config.apiUrl}/product/getStandardModels`;
  if (categoryId) {
    requestUrl += "?categoryId=" + categoryId;
  }

  return fetch(requestUrl, requestOptions).then(handleResponse);
}

function getStandardModel(id) {
  const requestOptions = {
    method: "GET",
    headers: { "Content-Type": "application/json" }
  };

  return fetch(
    `${config.apiUrl}/product/getStandardModel?id=${id}`,
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
