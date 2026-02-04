<template>
  <div :class="styles.albumCard">
    <div :class="styles.albumImage">
      <img 
        :src="album.image_url" 
        :alt="album.title"
        @error="handleImageError"
        loading="lazy"
      />
      <div :class="styles.playOverlay">
        <div :class="styles.playButton">▶</div>
      </div>
    </div>
    
    <div :class="styles.albumInfo">
      <h3 :class="styles.albumTitle">{{ album.title }}</h3>
      <p :class="styles.albumYear">{{ album.year }}</p>
      <p :class="styles.albumArtist">{{ album.artist }}</p>
      <div :class="styles.albumPrice">
        <span :class="styles.price">${{ album.price.toFixed(2) }}</span>
      </div>
    </div>
    
    <div :class="styles.albumActions">
      <button :class="[styles.btn, styles.btnEdit]" @click="handleEdit">✏️ Edit</button>
      <button :class="[styles.btn, styles.btnDelete]" @click="handleDelete">🗑️ Delete</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Album } from '../types/album'
import styles from './AlbumCard.module.css'

interface Props {
  album: Album
}

interface Emits {
  (e: 'edit', album: Album): void
  (e: 'delete', album: Album): void
}

const props = defineProps<Props>()
const emit = defineEmits<Emits>()

const handleImageError = (event: Event): void => {
  const target = event.target as HTMLImageElement
  target.src = 'https://via.placeholder.com/300x300/667eea/white?text=Album+Cover'
}

const handleEdit = () => {
  emit('edit', props.album)
}

const handleDelete = () => {
  emit('delete', props.album)
}
</script>
