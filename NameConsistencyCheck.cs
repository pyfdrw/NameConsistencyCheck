using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;

using Autofac;
using NameConsistencyCheck.Startup;
using NameConsistencyCheck.Views;

namespace NameConsistencyCheck
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                using (Application app = Application.CreateApplication(null, null))
                {
                    Execute(app);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
            }
        }
        static void Execute(Application app)
        {
            // TODO: add here your code
            var bs = new Bootstrapper();
            var container = bs.Bootstrap(app);
            var mainView = container.Resolve<CheckMainView>();
            mainView.ShowDialog();
        }
    }
}
