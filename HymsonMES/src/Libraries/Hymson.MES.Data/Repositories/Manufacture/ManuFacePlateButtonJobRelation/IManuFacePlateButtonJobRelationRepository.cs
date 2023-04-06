/*
 *creator: Karl
 *
 *describe: 操作面板按钮作业仓储类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 03:34:48
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 操作面板按钮作业仓储接口
    /// </summary>
    public interface IManuFacePlateButtonJobRelationRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuFacePlateButtonJobRelationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuFacePlateButtonJobRelationEntity manuFacePlateButtonJobRelationEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuFacePlateButtonJobRelationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuFacePlateButtonJobRelationEntity> manuFacePlateButtonJobRelationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuFacePlateButtonJobRelationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuFacePlateButtonJobRelationEntity manuFacePlateButtonJobRelationEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuFacePlateButtonJobRelationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuFacePlateButtonJobRelationEntity> manuFacePlateButtonJobRelationEntitys);

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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuFacePlateButtonJobRelationEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFacePlateButtonJobRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuFacePlateButtonJobRelationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuFacePlateButtonJobRelationEntity>> GetManuFacePlateButtonJobRelationEntitiesAsync(ManuFacePlateButtonJobRelationQuery manuFacePlateButtonJobRelationQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuFacePlateButtonJobRelationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuFacePlateButtonJobRelationEntity>> GetPagedInfoAsync(ManuFacePlateButtonJobRelationPagedQuery manuFacePlateButtonJobRelationPagedQuery);
        #endregion
    }
}
