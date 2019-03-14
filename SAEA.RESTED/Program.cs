/****************************************************************************
*项目名称：SAEA.RESTED
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.RESTED
*类 名 称：Program
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/12 10:31:06
*描述：
*=====================================================================
*修改时间：2019/3/12 10:31:06
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;
using SAEA.MVC;
using SAEA.RESTED.Libs;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;

namespace SAEA.RESTED
{
    class Program
    {

        private static readonly string Name = "SAEA.RESTED Service";
        private static readonly string Display = "SAEA.RESTED Service";
        private static readonly string Description = "这是SAEA.RESTED 服务，关闭此服务将无法访问SAEA.RESTED。";
        private static readonly string FilePath = Assembly.GetExecutingAssembly().Location;


        public static string VersionType = "Console版";

        static void ConsoleStart()
        {
            Console.Title = "SAEA.RESTED";

            ConsoleHelper.WriteLine("SAEA.RESTED 服务正在启动中...");

            SAEAMvcApplicationConfig mvcApplicationConfig = SAEAMvcApplicationConfigBuilder.Read();

            SAEAMvcApplication mvcApplication = new SAEAMvcApplication(mvcApplicationConfig);

            mvcApplication.Start();

            ConsoleHelper.WriteLine("SAEA.RESTED 服务已启动，请在浏览器中输入 \r\n\t\t\t http://localhost:39654/");

            ConsoleHelper.WriteLine("回车结束服务");

            Process.Start("http://localhost:39654/");

            Console.ReadLine();
        }

        static SAEARESTEDService _service;

        /// <summary>
        /// 启动入口
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                #region 参数相关功能

                if (args.Length != 0)
                {
                    switch (args[0].ToUpper())
                    {
                        case "/I":
                            WinServiceHelper.InstallAndStart(FilePath, Name, Display, Description);
                            return;
                        case "/U":
                            WinServiceHelper.Unstall(Name);
                            return;
                        default:
                            Console.WriteLine("args:");
                            Console.WriteLine("\t/i\t\t 安装服务");
                            Console.WriteLine("\t/u\t\t 卸载服务");
                            return;
                    }
                }
                else
                {
                    try
                    {
#if DEBUG
                        ConsoleStart();
#else
                        VersionType = "WinService版";
                        _service = new SAEARESTEDService();
                        _service.Run();
#endif

                    }
                    catch (Exception ex)
                    {
                        SystemLoger("启动失败，原因：" + ex.Message + ex.Source);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                SystemLoger("启动失败，原因：" + ex.Message + ex.Source);
            }

        }

        private static readonly string SysLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"SAEA.RESTED.Service.log");

        private static void SystemLoger(string message)
        {
            File.AppendAllText(SysLogPath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + message + "\r\n");
        }
    }

}
