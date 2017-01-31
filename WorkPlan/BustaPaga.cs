using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    public class BustaPaga : IComparable<BustaPaga>
    {
        public BustaPaga(BustaPaga rhs, int id)
        {
            Id = id;
            IdDip = rhs.IdDip;
            Mese = rhs.Mese;
            Anno = rhs.Anno;
            Importo = rhs.Importo;
        }

        public BustaPaga(int idDipendente, int mese, int anno, double importo)
        {
            IdDip = idDipendente;
            Mese = mese;
            Anno = anno;
            Importo = importo;
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

        public double Importo { get; set; }

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
