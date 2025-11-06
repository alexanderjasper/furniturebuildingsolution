<template>
  <!-- Section Social Media Start -->
  <div class="so-me">
    <div class="so-me__background"></div>
    <div class="container">
      <div class="row">
        <div class="col-md-6">
          <span class="hashtag hashtag--yellow">#instagram</span>
          <h2 class="so-me__title">Følg med i vores hverdag</h2>
        </div>
        <div class="col-md-6 so-me__background--image">
          <img src="/resources/images/shelfer-background.svg" alt="Shelfer title" />
        </div>
      </div>
    </div>

    <div class="container">
      <div class="row">
        <div class="col-md-12">
          <div class="elfsight-app-cb35d1d0-3fd1-44c8-9645-39bae0a181de"></div>
        </div>
      </div>
    </div>

    <div class="container">
      <div class="row">
        <div class="col-auto ml-auto">
          <a
            href="https://www.instagram.com/shelfer.dk/"
            class="text-link so-me__link"
            target="_blank"
          >Følg os på instagram</a>
        </div>
      </div>
    </div>
  </div>
  <!-- Section Social Media End -->
</template>

<script>
export default {
  name: "social-media",
  components: {},
  data() {
    return {};
  },
  computed: {},
  methods: {},
};
</script>

<style>
/*--------------------------------------------------------------
# Social Media styling
--------------------------------------------------------------*/
.so-me {
  position: relative;
  padding-top: 30px;
  padding-bottom: 30px;
  margin-top: 40px;
  /* &__link {
        margin-right: auto;
    } */
}

.so-me__link {
  margin-top: 20px;
}
@media only screen and (min-width: 768px) {
  .so-me {
    padding-top: 50px;
    padding-bottom: 50px;
    margin-top: 0;
  }
}
@media only screen and (min-width: 992px) {
  .so-me {
    padding-top: 70px;
    padding-bottom: 70px;
  }
}
.so-me__background {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: #cbe6df;
}
@media only screen and (min-width: 768px) {
  .so-me__background {
    width: -webkit-calc(50% + 15px);
    width: calc(50% + 15px);
  }
}
.so-me__background--image {
  display: none;
  overflow: hidden;
}
@media only screen and (min-width: 768px) {
  .so-me__background--image {
    display: block;
    margin-top: -50px;
  }
}
@media only screen and (min-width: 992px) {
  .so-me__background--image {
    display: block;
    margin-top: -70px;
  }
}
.so-me__background--image img {
  width: -webkit-calc(100% + 15px);
  width: calc(100% + 15px);
  max-width: unset;
  margin-top: -5px;
  margin-left: -15px;
}
.so-me__images {
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-pack: justify;
  -ms-flex-pack: justify;
  justify-content: space-between;
  overflow-x: scroll;
  margin-right: -15px;
  margin-left: -15px;
  margin-bottom: 20px;
  padding-left: 15px;
}
@media only screen and (min-width: 768px) {
  .so-me__images {
    overflow-x: hidden;
  }
}
.so-me__image {
  -webkit-box-flex: 1;
  -ms-flex: 1 1 20%;
  flex: 1 1 20%;
  margin-right: 20px;
}
@media only screen and (max-width: 768px) {
  .so-me__image {
    min-width: 200px;
  }
  .so-me__image:last-child img {
    margin-right: 15px;
  }
}
@media only screen and (min-width: 768px) {
  .so-me__image:nth-child(4),
  .so-me__image:nth-child(5),
  .so-me__image:nth-child(6) {
    display: none;
  }
}
@media only screen and (min-width: 992px) {
  .so-me__image:nth-child(4) {
    display: block;
  }
}
@media only screen and (min-width: 1200px) {
  .so-me__image:nth-child(5),
  .so-me__image:nth-child(6) {
    display: block;
  }
}
.so-me__image img {
  width: 100%;
  height: 100%;
  -o-object-fit: cover;
  object-fit: cover;
}
</style>
