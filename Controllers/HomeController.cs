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

        [HttpPost]
        [Route("filterData")]
        public JsonResult filterData([FromBody] userData userData)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return Json(new HomeClass().GetFilterModels(userData, clientip));
        }

        [HttpPost]
        [Route("scrollData")]
        public JsonResult scrollData([FromBody] sScollData sScollData)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return Json(new HomeClass().GetScrollModels(sScollData, clientip));
        }

        [HttpPost]
        [Route("sFilterData")]
        public JsonResult sFilterData([FromBody] sFiltData sFiltData)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return Json(new HomeClass().GetSFilterModels(sFiltData, clientip));
        }

        [HttpPost]
        [Route("deleteData")]
        public JsonResult deleteData([FromBody] dFormData dFormData)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return Json(new HomeClass().GetDeleteModels(dFormData, clientip));
        }

        [HttpPost]
        [Route("sItemData")]
        public JsonResult sItemData([FromBody] dFormData dFormData)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return Json(new HomeClass().GetSItemModels(dFormData, clientip));
        }

        [HttpPost]
        [Route("insertData")]
        public JsonResult insertData([FromBody] iFormData iFormData)
        {
            string clientip = Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd() == "::1" ? "127.0.0.1" : Request.HttpContext.Connection.RemoteIpAddress.ToString().TrimEnd();
            return Json(new HomeClass().GetInsertModels(iFormData, clientip));
        }
    }
}