using System;
using System.Net.Http;
using System.Threading.Tasks;
using AzSearchLib.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzSearchLib.services {
    /// <summary>
    /// 全文檢索Index管理服務
    /// </summary>
    public class FTSearchService {
        private HttpClient _client { get; set; }
        private AzSearchConfigService _configService { get; set; }
        private ILogger<FTSearchService> _logger;

        public FTSearchService (HttpClient client, AzSearchConfigService configService, ILogger<FTSearchService> logger) {
            _configService = configService;
            // client.BaseAddress = new Uri ($"https://{_configService.GetAzSearchConfig().ServiceName}.search.windows.net");
            client.DefaultRequestHeaders.Add ("api-key", _configService.GetAzSearchConfig ().ApiKey);
            _client = client;
            _logger = logger;
        }
        public async Task<FTSearchResModel<TModel>> SearchByLink<TModel> (string NextLink) where TModel : class {
            var dd = await _client.GetAsync (NextLink);
            var cd = await dd.Content.ReadAsStringAsync ();
            var bb = JsonConvert.DeserializeObject<FTSearchResModel<TModel>> (cd);
            return bb;
        }
        public async Task<FTSearchResModel<TModel>> Search<TModel> (FTSearchReqModel Data) where TModel : class {
            string url = $"https://{_configService.GetAzSearchConfig().ServiceName}.search.windows.net/indexes/{Data.IndexName}/docs?api-version={_configService.GetAzSearchConfig().ApiVersion}&queryType=full&search={Data.search}&$count={Data.count}";
            // url+="&$count="+Data.count;
            if (!string.IsNullOrEmpty (Data.searchFields)) {
                url += $"&searchFields={Data.searchFields}";
            }
            if (!string.IsNullOrEmpty (Data.select)) {
                url += $"&$select={Data.select}";
            }
            if (!string.IsNullOrEmpty (Data.filter)) {
                url += $"&$filter={Data.filter}";
            }
            if (!string.IsNullOrEmpty (Data.orderby)) {
                url += $"&$orderby={Data.orderby}";
            }
            if (!string.IsNullOrEmpty (Data.top)) {
                url += $"&$top={Data.top}";
            }
            if (!string.IsNullOrEmpty (Data.skip)) {
                url += $"&$skip={Data.skip}";
            }
            var dd = await _client.GetAsync (url);
            var cd = await dd.Content.ReadAsStringAsync ();
            var bb = JsonConvert.DeserializeObject<FTSearchResModel<TModel>> (cd);
            return bb;
        }
    }
}

/*
{

    "IndexName":"azureblob-index",
    
"search":"name:(台北 NOT 診所)",
    "count":"true",
    "select":"name,code,address",
    "searchFields":"name,address"
}


// Fielded search (回復查詢)
"search":"name:(台北 NOT 診所)"


// fuzzy search (模糊搜尋)
"search":"address:(公園路~2 AND 台南~2) AND name:(藥局) ",


//Proximity search (鄰近搜尋)
"search":"address:(公園路~6)  ",
"search":"台南",

// 字詞提升查詢
"search":"address:(公園路~2 AND 台南^2)",



https://docs.microsoft.com/en-us/azure/search/search-query-lucene-examples

https://docs.microsoft.com/zh-tw/azure/search/search-query-simple-examples

*/