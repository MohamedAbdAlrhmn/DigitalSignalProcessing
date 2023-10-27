﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } // upsampling factor
        public int M { get; set; } // downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        /*
         * 1) Filtering the signal 
         * 
         * -  Check the Sampling Frequency
         * 
         * 2) Resampling the signal
         * 
         * 3) Removeing DC_Component
         * 
         * 4) Normalizeing the signal from -1 TO 1
         * 
         * 5) Calculate the DFT
        */
        public override void Run()
        {
            // Loading the signal
            Signal Input_Signal = LoadSignal(SignalPath);

            // Filtering the signal 
            FIR BandPass_Filter = new FIR();
            BandPass_Filter.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS;
            BandPass_Filter.InputTimeDomainSignal = Input_Signal;
            BandPass_Filter.InputStopBandAttenuation = 50;
            BandPass_Filter.InputTransitionBand = 500;
            BandPass_Filter.InputF1 = miniF;
            BandPass_Filter.InputF2 = maxF;
            BandPass_Filter.InputFS= Fs;
            BandPass_Filter.Run();
            Signal Filter_OutPut = BandPass_Filter.OutputYn;

            // Saving the signal
            SaveSignalTimeDomain(Filter_OutPut, @"F:\unversity\level4_subject\DSP\Lap1\Package and Task\fcisdsp-dsp.toolbox-78ddd969882b\Saved_package\text_signal1.ds");
             
            DC_Component Remove_DC_Component = new DC_Component();
            Sampling Resampling_Obj = new Sampling();

            // Check the Sampling Frequency
            if (newFs >= 2 * maxF)
            {
                // Resampling the signal
                Resampling_Obj.InputSignal = Filter_OutPut;
                Resampling_Obj.L = L;
                Resampling_Obj.M = M;
                Resampling_Obj.Run();
                Signal Resampling_OutPut = Resampling_Obj.OutputSignal;

                // Saving the signal
                SaveSignalTimeDomain(Resampling_OutPut, @"F:\unversity\level4_subject\DSP\Lap1\Package and Task\fcisdsp-dsp.toolbox-78ddd969882b\Saved_package\text_signal2.ds");
                
                Remove_DC_Component.InputSignal = Resampling_OutPut;
            }
            else 
            {
                Console.Write("newFs is not valid");
                Remove_DC_Component.InputSignal = Filter_OutPut;
            }

            // Removeing DC_Component
            Remove_DC_Component.Run();
            Signal DC_Component_OutPut = Remove_DC_Component.OutputSignal;

            // Saving the signal
            SaveSignalTimeDomain(DC_Component_OutPut, @"F:\unversity\level4_subject\DSP\Lap1\Package and Task\fcisdsp-dsp.toolbox-78ddd969882b\Saved_package\text_signal3.ds");

            // Normalizeing the signal from -1 TO 1
            Normalizer Normalization_Obj= new Normalizer();
            Normalization_Obj.InputSignal = DC_Component_OutPut;
            Normalization_Obj.InputMinRange = -1;
            Normalization_Obj.InputMaxRange = 1;
            Normalization_Obj.Run();
            Signal Normalization_OutPut = Normalization_Obj.OutputNormalizedSignal;

            // Saving the signal
            SaveSignalTimeDomain(Normalization_OutPut, @"F:\unversity\level4_subject\DSP\Lap1\Package and Task\fcisdsp-dsp.toolbox-78ddd969882b\Saved_package\text_signal4.ds");
            
            // Calculate the DFT
            DiscreteFourierTransform dft_obj = new DiscreteFourierTransform();
            dft_obj.InputTimeDomainSignal = Normalization_OutPut;
            dft_obj.InputSamplingFrequency = newFs;
            dft_obj.Run();

            // Saving the signal
            SaveSignalFrequencyDomain(dft_obj.OutputFreqDomainSignal, @"F:\unversity\level4_subject\DSP\Lap1\Package and Task\fcisdsp-dsp.toolbox-78ddd969882b\Saved_package\text_signal5.ds");

            // Output in Frequency Domain 
            OutputFreqDomainSignal = dft_obj.OutputFreqDomainSignal;
        }

        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }

        // Saving the signal in time domain
        public static void SaveSignalTimeDomain(Signal sig, string filePath)
        {
            StreamWriter streamSaver = new StreamWriter(filePath);

            streamSaver.WriteLine(0);// timedomain
            streamSaver.WriteLine(0);// non periodic
            streamSaver.WriteLine(sig.Samples.Count);// #samples

            for (int i = 0; i < sig.Samples.Count; i++)
            {
                streamSaver.Write(sig.SamplesIndices[i]);
                streamSaver.WriteLine(" " + sig.Samples[i]);
            }
            streamSaver.Flush();
            streamSaver.Close();
        }

        // Saving the signal in frequency domain
        public static void SaveSignalFrequencyDomain(Signal sig, string filePath)
        {
            StreamWriter streamSaver = new StreamWriter(filePath);

            streamSaver.WriteLine(1);
            streamSaver.WriteLine(0);
            streamSaver.WriteLine(sig.Frequencies.Count);

            for (int i = 0; i < sig.Frequencies.Count; i++)
            {
                streamSaver.Write(sig.Frequencies[i]);
                streamSaver.Write(" " + sig.FrequenciesAmplitudes[i]);
                streamSaver.WriteLine(" " + sig.FrequenciesPhaseShifts[i]);
            }

            streamSaver.Flush();
            streamSaver.Close();
        }
    }
}
