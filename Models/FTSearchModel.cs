using System;
using System.Collections.Generic;
using AzSearchLib.Models;
using Newtonsoft.Json;

namespace AzSearchLib.Models {
    public class FTSearchReqModel {
        public string IndexName { get; set; }
        public string search { get; set; } = "*";
        public string count { get; set; } = "true"; //$
        public string searchFields { get; set; }
        public string select { get; set; } //$
        public string filter { get; set; }
        public string orderby { get; set; } // keyword asc|desc
        public string top { get; set; }
        public string skip { get; set; }
        public string nextLink { get; set; }

    }

    public class FTSearchResExtendsModel {
        [JsonProperty ("@search.score")]
        public double score { get; set; }
        public string AzureSearch_DocumentKey { get; set; }
    }
    public class FTSearchResModel<T> {

        [JsonProperty ("@odata.count")]
        public int count { get; set; }
        public List<T> value { get; set; }

        [JsonProperty ("@odata.nextLink")]

        public string nextLink { get; set; }

    }

}