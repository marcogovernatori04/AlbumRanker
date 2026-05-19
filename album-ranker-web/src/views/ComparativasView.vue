<script setup>
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { getArtistas } from '../api/artistaApi'

const router = useRouter()

const artistas = ref([])
const artistaId = ref('')
const loading = ref(true)
const error = ref(null)

onMounted(async () => {
  try {
    artistas.value = await getArtistas()
  } catch (err) {
    error.value = 'No se pudieron cargar los artistas.'
    console.error(err)
  } finally {
    loading.value = false
  }
})

function verComparativa() {
  if (!artistaId.value) return

  router.push(`/artistas/${artistaId.value}/comparativa`)
}
</script>

<template>
  <section>
    <div class="mb-5 border-b border-neutral-800 pb-4">
      <h2 class="terminal-heading">~/comparar</h2>
      <p class="mt-1 text-sm text-neutral-500">
        comparacion de puntuaciones por artista
      </p>
    </div>

    <div class="terminal-panel w-full max-w-xl p-4">
      <p v-if="loading" class="text-neutral-400">Cargando artistas...</p>

      <p v-else-if="error" class="text-red-400">
        {{ error }}
      </p>

      <div v-else class="space-y-4">
        <div>
          <label class="terminal-label">
            artista
          </label>

          <select v-model="artistaId"
            class="terminal-input">
            <option value="" disabled>Seleccionar artista</option>

            <option v-for="artista in artistas" :key="artista.id" :value="artista.id">
              {{ artista.nombre }}
            </option>
          </select>
        </div>

        <button type="button"
          class="terminal-btn-primary w-full sm:w-auto"
          :disabled="!artistaId" @click="verComparativa">
          ver comparativa
        </button>
      </div>
    </div>
  </section>
</template>
