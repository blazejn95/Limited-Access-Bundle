using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp8
{
    class Generator
    {
        private Random Seeder;
       public double lambda;
        double u;
       public Generator(double lambda)
        {
            Seeder = new Random();
            Thread.Sleep(22);
            this.lambda = lambda;
            u = 1;

        }
        private double rown_v1(int x)
        {
            int a = 16807, q = 127773, r = 2836, h;
            h = (int)(x / q);
            x = a * (x - q * h) - r * h;
            if (x < 0) x = x + 2147483647;
            return ((double)x) / 2147483647;
        }

        private double rown_v2(int x)
        {
            int a = 48271, q = 44488, r = 3399, h;
            h = (int)(x / q);
            x = a * (x - q * h) - r * h;
            if (x < 0) x = x + 2147483647;
            return ((double)x) / 2147483647;
        }

        private double rown_v3(int x)
        {
            int a = 69621, q = 30845, r = 23902, h;
            h = (int)(x / q);
            x = a * (x - q * h) - r * h;
            if (x < 0) x = x + 2147483647;
            return ((double)x) / 2147483647;
        }

        private double CzasObslugi(double obs, int x)
        {
            double w;
            w = Math.Log(rown_v2(x)) * (-obs);
            return w;
        }

        private double PojawienieZgloszenia(double lambda, int x)
        {
            double w;
            w = Math.Log(rown_v1(x)) * (-1 / lambda);
            return w;
        }
        public List<double> GenerateTimes()
        {
            List<double> Pair = new List<double>();
            Pair.Add(PojawienieZgloszenia(this.lambda,this.Seeder.Next()));
            Pair.Add(CzasObslugi(this.u, this.Seeder.Next()));
            return Pair;
        }
    }
}
