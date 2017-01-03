using CommonLib;
using DaiWan.Lib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SKGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DaiWan.Tentacle
{

    public class PostData
    {
        public string key { get; set; }
        public string json { get; set; }
    }



    public class Util
    {
        public static string GetTentacleToken()
        {
            string token = string.Empty;
            try
            {
                string tokenpath = System.AppDomain.CurrentDomain.BaseDirectory + @"token.lic";
                using (System.IO.StreamReader sr = new StreamReader(tokenpath))
                {
                    token = sr.ReadToEnd();
                }
                CommonLib.LogHelper.LogDebug(token);
            }
            catch
            {
                throw new Exception("没有授权文件 [not find lic]");
            }
            return token;
        }


        /// <summary>
        /// 将本地Token到服务器端进行验证，如果失败程序将不能启动
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool CheckToken(string token)
        {
            bool isok = false;
            try
            {
                JObject jo = Util.CallRemoteAPI(token, string.Format("http://tentacleapi.games-cube.com/CheckTentacleToken?token={0}", token));
                JArray jarr = jo["data"].ToObject<JArray>();
                foreach (JObject item in jarr)
                {
                    bool.TryParse(item["return"].ToString(), out isok);
                }

                CommonLib.LogHelper.LogDebug(isok);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("CheckToken(string token)", ex);
                throw new Exception("从服务器端验证Token失败");
            }
            return isok;
        }


        public static JObject CallRemoteAPI(string TOKEN, string url)
        {
            try
            {
                JObject result = new JObject();
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("DAIWAN-API-TOKEN", TOKEN);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("DAIWAN-API-TOKEN-ENCRYPTION", "RSA");
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<JObject>().Result;
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return APILib.Error(ex.Message);
            }
        }


        public static JObject CallRemoteAPI_Post(string TOKEN, string url, JObject postdata)
        {
            try
            {

                JObject result = new JObject();
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("DAIWAN-API-TOKEN", TOKEN);
                // 构造POST参数
                HttpContent postContent = new StringContent(postdata.ToString(), Encoding.UTF8);
                HttpResponseMessage response = httpClient.PostAsJsonAsync<JObject>(url, postdata).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<JObject>().Result;
                }
                return result;


            }
            catch (Exception ex)
            {
                LogHelper.LogError("Area", ex);
                return APILib.Error(ex.Message);
            }
        }



        /// <summary>
        /// 公共API Token
        /// </summary>
        /// <returns></returns>
        public static string GetPublicToken()
        {
            string tentacle_token = GetTentacleToken();
            System.Web.Caching.Cache objCache = System.Web.HttpRuntime.Cache;
            if (objCache["webapi"] != null)
            {
                //Console.WriteLine("from cache"); 
                return objCache["webapi"].ToString();
            }
            //Console.WriteLine("from server");

            JObject jo = Util.CallRemoteAPI(tentacle_token, string.Format("http://tentacleapi.games-cube.com/GetWebAPIToken?tentacletoken={0}", tentacle_token));
            JArray jarr = jo["data"].ToObject<JArray>();
            string webapi_token = string.Empty;
            foreach (JObject item in jarr)
            {
                webapi_token = item["return"].ToString();
                objCache.Insert("webapi", webapi_token, null, System.DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration);
            }
            return webapi_token;
        }
    }
}
