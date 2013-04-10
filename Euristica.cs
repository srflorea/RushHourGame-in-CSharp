using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RushHourGame
{
    public class Euristica
    {

        /// <summary>
        /// Metoda ce calculeaza distanta pana la iesire adunand mai multe rezultate: nuamrul de masini ce se afla intre masina rosie si iesire,
        /// distanta Manhattan a masinii rosii si iesire,
        /// numarul de masini ce se pot misca la un moment dat.
        /// </summary>
        /// <param name="parcare"></param>
        /// <returns></returns>
        public int distantaPanaLaIesire(Parcare parcare)
        {
            int hScor;
            Masina masinaRosie = null;
            foreach (Masina masina in parcare.Masini)
            {
                if (!masina.EsteRosie)
                    continue;
                masinaRosie = masina;
                break;
            }

            int distanta = 0;
            int posYIesire;
            int posXIesire; 

            if (masinaRosie.TipMasina == TipMasinaEnum.MasinaOrizontala)
            {
                posYIesire = parcare.PozitieCaracterIesire - masinaRosie.PozitieX * (parcare.LatimeParcare - 1);
                posXIesire = masinaRosie.PozitieX;
                if (posYIesire == 0)
                    for (int i = masinaRosie.PozitieY - 1; i > 0; i--)
                    {
                        if (parcare.OcupareParcare[masinaRosie.PozitieX, i] == true)
                            distanta++;
                    }
                else if (posYIesire == parcare.LatimeParcare - 1)
                    for (int i = masinaRosie.PozitieY + masinaRosie.Lungime; i < parcare.LatimeParcare - 1; i++)
                        if (parcare.OcupareParcare[masinaRosie.PozitieX, i] == true)
                            distanta++;
            }
            else
            {
                posYIesire = parcare.PozitieCaracterIesire % (parcare.LungimeParcare - 1);
                posXIesire = (parcare.PozitieCaracterIesire - posYIesire) / parcare.LatimeParcare;
                if (posXIesire == 0)
                    for (int i = masinaRosie.PozitieX - 1; i > 0; i--)
                    {
                        if (parcare.OcupareParcare[i, masinaRosie.PozitieY] == true)
                            distanta++;
                    }
                else if (posXIesire == parcare.LatimeParcare - 1)
                    for (int i = masinaRosie.PozitieX + masinaRosie.Lungime; i < parcare.LatimeParcare - 1; i++)
                        if (parcare.OcupareParcare[i, masinaRosie.PozitieY] == true)
                            distanta++;
            }
         //   if (distanta <= 1)
          //      return distanta;

            hScor = 10 * distanta + parcare.Masini.Count - masiniLibere(parcare) + distantaMahattan(masinaRosie, posXIesire, posYIesire);
            return hScor;
        }

        private int masiniLibere(Parcare parcare)
        {
            int nrMasiniLibere = 0;
            foreach (Masina masina in parcare.Masini)
            {
                if(masina.TipMasina == TipMasinaEnum.MasinaOrizontala)
                {
                    if(parcare.OcupareParcare[masina.PozitieX, masina.PozitieY - 1] == false || parcare.OcupareParcare[masina.PozitieX, masina.PozitieY + masina.Lungime] == false)
                    {
                        nrMasiniLibere++;
                    }
                }

                else if (masina.TipMasina == TipMasinaEnum.MasinaVerticala)
                {
                    if (parcare.OcupareParcare[masina.PozitieX - 1, masina.PozitieY] == false || parcare.OcupareParcare[masina.PozitieX + masina.Lungime, masina.PozitieY] == false)
                    {
                        nrMasiniLibere++;
                    }
                }
            }
            return nrMasiniLibere;
        }

        private bool contains(Dictionary<Parcare, int> parcariCunoscute, Parcare parcareNoua, out int index)
        {
            for (int i = 0; i < parcariCunoscute.Keys.Count; i++)
            {
                Parcare parcare = parcariCunoscute.Keys.ElementAt(i);
                if (parcare.Equals(parcareNoua))
                {
                    index = i;
                    return true;
                }
            }
            index = -1;
            return false;
        }

        public int distantaMahattan(Masina masinaRosie, int posXIesire, int posYIesire)
        {
            return 10 * (
                            Math.Abs(masinaRosie.PozitieX - masinaRosie.PozitieY)
                            + Math.Abs(posXIesire - posYIesire)
            );
        }

    }
}
