using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {     List<float>Amplitude_list=new List<float>(InputFreqDomainSignal.FrequenciesAmplitudes);  
              List<float> Phase_list=new List<float>(InputFreqDomainSignal.FrequenciesPhaseShifts);
              List<float> IDFT = new List<float>();
            for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                Complex X = 0;
                for (int j = 0; j < InputFreqDomainSignal.FrequenciesAmplitudes.Count; j++)
                {
                   double imagenry=  Amplitude_list[j] * (float)Math.Sin(Phase_list[j]);
                    double real = Amplitude_list[j] * (float)Math.Cos(Phase_list[j]);
                    Complex samples_x=new Complex(real, imagenry);
                    X += samples_x * Complex.Exp(Complex.ImaginaryOne * 2 * Math.PI * i * j / InputFreqDomainSignal.FrequenciesAmplitudes.Count);

                }
                   X/= InputFreqDomainSignal.FrequenciesAmplitudes.Count;
                   IDFT.Add((float)X.Real);

            }
            OutputTimeDomainSignal = new Signal(IDFT, true);
        }    
    }
}
