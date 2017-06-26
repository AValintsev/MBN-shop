using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Import.ItLink.Services;

namespace Nop.Plugin.Import.ItLink.Infrustructure
{
	public class DependencyRegistar : IDependencyRegistrar
	{
		public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig conf)
		{
			builder.RegisterType<XmlToXlsConverter>()
				.As<IXmlToXlsConverter>()
				.InstancePerLifetimeScope();

			builder.RegisterType<XmlImporter>()
				.As<IXmlImporter>()
				.InstancePerLifetimeScope();
		}

		public int Order
		{
			get { return 1; }
		}
	}
}
