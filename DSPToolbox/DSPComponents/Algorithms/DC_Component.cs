using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> out_DC = new List<float>();
            for (int i = 0; i<InputSignal.Samples.Count; i++) 
            {
                out_DC.Add(InputSignal.Samples[i]-InputSignal.Samples.Average());
            }
            OutputSignal = new Signal(out_DC, true);
        }
    }
}
