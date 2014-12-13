using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Way2Software2 {

    /// <summary>
    /// Classe responsável por cadastrar as configurações de roteamento da página.
    /// </summary>
    public class RouteConfig {

        /// <summary>
        /// Registra o roteamento necessário por definir a estrutura de navegação, juntamente como a opção padrão.
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}