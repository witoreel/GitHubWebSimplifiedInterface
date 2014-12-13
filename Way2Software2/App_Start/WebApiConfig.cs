using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Way2Software2 {
    
    /// <summary>
    /// Classe responsável por cadastrar as configurações de API
    /// </summary>
    public static class WebApiConfig {

        /// <summary>
        /// Registra a configuração padrão de API
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config) {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
