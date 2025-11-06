<template>
  <div>
    <hero></hero>

    <base-designs></base-designs>

    <text-image--right></text-image--right>

    <social-media></social-media>

    <text-image--left></text-image--left>

    <inspiration></inspiration>

    <page-footer></page-footer>
  </div>
</template>

<script>
import hero from "./sections/home/hero";
import baseDesigns from "./sections/home/base-designs";
import textImageRight from "./sections/home/text-image--right";
import socialMedia from "./sections/home/social-media";
import textImageLeft from "./sections/home/text-image--left";
import inspiration from "./sections/home/inspiration";
import pageFooter from "./page-footer";

export default {
  name: "home-page",
  components: {
    hero: hero,
    "base-designs": baseDesigns,
    "text-image--right": textImageRight,
    "social-media": socialMedia,
    "text-image--left": textImageLeft,
    inspiration: inspiration,
    "page-footer": pageFooter,
  },
  data() {
    return {};
  },
};
</script>

<style>
/*--------------------------------------------------------------
# Text Image styling
--------------------------------------------------------------*/
.text-image {
  margin-top: 70px;
}
.text-image .row {
  -webkit-box-orient: horizontal;
  -webkit-box-direction: reverse;
  -ms-flex-direction: row-reverse;
  flex-direction: row-reverse;
}
.text-image .row.image--left {
  -webkit-box-orient: horizontal;
  -webkit-box-direction: normal;
  -ms-flex-direction: row;
  flex-direction: row;
}
.text-image__image {
  position: relative;
  height: 100%;
  width: 100%;
}
.text-image__image img {
  min-height: 100%;
  min-width: 100%;
  -o-object-fit: cover;
  object-fit: cover;
  object-position: top;
}
@media only screen and (max-width: 768px) {
  .text-image__image img {
    max-height: 250px;
  }
}
.text-image__text {
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: vertical;
  -webkit-box-direction: normal;
  -ms-flex-direction: column;
  flex-direction: column;
  -webkit-box-pack: center;
  -ms-flex-pack: center;
  justify-content: center;
  height: 100%;
}
.text-image__text .btn {
  -ms-flex-item-align: start;
  align-self: flex-start;
  margin-bottom: 30px;
}
.text-image__paragraph {
  margin-bottom: 30px;
}
</style>
