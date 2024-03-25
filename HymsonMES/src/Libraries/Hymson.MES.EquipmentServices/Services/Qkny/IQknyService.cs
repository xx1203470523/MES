using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.Qkny.ProcSortingRule;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny
{
    /// <summary>
    /// 顷刻能源服务
    /// </summary>
    public interface IQknyService
    {
        /// <summary>
        /// 原材料上料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task FeedingAsync(FeedingDto dto);

        /// <summary>
        /// 半成品上料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task HalfFeedingAsync(HalfFeedingDto dto);

        /// <summary>
        /// AGV叫料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task AgvMaterialAsync(AgvMaterialDto dto);

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<string>> GenerateSfcAsync(GenerateSfcDto dto);

        /// <summary>
        /// 产出米数上报
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task OutboundMetersReportAsync(OutboundMetersReportDto dto);

        /// <summary>
        /// 获取下发条码(用于CCD面密度)025
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<CcdGetBarcodeReturnDto> CcdGetBarcodeAsync(CCDFileUploadCompleteDto dto);

        /// <summary>
        /// 设备过程参数
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task EquipmentProcessParamAsync(EquipmentProcessParamDto dto);

        /// <summary>
        /// 进站
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task InboundAsync(InboundDto dto);

        /// <summary>
        /// 出站
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task OutboundAsync(OutboundDto dto);

        /// <summary>
        /// 进站多个029
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<InboundMoreReturnDto>> InboundMoreAsync(InboundMoreDto dto);

        /// <summary>
        /// 出站多个
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<OutboundMoreReturnDto>> OutboundMoreAsync(OutboundMoreDto dto);

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
        /// 产品参数上传043
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ProductParamAsync(ProductParamDto dto);

        /// <summary>
        /// 卷绕极组产出上报
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task CollingPolarAsync(CollingPolarDto dto);

        /// <summary>
        /// 分选规则
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<ProcSortRuleDetailEquDto>> SortingRuleAsync(SortingRuleDto dto);
    }
}
