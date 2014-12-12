using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WaySoftware2.GitHubApi{


    public class GHUser {

        public string login { get; set; }

        public int id { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public string repos_url { get; set; }

        public string avatar_url { get; set; }

        public string html_url { get; set; }

        public string created_at { get; set; }

        public DateTime CreatedAt {
            get {
               return DateTime.Parse(created_at);
            }
        }

        public GHRepository[] Repositories;

        public static GHUser Load() {

            GitHubJSONRequest gh = new GitHubJSONRequest();
            GHUser user = gh.GitHubObject<GHUser>(gh.UserURL);
            user.Repositories = GHRepository.LoadList(user.repos_url);
            
            return user;
        }



    }




}
