using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture.ManuSfc
{
    /// <summary>
    /// 条码服务接口
    /// </summary>
    public interface IManuSfcService
    {
        /// <summary>
        /// 分页查询列表（条码打印）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcPassDownDto>> GetPagedListAsync(ManuSfcPassDownPagedQueryDto pagedQueryDto);

    }
}
