<template>
  <gltf-model
    path="../../../resources/3dmodels/tv.glb.txt"
    :rotation="rotation"
    :position="centerOfBase"
    :scale="scale"
    :scene="scene"
    :name="name"
  ></gltf-model>
</template>
<script>
import gltfModel from "./gltf-model";
export default {
  name: "tv-model",
  components: {
    "gltf-model": gltfModel
  },
  inject: ["vglNamespace"],
  props: {
    centerOfBase: Object,
    maxWidth: Number,
    maxHeight: Number,
    maxDepth: Number,
    scene: String,
    name: String
  },
  data() {
    return {
      rotation: { x: -Math.PI / 2, y: 0, z: 0 }
    };
  },
  computed: {
    scale: function() {
      let scale = Math.min(
        this.maxHeight / 45000,
        this.maxWidth / 70000,
        this.maxDepth / 20000
      );
      return { x: scale, y: scale, z: scale };
    }
  }
};
</script>
