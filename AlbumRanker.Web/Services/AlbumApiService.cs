using AlbumRanker.Web.DTOs.Albums;

namespace AlbumRanker.Web.Services
{
    public class AlbumApiService
    {
        private readonly HttpClient _httpClient;

        public AlbumApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AlbumListadoDTO>> GetAll()
        { 
            return await _httpClient.GetFromJsonAsync<List<AlbumListadoDTO>>("api/albums") 
                ?? new List<AlbumListadoDTO>();
        }

        public async Task<AlbumDetalleDTO?> GetById(int id)
        {
            return await _httpClient.GetFromJsonAsync<AlbumDetalleDTO>($"api/albums/{id}");
        }
    }
}
