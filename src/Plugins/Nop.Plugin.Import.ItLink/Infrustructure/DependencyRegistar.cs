using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Import.ItLink.Data;
using Nop.Plugin.Import.ItLink.Domain;
using Nop.Plugin.Import.ItLink.Services;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Import.ItLink.Infrustructure
{
	public class DependencyRegistar : IDependencyRegistrar
	{
		private const string CONTEXT_NAME = "nop_object_context_Importer";

		public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig conf)
		{
			//data context
			this.RegisterPluginDataContext<ImportObjectContext>(builder, CONTEXT_NAME);

			builder.RegisterType<ItLinkImportManager>()
				.As<IItLinkImportManager>()
				.InstancePerLifetimeScope();

			//override required repository with our custom context
			builder.RegisterType<EfRepository<CategoryInternalToExternalMap>>()
				.As<IRepository<CategoryInternalToExternalMap>>()
				.WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
				.InstancePerLifetimeScope();
		}

		public int Order
		{
			get { return 1; }
		}
	}
}
