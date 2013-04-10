using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RushHourGame
{
    /// <summary>
    /// Clasa ce reprezinta o masina in parcare si este caracterizata de tipul ei, pozitie, lungime si specificatia daca este rosie sau nu.
    /// </summary>
    public class Masina :ICloneable
    {
        private TipMasinaEnum tipMasina;
        private int pozitieX;
        private int pozitieY;
        private int lungime;
        private bool esteRosie;

        public TipMasinaEnum TipMasina
        {
            get { return tipMasina; }
        }

        public int PozitieX
        {
            get { return pozitieX; }
            set { pozitieX = value; }
        }

        public int PozitieY
        {
            get { return pozitieY; }
            set { pozitieY = value; }
        }

        public int Lungime
        {
            get { return lungime; }
        }

        public bool EsteRosie
        {
            get { return esteRosie; }
        }

        /// <summary>
        /// Constructor  ce initializeaza caracteristicile unei masini.
        /// </summary>
        /// <param name="tipMasina">Tipul masinii.</param>
        /// <param name="pozitieX">Pozitia masinii.</param>
        /// <param name="pozitieY">Pozitia masinii.</param>
        /// <param name="lungime">Lungimea masinii.</param>
        /// <param name="esteRosie">Este rosie sau nu?</param>
        public Masina(TipMasinaEnum tipMasina, int pozitieX, int pozitieY, int lungime, bool esteRosie)
        {
            this.tipMasina = tipMasina;
            this.pozitieX = pozitieX;
            this.pozitieY = pozitieY;
            this.lungime = lungime;
            this.esteRosie = esteRosie;
        }

        /// <summary>
        /// Metoda ce suprascrie metoda Clone() si realizeaza clonarea unei anumite masini.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Metoda ce suprascria metoda Equals pentru a compara doua masini.
        /// </summary>
        /// <param name="masina">Masina ce se doreste a fi comparata cu masina curenta.</param>
        /// <returns>Intoarce valoarea de adevat a egalitatii.</returns>
        public bool Equals(Masina masina)
        {
            return tipMasina == masina.TipMasina
                && pozitieX == masina.PozitieX
                && pozitieY == masina.PozitieY
                && lungime == masina.Lungime
                && esteRosie == masina.EsteRosie; ;
        }
    }
}
