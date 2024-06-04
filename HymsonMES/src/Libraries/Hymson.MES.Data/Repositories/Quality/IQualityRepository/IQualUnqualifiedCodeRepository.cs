using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
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
        /// 分页查询
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<PagedInfo<QualUnqualifiedCodeEntity>> GetPagedInfoAsync(QualUnqualifiedCodePagedQuery parm);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QualUnqualifiedCodeEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedCodeEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> InsertAsync(QualUnqualifiedCodeEntity parm);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<QualUnqualifiedCodeEntity> parm);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(QualUnqualifiedCodeEntity parm);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<QualUnqualifiedCodeEntity> parm);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> DeletesAsync(DeleteCommand param);

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        Task<QualUnqualifiedCodeEntity> GetByCodeAsync(QualUnqualifiedCodeByCodeQuery parm);

        /// <summary>
        /// 根据编码批量获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedCodeEntity>> GetByCodesAsync(QualUnqualifiedCodeByCodesQuery param);

        /// <summary>
        /// 获取不合格代码关联不合格代码关系表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedCodeGroupRelationView>> GetQualUnqualifiedCodeGroupRelationAsync(long Id);

        /// <summary>
        /// 根据不合格代码组id查询不合格代码列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedCodeEntity>> GetListByGroupIdAsync(QualUnqualifiedCodeQuery query);

        /// <summary>
        /// 根据不合格代码组id查询不合格代码列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<QualUnqualifiedCodeEntity>> GetListAsync(QualUnqualifiedCodeQuery query);
    }
}
