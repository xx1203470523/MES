using RestSharp;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务（业务数据）
    /// </summary>
    public class BuzDataPushService : BasePushService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BuzDataPushService() { }

        /// <summary>
        /// 业务数据（控制项）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> CollectionAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/processdata/collection", jsonBody, Method.Post);
        }

        /// <summary>
        /// 业务数据（生产业务）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> ProductionAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/processdata/production", jsonBody, Method.Post);
        }

        /// <summary>
        /// 业务数据（材料清单）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> MaterialAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/processdata/material", jsonBody, Method.Post);
        }

        /// <summary>
        /// 业务数据（产品一次合格率）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> PassrateProductAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/processdata/passrate_product", jsonBody, Method.Post);
        }

        /// <summary>
        /// 业务数据（工位一次合格率）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> PassrateStationAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/processdata/passrate_station", jsonBody, Method.Post);
        }

        /// <summary>
        /// 业务数据（环境业务）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> DataEnvAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/processdata/dataenv", jsonBody, Method.Post);
        }

        /// <summary>
        /// 业务数据（缺陷业务）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> IssueAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/processdata/issue", jsonBody, Method.Post);
        }

        /// <summary>
        /// 业务数据（工单业务）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> WorkOrderAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/processdata/workorder", jsonBody, Method.Post);
        }

        /// <summary>
        /// 业务数据（通用业务）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> CommonAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/processdata/common", jsonBody, Method.Post);
        }

        /// <summary>
        /// 业务数据（附件）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> AttachmentAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/processdata/attachment", jsonBody, Method.Post);
        }

    }
}
