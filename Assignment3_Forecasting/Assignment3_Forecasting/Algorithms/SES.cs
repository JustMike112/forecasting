using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3_Forecasting.Algorithms
{
    class SES : ISmoothing
    {
        private List<double> data;
        public double alpha;
        public double SSE;

        public SES(List<double> dataSet)
        {
            data = dataSet;
            BestValues();
        }
        public List<double> CalculateSmoothing(int forecastAmount)
        {
            List<double> dataSetSES = new List<double> { data[0] };
            for (var i = 1; i < data.Count + forecastAmount; i++)
            {
                var smoothedPointSES = 0.0;
                if (i >= data.Count)
                    smoothedPointSES = dataSetSES[data.Count - 1];
                else
                    smoothedPointSES = (alpha * data[i]) + (1 - alpha) * (dataSetSES[i - 1]);
                dataSetSES.Add(smoothedPointSES);
            }

            return dataSetSES;
        }

        public void BestValues()
        {
            var alphaErrorSES = new List<Tuple<double, double>>();

            for (var i = 0.0; i < 1.0; i += 0.01)
            {
                var alphaSES = i;
                var SSE = 0.0;
                var dataSetSES = new List<double> { data[0] };

                for (var j = 1; j < data.Count; j++)
                {
                    var smoothedPointSES = (alphaSES * data[j - 1]) + (1 - alphaSES) * (dataSetSES[j - 1]);
                    dataSetSES.Add(smoothedPointSES);
                    SSE += Math.Pow((smoothedPointSES - data[j]), 2);
                }
                SSE = Math.Sqrt(SSE / (data.Count - 1));
                alphaErrorSES.Add(new Tuple<double, double>(i, SSE));
            }

            var alphaErrorSESFinal = alphaErrorSES.Aggregate((l, r) => (l.Item2 < r.Item2) ? l : r);
            alpha =  alphaErrorSESFinal.Item1;
            SSE = alphaErrorSESFinal.Item2;
        }
    }
}
