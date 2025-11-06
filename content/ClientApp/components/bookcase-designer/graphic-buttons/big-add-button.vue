<template>
  <svg :width="size + 'px'" :height="size + 'px'">
    <rect :width="size" :height="size" fill="#2dac86" :rx="size / 20" :ry="size / 20" />
    <line
      :x1="size / 2"
      :y1="size * (0.5 + lineLength / 2)"
      :x2="size / 2"
      :y2="size * (0.5 - lineLength / 2)"
      :style="'stroke:black;stroke-width:' + size / 20"
      stroke-linecap="round"
    />
    <line
      :x1="size * (0.5 + lineLength / 2)"
      :y1="size / 2"
      :x2="size * (0.5 - lineLength / 2)"
      :y2="size / 2"
      :style="'stroke:black;stroke-width:' + size / 20"
      stroke-linecap="round"
    />
  </svg>
</template>

<script>
export default {
  props: {
    size: Number
  },
  data() {
    return {
      lineLength: 0.62
    };
  }
};
</script>
