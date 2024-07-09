using Hymson.MES.BackgroundServices.NIO.Utils;
using Hymson.Utils;
using RestSharp;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务基类
    /// </summary>
    public class BasePushService
    {
        /// <summary>
        /// 
        /// </summary>
        public const string HOST = "https://openapi-nexus-stg.nio.com";
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
        /// <param name="resource"></param>
        /// <param name="body"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static async Task<RestResponse> ExecuteAsync(string resource, object jsonBody, Method method = Method.Post)
        {
            var TIMESTAMP = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            var NONCE = Guid.NewGuid().ToString().Replace("-", "");
            var METHOD = method.ToString().ToUpper();
            var BODY = jsonBody.ToSerialize();
            var SIGN = NIOOpenApiSignUtil.Sign(APP_KEY, APP_SECRET, TIMESTAMP, NONCE, METHOD, resource, null, null, BODY);

            var client = new RestClient(HOST);
            var request = new RestRequest(resource, method);
            request.AddHeader("appKey", APP_KEY);
            request.AddHeader("timestamp", TIMESTAMP);
            request.AddHeader("nonce", NONCE);
            request.AddHeader("sign", SIGN);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("User-Agent", "Xnebula/1.1.0");
            request.AddParameter("application/json", BODY, ParameterType.RequestBody);
            return await client.ExecuteAsync(request);
        }

    }
}
