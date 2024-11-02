using sensores_web.Models;

namespace sensores_web.Services
{
    public class SensorApiService
    {
        private readonly HttpClient _httpClient;

        public SensorApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // Configure base address if needed
            _httpClient.BaseAddress = new Uri("http://192.168.15.47:5202/api/"); // Replace with your API URL
        }

        public async Task<List<SensorData>> GetSensorDataAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("sensordata");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<SensorData>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return new List<SensorData>(); // Or throw the exception
            }
        }
    }
}
