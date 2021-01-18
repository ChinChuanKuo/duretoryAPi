using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace duretoryApi.Models
{
    public class userData
    {
        public string userid { get; set; }
    }

    public class otherData
    {
        public string userid { get; set; }
        public string values { get; set; }
    }

    public class sItemModels
    {
        [Required]
        public List<Dictionary<string, object>> items { get; set; }
        [Required]
        public string status { get; set; }
    }

    public class sItemsModels
    {
        [Required]
        public bool showItem { get; set; }
        [Required]
        public int itemCount { get; set; }
        [Required]
        public List<Dictionary<string, object>> items { get; set; }
        [Required]
        public string status { get; set; }
    }

    public class iFormData
    {
        public string formId { get; set; }
        public string tile { get; set; }
        public string desc { get; set; }
        public List<Dictionary<string, object>> items { get; set; }
        public string newid { get; set; }
    }

    public class iItemsData
    {
        public List<Dictionary<string, object>> items { get; set; }
        public string newid { get; set; }
    }

    public class sRowsModels
    {
        [Required]
        public string formId { get; set; }
        [Required]
        public string tile { get; set; }
        [Required]
        public List<Dictionary<string, object>> items { get; set; }
        [Required]
        public string status { get; set; }
    }

    public class dFormData
    {
        public string formId { get; set; }
        public string newid { get; set; }
    }

    public class statusModels
    {
        [Required]
        public string status { get; set; }
    }

    public class sSiteModels
    {
        [Required]
        public bool images { get; set; }
        [Required]
        public bool videos { get; set; }
        [Required]
        public bool audios { get; set; }
        [Required]
        public string src { get; set; }
        [Required]
        public byte[] files { get; set; }
        [Required]
        public string status { get; set; }
    }

    public class sOptonModels
    {
        [Required]
        public List<Dictionary<string, object>> items { get; set; }
    }

    public class sScollData
    {
        public List<Dictionary<string, object>> items { get; set; }
        public string value { get; set; }
        public string newid { get; set; }
    }

    public class sFiltData
    {
        public List<Dictionary<string, object>> items { get; set; }
        public string index { get; set; }
        public string value { get; set; }
        public string newid { get; set; }
    }
}