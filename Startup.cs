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
        //������ע�����
        public void ConfigureServices(IServiceCollection services)
        {
            //���������������
            //services.AddControllers().AddNewtonsoftJson();
            //services.AddControllers().AddJsonOptions(options =>
            //{
            //    //options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            //    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            //});

            //ȫ�ֹ�����
            services.AddControllers(options=> {
                options.Filters.Add<ExceptionActionFilter>();
                options.Filters.Add(typeof(TestActionFilter));
                options.Filters.Add(typeof(ResultActionFilter));
            });
            //AddScoped
            services.AddSingleton<IUserService,InMemoryUserService>();

            //JWT��Ȩ��֤
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



            //ע��swagger����,����1�����߶��swagger�ĵ�
            services.AddSwaggerGen(s =>
            {
                //����swagger�ĵ������Ϣ
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "NoteWebApi�ĵ�",
                    Description = "Note��Ǳ���¼�ӿ��ĵ�",
                    Version = "v1.0"
                });

                //��ȡxmlע���ļ���Ŀ¼
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
                // ����xmlע��
                s.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //�м������
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

            //�м��1
            app.Use(async (context, next) =>
            {
                Console.WriteLine("�м��1��ʼִ��");
                DateTime starttime = DateTime.Now;
                //��������
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                await next();
                stopwatch.Stop();
                DateTime endtime = DateTime.Now;
                Console.WriteLine($"�м��1����URL:{context.Request.Path.Value}����ʼʱ��{starttime:yyyy-MM-dd HH:mm:ss.ffff},����ʱ��{endtime:yyyy-MM-dd HH:mm:ss.ffff}");
                Console.WriteLine("�м��1����ִ��");
            });

            //�м��2
            app.UserGetRequestTimeMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //����swagger�м��
            app.UseSwagger();
            //����SwaggerUI�м����htlm css js�ȣ�������swagger json ���
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "NoteWebapi�ĵ�v1");
                //Ҫ��Ӧ�õĸ� (http://localhost:&lt;port&gt;/) ���ṩ Swagger UI���뽫 RoutePrefix ��������Ϊ���ַ�����
                //s.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                //���ô�ͳ·��
                endpoints.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");

                //��������·��
                endpoints.MapControllers();
            });
        }
    }
}
