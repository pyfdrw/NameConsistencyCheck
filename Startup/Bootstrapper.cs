using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using NameConsistencyCheck.ViewModels;
using NameConsistencyCheck.Views;
using Prism.Events;
using VMS.TPS.Common.Model.API;

namespace NameConsistencyCheck.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap(Application app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            
            builder.RegisterType<CheckMainView>().AsSelf();
            
            builder.RegisterType<CheckMainViewModel>().AsSelf();
            
            builder.RegisterInstance<Application>(app);
            return builder.Build();
        }
    }
}
