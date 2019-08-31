using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using System.Windows.Media;

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

        public bool IsAllDatainished = false;

        public FieldModel(Beam _oneField)
        {
            BeamId = _oneField.Id;

            // 摆位野
            if (_oneField.IsSetupField)
            {
                // 判断CBCT或者0/90
                if (Math.Abs(Math.Abs(_oneField.ControlPoints[0].JawPositions.X1) - 50.0) < 0.1) // -5 5 -5 5
                {
                    RecommendId = "CBCT";
                    if (RecommendId.ToLower().Equals(_oneField.Id.ToLower()))
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
                    if (RecommendId.ToLower().Equals(_oneField.Id.ToLower()))
                    {
                        IsAllDatainished = true;
                        IsConsistent = true;
                        return;
                    }
                }
                else if (_oneField.ControlPoints[0].GantryAngle.Equals(90))
                {
                    RecommendId = "90";
                    if (RecommendId.ToLower().Equals(_oneField.Id.ToLower()))
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
                RecommendId = $"{_oneField.Id[0]}{_oneField.Id[1]}{beamAngle}";
                if (RecommendId.ToLower().Equals(_oneField.Id.ToLower()))
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

                RecommendId = $"{_oneField.Id[0]}{_oneField.Id[1]}G{beginAngle}-{endAngle}";
                if (RecommendId.ToLower().Equals(_oneField.Id.ToLower()))
                {
                    IsConsistent = true;
                    IsAllDatainished = true;
                    return;
                }
            }

            IsConsistent = false;
            // RecommendId = "Others";
            IsAllDatainished = true;
            return;
        }
    }
}
