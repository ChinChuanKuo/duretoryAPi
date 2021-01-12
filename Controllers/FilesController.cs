using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using duretoryApi.Models;
using duretoryApi.App_Code;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;
using System.IO;

namespace duretoryApi.Controllers
{
    [EnableCors("Files")]
    [ApiController]
    [Route("[controller]")]
    public class FilesController : Controller
    {
        [HttpPost]
        [Route("uploadData")]
        public async Task<JsonResult> uploadData()
        {
            if (Request.Form.Files.Count > 0)
            {
                string original = Request.Form.Files[0].FileName.Substring(0, Request.Form.Files[0].FileName.LastIndexOf('.')), encryption = new sha256().new256("mssql", "sysstring"), extension = Path.GetExtension(Request.Form.Files[0].FileName);
                database database = new database();
                DataTable mainRows = new DataTable();
                List<dbparam> dbparamlist = new List<dbparam>();
                dbparamlist.Add(new dbparam("@value", extension.Replace(".", "").Trim().ToLower()));
                dbparamlist.Add(new dbparam("@needed", "1"));
                mainRows = database.checkSelectSql("mssql", "sysstring", "exec web.uploadfileform @value,@needed;", dbparamlist);
                switch (mainRows.Rows.Count)
                {
                    case 0:
                        return Json(new sSiteModels() { status = "nodata" });
                }
                if (mainRows.Rows[0]["flImages"].ToString().TrimEnd() == "0" && mainRows.Rows[0]["flVideos"].ToString().TrimEnd() == "0" && mainRows.Rows[0]["flAudios"].ToString().TrimEnd() == "0")
                {
                    return Json(new sSiteModels() { status = "nodata" });
                }
                string filePath = $"{database.connectionString("sysFiles")}5E28F1D7-D153-430F-814D-82D6AD7C4E93\\";
                Directory.CreateDirectory(filePath);
                using (var fileStream = new FileStream($"{filePath}{original}({encryption}){extension}", FileMode.Create))
                {
                    await Request.Form.Files[0].CopyToAsync(fileStream);
                    string src = mainRows.Rows[0]["flImages"].ToString().TrimEnd() == "1" && mainRows.Rows[0]["flShowed"].ToString().TrimEnd() == "0" ? $"{database.connectionString("sysFiles")}{mainRows.Rows[0]["original"].ToString().TrimEnd()}({mainRows.Rows[0]["encryption"].ToString().TrimEnd()}){mainRows.Rows[0]["extension"].ToString().TrimEnd()}" : $"{filePath}{original}({encryption}){extension}";
                    return Json(new sSiteModels() { images = mainRows.Rows[0]["flImages"].ToString().TrimEnd() == "1", videos = mainRows.Rows[0]["flVideos"].ToString().TrimEnd() == "1", audios = mainRows.Rows[0]["flAudios"].ToString().TrimEnd() == "1", files = System.IO.File.ReadAllBytes(src), status = "istrue" });
                }
            }
            return Json(new sSiteModels() { status = "nodata" });
        }
    }
}
