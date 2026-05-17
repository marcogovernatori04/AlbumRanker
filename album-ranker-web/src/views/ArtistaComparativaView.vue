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
    <RouterLink to="/comparativas" class="mb-6 inline-block text-sm text-neutral-400 hover:text-white">
      ← Volver a comparativas
    </RouterLink>

    <p v-if="loading" class="text-neutral-400">Cargando...</p>

    <p v-else-if="error" class="text-red-400">
      {{ error }}
    </p>

    <div v-else>
      <div class="mb-6">
        <h2 class="text-2xl font-bold text-white">
          {{ artista?.nombre }}
        </h2>

        <p class="mt-1 text-sm text-neutral-500">
          Comparativa de puntuaciones por álbum
        </p>
      </div>

      <div v-if="albumes.length === 0"
        class="rounded-2xl border border-neutral-800 bg-neutral-900 p-6 text-neutral-400">
        Este artista no tiene álbumes cargados.
      </div>

      <div v-else class="overflow-x-auto rounded-2xl border border-neutral-800 bg-neutral-900">
        <table class="min-w-full border-collapse text-center">
          <thead>
            <tr class="border-b border-neutral-800 bg-neutral-950">
              <th
                class="sticky left-0 z-10 w-24 bg-neutral-950 px-4 py-4 text-left text-sm font-medium text-neutral-400">
                Track
              </th>

              <th v-for="album in albumes" :key="album.id" class="w-24 min-w-24 px-2 py-3 align-top">
                <div class="flex flex-col items-center gap-2">
                  <div class="h-16 w-16 overflow-hidden rounded-lg bg-neutral-800"
                    :title="`${album.titulo} (${album.anio ?? 'Sin año'})`">
                    <img v-if="album.urlPortada" :src="album.urlPortada" :alt="album.titulo"
                      class="h-full w-full object-cover" />

                    <div v-else
                      class="flex h-full w-full items-center justify-center text-2xl font-bold text-neutral-500">
                      {{ album.titulo.charAt(0) }}
                    </div>
                  </div>

                  <p class="text-[11px] text-neutral-500">
                    {{ album.anio ?? '' }}
                  </p>
                </div>
              </th>
            </tr>
          </thead>

          <tbody>
            <tr v-for="index in maxCanciones" :key="index" class="border-b border-neutral-800 hover:bg-neutral-800/40">
              <td class="sticky left-0 z-10 bg-neutral-900 px-4 py-3 text-left text-sm text-neutral-500">
                {{ index }}
              </td>

              <td v-for="album in albumes" :key="`${album.id}-${index}`" class="px-4 py-3">
                <template v-if="getCancionPorIndice(album, index - 1)">
                  <div class="flex justify-center">
                    <span :title="getCancionPorIndice(album, index - 1).titulo" :class="[
                      'inline-flex min-w-12 justify-center rounded-full px-3 py-1 text-sm font-semibold',
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
            <tr class="bg-neutral-950">
              <td class="sticky left-0 z-10 bg-neutral-950 px-4 py-4 text-left text-sm font-semibold text-neutral-300">
                Promedio
              </td>

              <td v-for="album in albumes" :key="`promedio-${album.id}`" class="px-4 py-4">
                <div class="flex justify-center">
                  <span :class="[
                    'inline-flex min-w-14 justify-center rounded-full px-3 py-1 text-sm font-bold',
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
