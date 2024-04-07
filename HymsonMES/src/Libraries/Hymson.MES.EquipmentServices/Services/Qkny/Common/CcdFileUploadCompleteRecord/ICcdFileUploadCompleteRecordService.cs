using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.CcdFileUploadCompleteRecord;

namespace Hymson.MES.Services.Services.CcdFileUploadCompleteRecord
{
    /// <summary>
    /// 服务接口（CCD文件上传完成）
    /// </summary>
    public interface ICcdFileUploadCompleteRecordService
    {
        /// <summary>
        /// 添加多个
        /// </summary>
        /// <param name="saveDtoList"></param>
        /// <returns></returns>
        Task<int> AddMultAsync(List<CcdFileUploadCompleteRecordSaveDto> saveDtoList);
    }
}