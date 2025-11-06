<template>
  <span></span>
</template>
<script>
export default {
  name: "data-tools",
  data() {
    return {
      supportHeight: 60, //TODO: Make this come from backend
      scale: 5000
    };
  },
  methods: {
    scaledNumber: function(inputInMillimeters) {
      return inputInMillimeters / this.scale;
    },
    getVectorString: function(position) {
      return position.x + " " + position.y + " " + position.z;
    },
    getScaledVectorString: function(position) {
      return (
        this.scaledNumber(position.x) +
        " " +
        this.scaledNumber(position.y) +
        " " +
        this.scaledNumber(position.z)
      );
    },
    getById: function(array, id) {
      var objects = array.filter(function(item) {
        return item.id === id;
      });
      if (objects.length > 0) {
        return objects[0];
      }
      return null;
    }
  }
};
</script>
