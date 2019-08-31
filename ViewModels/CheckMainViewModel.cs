using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using NameConsistencyCheck.Models;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using VMS.TPS.Common.Model.API;

namespace NameConsistencyCheck.ViewModels
{
    public class CheckMainViewModel : BindableBase
    {
        public ICollectionView ViewSource { get; set; }
        public ObservableCollection<PatientModel> PatientModels { get; set; }
        public ObservableCollection<PatientInfo> PatientIdCollection { get; set; }
        public ObservableCollection<string> PatientIdFailToOpen { get; set; }
        

        private Application app = null;

        public CheckMainViewModel(Application app)
        {
            this.app = app;

            PatientModels = new ObservableCollection<PatientModel>();
            PatientIdCollection = new ObservableCollection<PatientInfo>();
            PatientIdFailToOpen = new ObservableCollection<string>();
            
            ViewSource = CollectionViewSource.GetDefaultView(PatientIdCollection);

            // DoNameCheckCommand = new DelegateCommand(OnDoNameCheck, CanDoCheck);
            DoNameCheckCommand = new DelegateCommand(OnDoNameCheck);
        }

        public DelegateCommand DoNameCheckCommand { get; private set; }

        private void OnDoNameCheck()
        {
            PatientIdFailToOpen.Clear();
            PatientModels.Clear();
            foreach (var s in PatientIdCollection)
            {
                var patient = app.OpenPatientById(s.PatientId);
                if (null != patient)
                {
                    PatientModels.Add(new PatientModel(patient));
                }
                else
                {
                    PatientIdFailToOpen.Add(s.PatientId);
                }

                app.ClosePatient();
            }
        }

        private bool CanDoCheck()
        {
            return (app != null && PatientModels.Count != 0);
        }
    }

    public class PatientInfo : BindableBase
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

        public PatientInfo()
        {

        }
    }
}
