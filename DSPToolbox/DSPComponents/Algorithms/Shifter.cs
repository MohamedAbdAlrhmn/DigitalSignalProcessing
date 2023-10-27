using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            int s = 1;
            List<int> indecies = new List<int>();

            List<float> Values = new List<float>();
           
            for (int i = 0; i < InputSignal.Samples.Count; i++) 
            {
                if (InputSignal.Periodic == true)
                {
                    
                    indecies.Add(InputSignal.SamplesIndices[i] + ShiftingValue);

                }
                else 
                {
                    indecies.Add(InputSignal.SamplesIndices[i] - ShiftingValue);
                }
                
                Values.Add(InputSignal.Samples[i]);
            
            }
            OutputShiftedSignal = new Signal(Values, indecies, true);
        }
    }
}
