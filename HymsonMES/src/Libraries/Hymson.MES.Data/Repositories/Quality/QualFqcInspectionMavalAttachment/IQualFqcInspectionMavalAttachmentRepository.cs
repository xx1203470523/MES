/*
 *creator: Karl
 *
 *describe: 马威QFC检验附件仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-07-24 10:03:33
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.QualFqcInspectionMavalAttachment;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.QualFqcInspectionMavalAttachment
{
    /// <summary>
    /// 马威QFC检验附件仓储接口
    /// </summary>
    public interface IQualFqcInspectionMavalAttachmentRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="qualFqcInspectionMavalAttachmentEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(QualFqcInspectionMavalAttachmentEntity qualFqcInspectionMavalAttachmentEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="qualFqcInspectionMavalAttachmentEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<QualFqcInspectionMavalAttachmentEntity> qualFqcInspectionMavalAttachmentEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="qualFqcInspectionMavalAttachmentEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(QualFqcInspectionMavalAttachmentEntity qualFqcInspectionMavalAttachmentEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="qualFqcInspectionMavalAttachmentEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<QualFqcInspectionMavalAttachmentEntity> qualFqcInspectionMavalAttachmentEntitys);

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
        Task<QualFqcInspectionMavalAttachmentEntity> GetByIdAsync(long id);


        /// <summary>
        /// 根据FqcMavalId获取数据
        /// </summary>
        /// <param name="fqcMavalId"></param>
        /// <returns></returns> 
        Task<IEnumerable<QualFqcInspectionMavalAttachmentEntity>> GetByFqcMavalIdListAsync(long fqcMavalId); 


        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<QualFqcInspectionMavalAttachmentEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="qualFqcInspectionMavalAttachmentQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<QualFqcInspectionMavalAttachmentEntity>> GetQualFqcInspectionMavalAttachmentEntitiesAsync(QualFqcInspectionMavalAttachmentQuery qualFqcInspectionMavalAttachmentQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="qualFqcInspectionMavalAttachmentPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<QualFqcInspectionMavalAttachmentEntity>> GetPagedInfoAsync(QualFqcInspectionMavalAttachmentPagedQuery qualFqcInspectionMavalAttachmentPagedQuery);
        #endregion
    }
}
