using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3_Forecasting.Algorithms
{
    interface ISmoothing
    {
        List<double> CalculateSmoothing(int forecastAmount);
        void SetBestValues();
    }
}
