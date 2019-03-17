using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Mapping.Categories.Services;

namespace Nop.Plugin.Mapping.Categories
{
	public class DependencyRegistar : IDependencyRegistrar
	{
		public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig conf)
		{
			builder.RegisterType<ExportXmlToPromUa>()
				.As<IExportXml<ExportXmlToPromUa>>();

			builder.RegisterType<ExportXmlToHotline>()
				.As<IExportXml<ExportXmlToHotline>>();
		}

		public int Order
		{
			get { return 1; }
		}
	}
}
