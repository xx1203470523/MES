/*
 *creator: Karl
 *
 *describe: 工单信息表    服务接口 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-20 10:07:17
 */
using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.NioPushCollection;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 不良记录报表
    /// </summary>
    public interface IWorkshopJobControlReportService
    {

        /// <summary>
        /// 根据查询条件获取车间作业控制报表分页数据
        /// 优化: 不模糊查询，且通过关联ID查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedInfo<WorkshopJobControlReportViewDto>> GetWorkshopJobControlPageListAsync(WorkshopJobControlReportOptimizePagedQueryDto param);

        /// <summary>
        /// 根据查询条件导出参数数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<NioPushCollectionExportResultDto> ExprotAsync(WorkshopJobControlReportOptimizePagedQueryDto param);

        /// <summary>
        /// 获取SFC的车间作业控制步骤
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        Task<WorkshopJobControlStepReportDto> GetSfcInOutInfoAsync(string sfc);

        /// <summary>
        ///获取步骤详情
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<StepDetailDto> GetStepDetailAsync(StepQueryDto query);

        /// <summary>
        /// 根据SFC分页获取条码步骤信息
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        Task<PagedInfo<ManuSfcStepBySfcViewDto>> GetSFCStepsBySFCPageListAsync(ManuSfcStepBySfcPagedQueryDto queryParam);
    }
}
