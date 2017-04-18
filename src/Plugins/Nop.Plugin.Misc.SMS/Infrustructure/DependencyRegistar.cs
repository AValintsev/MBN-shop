using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Misc.SMS.Data;
using Nop.Plugin.Misc.SMS.Domain;
using Nop.Web.Framework.Mvc;
using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Services.Events;
using Nop.Core.Domain.Orders;

namespace Nop.Plugin.Misc.SMS.Infrustructure
{
    public class DependencyRegistar : IDependencyRegistrar
    {
        private const string CONTEXT_NAME = "nop_object_context_product_view_sms";

        public virtual  void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig conf)
        {

            //data context
            this.RegisterPluginDataContext<SMSObjectContext>(builder, CONTEXT_NAME);

            //override required repository with our custom context
            builder.RegisterType<EfRepository<Domain.SMS>>()
                .As<IRepository<Domain.SMS>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<SMSMessage>>()
                .As<IRepository<SMSMessage>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();

            builder.RegisterType<EfRepository<SMSProvider>>()
                .As<IRepository<SMSProvider>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
