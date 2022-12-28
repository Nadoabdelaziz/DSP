using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }
        public List<float> realPart { get; set; }
        public List<float> imagPart { get; set; }
        public override void Run()
        {
            List<float> Sines_List = new List<float>();
            List<float> Cosines_List = new List<float>();
            List<float> Samples = new List<float>();
            List<float> Frequencies = new List<float>();
            List<float> Frequencies_Amplitudes = new List<float>();
            List<float> Frequencies_PhaseShifts = new List<float>();

            OutputFreqDomainSignal = new Signal(Samples, false, Frequencies, Frequencies_Amplitudes, Frequencies_PhaseShifts);

            for (int k = 0; k < InputTimeDomainSignal.Samples.Count; k++)
            {
                float Real_Part = 0;
                float Imaginary_Part = 0;

                for (int n = 0; n < InputTimeDomainSignal.Samples.Count; n++)
                {
                    Real_Part += InputTimeDomainSignal.Samples[n] * ((float)Math.Cos((k * 2 * Math.PI * n / InputTimeDomainSignal.Samples.Count)));
                    Imaginary_Part += -1 * InputTimeDomainSignal.Samples[n] * ((float)Math.Sin((k * 2 * Math.PI * n / InputTimeDomainSignal.Samples.Count)));
                }

                Sines_List.Add(Imaginary_Part);
                Cosines_List.Add(Real_Part);

                OutputFreqDomainSignal.FrequenciesAmplitudes.Add((float)(Math.Sqrt(Math.Pow(Real_Part, 2) + Math.Pow(Imaginary_Part, 2))));
                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)Math.Atan2(Imaginary_Part, Real_Part));
            }

            realPart = Cosines_List;
            imagPart = Sines_List;
        }
    }
}
