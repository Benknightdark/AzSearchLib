using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzSearchLib.Models {
    public class SynonymMapsRequestModel {
        public string format { get; set; }
        public string synonyms { get; set; }
    }
    public class SynonymMapsResModel {
        public string name { get; set; }

        [JsonProperty ("@odata.etag")]
        public dynamic etag { get; set; }
        public string format { get; set; }
        public string synonyms { get; set; }
        public dynamic encryptionKey { get; set; }
    }
    public class SynonymMapsResListModel {
        public List<SynonymMapsResModel> value { get; set; }
    }
}