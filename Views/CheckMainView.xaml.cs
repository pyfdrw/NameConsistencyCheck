using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NameConsistencyCheck.ViewModels;

namespace NameConsistencyCheck.Views
{
    /// <summary>
    /// CheckMainView.xaml 的交互逻辑
    /// </summary>
    public partial class CheckMainView : Window
    {
        private CheckMainViewModel _checkMainViewModel;

        public CheckMainView(CheckMainViewModel checkMainViewModel)
        {
            _checkMainViewModel = checkMainViewModel;
            this.DataContext = _checkMainViewModel;
            InitializeComponent();
        }
    }
}
