using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Widgets.NivoSliderLocalized.Data;
using Nop.Plugin.Widgets.NivoSliderLocalized.Domain;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.NivoSliderLocalized.Infrustructure
{
	public class DependencyRegistar : IDependencyRegistrar
	{
		private const string CONTEXT_NAME = "nop_object_context_product_view_slider";

		public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig conf)
		{
			//data context
			this.RegisterPluginDataContext<SliderObjectContext>(builder, CONTEXT_NAME);

			builder.RegisterType<EfRepository<SliderItem>>()
				.As<IRepository<SliderItem>>()
				.WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
				.InstancePerLifetimeScope();
		}

		public int Order
		{
			get { return 1; }
		}
	}
}
