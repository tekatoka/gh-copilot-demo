<template>
  <div :id="chartContainerId" :class="styles.chartContainer"></div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { generateAlbumSalesPlot } from '../utils/viz';
import styles from './Graph.module.css';

const chartContainerId = 'chart-container';

onMounted(() => {
  const salesData = [
    { month: "January", sales: 150 },
    { month: "February", sales: 200 },
    { month: "March", sales: 250 },
    { month: "April", sales: 300 },
    { month: "May", sales: 280 },
    { month: "June", sales: 350 },
    { month: "July", sales: 400 },
    { month: "August", sales: 420 },
    { month: "September", sales: 380 },
    { month: "October", sales: 450 },
    { month: "November", sales: 480 },
    { month: "December", sales: 500 },
  ];
  generateAlbumSalesPlot(`#${chartContainerId}`, salesData);
});
</script>
