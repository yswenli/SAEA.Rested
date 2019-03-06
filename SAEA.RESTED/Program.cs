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

            SAEAMvcApplicationConfig mvcApplicationConfig = SAEAMvcApplicationConfigBuilder.Read();

            SAEAMvcApplication mvcApplication = new SAEAMvcApplication(mvcApplicationConfig);            

            mvcApplication.Start();

            Console.WriteLine("SAEA.RESTED 服务已启动，回车结束服务.");

            Process.Start("http://localhost:39654/");

            Console.ReadLine();

        }
    }
}
