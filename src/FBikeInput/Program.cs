using System;
using NAudio.Wave;

namespace FBikeInput
{
    public class Program
    {
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

            var selectedDevice = 0;

            if (WaveIn.DeviceCount > 1)
            {
                Console.Write("Enter the ID of the input device to listen on: ");
                var input = Console.ReadKey();
                int.TryParse(input.KeyChar.ToString(), out selectedDevice);
                Console.WriteLine(""); }

            var monitor = new FBikeMonitor();
            monitor.OneRotationDetected += (e, average) =>
            {
                Console.WriteLine($"Rotate! {average}");
            };
            monitor.Monitor(selectedDevice);

            Console.WriteLine("Listening");
            Console.ReadKey();
        }
    }
}
