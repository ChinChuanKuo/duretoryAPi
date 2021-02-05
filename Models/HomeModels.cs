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
            int itemCount = int.Parse(database.checkSelectSql("mssql", "flybookstring", "exec web.countallmainform;", dbparamlist).Rows[0]["itemCount"].ToString().TrimEnd()), index = int.Parse(otherData.values.TrimEnd()) / 10;
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
                dbparamlist.Add(new dbparam("@iid", "6"));
                List<string> collections = new List<string>();
                foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", "exec web.searchallsubform @formId,@iid;", dbparamlist).Rows)
                {
                    collections.Add(drs["value"].ToString().TrimEnd());
                }
                dbparamlist.Clear();
                dbparamlist.Add(new dbparam("@newid", dr["inoper"].ToString().TrimEnd()));
                items.Add(new Dictionary<string, object>() { { "id", dr["formId"].ToString().TrimEnd() }, { "index", 0 }, { "collections", collections.ToArray() }, { "tile", dr["model"].ToString().TrimEnd() }, { "creator", database.checkSelectSql("mssql", "sysstring", "exec web.searchsiteberinfo @newid;", dbparamlist).Rows[0]["username"].ToString().TrimEnd().Substring(0, 1) }, { "datetime", datetime.differentime($"{dr["indate"].ToString().TrimEnd()} {dr["intime"].ToString().TrimEnd()}") } });
            }
            return new sItemsModels() { showItem = itemCount != int.Parse(otherData.values.TrimEnd()) + mainRows.Rows.Count, itemCount = itemCount, items = items, status = "istrue" };
        }

        public sOptonModels GetFilterModels(userData userData, string cuurip)
        {
            database database = new database();
            List<dbparam> dbparamlist = new List<dbparam>();
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in database.checkSelectSql("mssql", "flybookstring", "exec web.filtermoduleform;", dbparamlist).Rows)
            {
                dbparamlist.Clear();
                bool doubleOption = dr["outValue"].ToString().TrimEnd() == "radio" || dr["outValue"].ToString().TrimEnd() == "checkbox";
                dbparamlist.Add(new dbparam("@value", doubleOption ? dr["iid"].ToString().TrimEnd() : dr["folder"].ToString().TrimEnd()));
                List<Dictionary<string, object>> optionitems = new List<Dictionary<string, object>>();
                foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", doubleOption ? "exec web.filtermoduleoption @value;" : "exec web.filtermodulevalue @value;", dbparamlist).Rows)
                {
                    optionitems.Add(new Dictionary<string, object>() { { "optionPadding", false }, { "value", drs["value"].ToString().TrimEnd() } });
                }
                items.Add(new Dictionary<string, object>() { { "filtIndex", dr["iid"].ToString().TrimEnd() }, { "filtTile", dr["title"].ToString().TrimEnd() }, { "filtOutValue", dr["outValue"].ToString().TrimEnd() }, { "filtValue", "" }, { "filtMenu", false }, { "filtOptions", optionitems } });
            }
            return new sOptonModels() { items = items };
        }

        public sItemsModels GetScrollModels(sScollData sScollData, string cuurip)
        {
            string sqlCode = "";
            foreach (var item in sScollData.items)
            {
                sqlCode += checkSqlCode(sqlCode, filterSqlCode(int.Parse(item["filtIndex"].ToString().TrimEnd()), item["filtValue"].ToString().TrimEnd()));
            }
            database database = new database();
            List<dbparam> dbparamlist = new List<dbparam>();
            dbparamlist.Add(new dbparam("@sqlCode", sqlCode));
            int itemCount = int.Parse(database.checkSelectSql("mssql", "flybookstring", sqlCode == "" ? "exec web.countallmainform;" : "exec web.countfilterallmainform @sqlCode;", dbparamlist).Rows[0]["itemCount"].ToString().TrimEnd()), index = int.Parse(sScollData.value.TrimEnd()) / 10;
            DataTable mainRows = new DataTable();
            dbparamlist.Add(new dbparam("@startId", index + 10 * index));
            dbparamlist.Add(new dbparam("@endId", index + 10 * (index + 1)));
            mainRows = database.checkSelectSql("mssql", "flybookstring", sqlCode == "" ? "exec web.searchallmainform @startId,@endId;" : "exec web.searchfilterallmainform @startId,@endId,@sqlCode;", dbparamlist);
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
                dbparamlist.Add(new dbparam("@iid", "6"));
                List<string> collections = new List<string>();
                foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", "exec web.searchallsubform @formId,@iid;", dbparamlist).Rows)
                {
                    collections.Add(drs["value"].ToString().TrimEnd());
                }
                dbparamlist.Clear();
                dbparamlist.Add(new dbparam("@newid", dr["inoper"].ToString().TrimEnd()));
                items.Add(new Dictionary<string, object>() { { "id", dr["formId"].ToString().TrimEnd() }, { "index", 0 }, { "collections", collections.ToArray() }, { "tile", dr["model"].ToString().TrimEnd() }, { "creator", database.checkSelectSql("mssql", "sysstring", "exec web.searchsiteberinfo @newid;", dbparamlist).Rows[0]["username"].ToString().TrimEnd().Substring(0, 1) }, { "datetime", datetime.differentime($"{dr["indate"].ToString().TrimEnd()} {dr["intime"].ToString().TrimEnd()}") } });
            }
            return new sItemsModels() { showItem = itemCount != int.Parse(sScollData.value.TrimEnd()) + mainRows.Rows.Count, itemCount = itemCount, items = items, status = "istrue" };
        }

        public string checkSqlCode(string sqlCode, string filterCode)
        {
            switch (sqlCode)
            {
                case "":
                    return $"where {filterCode}";
            }
            return $" and {filterCode}";
        }

        public string filterSqlCode(int index, string value)
        {
            DataTable mainRows = new DataTable();
            List<dbparam> dbparamlist = new List<dbparam>();
            dbparamlist.Add(new dbparam("@iid", index));
            mainRows = new database().checkSelectSql("mssql", "flybookstring", "exec web.searchfiltermoduleform @iid;", dbparamlist);
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return "";
            }
            return $"{mainRows.Rows[0]["folder"].ToString().TrimEnd()} = '{value}'";
        }

        public string filterSubCode(string value)
        {
            return $"value = '{value}'";
        }

        public sItemsModels GetSFilterModels(sFiltData sFiltData, string cuurip)
        {
            string sqlCode = "", subCode = "";
            foreach (var item in sFiltData.items)
            {
                if (item["filtIndex"].ToString().TrimEnd() != sFiltData.index.ToString().TrimEnd())
                {
                    switch (item["filtOutValue"].ToString().TrimEnd())
                    {
                        case "radio":
                        case "checkbox":
                            subCode += checkSqlCode(subCode, filterSubCode(item["filtValue"].ToString().TrimEnd()));
                            break;
                        default:
                            sqlCode += checkSqlCode(sqlCode, filterSqlCode(int.Parse(item["filtIndex"].ToString().TrimEnd()), item["filtValue"].ToString().TrimEnd()));
                            break;

                    }
                }
            }
            if (sFiltData.value.TrimEnd() != "")
            {
                switch (sFiltData.outValue.TrimEnd())
                {
                    case "radio":
                    case "checkbox":
                        subCode += checkSqlCode(subCode, filterSubCode(sFiltData.value.ToString().TrimEnd()));
                        break;
                    default:
                        sqlCode += checkSqlCode(sqlCode, filterSqlCode(int.Parse(sFiltData.index.TrimEnd()), sFiltData.value.TrimEnd()));
                        break;
                }
            }
            database database = new database();
            List<dbparam> dbparamlist = new List<dbparam>();
            dbparamlist.Add(new dbparam("@sqlCode", sqlCode));
            dbparamlist.Add(new dbparam("@subCode", subCode));
            int itemCount = int.Parse(database.checkSelectSql("mssql", "flybookstring", "exec web.countfilterallmainform @sqlCode,@subCode;", dbparamlist).Rows[0]["itemCount"].ToString().TrimEnd()), index = 0;
            DataTable mainRows = new DataTable();
            dbparamlist.Add(new dbparam("@startId", index + 10 * index));
            dbparamlist.Add(new dbparam("@endId", index + 10 * (index + 1)));
            mainRows = database.checkSelectSql("mssql", "flybookstring", "exec web.searchfilterallmainform @startId,@endId,@sqlCode,@subCode;", dbparamlist);
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
                dbparamlist.Add(new dbparam("@iid", "6"));
                List<string> collections = new List<string>();
                foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", "exec web.searchallsubform @formId,@iid;", dbparamlist).Rows)
                {
                    collections.Add(drs["value"].ToString().TrimEnd());
                }
                dbparamlist.Clear();
                dbparamlist.Add(new dbparam("@newid", dr["inoper"].ToString().TrimEnd()));
                items.Add(new Dictionary<string, object>() { { "id", dr["formId"].ToString().TrimEnd() }, { "index", 0 }, { "collections", collections.ToArray() }, { "tile", dr["model"].ToString().TrimEnd() }, { "creator", database.checkSelectSql("mssql", "sysstring", "exec web.searchsiteberinfo @newid;", dbparamlist).Rows[0]["username"].ToString().TrimEnd().Substring(0, 1) }, { "datetime", datetime.differentime($"{dr["indate"].ToString().TrimEnd()} {dr["intime"].ToString().TrimEnd()}") } });
            }
            return new sItemsModels() { showItem = itemCount != mainRows.Rows.Count, itemCount = itemCount, items = items, status = "istrue" };
        }

        public statusModels GetDeleteModels(dFormData dFormData, string cuurip)
        {
            database database = new database();
            List<dbparam> dpbaramlist = new List<dbparam>();
            dpbaramlist.Add(new dbparam("@formId", dFormData.formId.TrimEnd()));
            switch (database.checkSelectSql("mssql", "flybookstring", "exec web.searchsinglemainform @formId;", dpbaramlist).Rows.Count)
            {
                case 0:
                    return new statusModels() { status = "nodata" };
            }
            if (database.checkActiveSql("mssql", "flybookstring", "exec web.deleteallform @formId;", dpbaramlist) != "istrue")
            {
                return new statusModels() { status = "error" };
            }
            return new statusModels() { status = "istrue" };
        }

        public sRowsModels GetSViewModels(dFormData dFormData, string cuurip)
        {
            database database = new database();
            DataTable mainRows = new DataTable();
            List<dbparam> dbparamlist = new List<dbparam>();
            dbparamlist.Add(new dbparam("@formId", dFormData.formId.TrimEnd()));
            mainRows = database.checkSelectSql("mssql", "flybookstring", "exec web.checkmodulemainform @formId;", dbparamlist);
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new sRowsModels() { status = "nodata" };
            }
            dbparamlist.Add(new dbparam("@iid", "6"));
            List<string> collections = new List<string>();
            foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", "exec web.searchallsubform @formId,@iid;", dbparamlist).Rows)
            {
                collections.Add(drs["value"].ToString().TrimEnd());
            }
            int[] rowInt = new int[] { 4, 0, 1, 2, 3, 6, 7, 8, 9, 10 };
            List<Dictionary<string, object>> dataitems = new List<Dictionary<string, object>>();
            foreach (int row in rowInt)
            {
                List<string> data = new List<string>();
                switch (mainRows.Rows[row]["outValue"].ToString().TrimEnd())
                {
                    case "radio":
                    case "checkbox":
                        dbparamlist.Clear();
                        dbparamlist.Add(new dbparam("@formId", dFormData.formId.TrimEnd()));
                        dbparamlist.Add(new dbparam("@iid", row + 1));
                        foreach (DataRow dr in database.checkSelectSql("mssql", "flybookstring", "exec web.searchallsubform @formId,@iid;", dbparamlist).Rows)
                        {
                            data.Add(dr["value"].ToString().TrimEnd());
                        }
                        break;
                    default:
                        data.Add(mainRows.Rows[row]["value"].ToString().TrimEnd());
                        break;
                }
                dataitems.Add(new Dictionary<string, object>() { { "key", mainRows.Rows[row]["title"].ToString().TrimEnd() }, { "data", data.ToArray() } });
            }
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            items.Add(new Dictionary<string, object>() { { "viewIndex", 0 }, { "viewections", collections.ToArray() }, { "dataitems", dataitems.ToArray() } });
            return new sRowsModels() { formId = dFormData.formId.TrimEnd(), tile = "", items = items, status = "istrue" };
        }

        public sRowsModels GetSItemModels(dFormData dFormData, string cuurip)
        {
            database database = new database();
            DataTable mainRows = new DataTable();
            List<dbparam> dbparamlist = new List<dbparam>();
            dbparamlist.Add(new dbparam("@formId", dFormData.formId.TrimEnd()));
            mainRows = database.checkSelectSql("mssql", "flybookstring", "exec web.checkmodulemainform @formId;", dbparamlist);
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new sRowsModels() { status = "nodata" };
            }
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in mainRows.Rows)
            {
                bool showFile = false, showImage = false;
                List<Dictionary<string, object>> optionitems = new List<Dictionary<string, object>>(), answeritems = new List<Dictionary<string, object>>(), collitems = new List<Dictionary<string, object>>();
                switch (dr["outValue"].ToString().TrimEnd())
                {
                    case "radio":
                    case "checkbox":
                        dbparamlist.Clear();
                        dbparamlist.Add(new dbparam("@formId", dFormData.formId.TrimEnd()));
                        dbparamlist.Add(new dbparam("@iid", dr["iid"].ToString().TrimEnd()));
                        foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", "exec web.searchalloptionform @iid;", dbparamlist).Rows)
                        {
                            answeritems.Add(new Dictionary<string, object>() { { "id", drs["id"].ToString().TrimEnd() }, { "value", drs["value"].ToString().TrimEnd() }, { "showAnswer", drs["showAnswer"].ToString().TrimEnd() == "1" } });
                        }
                        break;
                    case "droplist":
                        dbparamlist.Clear();
                        dbparamlist.Add(new dbparam("@formId", dFormData.formId.TrimEnd()));
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
                            collitems.Add(new Dictionary<string, object>() { { "id", int.Parse(drs["id"].ToString().TrimEnd()) }, { "showImage", true }, { "showVideo", false }, { "showAudio", false }, { "value", drs["value"].ToString().TrimEnd() }, { "collInsert", false }, { "collDelete", drs["inoper"].ToString().TrimEnd() == dFormData.newid.TrimEnd() } });
                        }
                        break;
                }
                items.Add(new Dictionary<string, object>() { { "iid", dr["iid"].ToString().TrimEnd() }, { "title", dr["title"].ToString().TrimEnd() }, { "values", dr["value"].ToString().TrimEnd() }, { "showMenu", false }, { "showDrop", false }, { "showFile", showFile }, { "showImage", showImage }, { "showVideo", false }, { "showAudio", false }, { "outValue", dr["outValue"].ToString().TrimEnd() }, { "showShow", dr["showed"].ToString().TrimEnd() == "1" }, { "showCheck", dr["checked"].ToString().TrimEnd() == "1" }, { "showFilter", dr["filtered"].ToString().TrimEnd() == "1" }, { "collIndex", 0 }, { "collitems", collitems.ToArray() }, { "optionitems", optionitems.ToArray() }, { "answeritems", answeritems.ToArray() }, { "formModify", false } });
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
                sqlCode = sqlCode == "" ? filterSqlCode(int.Parse(item["iid"].ToString().TrimEnd()), item["values"].ToString().TrimEnd()) : $", {filterSqlCode(int.Parse(item["iid"].ToString().TrimEnd()), item["values"].ToString().TrimEnd())}";
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
                        List<dbparam> dbparamlist = new List<dbparam>();
                        dbparamlist.Add(new dbparam("@formId", iFormData.formId.TrimEnd()));
                        dbparamlist.Add(new dbparam("@id", item["iid"].ToString().TrimEnd()));
                        if (database.checkActiveSql("mssql", "flybookstring", "exec web.deletesubform @formId,@id;", dbparamlist) != "istrue")
                        {
                            return new statusModels() { status = "error" };
                        }
                        foreach (var collitem in JsonSerializer.Deserialize<List<Dictionary<string, object>>>(item["collitems"].ToString().TrimEnd()))
                        {
                            switch (bool.Parse(collitem["collInsert"].ToString().TrimEnd()))
                            {
                                case true:
                                    dbparamlist = new List<dbparam>();
                                    dbparamlist.Add(new dbparam("@formId", iFormData.formId.TrimEnd()));
                                    dbparamlist.Add(new dbparam("@id", item["iid"].ToString().TrimEnd()));
                                    dbparamlist.Add(new dbparam("@inoper", iFormData.newid.TrimEnd()));
                                    dbparamlist.Add(new dbparam("@value", collitem["value"].ToString().TrimEnd()));
                                    if (database.checkActiveSql("mssql", "flybookstring", "exec web.insertsubform @formId,@id,@inoper,@value;", dbparamlist) != "istrue")
                                    {
                                        return new statusModels() { status = "error" };
                                    }
                                    break;
                            }
                        }
                        break;
                    case "radio":
                    case "checkbox":
                        dbparamlist = new List<dbparam>();
                        dbparamlist.Add(new dbparam("@formId", iFormData.formId.TrimEnd()));
                        dbparamlist.Add(new dbparam("@id", item["iid"].ToString().TrimEnd()));
                        if (database.checkActiveSql("mssql", "flybookstring", "exec web.deletesubform @formId,@id;", dbparamlist) != "istrue")
                        {
                            return new statusModels() { status = "error" };
                        }
                        foreach (var answeritem in JsonSerializer.Deserialize<List<Dictionary<string, object>>>(item["answeritems"].ToString().TrimEnd()))
                        {
                            switch (bool.Parse(answeritem["showAnswer"].ToString().TrimEnd()))
                            {
                                case true:
                                    dbparamlist.Clear();
                                    dbparamlist.Add(new dbparam("@formId", iFormData.formId.TrimEnd()));
                                    dbparamlist.Add(new dbparam("@id", item["iid"].ToString().TrimEnd()));
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

        public sItemModels GetSRefreshModels(dFormData dFormData, string cuurip)
        {
            database database = new database();
            DataTable mainRows = new DataTable();
            List<dbparam> dbparamlist = new List<dbparam>();
            dbparamlist.Add(new dbparam("@formId", dFormData.formId.TrimEnd()));
            mainRows = database.checkSelectSql("mssql", "flybookstring", "exec web.checksinglemainform @formId;", dbparamlist);
            switch (mainRows.Rows.Count)
            {
                case 0:
                    return new sItemModels() { status = "nodata" };
            }
            dbparamlist.Add(new dbparam("@iid", "6"));
            List<string> collections = new List<string>();
            foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", "exec web.searchallsubform @formId,@iid;", dbparamlist).Rows)
            {
                collections.Add(drs["value"].ToString().TrimEnd());
            }
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            items.Add(new Dictionary<string, object>() { { "collections", collections.ToArray() }, { "tile", mainRows.Rows[0]["model"].ToString().TrimEnd() }, { "datetime", new datetime().differentime($"{mainRows.Rows[0]["indate"].ToString().TrimEnd()} {mainRows.Rows[0]["intime"].ToString().TrimEnd()}") } });
            return new sItemModels() { items = items, status = "istrue" };
        }
    }
}