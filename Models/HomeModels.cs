using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using duretoryApi.App_Code;

namespace duretoryApi.Models
{
    public class HomeClass
    {
        public sItemsModels GetSearchModels(otherData otherData, string cuurip)
        {
            database database = new database();
            List<dbparam> dbparamlist = new List<dbparam>();
            int itemCount = int.Parse(database.checkSelectSql("mssql", "flybookstring", "exec web.countallmainform;", dbparamlist).Rows[0]["itemcount"].ToString().TrimEnd()), index = int.Parse(otherData.values.TrimEnd()) / 10;
            DataTable mainRows = new DataTable();
            dbparamlist.Add(new dbparam("@startId", index + 10 * index));
            dbparamlist.Add(new dbparam("@endId", index + 10 * (index + 1)));
            mainRows = database.checkSelectSql("mssql", "flybookstring", "exec web.searchallmainform @startId,@endId;", dbparamlist);
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new sItemsModels() { status = "nodata" };
            }
            datetime datetime = new datetime();
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in mainRows.Rows)
            {
                dbparamlist.Clear();
                dbparamlist.Add(new dbparam("@formId", dr["formId"].ToString().TrimEnd()));
                dbparamlist.Add(new dbparam("@id", "6"));
                List<string> collections = new List<string>();
                foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", "exec web.searchallsubform @formId,@id;", dbparamlist).Rows)
                {
                    collections.Add(drs["value"].ToString().TrimEnd());
                }
                dbparamlist.Clear();
                dbparamlist.Add(new dbparam("@newid", dr["inoper"].ToString().TrimEnd()));
                items.Add(new Dictionary<string, object>() { { "id", dr["formId"].ToString().TrimEnd() }, { "index", 0 }, { "collections", collections.ToArray() }, { "attribute", dr["attribute"].ToString().TrimEnd() }, { "creator", database.checkSelectSql("mssql", "sysstring", "exec web.searchsiteberinfo @newid;", dbparamlist).Rows[0]["username"].ToString().TrimEnd().Substring(0, 1) }, { "datetime", datetime.differentime($"{dr["indate"].ToString().TrimEnd()} {dr["intime"].ToString().TrimEnd()}") }, { "itemDelete", dr["inoper"].ToString().TrimEnd() == otherData.userid.TrimEnd() } });
            }
            return new sItemsModels() { showItem = itemCount != int.Parse(otherData.values.TrimEnd()) + mainRows.Rows.Count, itemCount = itemCount, items = items, status = "istrue" };
        }

        public sItemsModels GetScrollModels(otherData otherData, string cuurip)
        {
            database database = new database();
            List<dbparam> dbparamlist = new List<dbparam>();
            int itemCount = int.Parse(database.checkSelectSql("mssql", "flybookstring", "exec web.countallmainform;", dbparamlist).Rows[0]["itemcount"].ToString().TrimEnd()), index = int.Parse(otherData.values.TrimEnd()) / 10;
            DataTable mainRows = new DataTable();
            dbparamlist.Add(new dbparam("@startId", index + 10 * index));
            dbparamlist.Add(new dbparam("@endId", index + 10 * (index + 1)));
            mainRows = database.checkSelectSql("mssql", "flybookstring", "exec web.searchallmainform @startId,@endId;", dbparamlist);
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new sItemsModels() { status = "nodata" };
            }
            datetime datetime = new datetime();
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in mainRows.Rows)
            {
                dbparamlist.Clear();
                dbparamlist.Add(new dbparam("@formId", dr["formId"].ToString().TrimEnd()));
                dbparamlist.Add(new dbparam("@id", "6"));
                List<string> collections = new List<string>();
                foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", "exec web.searchallsubform @formId;", dbparamlist).Rows)
                {
                    collections.Add(drs["value"].ToString().TrimEnd());
                }
                dbparamlist.Clear();
                dbparamlist.Add(new dbparam("@newid", dr["inoper"].ToString().TrimEnd()));
                items.Add(new Dictionary<string, object>() { { "id", dr["formId"].ToString().TrimEnd() }, { "index", 0 }, { "collections", collections.ToArray() }, { "attribute", dr["attribute"].ToString().TrimEnd() }, { "creator", database.checkSelectSql("mssql", "sysstring", "exec web.searchsiteberinfo @newid;", dbparamlist).Rows[0]["username"].ToString().TrimEnd().Substring(0, 1) }, { "datetime", datetime.differentime($"{dr["indate"].ToString().TrimEnd()} {dr["intime"].ToString().TrimEnd()}") }, { "itemDelete", dr["inoper"].ToString().TrimEnd() == otherData.userid.TrimEnd() } });
            }
            return new sItemsModels() { showItem = itemCount != int.Parse(otherData.values.TrimEnd()) + mainRows.Rows.Count, itemCount = itemCount, items = items, status = "istrue" };
        }

        public statusModels GetDeleteModels(dFormData dFormData, string cuurip)
        {
            database database = new database();
            List<dbparam> dpbaramlist = new List<dbparam>();
            dpbaramlist.Add(new dbparam("@formId", dFormData.formId.TrimEnd()));
            dpbaramlist.Add(new dbparam("@inoper", dFormData.newid.TrimEnd()));
            switch (database.checkSelectSql("mssql", "flybookstring", "exec web.searchsinglemainform @formId,@inoper;", dpbaramlist).Rows.Count)
            {
                case 0:
                    return new statusModels() { status = "nodata" };
            }
            if (database.checkActiveSql("mssql", "flybookstring", "exec web.deleteallform @formId,@inoper;", dpbaramlist) != "istrue")
            {
                return new statusModels() { status = "error" };
            }
            return new statusModels() { status = "istrue" };
        }

        public sRowsModels GetSItemModels(dFormData dFormData, string cuurip)
        {
            database database = new database();
            DataTable mainRows = new DataTable();
            List<dbparam> dbparamlist = new List<dbparam>();
            dbparamlist.Add(new dbparam("@formId", dFormData.formId.TrimEnd()));
            dbparamlist.Add(new dbparam("@inoper", dFormData.newid.TrimEnd()));
            mainRows = database.checkSelectSql("mssql", "flybookstring", "exec web.checkmodulemainform @formId,@inoper", dbparamlist);
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new sRowsModels() { status = "nodata" };
            }
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in mainRows.Rows)
            {
                bool showFile = false, showImage = false;
                List<Dictionary<string, object>> optionitems = new List<Dictionary<string, object>>(), answeritems = new List<Dictionary<string, object>>(), collectionitems = new List<Dictionary<string, object>>();
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
                    case "image":
                        showFile = dr["value"].ToString().TrimEnd() != ""; showImage = dr["value"].ToString().TrimEnd() != "";
                        break;
                    case "collections":
                        dbparamlist.Clear();
                        dbparamlist.Add(new dbparam("@formId", dFormData.formId.TrimEnd()));
                        dbparamlist.Add(new dbparam("@iid", dr["iid"].ToString().TrimEnd()));
                        foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", "exec web.searchallsubform @formId,@iid;", dbparamlist).Rows)
                        {
                            showFile = true;
                            showImage = true;
                            collectionitems.Add(new Dictionary<string, object>() { { "id", int.Parse(drs["id"].ToString().TrimEnd()) }, { "collectionImage", true }, { "collectionVideo", false }, { "collectionAudio", false }, { "value", drs["value"].ToString().TrimEnd() }, { "collectionInsert", false }, { "collectionDelete", drs["inoper"].ToString().TrimEnd() == dFormData.newid.TrimEnd() } });
                        }
                        break;
                }
                items.Add(new Dictionary<string, object>() { { "iid", dr["iid"].ToString().TrimEnd() }, { "title", dr["title"].ToString().TrimEnd() }, { "values", dr["value"].ToString().TrimEnd() }, { "showMenu", false }, { "showDrop", false }, { "showFile", showFile }, { "showImage", showImage }, { "showVideo", false }, { "showAudio", false }, { "outValue", dr["outValue"].ToString().TrimEnd() }, { "showShow", dr["showed"].ToString().TrimEnd() == "1" }, { "showCheck", dr["checked"].ToString().TrimEnd() == "1" }, { "showFilter", dr["filtered"].ToString().TrimEnd() == "1" }, { "collectionIndex", 0 }, { "collectionitems", collectionitems.ToArray() }, { "optionitems", optionitems.ToArray() }, { "answeritems", answeritems.ToArray() }, { "formModify", false } });
            }
            return new sRowsModels() { formId = dFormData.formId.TrimEnd(), tile = "", items = items, status = "istrue" };
        }

        public statusModels GetInsertModels(iFormData iFormData, string cuurip)
        {
            string checkValue = new CreateClass().checkFormItem(iFormData.items);
            if (checkValue != "")
            {
                return new statusModels() { status = checkValue };
            }
            string sqlCode = "";
            foreach (var item in iFormData.items)
            {
                sqlCode = sqlCode == "" ? checkFolderValue(item["iid"].ToString().TrimEnd(), item["values"].ToString().TrimEnd()) : $", {checkFolderValue(item["iid"].ToString().TrimEnd(), item["values"].ToString().TrimEnd())}";
            }
            database database = new database();
            if (sqlCode != "")
            {
                List<dbparam> dbparamlist = new List<dbparam>();
                dbparamlist.Add(new dbparam("@formId", iFormData.formId.TrimEnd()));
                dbparamlist.Add(new dbparam("@inoper", iFormData.newid.TrimEnd()));
                dbparamlist.Add(new dbparam("@sqlCode", sqlCode));
                if (database.checkSelectSql("mssql", "flybookstring", "exec web.modifymainform @formId,@inoper,@sqlCode;", dbparamlist).Rows.Count > 0)
                {
                    return new statusModels() { status = "error" };
                }
            }
            foreach (var item in iFormData.items)
            {
                switch (item["outValue"].ToString().TrimEnd())
                {
                    case "collections":
                        foreach (var collectitem in JsonSerializer.Deserialize<List<Dictionary<string, object>>>(item["collectionitems"].ToString().TrimEnd()))
                        {
                            switch (bool.Parse(collectitem["collectionDelete"].ToString().TrimEnd()))
                            {
                                case true:
                                    List<dbparam> dbparamlist = new List<dbparam>();
                                    dbparamlist.Add(new dbparam("@formId", iFormData.formId.TrimEnd()));
                                    dbparamlist.Add(new dbparam("@id", item["iid"].ToString().TrimEnd()));
                                    if (database.checkActiveSql("mssql", "flybookstring", "exec web.deletesubform @formId,@id;", dbparamlist) != "istrue")
                                    {
                                        return new statusModels() { status = "error" };
                                    }
                                    break;
                                default:
                                    switch (bool.Parse(collectitem["collectionInsert"].ToString().TrimEnd()))
                                    {
                                        case true:
                                            dbparamlist = new List<dbparam>();
                                            dbparamlist.Add(new dbparam("@formId", iFormData.formId.TrimEnd()));
                                            dbparamlist.Add(new dbparam("@id", item["iid"].ToString().TrimEnd()));
                                            dbparamlist.Add(new dbparam("@inoper", iFormData.newid.TrimEnd()));
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
                                    List<dbparam> dbparamlist = new List<dbparam>();
                                    dbparamlist.Add(new dbparam("@formId", iFormData.formId.TrimEnd()));
                                    dbparamlist.Add(new dbparam("@id", answeritem["id"].ToString().TrimEnd()));
                                    dbparamlist.Add(new dbparam("@inoper", iFormData.newid.TrimEnd()));
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

        public string checkFolderValue(string iid, string value)
        {
            switch (iid)
            {
                case "1":
                    return $"attribute = '{value}'";
                case "2":
                    return $"category = '{value}'";
                case "3":
                    return $"customer = '{value}'";
                case "4":
                    return $"sotime = '{value}'";
                case "5":
                    return $"model = '{value}'";
                case "6":
                    return $"collection = '{value}'";
                case "7":
                    return $"mb = '{value}'";
                case "8":
                    return $"sample = '{value}'";
                case "9":
                    return $"species = '{value}'";
                case "10":
                    return $"count = '{value}'";
            }
            return $"designer = '{value}'";
        }
    }
}