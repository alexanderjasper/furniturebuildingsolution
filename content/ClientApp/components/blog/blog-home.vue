<script>
import Butter from "buttercms";
const butter = Butter("056b97c3deb91ab2376f506aef5745f36cacc7f6");
import pageFooter from "../page-footer";
import newsletter from "../sections/products/newsletter";
export default {
  name: "blog-home",
  components: {
    "page-footer": pageFooter,
    newsletter: newsletter,
  },
  data() {
    return {
      page_title: "Blog",
      posts: [],
    };
  },
  methods: {
    getPosts() {
      butter.post
        .list({
          page: 1,
          page_size: 10,
        })
        .then((res) => {
          this.posts = res.data.data;
        });
    },
  },
  created() {
    this.getPosts();
  },
};
</script>

<template>
  <div>
    <div class="standard-box">
      <div class="text-container">
        <h1 class="page-header">{{ page_title }}</h1>
        <!-- Create `v-for` and apply a `key` for Vue. Here we are using a combination of the slug and index. -->
        <div class="container">
          <div v-for="(post,index) in posts" :key="post.slug + '_' + index">
            <router-link :to="'/' + post.slug">
              <div class="row blog-row">
                <div class="col-md-6 order-md-12 my-auto">
                  <h2>{{ post.title }}</h2>
                  <p>{{ post.summary }}</p>
                </div>
                <div class="col-md-6 order-md-1">
                  <figure>
                    <!-- Bind results using a `:` -->
                    <!-- Use a `v-if`/`else` if there is a `featured_image` -->
                    <img v-if="post.featured_image" :src="post.featured_image" alt />
                    <img v-else src="http://via.placeholder.com/250x250" alt />
                  </figure>
                </div>
              </div>
            </router-link>
          </div>
        </div>
      </div>
    </div>
    <newsletter></newsletter>
    <page-footer></page-footer>
  </div>
</template>

<style scoped>
.blog-row {
  margin-bottom: 60px;
}
</style>