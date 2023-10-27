using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<float> Conv = new List<float>();
            List<int> index = new List<int>();
            //List<float> Indx = new List<float>();

            int val = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            int c = InputSignal1.SamplesIndices[0] + InputSignal2.SamplesIndices[0];
            for (int n = 0; n < val; n++)
            {
                float sum = 0;

                for (int k = 0; k <= n; k++)
                {
                    //Indx.Add(InputSignal1.SamplesIndices[n] + InputSignal2.SamplesIndices[n]);
                    if (k < InputSignal1.Samples.Count && (n - k) >= 0 && (n - k) < InputSignal2.Samples.Count)
                    {
                        sum += InputSignal1.Samples[k] * InputSignal2.Samples[n - k];
                    }
                }
                Conv.Add(sum);
                index.Add(c);
                c++;

            }
            OutputConvolvedSignal = new Signal(Conv, index,true);
        }
    }
}
