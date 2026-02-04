<template>
  <div :class="styles.app">
    <header :class="styles.header">
      <h1 :class="styles.title">💿 Album Collection</h1>
      <p :class="styles.subtitle">Discover amazing music albums</p>
      <nav :class="styles.navMenu">
        <RouterLink to="/" :class="styles.navLink">Albums</RouterLink>
        <RouterLink to="/graph" :class="styles.navLink">Sales Graph</RouterLink>
      </nav>
    </header>

    <main :class="styles.main">
      <RouterView />
    </main>
  </div>
</template>

<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import styles from './Layout.module.css'
</script>
