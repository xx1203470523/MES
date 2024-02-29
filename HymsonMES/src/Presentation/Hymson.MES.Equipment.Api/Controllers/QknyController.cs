using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.EquipmentServices;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.Utils;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 顷刻能源设备接口控制器
    /// </summary>
    [ApiController]
    [Route("QknyEqu/api/v1")]
    public class QknyController : ControllerBase
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public QknyController()
        {

        }

        /// <summary>
        /// 操作员登录001
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OperatorLogin")]
        [LogDescription("操作员登录001", BusinessType.OTHER, "OperatorLoginMes001", ReceiverTypeEnum.MES)]
        public async Task OperatorLoginAsync(OperationLoginDto dto)
        {
            List<string> list = null;
            int count = list.Count;

            //throw new CustomerValidationException(nameof(ErrorCode.MES10100));
        }

        /// <summary>
        /// 设备运行报警信息002
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Alarm")]
        [LogDescription("设备运行报警信息002", BusinessType.OTHER, "Alarm002", ReceiverTypeEnum.MES)]
        public async Task AlarmAsync(AlarmDto dto)
        {
            
        }

        /// <summary>
        /// 时间同步003
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TimeSynch")]
        [LogDescription("时间同步003", BusinessType.OTHER, "TimeSynchMes003", ReceiverTypeEnum.MES)]
        public async Task<string> TimeSynchAsync(QknyBaseDto dto)
        {
            DateTime date = HymsonClock.Now();

            return date.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// 设备状态上报004
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("State")]
        [LogDescription("设备状态上报004", BusinessType.OTHER, "State004", ReceiverTypeEnum.MES)]
        public async Task StateAsync(StateDto dto)
        {

        }

        /// <summary>
        /// 设备心跳005
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Heartbeat")]
        [LogDescription("设备心跳005", BusinessType.OTHER, "Heartbeat005", ReceiverTypeEnum.MES)]
        public async Task HeartbeatAsync(HeartbeatDto dto)
        {

        }

        /// <summary>
        /// 设备过程参数006
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EquipmentProcessParam")]
        [LogDescription("设备过程参数006", BusinessType.OTHER, "EquipmentProcessParam006", ReceiverTypeEnum.MES)]
        public async Task EquipmentProcessParamAsync(EquipmentProcessParamDto dto)
        {

        }

        /// <summary>
        /// 产品进站007
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Inbound")]
        [LogDescription("产品进站007", BusinessType.OTHER, "Inbound007", ReceiverTypeEnum.MES)]
        public async Task InboundAsync(InboundDto dto)
        {

        }

        /// <summary>
        /// 产品出站008
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Outbound")]
        [LogDescription("产品进站008", BusinessType.OTHER, "Outbound008", ReceiverTypeEnum.MES)]
        public async Task OutboundAsync(OutboundDto dto)
        {

        }

        /// <summary>
        /// 进站多个009
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InboundMore")]
        [LogDescription("进站多个009", BusinessType.OTHER, "InboundMore009", ReceiverTypeEnum.MES)]
        public async Task<InboundMoreReturnDto> InboundMoreAsync(InboundMoreDto dto)
        {
            InboundMoreReturnDto result = new InboundMoreReturnDto();

            return result;
        }

        /// <summary>
        /// 出站多个010
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundMore")]
        [LogDescription("出站多个010", BusinessType.OTHER, "OutboundMore010", ReceiverTypeEnum.MES)]
        public async Task<OutboundMoreReturnDto> OutboundMoreAsync(OutboundMoreDto dto)
        {
            OutboundMoreReturnDto result = new OutboundMoreReturnDto();

            return result;
        }

        /// <summary>
        /// 获取开机参数列表011
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRecipeList")]
        [LogDescription("获取开机参数列表011", BusinessType.OTHER, "GetRecipeList011", ReceiverTypeEnum.MES)]
        public async Task<GetRecipeListReturnDto> GetRecipeListAsync(GetRecipeListDto dto)
        {
            GetRecipeListReturnDto result = new GetRecipeListReturnDto();

            return result;
        }

        /// <summary>
        /// 获取开机参数明细012
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRecipeDetail")]
        [LogDescription("获取开机参数明细012", BusinessType.OTHER, "GetRecipeDetail012", ReceiverTypeEnum.MES)]
        public async Task<GetRecipeDetailReturnDto> GetRecipeDetailAsync(GetRecipeDetailDto dto)
        {
            GetRecipeDetailReturnDto result = new GetRecipeDetailReturnDto();

            return result;
        }

        /// <summary>
        /// 开机参数校验采集013
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Recipe")]
        [LogDescription("开机参数校验采集013", BusinessType.OTHER, "Recipe013", ReceiverTypeEnum.MES)]
        public async Task RecipeAsync(RecipeDto dto)
        {
            
        }

        /// <summary>
        /// 原材料上料014
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Feeding")]
        [LogDescription("原材料上料014", BusinessType.OTHER, "Feeding014", ReceiverTypeEnum.MES)]
        public async Task FeedingAsync(FeedingDto dto)
        {

        }

        /// <summary>
        /// 半成品上料015
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HalfFeeding")]
        [LogDescription("半成品上料015", BusinessType.OTHER, "HalfFeeding015", ReceiverTypeEnum.MES)]
        public async Task HalfFeedingAsync(HalfFeedingDto dto)
        {

        }

        /// <summary>
        /// 上料呼叫Agv016
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AgvUpMaterial")]
        [LogDescription("上料呼叫Agv016", BusinessType.OTHER, "AgvUpMaterial016", ReceiverTypeEnum.MES)]
        public async Task AgvUpMaterialAsync(AgvUpMaterialDto dto)
        {

        }

        /// <summary>
        /// 下料呼叫Agv017
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AgvDownMaterial")]
        [LogDescription("下料呼叫Agv017", BusinessType.OTHER, "AgvDownMaterial017", ReceiverTypeEnum.MES)]
        public async Task AgvDownMaterialAsync(AgvUpMaterialDto dto)
        {

        }

        /// <summary>
        /// 补液数据上报018
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FillingData")]
        [LogDescription("补液数据上报018", BusinessType.OTHER, "FillingData018", ReceiverTypeEnum.MES)]
        public async Task FillingDataAsync(FillingDataDto dto)
        {

        }

        /// <summary>
        /// 托盘电芯绑定(在制品容器)019
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BindContainer")]
        [LogDescription("托盘电芯绑定(在制品容器)019", BusinessType.OTHER, "BindContainer019", ReceiverTypeEnum.MES)]
        public async Task BindContainerAsync(BindContainerDto dto)
        {

        }

        /// <summary>
        /// 托盘电芯解绑(在制品容器)020
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BindContainer")]
        [LogDescription("托盘电芯解绑(在制品容器)020", BusinessType.OTHER, "UnBindContainer020", ReceiverTypeEnum.MES)]
        public async Task UnBindContainerAsync(UnBindContainerDto dto)
        {

        }

        /// <summary>
        /// 请求产出极卷码021
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateSfc")]
        [LogDescription("请求产出极卷码021", BusinessType.OTHER, "GenerateSfc021", ReceiverTypeEnum.MES)]
        public async Task<List<string>> GenerateSfcAsync(GenerateSfcDto dto)
        {
            List<string> sfcList = new List<string>();

            return sfcList;
        }

        /// <summary>
        /// 产出米数上报022
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundMetersReport")]
        [LogDescription("产出米数上报022", BusinessType.OTHER, "OutboundMetersReport022", ReceiverTypeEnum.MES)]
        public async Task OutboundMetersReportAsync(OutboundMetersReportDto dto)
        {
            
        }

        /// <summary>
        /// CCD文件上传完成023"
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CcdFileUploadComplete")]
        [LogDescription("CCD文件上传完成023", BusinessType.OTHER, "CCDFileUploadComplete023", ReceiverTypeEnum.MES)]
        public async Task CcdFileUploadCompleteAsync(CCDFileUploadCompleteDto dto)
        {

        }

        /// <summary>
        /// 获取下发条码(用于CCD面密度)024
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CcdGetBarcode")]
        [LogDescription("获取下发条码(用于CCD面密度)024", BusinessType.OTHER, "CcdGetBarcode024", ReceiverTypeEnum.MES)]
        public async Task<CcdGetBarcodeReturnDto> CcdGetBarcodeAsync(CCDFileUploadCompleteDto dto)
        {
            CcdGetBarcodeReturnDto model = new CcdGetBarcodeReturnDto();

            return model;
        }

        /// <summary>
        /// 上料完成(制胶匀浆)025
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FeedingCompleted")]
        [LogDescription("上料完成(制胶匀浆)025", BusinessType.OTHER, "FeedingCompleted025", ReceiverTypeEnum.MES)]
        public async Task FeedingCompletedAsync(FeedingCompletedDto dto)
        {

        }

        /// <summary>
        /// 设备投料前校验(制胶匀浆)026
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConsumeEquBeforeCheck")]
        [LogDescription("设备投料前校验(制胶匀浆)026", BusinessType.OTHER, "ConsumeEquBeforeCheck026", ReceiverTypeEnum.MES)]
        public async Task ConsumeEquBeforeCheckAsync(ConsumeEquBeforeCheckDto dto)
        {

        }

        /// <summary>
        /// 设备投料(制胶匀浆)027
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConsumeEqu")]
        [LogDescription("设备投料(制胶匀浆)027", BusinessType.OTHER, "ConsumeEqu027", ReceiverTypeEnum.MES)]
        public async Task ConsumeEquAsync(ConsumeEquDto dto)
        {

        }

        /// <summary>
        /// 设备产出(制胶匀浆)028
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutputEqu")]
        [LogDescription("设备产出(制胶匀浆)028", BusinessType.OTHER, "OutputEqu028", ReceiverTypeEnum.MES)]
        public async Task<string> OutputEquAsync(QknyBaseDto dto)
        {
            string sfc = "SFC001";

            return sfc;
        }

        /// <summary>
        /// 批次转移(制胶匀浆)029
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BatchMove")]
        [LogDescription("批次转移(制胶匀浆)029", BusinessType.OTHER, "BatchMove029", ReceiverTypeEnum.MES)]
        public async Task BatchMoveAsync(BatchMoveDto dto)
        {
            
        }

        /// <summary>
        /// 设备投料非生产投料(制胶匀浆)030
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConsumeInNonProductionEqu")]
        [LogDescription("设备投料非生产投料(制胶匀浆)030", BusinessType.OTHER, "ConsumeInNonProductionEqu030", ReceiverTypeEnum.MES)]
        public async Task ConsumeInNonProductionEquAsync(ConsumeInNonProductionEquDto dto)
        {

        }

        /// <summary>
        /// 获取配方列表(制胶匀浆)031
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FormulaListGet")]
        [LogDescription("获取配方列表(制胶匀浆)031", BusinessType.OTHER, "FormulaListGet031", ReceiverTypeEnum.MES)]
        public async Task<List<FormulaListGetReturnDto>> FormulaListGetAsync(FormulaListGetDto dto)
        {
            List<FormulaListGetReturnDto> list = new List<FormulaListGetReturnDto>();

            return list;
        }

        /// <summary>
        /// 获取配方参数明细(制胶匀浆)032
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FormulaDetailGet")]
        [LogDescription("获取配方参数明细(制胶匀浆)032", BusinessType.OTHER, "FormulaDetailGet032", ReceiverTypeEnum.MES)]
        public async Task<List<FormulaDetailGetReturnDto>> FormulaDetailGetAsync(FormulaDetailGetDto dto)
        {
            List<FormulaDetailGetReturnDto> list = new List<FormulaDetailGetReturnDto>();

            return list;
        }

        /// <summary>
        /// 配方版本校验(制胶匀浆)033
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FormulaVersionExamine")]
        [LogDescription("配方版本校验(制胶匀浆)033", BusinessType.OTHER, "FormulaVersionExamine033", ReceiverTypeEnum.MES)]
        public async Task FormulaVersionExamineAsync(FormulaVersionExamineDto dto)
        {
        }

        /// <summary>
        /// 工装寿命上报034
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ToolLife")]
        [LogDescription("工装寿命上报034", BusinessType.OTHER, "ToolLife034", ReceiverTypeEnum.MES)]
        public async Task ToolLifeAsync(ToolLifeDto dto)
        {
        }

        /// <summary>
        /// 空托盘校验035
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EmptyContainerCheck")]
        [LogDescription("空托盘校验035", BusinessType.OTHER, "EmptyContainerCheck035", ReceiverTypeEnum.MES)]
        public async Task EmptyContainerCheckAsync(EmptyContainerCheckDto dto)
        {
        }

        /// <summary>
        /// 单电芯校验036
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ContainerSfcCheck")]
        [LogDescription("单电芯校验036", BusinessType.OTHER, "ContainerSfcCheck036", ReceiverTypeEnum.MES)]
        public async Task ContainerSfcCheckAsync(ContainerSfcCheckDto dto)
        {
        }

        /// <summary>
        /// 037托盘NG电芯上报037
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ContainerNgReport")]
        [LogDescription("037托盘NG电芯上报037", BusinessType.OTHER, "ContainerNgReport037", ReceiverTypeEnum.MES)]
        public async Task ContainerNgReportAsync(ContainerNgReportDto dto)
        {
        }

        /// <summary>
        /// 托盘进站(容器进站)038
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InboundInContainer")]
        [LogDescription("托盘进站(容器进站)038", BusinessType.OTHER, "InboundInContainer038", ReceiverTypeEnum.MES)]
        public async Task InboundInContainerAsync(InboundInContainerDto dto)
        {
        }

        /// <summary>
        /// 托盘出站(容器进站)039
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundInContainer")]
        [LogDescription("托盘出站(容器出站)039", BusinessType.OTHER, "OutboundInContainer039", ReceiverTypeEnum.MES)]
        public async Task OutboundInContainerAsync(OutboundInContainerDto dto)
        {
        }

        /// <summary>
        /// 多极组产品出站040
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundMultPolar")]
        [LogDescription("多极组产品出站040", BusinessType.OTHER, "OutboundMultPolar040", ReceiverTypeEnum.MES)]
        public async Task OutboundMultPolarAsync(OutboundMultPolarDto dto)
        {
        }

        /// <summary>
        /// 电芯极组绑定产品出站041
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundSfcPolar")]
        [LogDescription("电芯极组绑定产品出站041", BusinessType.OTHER, "OutboundSfcPolar041", ReceiverTypeEnum.MES)]
        public async Task OutboundSfcPolarAsync(OutboundSfcPolarDto dto)
        {
        }

        /// <summary>
        /// 电芯码下发042
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateCellSfc")]
        [LogDescription("电芯码下发042", BusinessType.OTHER, "GenerateCellSfc042", ReceiverTypeEnum.MES)]
        public async Task<string> GenerateCellSfcAsync(GenerateCellSfcDto dto)
        {
            string sfc = "SFC001";

            return sfc;
        }

    }
}
