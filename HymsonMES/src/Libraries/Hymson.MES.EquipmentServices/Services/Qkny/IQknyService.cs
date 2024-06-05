using Hymson.MES.CoreServices.Dtos.Manufacture.ManuBind;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.Qkny.ProcSortingRule;
using Hymson.MES.EquipmentServices.Dtos.Qkny.ToolBindMaterial;
using Hymson.MES.EquipmentServices.Services.Manufacture;
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
        Task<OutReportSfcReturnDto> OutboundMetersReportAsync(OutboundMetersReportDto dto);

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
        /// 产品参数上传043
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ProductParamAsync(ProductParamDto dto);

        /// <summary>
        /// 库存接收
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task MaterialInventoryAsync(MaterialInventoryDto dto);

        /// <summary>
        /// 工装条码绑定048
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task ToolBindMaterialAsync(ToolBindMaterialDto dto);

        /// <summary>
        /// 获取电芯降级信息051
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<SortingSfcInfo>> GetSfcInfoAsync(GetSfcInfoDto dto);

        /// <summary>
        /// 分选拆盘052
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task SortingUnBindAsync(SortingUnBindDto dto);

        /// <summary>
        /// 分选出站053
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task SortingOutboundAsync(SortingOutboundDto dto);

        /// <summary>
        /// 获取设备token
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<string> GetEquTokenAsync(QknyBaseDto dto);

        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<string> SendHttpAsync(SendHttpDto dto);
    }
}
