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
            List<float> correlationValues = new List<float>();
            DirectCorrelation x = new DirectCorrelation();
            List<float> samples1 = new List<float>();
            List<float> samples2 = new List<float>();

            for (int j = 0; j < InputSignal1.Samples.Count; j++)
                samples1.Add(InputSignal1.Samples[j]);

            for (int j = 0; j < InputSignal2.Samples.Count; j++)
                samples2.Add(InputSignal2.Samples[j]);


            x.InputSignal1 = new Signal(samples1, false);
            x.InputSignal2 = new Signal(samples2, false);
            
            // call direct coreelation function
            x.Run();

            int foundMax = 0;
            float maxCorrelation = x.OutputNonNormalizedCorrelation[0];
            for (int i = 1; i < x.OutputNonNormalizedCorrelation.Count; i++)
            {
                if (Math.Abs(x.OutputNonNormalizedCorrelation[i]) > maxCorrelation)
                {
                    maxCorrelation = x.OutputNonNormalizedCorrelation[i];
                    foundMax = i;
                }
            }
            OutputTimeDelay = foundMax * InputSamplingPeriod; 
        }
    }
}