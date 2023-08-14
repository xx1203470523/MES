/*
 *creator: Karl
 *
 *describe: 降级品录入记录    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:49
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 降级品录入记录 service接口
    /// </summary>
    public interface IManuDowngradingRecordService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuDowngradingRecordPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuDowngradingRecordDto>> GetPagedListAsync(ManuDowngradingRecordPagedQueryDto manuDowngradingRecordPagedQueryDto);
    }
}
