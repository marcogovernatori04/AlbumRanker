import axios from "axios";

const api = axios.create({
  baseURL: 'https://localhost:7293/api'
})

export async function getArtistas() {
  const response = await api.get('/artistas')
  return response.data
}

export async function getArtistaConAlbumes(id) {
  const response = await api.get(`/artistas/${id}/albumes`)
  return response.data
}
