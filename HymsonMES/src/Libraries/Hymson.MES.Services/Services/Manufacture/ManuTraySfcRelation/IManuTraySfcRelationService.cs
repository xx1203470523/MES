/*
 *creator: Karl
 *
 *describe: 托盘条码关系    服务接口 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:13
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 托盘条码关系 service接口
    /// </summary>
    public interface IManuTraySfcRelationService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuTraySfcRelationPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuTraySfcRelationDto>> GetPagedListAsync(ManuTraySfcRelationPagedQueryDto manuTraySfcRelationPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuTraySfcRelationCreateDto"></param>
        /// <returns></returns>
        Task CreateManuTraySfcRelationAsync(ManuTraySfcRelationCreateDto manuTraySfcRelationCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuTraySfcRelationModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuTraySfcRelationAsync(ManuTraySfcRelationModifyDto manuTraySfcRelationModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuTraySfcRelationAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuTraySfcRelationAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuTraySfcRelationDto> QueryManuTraySfcRelationByIdAsync(long id);
    }
}
