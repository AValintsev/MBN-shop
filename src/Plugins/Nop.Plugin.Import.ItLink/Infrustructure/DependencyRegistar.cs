using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Import.ItLink.Services;
using Nop.Services.ExportImport;
using Nop.Plugin.Import.ItLink.Domain;
using Autofac.Core;
using Nop.Data;
using Nop.Core.Data;
using Nop.Web.Framework.Mvc;
using Nop.Plugin.Import.ItLink.Data;

namespace Nop.Plugin.Import.ItLink.Infrustructure
{
	public class DependencyRegistar : IDependencyRegistrar
	{
		private const string CONTEXT_NAME = "nop_object_context_Importer";

		public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig conf)
		{
			//data context
			this.RegisterPluginDataContext<ImportObjectContext>(builder, CONTEXT_NAME);

			builder.RegisterType<XmlToXlsConverter>()
				.As<IXmlToXlsConverter>()
				.InstancePerLifetimeScope();

			builder.RegisterType<ItLinkExportManager>()
				.As<IExportManager>()
				.InstancePerLifetimeScope();


			//override required repository with our custom context
			builder.RegisterType<EfRepository<InternalToExternal>>()
				.As<IRepository<InternalToExternal>>()
				.WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
				.InstancePerLifetimeScope();
		}

		public int Order
		{
			get { return 1; }
		}
	}
}
