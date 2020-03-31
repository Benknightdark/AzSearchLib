using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AzSearchLib.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
namespace AzSearchLib.services {
    /// <summary>
    /// 全文檢索Index管理服務
    /// </summary>
    public class FTManageService {
        private HttpClient _client { get; set; }
        private AzSearchConfigService _configService { get; set; }
        private ILogger<FTManageService> _logger;


        public FTManageService (HttpClient client, AzSearchConfigService configService, ILogger<FTManageService> logger) {
            _configService = configService;
            client.BaseAddress = new Uri ($"https://{_configService.GetAzSearchConfig().ServiceName}.search.windows.net");
            client.DefaultRequestHeaders.Add ("api-key", _configService.GetAzSearchConfig ().ApiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue ("application/json"));
            _client = client;
            _logger = logger;
        }
        /// <summary>
        /// 取出所有Index
        /// </summary>
        /// <returns></returns>
        public async Task<FTManageResListModel> GetAll () {
            var ResData = await _client.GetAsync ($"/indexes?api-version={_configService.GetAzSearchConfig().ApiVersion}");
            FTManageResListModel ResFormatData = new FTManageResListModel ();
            try {
                if (ResData.IsSuccessStatusCode) {
                    var ResDataString = await ResData.Content.ReadAsStringAsync ();
                    ResFormatData = JsonConvert.DeserializeObject<FTManageResListModel> (ResDataString);
                } else {
                    _logger.LogError (ResData.StatusCode.ToString ());
                    _logger.LogTrace (await ResData.Content.ReadAsStringAsync ());
                }
            } catch (System.Exception e) {
                _logger.LogError (e.Message);
                _logger.LogTrace (e.StackTrace);
            }
            return ResFormatData;
        }
        /// <summary>
        /// 取出特定Index
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public async Task<FTManageResModel> Get (string Name) {
            var ResData = await _client.GetAsync ($"/indexes/{Name}?api-version={_configService.GetAzSearchConfig().ApiVersion}");
            FTManageResModel ResFormatData = new FTManageResModel ();
            try {
                if (ResData.IsSuccessStatusCode) {
                    var ResDataString = await ResData.Content.ReadAsStringAsync ();
                    ResFormatData = JsonConvert.DeserializeObject<FTManageResModel> (ResDataString);
                } else {
                    _logger.LogError (ResData.StatusCode.ToString ());
                    _logger.LogTrace (await ResData.Content.ReadAsStringAsync ());
                }
            } catch (System.Exception e) {
                _logger.LogError (e.Message);
                _logger.LogTrace (e.StackTrace);
            }
            return ResFormatData;

        }
        /// <summary>
        /// 更新特定Index
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public async Task<HttpStatusCode> Put (string Name, FTManageReqModel Data) {
            try {
                var JsonContent = JsonConvert.SerializeObject (Data);
                StringContent content = new StringContent (JsonContent, Encoding.UTF8, "application/json");
                var ResData = await _client.PutAsync ($"/indexes/{Name}?api-version={_configService.GetAzSearchConfig().ApiVersion}", content);
                if (!ResData.IsSuccessStatusCode) {
                    _logger.LogError (ResData.StatusCode.ToString ());
                    _logger.LogTrace (await ResData.Content.ReadAsStringAsync());
            
                }
                return ResData.StatusCode;
            } catch (System.Exception e) {
                _logger.LogError (e.Message);
                _logger.LogTrace (e.StackTrace);
                return HttpStatusCode.InternalServerError;
            }
        }

    }
}