using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace FBikeInput
{
    public class FBikeHub : Hub
    {
        public async Task RotationDetected(float avgVolume)
        {
            await Clients?.All?.SendAsync(nameof(RotationDetected), avgVolume);
        }
    }
}