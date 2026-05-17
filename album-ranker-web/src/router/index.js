import AlbumDetalleView from '@/views/AlbumDetalleView.vue'
import AlbumesView from '@/views/AlbumesView.vue'
import CrearAlbumView from '@/views/CrearAlbumView.vue'
import ArtistaComparativaView from '@/views/ArtistaComparativaView.vue'
import ComparativasView from '@/views/ComparativasView.vue'
import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/albumes'
    },
    {
      path: '/albumes',
      component: AlbumesView
    },
    {
      path: '/albumes/create',
      component: CrearAlbumView
    },
    {
      path: '/comparativas',
      component: ComparativasView
    },
    {
      path: '/artistas/:id/comparativa',
      component: ArtistaComparativaView
    },
    {
      path: '/albumes/:id',
      component: AlbumDetalleView
    }
  ]
})

export default router
