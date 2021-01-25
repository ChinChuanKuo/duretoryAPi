using System.Collections.Generic;
using System.Data;
using duretoryApi.App_Code;

namespace duretoryApi.Models
{
    public class SearchClass
    {
        public sItemsModels GetSearchModels(sRowsData sRowsData, string cuurip)
        {
            database database = new database();
            List<dbparam> dbparamlist = new List<dbparam>();
            dbparamlist.Add(new dbparam("@sqlCode", RecordSqlCode(sRowsData.formId.TrimEnd())));
            int itemCount = int.Parse(database.checkSelectSql("mssql", "flybookstring", "exec web.countfilterallmainform @sqlCode;", dbparamlist).Rows[0]["itemCount"].ToString().TrimEnd()), index = int.Parse(sRowsData.value.TrimEnd()) / 10;
            DataTable mainRows = new DataTable();
            dbparamlist.Add(new dbparam("@startId", index + 10 * index));
            dbparamlist.Add(new dbparam("@endId", index + 10 * (index + 1)));
            mainRows = database.checkSelectSql("mssql", "flybookstring", "exec web.searchfilterallmainform @startId,@endId,@sqlCode;", dbparamlist);
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
            return new sItemsModels() { showItem = itemCount != int.Parse(sRowsData.value.TrimEnd()) + mainRows.Rows.Count, itemCount = itemCount, items = items, status = "istrue" };
        }

        public string RecordSqlCode(string value)
        {
            return $"where attribute like '%{value}%' or category like '%{value}%' or customer like '%{value}%' or sotime like '%{value}%' or model like '%{value}%' or model like '%{value}%' or mb like '%{value}%' or sample like '%{value}%' or species like '%{value}%' or count like '%{value}%' or designer like '%{value}%'";
        }

        public sOptonModels GetFilterModels(dFormData dFormData, string cuurip)
        {
            int i = 0;
            database database = new database();
            List<dbparam> dbparamlist = new List<dbparam>();
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
            foreach (DataRow dr in database.checkSelectSql("mssql", "flybookstring", "exec web.filtermoduleform;", dbparamlist).Rows)
            {
                dbparamlist.Clear();
                dbparamlist.Add(new dbparam("@value", dr["folder"].ToString().TrimEnd()));
                dbparamlist.Add(new dbparam("@sqlCode", RecordSqlCode(dFormData.formId.TrimEnd())));
                List<Dictionary<string, object>> optionitems = new List<Dictionary<string, object>>();
                foreach (DataRow drs in database.checkSelectSql("mssql", "flybookstring", "exec web.searchfiltermodulevalue @value,@sqlCode;", dbparamlist).Rows)
                {
                    optionitems.Add(new Dictionary<string, object>() { { "optionPadding", false }, { "value", drs["value"].ToString().TrimEnd() } });
                }
                items.Add(new Dictionary<string, object>() { { "filtIndex", dr["iid"].ToString().TrimEnd() }, { "filtTile", dr["title"].ToString().TrimEnd() }, { "filtValue", "" }, { "filtMenu", false }, { "filtOptions", optionitems } });
                i++;
            }
            return new sOptonModels() { items = items };
        }
    }
}