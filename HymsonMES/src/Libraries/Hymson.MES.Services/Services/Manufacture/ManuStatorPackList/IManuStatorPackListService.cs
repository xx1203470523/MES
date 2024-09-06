using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务接口（定子装箱记录表）
    /// </summary>
    public interface IManuStatorPackListService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ManuStatorPackListSaveDto saveDto);

        /// <summary>
        /// 生成箱体码
        /// </summary>
        /// <returns></returns>
        Task<string> AddBoxAsync();

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> PrintAsync(ManuStatorPackPrintDto param);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ManuStatorPackListSaveDto saveDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(long[] ids);

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuStatorPackListDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuStatorPackListDto>> GetPagedListAsync(ManuStatorPackListPagedQueryDto pagedQueryDto);

    }
}