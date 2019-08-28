using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

namespace NameConsistencyCheck.Models
{
    public class FieldModel : BindableBase
    {
        private string _beamId;

        public string BeamId
        {
            get { return _beamId; }
            set
            {
                SetProperty(ref _beamId, value);
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
        private bool _isConsistent = false;

        public bool IsConsistent
        {
            get { return _isConsistent; }
            set
            {
                SetProperty(ref _isConsistent, value);
            }
        }

        public bool IsAllDatainished = false;

        public FieldModel(Beam _oneField, string _sessions = "A", string part = "a")
        {
            BeamId = _oneField.Id;

            // 摆位野
            if (_oneField.IsSetupField)
            {
                // 判断CBCT或者0/90
                if (Math.Abs(Math.Abs(_oneField.ControlPoints[0].JawPositions.X1) - 5.0) < 0.1) // -5 5 -5 5
                {
                    RecommendId = "CBCT";
                    if (RecommendId.ToLower().Equals(_oneField.Id))
                    {
                        IsConsistent = true;
                        IsAllDatainished = true;
                        return;
                    }
                }
                // 判断0 / 90
                else if (_oneField.ControlPoints[0].GantryAngle.Equals(0))
                {
                    RecommendId = "0";
                    if (RecommendId.ToLower().Equals(_oneField.Id))
                    {
                        IsAllDatainished = true;
                        IsConsistent = true;
                        return;
                    }
                }
                else if (_oneField.ControlPoints[0].GantryAngle.Equals(90))
                {
                    RecommendId = "90";
                    if (RecommendId.ToLower().Equals(_oneField.Id))
                    {
                        IsAllDatainished = true;
                        IsConsistent = true;
                        return;
                    }
                }
                else
                {
                    RecommendId = "Wrong Setup Beam Angle";
                    IsAllDatainished = true;
                    IsConsistent = false;
                    return;
                }
            }

            bool isStatic = _oneField.Technique.Id.ToLower().Equals("static");
            bool isArc = _oneField.Technique.Id.ToLower().Equals("arc");

            if (!isStatic && !isArc ||
                isStatic && isArc)
            {
                IsConsistent = false;
                RecommendId = "Not supported beam technique";
                IsAllDatainished = true;
                return;
            }

            if (isStatic)
            {
                int beamAngle = (int)_oneField.ControlPoints[0].GantryAngle;
                RecommendId = $"{_sessions}{part}{beamAngle}";
                if (RecommendId.ToLower().Equals(_oneField.Id))
                {
                    IsConsistent = true;
                    IsAllDatainished = true;
                    return;
                }
            }
            
            if(isArc)
            {
                int beginAngle = (int)_oneField.ControlPoints[0].GantryAngle;
                int endAngle = (int)_oneField.ControlPoints[index: _oneField.ControlPoints.Count - 1].GantryAngle;

                RecommendId = $"{_sessions}{part}G{beginAngle}-{endAngle}";
                if (RecommendId.ToLower().Equals(_oneField.Id))
                {
                    IsConsistent = true;
                    IsAllDatainished = true;
                    return;
                }
            }

            IsConsistent = false;
            RecommendId = "Others";
            IsAllDatainished = true;
            return;
        }
    }
}
