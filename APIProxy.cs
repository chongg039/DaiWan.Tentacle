using CommonLib;
using DaiWan.Lib;
using Newtonsoft.Json.Linq;
using SKGL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DaiWan.Tentacle
{
    /// <summary>
    /// area
    /// </summary>
    public class LolAPIProxy
    {
        public static string GetProxy()
        {
            return ConfigurationManager.AppSettings["Host"].ToString() + "/LolCore";
        }

        public static JObject UserArea(string auth,string keyword)
        {
            return CallRemoteAPI(string.Format(@"{0}/UserArea?auth={1}&keyword={2}", GetProxy(), auth, keyword));
        }

        public static  JObject UserHotInfo(string auth, string qquin, string vaid)
        {
            return CallRemoteAPI(string.Format(@"{0}/UserHotInfo?auth={1}&qquin={2}&vaid={3}", GetProxy(), auth, qquin, vaid));
        }

        public static  JObject UserExtInfo(string auth, string qquin, string vaid)
        {
            return CallRemoteAPI(string.Format(@"{0}/UserExtInfo?auth={1}&qquin={2}&vaid={3}", GetProxy(), auth, qquin, vaid));
        }
        public static  JObject BattleSummaryInfo(string auth, string qquin, string vaid)
        {
            return CallRemoteAPI(string.Format(@"{0}/BattleSummaryInfo?auth={1}&qquin={2}&vaid={3}", GetProxy(), auth, qquin, vaid));
        }
        public static  JObject CombatList(string auth, string qquin, string vaid, int pagesize, int p)
        {
            return CallRemoteAPI(string.Format(@"{0}/CombatList?auth={1}&qquin={2}&vaid={3}&pagesize={4}&p={5}", GetProxy(), auth, qquin, vaid,pagesize,p));

        }
        public static  JObject GameDetail(string auth, string qquin, string vaid, string gameid)
        {
            return CallRemoteAPI(string.Format(@"{0}/GameDetail?auth={1}&qquin={2}&vaid={3}&gameid={4}", GetProxy(), auth, qquin, vaid,gameid));
        }
        public static  JObject GetChampionDetail(string auth, string champion_id)
        {
            return CallRemoteAPI(string.Format(@"{0}/GetChampionDetail?auth={1}&champion_id={2}", GetProxy(), auth, champion_id));
        }
        public static  JObject GetMastery(string auth, string qquin, string vaid)
        {
            return CallRemoteAPI(string.Format(@"{0}/GetMastery?auth={1}&qquin={2}&vaid={3}", GetProxy(), auth, qquin, vaid));

        }
        public static  JObject UserChampion(string auth, string qquin, string vaid)
        {
            return CallRemoteAPI(string.Format(@"{0}/UserChampion?auth={1}&qquin={2}&vaid={3}", GetProxy(), auth, qquin, vaid));
        }
        public static  JObject GetChampionSkin(string auth, string champion_id, string skinid)
        {
            return CallRemoteAPI(string.Format(@"{0}/GetChampionSkin?auth={1}&champion_id={2}&skinid={3}", GetProxy(), auth, champion_id, skinid));
        }


        public static  JObject Free(string auth)
        {
            return CallRemoteAPI(string.Format(@"{0}/Free?auth={1}", GetProxy(), auth));
        }

        public static  JObject ChampionRank(string auth,string championid, int p)
        {
            return CallRemoteAPI(string.Format(@"{0}/ChampionRank?auth={1}&championid={2}&p={3}", GetProxy(), auth,championid,p));
        }


        public static JObject CallRemoteAPI(string url)
        {
            try
            {
                JObject result = new JObject();
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<JObject>().Result;
                }
                return result;
            }
            catch (Exception ex)
            {
                return APILib.Error(ex.Message);
            }
        }




    }
}