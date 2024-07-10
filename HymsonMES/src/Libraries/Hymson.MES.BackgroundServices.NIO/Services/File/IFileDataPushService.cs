namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务接口（业务数据-文件）
    /// </summary>
    public interface IFileDataPushService
    {
        /// <summary>
        /// 主数据（获取预授权上传URL）
        /// </summary>
        /// <returns></returns>
        Task DisposableUploadUrlAsync();

        /// <summary>
        /// 主数据（获取附件浏览URL）
        /// </summary>
        /// <returns></returns>
        Task AuthorizeUrlAsync();

    }
}
