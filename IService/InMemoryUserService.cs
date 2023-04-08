using ReckTec.NetCore.LiYang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReckTec.NetCore.LiYang.IService
{
    public class InMemoryUserService : IUserService
    {
        private List<UserInfo> _userInfoList;

        public InMemoryUserService()
        {
            this._userInfoList = new List<UserInfo>();
        }

        public void Add(UserInfo model)
        {
            _userInfoList.Add(model);
        }

        public void Delete(string id)
        {
            var user = _userInfoList.FirstOrDefault(o => o.UserId == id);
            if (user == null)
            {
                throw new Exception("找不到记录");
            }
            _userInfoList.Remove(user);
        }

        public IList<UserInfo> GetAll()
        {
            return _userInfoList;
        }

        public UserInfo GetById(string id)
        {
            return _userInfoList.FirstOrDefault(o => o.UserId == id);
        }

        public void Update(UserInfo newModel)
        {
            var oldModel = _userInfoList.FirstOrDefault(o => o.UserId == newModel.UserId);

            if (oldModel == null)
            {
                throw new Exception("找不到可更新的记录");
            }
            oldModel.UserName = newModel.UserName;
            oldModel.Age = newModel.Age;
            oldModel.Age = newModel.Age;
            oldModel.PermissionList = newModel.PermissionList;
        }
    }
}
