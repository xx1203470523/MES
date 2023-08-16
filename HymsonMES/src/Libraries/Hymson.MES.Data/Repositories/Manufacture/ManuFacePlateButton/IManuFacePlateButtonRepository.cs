/*
 *creator: Karl
 *
 *describe: 操作面板按钮仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 02:58:19
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 操作面板按钮仓储接口
    /// </summary>
    public interface IManuFacePlateButtonRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateButtonEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuFacePlateButtonEntity manuFacePlateButtonEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuFacePlateButtonEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuFacePlateButtonEntity> manuFacePlateButtonEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateButtonEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuFacePlateButtonEntity manuFacePlateButtonEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuFacePlateButtonEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuFacePlateButtonEntity> manuFacePlateButtonEntitys);

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
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        Task<int> DeleteTrueAsync(long facePlateId);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuFacePlateButtonEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据FacePlateId获取数据
        /// </summary>
        /// <param name="facePlateId"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFacePlateButtonEntity>> GetByFacePlateIdAsync(long facePlateId);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFacePlateButtonEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuFacePlateButtonQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFacePlateButtonEntity>> GetManuFacePlateButtonEntitiesAsync(ManuFacePlateButtonQuery manuFacePlateButtonQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuFacePlateButtonPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuFacePlateButtonEntity>> GetPagedInfoAsync(ManuFacePlateButtonPagedQuery manuFacePlateButtonPagedQuery);
        #endregion
    }
}
