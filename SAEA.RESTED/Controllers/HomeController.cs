﻿/****************************************************************************
*项目名称：SAEA.RESTED.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.RESTED.Controllers
*类 名 称：HomeController
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
using SAEA.Common;
using SAEA.Common.Serialization;
using SAEA.Http;
using SAEA.MVC;
using SAEA.RESTED.Libs;
using SAEA.RESTED.Models;

namespace SAEA.RESTED.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult GetList()
        {
            string groupID = string.Empty;

            HttpContext.Current.Request.Parmas.TryGetValue("groupID", out groupID);

            return Content(RecordDataHelper.GetRightList(groupID));
        }


        public ActionResult AddGroup(string groupName)
        {
            var result = "0";
            if (!string.IsNullOrWhiteSpace(groupName))
            {
                groupName = HttpUtility.UrlDecode(groupName);

                if (groupName.Length <= 13)
                {
                    RecordDataHelper.AddGroup(groupName);
                }
                result = "1";
            }
            return Content(result);
        }

        public ActionResult GetGroups()
        {
            return Content(RecordDataHelper.GetGroups());
        }

        public ActionResult AddItem(string groupID, string title, string method, string url, string header, string json)
        {
            var result = "0";
            if (!string.IsNullOrWhiteSpace(groupID) && !string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(method) && !string.IsNullOrWhiteSpace(url))
            {
                title = HttpUtility.UrlDecode(title);

                url = HttpUtility.UrlDecode(url);

                if (title.Length <= 15 && url.Length <= 2083)
                {
                    if (!string.IsNullOrWhiteSpace(header))
                    {
                        header = HttpUtility.UrlDecode(header);
                    }

                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        json = HttpUtility.UrlDecode(json);
                    }

                    RecordDataHelper.AddItem(groupID, title, method, url, header, json);

                    result = "1";
                }
            }
            return Content(result);
        }

        public ActionResult UpdateGroup(string groupID, string groupName)
        {
            var result = "0";
            if (!string.IsNullOrWhiteSpace(groupID) && !string.IsNullOrWhiteSpace(groupName))
            {
                groupName = HttpUtility.UrlDecode(groupName);

                RecordDataHelper.UpdateGroup(groupID, groupName);

                result = "1";
            }
            return Content(result);
        }


        public ActionResult RemoveGroup(string groupID)
        {
            var result = "0";
            if (!string.IsNullOrWhiteSpace(groupID))
            {
                RecordDataHelper.RemoveGroup(groupID);

                result = "1";
            }
            return Content(result);
        }

        public ActionResult RemoveItem(string groupID, string itemID)
        {
            var result = "0";
            if (!string.IsNullOrWhiteSpace(groupID) && !string.IsNullOrWhiteSpace(itemID))
            {
                RecordDataHelper.RemoveItem(groupID, itemID);

                result = "1";
            }
            return Content(result);
        }


        public ActionResult GetItem(string groupID, string itemID)
        {
            ListItem listItem = new ListItem();

            if (!string.IsNullOrWhiteSpace(groupID) && !string.IsNullOrWhiteSpace(itemID))
            {
                listItem = RecordDataHelper.GetItem(groupID, itemID);
            }

            if (listItem == null)
                return Content(string.Empty);
            return Json(listItem);
        }


        public ActionResult Search(string keywords)
        {
            return Content(RecordDataHelper.Search(keywords));
        }



        public ActionResult Import(string json)
        {
            var result = "0";

            try
            {
                if (!string.IsNullOrWhiteSpace(json))
                {
                    json = HttpUtility.UrlDecode(json);

                    var rd = Deserialize<RecordData>(json);

                    if (rd != null)
                    {
                        RecordDataHelper.Write(rd);

                        result = "1";
                    }
                }
            }
            catch { }

            return Content(result);
        }

        public ActionResult Export()
        {
            return Json(RecordDataHelper.Read());
        }

        public ActionResult GetVersion()
        {
            return Content(Program.VersionType);
        }



        public ActionResult Request(string method, string url, string data)
        {
            ResponseResult result = new ResponseResult();

            if (!string.IsNullOrWhiteSpace(method) && !string.IsNullOrWhiteSpace(url))
            {
                url = HttpUtility.UrlDecode(url);
            }

            var requestData = new RequestData();

            if (!string.IsNullOrWhiteSpace(data))
            {
                data = HttpUtility.UrlDecode(data);

                requestData = SerializeHelper.Deserialize<RequestData>(data);
            }

            string headers = "";
            string body = "";

            if (method == "POST")
            {
                body = WebClientHelper.Post(url, requestData.Header, requestData.Body,out headers);
            }
            else
            {
                body = WebClientHelper.Get(url, requestData.Header, out headers);
            }
            result.Headers = headers;
            result.Body = body;
            result.Code = string.IsNullOrEmpty(headers) ? 0 : 1;
            return Json(result);
        }
    }
}
