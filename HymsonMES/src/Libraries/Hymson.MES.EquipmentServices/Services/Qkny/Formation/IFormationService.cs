using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.Qkny.ProcSortingRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.Formation
{
    /// <summary>
    /// 化成接口
    /// </summary>
    public interface IFormationService
    {
        /// <summary>
        /// 补液数据上报034
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task FillingDataAsync(FillingDataDto dto);

        /// <summary>
        /// 空托盘校验035
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task EmptyContainerCheckAsync(EmptyContainerCheckDto dto);

        /// <summary>
        /// 单电芯校验036
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ContainerSfcCheckAsync(ContainerSfcCheckDto dto);

        /// <summary>
        /// 托盘电芯绑定(在制品容器)037
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task BindContainerAsync(BindContainerDto dto);

        /// <summary>
        /// 托盘电芯解绑(在制品容器)038
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UnBindContainerAsync(UnBindContainerDto dto);

        /// <summary>
        /// 托盘NG电芯上报039
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ContainerNgReportAsync(ContainerNgReportDto dto);

        /// <summary>
        /// 托盘进站(容器进站)040
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task InboundInContainerAsync(InboundInContainerDto dto);

        /// <summary>
        /// 托盘出站(容器出站)041
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task OutboundInContainerAsync(OutboundInContainerDto dto);

        /// <summary>
        /// 分选规则
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<ProcSortRuleDetailEquDto>> SortingRuleAsync(SortingRuleDto dto);
    }
}
