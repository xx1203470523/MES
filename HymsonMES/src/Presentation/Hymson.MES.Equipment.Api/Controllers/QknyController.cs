using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.CoreServices.Dtos.Manufacture.ManuBind;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.EquipmentServices;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Dtos.Qkny.ProcSortingRule;
using Hymson.MES.EquipmentServices.Dtos.Qkny.ToolBindMaterial;
using Hymson.MES.EquipmentServices.Services.Qkny;
using Hymson.MES.EquipmentServices.Services.Qkny.Common;
using Hymson.MES.EquipmentServices.Services.Qkny.FitTogether;
using Hymson.MES.EquipmentServices.Services.Qkny.Formation;
using Hymson.MES.EquipmentServices.Services.Qkny.GlueHomogenate;
using Hymson.Utils;
using Hymson.Web.Framework.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Hymson.MES.Equipment.Api.Controllers
{
    /// <summary>
    /// 顷刻能源设备接口控制器
    /// </summary>
    [ApiController]
    [Route("EquipmentService/api/v1")]
    public class QknyController : ControllerBase
    {
        /// <summary>
        /// 设备接口服务
        /// </summary>
        private readonly IQknyService _qknyService;

        /// <summary>
        /// 通用服务
        /// </summary>
        private readonly IEquCommonService _equCommonService;

        /// <summary>
        /// 制胶匀浆
        /// </summary>
        private readonly IGlueHomogenateService _glueHomogenateService;

        /// <summary>
        /// 装配
        /// </summary>
        private readonly IFitTogetherService _fitTogether;

        /// <summary>
        /// 化成
        /// </summary>
        private readonly IFormationService _formationService;

        /// <summary>
        /// 是否调试
        /// </summary>
        private readonly bool IS_DEBUG = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QknyController(
            IQknyService qknyService,
            IEquCommonService equCommonService,
            IGlueHomogenateService glueHomogenateService,
            IFitTogetherService fitTogether,
            IFormationService formationService)
        {
            _qknyService = qknyService;
            _equCommonService = equCommonService;
            _glueHomogenateService = glueHomogenateService;
            _fitTogether = fitTogether;
            _formationService = formationService;
        }

        /// <summary>
        /// 操作员登录001
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OperatorLogin")]
        [LogDescription("操作员登录001", BusinessType.OTHER, "001", ReceiverTypeEnum.MES)]
        [AllowAnonymous]
        public async Task<OperationLoginReturnDto> OperatorLoginAsync(OperationLoginDto dto)
        {
            if (IS_DEBUG == true)
            {
                OperationLoginReturnDto result = new OperationLoginReturnDto();
                result.AccountType = "1";
                return result;
            }

            return await _equCommonService.OperatorLoginAsync(dto);
            //await _qknyService.OperatorLoginAsync(dto);
        }

        /// <summary>
        /// 设备心跳002
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Heartbeat")]
        [LogDescription("设备心跳002", BusinessType.OTHER, "002", ReceiverTypeEnum.MES)]
        public async Task HeartbeatAsync(HeartbeatDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _equCommonService.HeartbeatAsync(dto);

            //TODO 业务逻辑
            //1. 新增equ_equipment_newest_info记录设备最后心跳时间
            //2. 记录心跳记录
            //await _qknyService.HeartbeatAsync(dto);
        }

        /// <summary>
        /// 设备状态上报003
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("State")]
        [LogDescription("设备状态上报003", BusinessType.OTHER, "003", ReceiverTypeEnum.MES)]
        public async Task StateAsync(StateDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _equCommonService.StateAsync(dto);
            //TODO 业务逻辑
            //1. 新增equ_equipment_newest_info记录设备最新状态和最后时间
            //2. 新增 equ_equipment_status_time 记录每个状态持续的时间
        }

        /// <summary>
        /// 设备运行报警信息004
        /// 报警不一定停机，状态不一定会发生切换
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Alarm")]
        [LogDescription("设备运行报警信息004", BusinessType.OTHER, "004", ReceiverTypeEnum.MES)]
        public async Task AlarmAsync(AlarmDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _equCommonService.AlarmAsync(dto);
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
        [LogDescription("时间同步005", BusinessType.OTHER, "005", ReceiverTypeEnum.MES)]
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
        [LogDescription("CCD文件上传完成006", BusinessType.OTHER, "006", ReceiverTypeEnum.MES)]
        public async Task CcdFileUploadCompleteAsync(CCDFileUploadCompleteDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _equCommonService.CcdFileUploadCompleteAsync(dto);
            //TODO
            //1. 新增表 ccd_file_upload_complete_record，用于记录每个条码对应的CCD文件路径及是否合格
            //  明细和主表记录到一起
        }

        /// <summary>
        /// 获取开机参数列表007
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRecipeList")]
        [LogDescription("获取开机参数列表007", BusinessType.OTHER, "007", ReceiverTypeEnum.MES)]
        public async Task<List<GetRecipeListReturnDto>> GetRecipeListAsync(GetRecipeListDto dto)
        {
            if (IS_DEBUG == true)
            {
                List<GetRecipeListReturnDto> resultList = new List<GetRecipeListReturnDto>();
                for (int i = 0; i < 5; ++i)
                {
                    GetRecipeListReturnDto model = new GetRecipeListReturnDto();
                    model.RecipeCode = $"recipe{i}";
                    model.ProductCode = $"product{i}";
                    model.Version = i.ToString();
                    model.LastUpdateOnTime = DateTime.Now;
                    resultList.Add(model);
                }
                return resultList;
            }

            var result = await _equCommonService.GetRecipeListAsync(dto);
            return result;
            //TODO
            //1. 获取 proc_equipment_group_param 表中type=1的数据，并转换成相应数据格式
            //2. 对应系统 Recipe参数 功能
            //3. 校验是否已经维护基础数据（）
        }

        /// <summary>
        /// 获取开机参数明细008
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRecipeDetail")]
        [LogDescription("获取开机参数明细008", BusinessType.OTHER, "008", ReceiverTypeEnum.MES)]
        public async Task<GetRecipeDetailReturnDto> GetRecipeDetailAsync(GetRecipeDetailDto dto)
        {
            //TODO
            //1. 获取proc_equipment_group_param_detail开机参数明细，并转成相应格式

            if (IS_DEBUG == true)
            {
                GetRecipeDetailReturnDto resultList = new GetRecipeDetailReturnDto();
                resultList.Version = "1.0";
                resultList.LastUpdateOnTime = HymsonClock.Now();
                List<RecipeParamDto> paramList = new List<RecipeParamDto>();
                for (var i = 0; i < 3; ++i)
                {
                    RecipeParamDto param = new RecipeParamDto();
                    param.ParamCode = $"param{i}";
                    param.ParamValue = "";
                    param.ParamLower = "0";
                    param.ParamUpper = "1000";
                    resultList.ParamList.Add(param);
                }
                return resultList;
            }

            var res = await _equCommonService.GetRecipeDetailAsync(dto);
            return res;
        }

        /// <summary>
        /// 开机参数校验采集009
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Recipe")]
        [LogDescription("开机参数校验采集009", BusinessType.OTHER, "009", ReceiverTypeEnum.MES)]
        public async Task RecipeAsync(RecipeDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _equCommonService.RecipeAsync(dto);

            //TODO
            //1. 校验开机参数是否启用状态
            //2. 新增proc_recipe_record记录表，用于记录开机参数中设定的实际值

            //新增开机参数记录表
        }

        /// <summary>
        /// 原材料上料010
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Feeding")]
        [LogDescription("原材料上料010", BusinessType.OTHER, "010", ReceiverTypeEnum.MES)]
        public async Task FeedingAsync(FeedingDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }
            await _qknyService.FeedingAsync(dto);
            //TODO
            //-- 不校验物料是在 wh_material_inventory 物料库存表中
            //1. 校验物料是否在lims系统发过来的条码表lims_material(wh_material_inventory)，验证是否存在及合格，以及生成日期
            //2. 添加上料表信息 manu_feeding
            //3. 添加上料记录表信息 manu_feeding_record
            //
        }

        /// <summary>
        /// 半成品上料011
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("HalfFeeding")]
        [LogDescription("半成品上料011", BusinessType.OTHER, "011", ReceiverTypeEnum.MES)]
        public async Task HalfFeedingAsync(HalfFeedingDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }
            await _qknyService.HalfFeedingAsync(dto);
        }

        /// <summary>
        /// 上料呼叫Agv012
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AgvUpMaterial")]
        [LogDescription("上料呼叫Agv012", BusinessType.OTHER, "012", ReceiverTypeEnum.MES)]
        public async Task AgvUpMaterialAsync(AgvUpMaterialDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            AgvMaterialDto agvDto = new AgvMaterialDto();
            agvDto.EquipmentCode = dto.EquipmentCode;
            agvDto.ResourceCode = dto.ResourceCode;
            agvDto.LocalTime = dto.LocalTime;
            if(string.IsNullOrEmpty(dto.TaskType) == true)
            {
                agvDto.TaskType = "1";
            }
            agvDto.Content = string.Empty;
            await _qknyService.AgvMaterialAsync(agvDto);

            //TODO
            //1. 针对涂布，辊分，模切，卷绕设备进行上料时，通过MES呼叫AGV，给AGV发一个任务
            //2. 调用AGV接口，添加 agv_task_record 记录表进行记录，表中有字段区分上料还是下料
        }

        /// <summary>
        /// 下料呼叫Agv013
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AgvDownMaterial")]
        [LogDescription("下料呼叫Agv013", BusinessType.OTHER, "013", ReceiverTypeEnum.MES)]
        public async Task AgvDownMaterialAsync(AgvUpMaterialDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            AgvMaterialDto agvDto = new AgvMaterialDto();
            agvDto.EquipmentCode = dto.EquipmentCode;
            agvDto.ResourceCode = dto.ResourceCode;
            agvDto.LocalTime = dto.LocalTime;
            if(string.IsNullOrEmpty(dto.TaskType) == true)
            {
                agvDto.TaskType = "2";
            }
            agvDto.Content = string.Empty;
            await _qknyService.AgvMaterialAsync(agvDto);

            //TODO
            //1. 针对涂布，辊分，模切，卷绕设备进行下料时，通过MES呼叫AGV，给AGV发一个任务
            //2. 调用AGV接口，添加agv_task_record记录表进行记录，表中有字段区分上料还是下料
        }

        /// <summary>
        /// 获取配方列表(制胶匀浆)014
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FormulaListGet")]
        [LogDescription("获取配方列表(制胶匀浆)014", BusinessType.OTHER, "014", ReceiverTypeEnum.MES)]
        public async Task<List<FormulaListGetReturnDto>> FormulaListGetAsync(FormulaListGetDto dto)
        {
            //TODO
            //1. 针对制胶匀浆设备时，在开机进行启动时，从MES获取配方列表
            //2. 获取 proc_formula 表数据，并进行字段转换

            if (IS_DEBUG == true)
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
            }

            return await _glueHomogenateService.FormulaListGetAsync(dto);
        }

        /// <summary>
        /// 获取配方参数明细(制胶匀浆)015
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FormulaDetailGet")]
        [LogDescription("获取配方参数明细(制胶匀浆)015", BusinessType.OTHER, "015", ReceiverTypeEnum.MES)]
        public async Task<FormulaDetailGetReturnDto> FormulaDetailGetAsync(FormulaDetailGetDto dto)
        {
            //TODO
            //1. 基于proc_formula，proc_formula_details表进行查询

            if (IS_DEBUG == true)
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

            return await _glueHomogenateService.FormulaDetailGetAsync(dto);
        }

        /// <summary>
        /// 配方版本校验(制胶匀浆)016
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FormulaVersionExamine")]
        [LogDescription("配方版本校验(制胶匀浆)016", BusinessType.OTHER, "016", ReceiverTypeEnum.MES)]
        public async Task FormulaVersionExamineAsync(FormulaVersionExamineDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _glueHomogenateService.FormulaVersionExamineAsync(dto);

            //TODO
            //1. 查询表proc_formula进行配方版本的校验，确认是否是激活的版本
        }

        /// <summary>
        /// 设备投料前校验(制胶匀浆)017
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConsumeEquBeforeCheck")]
        [LogDescription("设备投料前校验(制胶匀浆)017", BusinessType.OTHER, "017", ReceiverTypeEnum.MES)]
        public async Task ConsumeEquBeforeCheckAsync(ConsumeEquBeforeCheckDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _glueHomogenateService.ConsumeEquBeforeCheckAsync(dto);

            //TODO
            //待确认？此时应该应该根据什么是查激活的工单以及对应的BOM
            //1. 校验物料是否在工单BOM里
            //2. 需要查询设备当前所在工序
        }

        /// <summary>
        /// 设备投料(制胶匀浆)018 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConsumeEqu")]
        [LogDescription("设备投料(制胶匀浆)018", BusinessType.OTHER, "018", ReceiverTypeEnum.MES)]
        public async Task ConsumeEquAsync(ConsumeEquDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _glueHomogenateService.ConsumeEquAsync(dto);
            //TODO
            //1. 类似上料，上到搅拌机或者制胶机
        }

        /// <summary>
        /// 上料完成(制胶匀浆)019
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FeedingCompleted")]
        [LogDescription("上料完成(制胶匀浆)019", BusinessType.OTHER, "019", ReceiverTypeEnum.MES)]
        public async Task FeedingCompletedAsync(FeedingCompletedDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _glueHomogenateService.FeedingCompletedAsync(dto);

            //TODO
            //1. 类似上料，粉料，匀浆上到中转罐或者料仓（上料点）
            //
        }

        /// <summary>
        /// 设备产出(制胶匀浆)020
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutputEqu")]
        [LogDescription("设备产出(制胶匀浆)020", BusinessType.OTHER, "020", ReceiverTypeEnum.MES)]
        public async Task<string> OutputEquAsync(QknyBaseDto dto)
        {
            if (IS_DEBUG == true)
            {
                string sfc = "SFC001";
                return sfc;
            }

            return await _glueHomogenateService.OutputEquAsync(dto);

            //TODO
            //1. 根据工序返回对应的条码给设备
            //2. 执行条码生成方法
        }

        /// <summary>
        /// 批次转移(制胶匀浆)021
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BatchMove")]
        [LogDescription("批次转移(制胶匀浆)021", BusinessType.OTHER, "021", ReceiverTypeEnum.MES)]
        public async Task BatchMoveAsync(BatchMoveDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _glueHomogenateService.BatchMoveAsync(dto);

            //TODO
            //1. 当浆料或胶液在罐体间转移后，上报浆料或胶液条码、重量、转出罐和转入罐的编码
            //2. 处理罐子前后的数量
            //3. 添加转移记录
        }

        /// <summary>
        /// 设备投料非生产投料(制胶匀浆)022
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConsumeInNonProductionEqu")]
        [LogDescription("设备投料非生产投料(制胶匀浆)022", BusinessType.OTHER, "022", ReceiverTypeEnum.MES)]
        public async Task ConsumeInNonProductionEquAsync(ConsumeInNonProductionEquDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _glueHomogenateService.ConsumeInNonProductionEquAsync(dto);

            //TODO
            //1. 使用NMP和DIW洗罐子用到
        }

        /// <summary>
        /// 请求产出极卷码023
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateSfc")]
        [LogDescription("请求产出极卷码023", BusinessType.OTHER, "023", ReceiverTypeEnum.MES)]
        public async Task<List<string>> GenerateSfcAsync(GenerateSfcDto dto)
        {
            //TODO
            //1. 根据数量下发对应的条码给设备
            //2. 用的时候生成(需要的时候在生成条码)或从XX表里面取(提前生成就是下发，生成两个或者3个，)
            //3. 考虑提前生成条码如何标记是否使用

            if (IS_DEBUG == true)
            {
                List<string> sfcList = new List<string>();
                for (var i = 0; i < dto.Qty + 1; ++i)
                {
                    sfcList.Add($"sfc00{i + 1}");
                }

                return sfcList;
            }
            //如果型号设置的是多个，则一次只出一个
            //如果型号设置的是单个，则一次应该根据指定数量出
            dto.Qty = 1;
            return await _qknyService.GenerateSfcAsync(dto);
        }

        /// <summary>
        /// 产出上报024
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundMetersReport")]
        [LogDescription("产出上报024", BusinessType.OTHER, "024", ReceiverTypeEnum.MES)]
        public async Task OutboundMetersReportAsync(OutboundMetersReportDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.OutboundMetersReportAsync(dto);
        }

        /// <summary>
        /// 获取下发条码(用于CCD面密度)025
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CcdGetBarcode")]
        [LogDescription("获取下发条码(用于CCD面密度)025", BusinessType.OTHER, "025", ReceiverTypeEnum.MES)]
        public async Task<CcdGetBarcodeReturnDto> CcdGetBarcodeAsync(CCDFileUploadCompleteDto dto)
        {
            if (IS_DEBUG == true)
            {
                CcdGetBarcodeReturnDto model = new CcdGetBarcodeReturnDto();
                return model;
            }

            return await _qknyService.CcdGetBarcodeAsync(dto);
        }

        /// <summary>
        /// 设备过程参数026
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EquipmentProcessParam")]
        [LogDescription("设备过程参数026", BusinessType.OTHER, "026", ReceiverTypeEnum.MES)]
        public async Task EquipmentProcessParamAsync(EquipmentProcessParamDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _equCommonService.EquipmentProcessParamAsync(dto);
        }

        /// <summary>
        /// 产品进站027
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Inbound")]
        [LogDescription("产品进站027", BusinessType.OTHER, "027", ReceiverTypeEnum.MES)]
        public async Task InboundAsync(InboundDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.InboundAsync(dto);
        }

        /// <summary>
        /// 产品出站028
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Outbound")]
        [LogDescription("产品出站028", BusinessType.OTHER, "028", ReceiverTypeEnum.MES)]
        public async Task OutboundAsync(OutboundDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.OutboundAsync(dto);

            //TODO
            //1. 添加参数记录
            //2. 参考现有出站
            //3. 产出扣料(新视界)，根据上传物料列表
        }

        /// <summary>
        /// 进站多个029
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InboundMore")]
        [LogDescription("进站多个029", BusinessType.OTHER, "029", ReceiverTypeEnum.MES)]
        public async Task<List<InboundMoreReturnDto>> InboundMoreAsync(InboundMoreDto dto)
        {
            //TODO
            //1. 参考现有进站

            if (IS_DEBUG == true)
            {
                List<InboundMoreReturnDto> result = new List<InboundMoreReturnDto>();
                for (var i = 0; i < dto.SfcList.Count; ++i)
                {
                    InboundMoreReturnDto model = new InboundMoreReturnDto();
                    model.Code = 0;
                    model.Msg = "11";
                    model.Sfc = $"sfc00{i + 1}";

                    result.Add(model);
                }

                return result;
            }

            return await _qknyService.InboundMoreAsync(dto);
        }

        /// <summary>
        /// 出站多个030
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundMore")]
        [LogDescription("出站多个030", BusinessType.OTHER, "030", ReceiverTypeEnum.MES)]
        public async Task<List<OutboundMoreReturnDto>> OutboundMoreAsync(OutboundMoreDto dto)
        {
            //TODO
            //1. 参考现有出站
            if (IS_DEBUG == true)
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

            return await _qknyService.OutboundMoreAsync(dto);
        }

        /// <summary>
        /// 多极组产品出站031
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundMultPolar")]
        [LogDescription("多极组产品出站031", BusinessType.OTHER, "031", ReceiverTypeEnum.MES)]
        public async Task<List<OutboundMoreReturnDto>> OutboundMultPolarAsync(OutboundMultPolarDto dto)
        {
            //TODO
            //1. 极组和极组绑定（新视界极组条码接收）
            //2. 校验极组是否合格
            //3. 校验上工序是否合格
            //4. 考虑系统如何方便追溯

            if (IS_DEBUG == true)
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

            return await _fitTogether.OutboundMultPolarAsync(dto);
        }

        /// <summary>
        /// 电芯极组绑定产品出站032
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundSfcPolar")]
        [LogDescription("电芯极组绑定产品出站032", BusinessType.OTHER, "032", ReceiverTypeEnum.MES)]
        public async Task OutboundSfcPolarAsync(OutboundSfcPolarDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _fitTogether.OutboundSfcPolarAsync(dto);

            //TODO
            //1. 将极组和电芯进行绑定
            //2. 考虑系统如何追溯
        }

        /// <summary>
        /// 电芯码下发033
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateCellSfc")]
        [LogDescription("电芯码下发033", BusinessType.OTHER, "033", ReceiverTypeEnum.MES)]
        public async Task<string> GenerateCellSfcAsync(GenerateDxSfcDto dto)
        {
            //TODO
            //1. 生成条码进行下发
            //2. 参考现有创建条码 CreateBarcodeBySemiProductIdAsync，CreateBarcodeByWorkOrderIdAsync

            if (IS_DEBUG == true)
            {
                //List<string> sfcList = new List<string>();
                //for (var i = 0; i < dto.Qty; ++i)
                //{
                //    sfcList.Add($"SFC00{i + 1}");
                //}

                //string sfc = "SFC001";

                return "sfc001";
            }

            return await _fitTogether.Create24GbCodeAsync(dto);
            //return await _fitTogether.GenerateCellSfcAsync(dto);
        }

        /// <summary>
        /// 补液数据上报034
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("FillingData")]
        [LogDescription("补液数据上报034", BusinessType.OTHER, "034", ReceiverTypeEnum.MES)]
        public async Task FillingDataAsync(FillingDataDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _formationService.FillingDataAsync(dto);
        }

        /// <summary>
        /// 空托盘校验035
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EmptyContainerCheck")]
        [LogDescription("空托盘校验035", BusinessType.OTHER, "035", ReceiverTypeEnum.MES)]
        public async Task EmptyContainerCheckAsync(EmptyContainerCheckDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _formationService.EmptyContainerCheckAsync(dto);
        }

        /// <summary>
        /// 单电芯校验036
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ContainerSfcCheck")]
        [LogDescription("单电芯校验036", BusinessType.OTHER, "036", ReceiverTypeEnum.MES)]
        public async Task ContainerSfcCheckAsync(ContainerSfcCheckDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _formationService.ContainerSfcCheckAsync(dto);

            //TODO
            //1. 校验电芯是否合格
            //2. 校验电芯是否在托盘中 inte_vehicle_freight_stack
            //3. 校验电芯是否在上工序出站
            //4. 新增表inte_vehicle_check_record电芯校验记录表，用于绑定 inte_vehicle_check_record，绑定时不需要再次校验
        }

        /// <summary>
        /// 托盘电芯绑定(在制品容器)037
        /// 绑盘拆盘作为单独工序，需要做进出站
        /// 绑盘完后才会告诉MES，不会出现绑一半告诉MES
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BindContainer")]
        [LogDescription("托盘电芯绑定(在制品容器)037", BusinessType.OTHER, "037", ReceiverTypeEnum.MES)]
        public async Task BindContainerAsync(BindContainerDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }
            //TODO
            //绑盘拆盘作为单独工序，需要做进出站

            await _formationService.BindContainerAsync(dto);

            //TODO
            //1. 校验托盘数量
            //2. 校验电芯是否已经做校验 inte_vehicle_check_record
            //3. 托盘和电芯做绑定，删除电芯校验记录表 inte_vehicle_check_record
            //4. inte_vehicle_freight_stack 添加绑定数据
            //5. inte_vehicle_freight_record 添加绑定记录
        }

        /// <summary>
        /// 托盘电芯解绑(在制品容器)038
        /// 绑盘拆盘作为单独工序，需要做进出站
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UnBindContainer")]
        [LogDescription("托盘电芯解绑(在制品容器)038", BusinessType.OTHER, "038", ReceiverTypeEnum.MES)]
        public async Task UnBindContainerAsync(UnBindContainerDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }
            //TODO
            //绑盘拆盘作为单独工序，需要做进出站

            await _formationService.UnBindContainerAsync(dto);

            //TODO
            //1. 校验电芯是否在托盘中
            //2. inte_vehicle_freight_stack 删除绑定数据
            //3. 添加 inte_vehicle_freight_record 解绑记录
        }

        /// <summary>
        /// 托盘NG电芯上报039
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ContainerNgReport")]
        [LogDescription("托盘NG电芯上报039", BusinessType.OTHER, "039", ReceiverTypeEnum.MES)]
        public async Task ContainerNgReportAsync(ContainerNgReportDto dto)
        {
            //TODO
            //1. 确认设备能否给出不合格代码及发现不良工序
            if (IS_DEBUG == true)
            {
                return;
            }

            await _formationService.ContainerNgReportAsync(dto);
        }

        /// <summary>
        /// 托盘进站(容器进站)040
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InboundInContainer")]
        [LogDescription("托盘进站(容器进站)040", BusinessType.OTHER, "040", ReceiverTypeEnum.MES)]
        public async Task InboundInContainerAsync(InboundInContainerDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _formationService.InboundInContainerAsync(dto);

            //TODO
            //1. 参考 InBoundCarrierAsync 进站
        }

        /// <summary>
        /// 托盘出站(容器出站)041
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundInContainer")]
        [LogDescription("托盘出站(容器出站)041", BusinessType.OTHER, "041", ReceiverTypeEnum.MES)]
        public async Task OutboundInContainerAsync(OutboundInContainerDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _formationService.OutboundInContainerAsync(dto);

            //TODO
            //1. 托盘如果存在参数，在记录数据时，需要在额外记录托盘当时的条码
            //2. 添加托盘出站记录
        }

        /// <summary>
        /// 工装寿命上报042
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ToolLife")]
        [LogDescription("工装寿命上报042", BusinessType.OTHER, "042", ReceiverTypeEnum.MES)]
        public async Task ToolLifeAsync(ToolLifeDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _equCommonService.ToolLifeAsync(dto);

            //TODO
            //1. 添加设备工装寿命记录表，进行数据更新或者插入
        }

        /// <summary>
        /// 产品参数上传043
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ProductParam")]
        [LogDescription("产品参数上传043", BusinessType.OTHER, "043", ReceiverTypeEnum.MES)]
        public async Task ProductParamAsync(ProductParamDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _equCommonService.ProductParamAsync(dto);

            //TODO
            //1. 参考 ProductCollectionAsync
        }

        /// <summary>
        /// 卷绕极组产出044
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CollingPolar")]
        [LogDescription("卷绕极组产出044", BusinessType.OTHER, "044", ReceiverTypeEnum.MES)]
        public async Task CollingPolarAsync(CollingPolarDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _fitTogether.CollingPolarAsync(dto);
        }

        /// <summary>
        /// 分选规则045
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SortingRule")]
        [LogDescription("分选规则045", BusinessType.OTHER, "045", ReceiverTypeEnum.MES)]
        public async Task<List<ProcSortRuleDto>> SortingRuleAsync(SortingRuleDto dto)
        {
            if (IS_DEBUG == true)
            {
                List<ProcSortRuleDetailEquDto> sortList = new List<ProcSortRuleDetailEquDto>();
                for (var i = 0; i < 3; ++i)
                {
                    ProcSortRuleDetailEquDto sortModel = new ProcSortRuleDetailEquDto();
                    sortModel.MinValue = i;
                    sortModel.MinContainingType = Core.Enums.Process.ContainingTypeEnum.Lt;
                    sortModel.MaxValue = i + 10;
                    sortModel.MaxContainingType = Core.Enums.Process.ContainingTypeEnum.LtOrE;
                    sortModel.ParameterCode = $"param{i}";
                    sortModel.ParameterName = $"paramname{i}";
                    sortModel.Grade = $"grade{i}";
                    sortModel.ProcedureCode = $"procedurecode{i}";
                    sortList.Add(sortModel);
                }

                //return sortList;
            }

            return await _formationService.SortingRuleAsync(dto);
        }

        /// <summary>
        /// 产品参数上传046
        /// 多个条码参数相同
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ProductParamSameMultSfc")]
        [LogDescription("产品参数上传046", BusinessType.OTHER, "046", ReceiverTypeEnum.MES)]
        public async Task ProductParamSameMultSfcAsync(ProductParamSameMultSfcDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _equCommonService.ProductParamSameMultSfcAsync(dto);
        }

        /// <summary>
        /// 库存接收047
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("MaterialInventory")]
        [LogDescription("库存接收047", BusinessType.OTHER, "047", ReceiverTypeEnum.MES)]
        public async Task MaterialInventoryAsync(MaterialInventoryDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            //1. 如果是原材料，需要校验格式是否是5锻码
            //2. 物料型号和数量从条码截取

            await _qknyService.MaterialInventoryAsync(dto);
        }

        /// <summary>
        /// 工装条码绑定048
        /// 用于AGV工装和原材料条码绑定，后面直接扫工装上料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ToolBindMaterialAsync")]
        [LogDescription("工装条码绑定048", BusinessType.OTHER, "048", ReceiverTypeEnum.MES)]
        public async Task ToolBindMaterialAsync(ToolBindMaterialDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.ToolBindMaterialAsync(dto);
        }

        /// <summary>
        /// 绑定后极组单个条码进站049
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InboundBindJzSingle")]
        [LogDescription("绑定后极组单个条码进站049", BusinessType.OTHER, "049", ReceiverTypeEnum.MES)]
        public async Task InboundBindJzSingleAsync(InboundBindJzSingleDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _fitTogether.InboundBindJzSingleAsync(dto);

            //await _qknyService.InboundAsync(dto);

        }

        /// <summary>
        /// 绑定后极组单个条码出站050
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundBindJzSingle")]
        [LogDescription("绑定后极组单个条码出站050", BusinessType.OTHER, "050", ReceiverTypeEnum.MES)]
        public async Task OutboundBindJzSingleAsync(OutboundBindJzSingleDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _fitTogether.OutboundBindJzSingleAsync(dto);

        }

        /// <summary>
        /// 获取电芯信息051
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSfcInfo")]
        [LogDescription("获取电芯信息051", BusinessType.OTHER, "051", ReceiverTypeEnum.MES)]
        public async Task<List<SortingSfcInfo>> GetSfcInfoAsync(GetSfcInfoDto dto)
        {
            if (IS_DEBUG == true)
            {
                return new List<SortingSfcInfo> { new SortingSfcInfo { SFC = "SFC001", Grade = "B1" } };
            }

            return await _qknyService.GetSfcInfoAsync(dto);

        }

        /// <summary>
        /// 分选拆盘052
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SortingUnBind")]
        [LogDescription("分选拆盘052", BusinessType.OTHER, "052", ReceiverTypeEnum.MES)]
        public async Task SortingUnBindAsync(SortingUnBindDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.SortingUnBindAsync(dto);
        }

        /// <summary>
        /// 分选出站053
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SortingOutbound")]
        [LogDescription("分选出站053", BusinessType.OTHER, "053", ReceiverTypeEnum.MES)]
        public async Task SortingOutboundAsync(SortingOutboundDto dto)
        {
            if (IS_DEBUG)
            {
                return;
            }

            await _qknyService.SortingOutboundAsync(dto);
        }

        /// <summary>
        /// 设备文件上传054
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("EquFileUpload")]
        [LogDescription("设备文件上传054", BusinessType.OTHER, "054", ReceiverTypeEnum.MES)]
        public async Task EquFileUploadAsync([FromForm] EquFileUploadDto dto)
        {
            if (IS_DEBUG)
            {
                return;
            }

            await _equCommonService.EquFileUploadAsync(dto);
        }

        /// <summary>
        /// 发送请求Http请求097
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SendHttp")]
        [LogDescription("发送请求Http请求097", BusinessType.OTHER, "SendHttp097", ReceiverTypeEnum.MES)]
        [AllowAnonymous]
        public async Task<string> SendHttpAsync(SendHttpDto dto)
        {
            if (IS_DEBUG)
            {
                return "token";
            }

            return await _qknyService.SendHttpAsync(dto);
        }

        /// <summary>
        /// 获取设备Token098
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEquToken")]
        [LogDescription("获取设备Token098", BusinessType.OTHER, "GetEquToken098", ReceiverTypeEnum.MES)]
        [AllowAnonymous]
        public async Task<string> GetEquTokenAsync(QknyBaseDto dto)
        {
            if (IS_DEBUG)
            {
                return "token";
            }

            return await _qknyService.GetEquTokenAsync(dto);
        }

        /// <summary>
        /// 生成24位国标码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create24DxCode")]
        [LogDescription("生成24位国标码", BusinessType.OTHER, "Create24DxCode099", ReceiverTypeEnum.MES)]
        public async Task<string> Create24DxCodeAsync(GenerateDxSfcDto dto)
        {
            if (IS_DEBUG)
            {
                return "token";
            }

            return await _fitTogether.Create24GbCodeAsync(dto);
        }

    }
}
