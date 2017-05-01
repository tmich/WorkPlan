using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WorkPlan
{
    public class PeriodService
    {
        protected string connstr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
        private EmployeeRepository employeeRepository;
        private DutyRepository dutyRepository;
        private NoWorkRepository noworkRepository;

        public PeriodService()
        {
            employeeRepository = new EmployeeRepository();
            dutyRepository = new DutyRepository();
            noworkRepository = new NoWorkRepository();
        }

        public Dictionary<Employee, List<IShiftVM>> GetByDateRangeDict(DateTime startDate, DateTime endDate)
        {
            var results = new Dictionary<Employee, List<IShiftVM>>();
            var employees = employeeRepository.All();
            
            foreach (var employee in employees)
            {
                if (!results.ContainsKey(employee))
                {
                    results.Add(employee, new List<IShiftVM>());
                }

                foreach (var duty in dutyRepository.GetBy(employee, startDate, endDate))
                {
                    DutyVM dutyVM = new DutyVM(duty);
                    results[employee].Add(dutyVM);
                }

                foreach (var nowork in noworkRepository.GetAssenzeByDipDateRange(employee, startDate, endDate))
                {
                    NoworkVM noworkVM = new NoworkVM(nowork);
                    results[employee].Add(noworkVM);
                }
            }

            return results;
        }

        public List<IShiftVM> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            var results = new List<IShiftVM>();
            var employees = employeeRepository.All();

            foreach (var employee in employees)
            {
                foreach (var duty in dutyRepository.GetBy(employee, startDate, endDate))
                {
                    results.Add(new DutyVM(duty));
                }

                foreach (var nowork in noworkRepository.GetAssenzeByDipDateRange(employee, startDate, endDate))
                {
                    results.Add(new NoworkVM(nowork));
                }
            }

            return results;
        }

        public List<IShiftVM> GetByEmployeeAndDate(Employee employee, DateTime datetime)
        {
            var results = new List<IShiftVM>();
            
            foreach (var duty in dutyRepository.GetBy(employee, datetime))
            {
                results.Add(new DutyVM(duty));
            }

            foreach (var nowork in noworkRepository.GetAssenzeByEmployeeAndDate(employee, datetime))
            {
                results.Add(new NoworkVM(nowork));
            }
            

            return results;
        }

        public IDictionary<Employee, List<IShiftVM>> GetCassaByDateRangeDict(DateTime startDate, DateTime endDate)
        {
            //var results = new SortedDictionary<Employee, List<IShiftVM>>(new EmployeeFullNameComparer());
            var results = new ConcurrentDictionary<Employee, List<IShiftVM>>();
            foreach (var duty in dutyRepository.GetBy(startDate, endDate))
            {
                if (duty.Position.ToLower().Equals("cassa"))
                {
                    DutyVM dutyVM = new DutyVM(duty);
                    var val = results.GetOrAdd(dutyVM.Employee, new List<IShiftVM>());
                    val.Add(dutyVM);
                    results[dutyVM.Employee] = val;

                    //if (!results.ContainsKey(dutyVM.Employee))
                    //{
                    //    results.Add(dutyVM.Employee, new List<IShiftVM>());
                    //}
                    //results[dutyVM.Employee].Add(dutyVM);
                }
            }
            
            return results;
        }

        public IDictionary<Employee, List<IShiftVM>> GetNotCassaByDateRangeDict(DateTime startDate, DateTime endDate)
        {
            //var comparer = new EmployeeFullNameComparer();
            //var results = new SortedDictionary<Employee, List<IShiftVM>>(comparer);
            var results = new ConcurrentDictionary<Employee, List<IShiftVM>>();

            foreach (var duty in dutyRepository.GetBy(startDate, endDate))
            {
                if (!duty.Position.ToLower().Equals("cassa"))
                {
                    DutyVM dutyVM = new DutyVM(duty);
                    var val = results.GetOrAdd(dutyVM.Employee, new List<IShiftVM>());
                    val.Add(dutyVM);
                    results[dutyVM.Employee] = val;
                    //if (!results.ContainsKey(dutyVM.Employee))
                    //{
                    //    results.Add(dutyVM.Employee, new List<IShiftVM>());
                    //}
                    //results[dutyVM.Employee].Add(dutyVM);
                }
            }

            foreach (var nowork in noworkRepository.GetAssenzeByDateRange(startDate, endDate))
            {
                NoworkVM nowvm = new NoworkVM(nowork);
                var val = results.GetOrAdd(nowork.Employee, new List<IShiftVM>());
                val.Add(nowvm);
                results[nowvm.Employee] = val;
            }

            return results;
        }

        public void DeleteShift(IShiftVM shift)
        {
            DutyVM dutyVm = shift as DutyVM;
            if (dutyVm != null)
            {
                dutyRepository.Delete(dutyVm.Id);
            }
            else
            {
                NoworkVM noworkVm = shift as NoworkVM;
                
                if(!User.CurrentUser.CanDelete(noworkVm))
                {
                    GuiUtils.Error("Non autorizzato", null, "Errore");
                    return;
                }

                noworkRepository.Delete(noworkVm.Id);
            }
        }

    }

    public class EmployeeFullNameComparer : IComparer<Employee>
    {
        public int Compare(Employee x, Employee y)
        {
            return x.FullName.CompareTo(y.FullName);
        }
    }

    public class EmployeeDefaultPositionComparer : IComparer<Employee>
    {
        public int Compare(Employee x, Employee y)
        {
            if (x.DefaultPosition != null)
                return x.DefaultPosition.Id.CompareTo(y.DefaultPosition.Id);
            else
                return 0;
        }
    }
}
