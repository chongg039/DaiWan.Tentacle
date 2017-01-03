using CommonLib;
using DaiWan.Lib; 
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace DaiWan.Tentacle.Controllers
{
   
    public class CacheInfo
    {
        public string content;
        public DateTime cachedate;
        public DateTime timeout;

    }

    public class DefaultController : ApiController
    {
        #region Util
        
        /// <summary>
        /// 获取服务器端QQ请求认证
        /// </summary>
        /// <param name="tentacle_token"></param>
        /// <returns></returns>
        private string GetAuth(string tentacle_token)
        {
            //TODO:本地客户端加入缓存
            string Auth = string.Empty;
            try
            {


                /***********************Log***********************/
                DateTime beforDT = System.DateTime.Now;
                /***********************Log***********************/

                System.Web.Caching.Cache objCache = HttpRuntime.Cache;
                if (objCache["LolTentacle"] != null)
                    return objCache["LolTentacle"].ToString();

                JObject jo = null; //这里使用生成QQ认证信息
                JArray jarr = jo["data"].ToObject<JArray>();
                foreach (JObject item in jarr)
                {
                    Auth = item["return"].ToString();
                    objCache.Insert("LolTentacle", Auth, null, System.DateTime.Now.AddMinutes(5), TimeSpan.Zero);
                    //CommonLib.LogHelper.LogInfo(string.Format("加入缓存，Auth:{0}", Auth));
                }

                /***********************Log***********************/
                DateTime afterDT = System.DateTime.Now;
                TimeSpan ts = afterDT.Subtract(beforDT);
                CommonLib.LogHelper.LogInfo(string.Format("GetAuth {0} ms", ts.TotalMilliseconds));
                /***********************Log***********************/


            }
            catch (Exception ex)
            {
                LogHelper.LogError("GetAuth(string token)", ex);
                throw new Exception("从服务器端获取Auth失败 [get auth from daiwan server error]");
            }
            return Auth;
        }

        /// <summary>
        /// 获取服务器端QQ请求认证
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private string GetAuth()
        {
            string tentacletoken = Util.GetTentacleToken();
            return GetAuth(tentacletoken);
        }
        #endregion


        #region Mapping Server API
        [HttpGet]
        public JObject Area()
        {
            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, "http://lolapi.games-cube.com/Area");
        }

        [HttpGet]
        public JObject Champion()
        {
            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, "http://lolapi.games-cube.com/Champion");
        }
      
        [HttpGet]
        public JObject GetAreaName(string id)
        {
            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, string.Format("http://lolapi.games-cube.com/GetAreaName?id={0}",id));
        }


        [HttpGet]
        public JObject GetUserIcon(string iconid)
        {
            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, string.Format(@"http://lolapi.games-cube.com/GetUserIcon?iconid={0}", iconid));
        }

        [HttpGet]
        public JObject GetChampionIcon(string championname)
        {
            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, string.Format(@"http://lolapi.games-cube.com/GetChampionIcon?championname={0}", championname));
        }

        [HttpGet]
        public JObject GetChampionIcon(int id)
        {
            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, string.Format(@"http://lolapi.games-cube.com/GetChampionIcon?id={0}", id));
        }

        [HttpGet]
        public JObject GetSummonSpellIcon(string summonspellid)
        {
            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, string.Format(@"http://lolapi.games-cube.com/GetSummonSpellIcon?summonspellid={0}", summonspellid));
        }

        [HttpGet]
        public JObject GetitemIcon(string itemid)
        {
            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, string.Format(@"http://lolapi.games-cube.com/GetitemIcon?itemid={0}", itemid));
        }

        [HttpGet]
        public JObject GetChampionENName(string id)
        {
            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, string.Format(@"http://lolapi.games-cube.com/GetChampionENName?id={0}", id));
        }


        [HttpGet]
        public JObject GetChampionCNName(string id)
        {

            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, string.Format(@"http://lolapi.games-cube.com/GetChampionCNName?id={0}", id));
        }


        [HttpGet]
        public JObject GetMapName(string id)
        {

            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, string.Format(@"http://lolapi.games-cube.com/GetMapName?id={0}", id));
        }

        [HttpGet]
        public JObject GetJudgement(string flag)
        {
            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, string.Format(@"http://lolapi.games-cube.com/GetJudgement?flag={0}", flag));
        }

        [HttpGet]
        public JObject GetWin(string win)
        {
            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, string.Format(@"http://lolapi.games-cube.com/GetWin?win={0}", win));
        }


        [HttpGet]
        public JObject GetGameType(string game_type)
        {
            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, string.Format(@"http://lolapi.games-cube.com/GetGameType?game_type={0}", game_type));
        }


        [HttpGet]
        public JObject GetGameMode(string game_mode)
        {
            string token = Util.GetPublicToken();
            return Util.CallRemoteAPI(token, string.Format(@"http://lolapi.games-cube.com/GetGameMode?game_mode={0}", game_mode));

        }

#endregion

        #region Client API
        [HttpGet]
        public JObject UserArea(string keyword)
        {
            string auth = GetAuth();
            return LolAPIProxy.UserArea(auth, keyword);
        }
        [HttpGet]
        public JObject UserHotInfo(string qquin, string vaid)
        {
            string auth = GetAuth();
            return LolAPIProxy.UserHotInfo(auth, qquin, vaid);
        }
        [HttpGet]
        public JObject UserExtInfo(string qquin, string vaid)
        {
            string auth = GetAuth();
            return LolAPIProxy.UserExtInfo(auth, qquin, vaid);
        }
        [HttpGet]
        public JObject BattleSummaryInfo(string qquin, string vaid)
        {
            string auth = GetAuth();
            return LolAPIProxy.BattleSummaryInfo(auth, qquin, vaid);
        }

        [HttpGet]
        public JObject CombatList(string qquin, string vaid)
        {
            return CombatList(qquin, vaid, 0);
        }

        [HttpGet]
        public JObject CombatList(string qquin, string vaid, int p)
        {
            return CombatList(qquin, vaid, 10, p);
        }
        [HttpGet]
        public JObject CombatList(string qquin, string vaid, int pagesize, int p)
        {
            string auth = GetAuth();
            return LolAPIProxy.CombatList(auth, qquin, vaid, pagesize,p);
        }
        [HttpGet]
        public JObject GameDetail(string qquin, string vaid, string gameid)
        {
            string auth = GetAuth();
            return LolAPIProxy.GameDetail(auth, qquin, vaid, gameid);
        }

        [HttpGet]
        public JObject Free()
        {
            string auth = GetAuth();
            return LolAPIProxy.Free(auth);
        }

        [HttpGet]
        public JObject ChampionRank(string championid)
        {
            return ChampionRank(championid, 1);
        }
        [HttpGet]
        public JObject ChampionRank(string championid,int p)
        {
            string auth = GetAuth();
            return LolAPIProxy.ChampionRank(auth, championid, p);
        }
        [HttpGet]
        public JObject GetChampionDetail(string champion_id)
        {
            string auth = GetAuth();
            return LolAPIProxy.GetChampionDetail(auth, champion_id);
        }
        #endregion
    }
}
