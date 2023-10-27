using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }

        public override void Run()
        {
            List<float> avarage = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float sum = 0;
                for (int j = 0; j < InputWindowSize; j++)
                {
                    if (i >= InputWindowSize / 2 && i < InputSignal.Samples.Count - InputWindowSize / 2)
                    {
                        sum += InputSignal.Samples[i + j - InputWindowSize / 2];
                    }
                }
                if (i < InputWindowSize / 2 || i >= InputSignal.Samples.Count - InputWindowSize / 2)
                {
                    continue;
                }
                avarage.Add(sum / InputWindowSize);
            }
            OutputAverageSignal = new Signal(avarage, true);
        }
    }
}
