using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaySoftware2.GitHubApi;

namespace Way2Software2.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            ViewBag.Title = "Informações do candidato";
            ViewBag.SubTitle = "Meus repositórios";

            //Carrega informações sobre o usuário
            GHUser user = GHUser.Load();
            ViewBag.GitHubUserLogin = user.login;
            ViewBag.GitHubUserName = user.name;
            ViewBag.GitHubUserEmail = user.email;
            ViewBag.GitHubUserAvatarURL = user.avatar_url;
            ViewBag.GitHubHtmlURL = user.html_url;
            ViewBag.GitHubCreatedAt = user.CreatedAt.Day + " de " + user.CreatedAt.ToString("MMMM")+" de "+user.CreatedAt.Year;

            ViewBag.DataTable = GetDataTableFromRepositories(user.Repositories);           

            return View();
        }


        public ActionResult Repository(string url) {
            ViewBag.Title = "Informações do repositório";
            ViewBag.SubTitle = "Colaboradores";

            //Carrega informações sobre o repositório
            GHRepository repository = GHRepository.Load(url);

            ViewBag.GitHubRepositoryName = repository.name;
            ViewBag.GitHubRepositoryDescription = repository.description;
            ViewBag.GitHubRepositoryLanguage = repository.language;
            ViewBag.GitHubRepositoryUpdatedAt = repository.UpdatedAt;
            ViewBag.GitHubHtmlURL = user.html_url;
            ViewBag.GitHubCreatedAt = user.CreatedAt.Day + " de " + user.CreatedAt.ToString("MMMM") + " de " + user.CreatedAt.Year;

            ViewBag.DataTable = GetDataTableFromRepositories(user.Repositories);

            return View();
        }

        public ActionResult Favorites() {
            ViewBag.Title = "Repositórios favoritados";

            return View();
        }

        public ActionResult Search() {
            ViewBag.Title = "Pesquisar repositórios";
            ViewBag.DataTable = new DataTable();

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Search(string keyword) {

            GHRepository[] repositories = GHRepository.Search(keyword);
            ViewBag.EmptyMsg = repositories.Length == 0 ? "Não foram encontrados repositórios." : "";
            ViewBag.DataTable = GetDataTableFromRepositories(repositories);

            return View();
        }


        private DataTable GetDataTableFromRepositories(GHRepository[] repositories) {

            //Preenche a tabela com os dados dos repositórios
            string[] column_names = new string[] { "name", "description", "stargazers_count", "url" };
            DataTable table = new DataTable("Repositories");
            foreach (string column in column_names)
                table.Columns.Add(new DataColumn(column, typeof(string)));

            foreach (GHRepository repo in repositories) {
                DataRow row = table.NewRow();
                row[column_names[0]] = repo.name;
                row[column_names[1]] = repo.description;
                row[column_names[2]] = repo.stargazers_count;
                row[column_names[3]] = repo.url;
                table.Rows.Add(row);
            }
            
            return table;
        }

    }
}
