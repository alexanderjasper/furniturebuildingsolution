<template>
  <div class="custom-modal">
    <div class="modal-content" style="max-width: 550px" v-click-outside="close">
      <div style="margin-bottom: -1.5rem">
        <button class="close" v-on:click="close()">x</button>
      </div>

      <h3>Tilmeld nyhedsbrev</h3>
      <div id="mc_embed_signup">
        <form
          action="https://copalex.us3.list-manage.com/subscribe/post?u=6a53ff31002a21933ded758c0&amp;id=88392b2930"
          method="post"
          id="mc-embedded-subscribe-form"
          name="mc-embedded-subscribe-form"
          class="validate"
          target="_blank"
          novalidate
        >
          <div id="mc_embed_signup_scroll">
            <div id="mce-EMAIL-label">Din e-mailadresse:</div>
            <input
              type="email"
              name="EMAIL"
              class="email"
              id="mce-EMAIL"
              v-model="emailAddress"
              required
              style="width:100%"
            />
            <div
              id="mergeRow-gdpr"
              class="mergeRow gdpr-mergeRow content__gdprBlock mc-field-group"
            >
              <div class="content__gdpr">
                <p>Shelfer bruger oplysningerne, du indtaster i denne formular, til at komme i kontakt med dig og sende dig opdateringer og markedsføringsmateriale. Fortæl os, hvordan du ønsker at høre fra os:</p>
                <fieldset
                  class="mc_fieldset gdprRequired mc-field-group"
                  name="interestgroup_field"
                >
                  <label class="checkbox subfield" for="gdpr_24873">
                    <input
                      type="checkbox"
                      id="gdpr_24873"
                      name="gdpr[24873]"
                      value="Y"
                      class="av-checkbox gdpr"
                      v-model="emailChecked"
                    />
                    <span>
                      <b>Shelfer må kontakte mig via e-mail.</b>
                    </span>
                  </label>
                </fieldset>
                <p>Du kan altid afmelde ved at klikke på linket i bunden af de e-mails, du modtager.</p>
              </div>
              <div class="content__gdprLegal">
                <p>
                  Vi bruger Mailchimp som markedsføringsplatform. Ved at klikke på Tilmeld nedenfor accepterer du, at dine oplysninger sendes til Mailchimp til behandling.
                  <a
                    href="https://mailchimp.com/legal/"
                    target="_blank"
                  >Læs mere om Mailchimps privatlivspraksisser her.</a>
                </p>
              </div>
            </div>
            <!-- real people should not fill this in and expect good things - do not remove this or risk form bot signups-->
            <div style="position: absolute; left: -5000px;" aria-hidden="true">
              <input type="text" name="b_6a53ff31002a21933ded758c0_88392b2930" tabindex="-1" value />
            </div>
            <div class="clear">
              <input
                :disabled="!emailChecked || !emailAddress"
                type="submit"
                value="Tilmeld"
                name="subscribe"
                id="mc-embedded-subscribe"
                class="btn"
                v-on:click="submit()"
              />
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "email-subscribe-modal",
  props: {
    enteredEmail: String
  },
  data() {
    return {
      emailChecked: false,
      emailAddress: ""
    };
  },
  methods: {
    close: function() {
      this.$emit("close");
    },
    resetEmail: function() {
      this.$emit("resetEmail");
    },
    submit: function() {
      this.resetEmail();
      setTimeout(this.close);
    }
  },
  mounted() {
    this.emailAddress = this.enteredEmail;
  }
};
</script>
<style>
.content__gdpr,
.content__gdprLegal {
  font-size: 0.8rem;
}
#mc-embedded-subscribe-form input[type="checkbox"] {
  display: inline;
  margin-right: 10px;
}

#mergeRow-gdpr {
  margin-top: 20px;
}

#mce-EMAIL {
  font-size: 1rem;
  padding: 5px 7px;
  border-radius: 0.3125em;
  max-width: 100%;
  width: 30rem;
}

#mce-EMAIL-label {
  margin-bottom: 5px;
}
</style>
