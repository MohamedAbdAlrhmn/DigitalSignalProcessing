using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
           DirectCorrelation Correlation=new DirectCorrelation();
            Correlation.InputSignal1 = InputSignal1;
            Correlation.InputSignal2 = InputSignal2;
            Correlation.Run();
            List<float> output_correlation = Correlation.OutputNormalizedCorrelation;
            float Max_correlation = float.MinValue;
            int index_of_maxCorrelation=0;
            for (int i = 0; i < output_correlation.Count; i++)
            {
                if (Math.Abs(output_correlation[i]) > Max_correlation) 
                {
                    Max_correlation = Math.Abs(output_correlation[i]);
                    index_of_maxCorrelation = i;
                }
            }
            OutputTimeDelay = index_of_maxCorrelation * InputSamplingPeriod;

        }
    }
}
