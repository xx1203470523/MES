/*
 *creator: Karl
 *
 *describe: 条码表仓储类 | 代码由框架生成
 *builder:  wangkeming
 *build datetime: 2023-04-10 04:55:42
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.View;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码表仓储接口
    /// </summary>
    public interface IManuSfcRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcEntity manuSfcEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcEntitys"></param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(List<ManuSfcEntity> manuSfcEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcEntity manuSfcEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcEntitys"></param>
        /// <returns></returns>
        Task<int> UpdateRangeAsync(List<ManuSfcEntity> manuSfcEntitys);

        /// <summary>
        /// 删除  
        /// 最好使用批量删除，可以设置更新人和更新时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand command);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuSfcEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcEntity>> GetManuSfcEntitiesAsync(ManuSfcQuery manuSfcQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcPassDownView>> GetPagedListAsync(ManuSfcPassDownPagedQuery pagedQuery);

        /// <summary>
        /// 查询条码信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcView>> GetManuSfcInfoEntitiesAsync(ManuSfcStatusQuery param);

        /// <summary>
        /// 批量更新条码状态
        /// </summary>
        /// <param name="manuSfcInfoEntity"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(ManuSfcUpdateCommand command);

        /// <summary>
        /// 获取SFC
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<ManuSfcEntity> GetBySFCAsync(string sfc);

        /// <summary>
        /// 更具sfc 获取条码信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcEntity>> GetBySFCsAsync(IEnumerable<string> sfcs);
        #endregion
    }
}
