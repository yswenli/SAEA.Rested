/****************************************************************************
*项目名称：SAEA.RESTED.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.RESTED.Libs
*类 名 称：WebClientHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/6 11:04:35
*描述：
*=====================================================================
*修改时间：2019/3/6 11:04:35
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Net;
using System.Text;

namespace SAEA.RESTED.Libs
{
    public class WebClientHelper
    {
        public static string Get(string url)
        {
            try
            {
                using (var webClient = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    return webClient.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string Post(string url, string json = "")
        {
            try
            {
                using (var webClient = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    webClient.Headers["ContentType"] = "application/json";
                    return Encoding.UTF8.GetString(webClient.UploadData(url, "POST", Encoding.UTF8.GetBytes(json)));
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

    }
}
