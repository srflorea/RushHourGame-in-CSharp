using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RushHourGame
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime now1 = DateTime.Now;

            string[] lines = System.IO.File.ReadAllLines("in.txt");
            Parcare parcare = new Parcare(lines);
            parcare.afisare();
            Console.WriteLine();

            Euristica euristica = new Euristica();
            CautareCaleAStar cautareCale = new CautareCaleAStar(euristica);

            RezultatCautare rezultatCautare = cautareCale.cautare(parcare);
            using (StreamWriter writer = new StreamWriter("out.txt"))
            {
                writer.WriteLine(rezultatCautare.NrParcariPartiale);
                writer.WriteLine();
                Console.WriteLine(rezultatCautare.ParcariPartiale.Count);
                foreach (Parcare parcarePartiala in rezultatCautare.ParcariPartiale)
                    writer.WriteLine(parcarePartiala.afisare());
            }

            DateTime now2 = DateTime.Now;
            Console.WriteLine(now2 - now1);
        }
    }
}
