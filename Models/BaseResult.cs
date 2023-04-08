using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReckTec.NetCore.LiYang.Models
{
    public class BaseResult
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 信息提示
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
    }
}
