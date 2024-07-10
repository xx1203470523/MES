using Hymson.MES.Core.Enums.Mavel;

namespace Hymson.MES.BackgroundServices.NIO.Services
{
    /// <summary>
    /// 推送服务（业务数据-文件）
    /// </summary>
    public class FileDataPushService : BasePushService
    {
        /// <summary>
        /// 仓储接口（蔚来推送开关）
        /// </summary>
        private readonly INioPushSwitchRepository _nioPushSwitchRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nioPushSwitchRepository"></param>
        public FileDataPushService(INioPushSwitchRepository nioPushSwitchRepository)
        {
            _nioPushSwitchRepository = nioPushSwitchRepository;
        }

        /// <summary>
        /// 主数据（获取预授权上传URL）
        /// </summary>
        /// <returns></returns>
        public async Task DisposableUploadUrlAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.File_DisposableUpload);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据

            await ExecuteAsync(switchEntity.Path, "TODO");
        }

        /// <summary>
        /// 主数据（获取附件浏览URL）
        /// </summary>
        /// <returns></returns>
        public async Task AuthorizeUrlAsync()
        {
            var switchEntity = await _nioPushSwitchRepository.GetBySceneAsync(BuzSceneEnum.File_AuthorizeUrl);
            if (switchEntity == null) return;

            // TODO: 替换为实际数据

            await ExecuteAsync(switchEntity.Path, "TODO");
        }

    }
}
