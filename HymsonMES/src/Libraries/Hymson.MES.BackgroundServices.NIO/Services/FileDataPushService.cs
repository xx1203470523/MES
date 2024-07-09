using RestSharp;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务（业务数据-文件）
    /// </summary>
    public class FileDataPushService : BasePushService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileDataPushService() { }

        /// <summary>
        /// 主数据（获取预授权上传URL）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> DisposableUploadUrlAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/file/disposable_upload_url", jsonBody, Method.Post);
        }

        /// <summary>
        /// 主数据（获取附件浏览URL）
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns></returns>
        public static async Task<RestResponse> AuthorizeUrlAsync(object jsonBody)
        {
            return await ExecuteAsync("/v1/trans/file/authorize_url", jsonBody, Method.Post);
        }

    }
}
