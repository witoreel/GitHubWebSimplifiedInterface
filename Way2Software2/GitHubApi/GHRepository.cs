using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Way2Software2.Properties;

namespace WaySoftware2.GitHubApi{

    //public class GHRepository2 {

    //    public int id { get; set; }

    //    public int stargazers_count { get; set; }        

    //    public string name { get; set; }

    //    public string full_name { get; set; }

    //    public string html_url { get; set; }

    //    public string description { get; set; }

    //    public string language { get; set; }

    //    public string updated_at { get; set; }

    //    public string contributors_url { get; set; }

    //    public DateTime UpdatedAt {
    //        get {
    //            return DateTime.Parse(updated_at);
    //        }
    //    }

    //    private string owner_login { get {
    //        return full_name.Substring(0, full_name.IndexOf("/"));
    //    } }

    //    public GHUser Owner = new GHUser();

    //    public GHUser[] Contributors = new GHUser[0];

    //    public string url { get; set; }

    //    public void LoadOwner() {
    //        Owner = GHUser.Load(owner_login);
    //    }

    //    public void LoadContributors() {
    //        Contributors = GHUser.LoadList(contributors_url);
    //    }

    //    public static GHRepository2[] LoadList(string repos_url) {

    //        GitHubRequest gh = new GitHubRequest();
    //        GHRepository2[] repos = gh.GitHubObjectArray<GHRepository2>(repos_url);
           
    //        return repos;
    //    }

    //    public static GHRepository2 LoadByName(string repository_fullname) {

    //        GitHubRequest gh = new GitHubRequest();
    //        string url = Resources.GitHubRepositoryURL + repository_fullname;
    //        url = gh.InsertToken(url);

    //        GHRepository2 repo = gh.GitHubObject<GHRepository2>(url);

    //        return repo;

    //    }

    //    public static GHRepository2 Load(string repo_url) {

    //        GitHubRequest gh = new GitHubRequest();
    //        GHRepository2 repo = gh.GitHubObject<GHRepository2>(repo_url);

    //        return repo;
    //    }

    //    public static GHRepository2[] Search(string keyword) {

    //        GitHubRequest gh = new GitHubRequest();
    //        string url = String.Format(Resources.GitHubRepositorySearchURL, keyword);
    //        url = gh.InsertToken(url);

    //        //Coleta o json e identifica a parcela dos items
    //        string json = gh.RequestJSON(url);
    //        json = json.Substring(json.IndexOf("["));
    //        json = json.Substring(0, json.IndexOf("]") + 1);

    //        GHRepository2[] repos = gh.GitHubObjectArrayByJson<GHRepository2>(json);
            
    //        return repos;
    //    }

    //}




}
