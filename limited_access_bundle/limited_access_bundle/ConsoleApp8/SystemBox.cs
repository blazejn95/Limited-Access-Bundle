using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp8
{
    class SystemBox
    {
        public List< int> V;
        public List<double> CurrentV = new List<double>();
        public List< List<List<Action>> >Tracker = new List<List<List<Action>>>();
        public SystemBox(List<int> V)
        {
            this.V = V;
            for (int i = 0; i < V.Count; i++)
                CurrentV.Add(0) ;
        }

    }
}
