using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float>Acsum=new List<float>();
            
            Acsum.Add(InputSignal.Samples[0]);
            for (int i=1; i < InputSignal.Samples.Count;i++)
            {


               
                Acsum.Add( Acsum[i - 1] + InputSignal.Samples[i]);


            }

            OutputSignal = new Signal(Acsum, true);

        }
    }
}
