using System.Web;
using System.Web.Mvc;

namespace Way2Software2 {

    /// <summary>
    /// Classe responsável por cadastrar os filtros de navegação do sistema.
    /// </summary>
    public class FilterConfig {

        /// <summary>
        /// Registra os filtros globais.
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}