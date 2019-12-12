using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp8
{
    class DemandStream
    {

        public double A;
        public Generator R;
        List<double> GeneratedTimes;
        public int Prio;
        public int AU;
        public DemandStream(double t, double A, int pr, int d)
        {
            AU = d;
            this.A= A;
            R = new Generator(A); //since u=1
            GeneratedTimes = new List<double>();
            GeneratedTimes=R.GenerateTimes();
            Prio = pr;
           MyEvent Demand= new MyEvent(Simulation.Time+GeneratedTimes[0], new Action(Prio,"Demand",Simulation.Time,d),this);
            MyEvent Service=new MyEvent(Simulation.Time +GeneratedTimes[0]+ GeneratedTimes[1], new Action(Prio, "Service", Simulation.Time,d), this);//KEYS (TIMES!!!!!!) CANT BE THE SAME 
            Demand.EndPoint = Service;
            Service.EndPoint = Demand;
            Demand.A.EndPoint = Service.A;
            Service.A.EndPoint = Demand.A;
            Demand.A.Parent = Demand;
            Service.A.Parent = Service;
            Demand.A.SourceEvent = Demand;
            Service.A.SourceEvent = Service;
        }
        public void Go(SystemBox s)
        {

            GeneratedTimes = R.GenerateTimes();
            while ((double.IsInfinity(GeneratedTimes[0]) || double.IsInfinity(GeneratedTimes[1])))
                GeneratedTimes = R.GenerateTimes();
            MyEvent a = new MyEvent(Simulation.Time + GeneratedTimes[0], new Action(Prio,  "Demand", Simulation.Time,AU), this);
            MyEvent b = new MyEvent(Simulation.Time + GeneratedTimes[0] + GeneratedTimes[1], new Action(Prio,  "Service", Simulation.Time,AU), this);
            a.EndPoint = b;
            b.EndPoint = a;
            a.A.EndPoint = b.A;
            b.A.EndPoint = a.A;
            a.A.Parent = a;
            b.A.Parent = b;
            a.A.SourceEvent = a;
            b.A.SourceEvent = b;
        }
    }
}
