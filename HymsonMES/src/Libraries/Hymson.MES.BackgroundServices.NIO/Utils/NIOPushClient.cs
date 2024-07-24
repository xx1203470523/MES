using Hymson.MES.BackgroundServices.NIO.Utils;
using Hymson.MES.Core.NIO;
using Hymson.MES.CoreServices.Bos.NIO;
using Hymson.MES.CoreServices.Extension;
using RestSharp;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务基类
    /// </summary>
    public static class NIOPushClient
    {
        /// <summary>
        /// 主机
        /// </summary>
        public const string HOST = "https://openapi-nexus-stg.nio.com";
        public const string HOSTSUFFIX = "/qm/ppqm-trans-api";

        //public const string HOST = "openapi-nexus.nio.com/qm/ppqm-trans-api";
        //public const string HOST = "https://openapi-nexus-stg.nio.com";

        /// <summary>
        /// 固定（勿改）
        /// </summary>
        public const string APP_KEY = "APP17459815";
        /// <summary>
        /// 固定（勿改）
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
            var TIMESTAMP = $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}";
            var NONCE = $"{Guid.NewGuid()}".Replace("-", "");
            var METHOD = $"{method}".ToUpper();

            var BODY = jsonBody.ToSerializeLower();
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
            var path = $"{HOSTSUFFIX}{config.Path}";

            var TIMESTAMP = $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}";
            var NONCE = $"{Guid.NewGuid()}".Replace("-", "");
            var METHOD = $"{config.Method}".ToUpper();

            // 组装数据
            var dataObj = new NIORequestBo<T>
            {
                SchemaCode = config.SchemaCode,
                List = data
            };
            var BODY = dataObj.ToSerializeLower() ?? "";
            var SIGN = NIOOpenApiSignUtil.Sign(APP_KEY, APP_SECRET, TIMESTAMP, NONCE, METHOD, path, null, null, BODY);

            var client = new RestClient(HOST);
            var request = new RestRequest(path, (Method)config.Method);
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
            request.Timeout = TimeSpan.FromSeconds(15);
            var response = await client.ExecuteAsync(request);
            return response;
        }

        /// <summary>
        /// 公用方法
        /// </summary>
        /// <param name="config"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static async Task<RestResponse> ExecuteAsync(this NioPushSwitchEntity config, string body)
        {
            if (config == null || string.IsNullOrWhiteSpace(body)) return new RestResponse { IsSuccessStatusCode = false };
            var path = $"{HOSTSUFFIX}{config.Path}";

            var TIMESTAMP = $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}";
            var NONCE = $"{Guid.NewGuid()}".Replace("-", "");
            var METHOD = $"{config.Method}".ToUpper();

            var SIGN = NIOOpenApiSignUtil.Sign(APP_KEY, APP_SECRET, TIMESTAMP, NONCE, METHOD, path, null, null, body);

            var client = new RestClient(HOST);
            var request = new RestRequest(path, (Method)config.Method);
            request.AddHeader("appKey", APP_KEY);
            request.AddHeader("timestamp", TIMESTAMP);
            request.AddHeader("nonce", NONCE);
            request.AddHeader("sign", SIGN);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("User-Agent", "Pob_chen/1.1.0");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.Timeout = TimeSpan.FromSeconds(15);
            var response = await client.ExecuteAsync(request);
            return response;
        }

    }
}
