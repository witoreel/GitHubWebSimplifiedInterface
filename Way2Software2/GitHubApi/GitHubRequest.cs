using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Way2Software2.Models;
using Way2Software2.Properties;

namespace WaySoftware2.GitHubApi {

    /// <summary>
    /// Classe responsável por realizar as requisições de dados com a API do GitHub.
    /// Consiste na formatação das URL's e na serialização do JSON de retorno em objetos de modelo.
    /// </summary>
    public class GitHubRequest {

        #region ====== Enumeradores ======

        /// <summary>
        /// Define os tipos de parâmetros a serem utilizados nas requisições com a API do GitHub
        /// </summary>
        enum GitHubParameter {
            access_token
        }

        #endregion

        #region ====== Métodos Públicos ======

        /// <summary>
        /// Retorna um modelo de usuário a partir das informações do usuário logado ao github
        /// </summary>
        /// <returns>Objeto de modelo de usuário</returns>
        public GHUser LoadLoggedUser() {
            return GitHubObject<GHUser>(Resources.GitHubLoggedUserURL);
        }

        /// <summary>
        /// Retorna uma lista de modelos de usuários, a partir de uma URL que retorne uma lista de usuários.
        /// </summary>
        /// <param name="url">URL de retorno de lista de usuários</param>
        /// <returns>Lista com objetos de modelo de usuários</returns>
        public GHUser[] LoadUsersByURL(string url) {

            url = InsertToken(url);
            GHUser[] users = GitHubObjectArray<GHUser>(url);
            for (int i = 0; i < users.Length; i++)
                users[i] = LoadUserByLogin(users[i].login);

            return users;
        }

        /// <summary>
        /// Retorna um objeto de modelo de usuário a partir da informação de login no GitHub.
        /// </summary>
        /// <param name="login">Login do usuário</param>
        /// <returns>Objeto de modelo de usuário</returns>
        public GHUser LoadUserByLogin(string login) {
            string url = InsertToken(Resources.GitHubUsersURL + login);
            return GitHubObject<GHUser>(url);
        }

        /// <summary>
        /// Retorna uma lista com objetos de modelo de repositórios, a partir de uma URL que retorne uma lista de repositórios.
        /// </summary>
        /// <param name="url">URL de retorno de lista de repositórios</param>
        /// <returns>Lista com objetos de modelo de repositórios</returns>
        public GHRepository[] LoadRepositoriesByURL(string url) {
            return GitHubObjectArray<GHRepository>(url);
        }

        /// <summary>
        /// Retorna um objeto de modelo de repositório a partir de seu nome de identificação.
        /// </summary>
        /// <param name="fullname">Nome de identificação do repositório</param>
        /// <returns>Objeto de modelo de repositório</returns>
        public GHRepository LoadRepositoryByFullName(string fullname) {
            if (fullname == null || fullname.Length == 0)
                return null;

            string url = InsertToken(Resources.GitHubRepositoryURL + fullname);
            return GitHubObject<GHRepository>(url);
        }

        /// <summary>
        /// Retorna um objeto de modelo de repositório a partir de sua URL.
        /// </summary>
        /// <param name="url">URL relacionada a um repositório</param>
        /// <returns>Objeto de modelo de repositório</returns>
        public GHRepository LoadRepositoryByURL(string url) {
            return GitHubObject<GHRepository>(url);
        }

        /// <summary>
        /// Realiza uma busca por repositórios a partir de uma palavra chave.
        /// </summary>
        /// <param name="keyword">Palavra chave de busca</param>
        /// <returns>Lista com objetos de modelo de repositórios</returns>
        public GHRepository[] SearchRepositoriesByKeyWord(string keyword) {

            string url = InsertToken(String.Format(Resources.GitHubRepositorySearchURL, keyword));

            //Coleta o json e identifica a parcela dos items
            string json = RequestJSON(url);
            if (json.IndexOf("[") > -1 && json.IndexOf("]") > -1) {
                json = json.Substring(json.IndexOf("["));
                json = json.Substring(0, json.IndexOf("]") + 1);
            } else
                json = "";

            return GitHubObjectArrayByJson<GHRepository>(json);
        }

        #endregion

        #region ====== Métodos Privados ======

