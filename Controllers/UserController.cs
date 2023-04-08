using Microsoft.AspNetCore.Mvc;
using ReckTec.NetCore.LiYang.IService;
using ReckTec.NetCore.LiYang.Models;

namespace ReckTec.NetCore.LiYang.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController:ControllerBase
    {
        public readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(userService.GetAll());
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add(UserInfo model)
        {
            userService.Add(model);

            return Ok();
        }

        [HttpGet("id")]
        [Route("GetById")]
        public IActionResult GetById(string id)
        {
            return Ok(userService.GetById(id));   
        }

        [HttpGet("id")]
        [Route("Delete")]
        public IActionResult Delete(string id)
        {
            userService.Delete(id);
            return Ok();
        }

        [HttpPost]
        [Route("Update")]
        public IActionResult Update(UserInfo model)
        {
            userService.Update(model);
            return Ok();
        }


    }
}
