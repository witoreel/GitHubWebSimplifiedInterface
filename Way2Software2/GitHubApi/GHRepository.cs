using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Way2Software2.Properties;

namespace WaySoftware2.GitHubApi{


    public class GHRepository {

        public int id { get; set; }

        public int stargazers_count { get; set; }        

        public string name { get; set; }

        public string full_name { get; set; }

        public string html_url { get; set; }

        public string description { get; set; }

        public string language { get; set; }

        public string updated_at { get; set; }

        public DateTime UpdatedAt {
            get {
                return DateTime.Parse(updated_at);
            }
        }

        public string url { get; set; }

        public static GHRepository[] LoadList(string repos_url) {

            GitHubJSONRequest gh = new GitHubJSONRequest();
            GHRepository[] repos = gh.GitHubObjectArray<GHRepository>(repos_url);

            return repos;
        }

        public static GHRepository Load(string repo_url) {

            GitHubJSONRequest gh = new GitHubJSONRequest();
            GHRepository repo = gh.GitHubObject<GHRepository>(repo_url);

            return repo;
        }

        public static GHRepository[] Search(string keyword) {

            GitHubJSONRequest gh = new GitHubJSONRequest();
            string url = String.Format(Resources.GitHubRepositorySearchURL, keyword);
            url = gh.InsertToken(url);

            //Coleta o json e identifica a parcela dos items
            string json = gh.RequestJSON(url);
            json = json.Substring(json.IndexOf("["));
            json = json.Substring(0, json.IndexOf("]") + 1);

            GHRepository[] repos = new GHRepository[0];
            if (json.Length > 0)
                repos = gh.GitHubObjectArrayByJson<GHRepository>(json);

            return repos;
        }

    }




}
