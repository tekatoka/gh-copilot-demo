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

  <div v-else>
    <div :class="styles.header">
      <h2 :class="styles.title">Albums Collection</h2>
      <button @click="openAddModal" :class="styles.addBtn">
        ➕ Add New Album
      </button>
    </div>

    <div :class="styles.albumsGrid">
      <AlbumCard 
        v-for="album in albums" 
        :key="album.id" 
        :album="album"
        @edit="handleEdit"
        @delete="handleDeleteClick"
      />
    </div>
  </div>

  <AlbumModal 
    :show="showModal"
    :album="selectedAlbum"
    @close="closeModal"
    @save="handleSave"
  />

  <ConfirmDialog
    :show="showDeleteDialog"
    :title="`Delete ${albumToDelete?.title}?`"
    :message="`Are you sure you want to delete &quot;${albumToDelete?.title}&quot; by ${albumToDelete?.artist}? This action cannot be undone.`"
    @cancel="closeDeleteDialog"
    @confirm="handleDeleteConfirm"
  />
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import axios from 'axios'
import AlbumCard from '../components/AlbumCard.vue'
import AlbumModal from '../components/AlbumModal.vue'
import ConfirmDialog from '../components/ConfirmDialog.vue'
import type { Album } from '../types/album'
import styles from './AlbumsPage.module.css'

const albums = ref<Album[]>([])
const loading = ref<boolean>(true)
const error = ref<string | null>(null)
const showModal = ref(false)
const showDeleteDialog = ref(false)
const selectedAlbum = ref<Album | null>(null)
const albumToDelete = ref<Album | null>(null)

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

const openAddModal = () => {
  selectedAlbum.value = null
  showModal.value = true
}

const handleEdit = (album: Album) => {
  selectedAlbum.value = album
  showModal.value = true
}

const closeModal = () => {
  showModal.value = false
  selectedAlbum.value = null
}

const handleSave = async (albumData: Partial<Album>) => {
  try {
    if (selectedAlbum.value) {
      // Update existing album
      await axios.put(`/albums/${selectedAlbum.value.id}`, albumData)
    } else {
      // Create new album
      await axios.post('/albums', albumData)
    }
    
    await fetchAlbums()
    closeModal()
  } catch (err: any) {
    console.error('Error saving album:', err)
    throw err
  }
}

const handleDeleteClick = (album: Album) => {
  albumToDelete.value = album
  showDeleteDialog.value = true
}

const closeDeleteDialog = () => {
  showDeleteDialog.value = false
  albumToDelete.value = null
}

const handleDeleteConfirm = async () => {
  if (!albumToDelete.value) return
  
  try {
    await axios.delete(`/albums/${albumToDelete.value.id}`)
    await fetchAlbums()
    closeDeleteDialog()
  } catch (err) {
    console.error('Error deleting album:', err)
    error.value = 'Failed to delete album'
    closeDeleteDialog()
  }
}

onMounted(() => {
  fetchAlbums()
})
</script>
