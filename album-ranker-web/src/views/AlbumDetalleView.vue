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

function formatearDuracionInput() {
  const valor = String(nuevaCancion.value.duracionTexto ?? '').trim()

  if (!valor) {
    nuevaCancion.value.duracionTexto = ''
    return
  }

  if (valor.includes(':')) {
    const [minutos = '', segundos = ''] = valor.split(':')
    nuevaCancion.value.duracionTexto = `${Number(minutos) || 0}:${segundos.padStart(2, '0').slice(0, 2)}`
    return
  }

  const soloNumeros = valor.replace(/\D/g, '')

  if (!soloNumeros) {
    nuevaCancion.value.duracionTexto = ''
    return
  }

  if (soloNumeros.length <= 2) {
    nuevaCancion.value.duracionTexto = `0:${soloNumeros.padStart(2, '0')}`
    return
  }

  const minutos = soloNumeros.slice(0, -2)
  const segundos = soloNumeros.slice(-2)
  nuevaCancion.value.duracionTexto = `${Number(minutos)}:${segundos}`
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
    <RouterLink to="/albumes" class="terminal-link mb-4 inline-block">
      ../albumes
    </RouterLink>

    <p v-if="loading" class="text-neutral-400">Cargando...</p>

    <p v-else-if="error" class="text-red-400">
      {{ error }}
    </p>

    <div v-else-if="album">
      <div class="terminal-panel mb-6 p-4">
        <div class="flex flex-col gap-4 lg:flex-row lg:items-start lg:justify-between">
          <div class="flex flex-col gap-3 sm:flex-row">
            <div class="h-24 w-24 shrink-0 overflow-hidden border border-neutral-800 bg-black">
              <img v-if="album.urlPortada" :src="album.urlPortada" :alt="album.titulo"
                class="h-full w-full object-cover" />

              <div v-else class="flex h-full w-full items-center justify-center text-2xl font-bold text-lime-200">
                {{ album.titulo.charAt(0) }}
              </div>
            </div>

            <div class="min-w-0">
              <p class="text-xs uppercase tracking-[0.12em] text-neutral-500">album.detail</p>
              <h2 class="break-words text-2xl font-bold text-white">
                {{ album.titulo }}
              </h2>

              <p class="mt-1 text-sm text-neutral-400">
                por {{ album.artista }}
              </p>

              <div class="mt-3 flex flex-wrap gap-2 text-sm text-neutral-500">
                <span class="border border-neutral-800 px-2 py-1">
                  año={{ album.anio ?? 'null' }}
                </span>

                <span class="border border-neutral-800 px-2 py-1">
                  canciones={{ album.canciones.length }}
                </span>
              </div>
            </div>
          </div>

          <div :class="[
            'flex h-14 min-w-20 shrink-0 items-center justify-center border px-4 text-xl font-bold',
            clasePuntuacion(album.promedio)
          ]">
            {{ formatearPuntuacion(album.promedio) }}
          </div>
        </div>
      </div>

      <div class="mb-3 flex flex-col gap-3 border-b border-neutral-800 pb-3 sm:flex-row sm:items-center sm:justify-between">
        <h3 class="terminal-heading">canciones</h3>

        <button type="button"
          class="terminal-btn-primary"
          @click="prepNuevaCancion">
          + cancion
        </button>
      </div>

      <form v-if="mostrarFormCancion" class="terminal-panel mb-5 p-4"
        @submit.prevent="guardarNuevaCancion">
        <div class="mb-3 flex items-center justify-between">
          <h4 class="text-sm font-semibold uppercase tracking-[0.1em] text-white">agregar cancion</h4>

          <button type="button" class="terminal-link" @click="mostrarFormCancion = false">
            cancelar
          </button>
        </div>

        <div class="grid grid-cols-1 gap-3 md:grid-cols-5">
          <div>
            <label class="terminal-label">#</label>
            <input v-model="nuevaCancion.numeroPista" type="number"
              class="terminal-input" />
          </div>

          <div class="md:col-span-2">
            <label class="terminal-label">titulo</label>
            <input v-model="nuevaCancion.titulo" type="text" required
              class="terminal-input" />
          </div>

          <div>
            <label class="terminal-label">duracion</label>
            <input v-model="nuevaCancion.duracionTexto" type="text" inputmode="numeric"
              pattern="[0-9]*:?[0-9]{0,2}" placeholder="3:45" class="terminal-input"
              @blur="formatearDuracionInput" />
          </div>

          <div>
            <label class="terminal-label">puntuacion</label>
            <input v-model="nuevaCancion.puntuacion" type="number" min="0" max="10" step="0.5"
              class="terminal-input" />
          </div>
        </div>

        <div class="mt-3">
          <label class="terminal-label">nota</label>
          <input v-model="nuevaCancion.nota" type="text"
            class="terminal-input" />
        </div>

        <p v-if="errorCancion"
          class="mt-3 border border-red-900 bg-red-950/40 px-3 py-2 text-sm text-red-300">
          {{ errorCancion }}
        </p>

        <div class="mt-3 flex justify-end">
          <button type="submit"
            class="terminal-btn-primary"
            :disabled="guardandoCancion">
            {{ guardandoCancion ? 'guardando...' : 'guardar cancion' }}
          </button>
        </div>
      </form>

      <div class="space-y-2 md:hidden">
        <div v-for="cancion in album.canciones" :key="`mobile-${cancion.id}`"
          class="terminal-panel grid grid-cols-[2rem_minmax(0,1fr)_3.5rem] items-center gap-3 p-3">
          <span class="text-sm text-neutral-500">{{ cancion.numeroPista }}</span>

          <div class="min-w-0">
            <p class="truncate text-base font-medium text-white">{{ cancion.titulo }}</p>
            <p class="mt-1 text-sm text-neutral-500">
              {{ formatearDuracion(cancion.duracionSegundos) }} · {{ cancion.nota || 'sin nota' }}
            </p>
          </div>

          <span :class="[
            'inline-flex h-8 w-14 items-center justify-center border text-base font-semibold',
            clasePuntuacion(cancion.puntuacion)
          ]">
            {{ formatearPuntuacion(cancion.puntuacion) }}
          </span>
        </div>

        <div v-if="album.canciones.length === 0" class="terminal-panel px-4 py-8 text-center text-sm text-neutral-400">
          Este álbum todavía no tiene canciones.
        </div>
      </div>

      <div class="terminal-panel hidden overflow-x-auto md:block">
        <table class="w-full min-w-[760px] table-fixed border-collapse text-left">
          <thead class="bg-black text-sm uppercase tracking-[0.08em] text-neutral-500">
            <tr>
              <th class="w-16 px-4 py-3 font-medium">#</th>
              <th class="w-[45%] px-4 py-3 font-medium">cancion</th>
              <th class="w-28 px-4 py-3 font-medium">duracion</th>
              <th class="w-32 px-4 py-3 font-medium">puntuacion</th>
              <th class="w-[25%] px-4 py-3 font-medium">nota</th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="cancion in album.canciones" :key="cancion.id"
              class="border-t border-neutral-900 hover:bg-neutral-900/80">
              <td class="px-4 py-3 text-sm text-neutral-500">
                {{ cancion.numeroPista }}
              </td>

              <td class="px-4 py-3 text-base font-medium text-white">
                {{ cancion.titulo }}
              </td>

              <td class="px-4 py-3 text-sm text-neutral-400">
                {{ formatearDuracion(cancion.duracionSegundos) }}
              </td>

              <td class="px-4 py-3">
                <span :class="[
                  'inline-flex min-w-14 justify-center border px-3 py-1 text-base font-semibold',
                  clasePuntuacion(cancion.puntuacion)
                ]">
                  {{ formatearPuntuacion(cancion.puntuacion) }}
                </span>
              </td>

              <td class="px-4 py-3 text-sm text-neutral-400">
                {{ cancion.nota || '-' }}
              </td>
            </tr>
          </tbody>
        </table>

        <div v-if="album.canciones.length === 0" class="px-4 py-8 text-center text-sm text-neutral-400">
          Este álbum todavía no tiene canciones.
        </div>
      </div>
    </div>
  </section>
</template>
