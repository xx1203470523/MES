/*
 *creator: Karl
 *
 *describe: 设备点检模板与项目关系仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:22:39
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.EquSpotcheckTemplateItemRelation;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquSpotcheckTemplateItemRelation
{
    /// <summary>
    /// 设备点检模板与项目关系仓储接口
    /// </summary>
    public interface IEquSpotcheckTemplateItemRelationRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSpotcheckTemplateItemRelationEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquSpotcheckTemplateItemRelationEntity equSpotcheckTemplateItemRelationEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSpotcheckTemplateItemRelationEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquSpotcheckTemplateItemRelationEntity> equSpotcheckTemplateItemRelationEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSpotcheckTemplateItemRelationEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquSpotcheckTemplateItemRelationEntity equSpotcheckTemplateItemRelationEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equSpotcheckTemplateItemRelationEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquSpotcheckTemplateItemRelationEntity> equSpotcheckTemplateItemRelationEntitys);

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
        /// 批量删除（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeletesByIdsAsync(IEnumerable<long> spotCheckTemplateIds);

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSpotcheckTemplateItemRelationEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSpotcheckTemplateItemRelationEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equSpotcheckTemplateItemRelationQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSpotcheckTemplateItemRelationEntity>> GetEquSpotcheckTemplateItemRelationEntitiesAsync(EquSpotcheckTemplateItemRelationQuery equSpotcheckTemplateItemRelationQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSpotcheckTemplateItemRelationPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSpotcheckTemplateItemRelationEntity>> GetPagedInfoAsync(EquSpotcheckTemplateItemRelationPagedQuery equSpotcheckTemplateItemRelationPagedQuery);
        #endregion
    }
}
