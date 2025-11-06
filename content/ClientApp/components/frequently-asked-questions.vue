<template>
  <div>
    <div class="standard-box" style="text-align: center">
      <div class="text-container">
        <h1>Ofte stillede spørgsmål</h1>

        <div class="text-section">
          Her er nogle af spørgsmål, vi ofte stilles. Du må endelig kontakte os, hvis du har et spørgsmål eller et særligt ønske! Du kan komme i kontakt med Shelfer ved at ringe til os, skrive os en mail eller skrive via vores Messenger-chatfunktion her på siden. Du kan se vores kontaktoplysninger på siden
          <router-link to="/kontakt-os">Kontakt os</router-link>.
        </div>
        <div class="text-section">
          <router-link class="btn" to="/kontakt-os">Kontakt os</router-link>
        </div>

        <h2>Hvad er leveringstiden på mit Shelfer-møbel?</h2>
        <div
          class="text-section"
        >Vi leverer og monterer som udgangspunkt dit Shelfer-møbel inden for 1-4 uger.</div>

        <h2>Hvordan fungerer levering og montage hos Shelfer-reol?</h2>
        <div
          class="text-section"
        >Vi leverer og monterer dit Shelfer-møbel for dig (20 kr. pr. kilometer fra vores værksted i København). Vi kontakter dig for at aftale et tidspunkt, der passer dig. Derefter kan du bare læne dig tilbage og nyde dit værk!</div>

        <h2>Hvad koster et Shelfer-møbel?</h2>
        <div class="text-section">
          Dit Shelfer-møbel er helt unikt, og det er prisen også. Prisen opdateres automatisk i designværktøjet. Du kan se priseksempler under
          <router-link to="/produkter">Oplev produktet</router-link>.
        </div>
        <div class="text-section">
          <router-link class="btn" to="/produkter">Oplev produktet</router-link>
        </div>

        <h2>Hvilke materialer kan jeg vælge til mit Shelfer-møbel?</h2>
        <div
          class="text-section"
        >Dit Shelfer-møbel fremstilles af birkekrydsfiner i to varianter: en hvid variant belagt med Formica højtrykslaminat og en variant med klarlak. På begge varianter efterlades kanten rå med de synlige lag af birketræ.</div>
        <div
          class="text-section"
        >Birkekrydsfineren har en utrolig bæreevne. Både den hvide laminat og klarlakken giver en flot og slidstærk overflade – en ideel kombination for robuste og smukke møbler. Den lakerede birkekrydsfiner bevarer samtidigt den varme, smukke overflade med åretegninger fra birketræeet.</div>
        <div
          class="text-section"
        >Lågerne i dit Shelfer-møbel fås i tre varianter: en helt hvid, en hvid med grå kant og en helt grå. Den hvide variant fremstilles af melaminbelagt spånplade. Spånpladen er det traditionelle valg, da den er holdbar og nem at rengøre. Den grå lågevariant fremstilles af grå gennemfarvet MDF, som oliebehandles og giver et fantastisk samspil med birketræet. Den hvide låge med grå kant kombinerer fremstilles af den samme grå MDF-plade med en hvidmalet overflade og en smuk grå oliebehandlet MDF-kant.</div>
        <div
          class="text-section"
        >Ønsker du din reol i andre farver eller materialer, så tøv ikke med at kontakte os. Måske kan vi levere lige det du ønsker. Du vil i så fald modtage et uforpligtende tilbud på din specialordre.</div>
        <div class="text-section">
          <router-link class="btn" to="/kontakt-os">Kontakt os</router-link>
        </div>

        <h2>Jeg er i tvivl om målene på mit Shelfer-møbel. Tilbyder I opmåling?</h2>
        <div
          class="text-section"
        >Ja, vi vil hellere end gerne komme forbi og måle op! Vi tager dog lidt for det. Prisen for opmåling er 200 kr. + 20 kr. per kilometer fra vores værksted i København.</div>

        <h2>Jeg ønsker noget særligt, som jeg ikke kan konfigurere her på siden. Er det muligt at ændre i grunddesignet?</h2>
        <div
          class="text-section"
        >Ja, det er bestemt muligt, og der er mange flere muligheder, end du ser i designværktøjet. Vi kan for eksempel lave udskæringer til stikkontakter, dybere underskabe, en bagplade til et fjernsyn osv. Kontakt os endelig, hvis du har et særligt ønske. Så sender vi et uforpligtende tilbud.</div>

        <h2>Bliver mit Shelfer-møbel fastmonteret?</h2>
        <div
          class="text-section"
        >Hvis dit Shelfer-møbel har en vis højde og/eller tyngde, fastgør vi det til væggen. Vi sørger så vidt muligt for, at monteringsbeslagene er skjulte. Væghængte møbler monteres med et skjult vægophæng.</div>

        <h2>Kan jeg få en udskæring til min fodliste, få mit møbel væghængt eller få en sokkel i bunden af mit Shelfer-møbel?</h2>
        <div class="text-section">Ja, det kan du vælge i funktionen "Tilvalg" i designværktøjet.</div>

        <h2>Kan jeg designe min reol eller mit skab, så det følger mit skrå loft?</h2>
        <div
          class="text-section"
        >Ja, det har vi en løsning til. Den kan ikke vælges i designværktøjet endnu, men du kan kontakte os for at høre nærmere.</div>

        <h2>Kan jeg designe mit Shelfer-møbel med skuffer, skydelåger, garderobestænger, flere farvemuligheder, ...?</h2>
        <div
          class="text-section"
        >Det har vi ikke udviklet i vores designværktøj endnu, men vi udvikler konstant på vores produkt, og vi er ikke bange for en udfordring. Kontakt os med dit ønske, så tager vi et kig på det.</div>

        <div class="text-section">
          <router-link class="btn" to="/kontakt-os">Kontakt os</router-link>
          <router-link class="btn" to="/reoldesigner">Design din reol nu</router-link>
        </div>
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
