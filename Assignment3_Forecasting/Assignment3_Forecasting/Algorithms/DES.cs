using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3_Forecasting.Algorithms
{
    class DES : ISmoothing
    {
        private List<double> data;
        private double alpha;
        private double beta;
        private double SSE;

        public DES(List<double> dataSet)
        {
            data = dataSet;
            SetBestValues();
        }

        public List<double> CalculateSmoothing(int forecastAmount)
        {
            Console.WriteLine(beta);
            var dataSetDES = new List<double> { data[0], data[1] };
            var dataSetTrendDES = new List<double> { 0, data[1] - data[0] };
            var dataSetForecastDES = new List<double>();

            for (var i = 2; i < data.Count + forecastAmount; i++)
            {
                var smoothedPointDES = 0.0;
                var trendPointDES = 0.0;
                var forecastPointDES = 0.0;

                if (i >= data.Count)
                {
                    forecastPointDES = dataSetDES[data.Count - 1] + (dataSetTrendDES[data.Count - 1] * (i - data.Count));
                    dataSetForecastDES.Add(forecastPointDES);
                }
                else
                {
                    smoothedPointDES = (alpha * data[i]) + (1 - alpha) * (dataSetDES[i - 1] + dataSetTrendDES[i - 1]);
                    dataSetDES.Add(smoothedPointDES);
                    trendPointDES = (beta * (dataSetDES[i] - dataSetDES[i - 1])) + (1 - beta) * dataSetTrendDES[i - 1];
                    dataSetTrendDES.Add(trendPointDES);
                    forecastPointDES = dataSetDES[i - 1] + dataSetTrendDES[i - 1];
                    dataSetForecastDES.Add(forecastPointDES);
                }
            }

            return dataSetForecastDES;
        }

        public void SetBestValues()
        {
            var alphaBetaErrorDES = new List<Tuple<double, double, double>>();

            for (var i = 0.0; i < 1.0; i += 0.01)
            {
                var alphaDES = i;

                for (var j = 0.0; j < 1.0; j += 0.01)
                {
                    var betaDES = j;
                    var dataSetDES = new List<double> { data[0], data[1] };
                    var dataSetTrendDES = new List<double> { 0, data[1] - data[0] };
                    var dataSetForecastDES = new List<double>();
                    var SSE = 0.0;
                    for (var k = 2; k < data.Count; k++)
                    {
                        var smoothedPointDES = (alphaDES * data[k]) + (1 - alphaDES) * (dataSetDES[k - 1] + dataSetTrendDES[k - 1]);
                        dataSetDES.Add(smoothedPointDES);
                        var trendPointDES = (betaDES * (dataSetDES[k] - dataSetDES[k - 1])) + (1 - betaDES) * dataSetTrendDES[k - 1];
                        dataSetTrendDES.Add(trendPointDES);
                        var forecastPointDES = dataSetDES[k - 1] + dataSetTrendDES[k - 1];
                        dataSetForecastDES.Add(forecastPointDES);
                        SSE += Math.Pow((forecastPointDES - data[k]), 2);
                    }
                    SSE = Math.Sqrt(SSE / (data.Count - 2));
                    alphaBetaErrorDES.Add(new Tuple<double, double, double>(i, j, SSE));
                }
            }
            var alphaBetaErrorDESFinal = alphaBetaErrorDES.Aggregate((l, r) => (l.Item3 < r.Item3) ? l : r);
            alpha = alphaBetaErrorDESFinal.Item1;
            beta = alphaBetaErrorDESFinal.Item2;
            SSE = alphaBetaErrorDESFinal.Item3;
        }

        public Tuple<double, double, double> GetBestValues()
        {
            return new Tuple<double, double, double>(alpha, beta, SSE);
        }
    }
}
