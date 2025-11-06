<template>
  <svg :width="size + 'px'" :height="size + 'px'">
    <rect :width="size" :height="size" fill="rgb(255,130,130)" :rx="size/2.5" :ry="size/2.5"></rect>
    <line
      :x1="size * (0.5 + lineLength/2)"
      :y1="size * (0.5 + lineLength/2)"
      :x2="size * (0.5 -lineLength/2)"
      :y2="size * (0.5 -lineLength/2)"
      :style="'stroke:black;stroke-width:' + size/20 "
      stroke-linecap="round"
    ></line>
    <line
      :x1="size * (0.5+lineLength/2)"
      :y1="size * (0.5-lineLength/2)"
      :x2="size * (0.5-lineLength/2)"
      :y2="size * (0.5+lineLength/2)"
      :style="'stroke:black;stroke-width:' + size/20 "
      stroke-linecap="round"
    ></line>
  </svg>
</template>

<script>
export default {
  props: {
    size: Number
  },
  data() {
    return {
      lineLength: 0.5
    };
  }
};
</script>
