/*
 *creator: Karl
 *
 *describe: 托盘装载信息表    服务接口 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:10:43
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 托盘装载信息表 service接口
    /// </summary>
    public interface IManuTrayLoadService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="manuTrayLoadPagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuTrayLoadDto>> GetPagedListAsync(ManuTrayLoadPagedQueryDto manuTrayLoadPagedQueryDto);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuTrayLoadCreateDto"></param>
        /// <returns></returns>
        Task CreateManuTrayLoadAsync(ManuTrayLoadCreateDto manuTrayLoadCreateDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuTrayLoadModifyDto"></param>
        /// <returns></returns>
        Task ModifyManuTrayLoadAsync(ManuTrayLoadModifyDto manuTrayLoadModifyDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteManuTrayLoadAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesManuTrayLoadAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuTrayLoadDto> QueryManuTrayLoadByIdAsync(long id);
    }
}
