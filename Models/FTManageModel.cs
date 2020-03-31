using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzSearchLib.Models {
    public class Field {
        public string name { get; set; }
        public string type { get; set; }
        public bool searchable { get; set; }
        public bool filterable { get; set; }
        public bool retrievable { get; set; }
        public bool sortable { get; set; }
        public bool facetable { get; set; }
        public bool key { get; set; }
        public object indexAnalyzer { get; set; }
        public object searchAnalyzer { get; set; }
        public string analyzer { get; set; }
        public List<object> synonymMaps { get; set; }
    }
    public class FTManageResModel {
        // [JsonProperty ("@odata.context")]

        // public string context { get; set; }

        [JsonProperty ("@odata.etag")]
        public string etag { get; set; }
        public string name { get; set; }
        public string defaultScoringProfile { get; set; }
        public List<Field> fields { get; set; }
        public List<object> scoringProfiles { get; set; }
        public object corsOptions { get; set; }
        public List<object> suggesters { get; set; }
        public List<object> analyzers { get; set; }
        public List<object> tokenizers { get; set; }
        public List<object> tokenFilters { get; set; }
        public List<object> charFilters { get; set; }
        public object encryptionKey { get; set; }
    }

    public class FTManageResListModel {
        public List<FTManageResModel> value { get; set; }
    }

    public class FTManageReqModel {
        public string name { get; set; }
        public List<Field> fields { get; set; }
        

    }
}