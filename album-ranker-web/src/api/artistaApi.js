import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
})

export async function getArtistas() {
  const response = await api.get('/artistas')
  return response.data
}

export async function getArtistaConAlbumes(id) {
  const response = await api.get(`/artistas/${id}/albumes`)
  return response.data
}
