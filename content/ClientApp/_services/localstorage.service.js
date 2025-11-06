export const localstorageService = {
  storeOrderConfirmation,
  getOrderConfirmation,
  storeBookcase,
  removeBookcase,
  bookcaseExists,
  getBookcase,
  setOpenDirectly,
  getOpenDirectly,
  addToCart,
  removeFromCart,
  resetShoppingCart,
  setNumberOfItems,
  getCart,
  getNumberOfCartItems,
  helpHasBeenShown,
  setHelpShown,
  cookieAccepted
};

function storeOrderConfirmation(orderConfirmation) {
  localStorage.setItem("orderConfirmation", JSON.stringify(orderConfirmation));
}

function getOrderConfirmation() {
  return getObjectStoredAsJson("orderConfirmation");
}

function storeBookcase(bookcase) {
  if (bookcase.getJson) {
    localStorage.setItem("shelferBookcase", bookcase.getJson);
  } else {
    localStorage.setItem("shelferBookcase", JSON.stringify(bookcase));
  }
}

function removeBookcase() {
  localStorage.removeItem("shelferBookcase");
}

function bookcaseExists() {
  return (
    localStorage.getItem("shelferBookcase") &&
    localStorage.getItem("shelferBookcase") !== "null" &&
    localStorage.getItem("shelferBookcase") !== "undefined"
  );
}

function getBookcase() {
  return getObjectStoredAsJson("shelferBookcase");
}

function setOpenDirectly(value) {
  if (value) {
    localStorage.setItem("openBookcaseDirectly", "true");
  } else {
    localStorage.removeItem("openBookcaseDirectly");
  }
}

function getOpenDirectly() {
  if (localStorage.getItem("openBookcaseDirectly") !== null) {
    return true;
  } else {
    return false;
  }
}

function addToCart(bookcaseId, standardModelId) {
  var shoppingCart = getObjectStoredAsJson("shelferShoppingCart");
  if (shoppingCart[bookcaseId]) {
    shoppingCart[bookcaseId].quantity += 1;
  } else {
    shoppingCart[bookcaseId] = {
      quantity: 1,
      standardModelId: standardModelId
    };
  }
  localStorage.setItem("shelferShoppingCart", JSON.stringify(shoppingCart));
  return shoppingCart;
}

function removeFromCart(bookcaseId) {
  var shoppingCart = getObjectStoredAsJson("shelferShoppingCart");
  if (shoppingCart[bookcaseId]) {
    delete shoppingCart[bookcaseId];
  }
  localStorage.setItem("shelferShoppingCart", JSON.stringify(shoppingCart));
  return shoppingCart;
}

function resetShoppingCart() {
  localStorage.removeItem("shelferShoppingCart");
}

function setNumberOfItems(bookcaseId, numberOfItems, standardModelId) {
  var shoppingCart = getObjectStoredAsJson("shelferShoppingCart");
  if (shoppingCart[bookcaseId]) {
    shoppingCart[bookcaseId].quantity = numberOfItems;
  } else {
    shoppingCart[bookcaseId] = {
      quantity: numberOfItems,
      standardModelId: standardModelId
    };
  }
  localStorage.setItem("shelferShoppingCart", JSON.stringify(shoppingCart));
  return shoppingCart;
}

function getCart() {
  return getObjectStoredAsJson("shelferShoppingCart");
}

function getNumberOfCartItems() {
  var shoppingCartString = localStorage.getItem("shelferShoppingCart");
  if (shoppingCartString) {
    var shoppingCart = JSON.parse(shoppingCartString);
    var numberOfCartItems = 0;
    Object.keys(shoppingCart).forEach(function(key) {
      numberOfCartItems += shoppingCart[key].quantity;
    });
    return numberOfCartItems;
  } else {
    return 0;
  }
}

function helpHasBeenShown() {
  return JSON.parse(localStorage.getItem("helpShown"));
}

function setHelpShown(value) {
  localStorage.setItem("helpShown", JSON.stringify(value));
}

function cookieAccepted() {
  return JSON.parse(localStorage.getItem("cookie:accepted"));
}

function getObjectStoredAsJson(key) {
  var retrievedObject = localStorage.getItem(key);
  if (retrievedObject) {
    return JSON.parse(retrievedObject);
  } else {
    return {};
  }
}
