using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReckTec.NetCore.LiYang.Models
{
    public class UserInfo
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public int Age { get; set; }

        // PermissionList用于存放该用户能访问的URL
        public IList<string> PermissionList { get; set; }
    }
}
