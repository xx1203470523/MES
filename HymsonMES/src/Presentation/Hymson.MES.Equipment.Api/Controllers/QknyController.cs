﻿using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.EquipmentServices;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture;
using Hymson.MES.EquipmentServices.Services.Qkny;
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
    [Route("EquipmentService/api/v1")]
    public class QknyController : ControllerBase
    {
        /// <summary>
        /// 设备接口服务
        /// </summary>
        private readonly IQknyService _qknyService;

        /// <summary>
        /// 是否调试
        /// </summary>
        private readonly bool IS_DEBUG = true;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QknyController(IEquEquipmentRepository equEquipmentRepository, IQknyService qknyService)
        {
            _qknyService = qknyService;
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
            if(IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.OperatorLoginAsync(dto);
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
            if (IS_DEBUG == true)
            {
                return;
            }

            //TODO 业务逻辑
            //1. 新增equ_equipment_newest_info记录设备最后心跳时间
            //2. 记录心跳记录
            await _qknyService.HeartbeatAsync(dto);
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
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.StateAsync(dto);
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
        [LogDescription("设备运行报警信息004", BusinessType.OTHER, "Alarm004", ReceiverTypeEnum.MES)]
        public async Task AlarmAsync(AlarmDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.AlarmAsync(dto);
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
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.CcdFileUploadCompleteAsync(dto);
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
        [LogDescription("获取开机参数列表007", BusinessType.OTHER, "GetRecipeList007", ReceiverTypeEnum.MES)]
        public async Task<List<GetRecipeListReturnDto>> GetRecipeListAsync(GetRecipeListDto dto)
        {
            if (IS_DEBUG == true)
            {
                List<GetRecipeListReturnDto> resultList = new List<GetRecipeListReturnDto>();
                for (int i = 0;i < 5; ++i)
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

            var result = await _qknyService.GetRecipeListAsync(dto);
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
        [LogDescription("获取开机参数明细008", BusinessType.OTHER, "GetRecipeDetail008", ReceiverTypeEnum.MES)]
        public async Task<GetRecipeDetailReturnDto> GetRecipeDetailAsync(GetRecipeDetailDto dto)
        {
            //TODO
            //1. 获取proc_equipment_group_param_detail开机参数明细，并转成相应格式

            if(IS_DEBUG == true)
            {
                GetRecipeDetailReturnDto resultList = new GetRecipeDetailReturnDto();

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

            var res = await _qknyService.GetRecipeDetailAsync(dto);
            return res;
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
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.RecipeAsync(dto);

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
        [LogDescription("原材料上料010", BusinessType.OTHER, "Feeding010", ReceiverTypeEnum.MES)]
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
        [LogDescription("半成品上料011", BusinessType.OTHER, "HalfFeeding011", ReceiverTypeEnum.MES)]
        public async Task HalfFeedingAsync(HalfFeedingDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }
            await _qknyService.HalfFeedingAsync(dto);
            //TODO
            //1. 本用于涂布，辊分，模切，卷绕工序，现涂布，辊分，模切改为一个工单，这几个地方改为直接进站
            //2. 校验条码是否在上工序产出(manu_sfc_produce)
            //3. 走进站流程
            /*
             * select * from manu_sfc => 修改状态
             * select * from manu_sfc_info msi => 不处理
             * select * from manu_sfc_produce msp => 修改状态
             * 
             * select * from manu_sfc_step mss => 新增
             */
            //4. 工序产出时，在制品表 manu_sfc_produce 删除进站条码, manu_sfc_step 新增
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
            if (IS_DEBUG == true)
            {
                return;
            }

            AgvMaterialDto agvDto = new AgvMaterialDto();
            agvDto.EquipmentCode = dto.EquipmentCode;
            agvDto.ResourceCode = dto.ResourceCode;
            agvDto.LocalTime = dto.LocalTime;
            agvDto.Type = "1";
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
        [LogDescription("下料呼叫Agv013", BusinessType.OTHER, "AgvDownMaterial013", ReceiverTypeEnum.MES)]
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
            agvDto.Type = "2";
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
        [LogDescription("获取配方列表(制胶匀浆)014", BusinessType.OTHER, "FormulaListGet014", ReceiverTypeEnum.MES)]
        public async Task<List<FormulaListGetReturnDto>> FormulaListGetAsync(FormulaListGetDto dto)
        {
            //TODO
            //1. 针对制胶匀浆设备时，在开机进行启动时，从MES获取配方列表
            //2. 获取 proc_formula 表数据，并进行字段转换

            if(IS_DEBUG == true)
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

            return await _qknyService.FormulaListGetAsync(dto);
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

            return await _qknyService.FormulaDetailGetAsync(dto);
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
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.FormulaVersionExamineAsync(dto);

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
        [LogDescription("设备投料前校验(制胶匀浆)017", BusinessType.OTHER, "ConsumeEquBeforeCheck017", ReceiverTypeEnum.MES)]
        public async Task ConsumeEquBeforeCheckAsync(ConsumeEquBeforeCheckDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.ConsumeEquBeforeCheckAsync(dto);

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
        [LogDescription("设备投料(制胶匀浆)018", BusinessType.OTHER, "ConsumeEqu018", ReceiverTypeEnum.MES)]
        public async Task ConsumeEquAsync(ConsumeEquDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.ConsumeEquAsync(dto);
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
        [LogDescription("上料完成(制胶匀浆)019", BusinessType.OTHER, "FeedingCompleted019", ReceiverTypeEnum.MES)]
        public async Task FeedingCompletedAsync(FeedingCompletedDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.FeedingCompletedAsync(dto);

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
        [LogDescription("设备产出(制胶匀浆)020", BusinessType.OTHER, "OutputEqu020", ReceiverTypeEnum.MES)]
        public async Task<string> OutputEquAsync(QknyBaseDto dto)
        {
            if (IS_DEBUG == true)
            {
                string sfc = "SFC001";
                return sfc;
            }

            return await _qknyService.OutputEquAsync(dto);

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
        [LogDescription("批次转移(制胶匀浆)021", BusinessType.OTHER, "BatchMove021", ReceiverTypeEnum.MES)]
        public async Task BatchMoveAsync(BatchMoveDto dto)
        {
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
        [LogDescription("设备投料非生产投料(制胶匀浆)022", BusinessType.OTHER, "ConsumeInNonProductionEqu022", ReceiverTypeEnum.MES)]
        public async Task ConsumeInNonProductionEquAsync(ConsumeInNonProductionEquDto dto)
        {
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
        [LogDescription("请求产出极卷码023", BusinessType.OTHER, "GenerateSfc023", ReceiverTypeEnum.MES)]
        public async Task<List<string>> GenerateSfcAsync(GenerateSfcDto dto)
        {
            //TODO
            //1. 根据数量下发对应的条码给设备
            //2. 用的时候生成(需要的时候在生成条码)或从XX表里面取(提前生成就是下发，生成两个或者3个，)
            //3. 考虑提前生成条码如何标记是否使用

            if(IS_DEBUG == true)
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
        /// 产出米数上报024
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OutboundMetersReport")]
        [LogDescription("产出米数上报024", BusinessType.OTHER, "OutboundMetersReport024", ReceiverTypeEnum.MES)]
        public async Task OutboundMetersReportAsync(OutboundMetersReportDto dto)
        {
            if (IS_DEBUG == true)
            {
                return;
            }

            await _qknyService.OutboundMetersReportAsync(dto);

            //TODO
            //1. 设备上报条码和对应的长度
            //2. 去 manu_sfc，manu_sfc_produce 表修改条码的长度，manu_sfc根据manu_sfc_produce的id来
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
            //TODO
            //1. 用于异常情况，返回设备产出最近的一个条码，从manu_sfc_produce中取

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
            await _qknyService.EquipmentProcessParamAsync(dto);

            //TODO
            //1. 写入参数表，参考现有的EquipmentCollectionAsync，
            //2. 支持错误参数不NG，记录或者忽略
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
            //TODO
            //1. 上工序出站校验
            //2. 是否合格校验
            //3. 支持重复进站（重复进站当前系统能否处理，系统会有个将两条记录移到另一条记录）
            //4. 参考现有进站
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
        [LogDescription("进站多个029", BusinessType.OTHER, "InboundMore029", ReceiverTypeEnum.MES)]
        public async Task<List<InboundMoreReturnDto>> InboundMoreAsync(InboundMoreDto dto)
        {
            //TODO
            //1. 参考现有进站

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
            //TODO
            //1. 参考现有出站

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
            //TODO
            //1. 极组和极组绑定（新视界极组条码接收）
            //2. 校验极组是否合格
            //3. 校验上工序是否合格
            //4. 考虑系统如何方便追溯

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
        [LogDescription("电芯码下发033", BusinessType.OTHER, "GenerateCellSfc033", ReceiverTypeEnum.MES)]
        public async Task<string> GenerateCellSfcAsync(GenerateCellSfcDto dto)
        {
            //TODO
            //1. 生成条码进行下发
            //2. 参考现有创建条码 CreateBarcodeBySemiProductIdAsync，CreateBarcodeByWorkOrderIdAsync

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
            //TODO
            //1. 新增表进行记录
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
            //TODO
            //2. 校验托盘是否存在系统中（待确认）
            //3. 托盘(载具)表 inte_vehicle_freight_stack 是否存在数据
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
            //TODO
            //1. 校验电芯是否合格
            //2. 校验电芯是否在托盘中 inte_vehicle_freight_stack
            //3. 校验电芯是否在上工序出站
            //4. 新增表inte_vehicle_check_record电芯校验记录表，用于绑定 inte_vehicle_check_record，绑定时不需要再次校验
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
            //TODO
            //1. 校验托盘数量
            //2. 校验电芯是否已经做校验 inte_vehicle_check_record
            //3. 托盘和电芯做绑定，删除电芯校验记录表 inte_vehicle_check_record
            //4. inte_vehicle_freight_stack 添加绑定数据
            //5. inte_vehicle_freight_record 添加绑定记录
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
        [LogDescription("托盘NG电芯上报039", BusinessType.OTHER, "ContainerNgReport039", ReceiverTypeEnum.MES)]
        public async Task ContainerNgReportAsync(ContainerNgReportDto dto)
        {
            //TODO
            //1. inte_vehicle_freight_stack 删除绑定数据
            //2. 添加冗余表 inte_vehicle_freight_ng_record，NG解绑记录
            //3. 添加 inte_vehicle_freight_record 解绑记录
            //4. 添加电芯NG记录 manu_product_bad_record
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
            //TODO
            //1. 参考 InBoundCarrierAsync 进站
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
            //2. 添加托盘出站记录
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
        [LogDescription("产品参数上传043", BusinessType.OTHER, "ProductParam043", ReceiverTypeEnum.MES)]
        public async Task ProductParamAsync(ProductParamDto dto)
        {
            //TODO
            //1. 参考ProductCollectionAsync
        }
    }
}
