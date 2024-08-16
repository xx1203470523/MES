using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Manufacture.ManuJointProductAndByproductsReceiveRecord
{
    /// <summary>
    /// 服务接口（联副产品收货）
    /// </summary>
    public interface IManuJointProductAndByproductsReceiveRecordService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> CreateAsync(ManuJointProductAndByproductsReceiveRecordSaveDto saveDto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<int> ModifyAsync(ManuJointProductAndByproductsReceiveRecordSaveDto saveDto);

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
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ManuJointProductAndByproductsReceiveRecordDto?> QueryByIdAsync(long id);

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuJointProductAndByproductsReceiveRecordDto>> GetPagedListAsync(ManuJointProductAndByproductsReceiveRecordPagedQueryDto pagedQueryDto);

        /// <summary>
        /// 根据工单Id查询Bom联副产品列表
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<ManuJointProductAndByproductsReceiveRecordResult> GetWorkIdByBomJointProductAndByProductsListAsync(long workOrderId);

        /// <summary>
        /// 保存联副产品收货信息
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        Task<ManuJointProductAndByproductsReceiveRecordSaveResultDto> SaveJointProductAndByproductsInfoAsync(ManuJointProductAndByproductsReceiveRecordSaveDto saveDto);

        /// <summary>
        /// 查询副产品信息
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        Task<ManuJointProductAndByproductsReceiveRecordResult> GetWorkIdByProductsListAsync(long workOrderId);
    }
}