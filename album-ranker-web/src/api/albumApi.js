import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL
})

export async function getAlbumes() {
  const response = await api.get('/albumes')
  return response.data
}

export async function getAlbumById(id) {
  const response = await api.get(`/albumes/${id}`)
  return response.data
}

export async function createAlbum(album) {
  const response = await api.post('/albumes', album)
  return response.data
}

export async function addCancion(albumId, cancion) {
  const response = await api.post(`/albumes/${albumId}/canciones`, cancion)
  return response.data
}
