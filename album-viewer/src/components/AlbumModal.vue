<template>
  <Teleport to="body">
    <div v-if="show" :class="styles.overlay" @click.self="handleClose">
      <div :class="styles.modal">
        <div :class="styles.modalHeader">
          <h2 :class="styles.modalTitle">{{ isEdit ? 'Edit Album' : 'Add New Album' }}</h2>
          <button :class="styles.closeBtn" @click="handleClose">&times;</button>
        </div>
        
        <form @submit.prevent="handleSubmit" :class="styles.modalForm">
          <div :class="styles.formGroup">
            <label :class="styles.label">Title *</label>
            <input 
              v-model="formData.title" 
              type="text" 
              :class="styles.input"
              required 
            />
          </div>

          <div :class="styles.formGroup">
            <label :class="styles.label">Artist *</label>
            <input 
              v-model="formData.artist" 
              type="text" 
              :class="styles.input"
              required 
            />
          </div>

          <div :class="styles.formRow">
            <div :class="styles.formGroup">
              <label :class="styles.label">Year *</label>
              <input 
                v-model.number="formData.year" 
                type="number" 
                :class="styles.input"
                min="1900"
                max="2100"
                required 
              />
            </div>

            <div :class="styles.formGroup">
              <label :class="styles.label">Price *</label>
              <input 
                v-model.number="formData.price" 
                type="number" 
                step="0.01"
                min="0"
                :class="styles.input"
                required 
              />
            </div>
          </div>

          <div :class="styles.formGroup">
            <label :class="styles.label">Image URL</label>
            <input 
              v-model="formData.image_url" 
              type="url" 
              :class="styles.input"
              placeholder="https://example.com/image.jpg"
            />
          </div>

          <div v-if="errorMessage" :class="styles.errorMessage">
            {{ errorMessage }}
          </div>

          <div :class="styles.modalActions">
            <button type="button" :class="styles.cancelBtn" @click="handleClose">
              Cancel
            </button>
            <button type="submit" :class="styles.submitBtn" :disabled="submitting">
              {{ submitting ? 'Saving...' : (isEdit ? 'Update' : 'Create') }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </Teleport>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import type { Album } from '../types/album'
import styles from './AlbumModal.module.css'

interface Props {
  show: boolean
  album?: Album | null
}

interface Emits {
  (e: 'close'): void
  (e: 'save', album: Partial<Album>): Promise<void>
}

const props = defineProps<Props>()
const emit = defineEmits<Emits>()

const formData = ref({
  title: '',
  artist: '',
  year: new Date().getFullYear(),
  price: 0,
  image_url: ''
})

const submitting = ref(false)
const errorMessage = ref<string | null>(null)

const isEdit = ref(false)

watch(() => props.show, (newVal) => {
  if (newVal) {
    errorMessage.value = null
    if (props.album) {
      isEdit.value = true
      formData.value = {
        title: props.album.title,
        artist: props.album.artist,
        year: props.album.year,
        price: props.album.price,
        image_url: props.album.image_url
      }
    } else {
      isEdit.value = false
      formData.value = {
        title: '',
        artist: '',
        year: new Date().getFullYear(),
        price: 0,
        image_url: ''
      }
    }
  }
})

const handleClose = () => {
  if (!submitting.value) {
    emit('close')
  }
}

const handleSubmit = async () => {
  try {
    submitting.value = true
    errorMessage.value = null
    
    const albumData: Partial<Album> = {
      ...formData.value,
      id: props.album?.id
    }
    
    await emit('save', albumData)
  } catch (err: any) {
    errorMessage.value = err.response?.data?.message || err.message || 'Failed to save album'
  } finally {
    submitting.value = false
  }
}
</script>
