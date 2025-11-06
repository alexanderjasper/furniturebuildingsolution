<template>
  <div class="spinner-outer-container">
    <div class="spinner-inner-container">
      <span class="spinner-border full-page-spinner" role="status" aria-hidden="true"></span>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {};
  }
};
</script>

<style>
.full-page-spinner {
  width: 10rem;
  height: 10rem;
}
.spinner-outer-container {
  text-align: center;
  background-color: rgba(255, 255, 255, 0.8);
  width: 100%;
  height: 100vh;
  position: absolute;
}
.spinner-inner-container {
  margin: 0;
  position: absolute;
  top: 50%;
  left: 50%;
  -ms-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);
}
</style>
