<script setup>
import { computed, onMounted, ref } from 'vue'
import { getAlbumes } from '@/api/albumApi'
import { formatearPuntuacion, clasePuntuacion } from '@/utils/scoreUtils'

const albumes = ref([])
const loading = ref(true)
const error = ref(null)
const busqueda = ref('')

const albumesFiltrados = computed(() => {
	const texto = busqueda.value.trim().toLowerCase()

	if (!texto) {
		return albumes.value
	}

	return albumes.value.filter(album =>
		album.titulo.toLowerCase().includes(texto) ||
		album.artista.toLowerCase().includes(texto) ||
		String(album.anio ?? '').includes(texto)
	)
})


onMounted(async () => {
	try {
		albumes.value = await getAlbumes()
	} catch (err) {
		error.value = 'No se pudieron cargar los álbumes.'
		console.error(err)
	} finally {
		loading.value = false
	}
})
</script>

<template>
	<section>
		<div class="mb-6 flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
			<div>
				<h2 class="text-2xl font-bold text-white">Mis álbumes</h2>
				<p class="mt-1 text-sm text-neutral-500">
					{{ albumesFiltrados.length }} álbumes
				</p>
			</div>

			<div class="flex w-full flex-col gap-3 sm:flex-row lg:w-auto">
				<input v-model="busqueda" type="text" placeholder="Buscar álbum, artista o año..."
					class="h-10 w-full rounded-lg border border-neutral-800 bg-neutral-900 px-3 text-sm text-white placeholder:text-neutral-600 outline-none focus:border-neutral-600 sm:w-80" />

				<RouterLink to="/albumes/create"
					class="inline-flex h-10 items-center justify-center rounded-lg bg-white px-4 text-sm font-semibold text-neutral-950 hover:bg-neutral-200">
					Nuevo álbum
				</RouterLink>
			</div>
		</div>

		<p v-if="loading" class="text-neutral-400">Cargando...</p>
		<p v-else-if="error" class="text-red-400">{{ error }}</p>
		<p v-else-if="albumesFiltrados.length === 0" class="text-neutral-400">No se encontraron álbumes.</p>

		<div v-else class="grid grid-cols-1 gap-4 md:grid-cols-2">
			<RouterLink v-for="album in albumesFiltrados" :key="album.id" :to="`/albumes/${album.id}`"
				class="flex items-center gap-4 rounded-2xl border border-neutral-800 bg-neutral-900 p-4 transition hover:-translate-y-1 hover:bg-neutral-800">
				<div
					class="flex h-16 w-16 shrink-0 items-center justify-center overflow-hidden rounded-xl bg-neutral-800 text-2xl font-bold">
					<img v-if="album.urlPortada" :src="album.urlPortada" :alt="album.titulo"
						class="h-full w-full object-cover" />
					<span v-else>{{ album.titulo.charAt(0) }}</span>
				</div>

				<div class="min-w-0 flex-1">
					<h3 class="truncate text-lg font-semibold">{{ album.titulo }}</h3>
					<p class="text-neutral-400">{{ album.artista }}</p>
					<p class="text-sm text-neutral-500">
						{{ album.anio ?? 'Sin año' }} · {{ album.cantidadCanciones }} canciones
					</p>
				</div>

				<div :class="[
					'flex h-10 w-10 items-center justify-center rounded-full text-l font-bold',
					clasePuntuacion(album.promedio)
				]">
					{{ formatearPuntuacion(album.promedio) }}
				</div>
			</RouterLink>
		</div>
	</section>
</template>