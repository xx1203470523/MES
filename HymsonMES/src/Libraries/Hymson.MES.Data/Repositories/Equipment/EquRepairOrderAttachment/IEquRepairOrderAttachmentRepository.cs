/*
 *creator: Karl
 *
 *describe: 设备维修附件仓储类 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-07-01 04:52:14
 */
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.EquRepairOrderAttachment;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.EquRepairOrderAttachment
{
    /// <summary>
    /// 设备维修附件仓储接口
    /// </summary>
    public interface IEquRepairOrderAttachmentRepository
    {
        #region 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="equRepairOrderAttachmentEntity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(EquRepairOrderAttachmentEntity equRepairOrderAttachmentEntity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="equRepairOrderAttachmentEntitys"></param>
        /// <returns></returns>
        Task<int> InsertsAsync(List<EquRepairOrderAttachmentEntity> equRepairOrderAttachmentEntitys);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equRepairOrderAttachmentEntity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(EquRepairOrderAttachmentEntity equRepairOrderAttachmentEntity);

        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <param name="equRepairOrderAttachmentEntitys"></param>
        /// <returns></returns>
        Task<int> UpdatesAsync(List<EquRepairOrderAttachmentEntity> equRepairOrderAttachmentEntitys);

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
        Task<EquRepairOrderAttachmentEntity> GetByIdAsync(long id);

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<EquRepairOrderAttachmentEntity>> GetByIdsAsync(long[] ids);

        /// <summary>
        /// 获取List
        /// </summary>
        /// <param name="equRepairOrderAttachmentQuery"></param>
        /// <returns></returns>
        Task<IEnumerable<EquRepairOrderAttachmentEntity>> GetEquRepairOrderAttachmentEntitiesAsync(EquRepairOrderAttachmentQuery equRepairOrderAttachmentQuery);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="equRepairOrderAttachmentPagedQuery"></param>
        /// <returns></returns>
        Task<PagedInfo<EquRepairOrderAttachmentEntity>> GetPagedInfoAsync(EquRepairOrderAttachmentPagedQuery equRepairOrderAttachmentPagedQuery);
        #endregion

        /// <summary>
        /// 根据单据Id获取
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<IEnumerable<EquRepairOrderAttachmentEntity>> GetByOrderIdAsync(EquRepairOrderAttachmentQuery query);
    }
}
