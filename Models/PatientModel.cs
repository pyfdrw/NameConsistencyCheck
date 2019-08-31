using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace NameConsistencyCheck.Models
{
    public class PatientModel : BindableBase
    {
        private string _patientId;

        public string PatientId
        {
            get { return _patientId; }
            set
            {
                SetProperty(ref _patientId, value);
            }
        }

        private string _patientName;

        public string PatientName
        {
            get { return _patientName; }
            set
            {
                SetProperty(ref _patientName, value);
            }
        }

        private DateTime _historyDateTime;

        public DateTime HistoryDateTime
        {
            get { return _historyDateTime; }
            set
            {
                SetProperty(ref _historyDateTime, value);
            }
        }

        private int _approvedPlanNum;

        public int ApprovedPlanNum
        {
            get { return _approvedPlanNum; }
            set
            {
                SetProperty(ref _approvedPlanNum, value);
            }
        }

        private int _unApprovedPlanNum;

        public int UnApprovedPlanNum
        {
            get { return _unApprovedPlanNum; }
            set
            {
                SetProperty(ref _unApprovedPlanNum, value);
            }
        }

        public ObservableCollection<PlanModel> PlanModels { get; set; }

        public bool IsAllDatainished = false;

        public PatientModel(Patient onePatient)
        {
            if (null == onePatient)
                return;

            PatientId = onePatient.Id;
            PatientName = onePatient.LastName;
            HistoryDateTime = onePatient.HistoryDateTime;

            PlanModels = new ObservableCollection<PlanModel>();

            // Course 命名没有规定，此处跳过

            // 收集所有的External Beam Plan
            List<PlanSetup> plans = new List<PlanSetup>();
            foreach (Course onePatientCourse in onePatient.Courses)
            {
                plans.AddRange(onePatientCourse.PlanSetups.Where(x => x is ExternalPlanSetup));
            }

            // 对每个计划进行核查
            foreach (PlanSetup plan in plans)
            {
                PlanModels.Add(new PlanModel(plan));
                if (plan.ApprovalStatus == PlanSetupApprovalStatus.TreatmentApproved)
                    ApprovedPlanNum++;
                else
                {
                    UnApprovedPlanNum++;
                }
            }
        }
    }
}
