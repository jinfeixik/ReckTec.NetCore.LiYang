using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReckTec.NetCore.LiYang.Filters
{
    public class ResultActionFilter: Attribute, IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            // 在结果执行之后调用的操作...      
            Console.WriteLine(context.ActionDescriptor.DisplayName + "结果过滤器执行前");
            //var result = context.Result;
            //Console.WriteLine(result);
            //if (result != null && result is JsonResult resulta)
            //{
            //    Console.WriteLine(resulta.Value);
            //    context.Result = new JsonResult(new BaseResult(resulta.Value));
            //}

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {

            // 在结果执行之前调用的一系列操作
            Console.WriteLine(context.ActionDescriptor.DisplayName + "结果过滤器执行后");
            //往请求头中加入Author=From Ron.liang
            //context.HttpContext.Response.Headers.Add("Author", "From Ron.liang");
        }
    }
}
