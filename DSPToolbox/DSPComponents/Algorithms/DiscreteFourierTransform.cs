using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
                
            List<float> hermonic = InputTimeDomainSignal.Samples;
            List<float> Amplitude = new List<float>();
            List<float> phase_shift = new List<float>();

            for (int i = 0; i < InputTimeDomainSignal.Samples.Count; i++)
            {
                Complex dft_complex = 0;

                for (int j = 0; j < InputTimeDomainSignal.Samples.Count; j++)
                {
                    dft_complex += InputTimeDomainSignal.Samples[j] * Complex.Pow(Math.E, -2 * i * j * Math.PI * Complex.ImaginaryOne / InputTimeDomainSignal.Samples.Count);
                }
                Amplitude.Add((float)dft_complex.Magnitude);
                phase_shift.Add((float)dft_complex.Phase);
            }

            OutputFreqDomainSignal = new Signal(hermonic, true);
            OutputFreqDomainSignal.FrequenciesAmplitudes = Amplitude;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = phase_shift;
            OutputFreqDomainSignal.Frequencies = new List<float>();
            for (int m = 0; m < InputTimeDomainSignal.Samples.Count; m++)
            {
                OutputFreqDomainSignal.Frequencies.Add((float)Math.Round(((2 * (float)Math.PI * InputSamplingFrequency) / InputTimeDomainSignal.Samples.Count) * m, 1));
            }
        }
    }
}