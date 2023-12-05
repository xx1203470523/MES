using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码档位明细表仓储接口
    /// </summary>
    public interface IManuSfcGradeDetailRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcGradeDetailEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcGradeDetailEntity manuSfcGradeDetailEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcGradeDetailEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuSfcGradeDetailEntity> manuSfcGradeDetailEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcGradeDetailEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcGradeDetailEntity manuSfcGradeDetailEntity);
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="manuSfcGradeDetailEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<ManuSfcGradeDetailEntity> manuSfcGradeDetailEntitys);

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
        Task<ManuSfcGradeDetailEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcGradeDetailEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcGradeDetailQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcGradeDetailEntity>> GetManuSfcGradeDetailEntitiesAsync(ManuSfcGradeDetailQuery manuSfcGradeDetailQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcGradeDetailPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcGradeDetailEntity>> GetPagedInfoAsync(ManuSfcGradeDetailPagedQuery manuSfcGradeDetailPagedQuery);

        /// <summary>
        /// 根据档位Id查询明细
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcGradeDetailEntity>> GetByGradeIdAsync(ManuSfcGradeDetailByGradeIdQuery query);
        #endregion
    }
}
