using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReckTec.NetCore.LiYang.Controllers
{
    public class TestController: ControllerBase
    {
        [HttpPost]
        [Authorize]
        [Route("Test1")]
        public string Test1()
        {
            return "Test1";
        }

        [HttpPost]
        [Authorize]
        [Route("Test2")]
        public string Test2()
        {
            return "Test2";
        }

        [HttpPost]
        [Authorize]
        [Route("Test3")]
        public string Test3()
        {
            return "Test3";
        }
    }
}
