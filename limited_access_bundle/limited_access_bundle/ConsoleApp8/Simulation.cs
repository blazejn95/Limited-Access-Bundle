//szortening na obslufdze NA TYM ROBIE OGR DOST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
namespace ConsoleApp8
{
    class Simulation
    {
        public static double Time;
        public static SortedList<Double,MyEvent> Flow;
        public static List<int> ClassServices;
        public static List<int> ClassLoses;
        public double A;

        public Simulation(double A)
        {
            Time = 0;
            Flow = new SortedList<Double,MyEvent>();
            this.A = A;
        }

        public void start() 
        {
            List<int> AUsRequired = new List<int>();
            List<int> Capacities = new List<int>();
            List<DemandStream> Streams = new List<DemandStream>();
            List<int> Priorities = new List<int>();

            String line;
            StreamReader sr = new StreamReader("inputA.txt");
            while((line=sr.ReadLine() )!=null)
            {        
                Capacities.Add(Convert.ToInt32(line));
            }

            StreamReader sr2 = new StreamReader("inputB.txt");
            while ((line = sr2.ReadLine()) != null)
            {
 
                AUsRequired.Add(Convert.ToInt32(line));
 
            }

            SystemBox S = new SystemBox(Capacities);

            for (int i = 0; i < Capacities.Count ; i++)
            {
                S.Tracker.Add(new List<List<Action>>());
            }

            StreamReader sr3 = new StreamReader("prios.txt");
            while ((line = sr3.ReadLine()) != null)
            {
                Priorities.Add(Convert.ToInt32(line));
                for (int z = 0; z < S.Tracker.Count; z++)
                {
                    S.Tracker[z].Add(new List<Action>());
                }
            }


            List<double> Intensities = new List<double>();
            int TotalCapacity = 0;
            for (int i = 0; i < Capacities.Count ; i++)
            {
                TotalCapacity += Capacities[i];
            }
            for (int i =0;i<AUsRequired.Count();i++)
            {
               double a_i = this.A * TotalCapacity / AUsRequired[i] / AUsRequired.Count();
                Intensities.Add(a_i);
            }
      
                for(int j =0; j< AUsRequired.Count(); j++)
                {
                    Streams.Add(new DemandStream(AUsRequired[ j], Intensities[j],Priorities[j],AUsRequired[j]));  //tablica z lvlami kompresji
                }
            
            // Groups[0].Members[0].A = Groups[0].Members[0].A * 10;
          //  Groups[0].Members[0].R.lambda = 1.05;// Groups[0].Members[0].R.lambda *4/3;
          // Groups[0].Members[1].R.lambda = Groups[0].Members[1].R.lambda *2;
          // Groups[0].Members[2].R.lambda = Groups[0].Members[2].R.lambda / 3;

            ClassServices = new List<int>(new int[AUsRequired.Count()]);
            ClassLoses = new List<int>(new int[AUsRequired.Count()]);


            bool Stabilized = false;
            Random Indexer=new Random();
            while (Simulation.Time<10000)// do miliona najrzadszych OBSŁG
            {
      
                if (Simulation.Time > 300&& !Stabilized)
                {
                    Stabilized = true;
                    for (int i = 0; i < ClassServices.Count(); i++)
                    {
                        ClassServices[i] = 0;
                        ClassLoses[i] = 0;
                    }
                }
                MyEvent Earliest = Simulation.Flow.Values[0];
                Flow.RemoveAt(0);
                Simulation.Time = Earliest.Time;
                switch (Earliest.A.Name)
                {
                    case "Demand":
                        int Free = 1;
                        int Range = S.V.Count;
                        List<int> Options = new List<int>();
                        for (int s = 0; s < S.V.Count; s++)
                        {
                            Options.Add(s);
                        }
                        while (Options.Count != 0)
                        {
                            int SubBundle = Indexer.Next() % Range;
                            if (S.CurrentV[Options[SubBundle]] + Earliest.A.AU <= S.V[Options[SubBundle]])
                            {
                                S.CurrentV[Options[SubBundle]] = S.CurrentV[Options[SubBundle]] + Earliest.A.AU;
                                S.Tracker[Options[SubBundle]][Earliest.A.Prio].Add(Earliest.A);
                                Earliest.A.SubBundleId = Options[SubBundle];
                                Earliest.A.EndPoint.SubBundleId = Options[SubBundle];
                                break;
                            }
                            if (Options.Count == 1)
                            {
                                Free = 0;
                                break;
                            }

                            else
                            {
                                Options.RemoveAt(SubBundle);
                                Range--;
                            }
                        }
                        //for (int i = 0; i < S.V.Count; i++)
                        //{
                        //    if (S.CurrentV[i] + first.D.comprlvl[0] <= S.V[i])
                        //    {
                        //        S.CurrentV[i] = S.CurrentV[i] + first.D.comprlvl[0];
                        //        S.priotracker[i][first.D.Prio].Add(first.D);
                        //        first.D.fid = i;
                        //        first.D.endpoint.fid = i;
                        //        break;
                        //    }
                        //    if (i == S.V.Count - 1)
                        //        free = 0;
                        //}


                        if (Free==0) 
                        {
                            bool SufficientAUs = false;
                            for (int InspectedSubBundle = 0; InspectedSubBundle < S.V.Count; InspectedSubBundle++)  
                            {
                                List<Action> ToDelete;
                                 ToDelete = new List<Action>();
                                Random Killer = new Random();
                                double AUsExtracted = 0;

                                for (int InspectedPriority = S.Tracker[InspectedSubBundle].Count() - 1; InspectedPriority > Earliest.A.Prio; InspectedPriority--) ///szukaj po nizszych prio od dolu
                                {
                                    SufficientAUs = false;
                                    for (int InspectedEntry = 0; InspectedEntry < S.Tracker[InspectedSubBundle][InspectedPriority].Count(); InspectedEntry++)///przelec po i-tym nizszym prio
                                    {
                                        int DelIndex = Killer.Next() % S.Tracker[InspectedSubBundle][InspectedPriority].Count();
                                        ToDelete.Add(S.Tracker[InspectedSubBundle][InspectedPriority][DelIndex]);
                                        AUsExtracted = AUsExtracted + S.Tracker[InspectedSubBundle][InspectedPriority][DelIndex].AU;
                                        S.Tracker[InspectedSubBundle][InspectedPriority].RemoveAt(DelIndex);
                                        InspectedEntry--;
                                        if (AUsExtracted + S.V[InspectedSubBundle] - S.CurrentV[InspectedSubBundle] >= Earliest.A.AU) //dodawaj sizey do czasu az sum nie starczy zeby sie zmiescilo ,modyfikuj todelete, jak jest to wylot z petli
                                        {

                                            SufficientAUs = true;
                                            break;
                                        }
                                    }
                                    if (SufficientAUs == true) break;
                                }

                                if (SufficientAUs == true)
                                {
                                    S.CurrentV[InspectedSubBundle] = S.CurrentV[InspectedSubBundle] - AUsExtracted;

                                    for (int EntryToDelete = 0; EntryToDelete < ToDelete.Count(); EntryToDelete++)
                                    {


                                        for (int Event = 0; Event < Flow.Count(); Event++)
                                        {
                                            if (Object.ReferenceEquals(ToDelete[EntryToDelete].EndPoint, Flow.Values[Event].A))
                                            {
                                                Flow.RemoveAt(Event); 
                                            }
                                        }
                                        for (int Stream = 0; Stream < Streams.Count; Stream++)
                                        {
                                            if (ToDelete[EntryToDelete].SourceEvent.Source == Streams[Stream])
                                                ClassLoses[Stream]++;
                                        }
                                    }

                                    if (S.CurrentV[InspectedSubBundle] + Earliest.A.AU <= S.V[InspectedSubBundle])
                                    {

                                        S.CurrentV[InspectedSubBundle] = S.CurrentV[InspectedSubBundle] + Earliest.A.AU;
                                        S.Tracker[InspectedSubBundle][Earliest.A.Prio].Add(Earliest.A);
                                        Earliest.A.SubBundleId = InspectedSubBundle;
                                        Earliest.A.EndPoint.SubBundleId = InspectedSubBundle;
                                    }

                                }
                                else//rollback
                                {
                                    for (int RollbackEntry = 0; RollbackEntry < ToDelete.Count; RollbackEntry++)
                                    {
                                        S.Tracker[InspectedSubBundle][ToDelete[RollbackEntry].Prio].Add(ToDelete[RollbackEntry]);
                                    }
                                }
                                if (SufficientAUs == true) break;

                            }
                            //////TUTAJ SKONCZYLEM
                                if (SufficientAUs == false)
                                {
                                for (int u = 0; u < Streams.Count; u++)
                                {
                                    if (Earliest.Source == Streams[u])
                                        ClassLoses[u]++;
                                }
                                for (int i = 0; i < Flow.Count(); i++)
                                    {
                                        if (Object.ReferenceEquals(Earliest.EndPoint, Flow.Values[i]))
                                        {
                                            Flow.RemoveAt(i); 
                                        }
                                    }
                                }

                            
                        }
                        Earliest.Source.Go(S);
                        break;
                    case "Service":   

                        for (int u = 0; u < Streams.Count; u++)
                        {
                            if (Earliest.Source == Streams[u])
                                ClassServices[u]++;
                        }                  
                        for (int p = 0; p < S.Tracker[Earliest.A.EndPoint.SubBundleId][Earliest.A.Prio].Count(); p++)
                        {
                            if (Object.ReferenceEquals(S.Tracker[Earliest.A.EndPoint.SubBundleId][Earliest.A.Prio][p], Earliest.A.EndPoint))
                            {
                                S.CurrentV[Earliest.A.EndPoint.SubBundleId] -= S.Tracker[Earliest.A.EndPoint.SubBundleId][Earliest.A.Prio][p].AU;
                                S.Tracker[Earliest.A.EndPoint.SubBundleId][Earliest.A.Prio].RemoveAt(p);

                            }
                        }
                        break;
                }
            }
        }
    }
}