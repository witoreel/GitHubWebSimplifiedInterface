using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Way2Software2.Properties;

namespace WaySoftware2.GitHubApi {


    public class GitHubJSONRequest {

        enum GitHubParameter {
            access_token
        }

        enum GitHubStaticPage {
            user
        }

        public string UserURL {
            get {
                string url = Resources.GitHubBaseURL;
                url += GitHubStaticPage.user;
                return url;
            }

        }

        private string StaticParameters {
            get {
                return "?" + GitHubParameter.access_token + "=" + Resources.GitHubOAuthToken;
            }
        }

        private T Deserialise<T>(string json) {
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(json))) {
                T result = (T)deserializer.ReadObject(stream);
                return result;
            }
        }


        public T GitHubObject<T>(string url) {

            string json = RequestJSON(url);
            return Deserialise<T>(json);

        }

        public T[] GitHubObjectArray<T>(string url) {

            string[] json = JSONRootAsArray(RequestJSON(url));
            T[] objs = new T[json.Length];
            for (int i = 0; i < objs.Length; i++)
                objs[i] = Deserialise<T>(json[i]);

            return objs;

        }

        public T[] GitHubObjectArrayByJson<T>(string js) {

            string[] json = JSONRootAsArray(js);
            T[] objs = new T[json.Length];
            for (int i = 0; i < objs.Length; i++)
                objs[i] = Deserialise<T>(json[i]);

            return objs;

        }

        public string InsertToken(string url) {
            return url + (url.IndexOf("?") > -1 ? StaticParameters.Replace("?", "&") : StaticParameters);            
        }

        public string RequestJSON(string url) {

            url = InsertToken(url);
            string json = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "gh";
            try {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                json = reader.ReadToEnd();
            } catch (WebException ex) {

                //IMPLEMENTAR CONTROLE DO ERRO
                
                
                throw;
            }

            return json;
        }


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

            string[] jss = new string[comma_indexes.Count+1];
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




    }

}
