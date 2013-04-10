using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RushHourGame
{
    /// <summary>
    /// Clasa ce reprezinta implementarea proprie a unei liste ce mentine elementele sortate.
    /// </summary>
    public class Set
    {
        public List<Parcare> sortedList = new List<Parcare>();

        /// <summary>
        /// Metoda ce adauga elemente in lista astfel incat aceasta sa ramana sortata. Asta se realizeaza printr-o cautare binara, metoda ce este implementata in framework.
        /// </summary>
        /// <param name="parcare">Parcare ce va fi adaugata in lista sortata.</param>
        public void Add(Parcare parcare)
        {
            if (sortedList.Count == 0)
            {
                sortedList.Add(parcare);
                return;
            }
            int maxScor = sortedList[sortedList.Count - 1].GScor + sortedList[sortedList.Count - 1].HScor;
            if (maxScor <= parcare.GScor + parcare.HScor)
            {
                sortedList.Add(parcare);
                return;
            }
            int index = sortedList.BinarySearch(parcare);
            sortedList.Insert(Math.Abs(~index), parcare);
        }
    }
}
