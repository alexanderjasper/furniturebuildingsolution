<script>
import * as THREE from "three";
require("../../../extensions/threejs/OrbitControls.js");

export default {
  inject: ["vglNamespace"],
  props: {
    camera: String,
    noRotate: Boolean,
    enabled: Boolean,
    position: Object,
    bookcase: Object
  },
  data() {
    return {
      controls: null
    };
  },
  computed: {
    cmr() {
      return this.vglNamespace.cameras[this.camera];
    },
    plateDepth() {
      return this.bookcase.plateDepth;
    },
    doorMaterial() {
      return this.bookcase.doorMaterial;
    }
  },
  watch: {
    noRotate: function(newVal) {
      this.controls.enableRotate = !newVal;
    },
    enabled: function(newVal) {
      this.controls.enabled = newVal;
    },
    position: {
      deep: true,
      handler() {
        this.setCameraPosition();
      }
    },
    plateDepth() {
      this.vglNamespace.update();
    },
    doorMaterial() {
      this.vglNamespace.update();
    }
  },
  methods: {
    setCameraPosition() {
      this.cmr.position.set(this.position.x, this.position.y, this.position.z);
      this.controls.target.set(this.position.x, this.position.y, 0);
      this.controls.update();
    }
  },
  destroyed() {
    this.controls.dispose();
  },
  render(h) {
    return h("div");
  },
  mounted() {
    let container = document.getElementById("bookcaseDesignerContainer");
    this.controls = new THREE.OrbitControls(this.cmr, container);
    this.controls.enableRotate = !this.noRotate;
    this.controls.enabled = this.enabled;
    //TODO: make these into properties in the component.
    this.controls.minAzimuthAngle = -(Math.PI / 3);
    this.controls.maxAzimuthAngle = Math.PI / 3;
    this.controls.minPolarAngle = Math.PI / 6;
    this.controls.maxPolarAngle = Math.PI / 2;
    this.controls.minDistance = 0.1;
    this.controls.maxDistance = 4;
    // this.controls.enablePan = true;
    this.controls.screenSpacePanning = true;
    this.setCameraPosition();
    this.controls.addEventListener("change", () => {
      this.vglNamespace.update();
    });
  }
};
</script>
