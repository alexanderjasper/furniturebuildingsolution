<template>
  <div>
    <div class="standard-box">
      <div class="text-container">
        <h1 class="page-header">Mød Shelfer</h1>
        <div
          class="text-section"
        >Velkommen til Shelfer. Den billige og effektive vej til specialbyggede reoler og skabe. Du er designeren og vælger selv, om du vil designe en indbygningsreol, et klædeskab, en nichereol, et eltavleskab eller en skoreol – et Shelfer-møbel kan nemlig skræddersys nøjagtigt efter dine behov.</div>

        <h2 class="text-subheader">Hvem står bag Shelfer?</h2>
        <div class="text-section">
          <img class src="/resources/images/Om_os.1.jpg" alt />
        </div>
        <div
          class="text-section"
        >Bag Shelfer er Alexander og Emil, to brødre fra København med en passion for møbler, design og teknik. Vi har skabt Shelfer ud fra en dybfølt passion og tro på, at digitale muligheder kan forbedre måden vi køber, designer og producerer møbler på i dag. Vi stræber efter at tilbyde møbler med høj grad af design-frihed uden at gå på kompromis med kvalitet, pris og miljø. Og vi er de bedste i verden til det!</div>

        <div class="text-section" style="text-align: center">
          <router-link class="btn" to="/produkter">Få inspiration fra vores glade kunder</router-link>
        </div>

        <h2 class="text-subheader">Hvem er Alexander?</h2>
        <div class="text-section">
          <img class src="/resources/images/Om_os.alexander.jpg" alt />
        </div>
        <div
          class="text-section"
        >Alexander er medstifter af Shelfer og er kode-geniet bag Shelfers hjemmeside. Med en baggrund i matematik og software har Alexander både en veludviklet logisk sans og flair for digital udvikling. Udover sine logiske evner har Alexander også hænderne skruet godt på og elsker at få træ mellem hænderne.</div>

        <h2 class="text-subheader">Hvem er Emil?</h2>
        <div class="text-section">
          <img class src="/resources/images/Om_os.emil.jpg" alt />
        </div>
        <div
          class="text-section"
        >Emil er medstifter og produktionschef hos Shelfer. Hans baggrund som designingeniør og erfaring med produktudvikling gør, at han ikke er bange for at kaste sig ud i nytænkende projekter og forstår at gå fra ide til færdigt produkt. Emil er en skaber af hjertet og elsker at arbejde med computerstyret design.</div>

        <div class="text-section" style="text-align: center">
          <router-link class="btn" to="/blog">Læs flere ideer på vores blog</router-link>
        </div>

        <h2 class="text-subheader">Hvordan begyndte Shelfer?</h2>
        <div class="text-section">
          <img class src="/resources/images/Karens_reol.1.jpg" alt />
        </div>
        <div
          class="text-section"
        >Som så mange andre gode ideer blev Shelfer skabt af en frustration. Alexander og hans kæreste ventede barn og ville udnytte den begrænsede opbevaringsplads i deres københavner-lejlighed med en indbygningsreol. Alexander undersøgte markedet og fandt kun to gangbare muligheder: målfaste moduler, som kunne sættes sammen uden de helt rigtige mål, eller snedkerbyggede indbygningsreoler, som var alt for dyre og upraktiske at anskaffe. Løsningen midt imellem fandtes ikke - en billig og nem vej til specialtilpassede reoler og skabe efter mål. Alexander luftede ideen med sin bror Emil, og siden har de ikke set sig tilbage.</div>
      </div>

      <div class="text-section" style="text-align: center">
        <router-link class="btn" to="/reoldesigner">Design din egen reol nu</router-link>
      </div>
    </div>
    <inspiration></inspiration>
    <page-footer></page-footer>
  </div>
</template>

<script>
import pageFooter from "./page-footer";
import inspiration from "./sections/home/inspiration";
export default {
  components: {
    "page-footer": pageFooter,
    inspiration: inspiration
  },
  data() {
    return {};
  }
};
</script>

<style>
</style>
