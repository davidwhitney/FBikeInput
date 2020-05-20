using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NAudio.Wave;

namespace FBikeInput
{
    public class Program
    {
        private static int _selectedDeviceId;

        public static void Main(string[] args)
        {
            if (WaveIn.DeviceCount == 0)
            {
                throw new Exception("Cannot detect microphone.");
            }

            for (var waveInDevice = 0; waveInDevice < WaveIn.DeviceCount; waveInDevice++)
            {
                var deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                Console.WriteLine("Device {0}: {1}, {2} channels", waveInDevice, deviceInfo.ProductName, deviceInfo.Channels);
            }

            if (WaveIn.DeviceCount > 1)
            {
                Console.Write("Enter the ID of the input device to listen on: ");
                var input = Console.ReadKey();
                int.TryParse(input.KeyChar.ToString(), out _selectedDeviceId);
                Console.WriteLine("");
            }

            Console.WriteLine("Listening");

            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSetting("DeviceId", _selectedDeviceId.ToString());
                    webBuilder.UseStartup<Startup>();
                }).Build().Run();

            Console.ReadKey();
        }
    }
}
