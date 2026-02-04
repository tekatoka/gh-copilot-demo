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
      <button :class="[styles.btn, styles.btnPrimary]">Add to Cart</button>
      <button :class="[styles.btn, styles.btnSecondary]">Preview</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Album } from '../types/album'
import styles from './AlbumCard.module.css'

interface Props {
  album: Album
}

defineProps<Props>()

const handleImageError = (event: Event): void => {
  const target = event.target as HTMLImageElement
  target.src = 'https://via.placeholder.com/300x300/667eea/white?text=Album+Cover'
}
</script>
