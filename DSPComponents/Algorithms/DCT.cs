using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> output = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                int x = 0;
                double Result = 0;
                while(x < InputSignal.Samples.Count)
                {
                    float thesignal = InputSignal.Samples[x];
                    Result += thesignal * Math.Cos((Math.PI) / (4 * InputSignal.Samples.Count) * (((2 * x) - 1) * ((2 * i) - 1) ));
                    x++;
                }
                output.Add((float)(Result * Math.Sqrt(2f / InputSignal.Samples.Count)));
            }
            OutputSignal = new Signal(output, false);
        }
    }
}
