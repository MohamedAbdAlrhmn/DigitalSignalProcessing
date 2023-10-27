using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {

            if (InputNumBits == 0)
            {
                InputNumBits = (int)Math.Log(InputLevel, 2);
            }
            if (InputLevel == 0)
            {
                InputLevel = (int)Math.Pow(2, InputNumBits);
            }

            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();





            float Delta = (InputSignal.Samples.Max() - InputSignal.Samples.Min()) / InputLevel;
            List<float> Midpoint = new List<float>();
            List<float> intervals = new List<float>();
            List<float> samplesWithMidpoint = new List<float>();
          


            intervals.Add(InputSignal.Samples.Min());
            for (int i = 0; i < InputLevel; i++)
            {   //i-1
                intervals.Add(intervals[i] + Delta);
            }

            for (int j = 0; j < InputLevel; j++)
            {
                Midpoint.Add((intervals[j] + intervals[j + 1]) / 2);
            }

            for (int m = 0; m < InputSignal.Samples.Count; m++)
            {
                for (int j = 0; j < InputLevel; j++)
                {
                    if (InputSignal.Samples[m] >= intervals[j] && InputSignal.Samples[m] <= intervals[j + 1] + 0.00001)
                    {

                        samplesWithMidpoint.Add(Midpoint[j]);
                        OutputEncodedSignal.Add(Convert.ToString(j, 2).PadLeft(InputNumBits, '0'));
                        OutputIntervalIndices.Add(j + 1);
                        break;

                    }
                }
            }
            OutputQuantizedSignal = new Signal(samplesWithMidpoint, true);


            List<float> sampleError = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                sampleError.Add(OutputQuantizedSignal.Samples[i] - InputSignal.Samples[i]);

            }
            OutputSamplesError = sampleError;

        }
    }
}
