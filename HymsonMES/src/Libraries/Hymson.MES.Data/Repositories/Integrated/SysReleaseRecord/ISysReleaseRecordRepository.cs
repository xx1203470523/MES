/*
 *creator: Karl
 *
 *describe: 发布记录表仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-12-19 10:03:09
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Integrated
{
    /// <summary>
    /// 发布记录表仓储接口
    /// </summary>
    public interface ISysReleaseRecordRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sysReleaseRecordEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(SysReleaseRecordEntity sysReleaseRecordEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="sysReleaseRecordEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<SysReleaseRecordEntity> sysReleaseRecordEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sysReleaseRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(SysReleaseRecordEntity sysReleaseRecordEntity);

        /// <summary>
        /// 更新 状态
        /// </summary>
        /// <param name="sysReleaseRecordEntity"></param>
        /// <returns></returns>
        Task<int> UpdateStatusAsync(SysReleaseRecordEntity sysReleaseRecordEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="sysReleaseRecordEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<SysReleaseRecordEntity> sysReleaseRecordEntitys);

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
        Task<SysReleaseRecordEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据Version获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SysReleaseRecordEntity> GetByVersionAsync(SysReleaseRecordPagedQuery param);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<SysReleaseRecordEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="sysReleaseRecordQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<SysReleaseRecordEntity>> GetSysReleaseRecordEntitiesAsync(SysReleaseRecordQuery sysReleaseRecordQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="sysReleaseRecordPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<SysReleaseRecordEntity>> GetPagedInfoAsync(SysReleaseRecordPagedQuery sysReleaseRecordPagedQuery);
        #endregion
    }
}
