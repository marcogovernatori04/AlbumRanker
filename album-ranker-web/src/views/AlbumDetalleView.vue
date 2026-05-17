<script setup>
import { onMounted, ref } from 'vue'
import { useRoute, RouterLink } from 'vue-router'
import { getAlbumById, addCancion } from '../api/albumApi'
import { formatearPuntuacion, clasePuntuacion } from '@/utils/scoreUtils'
import { convertirDuracionASegundos } from '@/utils/duracionUtils'

const route = useRoute()

const album = ref(null)
const loading = ref(true)
const error = ref(null)

const mostrarFormCancion = ref(false)
const guardandoCancion = ref(false)
const errorCancion = ref(null)

const nuevaCancion = ref({
  numeroPista: 1,
  titulo: '',
  duracionTexto: '',
  puntuacion: null,
  nota: ''
})

onMounted(async () => {
  try {
    album.value = await getAlbumById(route.params.id)
  } catch (err) {
    error.value = 'No se pudo cargar el álbum.'
    console.error(err)
  } finally {
    loading.value = false
  }
})

function formatearDuracion(segundos) {
  if (!segundos && segundos !== 0) return '-'

  const minutos = Math.floor(segundos / 60)
  const resto = segundos % 60

  return `${minutos}:${resto.toString().padStart(2, '0')}`
}

function prepNuevaCancion() {
  const siguienteNum = album.value?.canciones?.length
    ? Math.max(...album.value.canciones.map(c => c.numeroPista)) + 1
    : 1
  nuevaCancion.value = {
    numeroPista: siguienteNum,
    titulo: '',
    duracionTexto: '',
    puntuacion: null,
    nota: ''
  }

  errorCancion.value = null
  mostrarFormCancion.value = true
}

async function guardarNuevaCancion() {
  guardandoCancion.value = true
  errorCancion.value = null

  try {
    const payload = {
      numeroPista: Number(nuevaCancion.value.numeroPista),
      titulo: nuevaCancion.value.titulo,
      duracionSegundos: convertirDuracionASegundos(nuevaCancion.value.duracionTexto),
      puntuacion:
        nuevaCancion.value.puntuacion === null || nuevaCancion.value.puntuacion === ''
          ? null
          : Number(nuevaCancion.value.puntuacion),
      nota: nuevaCancion.value.nota || null
    }

    const cancionCreada = await addCancion(album.value.id, payload)

    album.value.canciones.push(cancionCreada)
    album.value.canciones.sort((a, b) => a.numeroPista - b.numeroPista)

    album.value = await getAlbumById(album.value.id)

    mostrarFormCancion.value = false
  } catch (err) {
    errorCancion.value = err.response?.data || err.message || 'No se pudo agregar la canción.'
    console.error(err)
  } finally {
    guardandoCancion.value = false
  }
}


</script>

