<template>
  <div>
    <div class="standard-box" style="text-align: center">
      <div class="text-container">
        <h1>Kontakt os</h1>
        <div class="text-section">
          <div>Shelfer ApS</div>
          <div>Musvågevej 1, 3. th</div>
          <div>2400 København NV</div>
          <div>CVR: 40697713</div>
        </div>
        <h2>Værksted</h2>
        <div class="text-section">
          <div>Betafactory</div>
          <div>Bådehavnsgade 42</div>
          <div>2450 København SV</div>
        </div>

        <h2>Kontaktoplysninger</h2>
        <div class="text-section">
          <div>
            <b>Emil Jasper:</b>
          </div>
          <div>
            Tlf.
            <a href="tel:+4551179708">+45 4280 3076</a>
          </div>
          <div>
            E-mail
            <a href="mailto: info@shelfer.dk">info@shelfer.dk</a>
          </div>
        </div>

        <div class="text-section">
          <div>
            <a href="https://www.facebook.com/shelfer.dk/" target="_blank">Facebook</a> |
            <a href="https://www.instagram.com/shelfer.dk/" target="_blank">Instagram</a>
          </div>
        </div>
      </div>
    </div>
    <newsletter></newsletter>
    <page-footer></page-footer>
  </div>
</template>

<script>
import pageFooter from "./page-footer";
import newsletter from "./sections/products/newsletter";
export default {
  components: {
    "page-footer": pageFooter,
    newsletter: newsletter,
  },
  data() {
    return {};
  },
};
</script>

<style>
</style>
