<template>
  <div class="app">
    <header class="header">
      <h1>🎵 Album Collection</h1>
      <p>Discover amazing music albums</p>
      <nav class="nav-menu">
        <RouterLink to="/" class="nav-link">Albums</RouterLink>
        <RouterLink to="/graph" class="nav-link">Sales Graph</RouterLink>
      </nav>
    </header>

    <main class="main">
      <RouterView />
    </main>
  </div>
</template>

<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<style scoped>
.app {
  min-height: 100vh;
  padding: 2rem;
}

.header {
  text-align: center;
  margin-bottom: 3rem;
  color: white;
}

.header h1 {
  font-size: 3rem;
  margin-bottom: 0.5rem;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.3);
}

.header p {
  font-size: 1.2rem;
  opacity: 0.9;
}

.nav-menu {
  display: flex;
  gap: 2rem;
  justify-content: center;
  margin-top: 1.5rem;
}

.nav-link {
  color: white;
  text-decoration: none;
  padding: 0.75rem 1.5rem;
  border-radius: 25px;
  background: rgba(255, 255, 255, 0.1);
  transition: all 0.3s ease;
  border: 2px solid transparent;
  opacity: 0.6;
}

.nav-link:hover {
  background: rgba(255, 255, 255, 0.2);
  border-color: white;
  opacity: 0.9;
}

.nav-link.router-link-exact-active {
  background: white;
  color: #667eea;
  border-color: white;
  opacity: 1;
}

.main {
  max-width: 1200px;
  margin: 0 auto;
}

@media (max-width: 768px) {
  .app {
    padding: 1rem;
  }
  
  .header h1 {
    font-size: 2rem;
  }
}
</style>
