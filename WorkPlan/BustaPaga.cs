using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class VocePaga
    {
        public VocePaga(int id, string descrizione, double importo)
            :this(descrizione,importo)
        {
            Id = id;
        }

        public VocePaga(string descrizione, double importo)
        {
            Descrizione = descrizione;
            Importo = importo;
        }

        public int Id
        {
            get;
            private set;
        }

        public string Descrizione
        {
            get;
            private set;
        }

        public double Importo
        {
            get;
            private set;
        }
    }

    public class BustaPaga : IComparable<BustaPaga>
    {

        public BustaPaga(BustaPaga rhs, int id)
        {
            Id = id;
            IdDip = rhs.IdDip;
            Mese = rhs.Mese;
            Anno = rhs.Anno;
            Importo = rhs.Importo;
            Voci = rhs.Voci;
        }

        public BustaPaga(int idDipendente, int mese, int anno, double importo)
        {
            IdDip = idDipendente;
            Mese = mese;
            Anno = anno;
            Importo = importo;
            Voci = new List<VocePaga>();
        }

        public BustaPaga(int id, int idDipendente, int mese, int anno, double importo) 
            : this(idDipendente, mese, anno, importo)
        {
            Id = id;
        }

        public int Id { get; private set; }

        public int IdDip { get; private set; }

        public int Mese { get;set; }

        public int Anno { get; set; }

        public double Importo
        {
            get
            {
                double imp = 0;
                foreach (var vp in Voci)
                {
                    imp += vp.Importo;
                }

                return imp;
            }

            private set { }
        }

        void Aggiungi(string descrizione, double importo)
        {
            VocePaga vp = new VocePaga(descrizione, importo);
            Voci.Add(vp);
        }

        void Aggiungi(VocePaga voce)
        {
            Voci.Add(voce);
        }

        public List<VocePaga> Voci
        {
            get;
            private set;
        }


        public override string ToString()
        {
            string text;
            if (Mese <= 12)
            {
                DateTime dtrif = new DateTime(Anno, Mese, 1);
                text = dtrif.ToString("MMMM yyyy");
            }
            else if (Mese == 13)
            {
                text = "Tredicesima " + Anno.ToString();
            }
            else
            {
                text = "Quattordicesima " + Anno.ToString();
            }

            return text;
        }

        int IComparable<BustaPaga>.CompareTo(BustaPaga other)
        {
            if(this.Anno > other.Anno) return -1;
            if (this.Anno < other.Anno) return 1;
            
            if(this.Anno == other.Anno)
            {
                if (this.Mese > other.Mese) return -1;
                if (this.Mese < other.Mese) return 1;
            }

            return 0;
        }
    }
}
