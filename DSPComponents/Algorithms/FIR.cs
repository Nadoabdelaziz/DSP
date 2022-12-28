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
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public double window(double stopbandattenuation, int i, int N)
        {
            double wd = 0;
            if (stopbandattenuation <= 21)
            {
                wd = 1;
            }
            else if (stopbandattenuation <= 44)
            {
                wd = (double)(0.5 + (0.5 * Math.Cos((2 * Math.PI * i / N))));
            }
            else if (InputStopBandAttenuation <= 53)
            {
                wd = (double)(0.54 + (0.46 * Math.Cos((2 * Math.PI * i / N))));
            }
            else if (InputStopBandAttenuation <= 74)
            {
                wd = (double)(0.42 + (0.5 * (Math.Cos(2 * Math.PI * i / (N - 1)))) + (0.08 * (Math.Cos(4 * Math.PI * i / (N - 1)))));
            }
            return wd;
        }
        public int getN(double transition_width, double fs, double stopband)
        {
            double dleta_f = transition_width / fs;
            double number = 0.0;
            if (stopband <= 21)
            {
                number = 0.9;
            }
            else if (stopband <= 44)
            {
                number = 3.1;
            }
            else if (stopband <= 53)
            {
                number = 3.3;
            }
            else if (stopband <= 74)
            {
                number = 5.5;
            }

            int N = (int)Math.Round(number / dleta_f);
            if (N % 2 == 0)
                N++;//lazem tkon odd
            return N;

        }
        public override void Run()
        {
            OutputHn = new DSPAlgorithms.DataStructures.Signal(new List<float>(), false, new List<float>(), new List<float>(), new List<float>());
            double hd = 0;
            double wd = 0;
            int N = 0;
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                double fc_dash = (double)InputCutOffFrequency + (double)(InputTransitionBand / 2);
                fc_dash = fc_dash / InputFS;
                double omega = (double)(2 * Math.PI * fc_dash);
                N = getN(InputTransitionBand, InputFS, InputStopBandAttenuation);//l7d hna keda gbna N w fc' ely hn3wd beha flmo3dla

                for (int i = 0; i <= N / 2; i++)
                {
                    if (i == 0)
                    {
                        hd = 2 * fc_dash;
                    }
                    else
                    {
                        hd = (double)(2 * fc_dash * ((Math.Sin(i * omega)) / (i * omega)));
                    }

                    wd = window(InputStopBandAttenuation, i, N);

                    double result = (double)hd * (double)wd;
                    OutputHn.Samples.Add((float)Math.Round(result, 10));
                    OutputHn.SamplesIndices.Add(i);

                }
            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                double fc_dash = (double)InputCutOffFrequency - (double)(InputTransitionBand / 2);
                fc_dash = fc_dash / InputFS;
                double omega = (double)(2 * Math.PI * fc_dash);
                N = getN(InputTransitionBand, InputFS, InputStopBandAttenuation);//l7d hna keda gbna N w fc' ely hn3wd beha flmo3dla

                for (int i = 0; i <= N / 2; i++)
                {
                    if (i == 0)
                    {
                        hd = (double)(1.0 - (2 * fc_dash));
                    }
                    else
                    {
                        hd = (double)-(2 * fc_dash * (Math.Sin(i * omega)) / (i * omega));
                    }
                    wd = window(InputStopBandAttenuation, i, N);

                    double result = hd * wd;

                    OutputHn.Samples.Add((float)result);
                    OutputHn.SamplesIndices.Add(i);
                }

            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                double fc_dash1 = (double)InputF1 - (InputTransitionBand / 2);
                fc_dash1 = fc_dash1 / InputFS;
                double omega1 = (2 * Math.PI * fc_dash1);

                double fc_dash2 = (double)InputF2 + (InputTransitionBand / 2);
                fc_dash2 = fc_dash2 / InputFS;
                double omega2 = (2 * Math.PI * fc_dash2);
                //N = getN(InputStopBandAttenuation, InputTransitionBand, InputFS);//l7d hna keda gbna N w fc' ely hn3wd beha flmo3dla   .. de 8alt :( el parameters m3kosen
                N = getN(InputTransitionBand, InputFS, InputStopBandAttenuation);
                for (int i = 0; i <= N / 2; i++)
                {
                    if (i == 0)
                    {
                        hd = 2 * (fc_dash2 - fc_dash1);
                    }
                    else
                    {
                        hd = (double)((2 * fc_dash2 * ((Math.Sin(i * omega2)) / (i * omega2))) - (2 * fc_dash1 * ((Math.Sin(i * omega1)) / (i * omega1))));
                    }
                    wd = window(InputStopBandAttenuation, i, N);

                    double result = hd * wd;
                    OutputHn.Samples.Add((float)(result));
                    OutputHn.SamplesIndices.Add(i);
                }
            }
            else if (InputFilterType == FILTER_TYPES.BAND_STOP)
            {
                double fc_dash1 = (double)InputF1 + (InputTransitionBand / 2);
                fc_dash1 = fc_dash1 / InputFS;
                double omega1 = (2 * Math.PI * fc_dash1);

                double fc_dash2 = (double)InputF2 - (InputTransitionBand / 2);
                fc_dash2 = fc_dash2 / InputFS;
                double omega2 = (2 * Math.PI * fc_dash2);
                //N = getN(InputStopBandAttenuation, InputTransitionBand, InputFS);  nfs el 8alta el parameters m3kosen 
                N = getN(InputTransitionBand, InputFS, InputStopBandAttenuation);//l7d hna keda gbna N w fc' ely hn3wd beha flmo3dla   

                for (int i = 0; i <= N / 2; i++)
                {
                    if (i == 0)
                    {
                        hd = 1 - (2 * (fc_dash2 - fc_dash1));
                    }
                    else
                    {
                        hd = (double)((2 * fc_dash1 * ((Math.Sin(i * omega1)) / (i * omega1))) - (2 * fc_dash2 * ((Math.Sin(i * omega2)) / (i * omega2))));
                    }
                    wd = window(InputStopBandAttenuation, i, N);

                    double result = hd * wd;
                    OutputHn.Samples.Add((float)(result));
                    OutputHn.SamplesIndices.Add(i);
                }
            }


            List<float> sampless = new List<float>();
            List<int> indexxxs = new List<int>();
            for (int i = N / 2; i >= 0; i--)
            {
                if (i != 0)
                {
                    sampless.Add(OutputHn.Samples[i]);
                    indexxxs.Add(-i);
                }
            }
            OutputHn.Samples = sampless.Concat(OutputHn.Samples).ToList();
            OutputHn.SamplesIndices = indexxxs.Concat(OutputHn.SamplesIndices).ToList();
            DirectConvolution conv = new DirectConvolution();
            conv.InputSignal1 = InputTimeDomainSignal;
            conv.InputSignal2 = OutputHn;
            conv.Run();
            OutputYn = new DSPAlgorithms.DataStructures.Signal(new List<float>(), false, new List<float>(), new List<float>(), new List<float>());
            OutputYn = conv.OutputConvolvedSignal;
        }
        //  throw new NotImplementedException();
        //  }
         
    }
    }

