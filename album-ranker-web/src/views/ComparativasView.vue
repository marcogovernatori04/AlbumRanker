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
    <div class="mb-6">
      <h2 class="text-2xl font-bold text-white">Comparativas</h2>
      <p class="mt-1 text-sm text-neutral-500">
        Elegí un artista para comparar las puntuaciones de sus álbumes.
      </p>
    </div>

    <div class="max-w-xl rounded-2xl border border-neutral-800 bg-neutral-900 p-5">
      <p v-if="loading" class="text-neutral-400">Cargando artistas...</p>

      <p v-else-if="error" class="text-red-400">
        {{ error }}
      </p>

      <div v-else class="space-y-4">
        <div>
          <label class="mb-2 block text-sm text-neutral-400">
            Artista
          </label>

          <select v-model="artistaId"
            class="h-10 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-3 text-sm text-white outline-none focus:border-neutral-600">
            <option value="" disabled>Seleccionar artista</option>

            <option v-for="artista in artistas" :key="artista.id" :value="artista.id">
              {{ artista.nombre }}
            </option>
          </select>
        </div>

        <button type="button"
          class="inline-flex h-10 items-center rounded-lg bg-white px-4 text-sm font-semibold text-neutral-950 hover:bg-neutral-200 disabled:opacity-50"
          :disabled="!artistaId" @click="verComparativa">
          Ver comparativa
        </button>
      </div>
    </div>
  </section>
</template>
