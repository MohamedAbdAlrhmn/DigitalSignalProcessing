using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }//sampling frequency
        public float? InputCutOffFrequency { get; set; }//fc low and highpass
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }//delta f
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public override void Run()
        {
            int N = 0;
            int Range;
            float Fc_Dash;
            float FC_1;
            float FC_2;
            List<float> Window_Function = new List<float>();
            List<float> H_of_N = new List<float>();
            List<int> Indicies = new List<int>();
            List<float> H_of_samples = new List<float>();
            
            // Low pass filter
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                Fc_Dash = (float)((InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS);

                if (InputStopBandAttenuation <= 21)
                {
                    N = (int)Math.Ceiling(0.9 / (InputTransitionBand / InputFS));
                    
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }
                    Range = (int)N / 2;

                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add(1);
                        if (n == 0)
                        {
                            H_of_N.Add(2 * Fc_Dash);
                        }
                        else
                        {
                            H_of_N.Add((float)(2 * Fc_Dash * Math.Sin(n * 2 * Math.PI * Fc_Dash) / (n * 2 * Math.PI * Fc_Dash)));
                        }
                        Indicies.Add(n);
                    }
                }
                else if (InputStopBandAttenuation <= 44 && InputStopBandAttenuation > 21)
                {
                    N = (int)Math.Ceiling(3.1 / (InputTransitionBand / InputFS));
                    
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }
                    Range = (int)N / 2;

                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add((float)(0.5 + 0.5 * Math.Cos((2 * Math.PI * n) / N)));
                        if (n == 0)
                        {
                            H_of_N.Add(2 * Fc_Dash);
                        }
                        else
                        {
                            H_of_N.Add((float)(2 * Fc_Dash * Math.Sin(n * 2 * Math.PI * Fc_Dash) / (n * 2 * Math.PI * Fc_Dash)));
                        }
                        Indicies.Add(n);
                    }
                }
                else if (InputStopBandAttenuation <= 53 && InputStopBandAttenuation > 44)
                {
                    N = (int)Math.Ceiling(3.3 / (InputTransitionBand / InputFS));
                    
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }
                    Range = (int)N / 2;

                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add((float)(0.54 + 0.46 * Math.Cos((2 * Math.PI * n) / N)));
                        if (n == 0)
                        {
                            H_of_N.Add(2 * Fc_Dash);
                        }
                        else
                        {
                            H_of_N.Add((float)(2 * Fc_Dash * Math.Sin(n * 2 * Math.PI * Fc_Dash) / (n * 2 * Math.PI * Fc_Dash)));
                        }
                        Indicies.Add(n);
                    }
                }
                else if (InputStopBandAttenuation <= 74 && InputStopBandAttenuation > 53)
                {
                    N = (int)Math.Ceiling(5.5 / (InputTransitionBand / InputFS));
                    
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }
                    Range = (int)N / 2;

                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add((float)(0.42 + 0.5 * Math.Cos((2 * Math.PI * n) / (N - 1)) + 0.08 * Math.Cos((4 * Math.PI * n) / (N - 1))));
                        if (n == 0)
                        {
                            H_of_N.Add(2 * Fc_Dash);
                        }
                        else
                        {
                            H_of_N.Add((float)(2 * Fc_Dash * Math.Sin(n * 2 * Math.PI * Fc_Dash) / (n * 2 * Math.PI * Fc_Dash)));
                        }
                        Indicies.Add(n);
                    }
                }
            }
            // High pass filter
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                Fc_Dash = (float)((InputCutOffFrequency - (InputTransitionBand / 2)) / InputFS);

                if (InputStopBandAttenuation <= 21)
                {
                    N = (int)Math.Ceiling(0.9 / (InputTransitionBand / InputFS));
                    
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }
                    Range = (int)(N  / 2);

                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add(1);

                        if (n == 0)
                        {
                            H_of_N.Add(1 - 2 * Fc_Dash);
                        }
                        else
                        {
                            H_of_N.Add((float)(-2 * Fc_Dash * Math.Sin(n * 2 * Math.PI * Fc_Dash) / (n * 2 * Math.PI * Fc_Dash)));
                        }

                        Indicies.Add(n);
                    }
                }
                else if (InputStopBandAttenuation <= 44 && InputStopBandAttenuation > 21)
                {
                    N = (int)Math.Ceiling(3.1 / (InputTransitionBand / InputFS));
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }

                    Range = (int)N / 2;
                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add((float)(0.5 + 0.5 * Math.Cos((2 * Math.PI * n) / N)));

                        if (n == 0)
                        {
                            H_of_N.Add(1 - 2 * Fc_Dash);
                        }
                        else
                        {
                            H_of_N.Add((float)(-2 * Fc_Dash * Math.Sin(n * 2 * Math.PI * Fc_Dash) / (n * 2 * Math.PI * Fc_Dash)));
                        }
                        Indicies.Add(n);
                    }
                }
                else if (InputStopBandAttenuation <= 53 && InputStopBandAttenuation > 44)
                {
                    N = (int)Math.Ceiling(3.3 / (InputTransitionBand / InputFS));
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }

                    Range = (int)N / 2;
                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add((float)(0.54 + 0.46 * Math.Cos((2 * Math.PI * n) / N)));
                        if (n == 0)
                        {
                            H_of_N.Add(1 - 2 * Fc_Dash);
                        }
                        else
                        {
                            H_of_N.Add((float)(-2 * Fc_Dash * Math.Sin(n * 2 * Math.PI * Fc_Dash) / (n * 2 * Math.PI * Fc_Dash)));
                        }
                        Indicies.Add(n);
                    }
                }
                else if (InputStopBandAttenuation <= 74 && InputStopBandAttenuation > 53)
                {
                    N = (int)Math.Ceiling(5.5 / (InputTransitionBand / InputFS));
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }

                    Range = (int)N / 2;
                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add((float)(0.42 + 0.5 * Math.Cos((2 * Math.PI * n) / (N - 1)) + 0.08 * Math.Cos((4 * Math.PI * n) / (N - 1))));
                        if (n == 0)
                        {
                            H_of_N.Add(1 - 2 * Fc_Dash);
                        }
                        else
                        {
                            H_of_N.Add((float)(-2 * Fc_Dash * Math.Sin(n * 2 * Math.PI * Fc_Dash) / (n * 2 * Math.PI * Fc_Dash)));
                        }

                        Indicies.Add(n);
                    }
                }
            }
            // Band pass filter
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                FC_1=(float)((InputF1-(InputTransitionBand / 2)) / InputFS);
                FC_2=(float)((InputF2+(InputTransitionBand / 2)) / InputFS);

                if (InputStopBandAttenuation <= 21)
                {
                    N = (int)Math.Ceiling(0.9 / (InputTransitionBand / InputFS));
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }

                    Range = (int)N / 2;
                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add(1);
                        if (n == 0)
                        {
                            H_of_N.Add(2*(FC_2-FC_1));
                        }
                        else
                        {
                            H_of_N.Add((float)(2 * FC_2 * Math.Sin((n * 2 * Math.PI * FC_2)) / (n * 2 * Math.PI * FC_2) - 2 * FC_1 * Math.Sin((n * 2 * Math.PI * FC_1)) / (n * 2 * Math.PI * FC_1)));
                        }

                        Indicies.Add(n);
                    }
                }
                else if (InputStopBandAttenuation <= 44 && InputStopBandAttenuation > 21)
                {
                    N = (int)Math.Ceiling(3.1 / (InputTransitionBand / InputFS));

                    if (N % 2 == 0)
                    {
                        N += 1;
                    }

                    Range = (int)N / 2;
                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add((float)(0.5 + 0.5 * Math.Cos((2 * Math.PI * n) / N)));

                        if (n == 0)
                        {
                            H_of_N.Add(2 * (FC_2 - FC_1));
                        }
                        else
                        {
                            H_of_N.Add((float)(2 * FC_2 * Math.Sin((n * 2 * Math.PI * FC_2)) / (n * 2 * Math.PI * FC_2) - 2 * FC_1 * Math.Sin((n * 2 * Math.PI * FC_1)) / (n * 2 * Math.PI * FC_1)));
                        }
                        Indicies.Add(n);
                    }
                }
                else if (InputStopBandAttenuation <= 53 && InputStopBandAttenuation > 44)
                {
                    N = (int)Math.Ceiling(3.3 / (InputTransitionBand / InputFS));
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }

                    Range = (int)N / 2;
                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add((float)(0.54 + 0.46 * Math.Cos((2 * Math.PI * n) / N)));

                        if (n == 0)
                        {
                            H_of_N.Add(2 * (FC_2 - FC_1));
                        }
                        else
                        {
                            H_of_N.Add((float)(2 * FC_2 * Math.Sin((n * 2 * Math.PI * FC_2)) / (n * 2 * Math.PI * FC_2) - 2 * FC_1 * Math.Sin((n * 2 * Math.PI * FC_1)) / (n * 2 * Math.PI * FC_1)));
                        }
                        Indicies.Add(n);
                    }
                }
                else if (InputStopBandAttenuation <= 74 && InputStopBandAttenuation > 53)
                {
                    N = (int)Math.Ceiling(5.5 / (InputTransitionBand / InputFS));
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }

                    Range = (int)N / 2;
                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add((float)(0.42 + 0.5 * Math.Cos((2 * Math.PI * n) / (N - 1)) + 0.08 * Math.Cos((4 * Math.PI * n) / (N - 1))));

                        if (n == 0)
                        {
                            H_of_N.Add(2 * (FC_2 - FC_1));
                        }
                        else
                        {
                             H_of_N.Add((float)(2 * FC_2 * Math.Sin((n * 2 * Math.PI * FC_2)) / (n * 2 * Math.PI * FC_2) - 2 * FC_1 * Math.Sin((double)(n * 2 * Math.PI * FC_1)) / (n * 2 * Math.PI * FC_1)));
                        }
                        Indicies.Add(n);
                    }
                }
            }
            // Band reject filter
            else if (InputFilterType == FILTER_TYPES.BAND_STOP)
            {
                FC_1 = (float)((InputF1 + (InputTransitionBand / 2)) / InputFS);
                FC_2 = (float)((InputF2 - (InputTransitionBand / 2)) / InputFS);

                if (InputStopBandAttenuation <= 21)
                {
                    N = (int)Math.Ceiling(0.9 / (InputTransitionBand / InputFS));
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }

                    Range = (int)N / 2;
                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add(1);
                        if (n == 0)
                        {
                            H_of_N.Add(1-2 * (FC_2 - FC_1));
                        }
                        else
                        {
                            H_of_N.Add((float)(-2 * FC_2 * Math.Sin((n * 2 * Math.PI * FC_2)) / (n * 2 * Math.PI * FC_2) + 2 * FC_1 * Math.Sin((n * 2 * Math.PI * FC_1)) / (n * 2 * Math.PI * FC_1)));
                        }
                        Indicies.Add(n);
                    }
                }
                else if (InputStopBandAttenuation <= 44 && InputStopBandAttenuation > 21)
                {
                    N = (int)Math.Ceiling(3.1 / (InputTransitionBand / InputFS));
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }

                    Range = (int)N / 2;
                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add((float)(0.5 + 0.5 * Math.Cos((2 * Math.PI * n) / N)));
                        if (n == 0)
                        {
                            H_of_N.Add(1 - 2 * (FC_2 - FC_1));
                        }
                        else
                        {
                            H_of_N.Add((float)(-2 * FC_2 * Math.Sin((n * 2 * Math.PI * FC_2)) / (n * 2 * Math.PI * FC_2) + 2 * FC_1 * Math.Sin((n * 2 * Math.PI * FC_1)) / (n * 2 * Math.PI * FC_1)));
                        }
                        Indicies.Add(n);
                    }
                }
                else if (InputStopBandAttenuation <= 53 && InputStopBandAttenuation > 44)
                {
                    N = (int)Math.Ceiling(3.3 / (InputTransitionBand / InputFS));
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }

                    Range = (int)N / 2;
                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add((float)(0.54 + 0.46 * Math.Cos((2 * Math.PI * n) / N)));
                        if (n == 0)
                        {
                            H_of_N.Add(1 - 2 * (FC_2 - FC_1));
                        }
                        else
                        {
                            H_of_N.Add((float)(-2 * FC_2 * Math.Sin((n * 2 * Math.PI * FC_2)) / (n * 2 * Math.PI * FC_2) + 2 * FC_1 * Math.Sin((n * 2 * Math.PI * FC_1)) / (n * 2 * Math.PI * FC_1)));
                        }
                        Indicies.Add(n);
                    }
                }
                else if (InputStopBandAttenuation <= 74 && InputStopBandAttenuation > 53)
                {
                    N = (int)Math.Ceiling(5.5 / (InputTransitionBand / InputFS));
                    if (N % 2 == 0)
                    {
                        N += 1;
                    }

                    Range = (int)N / 2;
                    for (int n = -Range; n <= Range; n++)
                    {
                        Window_Function.Add((float)(0.42 + 0.5 * Math.Cos((2 * Math.PI * n) / (N - 1)) + 0.08 * Math.Cos((4 * Math.PI * n) / (N - 1))));

                        if (n == 0)
                        {
                            H_of_N.Add(1 - 2 * (FC_2 - FC_1));
                        }
                        else
                        {
                            H_of_N.Add((float)(-2 * FC_2 * Math.Sin((n * 2 * Math.PI * FC_2)) / (n * 2 * Math.PI * FC_2) + 2 * FC_1 * Math.Sin((n * 2 * Math.PI * FC_1)) / (n * 2 * Math.PI * FC_1)));
                        }
                        Indicies.Add(n);
                    }
                }
            }

            // h(n).w(n)
            for (int i = 0; i < N; i++)
            {
                H_of_samples.Add((float)(Window_Function[i] * H_of_N[i]));
            }

            // Input time * output_hn
            OutputHn = new Signal(H_of_samples, Indicies, false);
            DirectConvolution conv_obj = new DirectConvolution();
            conv_obj.InputSignal1 = OutputHn;
            conv_obj.InputSignal2 = InputTimeDomainSignal;
            conv_obj.Run();
            OutputYn = conv_obj.OutputConvolvedSignal;
        }
    }
}
