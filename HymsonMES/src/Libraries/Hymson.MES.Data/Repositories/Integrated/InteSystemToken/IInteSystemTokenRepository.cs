/*
 *creator: Karl
 *
 *describe: 系统Token仓储类 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-15 02:09:57
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 系统Token仓储接口
    /// </summary>
    public interface IInteSystemTokenRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="inteSystemTokenEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(InteSystemTokenEntity inteSystemTokenEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteSystemTokenEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<InteSystemTokenEntity> inteSystemTokenEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inteSystemTokenEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(InteSystemTokenEntity inteSystemTokenEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="inteSystemTokenEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<InteSystemTokenEntity> inteSystemTokenEntitys);

        /// <summary>
        /// 更新token信息
        /// </summary>
        /// <param name="inteSystemTokenEntity"></param>
        /// <returns></returns>
        Task<int> UpdateTokenAsync(InteSystemTokenEntity inteSystemTokenEntity);

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
        Task<InteSystemTokenEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<InteSystemTokenEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 根据系统编码获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<InteSystemTokenEntity> GetByCodeAsync(InteSystemTokenQuery param);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="inteSystemTokenQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<InteSystemTokenEntity>> GetInteSystemTokenEntitiesAsync(InteSystemTokenQuery inteSystemTokenQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<InteSystemTokenEntity>> GetPagedInfoAsync(InteSystemTokenPagedQuery pagedQuery);
        #endregion
    }
}
