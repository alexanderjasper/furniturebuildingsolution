<template>
  <div>
    <div class="standard-box">
      <div class="text-container">
        <div class="container">
          <div class="row">
            <div class="col-12">
              <h1>Handelsbetingelser og persondatapolitik</h1>
              <h2>Vores oplysninger</h2>
              <p>
                <b>Shelfer ApS</b>
                <br />Adresse: Musvågevej 1, 3. Th., 2400 København NV
                <br />Telefonnummer: +45 42 80 30 76
                <br />CVR-nummer: 40697713
                <br />
              </p>
              <h2>Betaling</h2>
              <p>Shelfer modtager betaling med bankoverførsel eller kort, hvilket vil fremgå af din faktura tilsendt via e-mail efter bestilling af et produkt. Shelfers hjemmeside bruger danske kroner (DKK) til prissætning, og alle beløb på Shelfers hjemmeside er inkl. moms. Da Shelfers produkter specialfremstilles på ordre, er de forudbetalte, medmindre andet er aftalt.</p>
              <h2>Levering</h2>
              <p>Leveringstiden for din ordre er 1-4 uger, medmindre andet er aftalt. De fleste ordrer leveres dog væsentligt hurtigere.</p>
              <h2>Fortrydelsesret og returpolitik</h2>
              <p>
                Der gives ikke fortrydelsesret på varer købt på Shelfers hjemmeside, da varerne er kundekonfigurerede og har specielle mål. I henhold til dansk lovgivning kan og bør der derfor ikke være fortrydelsesret. Af samme grund tages varer ikke retur. Du kan læse mere om dette på
                <a
                  href="https://www.forbrugerombudsmanden.dk/hvad-gaelder/markedsfoeringsloven/webshopguide/kundens-fortrydelsesret/"
                  target="_blank"
                >forbrugerombudsmandens hjemmeside</a>.
              </p>
              <h2>Reklamationsret</h2>
              <p>
                Der gives 2 års reklamationsret på Shelfers produkter i henhold til den danske købelov. Reklamationsretten gælder for fejl i materiale og fabrikation. Reklamation vedrørende fejl og mangler skal meddeles til Shelfer i rimelig tid efter, at varen er modtaget, eller fejlen er opstået. Her anses maks. to måneder som rimelig tid, medmindre andet er aftalt.
                <br />Reklamationsretten frafalder ved forkert eller ualmindelig betjening af produktet. Shelfer dækker returneringsomkostningerne, herunder fragtomkostninger, i rimeligt omfang. Ved returnering kontaktes Shelfer via kontaktoplysningerne under "Vores oplysninger" ovenfor.
              </p>
              <h2>Persondatapolitik</h2>
              <p>
                Vi foretager registreringen af dine personoplysninger for at kunne levere varen til dig, og Shelfer har derfor brug for følgende oplysninger: Navn, adresse, telefonnummer og e-mailadresse.
                <br />Du kan til enhver tid få slettet dine oplysninger. Personoplysningerne registreres hos Shelfer og opbevares i op til fem år, hvorefter oplysningerne slettes. Direktørerne og de ansatte hos Shelfer har adgang til de førnævnte oplysninger. Den dataansvarlige hos Shelfer er: Alexander Jasper
                <br />Som kunde hos Shelfer har du altid ret til at gøre indsigelse mod registreringen. Du har også ret til indsigt i, hvilke oplysninger der er registreret om dig. Disse rettigheder har du i henhold til persondataloven, og henvendelse rettes til
                <a
                  href="mailto:info@shelfer.dk"
                >info@shelfer.dk</a>.
              </p>
              <h2>Klagemuligheder</h2>
              <p>
                I første omgang skal klager sendes til Shelfer via kontaktoplysningerne under "Vores oplysninger" ovenfor.
                <br />I sidste ende kan klager også sendes til:
                <br />
                <b>Konkurrence- og Forbrugerstyrelsen</b>
                <br />Carl Jacobsens Vej 35
                <br />2500 Valby
                <br />
                <a href="https://www.forbrug.dk/" target="_blank">www.forbrug.dk</a>
                <br />For EU-borgere uden for EU kan klager sendes via EU Kommissionens online klageplatform:
                <a
                  href="https://ec.europa.eu/"
                  target="_blank"
                >ec.europa.eu</a>
                <br />Ved klage skal vores e-mailadresse angives:
                <a
                  href="mailto:info@shelfer.dk"
                >info@shelfer.dk</a> samt hjemmesiden, du har købt varen på.
              </p>
            </div>
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
  name: "terms-and-conditions",
  components: {
    "page-footer": pageFooter,
    newsletter: newsletter,
  },
};
</script>

<style>
</style>
