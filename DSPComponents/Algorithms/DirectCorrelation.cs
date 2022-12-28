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
            bool nuu = false;
            List<float> out_not_norm = new List<float>();
            List<float> l1 = new List<float>();
            List<float> l2 = new List<float>();

            float num = 0;
            bool p = InputSignal1.Periodic;


            // auto correlation condition
            if (InputSignal2 == null)
            {
                nuu = true;
                InputSignal2 = new Signal(new List<float>(), InputSignal1.Periodic);
                for (int i = 0; i < InputSignal1.Samples.Count; ++i)
                {
                    InputSignal2.Samples.Add(InputSignal1.Samples[i]);
                    l1.Add((float)InputSignal1.Samples[i]);
                    l2.Add((float)InputSignal2.Samples[i]);
                }
            }

            // Cross correlation condition
            else
            {
                for (int i = 0; i < InputSignal1.Samples.Count; ++i)
                {
                    l1.Add((float)InputSignal1.Samples[i]);
                }
                for (int i = 0; i < InputSignal2.Samples.Count; ++i)
                {
                    l2.Add((float)InputSignal2.Samples[i]);
                }
            }

            int j = 0;

            for (int i = 0; i < l1.Count; ++i)
                out_not_norm.Add(0);

            while (j < l1.Count)
            {
                //num = 0;
                for (int i = 0; i < l1.Count; ++i)
                {
                    out_not_norm[j] += (l1[i] * l2[i]);
                }

                out_not_norm[j] /= l1.Count;

                if (p == true)
                {
                    float tmp = l2[0];

                    for (int i = 0; i < l2.Count - 1; ++i)
                    {
                        // shift left
                        l2[i] = l2[i + 1];
                    }

                    // rotation
                    l2[l2.Count - 1] = tmp;
                }
                else
                {
                    for (int i = 0; i < l2.Count - 1; ++i)
                    {
                        l2[i] = l2[i + 1];
                    }
                    l2[l2.Count - 1] = 0;
                }
                ++j;
            }

            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            for (int i = 0; i < out_not_norm.Count; ++i)
            {
                OutputNonNormalizedCorrelation.Add((float)out_not_norm[i]);
            }

            // InputSignal2 equal null
            // auto correlation condition
            if (nuu == true)
            {
                for (int i = 0; i < InputSignal1.Samples.Count; ++i)
                {
                    l1[i] = ((float)InputSignal1.Samples[i]);
                    l2[i] = ((float)InputSignal1.Samples[i]);
                }
            }

            // Cross correlation condition
            // InputSignal2 equal null
            else
            {
                for (int i = 0; i < InputSignal1.Samples.Count; ++i)
                {
                    l1[i] = ((float)InputSignal1.Samples[i]);
                }
                for (int i = 0; i < InputSignal2.Samples.Count; ++i)
                {
                    l2[i] = ((float)InputSignal2.Samples[i]);
                }
            }


            // get normalization 

            float sum1 = 0;
            float sum2 = 0;

            for (int i = 0; i < l1.Count; ++i)
            {
                sum1 += (float)Math.Pow(l1[i],2);
            }
            for (int i = 0; i < l2.Count; ++i)
            {
                sum2 += (float)Math.Pow(l2[i], 2);
            }

            float norm = (float)Math.Sqrt(sum2 * sum1);
            norm /= l1.Count;

            if (norm != 0)
                for (int i = 0; i < out_not_norm.Count; ++i)
                {
                    OutputNormalizedCorrelation.Add((float)(out_not_norm[i] / norm));
                }
        }
    }
}