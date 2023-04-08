using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReckTec.NetCore.LiYang.Filters;
using ReckTec.NetCore.LiYang.IService;
using ReckTec.NetCore.LiYang.Middlewares;
using ReckTec.NetCore.LiYang.Models;

namespace ReckTec.NetCore.LiYang
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //往容器注入服务
        public void ConfigureServices(IServiceCollection services)
        {
            //解决中文乱码问题
            //services.AddControllers().AddNewtonsoftJson();
            //services.AddControllers().AddJsonOptions(options =>
            //{
            //    //options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            //    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            //});

            //全局过滤器
            services.AddControllers(options=> {
                options.Filters.Add<ExceptionActionFilter>();
                options.Filters.Add(typeof(TestActionFilter));
                options.Filters.Add(typeof(ResultActionFilter));
            });
            //AddScoped
            services.AddSingleton<IUserService,InMemoryUserService>();

            //JWT授权认证
            services.AddAuthentication("Bearer").AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("SecretKey"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };
            });



            //注册swagger服务,定义1个或者多个swagger文档
            services.AddSwaggerGen(s =>
            {
                //设置swagger文档相关信息
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NoteWebApi文档",
                    Description = "Note随记备忘录接口文档",
                    Version = "v1.0"
                });

                //获取xml注释文件的目录
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                // 启用xml注释
                s.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //中间件配置
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //中间件1
            app.Use(async (context, next) =>
            {
                Console.WriteLine("中间件1开始执行");
                DateTime starttime = DateTime.Now;
                //监听运行
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                await next();
                stopwatch.Stop();
                DateTime endtime = DateTime.Now;
                Console.WriteLine($"中间件1请求URL:{context.Request.Path.Value}，开始时间{starttime:yyyy-MM-dd HH:mm:ss.ffff},结束时间{endtime:yyyy-MM-dd HH:mm:ss.ffff}");
                Console.WriteLine("中间件1结束执行");
            });

            //中间件2
            app.UserGetRequestTimeMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //启用swagger中间件
            app.UseSwagger();
            //启用SwaggerUI中间件（htlm css js等），定义swagger json 入口
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "NoteWebapi文档v1");
                //要在应用的根 (http://localhost:&lt;port&gt;/) 处提供 Swagger UI，请将 RoutePrefix 属性设置为空字符串：
                //s.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                //配置传统路由
                endpoints.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");

                //启用特性路由
                endpoints.MapControllers();
            });
        }
    }
}
