using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Way2Software2 {
    
    /// <summary>
    /// Classe responsável por cadastrar uma nova aplicação Web do tipo MVC.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication {

        /// <summary>
        /// Inicia a aplicação.
        /// </summary>
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Realiza o tratamento dos erros genéricos ocorridos na página, prevenindo assim de ser exibido StackTrace para o usuário.
        /// </summary>
        protected void Application_Error() {
            Server.ClearError();
            Response.Redirect("/Main/Error");
        }

    }
}