using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.ManuJzBind;
using Hymson.MES.Data.Repositories.Equipment.Qkny.ManuJzBind.Command;
using Hymson.MES.Data.Repositories.ManuJzBind.Query;
using Hymson.MES.Services.Dtos.ManuJzBind;

namespace Hymson.MES.Services.Services.ManuJzBind
{
    /// <summary>
    /// 服务接口（极组绑定）
    /// </summary>
    public interface IManuJzBindService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> AddAsync(ManuJzBindSaveDto saveDto);

        /// <summary>
        /// 根据极组条码查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<ManuJzBindEntity> GetByJzSfcAsync(ManuJzBindQuery query);

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeletePhysicsAsync(long id);

        /// <summary>
        /// 根据id更新电芯码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> UpdateSfcById(UpdateSfcByIdCommand command);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ManuJzBindSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ManuJzBindSaveDto saveDto);

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
        Task<ManuJzBindDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuJzBindDto>> GetPagedListAsync(ManuJzBindPagedQueryDto pagedQueryDto);

    }
}