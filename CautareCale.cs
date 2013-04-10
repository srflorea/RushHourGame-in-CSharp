using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RushHourGame
{
    public class CautareCale
    {
        public RezultatCautare cautare(Parcare parcareInitiala)
        {
            Parcare parcareCurenta;
            Queue<Parcare> coadaParcari = new Queue<Parcare>();
            coadaParcari.Enqueue(parcareInitiala);

            while (coadaParcari.Count != 0)
            {
                parcareCurenta = coadaParcari.Dequeue();
                List<Mutare> mutari = parcareCurenta.mutariPosibile();
                foreach (Mutare mutare in mutari)
                {
                    Parcare parcareNoua = parcareCurenta.aplicaMutare(mutare);
                   // parcareNoua.afisare();
                   // parcareNoua.afisareOcupareParcare();
                    if (!contains(coadaParcari, parcareNoua))
                    {
                        Console.WriteLine("nu");
                        if (parcareNoua.SfarsitJoc)
                        {
                            Console.WriteLine("Jocul s-a incheiat cu succes!!!");
                            return refacereTraseu(parcareNoua);
                        }
                        coadaParcari.Enqueue(parcareNoua);
                    }
                    Console.WriteLine("contine");
                }
            }
            return new RezultatCautare();
        }

        private RezultatCautare refacereTraseu(Parcare parcare)
        {
            List<Parcare> parcariPartiale = new List<Parcare>();
            parcariPartiale.Add(parcare);
            int nrParcariPartiale = 1;
            while (parcare.ParcarePrecedenta != null)
            {
                parcare = parcare.ParcarePrecedenta;
                parcariPartiale.Add(parcare);
                nrParcariPartiale++;
            }
            return new RezultatCautare(parcariPartiale, nrParcariPartiale);
        }

        private bool contains(Queue<Parcare> parcari, Parcare parcareNoua)
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
