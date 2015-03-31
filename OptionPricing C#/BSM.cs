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
            double S = double.Parse(textBox1.Text);
            double X = double.Parse(textBox2.Text);
            double v = double.Parse(textBox3.Text);
            double r = double.Parse(textBox4.Text);
            double T = double.Parse(textBox5.Text) / 365;

            textBox7.AppendText(Math.Round(BSM("c", S, X, v, r, T),4).ToString());
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

        private double CND(double X)
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

        private double d1(double S, double X, double v, double r, double T)
        {
            double d1 = 0;
            d1 = (Math.Log(S / X) + (r + v * v / 2.0) * T) / (v * Math.Sqrt(T));
            return d1;
        }

        private double d2(double S, double X, double v, double r, double T)
        {
            double d2 = 0;
            d2 = (Math.Log(S / X) + (r + v * v / 2.0) * T) / (v * Math.Sqrt(T)) - v * Math.Sqrt(T);
            return d2;
        }

        private double dNd1(double S, double X, double v, double r, double T)
        {
            double dNd1 = 0;
            dNd1 = (1/Math.Sqrt(2*Math.PI))*Math.Exp(-(d1(S,X,v,r,T)*d1(S,X,v,r,T))/2);
            return dNd1;
        }

        private double dNd2(double S, double X, double v, double r, double T)
        {
            double dNd2 = 0;
            dNd2 = (1 / Math.Sqrt(2 * Math.PI)) * Math.Exp(-(d2(S, X, v, r, T) * d2(S, X, v, r, T)) / 2);
            return dNd2;
        }

        private double BSM(String Flag, double S, double X, double v, double r, double T)
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

        private double IV(String Flag, double S, double X, double v, double r, double T)
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
       
    }
}
