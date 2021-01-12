using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using duretoryApi.App_Code;

namespace duretoryApi.Models
{
    public class FormorClass
    {
        public sItemModels GetSearchModels(userData userData, string cuurip)
        {
            database database = new database();
            DataTable mainRows = new DataTable();
            List<dbparam> dbparamlist = new List<dbparam>();
            dbparamlist.Add(new dbparam("@inoper", userData.userid.TrimEnd()));
            mainRows = database.checkSelectSql("mssql", "flybookstring", "exec web.searchmoduleform @inoper;", dbparamlist);
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new sItemModels() { status = "nodata" };
            }
            dbparamlist.Clear();
            List<Dictionary<string, object>> opticonitems = new List<Dictionary<string, object>>();
            foreach (DataRow drs in database.checkSelectSql("mssql", "sysstring", "exec web.searchitemform;", dbparamlist).Rows)
            {
                opticonitems.Add(new Dictionary<string, object>() { { "opticonPadding", drs["padding"].ToString().TrimEnd() == "1" }, { "icon", drs["icon"].ToString().TrimEnd() }, { "value", drs["value"].ToString().TrimEnd() } });
            }
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in mainRows.Rows)
            {
                List<Dictionary<string, object>> answeritems = new List<Dictionary<string, object>>();
                switch (dr["outValue"].ToString().TrimEnd())
                {
                    case "radio":
                    case "checkbox":
                    case "droplist":
                        dbparamlist.Clear();
                        DataTable subRows = new DataTable();
                        dbparamlist.Add(new dbparam("@iid", dr["iid"].ToString().TrimEnd()));
                        dbparamlist.Add(new dbparam("@inoper", userData.userid.TrimEnd()));
                        subRows = database.checkSelectSql("mssql", "flybookstring", "exec web.searchoptionform @iid,@inoper;", dbparamlist);
                        switch (subRows.Rows.Count)
                        {
                            case 0:
                                answeritems.Add(new Dictionary<string, object>() { { "id", 1 }, { "value", "" }, { "ansrDelete", false } });
                                break;
                            default:
                                foreach (DataRow drs in subRows.Rows)
                                {
                                    answeritems.Add(new Dictionary<string, object>() { { "id", drs["id"].ToString().TrimEnd() }, { "value", drs["value"].ToString().TrimEnd() }, { "ansrDelete", false } });
                                }
                                break;
                        }
                        break;
                }
                items.Add(new Dictionary<string, object>() { { "iid", dr["iid"].ToString().TrimEnd() }, { "showLine", false }, { "title", dr["title"].ToString().TrimEnd() }, { "showOut", false }, { "showDrop", false }, { "showFile", false }, { "outValue", dr["outValue"].ToString().TrimEnd() }, { "showShow", dr["showed"].ToString().TrimEnd() == "1" }, { "showCheck", dr["checked"].ToString().TrimEnd() == "1" }, { "showFilter", dr["filtered"].ToString().TrimEnd() == "1" }, { "showMore", false }, { "opticonitems", opticonitems.ToArray() }, { "answeritems", answeritems.ToArray() }, { "itemModify", false }, { "itemDelete", false } });
            }
            return new sItemModels() { items = items, status = "istrue" };
        }

        public statusModels GetInsertModels(iItemsData iItemsData, string cuurip)
        {
            database database = new database();
            foreach (var item in iItemsData.items)
            {
                List<dbparam> dbparamlist = new List<dbparam>();
                dbparamlist.Add(new dbparam("@iid", item["iid"].ToString().TrimEnd()));
                dbparamlist.Add(new dbparam("@inoper", iItemsData.newid.TrimEnd()));
                dbparamlist.Add(new dbparam("@title", item["title"].ToString().TrimEnd()));
                dbparamlist.Add(new dbparam("@outValue", item["outValue"].ToString().TrimEnd()));
                dbparamlist.Add(new dbparam("@showed", bool.Parse(item["showShow"].ToString().TrimEnd()) ? "1" : "0"));
                dbparamlist.Add(new dbparam("@checked", bool.Parse(item["showCheck"].ToString().TrimEnd()) ? "1" : "0"));
                dbparamlist.Add(new dbparam("@filtered", bool.Parse(item["showFilter"].ToString().TrimEnd()) ? "1" : "0"));
                if (database.checkActiveSql("mssql", "flybookstring", "exec web.checkmoduleform @iid,@inoper,@title,@outValue,@showed,@checked,@filtered;", dbparamlist) != "istrue")
                {
                    return new statusModels() { status = "error" };
                }
                switch (item["outValue"].ToString().TrimEnd())
                {
                    case "radio":
                    case "checkbox":
                    case "droplist":
                        foreach (var answeritem in JsonSerializer.Deserialize<List<Dictionary<string, object>>>(item["answeritems"].ToString().TrimEnd()))
                        {
                            dbparamlist.Clear();
                            dbparamlist.Add(new dbparam("@iid", item["iid"].ToString().TrimEnd()));
                            dbparamlist.Add(new dbparam("@id", answeritem["id"].ToString().TrimEnd()));
                            dbparamlist.Add(new dbparam("@inoper", iItemsData.newid.TrimEnd()));
                            dbparamlist.Add(new dbparam("@value", answeritem["value"].ToString().TrimEnd()));
                            if (database.checkActiveSql("mssql", "flybookstring", "exec web.checkoptionform @iid,@id,@inoper,@value;", dbparamlist) != "istrue")
                            {
                                return new statusModels() { status = "error" };
                            }
                        }
                        break;
                }
            }
            return new statusModels() { status = "istrue" };
        }
    }
}