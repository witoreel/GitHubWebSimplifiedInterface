using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaySoftware2.GitHubApi;
using Way2Software2.Models;

namespace Way2Software2.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            ViewBag.Title = "Informações do candidato";
            ViewBag.SubTitle = "Meus repositórios";

            //Carrega informações sobre o usuário
            GitHubRequest request = new GitHubRequest();
            GHUser user = request.LoadLoggedUser();
            user.Repositories = request.LoadRepositoriesByURL(user.repos_url);

            return View(user);
        }



        public ActionResult Favorite(string repository_fullname) {

            LocalDBContext db = new LocalDBContext();
            db.Repositories.Add(new LocalRepositoryEntry { name = repository_fullname, favorite = true });
            db.SaveChanges();

            return RedirectToAction("Repository", "Home", new { repository_fullname = repository_fullname });
        }


        public ActionResult DeFavorite(string repository_fullname) {

            LocalDBContext db = new LocalDBContext();
            LocalRepositoryEntry entity = db.Repositories.FirstOrDefault(u => u.name.CompareTo(repository_fullname) == 0);
            db.Repositories.Remove(entity);
            db.SaveChanges();

            return RedirectToAction("Repository", "Home", new { repository_fullname = repository_fullname });
        }


        public ActionResult Repository(string repository_fullname) {
            ViewBag.Title = "Informações do repositório";
            ViewBag.SubTitle = "Colaboradores";

            //Carrega informações sobre o repositório            
            LocalDBContext db = new LocalDBContext();
            LocalRepositoryEntry entity = db.Repositories.FirstOrDefault(u => u.name.CompareTo(repository_fullname) == 0);
            bool favorite = entity != null ? entity.favorite : false;

            GitHubRequest request = new GitHubRequest();
            Models.GHRepository repository = request.LoadRepositoryByFullName(repository_fullname);
            repository.Owner = request.LoadUserByLogin(repository.owner_login);
            repository.Contributors = request.LoadUsersByURL(repository.contributors_url);
            repository.IsFavorite = favorite;

            return View(repository);
        }

        public ActionResult Favorites() {
            ViewBag.Title = "Repositórios favoritados";

            LocalDBContext db = new LocalDBContext();
            List<GHRepository> repositories = new List<GHRepository>();
            GitHubRequest request = new GitHubRequest();
            foreach(LocalRepositoryEntry entry in db.Repositories.OrderByDescending(a => a.star_gazers).ToList())
                repositories.Add(request.LoadRepositoryByFullName(entry.name));

            return View(repositories);
        }

        public ActionResult Search() {
            ViewBag.Title = "Pesquisar repositórios";
            
            return View((GHRepository) null);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Search(string keyword) {

            GitHubRequest request = new GitHubRequest();
            GHRepository[] repositories = request.SearchRepositoriesByKeyWord(keyword);
            ViewBag.EmptyMsg = repositories.Length == 0 ? "Não foram encontrados repositórios." : "";            

            return View(repositories.ToList());
        }


    }
}
