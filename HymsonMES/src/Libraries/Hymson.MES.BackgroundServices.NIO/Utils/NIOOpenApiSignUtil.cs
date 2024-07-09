using System.Security.Cryptography;
using System.Text;

namespace Hymson.MES.BackgroundServices.NIO.Utils
{
    /// <summary>
    /// OpenApi签名工具类
    /// </summary>
    public class NIOOpenApiSignUtil
    {
        private static readonly string APP_KEY = "appKey";
        private static readonly string TIMESTAMP = "timestamp";
        private static readonly string NONCE = "nonce";
        private static readonly string METHOD = "method";
        private static readonly string PATH = "path";
        private static readonly string JSON_BODY = "jsonBody";

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="appKey">应用唯一标识</param>
        /// <param name="appSecret">密钥</param>
        /// <param name="timestamp">当前时间戳，单位毫秒。</param>
        /// <param name="nonce">随机数</param>
        /// <param name="method">HTTP请求的方式，有POST、GET等</param>
        /// <param name="resource">请求路径</param>
        /// <param name="queryString">HTTP请求的QueryString参数值</param>
        /// <param name="formData">HTTP请求是POST并且类型是Form表单有值</param>
        /// <param name="jsonBody">HTTP请求是POST并且类型是JSON有值</param>
        /// <returns>签名结果</returns>
        public static string Sign(
            string appKey,
            string appSecret,
            string timestamp,
            string nonce,
            string method,
            string resource,
            Dictionary<string, object>? queryString,
            Dictionary<string, object>? formData,
            string jsonBody)
        {
            // 获取原始字符串
            string originStr = BuildOriginStr(appKey, timestamp, nonce, method, resource, queryString, formData, jsonBody);

            // 生成签名
            return GenerateHMACSHA256(appSecret, originStr);
        }

        /// <summary>
        /// 拼接签名的字符串
        /// </summary>
        /// <param name="appKey">应用唯一标识</param>
        /// <param name="timestamp">当前时间戳，单位毫秒。</param>
        /// <param name="nonce">随机数</param>
        /// <param name="method">HTTP请求方式</param>
        /// <param name="resource">请求路径</param>
        /// <param name="queryString">请求参数</param>
        /// <param name="formData">表单数据</param>
        /// <param name="jsonBody">请求的JSON Body数据</param>
        /// <returns>拼接后的字符串</returns>
        public static string BuildOriginStr(
            string appKey,
            string timestamp,
            string nonce,
            string method,
            string resource,
            Dictionary<string, object>? queryString,
            Dictionary<string, object>? formData,
            string jsonBody)
        {
            StringBuilder sb = new();
            sb.Append(METHOD).Append("=").Append(method.ToUpper());
            sb.Append("&").Append(PATH).Append("=").Append(resource);
            sb.Append("&").Append(GetCommonString(appKey, timestamp, nonce));
            if (queryString != null && queryString.Count > 0)
            {
                // 根据Key排序
                Dictionary<string, object> sort = queryString.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
                foreach (KeyValuePair<string, object> item in sort)
                {
                    sb.Append("&").Append(item.Key).Append("=").Append(item.Value);
                }
            }
            if (!string.IsNullOrWhiteSpace(jsonBody))
            {
                // 获取jsonBody的md5值
                MD5 md5 = MD5.Create();
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(jsonBody));
                string md5Value = Convert.ToHexString(bytes).ToLower();
                sb.Append("&").Append(JSON_BODY).Append("=").Append(md5Value);
            }
            if (formData != null && formData.Count > 0)
            {
                // 根据Key排序
                Dictionary<string, object> sort = formData.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
                foreach (KeyValuePair<string, object> item in sort)
                {
                    sb.Append("&").Append(item.Key).Append("=").Append(item.Value);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取公共字符串格式appKey={}&timestamp={}&nonce={}
        /// </summary>
        /// <param name="appKey">应用的唯一标识</param>
        /// <param name="timestamp">当前时间戳，单位秒。</param>
        /// <param name="nonce">随机数</param>
        /// <returns>结果</returns>
        public static string GetCommonString(string appKey, string timestamp, string nonce)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(APP_KEY).Append("=").Append(appKey);
            sb.Append("&");
            sb.Append(TIMESTAMP).Append("=").Append(timestamp);
            sb.Append("&");
            sb.Append(NONCE).Append("=").Append(nonce);
            return sb.ToString();
        }

        /// <summary>
        /// 生成SHA-256，返回16进制的签名。
        /// </summary>
        /// <param name="secret">密钥</param>
        /// <param name="message">消息内容</param>
        /// <returns>16进制的签名</returns>
        public static string GenerateHMACSHA256(string secret, string message)
        {
            string hash = "";
            using (HMACSHA256 mac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                byte[] hashBytes = mac.ComputeHash(Encoding.UTF8.GetBytes(message));
                hash = Convert.ToHexString(hashBytes);
            }
            // 转成小写
            return hash.ToLower();
        }

    }

}