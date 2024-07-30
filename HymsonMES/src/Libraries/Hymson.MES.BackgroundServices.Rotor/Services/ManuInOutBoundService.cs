using Google.Protobuf.WellKnownTypes;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.BackgroundServices.Rotor.Dtos.Manu;
using Hymson.MES.BackgroundServices.Rotor.Entity;
using Hymson.MES.BackgroundServices.Rotor.Repositories;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Mavel.Rotor;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcInfo.Query;
using Hymson.MES.Data.Repositories.Mavel.Rotor;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Command;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.WaterMark;
using IdGen;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Services
{
    /// <summary>
    /// 过站服务
    /// </summary>
    public class ManuInOutBoundService : IManuInOutBoundService
    {
        /// <summary>
        /// 上料信息
        /// </summary>
        private readonly IWorkItemInfoRepository _workItemInfoRepository;

        /// <summary>
        /// 参数信息
        /// </summary>
        private readonly IWorkProcessDataRepository _workProcessDataRepository;

        /// <summary>
        /// 过站数据
        /// </summary>
        private readonly IWorkProcessRepository _workProcessRepository;

        /// <summary>
        /// 绑定关系
        /// </summary>
        private readonly IWorkOrderRelationRepository _workOrderRelationRepository;

        /// <summary>
        /// 工单产品关联
        /// </summary>
        private readonly IWorkOrderListRepository _workOrderListRepository;

        /// <summary>
        /// 工序
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 物料
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 流转表
        /// </summary>
        private readonly IManuSfcCirculationRepository _manuSfcCirculationRepository;

        /// <summary>
        /// 工单
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 系统配置
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 服务接口（水位）
        /// </summary>
        private readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 条码表
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 条码信息表
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 生产参数
        /// </summary>
        private readonly Data.Repositories.Parameter.IManuProductParameterRepository _manuProductParameterRepository;

        /// <summary>
        /// 标准参数
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 转子条码
        /// </summary>
        private readonly IManuRotorSfcRepository _manuRotorSfcRepository;

        /// <summary>
        /// 仓储接口（产品不良录入）
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 仓储接口（产品NG记录表）
        /// </summary>
        private readonly IManuProductNgRecordRepository _manuProductNgRecordRepository;

        /// <summary>
        /// ERP工单表
        /// </summary>
        private readonly IPlanWorkPlanRepository _planWorkPlanRepository;

        /// <summary>
        /// ERP工单BOM
        /// </summary>
        private readonly IPlanWorkPlanMaterialRepository _planWorkPlanMaterialRepository;

        /// <summary>
        /// 库存
        /// </summary>
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;

        /// <summary>
        /// 转子线操作人
        /// </summary>
        private readonly string OperationBy = "RotorLMSJOB";

        /// <summary>
        /// 轴码和铁芯码绑定工序
        /// </summary>
        private readonly string PRODUCRE_Z_TX = "R01OP070";

        /// <summary>
        /// 成品和轴码绑定东旭
        /// </summary>
        private readonly string PRODUCRE_CP_Z = "R01OP130";

        /// <summary>
        /// 末工序
        /// </summary>
        private readonly string PRODUCRE_END = "R01OP150";

        /// <summary>
        /// 工序前缀
        /// </summary>
        private readonly string PRODUCRE_PREFIX = "R01";

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteID = 0;

        /// <summary>
        /// 调试变量
        /// </summary>
        public int VAR_DEBUG = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuInOutBoundService(
            IWorkItemInfoRepository workItemInfoRepository,
            IWorkProcessDataRepository workProcessDataRepository,
            IWorkProcessRepository workProcessRepository,
            IWorkOrderRelationRepository workOrderRelationRepository,
            IWorkOrderListRepository workOrderListRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcMaterialRepository procMaterialRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            ISysConfigRepository sysConfigRepository,
            IWaterMarkService waterMarkService,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            Data.Repositories.Parameter.IManuProductParameterRepository manuProductParameterRepository,
            IProcParameterRepository procParameterRepository,
            IManuRotorSfcRepository manuRotorSfcRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository,
            IManuProductNgRecordRepository manuProductNgRecordRepository,
            IPlanWorkPlanRepository planWorkPlanRepository,
            IPlanWorkPlanMaterialRepository planWorkPlanMaterialRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository)
        {
            _workItemInfoRepository = workItemInfoRepository;
            _workProcessDataRepository = workProcessDataRepository;
            _workProcessRepository = workProcessRepository;
            _workOrderRelationRepository = workOrderRelationRepository;
            _workOrderListRepository = workOrderListRepository;
            _procProcedureRepository = procProcedureRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _sysConfigRepository = sysConfigRepository;
            _waterMarkService = waterMarkService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
            _procParameterRepository = procParameterRepository;
            _manuRotorSfcRepository = manuRotorSfcRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _manuProductNgRecordRepository = manuProductNgRecordRepository;
            _planWorkPlanRepository = planWorkPlanRepository;
            _planWorkPlanMaterialRepository = planWorkPlanMaterialRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
        }

        /// <summary>
        /// 进站状态-S
        /// </summary>
        private readonly string ProductStatus_In = "S";

        /// <summary>
        /// 出站状态-Z
        /// </summary>
        private readonly string ProductStatus_Out = "Z";

        /// <summary>
        /// OK标识
        /// </summary>
        private readonly string Result_OK = "OK";

        /// <summary>
        /// 物料类型 1-批次
        /// </summary>
        private readonly int MatType_One = 1;

        /// <summary>
        /// 物料类型 2-批次
        /// </summary>
        private readonly int MatType_Batch = 2;

        #region 主体逻辑

        /// <summary>
        /// 进出站
        /// </summary>
        /// <param name="start"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public async Task InOutBoundAsync(int rows = 200)
        {
            DateTime now = HymsonClock.Now();

            string busKey = "MavelRatorTotal";
            long waterMarkId = await _waterMarkService.GetWaterMarkAsync(busKey);
            if(waterMarkId == 333)
            {
                return;
            }
            DateTime startWaterMarkTime = DateTime.Now;
            if(waterMarkId != 0)
            {
                startWaterMarkTime = UnixTimestampMillisToDateTime(waterMarkId);
            }
            else
            {
                startWaterMarkTime = DateTime.Parse("2024-07-25 01:01:01");
            }
            DateTime start = startWaterMarkTime;

            //TODO
            //1. 中间工序不管进出站，但是参数需要记录保存，上料信息需要记录
            //2. 进站工序的上料，直接查出来
            //3. 710工序当作芯轴上料工序，铁芯码和轴码绑定，轴码和最上面的铁芯码绑定关系在Work_ItemInfo里已经有
            //4. 中间工序NG后，后面不会在有进出站记录
            //5. 进站不会NG，出站才有NG
            //6. 进站没有参数上传，没有关联数据
            //7. 进出站是同一条记录
            //8. 只处理有出站时间，且出站（状态=Z）的数据
            //9. 只处理出站，进站忽略

            //获取MES需要处理管控的工位数据
            List<WorkPosDto> workPosList = GetPosNoList();
            //List<WorkPosDto> mesWorkPosList = workPosList.Where(m => m.WorkPosType != 0).ToList();
            List<string> workPosCodeList = workPosList.Select(m => m.WorkPosNo).ToList();

            var configEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (configEntities == null || !configEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139));
            }
            SiteID = long.Parse(configEntities.ElementAt(0).Value);

            #region 获取线体LMS数据
            //表名后缀
            string suffixTableName = GetFirstDayOfQuarter(start);
            //1. 根据开始时间，结束时间，读取线体MES过站数据
            List<WorkProcessDto> inOutList = await GetInOutBoundListAsync(start, rows, suffixTableName);
            if(inOutList == null || inOutList.Count == 0)
            {
                //如果数据为空，且当前水位的季度和当前时间的季度不一样，则把水位转为下个季度的初始时间
                string curDateStr = GetFirstDayOfQuarter(now);
                if(suffixTableName != curDateStr)
                {
                    //LMS数据一个季度一张表，季度不匹配，则水位往后推一个季度
                    DateTime curQuarter = DateTime.ParseExact(suffixTableName, "yyyyMMdd", CultureInfo.InvariantCulture).AddMonths(3);
                    long nowTimestamp = GetTimestampInMilliseconds(curQuarter);
                    await _waterMarkService.RecordWaterMarkAsync(busKey, nowTimestamp);
                }

                return;
            }
            List<string> sfcIdList = inOutList.Select(m => m.ID).ToList();
            List<string> sfcList = inOutList.Select(m => m.ProductNo).Distinct().ToList();
            //2. 查询参数信息
            List<WorkProcessDataDto> paramList = await GetParamListAsync(suffixTableName, sfcIdList);
            //3. 查询上料信息
            List<WorkItemInfoDto> upMatList = await GetUpMatListAsync(suffixTableName, sfcIdList);
            //4. 查询铁芯码和轴码对应关系
            //List<WorkOrderRelationDto> bindList = await GetBindListAsync(sfcList);
            //List<string> orderCodeList = bindList.Select(m => m.WorkNo).Distinct().ToList();
            //5. 查询产品和工单的对应关系
            List<WorkOrderListDto> productOrderList = await GetProductOrder(sfcList);
            List<string> allLmsOrderList = productOrderList.Select(m => m.WorkNo).Distinct().ToList();
            List<string> lmsOrderList = new List<string>();
            //转子线接收MES的工单后，会生成两个工单，一个带FX的工单，是铁芯工单，还有一个正常的工单是压轴主线 
            foreach(var item in allLmsOrderList)
            {
                var tmpOrderCode = LmsOrderFxChange(item);
                //if (tmpOrderCode == "T240722")
                //{
                //    tmpOrderCode = "MO-20240729001-6";
                //}
                lmsOrderList.Add(tmpOrderCode);
            }
            lmsOrderList = lmsOrderList.Distinct().ToList();
            #endregion

            #region 获取MES数据
            //MES工单数据
            List<PlanWorkOrderEntity> mesOrderList = await GetMesOrderAsync(lmsOrderList);
            //MES工序信息
            ProcProcedureQuery procedureQuery = new ProcProcedureQuery();
            procedureQuery.SiteId = SiteID;
            List<ProcProcedureEntity> mesProcedureList = await GetMesProcedureAsync(procedureQuery);
            //MES物料信息
            ProcMaterialQuery materialQuery = new ProcMaterialQuery();
            materialQuery.SiteId = SiteID;
            List<ProcMaterialEntity> mesMaterialList = await GetMesMaterialAsync(materialQuery);
            //MES参数
            ProcParameterQuery paramQuery = new ProcParameterQuery();
            paramQuery.SiteId = SiteID;
            List<ProcParameterEntity> mesParamList = (await _procParameterRepository.GetProcParameterEntitiesAsync(paramQuery)).ToList();
            #endregion

            //MES过站数据
            List<MesOutDto> mesList = new List<MesOutDto>();

            #region 线体LMS数据转成MES数据

            foreach(var item in inOutList)
            {
                //处理条码为空的异常数据
                if(string.IsNullOrEmpty(item.ProductNo) == true)
                {
                    continue;
                }
                //获取工位类型
                WorkPosDto? curWorkPos = workPosList.Where(m => item.WorkPosNo == m.WorkPosNo).FirstOrDefault();
                if(curWorkPos == null)
                {
                    continue;
                }

                string procedureCode = item.ProcedureCode;
                if(item.ProcedureCode == "OP710")
                {
                    procedureCode = "OP060";
                }
                //上铁芯码
                if(item.ProcedureCode == "OP720")
                {
                    procedureCode = "OP070";
                }
                procedureCode = PRODUCRE_PREFIX + procedureCode;

                //基础数据
                MesOutDto mesDto = new MesOutDto();
                mesDto.Sfc = item.ProductNo;
                mesDto.ProcedureCode = procedureCode;
                mesDto.IsPassed = item.Result == Result_OK ? true : false;
                mesDto.Date = item.CreateTime;
                WorkOrderListDto? curProductOrder = productOrderList.Where(m => m.ProductNo == item.ProductNo).FirstOrDefault();
                if(curProductOrder != null)
                {
                    mesDto.OrderCode = LmsOrderFxChange(curProductOrder.WorkNo);
                }

                //处理上料信息
                List<WorkItemInfoDto> curUpMatList = upMatList.Where(m => m.ProcessUID == item.ID).ToList();
                List<SfcUpMatDto> sfcUpList = new List<SfcUpMatDto>();
                if (curUpMatList != null && curUpMatList.Count > 0)
                {
                    foreach (var upItem in curUpMatList)
                    {
                        if(string.IsNullOrEmpty(upItem.MatValue) == false)
                        {
                            sfcUpList.Add(new SfcUpMatDto()
                            {
                                MainMatCode = upItem.ProductTypeNo,
                                MatName = upItem.MatName,
                                MatValue = upItem.MatValue,
                                MatBatchCode = upItem.MatBatchCode,
                                BarCode = upItem.MatValue,
                                MatCode = upItem.MatCode,
                                MatType = MatType_One,
                                MatNum = 1
                            });
                        }
                        //条码不为空，并且两个条码不同（设备异常数据会导致两个条码相同）
                        if(string.IsNullOrEmpty(upItem.MatBatchCode) == false && upItem.MatBatchCode != upItem.MatValue)
                        {
                            sfcUpList.Add(new SfcUpMatDto()
                            {
                                MainMatCode = upItem.ProductTypeNo,
                                MatName = upItem.MatName,
                                MatValue = upItem.MatValue,
                                MatBatchCode = upItem.MatBatchCode,
                                BarCode = upItem.MatBatchCode,
                                MatCode = upItem.LotCode,
                                MatType = MatType_Batch,
                                MatNum = upItem.MatNum
                            });
                        }
                    }
                }
                
                //处理参数信息
                List<WorkProcessDataDto> curParamList = paramList.Where(m => m.ProcessUID == item.ID).ToList();
                List<SfcParamDto> sfcParamList = new List<SfcParamDto>();
                if (curParamList != null && curParamList.Count > 0)
                {
                    foreach (var paramItem in curParamList)
                    {
                        SfcParamDto curParamDto = new SfcParamDto();
                        curParamDto.ParamName = paramItem.Name;
                        curParamDto.ParamCode = paramItem.NameCode;
                        curParamDto.CreateOn = paramItem.CreateTime;
                        curParamDto.Unit = paramItem.Unit;
                        curParamDto.Value = Convert.ToDecimal(paramItem.Value);
                        curParamDto.StrValue = paramItem.StrValue;
                        curParamDto.Result = paramItem.Result == 1 ? true : false;
                        curParamDto.ValueType = string.IsNullOrEmpty(paramItem.StrValue) ? 1 : 2;
                        curParamDto.ParamValue = string.IsNullOrEmpty(paramItem.StrValue) ? curParamDto.Value.ToString() : paramItem.StrValue;
                        sfcParamList.Add(curParamDto);
                    }
                }

                mesDto.ParamList = sfcParamList;
                mesDto.UpMatList = sfcUpList;
                mesDto.NgList = new List<string>();
                mesDto.Type = 0;

                //如果是中间工位，且不是NG，则只处理参数
                if (curWorkPos.WorkPosType == 0 && mesDto.IsPassed == true)
                {
                    mesDto.Type = 0;
                }

                //进站
                if ((curWorkPos.WorkPosType & 1) == 1)
                {
                    mesDto.Type = 1;
                }
                //NG出站（中间工位可能会NG,NG在后续不会在有记录，当成出站处理）
                if (mesDto.IsPassed == false)
                {
                    mesDto.Type = 2;
                    mesDto.ParamToNgList();

                    // 插入不良记录
                    //_manuProductBadRecordRepository.InsertRangeAsync(data.ProductBadRecordEntities),

                    // 插入NG记录
                    //_manuProductNgRecordRepository.InsertRangeAsync(data.ProductNgRecordEntities),
                }
                //正常出站
                if ((curWorkPos.WorkPosType & 2) == 2 && item.ProductStatus == ProductStatus_Out && mesDto.IsPassed == true)
                {
                    mesDto.Type = 2;
                }

                mesList.Add(mesDto);
            }

            #endregion

            List<ManuSfcCirculationEntity> circulaList = new List<ManuSfcCirculationEntity>();
            List<ManuBarCodeRelationEntity> circulaBarList = new List<ManuBarCodeRelationEntity>();
            List<ManuSfcStepEntity> stepList = new List<ManuSfcStepEntity>();
            List<ManuSfcDto> sfcUpdateList = new List<ManuSfcDto>(); //条码表，条码信息表
            List<Core.Domain.Parameter.ManuProductParameterEntity> manuParamList = new List<Core.Domain.Parameter.ManuProductParameterEntity>();
            List<ManuSfcDto> barCodeList = new List<ManuSfcDto>(); //原材料条码写条码表，条码信息表
            List<ManuRotorSfcEntity> addRotorList = new List<ManuRotorSfcEntity>();
            List<ManuRotorSfcEntity> updateRotorList = new List<ManuRotorSfcEntity>();
            List<ManuProductBadRecordEntity> badRecordList = new List<ManuProductBadRecordEntity>();
            List<ManuProductNgRecordEntity> ngRecordList = new List<ManuProductNgRecordEntity>();

            #region 整理成MES表结构数据

            //MES过站数据转成MES表数据结构
            //IManuSfcCirculationRepository ManuSfcCirculationEntity
            //IManuSfcStepRepository    ManuSfcStepEntity
            //参数
            //NG信息

            foreach (var mesItem in mesList)
            {
                //如果是过程数据，没有上料信息和参数信息，则直接忽略
                if (mesItem.Type == 0 && mesItem.UpMatList.Count == 0 && mesItem.ParamList.Count == 0)
                {
                    continue;
                }
                long stepId = 0;

                string productCode = string.Empty;
                //工单
                PlanWorkOrderEntity? mesOrder = mesOrderList.Where(m => m.OrderCode == LmsOrderFxChange(mesItem.OrderCode)).FirstOrDefault();
                if (mesOrder != null)
                {
                    var mesOrderMat = mesMaterialList.Where(m => m.Id == mesOrder.ProductId).FirstOrDefault();
                    productCode = mesOrderMat == null ? "" : mesOrderMat.MaterialCode;
                }
                else
                {
                    VAR_DEBUG = 3;
                }

                //工序
                long procedureId = 0;
                ProcProcedureEntity ?mesProcedure = mesProcedureList.Where(m => m.Code == mesItem.ProcedureCode).FirstOrDefault();
                if(mesProcedure != null)
                {
                    procedureId = mesProcedure.Id;
                }
                else
                {
                    VAR_DEBUG = 3;
                }

                //写入到步骤表
                if (mesItem.Type == 2)
                {
                    ManuSfcStepEntity step = GetStepEntity(mesItem.Sfc, mesItem.Type, mesItem.ProcedureCode,
                        mesItem.IsPassed, mesOrder, procedureId, mesItem.Date);
                    stepId = step.Id;
                    stepList.Add(step);
                    sfcUpdateList.Add(new ManuSfcDto()
                    {
                        WorkOrder = mesOrder,
                        SiteId = SiteID,
                        Status = SfcStatusEnum.lineUp,
                        Sfc = mesItem.Sfc,
                        UpdatedOn = now,
                        UserId = OperationBy
                    });
                }
                //写入到流转表ManuSfcCirculationEntity
                if (mesItem.UpMatList.Count > 0)
                {
                    List<ManuSfcCirculationEntity> curCirculaList = GetCirculaList(mesItem.Sfc, mesItem.UpMatList,
                        mesItem.ProcedureCode, mesOrder, mesMaterialList, procedureId, mesItem.Date);
                    circulaList.AddRange(curCirculaList);
                    //上料信息也要写入到条码表，条码信息表
                    foreach(var item in mesItem.UpMatList)
                    {
                        if (item.BarCode == mesItem.Sfc) //铁芯4个合一个的时候，会有条码合上料条码相同的记录
                        {
                            continue;
                        }
                        if(mesItem.ProcedureCode == PRODUCRE_CP_Z)
                        {
                            VAR_DEBUG = 3;
                        }
                        barCodeList.Add(new ManuSfcDto()
                        {
                            WorkOrder = mesOrder,
                            SiteId = SiteID,
                            Status = SfcStatusEnum.Complete,
                            Sfc = item.BarCode,
                            MaterialCode = item.MatCode,
                            UpdatedOn = now,
                            UserId = OperationBy
                        });
                    }
                    if(mesItem.ProcedureCode == PRODUCRE_Z_TX)
                    {
                        ManuRotorSfcEntity ?sfc070 = LmsSfcChange(mesItem.Sfc, mesItem.UpMatList, mesItem.ProcedureCode, mesOrder, productCode);
                        if(sfc070 != null)
                        {
                            addRotorList.Add(sfc070);
                        }
                    }
                    if(mesItem.ProcedureCode == PRODUCRE_CP_Z)
                    {
                        ManuRotorSfcEntity? sfc130 = LmsSfcChange(mesItem.Sfc, mesItem.UpMatList, mesItem.ProcedureCode, mesOrder, productCode);
                        if (sfc130 != null)
                        {
                            updateRotorList.Add(sfc130);
                        }
                    }
                }
                //写入到参数表
                if(mesItem.ParamList.Count > 0)
                {
                    List<Core.Domain.Parameter.ManuProductParameterEntity> addParamList =
                        GetParamList(mesItem.ParamList, stepId, mesItem.Sfc, procedureId, mesParamList);
                    manuParamList.AddRange(addParamList);
                }
                if(mesItem.IsPassed == false)
                {
                    ManuProductBadRecordEntity bdModel = new ManuProductBadRecordEntity();
                    bdModel.Id = IdGenProvider.Instance.CreateId();
                    bdModel.SiteId = SiteID;
                    bdModel.FoundBadOperationId = procedureId;
                    bdModel.OutflowOperationId = procedureId;
                    bdModel.UnqualifiedId = 0;
                    bdModel.SFC = mesItem.Sfc;
                    bdModel.SfcInfoId = 0;
                    bdModel.SfcStepId = stepId;
                    bdModel.Qty = 1;
                    bdModel.Status = Core.Enums.Manufacture.ProductBadRecordStatusEnum.Open;
                    bdModel.Source = Core.Enums.Manufacture.ProductBadRecordSourceEnum.EquipmentReBad;
                    bdModel.Remark = "";
                    bdModel.CreatedBy = OperationBy;
                    bdModel.CreatedOn = HymsonClock.Now();
                    bdModel.UpdatedBy = OperationBy;
                    bdModel.UpdatedOn = bdModel.CreatedOn;
                    badRecordList.Add(bdModel);

                    if(mesItem.NgList == null || mesItem.NgList.Count == 0)
                    {
                        ManuProductNgRecordEntity ngModel = new ManuProductNgRecordEntity();
                        ngModel.Id = IdGenProvider.Instance.CreateId();
                        ngModel.SiteId = SiteID;
                        ngModel.BadRecordId = bdModel.Id;
                        ngModel.UnqualifiedId = 0;
                        ngModel.NGCode = "未知";
                        ngModel.CreatedBy = OperationBy;
                        ngModel.CreatedOn = HymsonClock.Now();
                        ngModel.UpdatedBy = OperationBy;
                        ngModel.UpdatedOn = bdModel.CreatedOn;
                        ngRecordList.Add(ngModel);
                    }
                    else
                    {
                        foreach(var item in mesItem.NgList)
                        {
                            ManuProductNgRecordEntity ngModel = new ManuProductNgRecordEntity();
                            ngModel.Id = IdGenProvider.Instance.CreateId();
                            ngModel.SiteId = SiteID;
                            ngModel.BadRecordId = bdModel.Id;
                            ngModel.UnqualifiedId = 0;
                            ngModel.NGCode = item;
                            ngModel.CreatedBy = OperationBy;
                            ngModel.CreatedOn = HymsonClock.Now();
                            ngModel.UpdatedBy = OperationBy;
                            ngModel.UpdatedOn = bdModel.CreatedOn;
                            ngRecordList.Add(ngModel);
                        }
                    }

                }
            }

            #endregion

            //水位数据更新
            DateTime? maxEndTime = inOutList.Max(x => x.EndTime);
            long timestamp = GetTimestampInMilliseconds(maxEndTime);

            //MES数据入库
            using var trans = TransactionHelper.GetTransactionScope();

            List<Task<int>> tasks = new()
            {
                 _manuSfcCirculationRepository.InsertRangeAsync(circulaList),
                 //await _manuSfcStepRepository.InsertRangeMavleAsync(stepList),
                 _manuSfcStepRepository.InsertRangeAsync(stepList),
                 _waterMarkService.RecordWaterMarkAsync(busKey, timestamp),
                 InsertOrUpdateAsync(sfcUpdateList),
                 InsertRawMaterialAsync(barCodeList, mesMaterialList),
                 OrderCompleteQtyChangeAsync(mesOrderList),
                 InsertOrUpdateRotorSfcAsync(addRotorList, updateRotorList),
                 //await _manuProductParameterRepository.InsertRangeMavelAsync(manuParamList),
                 _manuProductBadRecordRepository.InsertRangeAsync(badRecordList),
                 _manuProductNgRecordRepository.InsertRangeAsync(ngRecordList),
                 _manuProductParameterRepository.InsertRangeAsync(manuParamList),
            };
            var rowArray = await Task.WhenAll(tasks);

            //await _manuSfcCirculationRepository.InsertRangeAsync(circulaList);
            ////await _manuSfcStepRepository.InsertRangeMavleAsync(stepList);
            //await _manuSfcStepRepository.InsertRangeAsync(stepList);
            //await _waterMarkService.RecordWaterMarkAsync(busKey, timestamp);
            //await InsertOrUpdateAsync(sfcUpdateList);
            //await InsertRawMaterialAsync(barCodeList, mesMaterialList);
            //await OrderCompleteQtyChangeAsync(mesOrderList);
            //await InsertOrUpdateRotorSfcAsync(addRotorList, updateRotorList);
            ////await _manuProductParameterRepository.InsertRangeMavelAsync(manuParamList);
            //await _manuProductBadRecordRepository.InsertRangeAsync(badRecordList);
            //await _manuProductNgRecordRepository.InsertRangeAsync(ngRecordList);
            //await _manuProductParameterRepository.InsertRangeAsync(manuParamList);

            trans.Complete();
        }

        #endregion

        #region LMS-SQLSERVER数据库查询

        /// <summary>
        /// 获取线体MES过站数据
        /// </summary>
        /// <param name="start"></param>
        /// <param name="rows"></param>
        /// <param name="suffixTableName"></param>
        /// <returns></returns>
        private async Task<List<WorkProcessDto>> GetInOutBoundListAsync(DateTime start, int rows,
            string suffixTableName)
        {
            List<WorkProcessDto> resultList = new List<WorkProcessDto>();

            string sql = $@"
                SELECT TOP {rows} * 
                FROM Work_Process_{suffixTableName} wp 
                WHERE IsDeleted = 0
                AND EndTime IS NOT NULL
                AND EndTime > '{start.ToString("yyyy-MM-dd HH:mm:ss.fff")}'
                AND ProductStatus = '{ProductStatus_Out}'
-- and ProductNo = 'E3001131AFV15881001001TPL060102H60400043'
                ORDER BY EndTime ASC;
            ";

            List<WorkProcessDto> dbList = await _workProcessRepository.GetList(sql);
            if(dbList == null || dbList.Count == 0)
            {
                return resultList;
            }

            return dbList;
        }

        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="suffixTableName"></param>
        /// <param name="sfcIdList"></param>
        /// <returns></returns>
        private async Task<List<WorkProcessDataDto>> GetParamListAsync(string suffixTableName, List<string> sfcIdList)
        {
            List<WorkProcessDataDto> resultLit = new List<WorkProcessDataDto>();

            if (sfcIdList == null ||  sfcIdList.Count == 0)
            {
                return resultLit;
            }

            List<WorkProcessDataDto> allDbList = new List<WorkProcessDataDto>();

            int batchDataNum = 999;
            int batchNum = sfcIdList.Count / batchDataNum + 1;
            for(int i = 0; i < batchNum; ++i)
            {
                List<string> curSfcIdList = sfcIdList.Skip(i * batchDataNum).Take(batchDataNum).ToList();
                string idListStr = string.Join(",", curSfcIdList.Select(uuid => $"'{uuid}'"));

                string sql = $@"
                    SELECT t2.*
                    FROM Work_Process_{suffixTableName} t1
                    inner join Work_ProcessData_{suffixTableName} t2 on t1.ID  = t2.ProcessUID 
                    WHERE T2.ProcessUID IN ( {idListStr} )
                ";

                List<WorkProcessDataDto> dbList = await _workProcessDataRepository.GetList(sql);
                allDbList.AddRange(dbList);
            }

            return allDbList;
        }

        /// <summary>
        /// 获取上料信息
        /// </summary>
        /// <param name="suffixTableName"></param>
        /// <param name="sfcIdList"></param>
        /// <returns></returns>
        private async Task<List<WorkItemInfoDto>> GetUpMatListAsync(string suffixTableName, List<string> sfcIdList)
        {
            List<WorkItemInfoDto> resultLit = new List<WorkItemInfoDto>();

            if (sfcIdList == null || sfcIdList.Count == 0)
            {
                return resultLit;
            }

            List<WorkItemInfoDto> allDbList = new List<WorkItemInfoDto>();

            int batchDataNum = 999;
            int batchNum = sfcIdList.Count / batchDataNum + 1;
            for (int i = 0; i < batchNum; ++i)
            {
                List<string> curSfcIdList = sfcIdList.Skip(i * batchDataNum).Take(batchDataNum).ToList();
                string idListStr = string.Join(",", sfcIdList.Select(uuid => $"'{uuid}'"));

                string sql = $@"
                    SELECT t1.*
                    FROM Work_ItemInfo t1
                    inner join Work_Process_{suffixTableName} t2 on t1.ProcessUID = t2.ID  and t2.IsDeleted  = 0
                    WHERE T1.ProcessUID IN ( {idListStr} )
                ";

                var dbList = await _workItemInfoRepository.GetList(sql);
                allDbList.AddRange(dbList);
            }

            resultLit = allDbList.Cast<WorkItemInfoDto>().ToList();

            return resultLit;
        }

        /// <summary>
        /// 获取铁芯码和轴码绑定关系
        /// </summary>
        /// <param name="sfcList"></param>
        /// <returns></returns>
        private async Task<List<WorkOrderRelationDto>> GetBindListAsync(List<string> sfcList)
        {
            List<WorkOrderRelationDto> resultLit = new List<WorkOrderRelationDto>();

            if (sfcList == null || sfcList.Count == 0)
            {
                return resultLit;
            }

            List<WorkOrderRelationDto> allDbList = new List<WorkOrderRelationDto>();

            int batchDataNum = 999;
            int batchNum = sfcList.Count / batchDataNum + 1;
            for (int i = 0; i < batchNum; ++i)
            {
                List<string> curSfcList = sfcList.Skip(i * batchDataNum).Take(batchDataNum).ToList();
                string sfcListStr = string.Join(",", curSfcList.Select(sfc => $"'{sfc}'"));

                string sql = $@"
                    SELECT * 
                    FROM Work_OrderRelation t1
                    WHERE T1.IsDeleted  = 0
                    AND ProductNo IN ( {sfcListStr} )
                ";

                List<WorkOrderRelationDto> dbList = await _workOrderRelationRepository.GetList(sql);
                allDbList.AddRange(dbList);
            }

            return allDbList;
        }

        /// <summary>
        /// 获取产品工单对应关系
        /// </summary>
        /// <param name="sfcList"></param>
        /// <returns></returns>
        private async Task<List<WorkOrderListDto>> GetProductOrder(List<string> sfcList)
        {
            List<WorkOrderListDto> resultLit = new List<WorkOrderListDto>();

            if (sfcList == null || sfcList.Count == 0)
            {
                return resultLit;
            }

            List<WorkOrderListDto> allDbList = new List<WorkOrderListDto>();

            int batchDataNum = 999;
            int batchNum = sfcList.Count / batchDataNum + 1;
            for (int i = 0; i < batchNum; ++i)
            {
                List<string> curSfcList = sfcList.Skip(i * batchDataNum).Take(batchDataNum).ToList();
                string sfcListStr = string.Join(",", curSfcList.Select(sfc => $"'{sfc}'"));

                string sql = $@"
                    SELECT  WorkNo,ProductNo 
                    FROM Work_OrderList t1
                    WHERE T1.IsDeleted  = 0
                    AND ProductNo IN ( {sfcListStr} )
                ";

                List<WorkOrderListDto> dbList = await _workOrderListRepository.GetList(sql);
                allDbList.AddRange(dbList);
            }

            return allDbList;
        }

        #endregion

        #region MES数据读写

        /// <summary>
        /// 获取步骤表
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="type">1-进站 2-出站</param>
        /// <param name="produceCode"></param>
        /// <param name="mesOrder"></param>
        /// <param name="procedureId"></param>
        /// <param name="createdOn"></param>
        /// <returns></returns>
        private ManuSfcStepEntity GetStepEntity(string sfc, int type, string produceCode,
            bool isPassed, PlanWorkOrderEntity ?mesOrder, long procedureId, DateTime createdOn)
        {
            ManuSfcStepEntity step = new ManuSfcStepEntity();
            step.Id = IdGenProvider.Instance.CreateId();
            step.SiteId = SiteID;
            step.SFC = sfc;
            step.Qty = 1;
            step.ScrapQty = isPassed == true ? 0 : 1; //有报废数量代表不合格
            step.Remark = produceCode; //备注字段存放工序，用于NIO直接取
            step.Operatetype = Core.Enums.Manufacture.ManuSfcStepTypeEnum.Receive;
            step.CurrentStatus = SfcStatusEnum.Activity;
            step.AfterOperationStatus = SfcStatusEnum.lineUp;
            step.UpdatedBy = OperationBy;
            step.CreatedBy = OperationBy;
            step.ProcedureId = procedureId;
            step.CreatedOn = createdOn;
            step.UpdatedOn = HymsonClock.Now();
            if(produceCode == PRODUCRE_END) //未工序设置为完成
            {
                step.CurrentStatus = SfcStatusEnum.Complete;
                step.AfterOperationStatus = SfcStatusEnum.Complete;
            }

            if (mesOrder != null)
            {
                step.ProductBOMId = mesOrder.ProductBOMId;
                step.ProcessRouteId = mesOrder.ProcessRouteId;
                step.ProductId = mesOrder.ProductId;
                step.WorkCenterId = mesOrder.WorkCenterId;
                step.WorkOrderId = mesOrder.Id;
            }
            else
            {
                VAR_DEBUG = 3;
            }

            return step;
        }

        /// <summary>
        /// 获取流转记录
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="upList"></param>
        /// <param name="produceCode"></param>
        /// <param name="mesOrder"></param>
        /// <param name="matList"></param>
        /// <returns></returns>
        private List<ManuSfcCirculationEntity> GetCirculaList(string sfc, List<SfcUpMatDto> upList,
            string produceCode, PlanWorkOrderEntity ?mesOrder, 
            List<ProcMaterialEntity> matList, long procedureId, DateTime createdOn)
        {
            List<ManuSfcCirculationEntity> list = new List<ManuSfcCirculationEntity>();

            foreach(var item in upList)
            {
                if(sfc == item.BarCode)
                {
                    continue;
                }
                ManuSfcCirculationEntity model = new ManuSfcCirculationEntity();
                model.Id = IdGenProvider.Instance.CreateId();
                model.SiteId = SiteID;
                if(produceCode != PRODUCRE_CP_Z)
                {
                    model.SFC = sfc;
                    model.CirculationBarCode = item.BarCode;
                }
                else
                {
                    model.SFC = item.BarCode;
                    model.CirculationBarCode = sfc;
                }

                model.CirculationQty = item.MatNum;
                model.Location = produceCode;
                model.CreatedBy = OperationBy;
                model.UpdatedBy = OperationBy;
                model.CirculationType = Core.Enums.Manufacture.SfcCirculationTypeEnum.Consume;
                model.ProcedureId = procedureId;
                model.CreatedOn = createdOn;
                model.UpdatedOn = HymsonClock.Now();

                //上料条码
                ProcMaterialEntity? upMatModel = matList.Where(m => m.MaterialCode == item.MatCode).FirstOrDefault();
                if(upMatModel != null)
                {
                    if(produceCode != PRODUCRE_CP_Z) 
                    {
                        model.CirculationProductId = upMatModel.Id;
                        model.CirculationMainProductId = upMatModel.Id;
                    }
                    else //如果是轴码对成品码工序，则调转两个产品
                    {
                        model.ProductId = upMatModel.Id;
                    }
                }
                else
                {
                    VAR_DEBUG = 0;
                }
                //当前产品条码
                ProcMaterialEntity? curMatModel = matList.Where(m => m.MaterialCode == item.MainMatCode).FirstOrDefault();
                if(curMatModel != null)
                {
                    if (produceCode != PRODUCRE_CP_Z)
                    {
                        model.ProductId = curMatModel.Id;
                    }
                    else //如果是轴码对成品码工序，则调转两个产品
                    {
                        model.CirculationProductId = curMatModel.Id;
                        model.CirculationMainProductId = curMatModel.Id;
                    }
                }
                else
                {
                    VAR_DEBUG = 0;
                }

                if (mesOrder != null)
                {
                    model.WorkOrderId = mesOrder.Id;
                    model.CirculationWorkOrderId = mesOrder.Id;
                }
                else
                {
                    VAR_DEBUG = 3;
                }

                //临时做法，不然没法追溯
                if(model.ProductId == 0)
                {
                    model.ProductId = 51558093782310912;
                }
                if(model.CirculationProductId == 0)
                {
                    model.CirculationProductId = 51558093782310912;
                }

                list.Add(model);
            }

            return list;
        }

        /// <summary>
        /// 获取参数列表
        /// </summary>
        /// <param name="parammList"></param>
        /// <param name="produceCode"></param>
        /// <param name="stepId"></param>
        /// <param name="sfc"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        private List<Core.Domain.Parameter.ManuProductParameterEntity> GetParamList(List<SfcParamDto> parammList,
            long stepId, string sfc, long procedureId, List<ProcParameterEntity> mesParamList)
        {
            List<Core.Domain.Parameter.ManuProductParameterEntity> resultList = new List<Core.Domain.Parameter.ManuProductParameterEntity>();
            
            if(parammList == null || parammList.Count == 0)
            {
                return resultList;
            }

            long paramId = 0;

            foreach (var item in parammList)
            {
                ProcParameterEntity? mesParam = mesParamList.Where(m => m.ParameterCode == item.ParamCode).FirstOrDefault();
                if(mesParam != null)
                {
                    paramId = mesParam.Id;
                }

                Core.Domain.Parameter.ManuProductParameterEntity model = new Core.Domain.Parameter.ManuProductParameterEntity();
                model.Id = IdGenProvider.Instance.CreateId();
                model.SiteId = SiteID;
                model.ProcedureId = procedureId;
                model.SfcstepId = stepId;
                model.SFC = sfc;
                model.ParameterId = paramId;
                model.ParameterValue = item.ParamValue;
                model.ParameterGroupId = item.Result == true ? 1 : 0;
                model.CreatedBy = OperationBy;
                model.UpdatedBy = OperationBy;
                model.CreatedOn = item.CreateOn;
                model.CollectionTime = item.CreateOn;
                model.UpdatedOn = HymsonClock.Now();

                resultList.Add(model);
            }

            return resultList;
        }

        /// <summary>
        /// 获取MES工单
        /// </summary>
        /// <param name="orderCodeList"></param>
        /// <returns></returns>
        private async Task<List<PlanWorkOrderEntity>> GetMesOrderAsync(List<string> orderCodeList)
        {
            List<PlanWorkOrderEntity> list = new List<PlanWorkOrderEntity>();
            if(orderCodeList == null || orderCodeList.Count == 0)
            {
                return list;
            }

            PlanWorkOrderNewQuery query = new PlanWorkOrderNewQuery();
            query.Codes = orderCodeList;
            query.SiteId = SiteID;
            var dbList = await _planWorkOrderRepository.GetEntitiesAsync(query);
            if(dbList == null)
            {
                return list;
            }

            return dbList!.ToList();
        }

        /// <summary>
        /// 获取工序
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private async Task<List<ProcProcedureEntity>> GetMesProcedureAsync(ProcProcedureQuery query)
        {
            var dbList = await _procProcedureRepository.GetEntitiesAsync(query);

            return dbList.ToList();
        }

        /// <summary>
        /// 获取物料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private async Task<List<ProcMaterialEntity>> GetMesMaterialAsync(ProcMaterialQuery query)
        {
            var dbList = await _procMaterialRepository.GetEntitiesAsync(query);

            return dbList.ToList();
        }

        /// <summary>
        /// 插入或者更新条码表
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        private async Task<int> InsertOrUpdateAsync(List<ManuSfcDto> commands)
        {
            List<string> sfcList = commands.Select(m => m.Sfc).Distinct().ToList();
            List<string> existSfcList = new List<string>();
            List<ManuSfcEntity> addSfcList = new List<ManuSfcEntity>();
            List<ManuSfcInfoEntity> addSfcInfoList = new List<ManuSfcInfoEntity>();

            ManuSfcStatusQuery sfcQuery = new ManuSfcStatusQuery();
            sfcQuery.SiteId = (long)commands[0].SiteId!;
            sfcQuery.Sfcs = sfcList;
            var dbList = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(sfcQuery);
            //if (dbList == null || dbList.Count() == 0)
            //{
            //    return 0;
            //}
            List<string> dbSfcList = dbList.Select(m => m.SFC).ToList();

            List<ManuSfcUpdateStatusCommand> updateSfcList = new List<ManuSfcUpdateStatusCommand>();
            foreach (var item in commands)
            {
                if (existSfcList.Contains(item.Sfc) == true)
                {
                    continue;
                }
                existSfcList.Add(item.Sfc);
                if (dbSfcList.Contains(item.Sfc) == true) //更新
                {
                    updateSfcList.Add(item);
                }
                else //插入
                {
                    var sfcEntity = new ManuSfcEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SFC = item.Sfc,
                        Qty = 1,
                        SiteId = (long)item.SiteId!,
                        ScrapQty = 0,
                        Status = (SfcStatusEnum)item.Status,
                        CreatedBy = item.UserId,
                        UpdatedBy = item.UserId
                    };
                    addSfcList.Add(sfcEntity);
                    addSfcInfoList.Add(new ManuSfcInfoEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SfcId = sfcEntity.Id,
                        ProcessRouteId = item.WorkOrder?.ProcessRouteId,
                        ProductBOMId = item.WorkOrder?.ProductBOMId,
                        ProductId = item.WorkOrder == null ? 0 : item.WorkOrder.ProductId,
                        WorkOrderId = item.WorkOrder?.Id,
                        SiteId = (long)item.SiteId,
                        IsUsed = true,
                        CreatedBy = item.UserId,
                        UpdatedBy = item.UserId
                    });
                }
            }

            await _manuSfcRepository.InsertRangeAsync(addSfcList);
            await _manuSfcRepository.ManuSfcUpdateStatusBySfcsAsync(updateSfcList);
            await _manuSfcInfoRepository.InsertsAsync(addSfcInfoList);

            return sfcList.Count;
        }

        /// <summary>
        /// 插入原材料到条码，条码信息表
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        private async Task<int> InsertRawMaterialAsync(List<ManuSfcDto> commands, List<ProcMaterialEntity> mesMaterialList)
        {
            List<string> sfcList = commands.Select(m => m.Sfc).Distinct().ToList();
            List<string> existSfcList = new List<string>();
            List<ManuSfcEntity> addSfcList = new List<ManuSfcEntity>();
            List<ManuSfcInfoEntity> addSfcInfoList = new List<ManuSfcInfoEntity>();

            ManuSfcStatusQuery sfcQuery = new ManuSfcStatusQuery();
            sfcQuery.SiteId = (long)commands[0].SiteId!;
            sfcQuery.Sfcs = sfcList;
            var dbList = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(sfcQuery);
            List<string> dbSfcList = dbList.Select(m => m.SFC).ToList();

            foreach (var item in commands)
            {
                if(item.Sfc == "E3001124ADV29892002001H71400002")
                {
                    VAR_DEBUG = 3;
                }
                if (existSfcList.Contains(item.Sfc) == true)
                {
                    continue;
                }
                existSfcList.Add(item.Sfc);
                if (dbSfcList.Contains(item.Sfc) == true) //已经存在
                {
                    continue;
                }
                //物料如果不为空，则直接取对应物料；如果为空，则取工单的型号
                long ProductId = 0;
                var curMaterial = mesMaterialList.Where(m => m.MaterialCode ==  item.MaterialCode).FirstOrDefault();
                if(curMaterial != null)
                {
                    ProductId = curMaterial.Id;
                }
                else
                {
                    ProductId = item.WorkOrder == null ? 0 : item.WorkOrder.ProductId;
                }
                var sfcEntity = new ManuSfcEntity()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = item.Sfc,
                    Qty = 1,
                    SiteId = (long)item.SiteId!,
                    ScrapQty = 0,
                    Status = SfcStatusEnum.Complete,
                    Type = Core.Enums.Manufacture.SfcTypeEnum.NoProduce,
                    CreatedBy = item.UserId,
                    UpdatedBy = item.UserId
                };
                addSfcList.Add(sfcEntity);
                addSfcInfoList.Add(new ManuSfcInfoEntity()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SfcId = sfcEntity.Id,
                    ProcessRouteId = item.WorkOrder?.ProcessRouteId,
                    ProductBOMId = item.WorkOrder?.ProductBOMId,
                    ProductId = ProductId,
                    WorkOrderId = item.WorkOrder?.Id,
                    SiteId = (long)item.SiteId,
                    IsUsed = true,
                    CreatedBy = item.UserId,
                    UpdatedBy = item.UserId
                });
            }

            int insertNum = await _manuSfcRepository.InsertRangeAsync(addSfcList);
            int updateNum = await _manuSfcInfoRepository.InsertsAsync(addSfcInfoList);

            return addSfcList.Count;
        }

        /// <summary>
        /// 工单数量变更
        /// </summary>
        /// <param name="mesOrderList"></param>
        /// <returns></returns>
        private async Task<int> OrderCompleteQtyChangeAsync(List<PlanWorkOrderEntity> mesOrderList)
        {
            if(mesOrderList == null || mesOrderList.Any() == false)
            {
                return 0;
            }

            //获取工单数量
            List<long> orderIdList = mesOrderList.Select(m => m.Id).ToList();
            WorkOrderQtyQuery query = new WorkOrderQtyQuery();
            query.SiteId = mesOrderList[0].SiteId;
            query.OrderIdList = orderIdList;
            var orderViewQty = await _manuSfcInfoRepository.GetWorkOrderQtyMavelAsync(query,PRODUCRE_END);
            List<long> dbOrderId = orderViewQty.Select(m => m.WorkOrderId).ToList();
            if(dbOrderId.Count == 0)
            {
                return 0;
            }

            //要更新的工单
            List<PlanWorkOrderEntity> updateList = new List<PlanWorkOrderEntity>();
            //要更新的库存
            List<UpdateQuantityResidueBySfcsMavelCommand> whUpdateList = new List<UpdateQuantityResidueBySfcsMavelCommand>();
            foreach(var item in mesOrderList)
            {
                var curOrderView = orderViewQty.Where(m => m.WorkOrderId == item.Id).FirstOrDefault();
                if(curOrderView == null)
                {
                    continue;
                }
                item.Qty = curOrderView.Qty;
                item.UpdatedBy = OperationBy;
                item.UpdatedOn = HymsonClock.Now();
                updateList.Add(item);

                long orderId = item.Id;
                //orderId = 51779354903695360;

                // 查询生产计划 1000000021
                var planWorkPlanEntity = await _planWorkPlanRepository.GetByIdAsync(item.WorkPlanId ?? 0);
                if(planWorkPlanEntity == null)
                {
                    continue;
                }

                // 查询生产计划物料 1000000022
                var planWorkPlanMaterialEntities = await _planWorkPlanMaterialRepository.GetEntitiesByPlanIdAsync(new Data.Repositories.Plan.Query.PlanWorkPlanByPlanIdQuery
                {
                    SiteId = item.SiteId,
                    PlanId = planWorkPlanEntity.Id,
                    PlanProductId = item.WorkPlanProductId ?? 0
                });
                if(planWorkPlanMaterialEntities == null || planWorkPlanMaterialEntities.Count() == 0) 
                {
                    continue;
                }
                //查询库存
                WhMaterialInventoryWorkOrderIdQuery whQuery = new WhMaterialInventoryWorkOrderIdQuery();
                whQuery.SiteId = item.SiteId;
                whQuery.WorkOrderId = orderId;
                var dbWhList = await _whMaterialInventoryRepository.GetByWorkOrderIdAsync(whQuery);
                if(dbWhList == null || dbWhList.Count() == 0)
                {
                    continue;
                }
                foreach(var whItem in dbWhList)
                {
                    var curPlan = planWorkPlanMaterialEntities.Where(m => m.MaterialId == whItem.MaterialId).FirstOrDefault();
                    if(curPlan == null)
                    {
                        continue;
                    }

                    UpdateQuantityResidueBySfcsMavelCommand whUpdateModel = new UpdateQuantityResidueBySfcsMavelCommand();
                    whUpdateModel.Id = whItem.Id;
                    whUpdateModel.UpdatedOn = HymsonClock.Now();
                    whUpdateModel.UpdatedBy = OperationBy;
                    whUpdateModel.QuantityResidue -= (item.Qty * (curPlan.Usages + curPlan.Loss));
                    if(whUpdateModel.QuantityResidue < 0)
                    {
                        whUpdateModel.QuantityResidue = 0;
                    }
                    whUpdateList.Add(whUpdateModel);
                }
            }

            int result = 0;
            //更新工单物料库存数量
            result += await _planWorkOrderRepository.UpdatesCompleteQtyMavleAsync(updateList);
            result += await _whMaterialInventoryRepository.UpdateQuantityResidueRangeMavelAsync(whUpdateList);

            return result;
        }
               
        /// <summary>
        /// 插入或者更新转子线条码
        /// </summary>
        /// <param name="addList"></param>
        /// <param name="updateList"></param>
        /// <returns></returns>
        private async Task<int> InsertOrUpdateRotorSfcAsync(List<ManuRotorSfcEntity> addList,List<ManuRotorSfcEntity> updateList)
        {
            int result = 0;

            if(addList != null && addList.Count > 0)
            {
                result = await _manuRotorSfcRepository.InsertsAsync(addList);
            }
            
            if(updateList != null && updateList.Count > 0)
            {
                result += await _manuRotorSfcRepository.UpdatesAsync(updateList);
            }
            return 0;
        }

        /// <summary>
        /// LMS条码转换
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="upList"></param>
        /// <param name="produceCode"></param>
        /// <param name="mesOrder"></param>
        /// <returns></returns>
        private ManuRotorSfcEntity? LmsSfcChange(string sfc, List<SfcUpMatDto> upList,
            string produceCode, PlanWorkOrderEntity? mesOrder, string productCode)
        {
            if(upList == null || upList.Any() == false)
            {
                return null;
            }

            ManuRotorSfcEntity model = new ManuRotorSfcEntity();
            model.Id = IdGenProvider.Instance.CreateId();
            model.CreatedOn = HymsonClock.Now();
            model.CreatedBy = OperationBy;
            model.UpdatedOn = model.CreatedOn;
            model.UpdatedBy = OperationBy;
            model.IsDeleted = 0;
            model.SiteId = SiteID;

            if (produceCode == PRODUCRE_Z_TX )
            {
                model.SfcMaterialCode = productCode;
                model.TxSfc = upList[0].BarCode;
                model.TxSfcMaterialCode = upList[0].MatCode;
                model.ZSfc = sfc;
                model.IsFinish = 0;
                model.WorkOrderId = mesOrder == null ? 0 : mesOrder.Id;
            }
            if (produceCode == PRODUCRE_CP_Z)
            {
                model.Sfc = upList[0].BarCode;
                model.ZSfc = sfc;
                model.ZSfcMaterialCode = upList[0].MatCode;
                model.IsFinish = 1;
            }
            return model;
        }


        #endregion

        /// <summary>
        /// 获取工位列表
        /// </summary>
        /// <returns></returns>
        private List<WorkPosDto> GetPosNoList()
        {
            // 创建并初始化工位信息列表
            List<WorkPosDto> workPosList = new List<WorkPosDto>
            {
                new WorkPosDto { WorkPosNo = "OP10.1.1.1", WorkPosName = "扫码工位", SortIndex = 1, WorkPosType = 1},
                new WorkPosDto { WorkPosNo = "OP10.1.1.2", WorkPosName = "测高工位", SortIndex = 2, WorkPosType = 2},
                new WorkPosDto { WorkPosNo = "OP20.1.1.1", WorkPosName = "1号插小磁钢工位", SortIndex = 20 ,WorkPosType = 1},
                new WorkPosDto { WorkPosNo = "OP20.1.1.2", WorkPosName = "1号插大磁钢工位", SortIndex = 21 ,WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP20.1.2.1", WorkPosName = "2号插小磁钢工位", SortIndex = 22 , WorkPosType = 1},
                new WorkPosDto { WorkPosNo = "OP20.1.2.2", WorkPosName = "2号插大磁钢工位", SortIndex = 23 , WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP20.1.3.1", WorkPosName = "3号插小磁钢工位", SortIndex = 24 , WorkPosType = 1},
                new WorkPosDto { WorkPosNo = "OP20.1.3.2", WorkPosName = "3号插大磁钢工位", SortIndex = 25 , WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP20.1.4.1", WorkPosName = "磁钢检测工位", SortIndex = 26 , WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP20.1.5.1", WorkPosName = "碟片下料工位", SortIndex = 27 , WorkPosType = 2},
                new WorkPosDto { WorkPosNo = "OP30.1.1.1", WorkPosName = "堆叠工位", SortIndex = 30 , WorkPosType = 3},
                new WorkPosDto { WorkPosNo = "OP40.1.1.1", WorkPosName = "1号加热工位", SortIndex = 40 , WorkPosType = 3},
                new WorkPosDto { WorkPosNo = "OP40.1.2.1", WorkPosName = "2号加热工位", SortIndex = 41 , WorkPosType = 3},
                new WorkPosDto { WorkPosNo = "OP40.1.3.1", WorkPosName = "3号加热工位", SortIndex = 42 ,WorkPosType=4},
                new WorkPosDto { WorkPosNo = "OP50.1.1.1", WorkPosName = "1号注塑工位", SortIndex = 50 ,WorkPosType = 3},
                new WorkPosDto { WorkPosNo = "OP50.1.2.1", WorkPosName = "2号注塑工位", SortIndex = 51 , WorkPosType = 3},
                new WorkPosDto { WorkPosNo = "OP55.1.1.1", WorkPosName = "注塑凸点检测工位", SortIndex = 55 , WorkPosType = 3},
                new WorkPosDto { WorkPosNo = "OP55.1.1.6", WorkPosName = "人工返工工位", SortIndex = 56 , WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP710.1.1.1", WorkPosName = "轴上料工位", SortIndex = 70 , WorkPosType = 1},
                new WorkPosDto { WorkPosNo = "OP710.1.3.1", WorkPosName = "上平衡盘压装工位", SortIndex = 71 , WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP710.1.4.1", WorkPosName = "下平衡盘压装工位", SortIndex = 72 , WorkPosType = 0},
                new WorkPosDto { WorkPosNo = "OP710.1.4.2", WorkPosName = "热套环压装", SortIndex = 73 , WorkPosType = 2},
                new WorkPosDto { WorkPosNo = "OP720.1.2.1", WorkPosName = "轴环压装工位", SortIndex = 76 , WorkPosType = 3},
                new WorkPosDto { WorkPosNo = "OP80.1.1.1", WorkPosName = "冷却工位", SortIndex = 80 ,WorkPosType = 3},
                new WorkPosDto { WorkPosNo = "OP90.1.2.1", WorkPosName = "充磁工位", SortIndex = 90 ,WorkPosType = 1},
                new WorkPosDto { WorkPosNo = "OP90.1.4.1", WorkPosName = "表磁工位", SortIndex = 91 ,WorkPosType = 2},
                new WorkPosDto { WorkPosNo = "OP100.1.1.1", WorkPosName = "动平衡工位", SortIndex = 100 ,WorkPosType = 3},
                new WorkPosDto { WorkPosNo = "OP110.1.1.1", WorkPosName = "清洁工位", SortIndex = 110 , WorkPosType = 3},
                new WorkPosDto { WorkPosNo = "OP120.1.1.1", WorkPosName = "全尺寸工位", SortIndex = 120 , WorkPosType = 3},
                new WorkPosDto { WorkPosNo = "OP130.1.1.1", WorkPosName = "激光刻印工位", SortIndex = 130 , WorkPosType = 3},
                new WorkPosDto { WorkPosNo = "OP140.1.1.1", WorkPosName = "涂油工位", SortIndex = 140 , WorkPosType = 3},
                new WorkPosDto { WorkPosNo = "OP150.1.1.1", WorkPosName = "成品下料工位", SortIndex = 150 , WorkPosType = 3}
            };

            return workPosList;
        }

        /// <summary>  
        /// 将给定日期转换为该日期所在季度的第一天。  
        /// </summary>  
        /// <param name="date">给定的日期。</param>  
        /// <returns>该日期所在季度的第一天。</returns>  
        private string GetFirstDayOfQuarter(DateTime date)
        {
            // 确定月份所在的季度  
            int quarter = (date.Month - 1) / 3 + 1;
            // 计算季度的第一个月份  
            int firstMonthOfQuarter = (quarter - 1) * 3 + 1;
            // 返回该季度的第一天  
            var datetime = new DateTime(date.Year, firstMonthOfQuarter, 1);

            return datetime.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 转为毫秒时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private long GetTimestampInMilliseconds(DateTime ?dateTime)
        {
            if(dateTime == null)
            {
                return 0;
            }

            // 首先将本地时间转换为UTC时间  
            DateTime utcDateTime = ((DateTime)dateTime).ToUniversalTime();
            // 然后计算UTC时间与Unix纪元（1970年1月1日UTC）之间的差值  
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = utcDateTime - epoch;
            return (long)timeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// 将Unix时间戳（毫秒）转换为DateTime  
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private DateTime UnixTimestampMillisToDateTime(long timestamp)
        {
            // 将Unix时间戳转换为UTC DateTime  
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime utcDateTime = epoch.AddMilliseconds(timestamp);
            // 然后将UTC DateTime转换为本地时间  
            DateTime localDateTime = utcDateTime.ToLocalTime();
            return localDateTime;
        }

        /// <summary>
        /// lms工单复线转换
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        private string LmsOrderFxChange(string orderCode)
        {
            return orderCode.Replace("FX", "").Replace("XF","").Replace("XFFX", "");
        }
    }
}
