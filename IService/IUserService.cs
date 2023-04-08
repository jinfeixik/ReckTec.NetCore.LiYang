using ReckTec.NetCore.LiYang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReckTec.NetCore.LiYang.IService
{
    public interface IUserService
    {
        public void Add(UserInfo model);

        public void Delete(string id);

        public UserInfo GetById(string id);

        public IList<UserInfo> GetAll();

        public void Update(UserInfo model);
    }
}
