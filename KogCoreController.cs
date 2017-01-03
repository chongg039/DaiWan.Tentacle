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
    public class KogCoreController : ApiController
    {

        [HttpGet]
        public JObject ChampionDetail(string auth, string hero_id)
        {
            string url = @"http://static.tgp.ieg.tencent-cloud.com/js/sgame/heros/hero#hero_id#.js"
                              .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                              .Replace("#hero_id#", hero_id)
                              .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());
            JObject jo = null;

            try
            {
                string reg = @"(callback_hero\()([\s\S]*?)(\)}\s*catch)";
                int groupindex = 2;
                jo = Lib.APILib.GetJSON(url, reg, groupindex);
                return Lib.APILib.Iced(jo);
            }
            catch (Exception ex)
            {
                if (jo != null)
                {
                    LogHelper.LogInfo(url + "\n" + jo.ToString(Formatting.Indented));
                    return Lib.APILib.Iced(jo);
                }

                LogHelper.LogInfo(url + "\n" + "return json is null");
                LogHelper.LogError("UserArea", ex);
                return Lib.APILib.Error("接口 UserArea 调用错误");
            }
        }




        // GET: combat/API
        [HttpGet]
        public JObject UserArea(string auth, string keyword)
        {

            string url = @"http://api.pallas.tgp.qq.com/sgame/search_player?p={""key"":""#key#"",""key_type"":2}&callback=jQuery22208382472461089492_#jqueryext#&_=#t#"
                               .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                               .Replace("#key#", HttpUtility.UrlEncode(keyword))
                               .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());
            JObject jo = null;
            JArray jarr = new JArray();

            try
            {
                jo = Lib.APILib.GetJSON(string.Empty, url,auth);

                foreach (JObject item in jo["data"].ToObject<JArray>())
                {
                    foreach (var role in item["role_list"].ToObject<JArray>())
                    {
                        jarr.Add(role);
                    }
                }

                JObject json = Lib.APILib.Iced(jarr);
                return json;
            }
            catch (Exception ex)
            {
                if (jo != null)
                {
                    LogHelper.LogInfo(url + "\n" + jo.ToString(Formatting.Indented));
                    return Lib.APILib.Iced(jo);
                }

                LogHelper.LogInfo(url + "\n" + "return json is null");
                LogHelper.LogError("UserArea", ex);
                return Lib.APILib.Error("接口 UserArea 调用错误");
            }
        }

        /// <summary>
        /// 个人基本信息
        /// </summary>
        /// <param name="qquin"></param>
        /// <param name="vaid"></param>
        /// <returns></returns>


        [HttpGet]
        public JObject PlayerInfo(string auth,string area_id, string world_id, string open_id)
        {

            string url = @"http://api.pallas.tgp.qq.com/sgame/player_info?p={""full_id"":{""area_id"":#area_id#,""world_id"":#world_id#,""open_id"":""#open_id#""}}&callback=jQuery222041034458903595805_1475576862509&_=1475576862509"
                                 .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                                 .Replace("#area_id#", area_id)
                                 .Replace("#world_id#", world_id)
                                 .Replace("#open_id#", open_id)
                                 .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());
            JObject jo = null;
            try
            {
                jo = Lib.APILib.GetJSON(string.Empty, url,auth);
                //正常调用并返回
                JObject json = Lib.APILib.Iced(jo["data"].ToObject<JObject>());
                return json;
            }
            catch (Exception ex)
            {
                if (jo != null)
                {
                    LogHelper.LogInfo(url + "\n" + jo.ToString(Formatting.Indented));
                    return Lib.APILib.Iced(jo);
                }

                LogHelper.LogInfo(url + "\n" + "return json is null");
                LogHelper.LogError("PlayerInfo", ex);
                return Lib.APILib.Error("接口 PlayerInfo 调用错误");


            }
        }

        [HttpGet]
        public JObject BattleSummary(string auth, string area_id, string world_id, string open_id, int offset)
        {

            string url = @"http://api.pallas.tgp.qq.com/sgame/player_battle_sum?p={""full_id"":{""area_id"":#area_id#,""world_id"":#world_id#,""open_id"":""#open_id#""},""offset"":#offset#,""model"":0}&callback=jQuery22208382472461089492_#jqueryext#&_=#t#"
                            .Replace("#area_id#", area_id)
                            .Replace("#world_id#", world_id)
                            .Replace("#open_id#", open_id)
                            .Replace("#offset#", offset.ToString())
                            .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                            .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());
            JObject jo = null;
            try
            {
                jo = Lib.APILib.GetJSON(string.Empty, url, auth);
                //正常调用并返回
                JObject json = Lib.APILib.Iced(jo["data"].ToObject<JArray>());
                return json;
            }
            catch (Exception ex)
            {
                if (jo != null)
                {
                    LogHelper.LogInfo(url + "\n" + jo.ToString(Formatting.Indented));
                    return Lib.APILib.Iced(jo);
                }

                LogHelper.LogInfo(url + "\n" + "return json is null");
                LogHelper.LogError("BattleSummary", ex);
                return Lib.APILib.Error("接口 BattleSummary 调用错误");
            }
        }

        [HttpGet]
        public JObject PreferHero(string auth, string area_id, string world_id, string open_id, int offset)
        {

            string url = @"http://api.pallas.tgp.qq.com/sgame/hero_info?p={""full_id"":{""area_id"":#area_id#,""world_id"":#world_id#,""open_id"":""#open_id#""},""offset"":0,""limit"":99}&callback=jQuery22208382472461089492_#jqueryext#&_=#t#"
                            .Replace("#area_id#", area_id)
                            .Replace("#world_id#", world_id)
                            .Replace("#open_id#", open_id)
                            .Replace("#offset#", offset.ToString())
                            .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                            .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());
            JObject jo = null;
            try
            {
                jo = Lib.APILib.GetJSON(string.Empty, url, auth);
                //正常调用并返回
                JObject json = Lib.APILib.Iced(jo["data"].ToObject<JObject>());
                return json;
            }
            catch (Exception ex)
            {
                if (jo != null)
                {
                    LogHelper.LogInfo(url + "\n" + jo.ToString(Formatting.Indented));
                    return Lib.APILib.Iced(jo);
                }

                LogHelper.LogInfo(url + "\n" + "return json is null");
                LogHelper.LogError("PreferHero", ex);
                return Lib.APILib.Error("接口 PreferHero 调用错误");
            }
        } 

        [HttpGet]
        public JObject PlayerBattles(string auth, string area_id, string world_id, string open_id, int offset, int limit)
        {
            string url = @"http://api.pallas.tgp.qq.com/sgame/player_battles?p={""world_id"":#world_id#,""openid"":""#openid#"",""offset"":#offset#,""limit"":#limit#,""champion_id"":0}&callback=jQuery22208382472461089492_#jqueryext#&_=#t#"
                                .Replace("#area_id#", area_id)
                                .Replace("#world_id#", world_id)
                                .Replace("#openid#", open_id)
                                .Replace("#offset#", offset.ToString())
                                .Replace("#limit#", limit.ToString())
                                .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                                .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());
            JObject jo = null;
            try
            {
                jo = Lib.APILib.GetJSON(string.Empty, url, auth);
                //正常调用并返回
                JObject json = Lib.APILib.Iced(jo["data"].ToObject<JObject>());
                return json;
            }
            catch (Exception ex)
            {
                if (jo != null)
                {
                    LogHelper.LogInfo(url + "\n" + jo.ToString(Formatting.Indented));
                    return Lib.APILib.Iced(jo);
                }

                LogHelper.LogInfo(url + "\n" + "return json is null");
                LogHelper.LogError("PlayerBattles", ex);
                return Lib.APILib.Error("接口 PlayerBattles 调用错误");
            }
        }

        [HttpGet]
        public JObject BattleInfo(string auth, string device_type, string game_id)
        {
            string url = @"http://api.pallas.tgp.qq.com/sgame/battle_info?p={""plat_id"":#plat_id#,""game_id"":""#game_id#""}&callback=jQuery222025660077249631286_#jqueryext#&_=#t#"
                               .Replace("#plat_id#", device_type)
                               .Replace("#game_id#", game_id)
                               .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                               .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());
            JObject jo = null;
            try
            {
                jo = Lib.APILib.GetJSON(string.Empty, url, auth);
                //正常调用并返回
                JObject json = Lib.APILib.Iced(jo["data"].ToObject<JObject>());
                return json;
            }
            catch (Exception ex)
            {
                if (jo != null)
                {
                    LogHelper.LogInfo(url + "\n" + jo.ToString(Formatting.Indented));
                    return Lib.APILib.Iced(jo);
                }

                LogHelper.LogInfo(url + "\n" + "return json is null");
                LogHelper.LogError("BattleInfo", ex);
                return Lib.APILib.Error("接口 BattleInfo 调用错误");
            }
        }




        // GET: combat/API
        [HttpGet]
        public JObject FreeHero(string auth,string area_id, string world_id, string open_id)
        {

            string url = @"http://api.pallas.tgp.qq.com/sgame/free_limit_hero?p={""area_id"":#area_id#,""world_id"":#world_id#,""open_id"":""#open_id#""}&callback=jQuery22208382472461089492_#jqueryext#&_=#t#"
                              .Replace("#area_id#", area_id)
                                .Replace("#world_id#", world_id)
                                .Replace("#open_id#", open_id)
                                .Replace("#jqueryext#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString())
                                .Replace("#t#", DateHelper.ConvertDateTime2Long13(DateTime.Now).ToString());
            JObject jo = null;

            try
            {
                jo = Lib.APILib.GetJSON(string.Empty, url, auth);
                JObject json = Lib.APILib.Iced(jo["data"].ToObject<JArray>());
                return json;
            }
            catch (Exception ex)
            {
                if (jo != null)
                {
                    LogHelper.LogInfo(url + "\n" + jo.ToString(Formatting.Indented));
                    return Lib.APILib.Iced(jo);
                }

                LogHelper.LogInfo(url + "\n" + "return json is null");
                LogHelper.LogError("FreeHero", ex);
                return Lib.APILib.Error("接口 FreeHero 调用错误");
            }
        }

    }
}