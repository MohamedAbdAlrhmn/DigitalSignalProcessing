using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            List<float> val = new List<float>();
            List<int> index = new List<int>();
            for (int i = InputSignal.Samples.Count-1; i >=0; i--)
            {
                val.Add(InputSignal.Samples[i]);
                index.Add(InputSignal.SamplesIndices[i]*-1);
            }
             
            OutputFoldedSignal = new Signal(val, index, !InputSignal.Periodic);
        }
    }
}
