/*
 *creator: Karl
 *
 *describe: 设备点检模板仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-05-13 03:06:41
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquSpotcheckTemplate
{
    /// <summary>
    /// 设备点检模板仓储接口
    /// </summary>
    public interface IEquSpotcheckTemplateRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equSpotcheckTemplateEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquSpotcheckTemplateEntity equSpotcheckTemplateEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equSpotcheckTemplateEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquSpotcheckTemplateEntity> equSpotcheckTemplateEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equSpotcheckTemplateEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquSpotcheckTemplateEntity equSpotcheckTemplateEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equSpotcheckTemplateEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquSpotcheckTemplateEntity> equSpotcheckTemplateEntitys);

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
        Task<EquSpotcheckTemplateEntity> GetByIdAsync(long id);


        /// <summary>
        /// 根据Code获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EquSpotcheckTemplateEntity> GetByCodeAsync(EquSpotcheckTemplateQuery param);


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSpotcheckTemplateEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equSpotcheckTemplateQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquSpotcheckTemplateEntity>> GetEquSpotcheckTemplateEntitiesAsync(EquSpotcheckTemplateQuery equSpotcheckTemplateQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equSpotcheckTemplatePagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquSpotcheckTemplateEntity>> GetPagedInfoAsync(EquSpotcheckTemplatePagedQuery equSpotcheckTemplatePagedQuery);
        #endregion
    }
}
