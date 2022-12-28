using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> samples = new List<float>();

            float sum = 0;

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                sum = sum + InputSignal.Samples[i];
            }

            float mean = sum / InputSignal.Samples.Count;

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                samples.Add(InputSignal.Samples[i] - mean);
            }

            OutputSignal = new Signal(samples, false);
        }
    }
}
