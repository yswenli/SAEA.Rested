/****************************************************************************
*项目名称：SAEA.RESTED.Models
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.RESTED.Models
*类 名 称：ResponseResult
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2021/2/23 15:26:17
*描述：
*=====================================================================
*修改时间：2021/2/23 15:26:17
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAEA.RESTED.Models
{
    public class ResponseResult
    {
        public int Code { get; set; }

        public string Headers { get; set; }

        public string Body { get; set; }
    }
}
