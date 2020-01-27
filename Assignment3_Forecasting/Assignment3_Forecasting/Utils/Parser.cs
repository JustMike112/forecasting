using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3_Forecasting.Utils
{
    static class Parser
    {
        public static List<double> Parse(string file, char delimiter)
        {
            var dataSet = File.ReadAllLines(file)
                .Select(line => double.Parse(line.Split(delimiter).ElementAt(1)))
                .ToList();
            return dataSet;
        }
    }
}
