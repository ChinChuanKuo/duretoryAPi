using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using duretoryApi.App_Code;

namespace duretoryApi.Models
{
    public class CreateClass
    {
        public sItemModels GetSearchModels(userData userData, string cuurip)
        {
            database database = new database();
            DataTable mainRows = new DataTable();
            List<dbparam> dbparamlist = new List<dbparam>();
            mainRows = database.checkSelectSql("mssql", "flybookstring", "exec web.searchallmoduleform;", dbparamlist);
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new sItemModels() { status = "nodata" };
            }
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in mainRows.Rows)
            {
                List<Dictionary<string, object>> optionitems = new List<Dictionary<string, object>>(), answeritems = new List<Dictionary<string, object>>(), collectitems = new List<Dictionary<string, object>>();
                switch (dr["outValue"].ToString().TrimEnd())
                {
                    case "radio":
                    case "checkbox":
                        dbparamlist.Clear();
                        dbparamlist.Add(new dbparam("@iid", dr["iid"].ToString().TrimEnd()));
                        foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", "exec web.searchalloptionform @iid;", dbparamlist).Rows)
                        {
                            answeritems.Add(new Dictionary<string, object>() { { "id", drs["id"].ToString().TrimEnd() }, { "value", drs["value"].ToString().TrimEnd() }, { "showAnswer", false } });
                        }
                        break;
                    case "droplist":
                        dbparamlist.Clear();
                        dbparamlist.Add(new dbparam("@iid", dr["iid"].ToString().TrimEnd()));
                        foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", "exec web.searchalloptionform @iid;", dbparamlist).Rows)
                        {
                            optionitems.Add(new Dictionary<string, object>() { { "optionPadding", false }, { "value", drs["value"].ToString().TrimEnd() } });
                        }
                        break;
                    case "collections":
                        dbparamlist.Clear();
                        dbparamlist.Add(new dbparam("@iid", dr["iid"].ToString().TrimEnd()));
                        foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", "exec web.searchalloptionform @iid;", dbparamlist).Rows)
                        {
                            collectitems.Add(new Dictionary<string, object>() { { "id", drs["id"].ToString().TrimEnd() }, { "collImage", true }, { "collVideo", false }, { "collAudio", false }, { "value", drs["value"].ToString().TrimEnd() }, { "collInsert", false }, { "collDelete", false } });
                        }
                        break;
                }
                items.Add(new Dictionary<string, object>() { { "iid", dr["iid"].ToString().TrimEnd() }, { "title", dr["title"].ToString().TrimEnd() }, { "values", "" }, { "showMenu", false }, { "showDrop", false }, { "showFile", false }, { "showImage", false }, { "showVideo", false }, { "showAudio", false }, { "outValue", dr["outValue"].ToString().TrimEnd() }, { "showShow", dr["showed"].ToString().TrimEnd() == "1" }, { "showCheck", dr["checked"].ToString().TrimEnd() == "1" }, { "showFilter", dr["filtered"].ToString().TrimEnd() == "1" }, { "collectitems", collectitems.ToArray() }, { "optionitems", optionitems.ToArray() }, { "answeritems", answeritems.ToArray() } });
            }
            return new sItemModels() { items = items, status = "istrue" };
        }

        public statusModels GetInsertModels(iItemsData iItemsData, string cuurip)
        {
            string checkValue = checkFormItem(iItemsData.items);
            if (checkValue != "")
            {
                return new statusModels() { status = checkValue };
            }
            database database = new database();
            DataTable mainRows = new DataTable();
            List<dbparam> dbparamlist = new List<dbparam>();
            dbparamlist.Add(new dbparam("@inoper", iItemsData.newid.TrimEnd()));
            dbparamlist.Add(new dbparam("@attribute", iItemsData.items[0]["values"].ToString().TrimEnd()));
            dbparamlist.Add(new dbparam("@category", iItemsData.items[1]["values"].ToString().TrimEnd()));
            dbparamlist.Add(new dbparam("@customer", iItemsData.items[2]["values"].ToString().TrimEnd()));
            dbparamlist.Add(new dbparam("@sotime", iItemsData.items[3]["values"].ToString().TrimEnd()));
            dbparamlist.Add(new dbparam("@model", iItemsData.items[4]["values"].ToString().TrimEnd()));
            dbparamlist.Add(new dbparam("@collection", iItemsData.items[5]["values"].ToString().TrimEnd()));
            dbparamlist.Add(new dbparam("@mb", iItemsData.items[6]["values"].ToString().TrimEnd()));
            dbparamlist.Add(new dbparam("@sample", iItemsData.items[7]["values"].ToString().TrimEnd()));
            dbparamlist.Add(new dbparam("@species", iItemsData.items[8]["values"].ToString().TrimEnd()));
            dbparamlist.Add(new dbparam("@count", iItemsData.items[9]["values"].ToString().TrimEnd()));
            dbparamlist.Add(new dbparam("@designer", iItemsData.items[10]["values"].ToString().TrimEnd()));
            mainRows = database.checkSelectSql("mssql", "flybookstring", "exec web.insertmainform @inoper,@attribute,@category,@customer,@sotime,@model,@collection,@mb,@sample,@species,@count,@designer;", dbparamlist);
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new statusModels() { status = "error" };
            }
            foreach (var item in iItemsData.items)
            {
                switch (item["outValue"].ToString().TrimEnd())
                {
                    case "collections":
                        foreach (var collectitem in JsonSerializer.Deserialize<List<Dictionary<string, object>>>(item["collectitems"].ToString().TrimEnd()))
                        {
                            switch (bool.Parse(collectitem["collDelete"].ToString().TrimEnd()))
                            {
                                case true:
                                    dbparamlist.Clear();
                                    dbparamlist.Add(new dbparam("@formId", mainRows.Rows[0]["formId"].ToString().TrimEnd()));
                                    dbparamlist.Add(new dbparam("@id", collectitem["id"].ToString().TrimEnd()));
                                    if (database.checkActiveSql("mssql", "flybookstring", "exec web.deletesubform @formId,@id;", dbparamlist) != "istrue")
                                    {
                                        return new statusModels() { status = "error" };
                                    }
                                    break;
                                default:
                                    switch (bool.Parse(collectitem["collInsert"].ToString().TrimEnd()))
                                    {
                                        case true:
                                            dbparamlist.Clear();
                                            dbparamlist.Add(new dbparam("@formId", mainRows.Rows[0]["formId"].ToString().TrimEnd()));
                                            dbparamlist.Add(new dbparam("@id", collectitem["id"].ToString().TrimEnd()));
                                            dbparamlist.Add(new dbparam("@inoper", iItemsData.newid.TrimEnd()));
                                            dbparamlist.Add(new dbparam("@value", collectitem["value"].ToString().TrimEnd()));
                                            if (database.checkActiveSql("mssql", "flybookstring", "exec web.insertsubform @formId,@id,@inoper,@value;", dbparamlist) != "istrue")
                                            {
                                                return new statusModels() { status = "error" };
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }
                        break;
                    case "radio":
                    case "checkbox":
                        foreach (var answeritem in JsonSerializer.Deserialize<List<Dictionary<string, object>>>(item["answeritems"].ToString().TrimEnd()))
                        {
                            switch (bool.Parse(answeritem["showAnswer"].ToString().TrimEnd()))
                            {
                                case true:
                                    dbparamlist.Clear();
                                    dbparamlist.Add(new dbparam("@formId", mainRows.Rows[0]["formId"].ToString().TrimEnd()));
                                    dbparamlist.Add(new dbparam("@id", answeritem["id"].ToString().TrimEnd()));
                                    dbparamlist.Add(new dbparam("@inoper", iItemsData.newid.TrimEnd()));
                                    dbparamlist.Add(new dbparam("@value", answeritem["value"].ToString().TrimEnd()));
                                    if (database.checkActiveSql("mssql", "flybookstring", "exec web.insertsubform @formId,@id,@inoper,@value;", dbparamlist) != "istrue")
                                    {
                                        return new statusModels() { status = "error" };
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            return new statusModels() { status = "istrue" };
        }

        public string checkFormItem(List<Dictionary<string, object>> items)
        {
            foreach (var item in items)
            {
                if (bool.Parse(item["showCheck"].ToString().TrimEnd()) && item["values"].ToString().TrimEnd() == "")
                {
                    switch (item["outValue"].ToString().TrimEnd())
                    {
                        case "radio":
                        case "checkbox":
                        case "droplist":
                            return $"{item["title"].ToString().TrimEnd()}尚未選擇項目";
                        case "image":
                        case "collections":
                            return $"{item["title"].ToString().TrimEnd()}尚未上傳圖檔";
                        default:
                            return $"{item["title"].ToString().TrimEnd()}尚未填寫資訊";
                    }
                }
            }
            return "";
        }
    }
}