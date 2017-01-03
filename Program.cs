using ConsoleUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace DaiWan.Tentacle
{
    class Program
    {
        static string Host = ConfigurationManager.AppSettings["Host"].ToString();

        static void Main(string[] args)
        {

            Console.WriteLine("启动中...");

          
            System.Timers.Timer timer = new System.Timers.Timer(1000 *60 * 60);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();



            string TentacleToken = Util.GetTentacleToken();
            bool c = Util.CheckToken(TentacleToken);


            Console.Clear();
            if (!c)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("本地TentacleToken:{0}",TentacleToken));
                Console.WriteLine("本地TentacleToken失效,请登录带玩平台重新申请！");
                Console.ReadLine();
            }
       
            var config = new HttpSelfHostConfiguration(Host); //配置主机

            config.Routes.MapHttpRoute(    //配置路由
                "API Default", "{action}",
                new
                {
                    controller = "Default",
                    id = RouteParameter.Optional
                });


            config.Routes.MapHttpRoute(    //配置路由
            "LolCore", "{controller}/{action}",
            new
            {
                controller = "LolCore",
                id = RouteParameter.Optional
            });


            config.Routes.MapHttpRoute(    //配置路由
            "KogCore", "{controller}/{action}",
            new
            {
                controller = "KogCore",
                id = RouteParameter.Optional
            });



            #region 注册服务器
            string HostName = System.Net.Dns.GetHostName(); //得到主机名
            Newtonsoft.Json.Linq.JObject jo = new Newtonsoft.Json.Linq.JObject{
                new Newtonsoft.Json.Linq.JProperty("TentacleToken",TentacleToken),
                new Newtonsoft.Json.Linq.JProperty("Date",DateTime.Now),
                new Newtonsoft.Json.Linq.JProperty("HostName",HostName)
            };
            Util.CallRemoteAPI_Post(TentacleToken, "http://tentacleapi.games-cube.com/Register", jo);
            #endregion


            using (HttpSelfHostServer server = new HttpSelfHostServer(config)) //监听HTTP
            {
                server.OpenAsync().Wait(); //开启来自客户端的请求

                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine(@"
 _____        ___          __                    _____ _____ 
|  __ \      (_) \        / /              /\   |  __ \_   _|
| |  | | __ _ _ \ \  /\  / /_ _ _ __      /  \  | |__) || |  
| |  | |/ _` | | \ \/  \/ / _` | '_ \    / /\ \ |  ___/ | |  
| |__| | (_| | |  \  /\  / (_| | | | |  / ____ \| |    _| |_ 
|_____/ \__,_|_|   \/  \/ \__,_|_| |_| /_/    \_\_|   |_____|
");

                Console.WriteLine(string.Format("本地TentacleToken:{0}", TentacleToken));
                Console.WriteLine("平台运行环境要求.net framework 4.5");
                Console.WriteLine("DAIWANLOL国服API触手版-V1.8");
                Console.WriteLine(string.Format("REST API 访问地址：{0}",Host));
                
                Console.WriteLine(@"API手册参考:http://api.games-cube.com/combat");
                Console.WriteLine("回车退出");
                Console.ReadLine();
            }
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string TentacleToken = Util.GetTentacleToken();
            bool c = Util.CheckToken(TentacleToken);
            if (!c)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("本地TentacleToken:{0}", TentacleToken));
                Console.WriteLine("本地TentacleToken失效,请登录带玩平台重新申请！");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
    }
}



