using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AzSearchLib.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzSearchLib.services {
   /// <summary>
   /// 同義字字典服務
   /// </summary>
   public class SynonymMapsService {
      private HttpClient _client { get; set; }
      private AzSearchConfigService _configService { get; set; }
      private ILogger<SynonymMapsService> _logger;

      public SynonymMapsService (HttpClient client, AzSearchConfigService configService, ILogger<SynonymMapsService> logger) {
         _configService = configService;
         client.BaseAddress = new Uri ($"https://{_configService.GetAzSearchConfig().ServiceName}.search.windows.net/");
         client.DefaultRequestHeaders.Add ("api-key", _configService.GetAzSearchConfig ().ApiKey);
         _client = client;
         _logger = logger;
      }
      /// <summary>
      /// 修改或新增同義字詞庫
      /// </summary>
      /// <param name="MapName"></param>
      /// <param name="Data"></param>
      /// <returns></returns>
      public async Task<HttpStatusCode> Put (string MapName, SynonymMapsRequestModel Data) {
         try {
            var JsonContent = JsonConvert.SerializeObject (Data);
            StringContent content = new StringContent (JsonContent, Encoding.UTF8, "application/json");
            var ResData = await _client.PutAsync ($"/synonymmaps/{MapName}?api-version={_configService.GetAzSearchConfig().ApiVersion}", content);
            if (!ResData.IsSuccessStatusCode) {
                _logger.LogError (ResData.StatusCode.ToString ());
               _logger.LogTrace (await ResData.Content.ReadAsStringAsync ());
            } 
            return ResData.StatusCode;
         } catch (System.Exception e) {

            _logger.LogError (e.Message);
            _logger.LogTrace (e.StackTrace);
            return HttpStatusCode.InternalServerError;
         }
      }

      /// <summary>
      /// 取得特定同義字詞庫
      /// </summary>
      /// <param name="MapName"></param>
      /// <returns></returns>
      public async Task<SynonymMapsResModel> Get (string MapName) {
         var ResData = await _client.GetAsync ($"/synonymmaps/{MapName}?api-version={_configService.GetAzSearchConfig().ApiVersion}");
         SynonymMapsResModel ResFormatData = new SynonymMapsResModel ();
         try {
            if (ResData.IsSuccessStatusCode) {
               var ResDataString = await ResData.Content.ReadAsStringAsync ();
               ResFormatData = JsonConvert.DeserializeObject<SynonymMapsResModel> (ResDataString);
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
      /// 取得所有同義字詞庫
      /// </summary>
      /// <returns></returns>
      public async Task<SynonymMapsResListModel> GetAll () {
         var ResData = await _client.GetAsync ($"/synonymmaps?api-version={_configService.GetAzSearchConfig().ApiVersion}");
         SynonymMapsResListModel ResFormatData = new SynonymMapsResListModel ();
         try {
            if (ResData.IsSuccessStatusCode) {
               var ResDataString = await ResData.Content.ReadAsStringAsync ();
               ResFormatData = JsonConvert.DeserializeObject<SynonymMapsResListModel> (ResDataString);
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
      /// 刪除特定同義字詞庫
      /// </summary>
      /// <param name="MapName"></param>
      /// <returns></returns>
      public async Task<HttpStatusCode> Delete (string MapName) {
         try {
           var ResData= await _client.DeleteAsync ($"/synonymmaps/{MapName}?api-version={_configService.GetAzSearchConfig().ApiVersion}");
            return  ResData.StatusCode;
         } catch (System.Exception e) {

            _logger.LogError (e.Message);
            _logger.LogTrace (e.StackTrace);
            return HttpStatusCode.InternalServerError;
         }
      }

   }
}