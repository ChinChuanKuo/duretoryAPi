using System.Collections.Generic;
using duretoryApi.App_Code;

namespace duretoryApi.Models
{
    public class HomeClass
    {
        public sItemsModels GetSearchModels(otherData otherData, string cuurip)
        {
            database database = new database();
            
            return new sItemsModels() { status = "nodata" };
        }
    }
}