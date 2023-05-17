/*
 *creator: Karl
 *
 *describe: 条码绑定关系表    服务接口 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-17 10:09:11
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 条码绑定关系表 service接口
    /// </summary>
    public interface IManuSfcBindService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuSfcBindPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcBindDto>> GetPagedListAsync(ManuSfcBindPagedQueryDto manuSfcBindPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcBindCreateDto"></param>
        /// <returns></returns>
        Task CreateManuSfcBindAsync(ManuSfcBindCreateDto manuSfcBindCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcBindModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuSfcBindAsync(ManuSfcBindModifyDto manuSfcBindModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuSfcBindAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuSfcBindAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcBindDto> QueryManuSfcBindByIdAsync(long id);
    }
}
