using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assignment3_Forecasting.Algorithms;
using Assignment3_Forecasting.Utils;

namespace Assignment3_Forecasting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            //Read SwordsDemand.csv containing 36 rows
            List<double> dataSet = Parser.Parse("SwordsDemand.csv", ',');

            SES ses = new SES(dataSet);
            DES des = new DES(dataSet);
            TES tes = new TES(dataSet);

            for (var i = 0; i < dataSet.Count; i++)
            {
                chart1.Series["Data"].Points.AddXY(i + 1, dataSet[i]);
            }

            List<double> dataSetSES = ses.CalculateSmoothing(12);
            for (var i = 0; i < dataSetSES.Count; i++)
            {
                chart1.Series["SES"].Points.AddXY(i + 1, dataSetSES[i]);
            }

            List<double> dataSetDES = des.CalculateSmoothing(12);
            for (var i = 0; i < dataSetDES.Count; i++)
            {
                chart1.Series["DES"].Points.AddXY(i + 2, dataSetDES[i]);
            }

            List<double> dataSetTES = tes.CalculateSmoothing(12);
            for (var i = 0; i < dataSetTES.Count; i++)
            {
                chart1.Series["TES"].Points.AddXY(i + 1, dataSetTES[i]);
            }

            Tuple<double, double> valuesSES = ses.GetBestValues();
            Tuple<double, double, double> valuesDES = des.GetBestValues();
            Tuple<double, double, double, double> valuesTES = tes.GetBestValues();

            textBox1.AppendText("Best Alpha for SES: " + valuesSES.Item1 + Environment.NewLine);
            textBox1.AppendText("Best Error for SES: " + valuesSES.Item2 + Environment.NewLine + Environment.NewLine);

            textBox1.AppendText("Best Alpha for DES: " + valuesDES.Item1 + Environment.NewLine);
            textBox1.AppendText("Best Beta for DES: " + valuesDES.Item2 + Environment.NewLine);
            textBox1.AppendText("Best Error for DES: " + valuesDES.Item3 + Environment.NewLine + Environment.NewLine);

            textBox1.AppendText("Best Alpha for TES: " + valuesTES.Item1 + Environment.NewLine);
            textBox1.AppendText("Best Beta for TES: " + valuesTES.Item2 + Environment.NewLine);
            textBox1.AppendText("Best Gamma for TES: " + valuesTES.Item3 + Environment.NewLine);
            textBox1.AppendText("Best Error for TES: " + valuesTES.Item4 + Environment.NewLine);
        }
    }
}
