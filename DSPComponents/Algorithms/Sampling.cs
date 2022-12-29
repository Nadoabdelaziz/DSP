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
            // L upSampling factor
            // M DownSampling factor

            // no sampling
            if (M == 0 && L == 0)
                {
                    Console.WriteLine("Error......");

                }
            // if not null => call up sampling function
                if (M == 0 && L != 0)
                {
                    Signal upsampledSignal = interpolateSignal();
                    filterSignal(upsampledSignal);
                }
                // if not null => call Down sampling function

                if (M != 0 && L == 0)
                {
                    filterSignal(InputSignal);
                    decimateSignal();
                }

                if (M != 0 && L != 0)
                {
                    Signal upsampledSignal = interpolateSignal();
                    filterSignal(upsampledSignal);
                    decimateSignal();
                }



            }
            private void filterSignal(Signal Input)
            {
            // call Fir function
                FIR FIR = new FIR();
                FIR.InputFilterType = FILTER_TYPES.LOW;
                FIR.InputFS = 8000;
                FIR.InputStopBandAttenuation = 50;
                FIR.InputCutOffFrequency = 1500;
                FIR.InputTransitionBand = 500;
                FIR.InputTimeDomainSignal = Input;
                FIR.Run();
                FIR.OutputYn.Samples.RemoveAt(FIR.OutputYn.Samples.Count() - 1);
                OutputSignal = FIR.OutputYn;
            }
            private Signal interpolateSignal()
            {
                Signal upsampledSignal = new Signal(new List<float>(), new List<int>(), InputSignal.Periodic);
                int index = -1;
                for (int ctr = 0; ctr < InputSignal.Samples.Count(); ctr++)
                {
                    index += 1;
                    upsampledSignal.SamplesIndices.Add(index);
                    upsampledSignal.Samples.Add(InputSignal.Samples[ctr]);
                    for (int i = 0; i < L - 1; i++)
                    {
                        index += 1;
                        upsampledSignal.SamplesIndices.Add(index);
                        
                        upsampledSignal.Samples.Add(0);
                    }
                }
                return upsampledSignal;
            }
            private void decimateSignal()
            {
                List<float> downsampled = new List<float>();
                for (int i = 0; i < OutputSignal.Samples.Count(); i += M)
                {
                    downsampled.Add(OutputSignal.Samples[i]);
                }
                OutputSignal.Samples = downsampled;
            }

         
   
    }
    }

