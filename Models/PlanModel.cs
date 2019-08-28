using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using VMS.TPS.Common.Model.API;

namespace NameConsistencyCheck.Models
{
    public class PlanModel : BindableBase
    {
        private string _planId;

        public string PlanId
        {
            get { return _planId; }
            set
            {
                SetProperty(ref _planId, value);
            }
        }

        // 是否已经被治疗
        private bool _isTreated;

        public bool IsTreated
        {
            get { return _isTreated; }
            set
            {
                SetProperty(ref _isTreated, value);
            }
        }

        // The name of the user who approved the plan for planning. 
        private string _planningApprover;

        public string PlanningApprover
        {
            get { return _planningApprover; }
            set
            {
                SetProperty(ref _planningApprover, value);
            }
        }

        // The name of the user who approved the plan for treatment. 
        private string _treatmentApprover;

        public string TreatmentApprover
        {
            get { return _treatmentApprover; }
            set
            {
                SetProperty(ref _treatmentApprover, value);
            }
        }

        // Treatment approve 事件
        private string _treatmentApproveDateTime;

        public string TreatmentApproveDateTime
        {
            get { return _treatmentApproveDateTime; }
            set
            {
                SetProperty(ref _treatmentApproveDateTime, value);
            }
        }

        // 计算出来的命名
        private string _recommendId;

        public string RecommendId
        {
            get { return _recommendId; }
            set
            {
                SetProperty(ref _recommendId, value);
            }
        }

        // 是否符合命名要求
        private bool _isConsistent;

        public bool IsConsistent
        {
            get { return _isConsistent; }
            set
            {
                SetProperty(ref _isConsistent, value);
            }
        }

        public ObservableCollection<FieldModel> FieldModels;

        public bool IsAllDatainished = false;

        public PlanModel(PlanSetup _planSetup)
        {
            if (null == _planSetup)
                return;
            PlanId = _planSetup.Id;
            IsTreated = _planSetup.IsTreated;
            PlanningApprover = _planSetup.PlanningApprover;
            TreatmentApprover = _planSetup.TreatmentApprover;
            TreatmentApproveDateTime = _planSetup.TreatmentApprovalDate;

        }
    }
}
