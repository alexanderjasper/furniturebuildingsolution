import config from "config";
import { authHeader } from "../_helpers";
import { userService } from "./user.service";

export const bookcaseService = {
  save,
  getAll,
  getMaterials,
  getBlueprint,
  getDoorsBlueprint,
  get,
  getPublic,
  getDisplacements,
  deleteBookcase,
  editPlate,
  editCompartment,
  getData
};

function save(bookcaseData) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: bookcaseData.getJson
  };

  return fetch(`${config.apiUrl}/bookcase/save`, requestOptions).then(
    handleResponse
  );
}

function getAll() {
  const requestOptions = {
    method: "GET",
    headers: { ...authHeader(), "Content-Type": "application/json" }
  };

  return fetch(`${config.apiUrl}/bookcase/getAll`, requestOptions).then(
    handleResponse
  );
}

function getData() {
  const requestOptions = {
    method: "GET",
    headers: { ...authHeader(), "Content-Type": "application/json" }
  };

  return fetch(`${config.apiUrl}/bookcase/getData`, requestOptions).then(
    handleResponse
  );
}

function getMaterials() {
  const requestOptions = {
    method: "GET",
    headers: { ...authHeader(), "Content-Type": "application/json" }
  };

  return fetch(`${config.apiUrl}/bookcase/getMaterials`, requestOptions).then(
    handleResponse
  );
}

function getBlueprint(
  bookcaseId,
  plateThickness,
  doorPlateThickness,
  includeSingleSided,
  includeDoubleSided
) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify({
      id: bookcaseId,
      plateThickness: plateThickness,
      doorPlateThickness: doorPlateThickness,
      includeSingleSided: includeSingleSided,
      includeDoubleSided: includeDoubleSided
    })
  };

  return fetch(`${config.apiUrl}/bookcase/getBlueprint`, requestOptions).then(
    handleResponse
  );
}

function getDoorsBlueprint(bookcaseId, plateThickness, doorPlateThickness) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify({
      id: bookcaseId,
      plateThickness: plateThickness,
      doorPlateThickness: doorPlateThickness
    })
  };

  return fetch(
    `${config.apiUrl}/bookcase/getDoorsBlueprint`,
    requestOptions
  ).then(handleResponse);
}

function get(bookcaseId) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify({ id: bookcaseId })
  };

  return fetch(`${config.apiUrl}/bookcase/get`, requestOptions).then(
    handleResponse
  );
}

function getPublic(bookcaseId) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify({ id: bookcaseId })
  };

  return fetch(`${config.apiUrl}/bookcase/getPublic`, requestOptions).then(
    handleResponse
  );
}

function getDisplacements(bookcaseId) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify({ id: bookcaseId })
  };

  return fetch(
    `${config.apiUrl}/bookcase/getDisplacements`,
    requestOptions
  ).then(handleResponse);
}

function deleteBookcase(bookcaseId) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: JSON.stringify({ id: bookcaseId })
  };

  return fetch(`${config.apiUrl}/bookcase/deleteBookcase`, requestOptions).then(
    handleResponse
  );
}

function editPlate(plate) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: plate.getJson
  };

  return fetch(`${config.apiUrl}/bookcase/editPlate`, requestOptions).then(
    handleResponse
  );
}

function editCompartment(compartment) {
  const requestOptions = {
    method: "POST",
    headers: { ...authHeader(), "Content-Type": "application/json" },
    body: compartment.getJson
  };

  return fetch(
    `${config.apiUrl}/bookcase/editCompartment`,
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
