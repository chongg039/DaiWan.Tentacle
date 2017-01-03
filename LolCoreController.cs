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
    public class LolCoreController : ApiController
    {

        // GET: combat/API
        [HttpGet]
        public JObject UserArea(string auth, string keyword)
        {
            try
            {
                string referer = @"http://game.tgp.qq.com/lol/search/v1603/search.html?kw=#kw#"
                                .Replace("#kw#", HttpUtility.UrlEncode(keyword));

                string url = @"http://api.pallas.tgp.qq.com/core/search_player?callback=jQuery1111026891932846046984_#jqueryext#&key=#key#&_=#t#"
                               .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                               .Replace("#key#", HttpUtility.UrlEncode(keyword))
                               .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());

                JArray jarray = new JArray();
                JObject jo = APILib.GetJSON(referer, url,auth);
                JObject json = APILib.Iced(jo["data"].ToObject<JArray>());
                return json;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("UserArea", ex);
                return APILib.Error(ex.Message);
            }
        }

        /// <summary>
        /// 个人基本信息
        /// </summary>
        /// <param name="qquin"></param>
        /// <param name="vaid"></param>
        /// <returns></returns>

        [HttpGet]
        public JObject UserHotInfo(string auth, string qquin, string vaid)
        {
            try
            {
                string referer = @"http://game.tgp.qq.com/lol/profile/v1602/overview.shtml?qquin=#qquin#&area_id=#area_id#"
                                  .Replace("#qquin#", qquin)
                                  .Replace("#area_id#", vaid);

                string url = "http://api.pallas.tgp.qq.com/core/get_user_hot_info?callback=jQuery111106649491745047271_#jqueryext#&area_id=#area_id#&qquin=#qquin#&_=#t#"
                                .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                                .Replace("#area_id#", vaid)
                                .Replace("#qquin#", qquin)
                                .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());

                JObject jo = APILib.GetJSON(referer, url, auth);
                //正常调用并返回
                JObject json = APILib.Iced(jo["data"].ToObject<JObject>());
                return json;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("UserHotInfo", ex);
                return APILib.Error(ex.Message);
            }
        }


        [HttpGet]
        public JObject UserExtInfo(string auth, string qquin, string vaid)
        {
            try
            {
                string para = @"[[7,{""item_num"":1,""items"":[{""qquin"":""#qquin#"",""area_id"":""#vaid#""}]}],[28,{""qquin"":""#qquin#"",""area_id"":""#vaid#""}],[29,{""qquin"":""#qquin#"",""area_id"":""#vaid#"",""top_mvp_type"":0}],[35,{""qquin"":""#qquin#"",""area_id"":""#vaid#"",""champion_id"":0}],[36,{""qquin"":""#qquin#"",""area_id"":""#vaid#""}]]"
                                    .Replace("#qquin#", qquin)
                                    .Replace("#vaid#", vaid);

                string referer = @"http://game.tgp.qq.com/lol/profile/v1602/overview.shtml?qquin=#qquin#&area_id=#area_id#"
                                    .Replace("#qquin#", qquin)
                                    .Replace("#area_id#", vaid);

                string url = @"http://api.pallas.tgp.qq.com/core/tcall?callback=jQuery17206660636231168173_#jqueryext#&p=#p#&_cache_time=#cache_time#"
                                    .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                                    .Replace("#p#", para)
                                    .Replace("#cache_time#", "300");
                JObject jo = APILib.GetJSON(referer, url,auth);
                //正常调用并返回
                JObject json = APILib.Iced(jo["data"].ToObject<JArray>());
                return json;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("UserExtInfo", ex);
                return APILib.Error(ex.Message);
            }
        }


        /// <summary>
        /// 战斗概览信息
        /// </summary>
        /// <param name="qquin"></param>
        /// <param name="vaid"></param>
        /// <returns></returns>
        [HttpGet]
        public JObject BattleSummaryInfo(string auth, string qquin, string vaid)
        {

            Random random = new Random();
            string p = @"[[14,{""battle_type"":-1,""qquin"":""#qquin#"",""area_id"":""#area_id#""}],[44,{""sid"":5,""qquin"":""#qquin#"",""area_id"":""#area_id#""}],[44,{""sid"":4,""qquin"":""#qquin#"",""area_id"":""#area_id#""}],[44,{""sid"":3,""qquin"":""#qquin#"",""area_id"":""#area_id#""}],[44,{""sid"":2,""qquin"":""#qquin#"",""area_id"":""#area_id#""}],[44,{""sid"":1,""qquin"":""#qquin#"",""area_id"":""#area_id#""}]]"
                        .Replace("#qquin#", qquin)
                        .Replace("#area_id#", vaid);
            string referer = @"http://game.tgp.qq.com/lol/profile/v1602/overview.shtml?qquin=#qquin#&area_id=#area_id#";
            string url = @"http://api.pallas.tgp.qq.com/core/tcall?callback=jQuery#jquery#_#jqueryext#&p=#p#&_cache_time=#cache_time#"
                                .Replace("#p#", HttpUtility.UrlEncode(p))
                                .Replace("#jquery#", "1111009980190638452" + random.Next(10, 99))
                                .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                                .Replace("#t#", DateHelper.ConvertDateTime2Long12(DateTime.Now).ToString())
                                .Replace("#cache_time#", "300"); ;
            JObject jo = null;
            try
            {
                jo = APILib.GetJSON(referer, url, auth);
                //正常调用并返回
                JObject json = APILib.Iced(jo["data"].ToObject<JArray>());
                return json;
            }
            catch (Exception ex)
            {
                if (jo != null)
                {
                    return APILib.Iced(jo);
                }

                LogHelper.LogInfo(referer + "\n" + url + "\n" + "return json is null");
                LogHelper.LogError("BattleSummaryInfo", ex);
                return APILib.Error("接口 BattleSummaryInfo 调用错误");
            }


        }

        /// <summary>
        /// 个人战绩列表
        /// </summary>
        /// <param name="qquin"></param>
        /// <param name="vaid"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpGet]
        public JObject CombatList(string auth,string qquin, string vaid, int pagesize, int p)
        {
            try
            {
                string para = @"[[3,{""qquin"":""#qquin#"",""area_id"":""#area_id#"",""bt_num"":""0"",""bt_list"":[],""champion_id"":0,""offset"":#offset#,""limit"":#limit#,""mvp_flag"":-1}]]"
                                    .Replace("#qquin#", qquin)
                                    .Replace("#area_id#", vaid)
                                    .Replace("#limit#", pagesize.ToString())
                                    .Replace("#offset#", (p * pagesize).ToString());

                string callback = "jQuery17209483979241204745_#jqueryext#"
                                    .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());

                string referer = @"http://api.tgp.qq.com/profile/v1602/overview.shtml?vuin=#vuin#&vaid=#vaid#"
                                    .Replace("#vuin#", qquin)
                                    .Replace("#vaid#", vaid);

                string url = @"http://api.pallas.tgp.qq.com/core/tcall?callback=#callback#&p=#p#&_=#t#"
                                    .Replace("#callback#", HttpUtility.UrlEncode(callback))
                                    .Replace("#p#", HttpUtility.UrlEncode(para))
                                    .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());

                JObject jo = APILib.GetJSON(referer, url, auth);
                //正常调用并返回
                JObject json = APILib.Iced(jo["data"].ToObject<JArray>());

                return json;

            }
            catch (Exception ex)
            {
                LogHelper.LogError("CombatList", ex);

                return APILib.Error(ex.Message);
            }
        }

        /// <summary>
        ///  通过gameid返回具体单场对战信息
        /// </summary>
        /// <param name="qquin"></param>
        /// <param name="vaid"></param>
        /// <param name="gameid"></param>
        /// <returns></returns>
        [HttpGet]
        public JObject GameDetail(string auth, string qquin, string vaid, string gameid)
        {
            Random random = new Random();
            string para = @"""area_id"":""#area_id#"",""game_id"":#game_id#"
                                .Replace("#area_id#", vaid)
                                .Replace("#game_id#", gameid)
                                .Replace(@"""", HttpUtility.UrlEncode(@""""));


            string referer = @"http://game.tgp.qq.com/lol/profile/v1602/history.shtml?qquin=#vuin#&area_id=#vaid#&game_id=#game_id#&battle_type=0&champion_id=0&cur_page=1"
                  .Replace("#vuin#", qquin)
                  .Replace("#vaid#", vaid)
                  .Replace("#game_id#", gameid);


            string url = @"http://api.pallas.tgp.qq.com/core/get_battle_info?p={#p#}&callback=jQuery#jquery#_#jqueryext#&_=#t#"
                             .Replace("#dtag#", qquin)
                             .Replace("#p#", para)
                             .Replace("#jquery#", "1111015899130702018" + random.Next(100, 999))
                             .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                             .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());
            JObject jo = null;
            try
            {
                jo = APILib.GetJSON(referer, url, auth);
                //正常调用并返回
                JObject json = APILib.Iced(jo["data"].ToObject<JObject>());
                return json;
            }
            catch (Exception ex)
            {
                if (jo != null)
                {
                    return APILib.Iced(jo);
                }

                LogHelper.LogInfo(referer + "\n" + url + "\n" + "return json is null");
                LogHelper.LogError("GameDetail", ex);
                return APILib.Error("接口 GameDetail 调用错误");
            }
        }

        [HttpGet]
        public JObject GetChampionDetail(string auth,string champion_id)
        {

            string referer = @"http://game.tgp.qq.com/lol/champions/v1605/info.shtml?champion_id=#champion_id#"
                                .Replace("#champion_id#", champion_id);
            string url = "http://cdn.tgp.qq.com/pallas/conf/heros/#champion_id#.js?t=#t#&_=#t#"
                                .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                                .Replace("#champion_id#", champion_id); ;
            string host = "cdn.tgp.qq.com";
            string jsonrawdata = APILib.GetJSONRawData(host, referer, url, auth);

            string reg = @"data:\s*({[\S\s]*),updated:";
            string result = CommonLib.TextHelper.MatchContent(reg, jsonrawdata, RegexOptions.Multiline, 1);
            JObject jobject = JObject.Parse(result);
            JObject json = APILib.Iced(jobject);
            return json;
        }

        [HttpGet]
        public JObject GetMastery(string auth,string qquin, string vaid)
        {
            Random random = new Random();
            string referer = @"http://game.tgp.qq.com/lol/profile/v1602/talent.html?nid=45&qquin=#qquin#&area_id=#area_id#"
                        .Replace("#qquin#", qquin)
                        .Replace("#area_id#", vaid);

            string url = @"http://api.pallas.tgp.qq.com/core/get_player_mastery_spell?callback=jQuery#jquery#_#jqueryext#&area_id=#area_id#&qquin=#qquin#&get_type=2&dtag=runes&_=#t#"
                                .Replace("#jquery#", "1111009980190638452" + random.Next(10, 99))
                                .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                                .Replace("#t#", DateHelper.ConvertDateTime2Long12(DateTime.Now).ToString())
                                .Replace("#qquin#", qquin)
                                .Replace("#area_id#", vaid);
            JObject jo = null;
            try
            {
                jo = APILib.GetJSON(referer, url, auth);
                //正常调用并返回
                JObject json = APILib.Iced(jo["data"].ToObject<JObject>());
                return json;
            }
            catch (Exception ex)
            {
                if (jo != null)
                {
                    return APILib.Iced(jo);
                }

                LogHelper.LogInfo(referer + "\n" + url + "\n" + "return json is null");
                LogHelper.LogError("GetMastery", ex);
                return APILib.Error("接口 GetMastery 调用错误");
            }



        }



        [HttpGet]
        public JObject UserChampion(string auth, string qquin, string vaid)
        {

            string p = @"[[35,{""area_id"":""#area_id#"",""qquin"":""#qquin#""}]]"
                       .Replace("#qquin#", qquin)
                       .Replace("#area_id#", vaid);

            Random random = new Random();
            string referer = @"http://game.tgp.qq.com/lol/profile/v1602/my_champs.shtml?nid=44&qquin=#qquin#&area_id=#area_id#"
                        .Replace("#qquin#", qquin)
                        .Replace("#area_id#", vaid);


            string url = @"http://api.pallas.tgp.qq.com/core/tcall?callback=jQuery#jquery#_#jqueryext#&p=#p#&_=#t#"
                                .Replace("#jquery#", "1111009980190638452" + random.Next(10, 99))
                                .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                                .Replace("#p#", HttpUtility.UrlEncode(p))
                                .Replace("#t#", DateHelper.ConvertDateTime2Long12(DateTime.Now).ToString())
                                .Replace("#qquin#", qquin)
                                .Replace("#area_id#", vaid);
            JObject jo = null;
            try
            {
                jo = APILib.GetJSON(referer, url, auth);
                //正常调用并返回
                JObject json = APILib.Iced(jo["data"].ToObject<JArray>());
                return json;
            }
            catch (Exception ex)
            {
                if (jo != null)
                {
                    return APILib.Iced(jo);
                }

                LogHelper.LogInfo(referer + "\n" + url + "\n" + "return json is null");
                LogHelper.LogError("UserChampion", ex);
                return APILib.Error("接口 UserChampion 调用错误");
            }



        }



        [HttpGet]
        public JObject GetChampionSkin(string auth,string champion_id, string skinid)
        {

            string result = @"http://cdn.tgp.qq.com/pallas/images/skins/original/#champion_id#-#skinid#.jpg"
                                .Replace("#champion_id#", champion_id)
                                .Replace("#skinid#", skinid);

            return APILib.Data(result);
        }


        [HttpGet]
        public JObject Free(string auth)
        {
            try
            {
                string referer = @"http://game.tgp.qq.com/lol/champions/v1605/lib.shtml";
                string url = @"http://lol.qq.com/biz/hero/free.js?t=#t#&_=#t#"
                                    .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());
                string host = "lol.qq.com";

                string jsonrawdata = APILib.GetJSONRawData(host, referer, url,auth);

                string reg = @"""data"":\s*({.*}),(\r\n)*\s*""";
                string result = CommonLib.TextHelper.MatchContent(reg, jsonrawdata, RegexOptions.Multiline, 1);
                JObject jobject = JObject.Parse(result);
                JObject json = APILib.Iced(jobject);
                return json;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Free", ex);
                return APILib.Error(ex.Message);
            }
        }


        [HttpGet]
        public JObject ChampionRank(string auth ,string championid, int p)
        {
            try
            {
                string referer = @"http://game.tgp.qq.com/lol/champions/v1605/rank.shtml";
                string url = @"http://img.lol.qq.com/js/cevRank/#championid#/#p#.js?t=#t#"
                                    .Replace("#championid#", championid)
                                    .Replace("#p#", p.ToString())
                                    .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());

                string host = "img.lol.qq.com";
                string jsonrawdata = APILib.GetJSONRawData(host, referer, url, auth);
                string reg = @"""data"":\s*({.*}),(\r\n)*\s*""";
                string result = CommonLib.TextHelper.MatchContent(reg, jsonrawdata, RegexOptions.Multiline, 1);
                JObject jobject = JObject.Parse(result);
                JObject json = APILib.Iced(jobject);
                return json;
            }
            catch (Exception ex)
            {
                LogHelper.LogError("ChampionRank", ex);
                return APILib.Error(ex.Message);
            }
        }



    }
}