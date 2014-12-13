using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WaySoftware2.GitHubApi;

namespace Way2Software2.Models
{
    public class LocalDBContext : DbContext {
        public LocalDBContext()
            : base("Local") {
        }

        public DbSet<LocalRepositoryEntry> Repositories { get; set; }
    }

    [Table("Repositories")]
    public class LocalRepositoryEntry {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public bool favorite { get; set; }
        public int star_gazers { get; set; }
    }


    public class GHRepository {

        public int id { get; set; }

        public int stargazers_count { get; set; }

        public string name { get; set; }

        public string full_name { get; set; }

        public string html_url { get; set; }

        public string description { get; set; }

        public string language { get; set; }

        public string updated_at { get; set; }

        public string contributors_url { get; set; }

        public bool IsFavorite = false;

        public DateTime UpdatedAt {
            get {
                return updated_at == null ? DateTime.Parse(updated_at) : DateTime.MinValue;
            }
        }

        public string UpdatedAtFormated {
            get {
                return UpdatedAt.Day+" de "+UpdatedAt.ToString("MMMM") + " de "+UpdatedAt.Year;
            }
        }

        public string owner_login {
            get {
                return full_name.Substring(0, full_name.IndexOf("/"));
            }
        }

        public GHUser Owner = new GHUser();

        public GHUser[] Contributors = new GHUser[0];

        public string url { get; set; }

    }

    public class GHUser {

        public string login { get; set; }

        public int id { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public string repos_url { get; set; }

        public string avatar_url { get; set; }

        public string html_url { get; set; }

        public string created_at { get; set; }

        public string type { get; set; }

        public DateTime CreatedAt {
            get {
                return created_at == null ? DateTime.MinValue : DateTime.Parse(created_at);
            }
        }

        public string CreatedAtFormated {
            get {
                return CreatedAt.Day + " de " + CreatedAt.ToString("MMMM") + " de " + CreatedAt.Year;
            }
        }

        public GHRepository[] Repositories;

    }

   
}
