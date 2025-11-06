<template>
  <div>
    <div class="standard-box align-center">
      <div style="max-width: 800px; margin: auto">
        <h1>Cookies og tracking på shelfer.dk</h1>
        <p>På shelfer.dk benytter vi os af følgende værktøjer for at hjælpe os med at forbedre brugeroplevelsen og vores markedsføring. Alle dataene indsamles i anonymiseret form og kan ikke spores specifikt til dig. Disse funktioner aktiveres, når du klikker på "Accepter" i bunden af siden.</p>
        <h2>Google Tag Manager</h2>
        <p>
          Anvendes til at analysere trafikken på vores websted.
          <a
            href="https://marketingplatform.google.com/about/tag-manager/"
            target="_blank"
          >Læs mere</a>
        </p>
        <h2>Facebook pixel</h2>
        <p>
          Anvendes til at analysere trafikken på vores websted og optimere vores online annoncer.
          <a
            href="https://da-dk.facebook.com/business/help/742478679120153?id=1205376682832142"
            target="_blank"
          >Læs mere</a>
        </p>
        <h2>Hotjar</h2>
        <p>
          Anvendes til at analysere trafikken på vores websted for at optimere brugeroplevelsen.
          <a
            href="https://help.hotjar.com/hc/en-us/sections/115003204947-What-is-Hotjar-"
            target="_blank"
          >Læs mere</a>
        </p>
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
