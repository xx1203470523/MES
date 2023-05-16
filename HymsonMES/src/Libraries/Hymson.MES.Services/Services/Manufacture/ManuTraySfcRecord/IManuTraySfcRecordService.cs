/*
 *creator: Karl
 *
 *describe: 托盘条码记录表    服务接口 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:02
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 托盘条码记录表 service接口
    /// </summary>
    public interface IManuTraySfcRecordService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuTraySfcRecordPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuTraySfcRecordDto>> GetPagedListAsync(ManuTraySfcRecordPagedQueryDto manuTraySfcRecordPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuTraySfcRecordCreateDto"></param>
        /// <returns></returns>
        Task CreateManuTraySfcRecordAsync(ManuTraySfcRecordCreateDto manuTraySfcRecordCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuTraySfcRecordModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuTraySfcRecordAsync(ManuTraySfcRecordModifyDto manuTraySfcRecordModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuTraySfcRecordAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuTraySfcRecordAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuTraySfcRecordDto> QueryManuTraySfcRecordByIdAsync(long id);
    }
}
