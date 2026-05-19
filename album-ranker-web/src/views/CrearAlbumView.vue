<script setup>
import { ref } from 'vue'
import { useRouter, RouterLink } from 'vue-router'
import { convertirDuracionASegundos } from '@/utils/duracionUtils'
import { createAlbum } from '../api/albumApi'

const router = useRouter()

const loading = ref(false)
const error = ref(null)

const album = ref({
  titulo: '',
  nombreArtista: '',
  anio: null,
  urlPortada: '',
  canciones: [
    {
      numeroPista: 1,
      titulo: '',
      duracionTexto: 0,
      puntuacion: null,
      nota: '',
    },
  ],
})

function agregarCancion() {
  album.value.canciones.push({
    numeroPista: album.value.canciones.length + 1,
    titulo: '',
    duracionTexto: '',
    puntuacion: null,
    nota: '',
  })
}

function eliminarCancion(index) {
  album.value.canciones.splice(index, 1)
  album.value.canciones.forEach((cancion, i) => {
    cancion.numeroPista = i + 1
  })
}



async function guardarAlbum() {
  loading.value = true
  error.value = null

  try {
    const payload = {
      titulo: album.value.titulo,
      nombreArtista: album.value.nombreArtista,
      anio: album.value.anio || null,
      urlPortada: album.value.urlPortada || null,
      canciones: album.value.canciones.map((cancion) => ({
        numeroPista: Number(cancion.numeroPista),
        titulo: cancion.titulo,
        duracionSegundos: convertirDuracionASegundos(cancion.duracionTexto),
        puntuacion:
          cancion.puntuacion === null || cancion.puntuacion === ''
            ? null
            : Number(cancion.puntuacion),
        nota: cancion.nota || null,
      })),
    }

    const creado = await createAlbum(payload)

    router.push(`/albumes/${creado.id}`)
  } catch (err) {
    error.value = err.response?.data || err.message || 'No se pudo crear el álbum.'
    console.error(err)
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <section>
    <RouterLink to="/albumes" class="terminal-link mb-4 inline-block">
      ../albumes
    </RouterLink>

    <div class="mb-5 flex items-center justify-between border-b border-neutral-800 pb-4">
      <div>
        <h2 class="terminal-heading">insert album</h2>
        <p class="mt-1 text-sm text-neutral-500">metadata + lista de canciones</p>
      </div>
    </div>

    <form class="space-y-5" @submit.prevent="guardarAlbum">
      <div class="terminal-panel p-4">
        <h3 class="mb-4 text-sm font-semibold uppercase tracking-[0.1em] text-white">datos.album</h3>

        <div class="grid grid-cols-1 gap-4 md:grid-cols-2">
          <div>
            <label class="terminal-label">titulo</label>
            <input v-model="album.titulo" type="text"
              class="terminal-input"
              required />
          </div>

          <div>
            <label class="terminal-label">artista</label>
            <input v-model="album.nombreArtista" type="text"
              class="terminal-input"
              required />
          </div>

          <div>
            <label class="terminal-label">anio</label>
            <input v-model="album.anio" type="number"
              class="terminal-input" />
          </div>

          <div>
            <label class="terminal-label">url portada</label>
            <input v-model="album.urlPortada" type="text"
              class="terminal-input" />
          </div>
        </div>
      </div>

      <div class="terminal-panel p-4">
        <div class="mb-4 flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
          <h3 class="text-sm font-semibold uppercase tracking-[0.1em] text-white">canciones</h3>

          <button type="button"
            class="terminal-btn"
            @click="agregarCancion">
            + cancion
          </button>
        </div>

        <div class="space-y-3 lg:hidden">
          <div v-for="(cancion, index) in album.canciones" :key="`mobile-${index}`"
            class="border border-neutral-900 bg-black/50 p-3">
            <div class="mb-3 flex items-center justify-between">
              <p class="text-sm font-semibold uppercase tracking-[0.08em] text-neutral-300">
                cancion {{ index + 1 }}
              </p>

              <button type="button" class="px-2 py-1 text-sm text-red-400 hover:bg-red-950/40"
                :disabled="album.canciones.length === 1" @click="eliminarCancion(index)">
                borrar
              </button>
            </div>

            <div class="grid grid-cols-2 gap-3">
              <div>
                <label class="terminal-label">#</label>
                <input v-model="cancion.numeroPista" type="number" class="terminal-input" />
              </div>

              <div>
                <label class="terminal-label">puntuacion</label>
                <input v-model="cancion.puntuacion" type="number" min="0" max="10" step="0.5"
                  class="terminal-input" />
              </div>

              <div class="col-span-2">
                <label class="terminal-label">titulo</label>
                <input v-model="cancion.titulo" type="text" class="terminal-input" />
              </div>

              <div>
                <label class="terminal-label">duracion</label>
                <input v-model="cancion.duracionTexto" type="number" class="terminal-input" placeholder="seg." />
              </div>

              <div>
                <label class="terminal-label">nota</label>
                <input v-model="cancion.nota" type="text" class="terminal-input" />
              </div>
            </div>
          </div>
        </div>

        <div class="hidden overflow-x-auto lg:block">
          <table class="w-full min-w-[900px] table-fixed border-collapse text-left">
            <thead class="bg-black text-sm uppercase tracking-[0.08em] text-neutral-500">
              <tr>
                <th class="w-16 px-3 py-3 font-medium">#</th>
                <th class="w-[32%] px-3 py-3 font-medium">titulo</th>
                <th class="w-32 px-3 py-3 font-medium">duracion</th>
                <th class="w-32 px-3 py-3 font-medium">puntuacion</th>
                <th class="px-3 py-3 font-medium">nota</th>
                <th class="w-24 px-3 py-3"></th>
              </tr>
            </thead>

            <tbody>
              <tr v-for="(cancion, index) in album.canciones" :key="index" class="border-t border-neutral-900">
                <td class="px-3 py-3">
                  <input v-model="cancion.numeroPista" type="number"
                    class="terminal-input" />
                </td>

                <td class="px-3 py-3">
                  <input v-model="cancion.titulo" type="text"
                    class="terminal-input" />
                </td>

                <td class="px-3 py-3">
                  <input v-model="cancion.duracionTexto" type="number"
                    class="terminal-input"
                    placeholder="seg." />
                </td>

                <td class="px-3 py-3">
                  <input v-model="cancion.puntuacion" type="number" min="0" max="10" step="0.5"
                    class="terminal-input" />
                </td>

                <td class="px-3 py-3">
                  <input v-model="cancion.nota" type="text"
                    class="terminal-input" />
                </td>

                <td class="px-3 py-3 text-right">
                  <button type="button" class="px-2 py-1 text-sm text-red-400 hover:bg-red-950/40"
                    :disabled="album.canciones.length === 1" @click="eliminarCancion(index)">
                    borrar
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <p v-if="error" class="border border-red-900 bg-red-950/40 px-3 py-2 text-sm text-red-300">
        {{ error }}
      </p>

      <div class="flex flex-col-reverse gap-2 sm:flex-row sm:justify-end">
        <RouterLink to="/albumes"
          class="terminal-btn">
          cancelar
        </RouterLink>

        <button type="submit"
          class="terminal-btn-primary"
          :disabled="loading">
          {{ loading ? 'guardando...' : 'guardar album' }}
        </button>
      </div>
    </form>
  </section>
</template>
