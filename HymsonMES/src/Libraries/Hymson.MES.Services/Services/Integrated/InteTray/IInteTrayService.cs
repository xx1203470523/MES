using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 托盘信息 service接口
    /// </summary>
    public interface IInteTrayService
    {
        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<PagedInfo<InteTrayDto>> GetPagedListAsync(InteTrayPagedQueryDto parm);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> CreateAsync(InteTraySaveDto parm);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(InteTraySaveDto parm);

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
        Task<InteTrayDto> QueryByIdAsync(long id);
    }
}
