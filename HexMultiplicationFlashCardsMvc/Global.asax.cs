using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HexMultiplicationFlashCardsMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //existed in .NET MVC template
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //added later
            AutoMapperConfig.Initialize();
            ModelBinders.Binders.Add(typeof(ViewModels.FlashCard), new ViewModels.FlashCardModelBinder());
        }
    }
}
