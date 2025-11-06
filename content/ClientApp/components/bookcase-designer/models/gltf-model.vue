<template>
  <span></span>
</template>
<script>
import * as THREE from "three";
require("../../../extensions/threejs/GLTFLoader.js");
export default {
  name: "gltf-model",
  inject: ["vglNamespace"],
  props: {
    path: String,
    rotation: Object,
    position: Object,
    scale: Object,
    scene: String,
    name: String
  },
  data() {
    return {
      gltf: null,
      object3d: new THREE.Object3D(),
      loader: new THREE.GLTFLoader()
    };
  },
  inject: {
    vglNamespace: "vglNamespace"
  },
  created() {
    this.vglNamespace.update();
  },
  beforeUpdate() {
    this.vglNamespace.update();
  },
  beforeDestroy() {
    if (this.object3d.parent) this.object3d.parent.remove(this.object3d);
    if (this.vglNamespace.object3ds[name] === this.object3d) {
      delete this.vglNamespace.object3ds[name];
    }
    this.vglNamespace.update();
  },
  computed: {
    threeScene: function() {
      return this.vglNamespace.object3ds[this.scene];
    }
  },
  methods: {
    loadModel: function() {
      this.loader.load(this.path, this.addModelToScene, undefined, function(
        error
      ) {
        console.error(error);
      });
    },
    addModelToScene: function(gltf) {
      this.gltf = gltf;

      this.object3d.add(...this.gltf.scene.children);

      this.object3d.scale.set(this.scale.x, this.scale.y, this.scale.z);
      this.object3d.rotation.set(
        this.rotation.x,
        this.rotation.y,
        this.rotation.z
      );
      this.object3d.position.set(
        this.position.x,
        this.position.y,
        this.position.z
      );
      if (this.name !== undefined) {
        this.vglNamespace.object3ds[this.name] = this.object3d;
      }
      this.threeScene.add(this.object3d);
      this.vglNamespace.update();
    }
  },
  watch: {
    scale: function(scale) {
      this.object3d.scale.set(this.scale.x, this.scale.y, this.scale.z);
    },
    rotation: function(rotation) {
      this.object3d.rotation.set(
        this.rotation.x,
        this.rotation.y,
        this.rotation.z
      );
    },
    position: function(position) {
      this.object3d.position.set(
        this.position.x,
        this.position.y,
        this.position.z
      );
    }
  },
  mounted() {
    this.loadModel();
  }
};
</script>
