using System;
using NUnit.Framework;

namespace FBikeInput.Test.Unit
{
    [TestFixture]
    public class FBikeMonitorTests
    {
        [Test]
        public void ProcessSamples_AllSamplesContainNoise_EventNotRaised()
        {
            var monitor = new FBikeMonitor();
            monitor.OneRotationDetected += (sender, avgVolume) 
                => throw new Exception("Should not be raised");

            Assert.DoesNotThrow(() => monitor.ProcessSamples(new[] {1f, 1f, 1f, 1f}));
        }

        [Test]
        public void ProcessSamples_SampleAveragesToSilence_EventRaised()
        {
            var detectedVolume = float.MinValue;
            var monitor = new FBikeMonitor();
            monitor.OneRotationDetected += (sender, avgVolume) => detectedVolume = avgVolume;

            monitor.ProcessSamples(new[] { 0.0002f });

            Assert.That(detectedVolume, Is.EqualTo(0.0002f));
        }
    }
}
