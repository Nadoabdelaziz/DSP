using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {

            //IF AUTO CORRELATION
            if (InputSignal2 == null)
            {
                InputSignal2 = InputSignal1;
            }

            //NORMALIZATION
            float sum1 = 0;
            float sum2 = 0;

            for (int i = 0; i < InputSignal1.Samples.Count; ++i)
            {
                sum1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
            }
            for (int i = 0; i < InputSignal2.Samples.Count; ++i)
            {
                sum2 += (float)Math.Pow(InputSignal2.Samples[i], 2);
            }

            float norm = (float)Math.Sqrt(sum2 * sum1);
            norm /= InputSignal1.Samples.Count;

            //FOR EACH SIGNAL ( AMPLITUDE and PHASESHIFT)
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();

            //FOR FIRST SIGNAL
            dft.InputTimeDomainSignal = InputSignal1;
            dft.Run();

            List<float> amplitudeS1 = new List<float>();
            List<float> frequencyS1 = new List<float>();
            amplitudeS1 = dft.OutputFreqDomainSignal.FrequenciesAmplitudes;
            frequencyS1 = dft.OutputFreqDomainSignal.FrequenciesPhaseShifts;

            //FOR SECOND SIGNAL
            dft.InputTimeDomainSignal = InputSignal2;
            dft.Run();

            List<float> amplitudeS2 = new List<float>();
            List<float> frequencyS2 = new List<float>();
            amplitudeS2 = dft.OutputFreqDomainSignal.FrequenciesAmplitudes;
            frequencyS2 = dft.OutputFreqDomainSignal.FrequenciesPhaseShifts;

            //OUTPUT RESULT FOR Amplitudes & Phaseshifts
            List<float> out_amp = new List<float>();
            List<float> out_freq = new List<float>();

            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                out_amp.Add(amplitudeS1[i] * amplitudeS2[i]);
                out_freq.Add(-frequencyS1[i] + frequencyS2[i]); //CONGICATE
            }

            //Sending them to a signal
            Signal input_freq_domain = new Signal(false, null, out_amp, out_freq);
            idft.InputFreqDomainSignal = input_freq_domain;
            idft.Run();

            //NORMALIZATION
            OutputNonNormalizedCorrelation = idft.OutputTimeDomainSignal.Samples;
            OutputNormalizedCorrelation = new List<float>(OutputNonNormalizedCorrelation);
            for (int i = 0; i < OutputNonNormalizedCorrelation.Count; i++)
            {
                OutputNonNormalizedCorrelation[i] /= InputSignal1.Samples.Count;
                OutputNormalizedCorrelation[i] = OutputNormalizedCorrelation[i] / (float)norm / InputSignal1.Samples.Count;
            }
        }
    }
    }
