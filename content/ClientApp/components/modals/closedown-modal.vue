<template>
  <div class="custom-modal">
    <div class="modal-content" style="max-width: 600px" v-click-outside="close">
      <h3>Shelfer er lukket</h3>
      <div>
        Hej venner! Vi har desværre en kedelig nyhed. <br /><br />

        Vi har besluttet at lukke ned for Shelfer.<br /><br />

        Vi har igennem et stykke tid haft alt for svært ved at få både økonomien
        og vores indbyrdes samarbejde til at fungere. Til sidst har vi så fundet
        ud af, at det hele er steget os over hovedet, og at vi begge har mistet
        både gejsten og troen på projektet. Derfor er vi enige om, at den
        rigtige beslutning er at lukke Shelfer stilfærdigt ned, lægge det bag os
        og søge lykken ad nye veje.<br /><br />

        Vi er enormt glade for og stolte af de produkter, vi nåede at udvikle,
        og vi er dybt taknemmelige for vores mange glade kunder, som har vist os
        stor tillid ved at bestille hos os som ny spiller på markedet. Det skal
        I have den største, dybfølte tak for! Samtidig er vi også kede af at
        måtte skuffe de potentielle nye kunder, som måske har gået og glædet sig
        til at bestille et Shelfer-møbel.<br /><br />

        Allerede bestilte ordrer leveres selvfølgelig som aftalt!<br /><br />

        Tak for alt, og må I have det skønt, indtil vi ses igen. 🙂<br /><br />

        Mvh. Emil og Alexander
      </div>
      <div class="modal-button-container">
        <button v-on:click="close()" class="btn btn-secondary">Luk</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "closedown-modal",
  props: {},
  methods: {
    close: function () {
      this.$emit("close");
    },
  },
};
</script>