<script>
import Butter from "buttercms";
const butter = Butter("056b97c3deb91ab2376f506aef5745f36cacc7f6");
import pageFooter from "../page-footer";
import inspiration from "../sections/home/inspiration";
export default {
  name: "blog-post",
  components: {
    "page-footer": pageFooter,
    inspiration: inspiration
  },
  data() {
    return {
      post: null
    };
  },
  methods: {
    getPost() {
      butter.post
        .retrieve(this.$route.params.slug)
        .then(res => {
          this.post = res.data;
          document.title = "Shelfer - " + this.post.data.title;
          const tag = document.createElement("meta");
          tag.setAttribute("description", this.post.data.meta_description);
          tag.setAttribute("data-vue-router-controlled", "");
          document.head.appendChild(tag);
        })
        .catch(res => {
          console.log(res);
        });
    }
  },
  watch: {
    $route: {
      immediate: true,
      handler(to, from) {
        this.getPost();
      }
    }
  }
};
</script>

<template>
  <div>
    <div class="standard-box">
      <div class="text-container" v-if="post && post.data">
        <h1 class="page-header">{{ post.data.title }}</h1>
        <div v-html="post.data.body"></div>

        <div class="container">
          <div class="other-blog-posts">
            <h4>Læs vores andre blogindlæg</h4>
            <router-link to="/blog" class="btn">Forside</router-link>
            <router-link
              v-if="post.meta.previous_post"
              :to="'/' + post.meta.previous_post.slug"
              class="btn"
            >
              <icon fixed-width icon="arrow-left" />
              {{ post.meta.previous_post.title }}
            </router-link>
            <router-link
              v-if="post.meta.next_post"
              :to="'/' + post.meta.next_post.slug"
              class="btn"
            >
              {{ post.meta.next_post.title }}
              <icon fixed-width icon="arrow-right" />
            </router-link>
          </div>
        </div>
      </div>
    </div>
    <inspiration></inspiration>
    <page-footer></page-footer>
  </div>
</template>

<style scoped>
.other-blog-posts {
  margin: 40px 0;
}
</style>