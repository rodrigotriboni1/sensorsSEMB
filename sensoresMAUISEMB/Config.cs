using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using sensoresMAUISEMB.Models;

namespace sensoresMAUISEMB
{
    public static class Config
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://sensoresdataapi.azurewebsites.net/api/SensorData";

        public static async Task SendSensorDataAsync(SensorData sensorData)
        {
            try
            {
                var json = JsonSerializer.Serialize(sensorData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ApiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var returnedData = await response.Content.ReadFromJsonAsync<SensorData>();
                    if (returnedData != null)
                    {
                        sensorData.Id = returnedData.Id;
                        Console.WriteLine($"Data sent successfully! ID: {sensorData.Id}");
                    }
                    else
                    {
                        Console.WriteLine("Response was successful, but no ID returned.");
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}

