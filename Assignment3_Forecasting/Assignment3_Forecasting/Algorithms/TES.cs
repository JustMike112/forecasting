using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3_Forecasting.Algorithms
{
    class TES : ISmoothing
    {
        private List<double> data;
        private double alpha;
        private double beta;
        private double gamma;
        private double SSE;

        // these values were given using linear regression
        private readonly double intercept = 144.423542254481;
        private readonly double rico = 2.29045000493894;

        public TES(List<double> dataSet)
        {
            data = dataSet;
            SetBestValues();
        }

        public List<double> CalculateSmoothing(int forecastAmount)
        {
            Console.WriteLine("alpha = " + alpha + ", beta = " + beta + ", gamma = " + gamma);
            var forecast = Forecast(forecastAmount);

            return forecast.Item1;
        }

        private Tuple<List<double>, double> Forecast(int forecastAmount)
        {
            var dataSetTES = new List<double> { intercept };
            var dataSetTrendTES = new List<double> { rico };
            var dataSetSeasonsTES = InitialSeasonalAdjustments();
            var dataSetForecastTES = new List<double>();
            var errors = new List<double>();
            var oneStepForecasts = new List<double>();
            var localSSE = 0.0;

            for (int i = 0; i < data.Count; i++)
            {
                var oneStepForecast = (dataSetTES[i] + dataSetTrendTES[i]) * dataSetSeasonsTES[i];
                var error = data[i] - oneStepForecast;
                var smoothed = dataSetTES[i] + dataSetTrendTES[i] + alpha * error / dataSetSeasonsTES[i];
                var trend = dataSetTrendTES[i] + alpha * beta * error / dataSetSeasonsTES[i];
                var season = dataSetSeasonsTES[i] + gamma * (1 - alpha) * error / (dataSetTES[i] + dataSetTrendTES[i]);

                oneStepForecasts.Add(oneStepForecast);
                errors.Add(error);
                dataSetTES.Add(smoothed);
                dataSetTrendTES.Add(trend);
                dataSetSeasonsTES.Add(season);
            }

            for (int i = data.Count; i < data.Count + forecastAmount; i++)
            {
                var smoothed = (dataSetTES[data.Count - 1] + (i - data.Count - 1) * dataSetTrendTES[data.Count]) * dataSetSeasonsTES[i];
                dataSetTES.Add(smoothed);
            }

            for (int i = 0; i < errors.Count; i++)
            {
                localSSE += Math.Pow(errors[i], 2);
            }

            for (int i = 1; i < dataSetTES.Count; i++)
            {
                dataSetForecastTES.Add(dataSetTES[i]);
            }

            for (int i = 0; i < dataSetForecastTES.Count; i++)
            {
                Console.WriteLine(dataSetForecastTES[i]);
            }

            return new Tuple<List<double>, double>(dataSetForecastTES, localSSE);
        }

        // take the average of the surrounding 12 months both ways (for t=7 use the average of (t=1->12) and (t=2->13))
        public List<double> InitialSeasonalAdjustments()
        {
            var total_amount = data.Count;
            List<double> movingAverages = new List<double>();
            List<double> skew = new List<double>();
            List<double> seasons = new List<double>();

            for (int i=6; i < data.Count-6; i++)
            {
                var movingAverage = 0.0;
                var movingAverageBefore = 0.0;
                var movingAverageAfter = 0.0;

                for (int j = -6; j < 6; j++)
                {
                    movingAverageBefore += data[i + j];
                    movingAverageAfter += data[i + j + 1];
                }

                movingAverage = ((movingAverageBefore / 12) + (movingAverageAfter / 12)) / 2;
                movingAverages.Add(movingAverage);
            }

            for (int i = 6; i < data.Count - 6; i++)
            {
                skew.Add(data[i] / movingAverages[i - 6]);
            }

            //We start in july, so the seventh month is januari
            for (int i = 0; i < 12; i++)
            {
                var years = skew.Count / 12;
                var average = 0.0;

                for (int j = 0; j < years; j++)
                {
                    if (i < 6)
                        average += skew[i + 6 + (j * 12)];
                    else
                        average += skew[i - 6 + (j * 12)];
                }

                seasons.Add(average / years);
            }

            return seasons;
        }

        public void SetBestValues()
        {
            double bestSSE = 100000000.0;
            double bestAlpha = 0.0;
            double bestBeta = 0.0;
            double bestGamma = 0.0;

            for (double a = 0.0; a < 1.0; a += 0.1)
            {
                alpha = a;
                for (double b = 0.0; b < 1.0; b += 0.1)
                {
                    beta = b;
                    for (double c = 0.0; c < 1.0; c += 0.1)
                    {
                        gamma = c;
                        var forecast = Forecast(0);
                        if (forecast.Item2 < bestSSE)
                        {
                            bestSSE = forecast.Item2;
                            bestAlpha = a;
                            bestBeta = b;
                            bestGamma = c;
                        }
                    }
                }
            }

            SSE = Math.Sqrt(bestSSE / (data.Count - 3));
            alpha = bestAlpha;
            beta = bestBeta;
            gamma = bestGamma;
        }

        public Tuple<double, double, double, double> GetBestValues()
        {
            return new Tuple<double, double, double, double>(alpha, beta, gamma, SSE);
        }

    }
}
