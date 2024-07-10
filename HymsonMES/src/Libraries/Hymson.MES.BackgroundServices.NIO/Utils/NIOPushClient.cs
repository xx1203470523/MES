using Hymson.MES.BackgroundServices.NIO.Utils;
using Hymson.Utils;
using RestSharp;
using System.IO;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务基类
    /// </summary>
    public static class NIOPushClient
    {
        /// <summary>
        /// 
        /// </summary>
        public const string HOST = "https://openapi-nexus-stg.nio.com";
        public const string SUFFIX = "/qm/ppqm-trans-api";

        //public const string HOST = "openapi-nexus.nio.com/qm/ppqm-trans-api";
        //public const string HOST = "https://openapi-nexus-stg.nio.com";

        /// <summary>
        /// 
        /// </summary>
        public const string APP_KEY = "APP17459815";
        /// <summary>
        /// 
        /// </summary>
        public const string APP_SECRET = "mcqh6XEHPoyvDAbSlXKYR9CCfQfLC4Hj6GAWafgbOmh6ONCkSSDBquhEGXtHKaFS7dOhvdKVPiDTU2zedifWQZ4j5Tuk0d5z4voKsYoUucvOehPC6wHUGWUNP2RvYJPh";

        /// <summary>
        /// 公用方法
        /// </summary>
        /// <param name="path"></param>
        /// <param name="jsonBody"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static async Task<RestResponse> ExecuteAsync(string path, object jsonBody, Method method = Method.Post)
        {
            path = $"{SUFFIX}{path}";

            var TIMESTAMP = $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}";
            var NONCE = $"{Guid.NewGuid()}".Replace("-", "");
            var METHOD = $"{method}".ToUpper();

            var BODY = jsonBody.ToSerialize();
            var SIGN = NIOOpenApiSignUtil.Sign(APP_KEY, APP_SECRET, TIMESTAMP, NONCE, METHOD, path, null, null, BODY);

            var client = new RestClient(HOST);
            var request = new RestRequest(path, method);
            request.AddHeader("appKey", APP_KEY);
            request.AddHeader("timestamp", TIMESTAMP);
            request.AddHeader("nonce", NONCE);
            request.AddHeader("sign", SIGN);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("User-Agent", "Pob_chen/1.1.0");
            request.AddParameter("application/json", BODY, ParameterType.RequestBody);
            var response = await client.ExecuteAsync(request);
            return response;
        }

        /// <summary>
        /// 公用方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<RestResponse> ExecuteAsync<T>(this NioPushSwitchEntity config, IEnumerable<T> data)
        {
            if (config == null || data == null || !data.Any()) return new RestResponse { IsSuccessStatusCode = false };
            var path = $"{SUFFIX}{config.Path}";

            var TIMESTAMP = $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}";
            var NONCE = $"{Guid.NewGuid()}".Replace("-", "");
            var METHOD = $"{config.Method}".ToUpper();

            // 组装数据
            var dataObj = new
            {
                schemaCode = config.SchemaCode,
                list = data
            };
            var BODY = dataObj.ToSerialize() ?? "";
            var SIGN = NIOOpenApiSignUtil.Sign(APP_KEY, APP_SECRET, TIMESTAMP, NONCE, METHOD, path, null, null, BODY);

            var client = new RestClient(HOST);
            var request = new RestRequest(path, config.Method);
            request.AddHeader("appKey", APP_KEY);
            request.AddHeader("timestamp", TIMESTAMP);
            request.AddHeader("nonce", NONCE);
            request.AddHeader("sign", SIGN);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("User-Agent", "Pob_chen/1.1.0");
            request.AddParameter("application/json", BODY, ParameterType.RequestBody);
            var response = await client.ExecuteAsync(request);
            return response;
        }

    }
}
