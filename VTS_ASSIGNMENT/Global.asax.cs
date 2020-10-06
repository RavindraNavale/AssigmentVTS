
using System.Web.Http;
using VTS_ASSIGNMENT.Repository;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using System.Configuration;

namespace VTS_ASSIGNMENT
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Container container = new Container();
            container.Register<IUserRepository>(() => new UserRepository(ConfigurationManager.AppSettings["ConnectionString"].ToString() ), Lifestyle.Singleton);
            container.Register<IVehicleRepository>(() => new VehicleRepository(ConfigurationManager.AppSettings["ConnectionString"].ToString() ), Lifestyle.Singleton);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
