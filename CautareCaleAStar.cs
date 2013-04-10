using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RushHourGame
{
    /// <summary>
    /// Clasa ce contine implementarea algorimului de cautare euristica A*.
    /// </summary>
    public class CautareCaleAStar
    {
        Euristica euristica;

        /// <summary>
        /// Cosntructorul clasei.
        /// </summary>
        /// <param name="euristica">Reprezinta euristica ce o va folosi algoritmul A*.</param>
        public CautareCaleAStar(Euristica euristica)
        {
            this.euristica = euristica;
        }

        /// <summary>
        /// Reprezinta functia de cautare a caii spre iesire din parcare. Implementeaza algorimul A*.
        /// </summary>
        /// <param name="parcareInitiala">Reprezinta configuratia initiala a parcarii de la care se va incepe cautarea iesirii din parcare.</param>
        /// <returns>Intoarce un obiect ce va contine parcarile partiale si numarula acestora, care vor fi scrise in fisier.</returns>
        public RezultatCautare cautare(Parcare parcareInitiala)
        {
            int expandate = 1;
            Set open = new Set();
            List<Parcare> closed = new List<Parcare>();
            parcareInitiala.GScor = 0;
            parcareInitiala.HScor = euristica.distantaPanaLaIesire(parcareInitiala);
            open.Add(parcareInitiala);
            
            Parcare parcareBest = null;

            while (open.sortedList.Count != 0)
            {
                parcareBest = open.sortedList.ElementAt(0);
                if (parcareBest.SfarsitJoc)
                {
                    Console.WriteLine(expandate);
                    return refacereTraseu(parcareBest);
                }
                open.sortedList.Remove(parcareBest);
                closed.Add(parcareBest);
                List<Mutare> mutari = parcareBest.mutariPosibile();
                foreach (Mutare mutare in mutari)
                {
                    expandate++;
                    Parcare parcareNoua = parcareBest.aplicaMutare(mutare);
                    int gScorParcareNoua = parcareBest.GScor + 1;
                    int hScorParcareNoua = euristica.distantaPanaLaIesire(parcareNoua);
                    if (!contains(open.sortedList, parcareNoua) && !contains(closed, parcareNoua))
                    {
                        parcareNoua.GScor = gScorParcareNoua;
                        parcareNoua.HScor = hScorParcareNoua;
                        open.Add(parcareNoua);
                    }
                    else
                    {
                        if (gScorParcareNoua < parcareNoua.GScor)
                        {
                            parcareNoua.GScor = gScorParcareNoua;
                            parcareNoua.HScor = hScorParcareNoua;
                            if (contains(closed, parcareNoua))
                            {
                                closed.Remove(parcareNoua);
                                open.Add(parcareNoua);
                            }
                        }
                    }
                }
            }
            return new RezultatCautare();
        }

        /// <summary>
        /// Metoda ce va reface traseul catre starea intialaza, si pe care le va adauga intr-o lista.
        /// </summary>
        /// <param name="parcare"></param>
        /// <returns></returns>
        private RezultatCautare refacereTraseu(Parcare parcare)
        {
            List<Parcare> parcariPartiale = new List<Parcare>();
            parcariPartiale.Add(parcare);
            int nrParcariPartiale = 1;
            while (parcare.ParcarePrecedenta != null)
            {
                parcare = parcare.ParcarePrecedenta;
                parcariPartiale.Insert(0, parcare);
                nrParcariPartiale++;
            }
            return new RezultatCautare(parcariPartiale, nrParcariPartiale);
        }

        /// <summary>
        /// Metoda ce verifican apartenenta la o lista a unei anumite parcari. 
        /// </summary>
        /// <param name="parcari">Reprezinta lista de parcari inc are se va face cautarea.</param>
        /// <param name="parcareNoua">Reprezinta parcare ce se doreste a fi cautata.</param>
        /// <returns>Intorce valoarea de adevar a cautarii.</returns>
        private bool contains(List<Parcare> parcari, Parcare parcareNoua)
        {
            foreach (Parcare parcare in parcari)
            {
                if (parcare.Equals(parcareNoua))
                    return true;
            }
            return false;
        }
    }
}
