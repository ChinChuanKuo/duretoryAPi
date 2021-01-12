using System.Collections.Generic;
using System.Data;
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
                dbparamlist.Add(new dbparam("@newid", dr["inoper"].ToString().TrimEnd()));
                items.Add(new Dictionary<string, object>() { { "id", dr["formId"].ToString().TrimEnd() }, { "collitems", new List<Dictionary<string, object>>().ToArray() }, { "attribute", dr["attribute"].ToString().TrimEnd() }, { "creator", database.checkSelectSql("mssql", "sysstring", "exec web.searchsiteberinfo @newid;", dbparamlist).Rows[0]["username"].ToString().TrimEnd().Substring(0, 1) }, { "datetime", datetime.differentime($"{dr["indate"].ToString().TrimEnd()} {dr["intime"].ToString().TrimEnd()}") } });
            }
            return new sItemsModels() { showItem = itemCount != int.Parse(otherData.values.TrimEnd()) + mainRows.Rows.Count, itemCount = itemCount, items = items, status = "istrue" };
        }
    }
}