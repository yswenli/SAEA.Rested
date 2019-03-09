using SAEA.Common;
using SAEA.MVC;
using System;
using System.Diagnostics;

namespace SAEA.RESTED
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SAEA.RESTED";

            ConsoleHelper.WriteLine("SAEA.RESTED 服务正在启动中...");

            SAEAMvcApplicationConfig mvcApplicationConfig = SAEAMvcApplicationConfigBuilder.Read();

            SAEAMvcApplication mvcApplication = new SAEAMvcApplication(mvcApplicationConfig);            

            mvcApplication.Start();

            ConsoleHelper.WriteLine("SAEA.RESTED 服务已启动，请在浏览器中输入 http://localhost:39654/");

            ConsoleHelper.WriteLine("回车结束服务");

            Process.Start("http://localhost:39654/");

            Console.ReadLine();

        }
    }
}
