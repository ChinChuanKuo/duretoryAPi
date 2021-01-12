using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using duretoryApi.Models;

namespace duretoryApi.Controllers
{
    [EnableCors("Home")]
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpPost]
        [Route("searchData")]
        public JsonResult searchData([FromBody] otherData otherData)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return Json(new HomeClass().GetSearchModels(otherData, clientip));
        }
    }
}