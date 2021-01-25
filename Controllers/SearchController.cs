using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using duretoryApi.Models;

namespace duretoryApi.Controllers
{
    [EnableCors("Search")]
    [ApiController]
    [Route("[controller]")]
    public class SearchController : Controller
    {
        [HttpPost]
        [Route("searchData")]
        public JsonResult searchData([FromBody] sRowsData sRowsData)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return Json(new SearchClass().GetSearchModels(sRowsData, clientip));
        }

        [HttpPost]
        [Route("filterData")]
        public JsonResult filterData([FromBody] dFormData dFormData)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return Json(new SearchClass().GetFilterModels(dFormData, clientip));
        }
    }
}