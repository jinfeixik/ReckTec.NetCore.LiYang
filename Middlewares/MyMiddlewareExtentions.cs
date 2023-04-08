using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReckTec.NetCore.LiYang.Middlewares
{
    public static class MyMiddlewareExtentions
    {
        public static IApplicationBuilder UserGetRequestTimeMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<GetRequestTimeMiddleware>();

            return app;
        }
    }

    public class GetRequestTimeMiddleware
    {
        private readonly RequestDelegate _next;
        //日志
        private readonly ILogger _logger;
        //进入时间
        private const string key = "GetRequestTime";
        public GetRequestTimeMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<GetRequestTimeMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            DateTime starttime = DateTime.Now;
            if (!context.Request.Headers.ContainsKey("GetRequestTime"))
            {
                context.Request.Headers.Add("GetRequestTime", starttime.Ticks.ToString());//键 值
            }

            Console.WriteLine("中间件2开始执行");

            //监听运行   
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await _next(context);
            stopwatch.Stop();
            DateTime endtime = DateTime.Now;
            long enterTime = 0;
            long costTime = 0;
            //var costTime = string.Empty;

            //测试存取
            if (context.Request.Headers.TryGetValue(key, out var request_entertime))
            {
                enterTime = Convert.ToInt64(request_entertime);
                //Console.WriteLine(enterTime);
                //costTime = new TimeSpan(DateTime.Now.Ticks - enterTime).ToString();//转刻度数
                costTime = endtime.Ticks - enterTime;//单位是一百纳秒，即一千万分之一秒
                //Console.WriteLine(costTime);
            }
            //判断是否慢请求
            if (costTime > GetSingle())
            {
                _logger.LogInformation($"中间件2请求URL:{context.Request.Path.Value}，开始时间{starttime:yyyy-MM-dd HH:mm:ss.ffff},结束时间{endtime:yyyy-MM-dd HH:mm:ss.ffff}，耗时{Math.Round(Convert.ToDecimal(costTime)/100000,2)}ms,是慢请求");
            }
            else
            {
                //_logger.LogDebug($"中间件2请求URL:{context.Request.Path.Value}，开始时间{starttime:yyyy-MM-dd HH:mm:ss.ffff},结束时间{endtime:yyyy-MM-dd HH:mm:ss.ffff},不是慢请求");
                _logger.LogInformation($"中间件2请求URL:{context.Request.Path.Value}，开始时间{starttime:yyyy-MM-dd HH:mm:ss.ffff},结束时间{endtime:yyyy-MM-dd HH:mm:ss.ffff}，耗时{Math.Round(Convert.ToDecimal(costTime) / 100000, 2)}ms,不是慢请求");
            }
            Console.WriteLine("中间件2结束执行");
        }

        //获取配置文件中的值
        public long GetSingle()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();//创建ConfigurationBuilder对象
            //给configurationBuilder对象设置appsettings的路径
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuration = configurationBuilder.Build();
            //获取单独字段
            long name = configuration.GetValue<long>("LongTimeRequest");
            //string name1 = configuration["LongTimeRequest"];

            return name;
        }
    }
}
