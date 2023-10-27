using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            // Auto Correlation
            if (InputSignal2 == null)
            {
                List<float> AutoCorrelation = new List<float>();
                List<float> Signal = new List<float>();
                List<float> SignalCOPY = new List<float>();

                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    Signal.Add(InputSignal1.Samples[i]);
                    SignalCOPY.Add(InputSignal1.Samples[i]);
                }
                // Periodic
                if (InputSignal1.Periodic == true)
                {

                    for (int i = 0; i < SignalCOPY.Count; i++)
                    {
                        float sum = 0;
                        // First itteration
                        if (i == 0)
                        {
                            for (int j = 0; j < SignalCOPY.Count; j++)
                            {
                                sum += SignalCOPY[j] * SignalCOPY[j];
                            }
                        }
                        else
                        {
                            float E = SignalCOPY[0];
                            for (int j = 0; j < SignalCOPY.Count - 1; j++)
                            {
                                SignalCOPY[j] = SignalCOPY[j + 1];
                                sum += Signal[j] * SignalCOPY[j];
                            }
                            SignalCOPY[SignalCOPY.Count - 1] = E;
                            sum += SignalCOPY[SignalCOPY.Count - 1] * Signal[Signal.Count - 1];
                        }
                        AutoCorrelation.Add((float)sum / SignalCOPY.Count);
                    }
                }
                // Non Periodic
                else
                {
                    for (int i = 0; i < SignalCOPY.Count; i++)
                    {
                        float sum = 0;
                        // First itteration
                        if (i == 0)
                        {
                            for (int j = 0; j < SignalCOPY.Count; j++)
                            {
                                sum += SignalCOPY[j] * SignalCOPY[j];
                            }
                        }
                        else
                        {
                            for (int j = 0; j < SignalCOPY.Count - 1; j++)
                            {
                                SignalCOPY[j] = SignalCOPY[j + 1];
                                sum += Signal[j] * SignalCOPY[j];
                            }
                            SignalCOPY[SignalCOPY.Count - 1] = 0;
                        }
                        AutoCorrelation.Add(sum / SignalCOPY.Count);
                    }
                }

                // Normalization Sum
                float NormSum = 0;
                for (int i = 0; i < Signal.Count; i++)
                {
                    NormSum += Signal[i] * Signal[i];
                }
                NormSum /= Signal.Count;

                // Output
                OutputNonNormalizedCorrelation = AutoCorrelation;
                for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                {
                    OutputNormalizedCorrelation.Add(OutputNonNormalizedCorrelation[i] / NormSum);
                }
            }

            else // cross-correlation
            {
                List<float> CrossCorrelation = new List<float>();
                List<double> Signal_1 = new List<double>();
                List<double> Signal_2 = new List<double>();

                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    Signal_1.Add(InputSignal1.Samples[i]);
                }
                for (int i = 0; i < InputSignal2.Samples.Count; i++)
                {
                    Signal_2.Add(InputSignal2.Samples[i]);
                }


                double normalization_summation = 0, signal1_samples_summation = 0, signal2_samples_summation = 0;
                for (int i = 0; i < Signal_1.Count; i++)
                {
                    signal1_samples_summation += Signal_1[i] * Signal_1[i];
                    signal2_samples_summation += Signal_2[i] * Signal_2[i];
                }
                normalization_summation = signal1_samples_summation * signal2_samples_summation;
                normalization_summation = Math.Sqrt(normalization_summation);
                normalization_summation /= Signal_1.Count;

                if (InputSignal1.Periodic != true)
                {
                    for (int i = 0; i < Signal_2.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0) // so shift 
                        {
                            double first_element = 0;
                            for (int j = 0; j < Signal_2.Count - 1; j++)
                            {
                                Signal_2[j] = Signal_2[j + 1];
                                sum += Signal_2[j] * Signal_1[j];
                            }
                            Signal_2[Signal_2.Count - 1] = first_element;
                            sum += Signal_2[Signal_2.Count - 1] * Signal_1[Signal_1.Count - 1];
                        }
                        else
                        {
                            for (int j = 0; j < Signal_2.Count; j++)
                                sum += Signal_1[j] * Signal_2[j];
                        }
                        CrossCorrelation.Add((float)sum / Signal_2.Count);
                    }
                }

                else
                {
                    for (int i = 0; i < Signal_2.Count; i++)
                    {
                        double sum = 0;
                        if (i != 0) // so shift 
                        {
                            double first_element = Signal_2[0];
                            for (int j = 0; j < Signal_2.Count - 1; j++)
                            {
                                Signal_2[j] = Signal_2[j + 1];
                                sum += Signal_2[j] * Signal_1[j];
                            }
                            Signal_2[Signal_2.Count - 1] = first_element;
                            sum += Signal_2[Signal_2.Count - 1] * Signal_1[Signal_1.Count - 1];
                        }
                        else
                        {
                            for (int j = 0; j < Signal_2.Count; j++)
                                sum += Signal_1[j] * Signal_2[j];
                        }
                        CrossCorrelation.Add((float)sum / Signal_2.Count);
                    }
                }

                OutputNonNormalizedCorrelation = CrossCorrelation;

                for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
                    OutputNormalizedCorrelation.Add((float)(OutputNonNormalizedCorrelation[i] / normalization_summation));
            }
        }
    }
}