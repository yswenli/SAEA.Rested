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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SAEA.RESTED.Libs
{
    public static class WebClientHelper
    {

        static WebClientHelper()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            ServicePointManager.Expect100Continue = true;
        }


        public static string Get(string url, string header, out string reHeaders)
        {
            reHeaders = "";
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None;
                request.Method = "GET";
                SetHeaders(header, ref request);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                reHeaders = GetHeaders(response);
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string Post(string url, string header, string json, out string reHeaders)
        {
            reHeaders = "";
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None;
                request.Method = "POST";
                SetHeaders(header, ref request);
                byte[] buffer = encoding.GetBytes(json);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                reHeaders = GetHeaders(response);
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }



        static void SetHeaders(string header, ref HttpWebRequest request)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            try
            {
                var arr = header.Split('\n');

                if (arr != null)
                {
                    foreach (var item in arr)
                    {
                        if (!string.IsNullOrWhiteSpace(item) && item.Substring(0, 1) == ":")
                        {
                            var index = item.LastIndexOf(":");
                            var k = item.Substring(0, index);
                            var v = item.Substring(index + 1);
                            keyValuePairs[k] = string.IsNullOrEmpty(v) ? "" : v.Trim();
                        }
                        else
                        {
                            var kvs = item.Split(':');
                            keyValuePairs[kvs[0].ToLower()] = string.IsNullOrEmpty(kvs[1]) ? "" : kvs[1].Trim();
                        }
                    }

                    if (keyValuePairs.ContainsKey("accept"))
                    {
                        request.Accept = keyValuePairs["accept"];

                        keyValuePairs.Remove("accept");
                    }

                    if (keyValuePairs.ContainsKey("content-type"))
                    {
                        request.ContentType = keyValuePairs["content-type"];

                        keyValuePairs.Remove("content-type");
                    }

                    if (keyValuePairs.ContainsKey("user-agent"))
                    {
                        request.UserAgent = keyValuePairs["user-agent"];

                        keyValuePairs.Remove("user-agent");
                    }

                    if (keyValuePairs.ContainsKey("host"))
                    {
                        request.Host = keyValuePairs["host"];

                        keyValuePairs.Remove("host");
                    }

                    if (keyValuePairs.ContainsKey("connection"))
                    {
                        keyValuePairs.Remove("connection");
                    }

                    if (keyValuePairs.ContainsKey("referer"))
                    {
                        request.Referer = keyValuePairs["referer"];
                        keyValuePairs.Remove("referer");
                    }

                    if (keyValuePairs.ContainsKey("cookie"))
                    {
                        var cc = new CookieContainer();

                        var carr = keyValuePairs["cookie"].Split(';');

                        foreach (var sitem in carr)
                        {
                            var ckv = sitem.Split('=');

                            if (ckv.Length == 2)
                            {
                                cc.Add(new Cookie(ckv[0].Trim(), ckv[1], "/", request.Host));
                            }
                            else
                            {
                                cc.Add(new Cookie(ckv[0].Trim(), ""));
                            }
                        }

                        request.CookieContainer = cc;

                        keyValuePairs.Remove("cookie");
                    }

                    foreach (var item in keyValuePairs)
                    {
                        try
                        {
                            request.Headers.Set(item.Key, item.Value);
                        }
                        catch (Exception exp)
                        {

                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        static string GetHeaders(HttpWebResponse response)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in response.Headers)
            {
                sb.AppendLine($"{key}:{response.Headers[key]}");
            }
            return sb.ToString();
        }
    }
}