        /// <summary>
        /// Converte as informações de um arquivo JSON em um objeto definido de modelo.
        /// </summary>
        /// <typeparam name="T">Classe do modelo do objeto a ser obtido</typeparam>
        /// <param name="json">Texto do JSON obtido do GitHub</param>
        /// <returns>Objeto de modelo</returns>
        private T Deserialise<T>(string json) {
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(json))) {
                T result = default(T);
                try {
                    result = (T)deserializer.ReadObject(stream);
                } catch {
                }
                return result;
            }
        }

        /// <summary>
        /// Retorna um objeto de modelo a partir de uma URL do GitHub.
        /// </summary>
        /// <typeparam name="T">Classe do modelo do objeto a ser obtido</typeparam>
        /// <param name="url">URL associada ao objeto no GitHub</param>
        /// <returns>Objeto de modelo</returns>
        private T GitHubObject<T>(string url) {

            string json = RequestJSON(url);
            return Deserialise<T>(json);

        }

        /// <summary>
        /// Retorna uma lista de objetos de modelo a partir de uma URL de lista do GitHub.
        /// </summary>
        /// <typeparam name="T">Classe do modelo do objeto a ser obtido</typeparam>
        /// <param name="url">URL associada à uma lista de objetos no GitHub</param>
        /// <returns>Lista de objetos de modelo</returns>
        private T[] GitHubObjectArray<T>(string url) {

            string[] json = JSONRootAsArray(RequestJSON(url));
            T[] objs = new T[json.Length];
            for (int i = 0; i < objs.Length; i++)
                objs[i] = Deserialise<T>(json[i]);

            return objs;

        }

        /// <summary>
        /// Retorna uma lista de objetos de modelo a partir de texto no formato JSON de lista.
        /// </summary>
        /// <typeparam name="T">Classe do modelo do objeto a ser obtido</typeparam>
        /// <param name="js">Texto no formato JSON de lista</param>
        /// <returns>Lista de objetos de modelo</returns>
        private T[] GitHubObjectArrayByJson<T>(string js) {

            if (js == null || js.Length == 0)
                return new T[0];

            string[] json = JSONRootAsArray(js);
            List<T> objs = new List<T>();
            for (int i = 0; i < json.Length; i++)
                try {
                    objs.Add(Deserialise<T>(json[i]));
                }catch{
                }

            return objs.ToArray();

        }

        /// <summary>
        /// Insere a chave de autorização de um usuário na URL de requisição do GitHub, a fim de aumentar a quantidade de requisições permitidas.
        /// </summary>
        /// <param name="url">URL de requisição do GitHub</param>
        /// <returns>URL de requisição do GitHub, com o token adicionado</returns>
        private string InsertToken(string url) {
            string staticParameters = "?" + GitHubParameter.access_token + "=" + Resources.GitHubOAuthToken;
            return url + (url.IndexOf("?") > -1 ? staticParameters.Replace("?", "&") : staticParameters);
        }

        /// <summary>
        /// Retorna um texto no formato JSON a partir de uma requisição ao servidor do GitHub.
        /// </summary>
        /// <param name="url">URL de requisição do GitHub</param>
        /// <returns>Texto no formato JSON</returns>
        private string RequestJSON(string url) {

            url = InsertToken(url);
            string json = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "gh";

            try {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                json = reader.ReadToEnd();
            } catch {
                return "";
            }

            return json;
        }

        /// <summary>
        /// Quebra um texto no formato JSON no formato lista, retornando um array com os subtrechos JSON.
        /// </summary>
        /// <param name="json">Texto JSON inicial</param>
        /// <returns>Texto JSON separado em uma lista</returns>
        private string[] JSONRootAsArray(string json) {

            string js = json.Trim();

            //Verifica se não é um arquivo json com um único objeto
            if (js.Length > 0 && js[0] != '[')
                return new string[] { json };

            //Identifica a posição das virgulas no arquivo, ignorando as que estão dentro de chaves
            List<int> comma_indexes = new List<int>();
            int brackets = 0;
            for (int i = 1; i < js.Length - 1; i++) {
                if (js[i] == '{')
                    brackets++;
                if (js[i] == '}')
                    brackets--;
                if (js[i] == ',' && brackets == 0)
                    comma_indexes.Add(i);
            }

            string[] jss = new string[comma_indexes.Count + 1];
            if (jss.Length > 1) {
                for (int i = 0; i < jss.Length; i++) {
                    int start_idx = i == 0 ? 1 : comma_indexes[i - 1] + 1;
                    int end_idx = i == jss.Length - 1 ? js.Length - 2 : comma_indexes[i] - 1;
                    jss[i] = js.Substring(start_idx, end_idx - start_idx);
                }
            } else {
                jss[0] = js.Substring(1, js.Length - 2);
                if (jss[0].Length == 0)
                    jss = new string[0];
            }

            return jss;
        }

        #endregion

    }

}
