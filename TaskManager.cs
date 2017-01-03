using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaiWan.LolTentacle
{
  public static  class TaskManager
    {
        static int count = 0;
     static   public void Process(string par)
        {

            Task t = new Task(() => {
                ++count;
                Console.WriteLine(count);
                //Console.Read();
            });
            t.Start();
        }
    }
}
