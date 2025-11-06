<template></template>
<script>
import * as THREE from "three";
import VglGeometry from "vue-gl/src/core/vgl-geometry";

export default {
  mixins: [VglGeometry],
  props: {
    data: Object,
    depth: Number
  },
  methods: {
    addPointsToShapeOrPath: function(shapeOrPath, points) {
      var startShapePoint = points[0];
      shapeOrPath.moveTo(startShapePoint[0], startShapePoint[1]);
      for (var k = 1; k < points.length; k++) {
        var shapePoint = points[k];
        if (shapePoint[2]) {
          shapeOrPath.bezierCurveTo(
            shapePoint[2][0],
            shapePoint[2][1],
            shapePoint[0],
            shapePoint[1],
            shapePoint[0],
            shapePoint[1]
          );
        } else {
          shapeOrPath.lineTo(shapePoint[0], shapePoint[1]);
        }
      }
      if (startShapePoint[2]) {
        shapeOrPath.bezierCurveTo(
          startShapePoint[2][0],
          startShapePoint[2][1],
          startShapePoint[0],
          startShapePoint[1],
          startShapePoint[0],
          startShapePoint[1]
        );
      } else {
        shapeOrPath.lineTo(startShapePoint[0], startShapePoint[1]);
      }
    }
  },
  computed: {
    inst() {
      var geometryPairs = [];
      for (var i = 0; i < this.data.components.length; i++) {
        var component = this.data.components[i];
        var shape = new THREE.Shape();
        this.addPointsToShapeOrPath(shape, component.shape);
        for (var j = 0; j < component.holes.length; j++) {
          var path = new THREE.Path();
          this.addPointsToShapeOrPath(path, component.holes[j]);
          shape.holes.push(path);
        }
        var geometry;

        if (this.depth) {
          var extrudeSettings = {
            depth: this.depth / 2,
            steps: 1,
            bevelEnabled: false,
            curveSegments: 16
          };
          geometry = new THREE.ExtrudeGeometry(shape, extrudeSettings);
        } else {
          geometry = new THREE.ShapeGeometry(shape);
        }
        geometry.applyMatrix(
          new THREE.Matrix4().makeTranslation(0, 0, -this.depth / 4)
        );

        var mesh = new THREE.Mesh(geometry);
        mesh.updateMatrix();
        geometryPairs.push([mesh.geometry, mesh.matrix]);
      }
      var mergedGeometry = new THREE.Geometry();
      geometryPairs.map(pair => mergedGeometry.merge(pair[0], pair[1]));
      return mergedGeometry;
    }
  }
};
</script>