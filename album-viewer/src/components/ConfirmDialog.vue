<template>
  <Teleport to="body">
    <div v-if="show" :class="styles.overlay" @click.self="handleCancel">
      <div :class="styles.dialog">
        <div :class="styles.iconContainer">
          <div :class="styles.warningIcon">⚠️</div>
        </div>
        
        <h3 :class="styles.title">{{ title }}</h3>
        <p :class="styles.message">{{ message }}</p>
        
        <div :class="styles.actions">
          <button :class="styles.cancelBtn" @click="handleCancel" :disabled="loading">
            Cancel
          </button>
          <button :class="styles.confirmBtn" @click="handleConfirm" :disabled="loading">
            {{ loading ? 'Deleting...' : 'Delete' }}
          </button>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue'
import styles from './ConfirmDialog.module.css'

interface Props {
  show: boolean
  title?: string
  message?: string
}

interface Emits {
  (e: 'cancel'): void
  (e: 'confirm'): Promise<void>
}

const props = withDefaults(defineProps<Props>(), {
  title: 'Confirm Delete',
  message: 'Are you sure you want to delete this item? This action cannot be undone.'
})

const emit = defineEmits<Emits>()

const loading = ref(false)

const handleCancel = () => {
  if (!loading.value) {
    emit('cancel')
  }
}

const handleConfirm = async () => {
  loading.value = true
  try {
    await emit('confirm')
  } finally {
    loading.value = false
  }
}

const handleKeyDown = (e: KeyboardEvent) => {
  if (e.key === 'Escape') {
    handleCancel()
  }
}

watch(() => props.show, (newVal) => {
  if (newVal) {
    document.addEventListener('keydown', handleKeyDown)
  } else {
    document.removeEventListener('keydown', handleKeyDown)
  }
})
</script>
