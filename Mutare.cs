using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RushHourGame
{
    /// <summary>
    /// Clasa ce reprezinta o mutare si este caracterizata de o masina si noua pozitie pe care urmeaza a fi mutata.
    /// </summary>
    public class Mutare
    {
        private Masina masina;
        private int pozitieX;
        private int pozitieY;

        public Masina Masina
        {
            get { return masina; }
        }

        public int PozitieX
        {
            get { return pozitieX; }
        }

        public int PozitieY
        {
            get { return pozitieY; }
        }

        /// <summary>
        /// Constructor ce initializeaza caracteristicile unei mutari.
        /// </summary>
        /// <param name="masina">Masina ce urmeaza a fi mutata.</param>
        /// <param name="pozitieX">Pozitia pe care urmeaza a fi mutata.</param>
        /// <param name="pozitieY">Pozitia pe care urmeaza a fi mutata.</param>
        public Mutare(Masina masina, int pozitieX, int pozitieY)
        {
            this.masina = masina;
            this.pozitieX = pozitieX;
            this.pozitieY = pozitieY;
        }
    }
}
