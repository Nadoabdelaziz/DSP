using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            //CHECKING FOR INPUT SIZE
            int size = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;

            for (int i = InputSignal1.Samples.Count; i < size; i++)
            {
                InputSignal1.Samples.Add(0);
            }
            for (int z = InputSignal2.Samples.Count; z < size; z++)
            {
                InputSignal2.Samples.Add(0);
            }

            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();

            //FOR FIRST SIGNAL
            //Listing R and I (DFT)
            List<Complex> CList1 = new List<Complex>();
            dft.InputTimeDomainSignal = InputSignal1;
            dft.Run();
            for (int i = 0; i < size; i++)
            {
                Complex temp = new Complex(dft.realPart[i], dft.imagPart[i]);
                CList1.Add(temp);
            }

            //FOR SECOND SIGNAL
            //Listing R and I (DFT)
            List<Complex> CList2 = new List<Complex>();
            dft.InputTimeDomainSignal = InputSignal2;
            dft.Run();
            for (int j = 0; j < size; j++)
            {
                Complex temp = new Complex(dft.realPart[j], dft.imagPart[j]);
                CList2.Add(temp);
            }

            //MULTIPLYING S1 & S2 after fourier transform
            List<Complex> listcomplex = new List<Complex>();
            for (int k = 0; k < size; k++)
            {
                listcomplex.Add(Complex.Multiply(CList1[k], CList2[k]));
            }

            //AMPLITUDES and PHASESHIFTS
            List<float> amplitudes = new List<float>();
            List<float> phaseshifts = new List<float>();
            for (int a = 0; a < size; a++)
            {
                amplitudes.Add((float)(Math.Sqrt(Math.Pow(listcomplex[a].Real, 2) + Math.Pow(listcomplex[a].Imaginary, 2))));
                phaseshifts.Add((float)Math.Atan2(listcomplex[a].Imaginary, listcomplex[a].Real));
            }

            //Sending them to a signal
            Signal res = new Signal(false, new List<float>(), amplitudes, phaseshifts);
            idft.InputFreqDomainSignal = res;
            idft.Run();

            OutputConvolvedSignal = new Signal(idft.OutputTimeDomainSignal.Samples, false);
        }
    }
}

