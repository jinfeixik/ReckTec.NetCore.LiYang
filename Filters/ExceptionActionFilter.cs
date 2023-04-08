using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReckTec.NetCore.LiYang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReckTec.NetCore.LiYang.Filters
{
    public class ExceptionActionFilter : ActionFilterAttribute
    {
        private ILogger<ExceptionActionFilter> _logger;
        //构造注入日志组件
        public ExceptionActionFilter(ILogger<ExceptionActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            Console.WriteLine(context.ActionDescriptor.DisplayName + "异常过滤器执行");
            var e = context.Exception;
            //日志收集
            _logger.LogError(e, e.Message + "\r\n" + e.StackTrace);
            //异常返回

            var resultJson = new
            BaseResult
            {
                ErrorCode = 0,
                Message = "执行出错",
                Data = e
            };

            ContentResult con = new ContentResult();
            con.Content = JsonConvert.SerializeObject(resultJson);
            context.Result = con;
            context.ExceptionHandled = true;
            //context.ExceptionHandled = true;
            //context.HttpContext.Response.WriteAsync($"CustomExceptionFilterAttribute错误:{context.Exception.Message}");
            //OnException(context);

        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var exception = context.Exception;
            if (exception != null)
            {
                context.ExceptionHandled = true;
                ContentResult con = new ContentResult();
                con.Content = JsonConvert.SerializeObject(new
                BaseResult
                {
                    ErrorCode = 1,
                    Message = "执行出错",
                    Data = exception.Message
                });
                context.Result = con;
            }
            base.OnActionExecuted(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var exception = context.Exception;
            if (exception != null)
            {
                context.ExceptionHandled = true;
                context.HttpContext.Response.WriteAsync($"错误 : {exception.Message}");
            }
            base.OnResultExecuted(context);
        }
    }
}
