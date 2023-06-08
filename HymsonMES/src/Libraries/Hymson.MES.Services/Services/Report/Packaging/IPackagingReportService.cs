using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 包装报告service接口
    /// </summary>
    public interface IPackagingReportService
    {
        /// <summary>
        /// 根据容器编码或者装载条码查询容器当前信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<ManuContainerBarcodeViewDto> QueryManuContainerByCodeAsync(PackagingQueryDto queryDto);

        /// <summary>
        /// 查询工单信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<PlanWorkPackDto> GetByWorkOrderCodeAsync(PackagingQueryDto queryDto);

        /// <summary>
        /// 根据查询条件获取分页数据:根据容器编号、sfc查询包装信息
        /// </summary>
        /// <param name="manuContainerPackPagedQueryDto"></param>
        /// <returns></returns>
         Task<PagedInfo<ManuContainerPackDto>> GetContainerPackPagedListAsync(ManuContainerPackPagedQueryDto manuContainerPackPagedQueryDto);

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkPackingDto>> GetPagedListAsync(ManuContainerBarcodePagedQueryDto queryDto);

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        Task<PagedInfo<PlanWorkPackingDto>> GetPagedRecordListAsync(ManuContainerBarcodePagedQueryDto queryDto);
    }
}
