<script>
import * as THREE from "three";

export default {
  inject: ["vglNamespace"],
  props: {
    camera: String,
    dragging: Boolean
  },
  data() {
    return {
      raycaster: null,
      mouse: null,
      eventListenerLoaded: false,
      clickedKey: "",
      lastClick: null,
      clickMoveLimit: 0.04
    };
  },
  computed: {
    cmr() {
      return this.vglNamespace.cameras[this.camera];
    },
    container() {
      return document.getElementById("bookcaseDesignerContainer");
    },
    renderer() {
      return this.vglNamespace.renderers[0].inst;
    }
  },
  watch: {
    cmr: {
      handler(cmr) {
        this.raycaster = new THREE.Raycaster();
        this.mouse = new THREE.Vector2();
        this.container.addEventListener("mousedown", this.onMouseDown, false);
        this.container.addEventListener("mouseup", this.onMouseCancel, false);
        this.container.addEventListener("touchstart", this.onTouchStart, false);
        this.container.addEventListener("touchend", this.onTouchEnd, false);
      },
      immediate: true
    }
  },
  created() {
    window.addEventListener('resize', this.onWindowResize, false);
  },
  destroyed() {
    if (this.container) {
      this.container.removeEventListener("mousedown", this.onMouseDown, false);
      this.container.removeEventListener("mouseup", this.onMouseCancel, false);
      this.container.removeEventListener(
        "touchstart",
        this.onTouchStart,
        false
      );
      this.container.removeEventListener("touchend", this.onTouchEnd, false);
    }
    window.removeEventListener('resize', this.onWindowResize, false);
  },
  methods: {
    onMouseDown: function(event) {
      if (event.button === 2) {
        return;
      }
      this.handleInteraction(event, false, false);
    },
    onMouseCancel: function(event) {
      if (event.button === 2) {
        return;
      }
      this.handleInteraction(event, true, false);
    },
    onTouchStart: function(event) {
      this.handleInteraction(event, false, true);
    },
    onTouchEnd: function(event) {
      this.handleInteraction(event, true, true);
    },
    onWindowResize: function() {
      this.cmr.aspect = window.innerWidth / window.innerHeight;
      this.cmr.updateProjectionMatrix();

      this.renderer.setSize(window.innerWidth, window.innerHeight);
    },
    handleInteraction: function(event, isMouseCancelOrTouchEnd, isTouch) {
      if (this.dragging) {
        return;
      }

      if (
        event.target.closest(".control-panel-content") ||
        event.target.closest(".undo-redo-help") ||
        event.target.closest(".modal-content")
      ) {
        return;
      }

      this.setMouse(event, isTouch, isMouseCancelOrTouchEnd);

      var clickedKey = this.getClickedKey();
      if (!isMouseCancelOrTouchEnd) {
        this.lastClick = { x: this.mouse.x, y: this.mouse.y };
        this.clickedKey = clickedKey;
        return;
      } else if (
        this.lastClick &&
        Math.sqrt(
          Math.pow(Math.abs(this.lastClick.x - this.mouse.x), 2) +
            Math.pow(Math.abs(this.lastClick.y - this.mouse.y), 2)
        ) < this.clickMoveLimit
      ) {
        this.handleKeyPress(clickedKey, false);
      } else {
        this.lastClick = null;
        this.clickedKey = "";
      }
    },
    getClickedKey: function() {
      this.raycaster.setFromCamera(this.mouse, this.cmr);
      var objects = this.convertObjectToArray(this.vglNamespace.object3ds);
      var intersects = this.raycaster.intersectObjects(objects);
      return this.findKey(intersects[0]);
    },
    setMouse: function(event, isTouch, isMouseCancelOrTouchEnd) {
      if (isTouch && !isMouseCancelOrTouchEnd) {
        if (event.touches.length === 1) {
          this.mouse.set(
            (event.touches[0].clientX / this.container.clientWidth) * 2 - 1,
            (1 - event.touches[0].clientY / this.container.clientHeight) * 2 - 1
          );
        }
      } else if (isTouch && isMouseCancelOrTouchEnd) {
        if (event.changedTouches.length === 1) {
          this.mouse.set(
            (event.changedTouches[0].clientX / this.container.clientWidth) * 2 -
              1,
            (1 -
              event.changedTouches[0].clientY / this.container.clientHeight) *
              2 -
              1
          );
        }
      } else {
        this.mouse.set(
          (event.offsetX / this.container.clientWidth) * 2 - 1,
          (1 - event.offsetY / this.container.clientHeight) * 2 - 1
        );
      }
    },
    handleKeyPress: function(key, shouldIgnore) {
      if (!shouldIgnore) {
        if (key) {
          key = key.substring(1);
          this.$emit("click", key);
        } else {
          this.$emit("click", "");
        }
      }
    },
    convertObjectToArray: function(object) {
      var array = [];
      for (var key in object) {
        if (key !== "c") {
          var foundObject = object[key];
          foundObject.key = key;
          array.push(foundObject);
        }
      }
      return array;
    },
    findKey: function(intersect) {
      if (intersect) {
        return intersect.object.key;
      }
      return null;
    }
  },
  render(h) {
    return h("div");
  }
};
</script>
