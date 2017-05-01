using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkPlan
{
    // lista funzioni
    public enum Function
    {
        InserisciTurno,
        ModificaTurno,
        EliminaTurno,
        InserisciAssenza,
        ModificaAssenza,
        EliminaAssenza,
        VisualizzaDipendenti,
        Configurazione,
        StampaTurni,
        StampaResocontoMensile,
        VisualizzaTurniPassati
    }

    public abstract class Profile
    {
        public static Profile Create(string userProfile)
        {
            switch(userProfile)
            {
                case "A":
                    return new AdminProfile();
                default:
                    return new UserProfile();
            }
        }

        public abstract bool IsAuthorized(Function function);

        public virtual bool IsAdmin()
        {
            return false;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        //public abstract bool CanDelete(IShiftVM shift);
        //public abstract bool CanDelete(NoworkVM noworkVM);
        //public abstract bool CanDelete(DutyVM dutyVm);
        //public abstract bool CanEdit(NoworkVM noWorkVM);
        //public abstract bool CanEdit(DutyVM dutyVM);
    }
    
    public class UserProfile : Profile
    {
        List<Function> authorizedFunctions;

        public UserProfile()
        {
            authorizedFunctions=new List<Function>()
            {
                Function.InserisciTurno,
                Function.InserisciAssenza,
                Function.StampaTurni
            };
        }

        public override bool IsAuthorized(Function function)
        {
            return authorizedFunctions.Exists(x => x.Equals(function));
        }

        public override string ToString()
        {
            return "UTENTE";
        }

        //public override bool CanDelete(IShiftVM shift)
        //{
        //    return noworkVM.Reason.Code.Equals("GEN");
        //}

        //public override bool CanDelete(DutyVM dutyVm)
        //{
        //    return false;
        //}

        //public override bool CanEdit(NoworkVM noworkVM)
        //{
        //    return noworkVM.Reason.Code.Equals("GEN");
        //}

        //public override bool CanEdit(DutyVM dutyVM)
        //{
        //    return false;
        //}
    }

    public class AdminProfile : Profile
    {
        public override bool IsAuthorized(Function function)
        {
            // l'admin è sempre autorizzato
            return true;
        }

        public override bool IsAdmin()
        {
            return true;
        }

        public override string ToString()
        {
            return "AMMINISTRATORE";
        }

        //public override bool CanDelete(NoworkVM noworkVM)
        //{
        //    return true;
        //}

        //public override bool CanDelete(DutyVM dutyVm)
        //{
        //    return true;
        //}

        //public override bool CanEdit(NoworkVM noworkVM)
        //{
        //    return true;
        //}

        //public override bool CanEdit(DutyVM dutyVM)
        //{
        //    return true;
        //}
    }
}
