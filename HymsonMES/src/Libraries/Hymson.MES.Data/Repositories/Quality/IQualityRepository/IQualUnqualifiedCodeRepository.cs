using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.Query;
using Hymson.MES.Data.Repositories.Quality.QualUnqualifiedCode.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Quality.IQualityRepository
{
    /// <summary>
    /// 不合格代码仓储接口
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public interface IQualUnqualifiedCodeRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualUnqualifiedCodeEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(QualUnqualifiedCodeEntity qualUnqualifiedCodeEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="qualUnqualifiedCodeEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<QualUnqualifiedCodeEntity> qualUnqualifiedCodeEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="qualUnqualifiedCodeEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(QualUnqualifiedCodeEntity qualUnqualifiedCodeEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="qualUnqualifiedCodeEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<QualUnqualifiedCodeEntity> qualUnqualifiedCodeEntitys);

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
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualUnqualifiedCodeEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedCodeEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<QualUnqualifiedCodeEntity> GetByCodeAsync(QualUnqualifiedCodeByCodeQuery parm);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="qualUnqualifiedCodeQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedCodeEntity>> GetQualUnqualifiedCodeEntitiesAsync(QualUnqualifiedCodeQuery qualUnqualifiedCodeQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="qualUnqualifiedCodePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<QualUnqualifiedCodeEntity>> GetPagedInfoAsync(QualUnqualifiedCodePagedQuery qualUnqualifiedCodePagedQuery);

        /// <summary>
        /// 获取不合格代码关联不合格代码关系表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<IEnumerable<UnqualifiedCodeGroupRelationView>> GetQualUnqualifiedCodeGroupRelationAsync(long Id);
    }
}
