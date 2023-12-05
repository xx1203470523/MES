using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcGrade.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    /// <summary>
    /// 条码档位表仓储接口
    /// </summary>
    public interface IManuSfcGradeRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="manuSfcGradeEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(ManuSfcGradeEntity manuSfcGradeEntity);
        
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="manuSfcGradeEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<ManuSfcGradeEntity> manuSfcGradeEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="manuSfcGradeEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(ManuSfcGradeEntity manuSfcGradeEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="gradeCommands"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<UpdateGradeCommand> gradeCommands);

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
        Task<ManuSfcGradeEntity> GetByIdAsync(long id);
    
        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcGradeEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="manuSfcGradeQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuSfcGradeEntity>> GetManuSfcGradeEntitiesAsync(ManuSfcGradeQuery manuSfcGradeQuery);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="manuSfcGradePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcGradeEntity>> GetPagedInfoAsync(ManuSfcGradePagedQuery manuSfcGradePagedQuery);
        #endregion
    }
}
