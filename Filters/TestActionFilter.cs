using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using ReckTec.NetCore.LiYang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReckTec.NetCore.LiYang.Filters
{
    public class TestActionFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine(context.ActionDescriptor.DisplayName+"动作过滤器执行前");

        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine(context.ActionDescriptor.DisplayName + "动作过滤器执行后");
            context.ExceptionHandled = true;

            var result = context.Result as ObjectResult;
            
            if (result != null)
            {
                var resultJson = new
                BaseResult{
                    ErrorCode = 0,
                    Message = "执行成功",
                    Data = result
                };
                ContentResult con = new ContentResult();
                con.Content = JsonConvert.SerializeObject(resultJson);
                context.Result = con;
            }
        }
    }
}
