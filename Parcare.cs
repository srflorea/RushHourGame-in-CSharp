using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace RushHourGame
{
    /// <summary>
    /// Clasa ce reprezinta o anumita stare a jocului cu toate caracteristicile acesteia. Tot aici sunt implementate metodele de obtinere de posbile mutari din aceasta stare si de aplicare 
    /// a unei mutari.
    /// </summary>
    public class Parcare : ICloneable, IComparer<Parcare>, IComparable<Parcare>
    {
        private List<Masina> masini;
        private bool[,] ocupareParcare;
        private int lungimeParcare;
        private int latimeParcare;
        private int pozitieCaracterIesire;
        private bool sfarsitJoc;

        private Parcare parcarePrecedenta;

        private int gScor;
        private int hScor;

        public static readonly char caracterInceputMasinaOrizontala = '<';
        public static readonly char caracterSfarsitMasinaOrizontala = '>';
        public static readonly char caracterInceputMasinaVerticala  = '^';
        public static readonly char caracterSfarsitMasinaVerticala  = 'v';
        public static readonly char caracterMasinaRosie             = '?';
        public static readonly char caracterZidParcare              = '0';
        public static readonly char caracterIesireParcare           = '*';


        internal List<Masina> Masini
        {
            get { return masini; }
        }

        public bool[,] OcupareParcare
        {
            get { return ocupareParcare; }
        }

        public int LungimeParcare
        {
          get { return lungimeParcare; }
        }        

        public int LatimeParcare
        {
          get { return latimeParcare; }
        }

        public Parcare ParcarePrecedenta
        {
            get { return parcarePrecedenta; }
            set { parcarePrecedenta = value; }
        }

        public int PozitieCaracterIesire
        {
            get { return pozitieCaracterIesire; }
        }

        public bool SfarsitJoc
        {
            get { return sfarsitJoc; }
            set { sfarsitJoc = value; }
        }

        public int GScor
        {
            get { return gScor; }
            set { gScor = value; }
        }

        public int HScor
        {
            get { return hScor; }
            set { hScor = value; }
        }

        /// <summary>
        /// Cosntructor ce realizeaza intializarea unei parcari(de exmplu in moementul in care se cloneaza o parcare).
        /// </summary>
        /// <param name="masini">Lista cu masinile curente in parcare si pozitiile acesteia.</param>
        /// <param name="ocupareParcare">Matrice ce contine 'true' sau 'false' in functie de ocuparea respectivei pozitii in parcare.</param>
        /// <param name="lungimeParcare">Reprezinta lungimea parcarii citita din fisier.</param>
        /// <param name="latimeParcare">Reprezinta latimea parcarii citita din fisier.</param>
        /// <param name="parcarePrecedenta">Parametru ce reprezinta starea din care s-a ajuns aici. Ajuta la refacerea traseului pana la starea finala.</param>
        /// <param name="pozitieCaracterIesire">Reprezinta pozitia in parcare a pucntuluid e iesire.</param>
        /// <param name="gScor">Functia g ce este folosita in implementarea algoritmului de cautare eursitica A*.</param>
        /// <param name="hScor">Functia h ce este folosita in implementarea algoritmului de cautare eursitica A*.</param>
        public Parcare(List<Masina> masini, bool[,] ocupareParcare, int lungimeParcare, int latimeParcare, Parcare parcarePrecedenta, int pozitieCaracterIesire, int gScor, int hScor)
        {
            this.masini = masini;
            this.ocupareParcare = ocupareParcare;
            this.lungimeParcare = lungimeParcare;
            this.latimeParcare = latimeParcare;
            this.parcarePrecedenta = parcarePrecedenta;
            this.pozitieCaracterIesire = pozitieCaracterIesire;
            this.gScor = gScor;
            this.hScor = hScor;
        }

        /// <summary>
        /// Constructor ce este apelat dupa momentul ce se face citirea din fisier in debutul aplicatiei. Aici se initializeaza lungimea si latimea parcarii, se contruieste matricea de
        /// ocupare a parcarii si se populeaza lista cu masini.
        /// </summary>
        /// <param name="lines">Reprezinta liniile citite din fisier.</param>
        public Parcare(string[] lines)
        {
            sfarsitJoc = false;
            masini = new List<Masina>();
            ocupareParcare = new bool[8,8];
            lungimeParcare = int.Parse(lines[0].ElementAt(0).ToString());
            latimeParcare = int.Parse(lines[0].ElementAt(2).ToString());
            for (int i = 0; i < latimeParcare; i++)
                for (int j = 0; j < lungimeParcare; j++)
                    ocupareParcare[i,j] = false;
            int pozitieX;
            int pozitieY;
            for (int i = 2; i < lines.Length; i++)
            {
                pozitieX = i - 2;
                for (int j = 0; j < this.latimeParcare * 2; j += 2)
                {
                    pozitieY = j / 2;
                    TipMasinaEnum tipMasina;
                    int lungime;
                    bool esteRosie = false;
                    if (lines[i].ElementAt(j) == caracterMasinaRosie)
                    {
                        esteRosie = true;
                        if (lines[i].ElementAt(latimeParcare * 2 - 2) == caracterIesireParcare || lines[i].ElementAt(0) == caracterIesireParcare)
                        {
                            tipMasina = TipMasinaEnum.MasinaOrizontala;
                            if (lines[i].ElementAt(latimeParcare * 2 - 2) == caracterIesireParcare)
                                pozitieCaracterIesire = (i - 1) * (latimeParcare - 1);
                            else pozitieCaracterIesire = (i - 2) * (latimeParcare - 1);
                        }
                        else
                        {
                            tipMasina = TipMasinaEnum.MasinaVerticala;
                            pozitieCaracterIesire = (i - 2) * (latimeParcare - 1) + j / 2;
                        }
                    }
                    else if (lines[i].ElementAt(j) == caracterInceputMasinaOrizontala)
                    {
                        tipMasina = TipMasinaEnum.MasinaOrizontala;
                    }
                    else if (lines[i].ElementAt(j) == caracterInceputMasinaVerticala)
                    {
                        tipMasina = TipMasinaEnum.MasinaVerticala;
                    }
                    else continue;

                    lungime = 2;
                    if (tipMasina == TipMasinaEnum.MasinaOrizontala)
                    {
                        j += 2;
                        if (esteRosie)
                        {
                            lungime--;
                            while (lines[i].ElementAt(j) == caracterMasinaRosie)
                            {
                                j += 2;
                                lungime++;
                            }
                        }
                        else
                        {
                            while (lines[i].ElementAt(j) != caracterSfarsitMasinaOrizontala)
                            {
                                j += 2;
                                lungime++;
                            }
                        }
                    }
                    else
                    {
                        int k = i + 1;
                        if (esteRosie)
                        {
                            lungime--;
                            while (lines[k].ElementAt(j) == caracterMasinaRosie)
                            {
                                k++;
                                lungime++;
                            }
                        }
                        else
                        {
                            while (lines[k].ElementAt(j) != caracterSfarsitMasinaVerticala)
                            {
                                k++;
                                lungime++;
                            }
                        }
                    }

                    masini.Add(new Masina(tipMasina, pozitieX, pozitieY, lungime, esteRosie));
                }
            }
            foreach (Masina masina in Masini)
            {
                if (masina.TipMasina == TipMasinaEnum.MasinaOrizontala)
                {
                    for (int i = 0; i < masina.Lungime; i++)
                        ocupareParcare[masina.PozitieX, masina.PozitieY + i] = true;
                }
                else
                {
                    for (int i = 0; i < masina.Lungime; i++)
                    ocupareParcare[masina.PozitieX + i, masina.PozitieY] = true;                
                }
            }
        }

        /// <summary>
        /// Metoda ce contruieste un string ce va reprezenta o parcare la un moment dat.
        /// </summary>
        /// <returns>Intorce stringulc e reprezinta parcarea.</returns>
        public String afisare()
        {
            String str = "";
            String[,] parcare = construireParcare();
            for (int i = 0; i < latimeParcare; i++)
                str += "0 ";
            str += "\n";
            for (int i = 0; i < lungimeParcare - 2; i++)
            {
                str += "0 ";
                for (int j = 0; j < latimeParcare - 2; j++)
                {
                    str += parcare[i, j];
                    str += " ";
                }
                if ((i + 1) * (lungimeParcare - 1) + latimeParcare - 1 == pozitieCaracterIesire)
                    str += "*";
                else str += "0";
                str += "\n";
            }
            for (int i = 0; i < latimeParcare; i++)
                str += "0 ";
            str += "\n";
            return str;
        }

        /// <summary>
        /// Metoda ce construieste o matrice ce contine caracterele din interiorul parcarii pe baza listei de masini din parcarea curenta.
        /// </summary>
        /// <returns>Intoarce matricea de stringuri.</returns>
        private String[,] construireParcare()
        {
            String[,] rezultat = new String[lungimeParcare - 2, latimeParcare - 2];
            for (int i = 0; i < lungimeParcare - 2; i++)
                for (int j = 0; j < latimeParcare - 2; j++)
                    rezultat[i, j] = "-";
            foreach (Masina masina in masini)
            {
                if (masina.TipMasina == TipMasinaEnum.MasinaOrizontala)
                {
                    if (masina.EsteRosie)
                        for (int i = 0; i < masina.Lungime; i++)
                            rezultat[masina.PozitieX - 1, masina.PozitieY - 1 + i] = "?";
                    else
                    {
                        rezultat[masina.PozitieX - 1, masina.PozitieY - 1] = "<";
                        rezultat[masina.PozitieX - 1, masina.PozitieY - 1 + masina.Lungime - 1] = ">";
                    }
                }
                else
                {
                    if (masina.EsteRosie)
                        for (int i = 0; i < masina.Lungime; i++)
                            rezultat[masina.PozitieX - 1 + i, masina.PozitieY - 1] = "?";
                    else
                    {
                        rezultat[masina.PozitieX -1, masina.PozitieY - 1] = "^";
                        for (int i = 1; i < masina.Lungime - 1; i++)
                            rezultat[masina.PozitieX - 1 + i, masina.PozitieY - 1] = "|";
                        rezultat[masina.PozitieX - 1 + masina.Lungime - 1, masina.PozitieY - 1] = "v";
                    }
                }
            }
            return rezultat;
        }

        /// <summary>
        /// Metoda ce calculeaza toata mutarile posibile pentru o parcare data.
        /// </summary>
        /// <returns>Intoarce o lista de mutari ce reprezinta toate posbilitatile de a muta masinile in parcare.</returns>
        public List<Mutare> mutariPosibile()
        {
            List<Mutare> mutari = new List<Mutare>();
            Mutare mutare;
            //luam fiecare masina si ii analizam posbilitatile de mutare
            foreach(Masina masina in masini)
            {
                int posX = masina.PozitieX;
                int posY = masina.PozitieY;
                int lungime = masina.Lungime;
                TipMasinaEnum tip = masina.TipMasina;

                //cazul in care masina este orizonatala analizam cazurile de mutare la dreapta si la stanga 
                if (tip == TipMasinaEnum.MasinaOrizontala)
                {
                    for (int i = posY + lungime; i < latimeParcare - 1; i++)
                    {
                        if (ocupareParcare[posX, i] == false)
                        {
                            mutare = new Mutare((Masina)masina.Clone(), posX, i - lungime + 1);
                            mutari.Add(mutare);
                        }
                        else break;
                    }
                    for (int i = posY - 1; i > 0; i--)
                    {
                        if (ocupareParcare[posX, i] == false)
                        {
                            mutare = new Mutare((Masina)masina.Clone(), posX, i);
                            mutari.Add(mutare);
                        }
                        else break;
                    }
                }
                //in cazul in care masina este verticala analizam cazurile de mutare in jos si in sus
                else if (tip == TipMasinaEnum.MasinaVerticala)
                {
                    for (int i = posX + lungime; i < lungimeParcare - 1; i++)
                    {
                        if (ocupareParcare[i, posY] == false)
                        {
                            mutare = new Mutare((Masina)masina.Clone(), i - lungime + 1, posY);
                            mutari.Add(mutare);
                        }
                        else break;
                    }
                    for (int i = posX - 1; i > 0; i--)
                    {
                        if (ocupareParcare[i, posY] == false)
                        {
                            mutare = new Mutare((Masina)masina.Clone(), i, posY);
                            mutari.Add(mutare);
                        }
                        else break;
                    }
                }
            }
            return mutari;
        }

        /// <summary>
        /// Aceasta metoda realizeaza o mutare pe parcarea curenta
        /// </summary>
        /// <param name="mutare">Reprezinta mutarea ce trebuie realizata. Acesta este un obiect ce contine masina ce urmeaza a fi mutata si pozitia in parcare unde trebuie sa fie mutata.</param>
        /// <returns>Metoda intoarce noua tabla actualizata cu mutarea realizata.</returns>
        public Parcare aplicaMutare(Mutare mutare)
        {
            Parcare parcareNoua = (Parcare)this.Clone();
           // parcareNoua.afisareOcupareParcare();
            parcareNoua.parcarePrecedenta = this;
            int indexMasina = indexOf(mutare.Masina);
            Masina masina = mutare.Masina;
            int nrDeplasari;
            //cazul in care avem o masina orizonatala
            if (masina.TipMasina == TipMasinaEnum.MasinaOrizontala)
            {
                nrDeplasari = Math.Abs(masina.PozitieY - mutare.PozitieY);
               // Console.WriteLine("nuamruld e deplasari este: {0} pentru posX: {1}, posY: {2}", nrDeplasari, masina.PozitieX, masina.PozitieY);
                
                //cazul in care trebuie mutata la stanga
                if (masina.PozitieY > mutare.PozitieY)
                {
                    for (int i = 0; i < nrDeplasari; i++)
                    {
                        parcareNoua.ocupareParcare[masina.PozitieX, masina.PozitieY - i - 1] = true;
                       // Console.WriteLine(masina.PozitieY + masina.Lungime + i - 1);
                        parcareNoua.ocupareParcare[masina.PozitieX, masina.PozitieY + masina.Lungime - i - 1] = false;
                    }
                }
                //cazul in care trebuie mutata la dreapta
                else if (masina.PozitieY < mutare.PozitieY)
                {
                    for (int i = 0; i < nrDeplasari; i++)
                    {
                        parcareNoua.ocupareParcare[masina.PozitieX, masina.PozitieY + masina.Lungime + i] = true;
                        parcareNoua.ocupareParcare[masina.PozitieX, masina.PozitieY + i] = false;
                    }
                }
                else throw new Exception("Mutarea nu este corecta");

                
            }
            //cazul in care masina este verticala
            else if (masina.TipMasina == TipMasinaEnum.MasinaVerticala)
            {
                nrDeplasari = Math.Abs(masina.PozitieX - mutare.PozitieX);

                //cazul in care trebuie mutata in sus
                if (masina.PozitieX > mutare.PozitieX)
                {
                    for (int i = 0; i < nrDeplasari; i++)
                    {
                        parcareNoua.ocupareParcare[masina.PozitieX - i - 1, masina.PozitieY] = true;
                        parcareNoua.ocupareParcare[masina.PozitieX + masina.Lungime - i - 1, masina.PozitieY] = false;
                    }
                }
                //cazul in care trebuie mutata in jos
                else if (masina.PozitieX < mutare.PozitieX)
                {
                    for (int i = 0; i < nrDeplasari; i++)
                    {
                        parcareNoua.ocupareParcare[masina.PozitieX + masina.Lungime + i, masina.PozitieY] = true;
                        parcareNoua.ocupareParcare[masina.PozitieX + i, masina.PozitieY] = false;
                    }
                }
                else throw new Exception("Mutarea nu este corecta");
            }

            //actualizam lista de masini cu masina pe noua pozitie
            parcareNoua.masini.RemoveAt(indexMasina);
            masina.PozitieX = mutare.PozitieX;
            masina.PozitieY = mutare.PozitieY;
            parcareNoua.masini.Insert(indexMasina, masina);

            if(mutareCastigatoare(mutare))
                parcareNoua.SfarsitJoc = true;

            return parcareNoua;
        }

        /// <summary>
        /// Metoda folosita pentru a sti daca o anumita mutare este castigatore sau nu.
        /// </summary>
        /// <param name="mutare">Reprezinta mutarea ce urmeaza a fi aplicata unei anumite stari a parcarii.</param>
        /// <returns>Intoarce 'true' sau 'false' in functie de rezultatul obtinut.</returns>
        private bool mutareCastigatoare(Mutare mutare)
        {
            Masina masina = mutare.Masina;
            if (!masina.EsteRosie)
                return false;
            if (masina.TipMasina == TipMasinaEnum.MasinaOrizontala)
            {
                if (mutare.PozitieX * (latimeParcare - 1) + mutare.PozitieY + masina.Lungime == pozitieCaracterIesire || mutare.PozitieX * (latimeParcare - 1) + mutare.PozitieY - 1 == pozitieCaracterIesire)
                    return true;
            }
            else
            {
                if (mutare.PozitieX * (latimeParcare - 1) + mutare.PozitieY + masina.Lungime + latimeParcare == pozitieCaracterIesire || mutare.PozitieX * (latimeParcare - 1) + mutare.PozitieY - latimeParcare == pozitieCaracterIesire)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Metoda ce realizeaza clonarea unei anumite parcari. Este folosita in functia aplicaMutare pentru a obitine noi configuratii ale parcarii.
        /// </summary>
        /// <returns>Intoarce obiectul clonat.</returns>
        public object Clone()
        {
            List<Masina> masini = cloneazaMasini();
            bool[,] ocupareParcare = (bool[,])this.ocupareParcare.Clone();
            Parcare parcare = new Parcare(masini, ocupareParcare, lungimeParcare, latimeParcare, parcarePrecedenta, pozitieCaracterIesire, gScor, hScor);
            return parcare;
        }

        /// <summary>
        /// Metoda auxiliara, folosita in metoda Clone(), ce realizeaza copierea listei de masini.
        /// </summary>
        /// <returns>Intoarce noua lista de masini.</returns>
        private List<Masina> cloneazaMasini()
        {
            List<Masina> masini = new List<Masina>();
            foreach (Masina masina in this.masini)
            {
                masini.Add((Masina)masina.Clone());
            }

            return masini;
        }

        /// <summary>
        /// Metoda ce suprascrie metoda Equals pentru a verifica egalitatea intre doua stari.
        /// </summary>
        /// <param name="parcare">Stare ce va fi comparat cu starea curenta.</param>
        /// <returns>Intorce valoarea de adevar a egalitatii.</returns>
        public bool Equals(Parcare parcare)
        {
            for (int i = 0; i < masini.Count; i++)
            {
                if(!masini.ElementAt(i).Equals(parcare.Masini.ElementAt(i)))
                    return false;
            }

            return EqualsOcupareParcare(parcare.OcupareParcare);
                //&& ((parcarePrecedenta == null && parcare.parcarePrecedenta == null) || parcarePrecedenta.Equals(parcare.parcarePrecedenta));
        }

        /// <summary>
        /// Metoda auxiliara ce verifica egalitatea a doua matrici de ocupare a parcarii.
        /// </summary>
        /// <param name="ocupareParcare">Matricea ce va fi comparata cu matricea din starea curenta.</param>
        /// <returns>Intoarce valorea de adevar a egalitatii.</returns>
        private bool EqualsOcupareParcare(bool[,] ocupareParcare)
        {
            for (int i = 0; i < lungimeParcare; i++)
                for (int j = 0; j < latimeParcare; j++)
                    if (this.ocupareParcare[i, j] != ocupareParcare[i, j])
                        return false;
            return true;
        }

        /// <summary>
        /// Metoda ce afla indexul unei anumite masini in lista de masini.
        /// </summary>
        /// <param name="masina">Reprezinta masina careia ii vrem idnexul.</param>
        /// <returns>Intoarce indexul in lista de masini.</returns>
        public int indexOf(Masina masina)
        {
            Masina masinaInLista = null;
            for (int i = 0; i < masini.Count; i++)
            {
                masinaInLista = masini.ElementAt(i);
                if (masinaInLista.PozitieX == masina.PozitieX && masinaInLista.PozitieY == masina.PozitieY)
                    return i;
            }

            throw new Exception("Masina nu exista in lista!!!!");
        }

        /// <summary>
        /// Metoda ce suprasrie metoda din interfata 'IComparer<Parcare>' pentru a compara doua stari in functie de scor.
        /// </summary>
        /// <param name="parcare1">Prima parcare ce va fi comparata.</param>
        /// <param name="parcare2">A doua parcare ce va fi comparata.</param>
        /// <returns></returns>
        public int Compare(Parcare parcare1, Parcare parcare2)
        {
            return parcare1.GScor + parcare1.HScor - parcare2.GScor - parcare2.HScor;
        }

        /// <summary>
        /// Metoda ce suprasrie metoda din interfata 'IComparable<Parcare>' pentru a compara doua stari in functie de scor.
        /// </summary>
        /// <param name="other">Parcarea ce va fi comparata cu parcarea curenta</param>
        /// <returns></returns>
        public int CompareTo(Parcare other)
        {
            return gScor + hScor - other.GScor - other.HScor;
        }
    }
}
