using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            double S = 0;
            double X = 0;
            double v = 0;
            double r = 0;
            double T = 0;
            double MVc = 0;
            double MVp = 0;

            try
            {
                S = double.Parse(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("There was an error in input of Stock Price. The input should be on the form 100,00.");
            }

            try
            {
                X = double.Parse(textBox2.Text); 
            }
            catch
            {
                MessageBox.Show("There was an error in input of Strike Price. The input should be on the form 100,00.");
            }

            try
            {
                v = double.Parse(textBox3.Text);
            }
            catch
            {
                MessageBox.Show("There was an error in input of Standard Deviation. The input should be on the form 0,25 if 25%.");
            }

            try
            {
                r = double.Parse(textBox4.Text);
            }
            catch
            {
                MessageBox.Show("There was an error in input of Risk Free Rate. The input should be on the form 0,05 for 5%.");
            }

            try
            {
                T = double.Parse(textBox5.Text) / 365;
            }
            catch
            {
                MessageBox.Show("There was an error in input of Days to Expiration. The input should be on the form 30 for 30 days.");
            }
          
            textBox8.Text = "";
            textBox7.Text = "";
            textBox6.Text = "";
            textBox9.Text = "";
            textBox11.Text = "";
            textBox10.Text = "";
            textBox15.Text = "";
            textBox14.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
            textBox19.Text = "";
            textBox18.Text = "";
            textBox17.Text = "";
            textBox16.Text = "";
            textBox21.Text = "";
            textBox20.Text = "";
            textBox23.Text = "";
            textBox25.Text = "";


            try
            {
                textBox7.AppendText(Math.Round(BSM("c", S, X, v, r, T), 4).ToString());
                textBox8.AppendText(Math.Round(BSM("p", S, X, v, r, T), 4).ToString());
                textBox6.AppendText(Math.Round(IV("c", S, X, v, r, T), 4).ToString());
                textBox9.AppendText(Math.Round(IV("p", S, X, v, r, T), 4).ToString());
                textBox11.AppendText(Math.Round(TV("c", S, X, v, r, T), 4).ToString());
                textBox10.AppendText(Math.Round(TV("p", S, X, v, r, T), 4).ToString());
                textBox15.AppendText(Math.Round(Delta("c", S, X, v, r, T), 4).ToString());
                textBox14.AppendText(Math.Round(Delta("p", S, X, v, r, T), 4).ToString());
                textBox13.AppendText(Math.Round(Vega(S, X, v, r, T), 4).ToString());
                textBox12.AppendText(Math.Round(Vega(S, X, v, r, T), 4).ToString());
                textBox19.AppendText(Math.Round(Gamma(S, X, v, r, T), 4).ToString());
                textBox18.AppendText(Math.Round(Gamma(S, X, v, r, T), 4).ToString());
                textBox17.AppendText(Math.Round(Theta("c", S, X, v, r, T), 4).ToString());
                textBox16.AppendText(Math.Round(Theta("p", S, X, v, r, T), 4).ToString());
                textBox21.AppendText(Math.Round(Rho("c", S, X, v, r, T), 4).ToString());
                textBox20.AppendText(Math.Round(Rho("p", S, X, v, r, T), 4).ToString());
                
            }
            catch
            {
                MessageBox.Show("There was an error in some of the calculations.");
            }


            try
            {
                MVc = double.Parse(textBox22.Text);
                textBox23.Text = "";
                textBox23.AppendText(Math.Round(newtonc(S, X, r, T, MVc), 5).ToString());
            }
            catch
            {
                textBox23.Text = "";
            }

            try
            {
                MVp = double.Parse(textBox24.Text);
                textBox25.Text = "";
                textBox25.AppendText(Math.Round(newtonp(S, X, r, T, MVp), 5).ToString());
            }
            catch
            {
                textBox25.Text = "";
            }
        }

        public double CND(double X)
        {
            double L = 0.0;
            double K = 0.0;
            double dCND = 0.0;
            const double a1 = 0.31938153;
            const double a2 = -0.356563782;
            const double a3 = 1.781477937;
            const double a4 = -1.821255978;
            const double a5 = 1.330274429;
            L = Math.Abs(X);
            K = 1.0 / (1.0 + 0.2316419 * L);
            dCND = 1.0 - 1.0 / Math.Sqrt(2 * Convert.ToDouble(Math.PI.ToString())) *
                Math.Exp(-L * L / 2.0) * (a1 * K + a2 * K * K + a3 * Math.Pow(K, 3.0) +
                a4 * Math.Pow(K, 4.0) + a5 * Math.Pow(K, 5.0));

            if (X < 0)
            {
                return 1.0 - dCND;
            }
            else
            {
                return dCND;
            }
        }

        public double d1(double S, double X, double v, double r, double T)
        {
            double d1 = 0;
            d1 = (Math.Log(S / X) + (r + v * v / 2.0) * T) / (v * Math.Sqrt(T));
            return d1;
        }

        public double d2(double S, double X, double v, double r, double T)
        {
            double d2 = 0;
            d2 = (Math.Log(S / X) + (r + v * v / 2.0) * T) / (v * Math.Sqrt(T)) - v * Math.Sqrt(T);
            return d2;
        }

        public double dNd1(double S, double X, double v, double r, double T)
        {
            double dNd1 = 0;
            dNd1 = (1/Math.Sqrt(2*Math.PI))*Math.Exp(-(d1(S,X,v,r,T)*d1(S,X,v,r,T))/2);
            return dNd1;
        }

        public double dNd2(double S, double X, double v, double r, double T)
        {
            double dNd2 = 0;
            dNd2 = (1 / Math.Sqrt(2 * Math.PI)) * Math.Exp(-(d2(S, X, v, r, T) * d2(S, X, v, r, T)) / 2);
            return dNd2;
        }

        public double BSM(String Flag, double S, double X, double v, double r, double T)
        {
            double BSM = 0;
            if (Flag == "c")
            {
                BSM = S * CND(d1(S, X, v, r, T)) - X * Math.Exp(-r * T) * CND(d2(S, X, v, r, T));
                return BSM;
            }
            else
            {
                BSM = X * Math.Exp(-r * T) * CND(-d2(S, X, v, r, T)) - S * CND(-d1(S, X, v, r, T));
                return BSM;
            }
        }

        public double IV(String Flag, double S, double X, double v, double r, double T)
        {
            double IV = 0;
            if (Flag == "c")
            {
                IV = S - X;
                if (IV > 0)
                {
                    return IV;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                IV = X - S;
                if (IV > 0)
                {
                    return IV;
                }
                else
                {
                    return 0;
                }
            }
        }

        private double TV(String Flag, double S, double X, double v, double r, double T)
        {
            double TV = 0;
            if (Flag == "c")
            {
                TV = BSM("c", S, X, v, r, T) - IV("c", S, X, v, r, T);
                return TV;
            }
            else
            {
                TV = BSM("p", S, X, v, r, T) - IV("p", S, X, v, r, T);
                return TV;
            }
        }

        private double Delta(String Flag, double S, double X, double v, double r, double T)
        {
            double Delta = 0;
            if (Flag == "c")
            {
                Delta = CND(d1(S, X, v, r, T));
                return Delta;
            }
            else
            {
                Delta = CND(d1(S, X, v, r, T)) - 1;
                return Delta;
            }
        }

        private double Vega(double S, double X, double v, double r, double T)
        {
            double Vega = 0;
            Vega = (S * Math.Sqrt(T) * (Math.Exp((-0.5 * d1(S, X, v, r, T) * d1(S, X, v, r, T))))) / (Math.Sqrt(2 * Math.PI));
            return (Vega / 100);
        }

        private double Gamma(double S, double X, double v, double r, double T)
        {
            double Gamma = 0;
            Gamma = (X * Math.Exp(-r * T) * dNd2(S, X, v, r, T)) / (S * S * v * Math.Sqrt(T));
            return Gamma;
        }

        private double Theta(String Flag, double S, double X, double v, double r, double T)
        {
            double Theta = 0;
            if (Flag == "c")
            {
                Theta = -X * Math.Exp(-r * T) * (r * CND(d2(S, X, v, r, T)) + ((v * dNd2(S, X, v, r, T)) / (S * Math.Sqrt(T))));
                return (Theta / 365);
            }
            else
            {
                Theta = -X * Math.Exp(-r * T) * (r * CND(-d2(S, X, v, r, T)) - ((v * dNd2(S, X, v, r, T)) / (S * Math.Sqrt(T))));
                return (Theta / 365);
            }
        }

        private double Rho(String Flag, double S, double X, double v, double r, double T)
        {
            double Rho = 0;
            if (Flag == "c")
            {
                Rho = T * X * Math.Exp(-r * T) * CND(d2(S, X, v, r, T));
                return (Rho / 100);
            }
            else
            {
                Rho = T * X * Math.Exp(-r * T) * CND(-d2(S, X, v, r, T));
                return (Rho / 100);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox8.Text = "";
            textBox7.Text = "";
            textBox6.Text = "";
            textBox9.Text = "";
            textBox11.Text = "";
            textBox10.Text = "";
            textBox15.Text = "";
            textBox14.Text = "";
            textBox13.Text = "";
            textBox12.Text = "";
            textBox19.Text = "";
            textBox18.Text = "";
            textBox17.Text = "";
            textBox16.Text = "";
            textBox21.Text = "";
            textBox20.Text = "";
            textBox22.Text = "";
            textBox23.Text = "";
            textBox24.Text = "";
            textBox25.Text = "";

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Documentation f2 = new Documentation();
            f2.ShowDialog();
        }

        private double newtonc(double S, double X, double r, double T, double MVc)
        {
            double v = 0.5;
            double v1 = 0.5 - ((S * CND((Math.Log(S / X) + (r + v * v / 2.0) * T) / (v * Math.Sqrt(T))) - X * Math.Exp(-r * T) * CND((Math.Log(S / X) + (r + v * v / 2.0) * T) / (v * Math.Sqrt(T)) - v * Math.Sqrt(T))) - MVc) / ((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v * v / 2.0) * T) / (v * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v * v / 2.0) * T) / (v * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)));
            double v2 = v1 - ((S * CND((Math.Log(S / X) + (r + v1 * v1 / 2.0) * T) / (v1 * Math.Sqrt(T))) - X * Math.Exp(-r * T) * CND((Math.Log(S / X) + (r + v1 * v1 / 2.0) * T) / (v1 * Math.Sqrt(T)) - v1 * Math.Sqrt(T))) - MVc) / ((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v1 * v1 / 2.0) * T) / (v1 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v1 * v1 / 2.0) * T) / (v1 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)));
            double v3 = v2 - ((S * CND((Math.Log(S / X) + (r + v2 * v2 / 2.0) * T) / (v2 * Math.Sqrt(T))) - X * Math.Exp(-r * T) * CND((Math.Log(S / X) + (r + v2 * v2 / 2.0) * T) / (v2 * Math.Sqrt(T)) - v2 * Math.Sqrt(T))) - MVc) / ((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v2 * v2 / 2.0) * T) / (v2 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v2 * v2 / 2.0) * T) / (v2 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)));
            double v4 = v3 - ((S * CND((Math.Log(S / X) + (r + v3 * v3 / 2.0) * T) / (v3 * Math.Sqrt(T))) - X * Math.Exp(-r * T) * CND((Math.Log(S / X) + (r + v3 * v3 / 2.0) * T) / (v3 * Math.Sqrt(T)) - v3 * Math.Sqrt(T))) - MVc) / ((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v3 * v3 / 2.0) * T) / (v3 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v3 * v3 / 2.0) * T) / (v3 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)));
            double v5 = v4 - ((S * CND((Math.Log(S / X) + (r + v4 * v4 / 2.0) * T) / (v4 * Math.Sqrt(T))) - X * Math.Exp(-r * T) * CND((Math.Log(S / X) + (r + v4 * v4 / 2.0) * T) / (v4 * Math.Sqrt(T)) - v4 * Math.Sqrt(T))) - MVc) / ((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v4 * v4 / 2.0) * T) / (v4 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v4 * v4 / 2.0) * T) / (v4 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)));
            double v6 = v5 - ((S * CND((Math.Log(S / X) + (r + v5 * v5 / 2.0) * T) / (v5 * Math.Sqrt(T))) - X * Math.Exp(-r * T) * CND((Math.Log(S / X) + (r + v5 * v5 / 2.0) * T) / (v5 * Math.Sqrt(T)) - v5 * Math.Sqrt(T))) - MVc) / ((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v5 * v5 / 2.0) * T) / (v5 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v5 * v5 / 2.0) * T) / (v5 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)));
            double v7 = v6 - ((S * CND((Math.Log(S / X) + (r + v6 * v6 / 2.0) * T) / (v6 * Math.Sqrt(T))) - X * Math.Exp(-r * T) * CND((Math.Log(S / X) + (r + v6 * v6 / 2.0) * T) / (v6 * Math.Sqrt(T)) - v6 * Math.Sqrt(T))) - MVc) / ((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v6 * v6 / 2.0) * T) / (v6 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v6 * v6 / 2.0) * T) / (v6 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)));
            double v8 = v7 - ((S * CND((Math.Log(S / X) + (r + v7 * v7 / 2.0) * T) / (v7 * Math.Sqrt(T))) - X * Math.Exp(-r * T) * CND((Math.Log(S / X) + (r + v7 * v7 / 2.0) * T) / (v7 * Math.Sqrt(T)) - v7 * Math.Sqrt(T))) - MVc) / ((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v7 * v7 / 2.0) * T) / (v7 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v7 * v7 / 2.0) * T) / (v7 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)));
            double v9 = v8 - ((S * CND((Math.Log(S / X) + (r + v8 * v8 / 2.0) * T) / (v8 * Math.Sqrt(T))) - X * Math.Exp(-r * T) * CND((Math.Log(S / X) + (r + v8 * v8 / 2.0) * T) / (v8 * Math.Sqrt(T)) - v8 * Math.Sqrt(T))) - MVc) / ((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v8 * v8 / 2.0) * T) / (v8 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v8 * v8 / 2.0) * T) / (v8 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)));
            double v10 = v9 - ((S * CND((Math.Log(S / X) + (r + v9 * v9 / 2.0) * T) / (v9 * Math.Sqrt(T))) - X * Math.Exp(-r * T) * CND((Math.Log(S / X) + (r + v9 * v9 / 2.0) * T) / (v9 * Math.Sqrt(T)) - v9 * Math.Sqrt(T))) - MVc) / ((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v9 * v9 / 2.0) * T) / (v9 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v9 * v9 / 2.0) * T) / (v9 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)));

            return v10;    
        }

        private double newtonp(double S, double X, double r, double T, double MVp)
        {
            double v = 0.5;
            double v1 = 0.5 - (((X * Math.Exp(-r * T) * CND(-((Math.Log(S / X) + (r + v * v / 2.0) * T) / (v * Math.Sqrt(T)) - v * Math.Sqrt(T))) - S * CND(-((Math.Log(S / X) + (r + v * v / 2.0) * T) / (v * Math.Sqrt(T))))) - MVp) / (((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v * v / 2.0) * T) / (v * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v * v / 2.0) * T) / (v * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)))));
            double v2 = v1 - (((X * Math.Exp(-r * T) * CND(-((Math.Log(S / X) + (r + v1 * v1 / 2.0) * T) / (v1 * Math.Sqrt(T)) - v1 * Math.Sqrt(T))) - S * CND(-((Math.Log(S / X) + (r + v1 * v1 / 2.0) * T) / (v1 * Math.Sqrt(T))))) - MVp) / (((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v1 * v1 / 2.0) * T) / (v1 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v1 * v1 / 2.0) * T) / (v1 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)))));
            double v3 = v2 - (((X * Math.Exp(-r * T) * CND(-((Math.Log(S / X) + (r + v2 * v2 / 2.0) * T) / (v2 * Math.Sqrt(T)) - v2 * Math.Sqrt(T))) - S * CND(-((Math.Log(S / X) + (r + v2 * v2 / 2.0) * T) / (v2 * Math.Sqrt(T))))) - MVp) / (((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v2 * v2 / 2.0) * T) / (v2 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v2 * v2 / 2.0) * T) / (v2 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)))));
            double v4 = v3 - (((X * Math.Exp(-r * T) * CND(-((Math.Log(S / X) + (r + v3 * v3 / 2.0) * T) / (v3 * Math.Sqrt(T)) - v3 * Math.Sqrt(T))) - S * CND(-((Math.Log(S / X) + (r + v3 * v3 / 2.0) * T) / (v3 * Math.Sqrt(T))))) - MVp) / (((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v3 * v3 / 2.0) * T) / (v3 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v3 * v3 / 2.0) * T) / (v3 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)))));
            double v5 = v4 - (((X * Math.Exp(-r * T) * CND(-((Math.Log(S / X) + (r + v4 * v4 / 2.0) * T) / (v4 * Math.Sqrt(T)) - v4 * Math.Sqrt(T))) - S * CND(-((Math.Log(S / X) + (r + v4 * v4 / 2.0) * T) / (v4 * Math.Sqrt(T))))) - MVp) / (((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v4 * v4 / 2.0) * T) / (v4 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v4 * v4 / 2.0) * T) / (v4 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)))));
            double v6 = v5 - (((X * Math.Exp(-r * T) * CND(-((Math.Log(S / X) + (r + v5 * v5 / 2.0) * T) / (v5 * Math.Sqrt(T)) - v5 * Math.Sqrt(T))) - S * CND(-((Math.Log(S / X) + (r + v5 * v5 / 2.0) * T) / (v5 * Math.Sqrt(T))))) - MVp) / (((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v5 * v5 / 2.0) * T) / (v5 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v5 * v5 / 2.0) * T) / (v5 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)))));
            double v7 = v6 - (((X * Math.Exp(-r * T) * CND(-((Math.Log(S / X) + (r + v6 * v6 / 2.0) * T) / (v6 * Math.Sqrt(T)) - v6 * Math.Sqrt(T))) - S * CND(-((Math.Log(S / X) + (r + v6 * v6 / 2.0) * T) / (v6 * Math.Sqrt(T))))) - MVp) / (((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v6 * v6 / 2.0) * T) / (v6 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v6 * v6 / 2.0) * T) / (v6 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)))));
            double v8 = v7 - (((X * Math.Exp(-r * T) * CND(-((Math.Log(S / X) + (r + v7 * v7 / 2.0) * T) / (v7 * Math.Sqrt(T)) - v7 * Math.Sqrt(T))) - S * CND(-((Math.Log(S / X) + (r + v7 * v7 / 2.0) * T) / (v7 * Math.Sqrt(T))))) - MVp) / (((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v7 * v7 / 2.0) * T) / (v7 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v7 * v7 / 2.0) * T) / (v7 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)))));
            double v9 = v8 - (((X * Math.Exp(-r * T) * CND(-((Math.Log(S / X) + (r + v8 * v8 / 2.0) * T) / (v8 * Math.Sqrt(T)) - v8 * Math.Sqrt(T))) - S * CND(-((Math.Log(S / X) + (r + v8 * v8 / 2.0) * T) / (v8 * Math.Sqrt(T))))) - MVp) / (((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v8 * v8 / 2.0) * T) / (v8 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v8 * v8 / 2.0) * T) / (v8 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)))));
            double v10 = v9 - (((X * Math.Exp(-r * T) * CND(-((Math.Log(S / X) + (r + v9 * v9 / 2.0) * T) / (v9 * Math.Sqrt(T)) - v9 * Math.Sqrt(T))) - S * CND(-((Math.Log(S / X) + (r + v9 * v9 / 2.0) * T) / (v9 * Math.Sqrt(T))))) - MVp) / (((S * Math.Sqrt(T) * (Math.Exp((-0.5 * (Math.Log(S / X) + (r + v9 * v9 / 2.0) * T) / (v9 * Math.Sqrt(T)) * (Math.Log(S / X) + (r + v9 * v9 / 2.0) * T) / (v9 * Math.Sqrt(T)))))) / (Math.Sqrt(2 * Math.PI)))));

            return v10;
        }


    }
}
