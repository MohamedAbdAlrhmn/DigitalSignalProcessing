﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            // Low pass filter
            FIR Low_Filter = new FIR();
            Low_Filter.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            Low_Filter.InputFS = 8000;
            Low_Filter.InputStopBandAttenuation = 50;
            Low_Filter.InputCutOffFrequency = 1500;
            Low_Filter.InputTransitionBand = 500;

            List<float> Sampled_Signal = new List<float>();
            List<int> Sampled_Signal_Indices = new List<int>();

            List<float> Sampled_Signal_Second = new List<float>();
            List<int> Sampled_Signal_Indices_Second = new List<int>();

            // Down Sampling
            if (L == 0 && M != 0)
            {
                Low_Filter.InputTimeDomainSignal = InputSignal;
                Low_Filter.Run();

                int Indx = Low_Filter.OutputYn.SamplesIndices[0];
                int Skip_Val = 0;

                for (int i = 0; i < Low_Filter.OutputYn.Samples.Count; i += M)
                {
                    Sampled_Signal.Add(Low_Filter.OutputYn.Samples[Skip_Val]);
                    Sampled_Signal_Indices.Add(Indx);
                    Indx++;
                    Skip_Val += M;
                }
                OutputSignal = new Signal(Sampled_Signal, Sampled_Signal_Indices, false);
            }

            // Up Sampling
            else if (L != 0 && M == 0)
            {
                int Indx = InputSignal.SamplesIndices[0];
                for (int Outer = 0; Outer < InputSignal.Samples.Count; Outer++)
                {
                    Sampled_Signal.Add(InputSignal.Samples[Outer]);
                    Sampled_Signal_Indices.Add(Indx);
                    Indx++;

                    if (Outer < InputSignal.Samples.Count - 1)
                    {
                        for (int Inner = 0; Inner < L - 1; Inner++)
                        {
                            Sampled_Signal.Add(0);
                            Sampled_Signal_Indices.Add(Indx);
                            Indx++;
                        }
                    }
                }
                Signal Result = new Signal(Sampled_Signal, Sampled_Signal_Indices, false);
                Low_Filter.InputTimeDomainSignal = Result;
                Low_Filter.Run();
                OutputSignal = Low_Filter.OutputYn;
            }

            // Sampling by fraction
            else if (L != 0 && M != 0)
            {
                // Up Sampling
                int Indx = InputSignal.SamplesIndices[0];
                for (int Outer = 0; Outer < InputSignal.Samples.Count; Outer++)
                {
                    Sampled_Signal.Add(InputSignal.Samples[Outer]);
                    Sampled_Signal_Indices.Add(Indx);
                    Indx++;

                    if (Outer < InputSignal.Samples.Count - 1)
                    {
                        for (int Inner = 0; Inner < L - 1; Inner++)
                        {
                            Sampled_Signal.Add(0);
                            Sampled_Signal_Indices.Add(Indx);
                            Indx++;
                        }
                    }
                }
                Signal Result = new Signal(Sampled_Signal, Sampled_Signal_Indices, false);

                /////////////////////////////
                /// Filter the signal
                Low_Filter.InputTimeDomainSignal = Result;
                Low_Filter.Run();
                Signal Up_Result = Low_Filter.OutputYn;
                /////////////////////////////
                
                // Down Sampling
                int Indx_2 = Up_Result.SamplesIndices[0];
                int Skip_Val = 0;

                for (int i = 0; i < Up_Result.Samples.Count; i += M)
                {
                    Sampled_Signal_Second.Add(Up_Result.Samples[Skip_Val]);
                    Sampled_Signal_Indices_Second.Add(Indx_2);
                    Indx_2++;
                    Skip_Val += M;
                }
                OutputSignal = new Signal(Sampled_Signal_Second, Sampled_Signal_Indices_Second, false);
            }
            else
            {
                Console.Write("ERROR!");
            }
        }
    }
}