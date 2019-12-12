using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp8
{
    class Action
    {
        public int Prio;
        public String Name;
        public double CreationTime;
        public Action EndPoint;
        public MyEvent Parent;
       public  int AU;
        public MyEvent SourceEvent;
        public int SubBundleId=-1;
       public Action(int Prio,  String Name,double CreationTime,int AU)
        {
            this.AU = AU;
            this.Prio = Prio;
            this.Name = Name;
            this.CreationTime = CreationTime;
        }
    }
}
