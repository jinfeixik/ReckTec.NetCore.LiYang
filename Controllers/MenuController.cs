using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReckTec.NetCore.LiYang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace ReckTec.NetCore.LiYang.Controllers
{
    [ApiController]
    [Route("api/Menu")]
    public class MenuController : ControllerBase
    {

        public  MenuController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpGet]
        [Route("GetMenu")]
        public string GetMunu()
        {
            //return Configuration["menu"];

            //Json Section获取对象类型
            //var groups=Configuration.GetSection("Groups").Get<List<MenuGroup>>();
            //return JsonSerializer.Serialize(groups);

            //Json bind获取对象类型
            var options = new JsonSerializerOptions();
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All);
            var MenuGroup = new List<MenuGroup>();
            Configuration.GetSection("Groups").Bind(MenuGroup);
            return JsonSerializer.Serialize(new { title = MenuGroup }, options);

            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //return Encoding.GetEncoding("GB2312").GetString(System.Text.Encoding.Default.GetBytes(JsonSerializer.Serialize(MenuGroup)));
        }
    }
}
