using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;

namespace FBikeInput
{
    public class FBikeMonitor
    {
        public float CurrentVolume { get; set; }

        public delegate void SampleProcessedEventHandler(object sender, float avgVolume);
        public event SampleProcessedEventHandler SampleProcessed;

        public delegate void OneRotationDetectedEventHandler(object sender, float avgVolume);
        public event OneRotationDetectedEventHandler OneRotationDetected;

        public void Monitor(int deviceId)
        {
            var waveIn = new WaveInEvent { DeviceNumber = deviceId };
            waveIn.DataAvailable += (sender, e) => ProcessSamples(CollectSamples(e));
            waveIn.StartRecording();
        }

        public void ProcessSamples(IReadOnlyCollection<float> samples)
        {
            CurrentVolume = samples.Sum() / samples.Count;
            SampleProcessed?.Invoke(this, CurrentVolume);

            if (CurrentVolume <= 0.0002f)
            {
                OneRotationDetected?.Invoke(this, CurrentVolume);
            }
        }

        private static List<float> CollectSamples(WaveInEventArgs e)
        {
            var samples = new List<float>();

            // interpret as 16 bit audio
            for (var index = 0; index < e.BytesRecorded; index += 2)
            {
                var sample = (short) ((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]);
                var sample32 = sample / 32768f; // to floating point
                sample32 = Math.Abs(sample32);
                samples.Add(sample32);
            }

            return samples;
        }
    }
}