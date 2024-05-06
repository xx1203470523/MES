using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquOpenParamRecord;

namespace Hymson.MES.Services.Services.EquOpenParamRecord
{
    /// <summary>
    /// 服务接口（开机参数记录表）
    /// </summary>
    public interface IEquOpenParamRecordService
    {
        /// <summary>
        /// 新增多个
        /// </summary>
        /// <param name="saveDtoList"></param>
        /// <returns></returns>
        Task<int> AddMultAsync(List<EquOpenParamRecordSaveDto> saveDtoList);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquOpenParamRecordDto>> GetPagedListAsync(EquOpenParamRecordPagedQueryDto pagedQueryDto);

    }
}