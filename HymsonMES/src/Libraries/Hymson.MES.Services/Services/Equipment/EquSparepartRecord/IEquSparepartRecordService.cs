/*
 *creator: Karl
 *
 *describe: 设备备件记录表    服务接口 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:29:55
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.EquSparepartRecord;

namespace Hymson.MES.Services.Services.EquSparepartRecord
{
    /// <summary>
    /// 设备备件记录表 service接口
    /// </summary>
    public interface IEquSparepartRecordService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="equSparepartRecordPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSparepartRecordDto>> GetPagedListAsync(EquSparepartRecordPagedQueryDto equSparepartRecordPagedQueryDto);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSparepartRecordDto> QueryEquSparepartRecordByIdAsync(long id);
    }
}
