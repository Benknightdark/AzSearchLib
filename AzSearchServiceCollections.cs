using System;
using System.Net.Http;
using AzSearchLib.services;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace AzSearchLib {
    public static class AzSearchServiceCollections {
        #region IAsyncPolicy function
        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy () {
            return HttpPolicyExtensions
                .HandleTransientHttpError ()
                .CircuitBreakerAsync (5, TimeSpan.FromSeconds (30));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy () {
            return HttpPolicyExtensions
                .HandleTransientHttpError ()
                .OrResult (msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync (2, retryAttempt => TimeSpan.FromSeconds (Math.Pow (2, retryAttempt)));
        }

        #endregion IAsyncPolicy function
        /// <summary>
        /// 加入Azure Cognitive Search服務
        /// </summary>
        /// <param name="services"></param>
        public static void AddAzSearchService (this IServiceCollection services) {
            // ConfigService
            services.AddScoped<AzSearchConfigService> ();
            // 同義字字典管理服務
            services.AddHttpClient<SynonymMapsService> ()
                .AddPolicyHandler (Policy.TimeoutAsync<HttpResponseMessage> (10))
                .AddPolicyHandler (GetRetryPolicy ())
                .AddPolicyHandler (GetCircuitBreakerPolicy ());
            // 全文檢索Index管理服務
            services.AddHttpClient<FTManageService> ()
                .AddPolicyHandler (Policy.TimeoutAsync<HttpResponseMessage> (10))
                .AddPolicyHandler (GetRetryPolicy ())
                .AddPolicyHandler (GetCircuitBreakerPolicy ());
            // 全文檢索搜尋服務
            services.AddHttpClient<FTSearchService> ()
                .AddPolicyHandler (Policy.TimeoutAsync<HttpResponseMessage> (10))
                .AddPolicyHandler (GetRetryPolicy ())
                .AddPolicyHandler (GetCircuitBreakerPolicy ());
        }

    }
}