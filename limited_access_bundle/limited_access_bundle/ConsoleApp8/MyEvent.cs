using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp8
{
    class MyEvent
    {
        public double Time;
        public Action A;
       public DemandStream Source;
        public double CreationTime;
       public MyEvent EndPoint;
        public MyEvent(double Time, Action A, DemandStream Source)
        {
            this.Time = Time;
            this.A = A;
            this.Source = Source;
                Simulation.Flow.Add(this.Time, this);

            this.CreationTime = Simulation.Time;
        }
    }
}
