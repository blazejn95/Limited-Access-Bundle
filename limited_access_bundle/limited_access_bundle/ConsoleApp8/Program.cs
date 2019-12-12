using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp8
{
    class Program
    {
      static  double rown_v1(int x)
        {
            int a = 16807, q = 127773, r = 2836, h;
            h = (int)(x / q);
            x = a * (x - q * h) - r * h;
            if (x < 0) x = x + 2147483647;
            return(( double)x )/ 2147483647;
        }

       static double rown_v2(int x)
        {
            int a = 48271, q = 44488, r = 3399, h;
            h = (int)(x / q);
            x = a * (x - q * h) - r * h;
            if (x < 0) x = x + 2147483647;
            return ((double) x) / 2147483647;
        }

       static double rown_v3(int x)
        {
            int a = 69621, q = 30845, r = 23902, h;
            h = (int)(x / q);
            x = a * (x - q * h) - r * h;
            if (x < 0) x = x + 2147483647;
            return ((double)x) / 2147483647;
        }

       static double CzasObslugi(double obs, int x)
        {
            double w;
            w = Math.Log(rown_v2(x)) * (-obs);
            return w;
        }

       static double PojawienieZgloszenia(double lambda, int x)
        {
            double w;
            w = Math.Log(rown_v1(x)) * (-1 / lambda);
            return w;
        }
        static void rst()
        {
            Simulation.Flow = new SortedList<Double, MyEvent>();

            Simulation.Time = 0;
            for (int i = 0; i < Simulation.ClassServices.Count; i++)
            {
                Simulation.ClassLoses[i] = 0;
                Simulation.ClassServices[i] = 0;
            }

        }
        static void Main(string[] args)
        {

            double alpha = 0.05;
            double t = 2.4469;

            //////////////////////////////////
            double curr =0.95;
            double step = 0.1;
            double stop = 1.6;
            ///////////////////////////////////////////
            int lsymul = 7;
            double firstval = curr;
            for (; curr < stop; curr += step)
            {
                List<List<double>> xi = new List<List<double>>();
                for (int i = 0; i < lsymul; i++)
                {
                    xi.Add(new List<double>());
                    Simulation A = new Simulation(curr);
                    A.start();
                    for (int j = 0; j < Simulation.ClassLoses.Count(); j++)
                    {

                        xi[i].Add(Convert.ToDouble(Simulation.ClassLoses[j]) / (Simulation.ClassLoses[j] + Simulation.ClassServices[j]));
                    }

                    rst();

                }
                List<double> sum = new List<double>(new double[xi[0].Count()]);
                List<double> sumosq = new List<double>(new double[xi[0].Count()]);
                List<double> avg = new List<double>(new double[xi[0].Count()]);
                List<double> var = new List<double>(new double[xi[0].Count()]);
                for (int i = 0; i < xi.Count(); i++)
                {
                    for (int j = 0; j < xi[i].Count(); j++)
                    {
                        sum[j] += xi[i][j];
                        sumosq[j] += Math.Pow(xi[i][j], 2);
                    }
                }
                for (int i = 0; i < sum.Count(); i++)
                {
                    avg[i] = sum[i] / lsymul;
                    var[i] = (sumosq[i] / lsymul - Math.Pow(avg[i], 2));
                }



                List<Double> LB = new List<double>(new double[xi[0].Count()]);

                for (int i = 0; i < sum.Count(); i++)
                {
                    LB[i] = -(t * Math.Sqrt(var[i] / (lsymul-1))) + avg[i];
                }
                List<Double> trustspan = new List<double>(new double[xi[0].Count()]);
                for (int i = 0; i < sum.Count(); i++)
                {
                    trustspan[i] = avg[i] - LB[i];
                }



                List<String> others = new List<String>(); /// V,a,groups


                String line;
                StreamReader sr = new StreamReader("inputA.txt");

                while ((line = sr.ReadLine()) != null)
                {
                    others.Add(line);
                }


                string path = @"result.txt";

                // Create a file to write to.
                if (curr == firstval)
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        for (int i = 0; i < trustspan.Count(); i++)
                        {
                            sw.WriteLine(curr + " " + avg[i] + " " + trustspan[i]);
                        }

                    }
                }
                else {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        for (int i = 0; i < trustspan.Count(); i++)
                        {
                            sw.WriteLine(curr + " " + avg[i] + " " + trustspan[i]);
                        }

                    }
                }
            }
        }
    }
}
