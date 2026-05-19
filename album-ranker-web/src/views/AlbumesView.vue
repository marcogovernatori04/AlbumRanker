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
		<div class="mb-5 flex flex-col gap-4 border-b border-neutral-800 pb-4 lg:flex-row lg:items-center lg:justify-between">
			<div>
				<h2 class="terminal-heading">~/albumes</h2>
				<p class="mt-1 text-sm text-neutral-500">
					{{ albumesFiltrados.length }} registros indexados
				</p>
			</div>

			<div class="flex w-full flex-col gap-2 sm:flex-row lg:w-auto">
				<input v-model="busqueda" type="text" placeholder="Buscar álbum, artista o año..."
					class="terminal-input sm:w-80" />

				<RouterLink to="/albumes/create"
					class="terminal-btn-primary">
					+ album
				</RouterLink>
			</div>
		</div>

		<p v-if="loading" class="text-neutral-400">Cargando...</p>
		<p v-else-if="error" class="text-red-400">{{ error }}</p>
		<p v-else-if="albumesFiltrados.length === 0" class="text-neutral-400">No se encontraron álbumes.</p>

		<div v-else class="terminal-panel overflow-hidden">
			<RouterLink v-for="album in albumesFiltrados" :key="album.id" :to="`/albumes/${album.id}`"
				class="grid grid-cols-[3rem_minmax(0,1fr)_5rem] items-center gap-4 border-b border-neutral-900 px-4 py-3 last:border-b-0 hover:bg-neutral-900/80 sm:grid-cols-[3.5rem_minmax(0,1fr)_9rem_5rem]">
				<div
					class="flex h-10 w-10 shrink-0 items-center justify-center overflow-hidden border border-neutral-800 bg-black text-sm font-bold text-lime-200">
					<img v-if="album.urlPortada" :src="album.urlPortada" :alt="album.titulo"
						class="h-full w-full object-cover" />
					<span v-else>{{ album.titulo.charAt(0) }}</span>
				</div>

				<div class="min-w-0 flex-1">
					<h3 class="truncate text-base font-semibold text-neutral-100">{{ album.titulo }}</h3>
					<p class="truncate text-sm text-neutral-500">{{ album.artista }}</p>
					<p class="mt-1 text-xs text-neutral-600 sm:hidden">
						{{ album.anio ?? 's/a' }} / {{ album.cantidadCanciones }} canciones
					</p>
				</div>

				<div class="hidden text-sm text-neutral-500 sm:block">
					<p>
						{{ album.anio ?? 's/a' }} / {{ album.cantidadCanciones }} canciones
					</p>
				</div>

				<div :class="[
					'flex h-8 min-w-16 items-center justify-center border px-3 text-base font-bold',
					clasePuntuacion(album.promedio)
				]">
					{{ formatearPuntuacion(album.promedio) }}
				</div>
			</RouterLink>
		</div>
	</section>
</template>
