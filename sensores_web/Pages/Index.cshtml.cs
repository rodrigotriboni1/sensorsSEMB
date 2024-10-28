using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sensores_web.Models;
using sensores_web.Services;
using System.Net.Http;

namespace sensores_web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SensorApiService _sensorApiService;


        public IndexModel(SensorApiService sensorApiService)
        {
            _sensorApiService = sensorApiService;
        }
        public List<SensorData> SensorDataList { get; set; } = new();
        public async Task OnGetAsync()
        {
            SensorDataList = await _sensorApiService.GetSensorDataAsync();
        }
    }
}
