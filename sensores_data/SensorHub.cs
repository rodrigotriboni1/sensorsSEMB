using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace sensores_data

{
    public class SensorHub : Hub
    {

        public async Task SendSensorData(SensorData data)
        {
  
            await Clients.All.SendAsync("ReceiveSensorData", data);
        }
    }
}
