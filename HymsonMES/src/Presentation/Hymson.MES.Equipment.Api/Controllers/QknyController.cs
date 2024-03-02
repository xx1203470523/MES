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
            //List<string> list = null;
            //int count = list.Count;

            //throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            //TODO 业务逻辑
            //1. 校验用户名密码是否和设备匹配(equ_equipment_verify)
            //2. 新增equ_equipment_login_record表，记录用户登录时间，统计每个用户的使用时间

        }

        /// <summary>
        /// 设备心跳002
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Heartbeat")]
        [LogDescription("设备心跳002", BusinessType.OTHER, "Heartbeat002", ReceiverTypeEnum.MES)]
        public async Task HeartbeatAsync(HeartbeatDto dto)
        {
            //TODO 业务逻辑
            //1. 新增equ_equipment_newest_info记录设备最后心跳时间
        }

        /// <summary>
        /// 设备状态上报003
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("State")]
        [LogDescription("设备状态上报003", BusinessType.OTHER, "State003", ReceiverTypeEnum.MES)]
        public async Task StateAsync(StateDto dto)
        {
            //TODO 业务逻辑
            //1. 新增equ_equipment_newest_info记录设备最新状态和最后时间
            //2. 新增equ_equipment_status_time记录每个状态持续的时间
        }

        /// <summary>
        /// 设备运行报警信息004
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Alarm")]
        [LogDescription("设备运行报警信息004", BusinessType.OTHER, "Alarm004", ReceiverTypeEnum.MES)]
        public async Task AlarmAsync(AlarmDto dto)
        {
            //TODO 业务逻辑
            //1. 新增equ_equipment_alarm记录故障时间和恢复时间，用于统计每台设备故障具体时间和故障代码
        }

        /// <summary>
        /// 时间同步005
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("TimeSynch")]
        [LogDescription("时间同步005", BusinessType.OTHER, "TimeSynchMes005", ReceiverTypeEnum.MES)]
        public async Task<string> TimeSynchAsync(QknyBaseDto dto)
        {
            DateTime date = HymsonClock.Now();

            return date.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// CCD文件上传完成006
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CcdFileUploadComplete")]
        [LogDescription("CCD文件上传完成006", BusinessType.OTHER, "CCDFileUploadComplete006", ReceiverTypeEnum.MES)]
        public async Task CcdFileUploadCompleteAsync(CCDFileUploadCompleteDto dto)
        {

        }

        /// <summary>
        /// 获取开机参数列表007
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRecipeList")]
        [LogDescription("获取开机参数列表007", BusinessType.OTHER, "GetRecipeList007", ReceiverTypeEnum.MES)]
        public async Task<GetRecipeListReturnDto> GetRecipeListAsync(GetRecipeListDto dto)
        {
            GetRecipeListReturnDto result = new GetRecipeListReturnDto();

            return result;
        }

        /// <summary>
        /// 获取开机参数明细008
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRecipeDetail")]
        [LogDescription("获取开机参数明细008", BusinessType.OTHER, "GetRecipeDetail008", ReceiverTypeEnum.MES)]
        public async Task<GetRecipeDetailReturnDto> GetRecipeDetailAsync(GetRecipeDetailDto dto)
        {
            GetRecipeDetailReturnDto result = new GetRecipeDetailReturnDto();

            List<RecipeParamDto> paramList = new List<RecipeParamDto>();
            for (var i = 0; i < 3; ++i)
            {
                RecipeParamDto param = new RecipeParamDto();
                param.ParamCode = $"param{i}";
                param.ParamValue = "";
                param.ParamLower = "0";
                param.ParamUpper = "1000";
                result.ParamList.Add(param);
            }

            return result;
        }

        /// <summary>
        /// 开机参数校验采集009
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Recipe")]
        [LogDescription("开机参数校验采集009", BusinessType.OTHER, "Recipe009", ReceiverTypeEnum.MES)]
        public async Task RecipeAsync(RecipeDto dto)
        {

        }

        /// <summary>
        /// 原材料上料010
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Feeding")]
        [LogDescription("原材料上料010", BusinessType.OTHER, "Feeding010", ReceiverTypeEnum.MES)]
        public async Task FeedingAsync(FeedingDto dto)
        {

        }

        /// <summary>
        /// 半成品上料011
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HalfFeeding")]
        [LogDescription("半成品上料011", BusinessType.OTHER, "HalfFeeding011", ReceiverTypeEnum.MES)]
        public async Task HalfFeedingAsync(HalfFeedingDto dto)
        {

        }

        /// <summary>
        /// 上料呼叫Agv012
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AgvUpMaterial")]
        [LogDescription("上料呼叫Agv012", BusinessType.OTHER, "AgvUpMaterial012", ReceiverTypeEnum.MES)]
        public async Task AgvUpMaterialAsync(AgvUpMaterialDto dto)
        {

        }

        /// <summary>
        /// 下料呼叫Agv013
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AgvDownMaterial")]
        [LogDescription("下料呼叫Agv013", BusinessType.OTHER, "AgvDownMaterial013", ReceiverTypeEnum.MES)]
        public async Task AgvDownMaterialAsync(AgvUpMaterialDto dto)
        {

        }

        /// <summary>
        /// 获取配方列表(制胶匀浆)014
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FormulaListGet")]
        [LogDescription("获取配方列表(制胶匀浆)014", BusinessType.OTHER, "FormulaListGet014", ReceiverTypeEnum.MES)]
        public async Task<List<FormulaListGetReturnDto>> FormulaListGetAsync(FormulaListGetDto dto)
        {
            List<FormulaListGetReturnDto> list = new List<FormulaListGetReturnDto>();
            for (var i = 0; i < 3; ++i)
            {
                FormulaListGetReturnDto model = new FormulaListGetReturnDto();
                model.FormulaCode = $"formulaCode{i + 1}";
                model.Version = "1.0";
                model.ProductCode = $"productCode{i}";
                model.LastUpdateOnTime = DateTime.Now;

                list.Add(model);
            }

            return list;
        }

        /// <summary>
        /// 获取配方参数明细(制胶匀浆)015
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FormulaDetailGet")]
        [LogDescription("获取配方参数明细(制胶匀浆)015", BusinessType.OTHER, "FormulaDetailGet015", ReceiverTypeEnum.MES)]
        public async Task<FormulaDetailGetReturnDto> FormulaDetailGetAsync(FormulaDetailGetDto dto)
        {
            FormulaDetailGetReturnDto result = new FormulaDetailGetReturnDto();
            result.Version = "1.0";

            for (var i = 0; i < 5; ++i)
            {
                FormulaParamList model = new FormulaParamList();
                model.SepOrder = i + 1;
                model.Category = "A|B|C";
                model.MarterialCode = $"materialCode{i}";
                model.MarerialGroupCode = $"MarerialGroupCode{i}";
                model.ParameCode = $"ParameCode{i}";
                model.ParamValue = $"ParamValue{i}";
                model.FunctionCode = $"FunctionCode{i}";
                model.Unit = $"Unit{i}";

                result.ParamList.Add(model);
            }

            return result;
        }

        /// <summary>
        /// 配方版本校验(制胶匀浆)016
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FormulaVersionExamine")]
        [LogDescription("配方版本校验(制胶匀浆)016", BusinessType.OTHER, "FormulaVersionExamine016", ReceiverTypeEnum.MES)]
        public async Task FormulaVersionExamineAsync(FormulaVersionExamineDto dto)
        {
        }

        /// <summary>
        /// 设备投料前校验(制胶匀浆)017
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConsumeEquBeforeCheck")]
        [LogDescription("设备投料前校验(制胶匀浆)017", BusinessType.OTHER, "ConsumeEquBeforeCheck017", ReceiverTypeEnum.MES)]
        public async Task ConsumeEquBeforeCheckAsync(ConsumeEquBeforeCheckDto dto)
        {

        }

        /// <summary>
        /// 设备投料(制胶匀浆)018
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConsumeEqu")]
        [LogDescription("设备投料(制胶匀浆)018", BusinessType.OTHER, "ConsumeEqu018", ReceiverTypeEnum.MES)]
        public async Task ConsumeEquAsync(ConsumeEquDto dto)
        {

        }

        /// <summary>
        /// 上料完成(制胶匀浆)019
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FeedingCompleted")]
        [LogDescription("上料完成(制胶匀浆)019", BusinessType.OTHER, "FeedingCompleted019", ReceiverTypeEnum.MES)]
        public async Task FeedingCompletedAsync(FeedingCompletedDto dto)
        {

        }

        /// <summary>
        /// 设备产出(制胶匀浆)020
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutputEqu")]
        [LogDescription("设备产出(制胶匀浆)020", BusinessType.OTHER, "OutputEqu020", ReceiverTypeEnum.MES)]
        public async Task<string> OutputEquAsync(QknyBaseDto dto)
        {
            string sfc = "SFC001";

            return sfc;
        }

        /// <summary>
        /// 批次转移(制胶匀浆)021
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BatchMove")]
        [LogDescription("批次转移(制胶匀浆)021", BusinessType.OTHER, "BatchMove021", ReceiverTypeEnum.MES)]
        public async Task BatchMoveAsync(BatchMoveDto dto)
        {

        }

        /// <summary>
        /// 设备投料非生产投料(制胶匀浆)022
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConsumeInNonProductionEqu")]
        [LogDescription("设备投料非生产投料(制胶匀浆)022", BusinessType.OTHER, "ConsumeInNonProductionEqu022", ReceiverTypeEnum.MES)]
        public async Task ConsumeInNonProductionEquAsync(ConsumeInNonProductionEquDto dto)
        {

        }

        /// <summary>
        /// 请求产出极卷码023
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateSfc")]
        [LogDescription("请求产出极卷码023", BusinessType.OTHER, "GenerateSfc023", ReceiverTypeEnum.MES)]
        public async Task<List<string>> GenerateSfcAsync(GenerateSfcDto dto)
        {
            List<string> sfcList = new List<string>();
            for (var i = 0; i < dto.Qty + 1; ++i)
            {
                sfcList.Add($"sfc00{i + 1}");
            }

            return sfcList;
        }

        /// <summary>
        /// 产出米数上报024
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundMetersReport")]
        [LogDescription("产出米数上报024", BusinessType.OTHER, "OutboundMetersReport024", ReceiverTypeEnum.MES)]
        public async Task OutboundMetersReportAsync(OutboundMetersReportDto dto)
        {

        }

        /// <summary>
        /// 获取下发条码(用于CCD面密度)025
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CcdGetBarcode")]
        [LogDescription("获取下发条码(用于CCD面密度)025", BusinessType.OTHER, "CcdGetBarcode025", ReceiverTypeEnum.MES)]
        public async Task<CcdGetBarcodeReturnDto> CcdGetBarcodeAsync(CCDFileUploadCompleteDto dto)
        {
            CcdGetBarcodeReturnDto model = new CcdGetBarcodeReturnDto();

            return model;
        }

        /// <summary>
        /// 设备过程参数026
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EquipmentProcessParam")]
        [LogDescription("设备过程参数026", BusinessType.OTHER, "EquipmentProcessParam026", ReceiverTypeEnum.MES)]
        public async Task EquipmentProcessParamAsync(EquipmentProcessParamDto dto)
        {

        }

        /// <summary>
        /// 产品进站027
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Inbound")]
        [LogDescription("产品进站027", BusinessType.OTHER, "Inbound027", ReceiverTypeEnum.MES)]
        public async Task InboundAsync(InboundDto dto)
        {

        }

        /// <summary>
        /// 产品出站028
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Outbound")]
        [LogDescription("产品进站028", BusinessType.OTHER, "Outbound028", ReceiverTypeEnum.MES)]
        public async Task OutboundAsync(OutboundDto dto)
        {

        }

        /// <summary>
        /// 进站多个029
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InboundMore")]
        [LogDescription("进站多个029", BusinessType.OTHER, "InboundMore029", ReceiverTypeEnum.MES)]
        public async Task<List<InboundMoreReturnDto>> InboundMoreAsync(InboundMoreDto dto)
        {
            List<InboundMoreReturnDto> result = new List<InboundMoreReturnDto>();
            for(var i = 0;i < dto.SfcList.Count; ++i)
            {
                InboundMoreReturnDto model = new InboundMoreReturnDto();
                model.Code = 0;
                model.Msg = "11";
                model.Sfc = $"sfc00{i+1}";

                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// 出站多个030
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundMore")]
        [LogDescription("出站多个030", BusinessType.OTHER, "OutboundMore030", ReceiverTypeEnum.MES)]
        public async Task<List<OutboundMoreReturnDto>> OutboundMoreAsync(OutboundMoreDto dto)
        {
            List<OutboundMoreReturnDto> result = new List<OutboundMoreReturnDto>();
            for (var i = 0; i < dto.SfcList.Count; ++i)
            {
                OutboundMoreReturnDto model = new OutboundMoreReturnDto();
                model.Code = 0;
                model.Msg = "11";
                model.Sfc = $"sfc00{i + 1}";

                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// 多极组产品出站031
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundMultPolar")]
        [LogDescription("多极组产品出站031", BusinessType.OTHER, "OutboundMultPolar031", ReceiverTypeEnum.MES)]
        public async Task<List<OutboundMoreReturnDto>> OutboundMultPolarAsync(OutboundMultPolarDto dto)
        {
            List<OutboundMoreReturnDto> result = new List<OutboundMoreReturnDto>();
            for (var i = 0; i < dto.SfcList.Count; ++i)
            {
                OutboundMoreReturnDto model = new OutboundMoreReturnDto();
                model.Code = 0;
                model.Msg = "11";
                model.Sfc = $"sfc00{i + 1}";

                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// 电芯极组绑定产品出站032
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundSfcPolar")]
        [LogDescription("电芯极组绑定产品出站032", BusinessType.OTHER, "OutboundSfcPolar032", ReceiverTypeEnum.MES)]
        public async Task OutboundSfcPolarAsync(OutboundSfcPolarDto dto)
        {
        }

        /// <summary>
        /// 电芯码下发033
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateCellSfc")]
        [LogDescription("电芯码下发033", BusinessType.OTHER, "GenerateCellSfc033", ReceiverTypeEnum.MES)]
        public async Task<string> GenerateCellSfcAsync(GenerateCellSfcDto dto)
        {
            string sfc = "SFC001";

            return sfc;
        }

        /// <summary>
        /// 补液数据上报034
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FillingData")]
        [LogDescription("补液数据上报034", BusinessType.OTHER, "FillingData034", ReceiverTypeEnum.MES)]
        public async Task FillingDataAsync(FillingDataDto dto)
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
        /// 托盘电芯绑定(在制品容器)037
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BindContainer")]
        [LogDescription("托盘电芯绑定(在制品容器)037", BusinessType.OTHER, "BindContainer037", ReceiverTypeEnum.MES)]
        public async Task BindContainerAsync(BindContainerDto dto)
        {

        }

        /// <summary>
        /// 托盘电芯解绑(在制品容器)038
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnBindContainer")]
        [LogDescription("托盘电芯解绑(在制品容器)038", BusinessType.OTHER, "UnBindContainer038", ReceiverTypeEnum.MES)]
        public async Task UnBindContainerAsync(UnBindContainerDto dto)
        {

        }

        /// <summary>
        /// 托盘NG电芯上报039
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ContainerNgReport")]
        [LogDescription("托盘NG电芯上报039", BusinessType.OTHER, "ContainerNgReport039", ReceiverTypeEnum.MES)]
        public async Task ContainerNgReportAsync(ContainerNgReportDto dto)
        {
        }

        /// <summary>
        /// 托盘进站(容器进站)040
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InboundInContainer")]
        [LogDescription("托盘进站(容器进站)040", BusinessType.OTHER, "InboundInContainer040", ReceiverTypeEnum.MES)]
        public async Task InboundInContainerAsync(InboundInContainerDto dto)
        {
        }

        /// <summary>
        /// 托盘出站(容器进站)041
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundInContainer")]
        [LogDescription("托盘出站(容器出站)041", BusinessType.OTHER, "OutboundInContainer041", ReceiverTypeEnum.MES)]
        public async Task OutboundInContainerAsync(OutboundInContainerDto dto)
        {
            //TODO
            //1. 托盘如果存在参数，在记录数据时，需要在额外记录托盘当时的条码
        }

        /// <summary>
        /// 工装寿命上报042
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ToolLife")]
        [LogDescription("工装寿命上报042", BusinessType.OTHER, "ToolLife042", ReceiverTypeEnum.MES)]
        public async Task ToolLifeAsync(ToolLifeDto dto)
        {
        }

        /// <summary>
        /// 产品参数上传043
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ProductParam")]
        [LogDescription("产品参数上传043", BusinessType.OTHER, "ProductParam043", ReceiverTypeEnum.MES)]
        public async Task ProductParamAsync(ProductParamDto dto)
        {
        }














    }
}
