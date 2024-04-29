using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PinewoodTechnicalTask.Client
{
    public interface IApiClient
    {
        Task<TResponse> GetAsync<TResponse>(string uri);
        Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest data);
        Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest data);
        Task<TResponse> DeleteAsync<TResponse>(string uri);
    }

    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> GetAsync<TResponse>(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<TResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Optional: Deserialize using case-insensitive property names
            });

            return responseData;
        }


        public async Task<TResponse> PostAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            var jsonContent = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<TResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Optional: Deserialize using case-insensitive property names
            });

            return responseData;
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string uri, TRequest data)
        {
            var jsonContent = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(uri, content);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<TResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Optional: Deserialize using case-insensitive property names
            });

            return responseData;
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string uri)
        {
            var response = await _httpClient.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<TResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Optional: Deserialize using case-insensitive property names
            });

            return responseData;
        }

    }

}