<template>
  <section>
    <RouterLink to="/albumes" class="mb-6 inline-block text-sm text-neutral-400 hover:text-white">
      ← Volver a álbumes
    </RouterLink>

    <p v-if="loading" class="text-neutral-400">Cargando...</p>

    <p v-else-if="error" class="text-red-400">
      {{ error }}
    </p>

    <div v-else-if="album">
      <div class="mb-8 rounded-2xl border border-neutral-800 bg-neutral-900 p-6">
        <div class="flex flex-col gap-6 lg:flex-row lg:items-start lg:justify-between">
          <div class="flex flex-col gap-6 sm:flex-row">
            <div class="h-40 w-40 shrink-0 overflow-hidden rounded-2xl bg-neutral-800">
              <img v-if="album.urlPortada" :src="album.urlPortada" :alt="album.titulo"
                class="h-full w-full object-cover" />

              <div v-else class="flex h-full w-full items-center justify-center text-5xl font-bold text-neutral-500">
                {{ album.titulo.charAt(0) }}
              </div>
            </div>

            <div class="min-w-0">
              <h2 class="break-words text-4xl font-bold text-white">
                {{ album.titulo }}
              </h2>

              <p class="mt-2 text-lg text-neutral-400">
                {{ album.artista }}
              </p>

              <div class="mt-4 flex flex-wrap gap-3 text-sm text-neutral-400">
                <span class="rounded-full bg-neutral-800 px-3 py-1">
                  {{ album.anio ?? 'Sin año' }}
                </span>

                <span class="rounded-full bg-neutral-800 px-3 py-1">
                  {{ album.canciones.length }} canciones
                </span>
              </div>
            </div>
          </div>

          <div :class="[
            'flex h-20 w-20 shrink-0 items-center justify-center rounded-2xl text-2xl font-bold',
            clasePuntuacion(album.promedio)
          ]">
            {{ formatearPuntuacion(album.promedio) }}
          </div>
        </div>
      </div>

      <div class="mb-4 flex items-center justify-between">
        <h3 class="text-lg font-semibold text-white">Canciones</h3>

        <button type="button"
          class="rounded-lg bg-white px-4 py-2 text-sm font-semibold text-neutral-950 hover:bg-neutral-200"
          @click="prepNuevaCancion">
          Nueva canción
        </button>
      </div>

      <form v-if="mostrarFormCancion" class="mb-5 rounded-2xl border border-neutral-800 bg-neutral-900 p-5"
        @submit.prevent="guardarNuevaCancion">
        <div class="mb-4 flex items-center justify-between">
          <h4 class="font-semibold text-white">Agregar canción</h4>

          <button type="button" class="text-sm text-neutral-400 hover:text-white" @click="mostrarFormCancion = false">
            Cancelar
          </button>
        </div>

        <div class="grid grid-cols-1 gap-4 md:grid-cols-5">
          <div>
            <label class="mb-1 block text-sm text-neutral-400">#</label>
            <input v-model="nuevaCancion.numeroPista" type="number"
              class="h-10 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-3 text-sm text-white outline-none focus:border-neutral-600" />
          </div>

          <div class="md:col-span-2">
            <label class="mb-1 block text-sm text-neutral-400">Título</label>
            <input v-model="nuevaCancion.titulo" type="text" required
              class="h-10 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-3 text-sm text-white outline-none focus:border-neutral-600" />
          </div>

          <div>
            <label class="mb-1 block text-sm text-neutral-400">Duración</label>
            <input v-model="nuevaCancion.duracionTexto" type="text" placeholder="3:45"
              class="h-10 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-3 text-sm text-white outline-none focus:border-neutral-600" />
          </div>

          <div>
            <label class="mb-1 block text-sm text-neutral-400">Puntuación</label>
            <input v-model="nuevaCancion.puntuacion" type="number" min="0" max="10" step="0.5"
              class="h-10 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-3 text-sm text-white outline-none focus:border-neutral-600" />
          </div>
        </div>

        <div class="mt-4">
          <label class="mb-1 block text-sm text-neutral-400">Nota</label>
          <input v-model="nuevaCancion.nota" type="text"
            class="h-10 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-3 text-sm text-white outline-none focus:border-neutral-600" />
        </div>

        <p v-if="errorCancion"
          class="mt-4 rounded-lg border border-red-900 bg-red-950/40 px-4 py-3 text-sm text-red-300">
          {{ errorCancion }}
        </p>

        <div class="mt-5 flex justify-end">
          <button type="submit"
            class="rounded-lg bg-white px-4 py-2 text-sm font-semibold text-neutral-950 hover:bg-neutral-200 disabled:opacity-60"
            :disabled="guardandoCancion">
            {{ guardandoCancion ? 'Guardando...' : 'Guardar canción' }}
          </button>
        </div>
      </form>

      <div class="overflow-hidden rounded-2xl border border-neutral-800 bg-neutral-900">
        <table class="w-full table-fixed border-collapse text-left">
          <thead class="bg-neutral-950 text-sm text-neutral-400">
            <tr>
              <th class="w-16 px-5 py-4 font-medium">#</th>
              <th class="w-[45%] px-5 py-4 font-medium">Canción</th>
              <th class="w-28 px-5 py-4 font-medium">Duración</th>
              <th class="w-32 px-5 py-4 font-medium">Puntuación</th>
              <th class="w-[25%] px-5 py-4 font-medium">Nota</th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="cancion in album.canciones" :key="cancion.id"
              class="border-t border-neutral-800 hover:bg-neutral-800/60">
              <td class="px-5 py-4 text-neutral-500">
                {{ cancion.numeroPista }}
              </td>

              <td class="px-5 py-4 font-medium text-white">
                {{ cancion.titulo }}
              </td>

              <td class="px-5 py-4 text-neutral-400">
                {{ formatearDuracion(cancion.duracionSegundos) }}
              </td>

              <td class="px-5 py-4">
                <span :class="[
                  'inline-flex min-w-12 justify-center rounded-full px-3 py-1 font-semibold',
                  clasePuntuacion(cancion.puntuacion)
                ]">
                  {{ formatearPuntuacion(cancion.puntuacion) }}
                </span>
              </td>

              <td class="px-5 py-4 text-neutral-400">
                {{ cancion.nota || '-' }}
              </td>
            </tr>
          </tbody>
        </table>

        <div v-if="album.canciones.length === 0" class="px-5 py-8 text-center text-neutral-400">
          Este álbum todavía no tiene canciones.
        </div>
      </div>
    </div>
  </section>
</template>
