<script setup>
import { ref } from 'vue'
import { useRouter, RouterLink } from 'vue-router'
import { convertirDuracionASegundos } from '@/utils/duracionUtils'
import { createAlbum } from '../api/albumApi'

const router = useRouter()

const loading = ref(false)
const error = ref(null)
const cantidadCancionesAAgregar = ref(1)

const album = ref({
  titulo: '',
  nombreArtista: '',
  anio: null,
  urlPortada: '',
  canciones: [],
})

function crearCancionVacia(numeroPista) {
  return {
    numeroPista,
    titulo: '',
    duracionTexto: '',
    puntuacion: null,
    nota: '',
  }
}

function agregarCanciones(cantidad = 1) {
  const total = Math.max(1, Math.min(Number(cantidad) || 1, 99))

  for (let i = 0; i < total; i += 1) {
    album.value.canciones.push(crearCancionVacia(album.value.canciones.length + 1))
  }

  cantidadCancionesAAgregar.value = 1
}

function agregarCancion() {
  agregarCanciones(1)
}

function eliminarCancion(index) {
  album.value.canciones.splice(index, 1)
  album.value.canciones.forEach((cancion, i) => {
    cancion.numeroPista = i + 1
  })
}

function formatearDuracionInput(cancion) {
  const valor = String(cancion.duracionTexto ?? '').trim()

  if (!valor) {
    cancion.duracionTexto = ''
    return
  }

  if (valor.includes(':')) {
    const [minutos = '', segundos = ''] = valor.split(':')
    cancion.duracionTexto = `${Number(minutos) || 0}:${segundos.padStart(2, '0').slice(0, 2)}`
    return
  }

  const soloNumeros = valor.replace(/\D/g, '')

  if (!soloNumeros) {
    cancion.duracionTexto = ''
    return
  }

  if (soloNumeros.length <= 2) {
    cancion.duracionTexto = `0:${soloNumeros.padStart(2, '0')}`
    return
  }

  const minutos = soloNumeros.slice(0, -2)
  const segundos = soloNumeros.slice(-2)
  cancion.duracionTexto = `${Number(minutos)}:${segundos}`
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
            <label class="terminal-label">año</label>
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
        <div class="mb-4 flex items-center justify-between">
          <h3 class="text-sm font-semibold uppercase tracking-[0.1em] text-white">canciones</h3>
        </div>

        <div class="space-y-3 lg:hidden">
          <div v-if="album.canciones.length === 0" class="border border-neutral-900 bg-black/50 p-4 text-sm text-neutral-500">
            Todavía no hay filas. Indicá una cantidad abajo y agregá canciones.
          </div>

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

            <div class="grid grid-cols-[5rem_minmax(0,1fr)] gap-3">
              <div>
                <label class="terminal-label">#</label>
                <div class="flex h-10 items-center border border-neutral-800 bg-black px-3 text-sm text-neutral-300">
                  {{ cancion.numeroPista }}
                </div>
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
                <input v-model="cancion.duracionTexto" type="text" inputmode="numeric" pattern="[0-9]*:?[0-9]{0,2}"
                  class="terminal-input" placeholder="3:45" @blur="formatearDuracionInput(cancion)" />
              </div>

              <div>
                <label class="terminal-label">nota</label>
                <input v-model="cancion.nota" type="text" class="terminal-input" />
              </div>
            </div>
          </div>
        </div>

        <div v-if="album.canciones.length === 0" class="hidden border border-neutral-900 bg-black/50 p-4 text-sm text-neutral-500 lg:block">
          Todavía no hay filas. Indicá una cantidad abajo y agregá canciones.
        </div>

        <div v-else class="hidden overflow-x-auto lg:block">
          <table class="w-full min-w-[900px] table-fixed border-collapse text-left">
            <thead class="bg-black text-sm uppercase tracking-[0.08em] text-neutral-500">
              <tr>
                <th class="w-24 px-3 py-3 font-medium">#</th>
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
                  <div class="flex h-10 items-center border border-neutral-800 bg-black px-3 text-sm text-neutral-300">
                    {{ cancion.numeroPista }}
                  </div>
                </td>

                <td class="px-3 py-3">
                  <input v-model="cancion.titulo" type="text"
                    class="terminal-input" />
                </td>

                <td class="px-3 py-3">
                  <input v-model="cancion.duracionTexto" type="text" inputmode="numeric" pattern="[0-9]*:?[0-9]{0,2}"
                    class="terminal-input"
                    placeholder="3:45" @blur="formatearDuracionInput(cancion)" />
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

        <div class="mt-4 flex flex-col gap-2 border-t border-neutral-900 pt-4 sm:flex-row sm:items-end sm:justify-end">
          <div class="w-full sm:w-24">
            <label class="terminal-label">cantidad</label>
            <input v-model="cantidadCancionesAAgregar" type="number" min="1" max="99" class="terminal-input" />
          </div>

          <button type="button"
            class="terminal-btn-primary"
            @click="agregarCanciones(cantidadCancionesAAgregar)">
            + cancion
          </button>
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
