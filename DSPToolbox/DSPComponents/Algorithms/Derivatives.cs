using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> first_derv = new List<float>();
            List<float> second_derv = new List<float>();
            
            first_derv.Add(InputSignal.Samples[0]);
            second_derv.Add(InputSignal.Samples[1] - 2 * InputSignal.Samples[0]);

            for (int i = 1; i < InputSignal.Samples.Count - 1; i++)
            {
                first_derv.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]);
            }

            for (int j=1;j<InputSignal.Samples.Count-2; j++) 
            { 
                second_derv.Add(InputSignal.Samples[j + 1] - 2 * (InputSignal.Samples[j]) + InputSignal.Samples[j - 1]);
            }

            FirstDerivative = new Signal(first_derv, true);
            SecondDerivative = new Signal(second_derv, true);

        }
    }
}
