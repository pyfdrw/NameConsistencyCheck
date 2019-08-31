using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Prism.Mvvm;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

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

        private string _approvalStatus;

        public string ApprovalStatus
        {
            get { return _approvalStatus; }
            set
            {
                SetProperty(ref _approvalStatus, value);
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
                if (!value)
                {
                    SolidColorBrush = new SolidColorBrush(Color.FromArgb(50, 255, 0, 0));
                }
                else
                {
                    SolidColorBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                }
                SetProperty(ref _isConsistent, value);
            }
        }

        // 不符合要求则醒目的背景色
        private SolidColorBrush _solidColorBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        public SolidColorBrush SolidColorBrush
        {
            get { return _solidColorBrush; }
            set
            {
                SetProperty(ref _solidColorBrush, value);
            }
        }

        public ObservableCollection<FieldModel> FieldModels { get; set; }

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

            FieldModels = new ObservableCollection<FieldModel>();

            // 现阶段暂时无法利用ESAPI获取计划的程数或者是部位信息
            // 假定现阶段的程数和部位都是正确的
            // TODO 查找确定程数和部位的方法

            // 获取Plan的命名
            ApprovalStatus = Enum.GetName(typeof(PlanSetupApprovalStatus), _planSetup.ApprovalStatus);

            bool isArcPlan = false;
            int treatBeamNum = 0;
            foreach (Beam planSetupBeam in _planSetup.Beams)
            {
                FieldModel fm = new FieldModel(planSetupBeam);
                FieldModels.Add(fm);
                if (!planSetupBeam.IsSetupField)
                {
                    treatBeamNum++;
                    if (planSetupBeam.Technique.Id.ToLower().Equals("arc"))
                    {
                        isArcPlan = true;
                    }
                }
            }

            // 生成命名
            if (0 == treatBeamNum)
            {
                RecommendId = PlanId;
                if (RecommendId.ToLower().Equals(PlanId.ToLower()))
                {
                    IsAllDatainished = true;
                    IsConsistent = true;
                    return;
                }
            }
            RecommendId = $"{PlanId[0]}{treatBeamNum}{(isArcPlan ? "VMAT" : "IMRT")}a";
            if (RecommendId.ToLower().Equals(PlanId.ToLower()))
            {
                IsAllDatainished = true;
                IsConsistent = true;
                return;
            }
            else
            {
                IsAllDatainished = true;
                IsConsistent = false;
                return;
            }
        }
    }
}
