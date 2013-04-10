using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RushHourGame
{
    /// <summary>
    /// Reprezinta clasa ce va retine rezultatul rezultatul in urma cautarii.
    /// </summary>
    public class RezultatCautare
    {
        private int nrParcariPartiale;
        private List<Parcare> parcariPartiale;

        /// <summary>
        /// Constructor fara parametrii ce anunta faptul ca parcarea nu are solutie.
        /// </summary>
        public RezultatCautare()
        {
            parcariPartiale = null;
            Console.WriteLine("Nu exista nici o posibilitate de iesire din parcare!!!!");
        }

        /// <summary>
        /// Constructor ce initializeaza rezultatele cautarii.
        /// </summary>
        /// <param name="parcariPartiale">Starile prin care s-a trecut pentru a se ajunge la starea scop.</param>
        /// <param name="nrParcariPartiale">Numarul starilor prin care s-a trecut pentru a se ajunge la starea scop.</param>
        public RezultatCautare(List<Parcare> parcariPartiale, int nrParcariPartiale)
        {
            this.nrParcariPartiale = nrParcariPartiale;
            this.parcariPartiale = parcariPartiale;
        }

        public int NrParcariPartiale
        {
            get { return nrParcariPartiale; }
            set { nrParcariPartiale = value; }
        }

        public List<Parcare> ParcariPartiale
        {
            get { return parcariPartiale; }
            set { parcariPartiale = value; }
        }
    }
}
