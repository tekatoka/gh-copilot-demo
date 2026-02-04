<!-- AlbumsPage.vue -->
<template>
  <div v-if="loading" :class="styles.loading">
    <div :class="styles.spinner"></div>
    <p>Loading albums...</p>
  </div>

  <div v-else-if="error" :class="styles.error">
    <p>{{ error }}</p>
    <button @click="fetchAlbums" :class="styles.retryBtn">Try Again</button>
  </div>

  <div v-else :class="styles.albumsGrid">
    <AlbumCard 
      v-for="album in albums" 
      :key="album.id" 
      :album="album" 
    />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import axios from 'axios'
import AlbumCard from '../components/AlbumCard.vue'
import type { Album } from '../types/album'
import styles from './AlbumsPage.module.css'

const albums = ref<Album[]>([])
const loading = ref<boolean>(true)
const error = ref<string | null>(null)

const fetchAlbums = async (): Promise<void> => {
  try {
    loading.value = true
    error.value = null
    const response = await axios.get<Album[]>('/albums')
    albums.value = response.data
  } catch (err) {
    error.value = 'Failed to load albums. Please make sure the API is running.'
    console.error('Error fetching albums:', err)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchAlbums()
})
</script>
