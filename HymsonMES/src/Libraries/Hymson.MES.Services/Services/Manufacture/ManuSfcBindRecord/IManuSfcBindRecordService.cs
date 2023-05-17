/*
 *creator: Karl
 *
 *describe: 条码绑定解绑记录表    服务接口 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-17 10:09:25
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 条码绑定解绑记录表 service接口
    /// </summary>
    public interface IManuSfcBindRecordService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuSfcBindRecordPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcBindRecordDto>> GetPagedListAsync(ManuSfcBindRecordPagedQueryDto manuSfcBindRecordPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcBindRecordCreateDto"></param>
        /// <returns></returns>
        Task CreateManuSfcBindRecordAsync(ManuSfcBindRecordCreateDto manuSfcBindRecordCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcBindRecordModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuSfcBindRecordAsync(ManuSfcBindRecordModifyDto manuSfcBindRecordModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuSfcBindRecordAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuSfcBindRecordAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcBindRecordDto> QueryManuSfcBindRecordByIdAsync(long id);
    }
}
