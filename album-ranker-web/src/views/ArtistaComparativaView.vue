<script setup>
import { computed, onMounted, ref } from 'vue'
import { useRoute, RouterLink } from 'vue-router'
import { getArtistaConAlbumes } from '../api/artistaApi'
import { getAlbumById } from '../api/albumApi'
import { formatearPuntuacion, clasePuntuacion } from '@/utils/scoreUtils'

const route = useRoute()

const artista = ref(null)
const albumes = ref([])
const loading = ref(true)
const error = ref(null)

onMounted(async () => {
  try {
    const artistaData = await getArtistaConAlbumes(route.params.id)
    console.log('Artista:', artistaData)
    artista.value = artistaData

    const albumesArtista = artistaData.albumes ?? []

    const detalles = await Promise.all(
      albumesArtista.map(album => getAlbumById(album.id))
    )

    console.log('Detalles:', detalles)

    albumes.value = detalles
      .filter(album => album !== null && album !== undefined)
      .map(album => ({
        ...album,
        canciones: [...(album.canciones ?? [])].sort((a, b) => a.numeroPista - b.numeroPista)
      }))

    console.log('Álbumes:', albumes.value)
  } catch (err) {
    error.value = 'No se pudo cargar la comparación del artista.'
    console.error(err)
  } finally {
    loading.value = false
  }
})

const maxCanciones = computed(() => {
  if (albumes.value.length === 0) return 0

  return Math.max(...albumes.value.map(album => album.canciones.length))
})

function getCancionPorIndice(album, index) {
  return album.canciones[index] ?? null
}
</script>

<template>
  <section>
    <RouterLink to="/comparativas" class="terminal-link mb-4 inline-block">
      ../comparar
    </RouterLink>

    <p v-if="loading" class="text-neutral-400">Cargando...</p>

    <p v-else-if="error" class="text-red-400">
      {{ error }}
    </p>

    <div v-else>
      <div class="mb-5 border-b border-neutral-800 pb-4">
        <h2 class="terminal-heading">
          {{ artista?.nombre }}
        </h2>

        <p class="mt-1 text-sm text-neutral-500">
          matriz de puntuaciones por album
        </p>
      </div>

      <div v-if="albumes.length === 0"
        class="terminal-panel p-5 text-sm text-neutral-400">
        Este artista no tiene álbumes cargados.
      </div>

      <div v-else class="terminal-panel overflow-x-auto [scrollbar-width:thin]">
        <table class="min-w-max border-collapse text-center">
          <thead>
            <tr class="border-b border-neutral-800 bg-black">
              <th
                class="sticky left-0 z-10 w-20 bg-black px-3 py-3 text-left text-sm font-medium uppercase tracking-[0.08em] text-neutral-500">
                cancion
              </th>

              <th v-for="album in albumes" :key="album.id" class="w-20 min-w-20 px-1.5 py-3 align-top">
                <div class="flex flex-col items-center gap-1">
                  <div class="h-15 w-15 overflow-hidden border border-neutral-800 bg-black"
                    :title="`${album.titulo} (${album.anio ?? 'Sin año'})`">
                    <img v-if="album.urlPortada" :src="album.urlPortada" :alt="album.titulo"
                      class="h-full w-full object-cover" />

                    <div v-else
                      class="flex h-full w-full items-center justify-center text-base font-bold text-lime-200">
                      {{ album.titulo.charAt(0) }}
                    </div>
                  </div>

                  <p class="text-xs text-neutral-500">
                    {{ album.anio ?? '' }}
                  </p>
                </div>
              </th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="index in maxCanciones" :key="index" class="border-b border-neutral-900 hover:bg-neutral-900/80">
              <td class="sticky left-0 z-10 bg-neutral-950 px-3 py-3 text-left text-sm text-neutral-500">
                {{ index }}
              </td>

              <td v-for="album in albumes" :key="`${album.id}-${index}`" class="px-1 py-3">
                <template v-if="getCancionPorIndice(album, index - 1)">
                  <div class="flex justify-center">
                    <span :title="getCancionPorIndice(album, index - 1).titulo" :class="[
                      'inline-flex h-8 w-14 items-center justify-center border text-base font-semibold',
                      clasePuntuacion(getCancionPorIndice(album, index - 1).puntuacion)
                    ]">
                      {{ formatearPuntuacion(getCancionPorIndice(album, index - 1).puntuacion) }}
                    </span>
                  </div>
                </template>

                <span v-else class="text-neutral-700">—</span>
              </td>
            </tr>
          </tbody>

          <tfoot>
            <tr class="bg-black">
              <td class="sticky left-0 z-10 bg-black px-3 py-3 text-left text-sm font-semibold uppercase tracking-[0.08em] text-neutral-300">
                promedio
              </td>

              <td v-for="album in albumes" :key="`promedio-${album.id}`" class="px-1.5 py-3">
                <div class="flex justify-center">
                  <span :class="[
                    'inline-flex h-8 w-14 items-center justify-center border text-base font-bold',
                    clasePuntuacion(album.promedio)
                  ]">
                    {{ formatearPuntuacion(album.promedio) }}
                  </span>
                </div>
              </td>
            </tr>
          </tfoot>
        </table>
      </div>
    </div>
  </section>
</template>
