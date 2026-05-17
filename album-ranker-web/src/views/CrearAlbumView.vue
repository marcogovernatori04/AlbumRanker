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
    <RouterLink to="/albumes" class="mb-6 inline-block text-sm text-neutral-400 hover:text-white">
      ← Volver a álbumes
    </RouterLink>

    <div class="mb-6 flex items-center justify-between">
      <div>
        <h2 class="text-2xl font-bold text-white">Nuevo álbum</h2>
        <p class="mt-1 text-sm text-neutral-500">Cargá los datos generales y sus canciones.</p>
      </div>
    </div>

    <form class="space-y-6" @submit.prevent="guardarAlbum">
      <div class="rounded-2xl border border-neutral-800 bg-neutral-900 p-5">
        <h3 class="mb-4 text-lg font-semibold text-white">Datos del álbum</h3>

        <div class="grid grid-cols-1 gap-4 md:grid-cols-2">
          <div>
            <label class="mb-1 block text-sm text-neutral-400">Título</label>
            <input v-model="album.titulo" type="text"
              class="h-10 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-3 text-sm text-white outline-none focus:border-neutral-600"
              required />
          </div>

          <div>
            <label class="mb-1 block text-sm text-neutral-400">Artista</label>
            <input v-model="album.nombreArtista" type="text"
              class="h-10 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-3 text-sm text-white outline-none focus:border-neutral-600"
              required />
          </div>

          <div>
            <label class="mb-1 block text-sm text-neutral-400">Año</label>
            <input v-model="album.anio" type="number"
              class="h-10 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-3 text-sm text-white outline-none focus:border-neutral-600" />
          </div>

          <div>
            <label class="mb-1 block text-sm text-neutral-400">URL portada</label>
            <input v-model="album.urlPortada" type="text"
              class="h-10 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-3 text-sm text-white outline-none focus:border-neutral-600" />
          </div>
        </div>
      </div>

      <div class="rounded-2xl border border-neutral-800 bg-neutral-900 p-5">
        <div class="mb-4 flex items-center justify-between">
          <h3 class="text-lg font-semibold text-white">Canciones</h3>

          <button type="button"
            class="rounded-lg border border-neutral-700 px-3 py-2 text-sm text-neutral-200 hover:bg-neutral-800"
            @click="agregarCancion">
            Agregar canción
          </button>
        </div>

        <div class="overflow-x-auto">
          <table class="w-full table-fixed border-collapse text-left">
            <thead class="text-sm text-neutral-400">
              <tr>
                <th class="w-16 px-3 py-2 font-medium">#</th>
                <th class="w-[32%] px-3 py-2 font-medium">Título</th>
                <th class="w-32 px-3 py-2 font-medium">Duración</th>
                <th class="w-32 px-3 py-2 font-medium">Puntuación</th>
                <th class="px-3 py-2 font-medium">Nota</th>
                <th class="w-20 px-3 py-2"></th>
              </tr>
            </thead>

            <tbody>
              <tr v-for="(cancion, index) in album.canciones" :key="index" class="border-t border-neutral-800">
                <td class="px-3 py-2">
                  <input v-model="cancion.numeroPista" type="number"
                    class="h-9 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-2 text-sm text-white outline-none focus:border-neutral-600" />
                </td>

                <td class="px-3 py-2">
                  <input v-model="cancion.titulo" type="text"
                    class="h-9 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-2 text-sm text-white outline-none focus:border-neutral-600"
                    required />
                </td>

                <td class="px-3 py-2">
                  <input v-model="cancion.duracionTexto" type="number"
                    class="h-9 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-2 text-sm text-white outline-none focus:border-neutral-600"
                    placeholder="seg." />
                </td>

                <td class="px-3 py-2">
                  <input v-model="cancion.puntuacion" type="number" min="0" max="10" step="0.5"
                    class="h-9 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-2 text-sm text-white outline-none focus:border-neutral-600" />
                </td>

                <td class="px-3 py-2">
                  <input v-model="cancion.nota" type="text"
                    class="h-9 w-full rounded-lg border border-neutral-800 bg-neutral-950 px-2 text-sm text-white outline-none focus:border-neutral-600" />
                </td>

                <td class="px-3 py-2 text-right">
                  <button type="button" class="rounded-lg px-2 py-1 text-sm text-red-400 hover:bg-red-950/40"
                    :disabled="album.canciones.length === 1" @click="eliminarCancion(index)">
                    Borrar
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <p v-if="error" class="rounded-lg border border-red-900 bg-red-950/40 px-4 py-3 text-sm text-red-300">
        {{ error }}
      </p>

      <div class="flex justify-end gap-3">
        <RouterLink to="/albumes"
          class="inline-flex h-10 items-center rounded-lg border border-neutral-700 px-4 text-sm text-neutral-300 hover:bg-neutral-800">
          Cancelar
        </RouterLink>

        <button type="submit"
          class="inline-flex h-10 items-center rounded-lg bg-white px-4 text-sm font-semibold text-neutral-950 hover:bg-neutral-200 disabled:opacity-60"
          :disabled="loading">
          {{ loading ? 'Guardando...' : 'Guardar álbum' }}
        </button>
      </div>
    </form>
  </section>
</template>
