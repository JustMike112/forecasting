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

            var x = tes.InitialSeasonalAdjustments();

            for (var i = 0; i < dataSet.Count; i++)
            {
                chart1.Series["Data"].Points.AddXY(i + 1, dataSet[i]);
            }

            List<double> dataSetSES = ses.CalculateSmoothing(12);
            for (var i = 0; i < dataSetSES.Count; i++)
            {
                chart1.Series["SES"].Points.AddXY(i + 1, dataSetSES[i]);
            }

            var dataSetDES = des.CalculateSmoothing(12);
            for (var i = 0; i < dataSetDES.Count; i++)
            {
                chart1.Series["DES"].Points.AddXY(i + 2, dataSetDES[i]);
            }

            var dataSetTES = tes.CalculateSmoothing(12);
            for (var i = 0; i < dataSetTES.Count; i++)
            {
                chart1.Series["TES"].Points.AddXY(i + 1, dataSetTES[i]);
            }

            textBox1.AppendText("Best Alpha for SES: " + ses.alpha + Environment.NewLine);
            textBox1.AppendText("Best Error for SES: " + ses.SSE + Environment.NewLine + Environment.NewLine);

            textBox1.AppendText("Best Alpha for DES: " + des.alpha + Environment.NewLine);
            textBox1.AppendText("Best Beta for DES: " + des.beta + Environment.NewLine);
            textBox1.AppendText("Best Error for DES: " + des.SSE + Environment.NewLine + Environment.NewLine);

            textBox1.AppendText("Best Alpha for TES: " + tes.alpha + Environment.NewLine);
            textBox1.AppendText("Best Beta for TES: " + tes.beta + Environment.NewLine);
            textBox1.AppendText("Best Gamma for TES: " + tes.gamma + Environment.NewLine);
            textBox1.AppendText("Best Error for TES: " + tes.SSE + Environment.NewLine);
        }
    }
}
