using Google.Protobuf.WellKnownTypes;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.BackgroundServices.Rotor.Dtos.Manu;
using Hymson.MES.BackgroundServices.Rotor.Entity;
using Hymson.MES.BackgroundServices.Rotor.Repositories;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using Hymson.WaterMark;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
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
        public readonly IWaterMarkService _waterMarkService;

        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteID = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuInOutBoundService(
            IWorkItemInfoRepository workItemInfoRepository,
            IWorkProcessDataRepository workProcessDataRepository,
            IWorkProcessRepository workProcessRepository,
            IWorkOrderRelationRepository workOrderRelationRepository,
            IWorkOrderListRepository workOrderListRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IManuSfcCirculationRepository manuSfcCirculationRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            ISysConfigRepository sysConfigRepository,
            IWaterMarkService waterMarkService)
        {
            _workItemInfoRepository = workItemInfoRepository;
            _workProcessDataRepository = workProcessDataRepository;
            _workProcessRepository = workProcessRepository;
            _workOrderRelationRepository = workOrderRelationRepository;
            _workOrderListRepository = workOrderListRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuSfcCirculationRepository = manuSfcCirculationRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _sysConfigRepository = sysConfigRepository;
            _waterMarkService = waterMarkService;
        }

        /// <summary>
        /// 进站状态
        /// </summary>
        private readonly string ProductStatus_In = "S";

        /// <summary>
        /// 出站状态
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
        public async Task InOutBoundAsync(DateTime start, int rows = 200)
        {
            string busKey = "MavelRatorTotal";
            long waterMarkId = await _waterMarkService.GetWaterMarkAsync(busKey);
            DateTime startWaterMarkTime = DateTime.Now;
            if(waterMarkId != 0)
            {
                startWaterMarkTime = DateTimeOffset.FromUnixTimeMilliseconds(waterMarkId).DateTime;
            }
            else
            {
                startWaterMarkTime = DateTime.Parse("2024-04-01 01:01:01");
            }
            start = startWaterMarkTime;

            //TODO
            //1. 中间工序不管进出站，但是参数需要记录保存，上料信息需要记录
            //2. 进站工序的上料，直接查出来
            //3. 710工序当作芯轴上料工序，铁芯码和轴码绑定，轴码和最上面的铁芯码绑定关系在Work_ItemInfo里已经有
            //4. 中间工序NG后，后面不会在有进出站记录
            //5. 进站不会NG，出站才有NG
            //6. 进站没有参数上传，没有关联数据

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
            List<string> lmsOrderList = productOrderList.Select(m => m.WorkNo).Distinct().ToList();
            #endregion

            #region 获取MES数据
            //MES工单数据
            List<PlanWorkOrderEntity> mesOrderList = await GetMesOrderAsync(lmsOrderList);
            #endregion

            //MES过站数据
            List<MesOutDto> mesList = new List<MesOutDto>();

            #region 线体LMS数据转成MES数据
            //过滤MES不需要的数据
            //List<WorkProcessDto> mesList = inOutList.Where(m => workPosCodeList.Contains(m.WorkPosNo) == true).ToList();
            //依次处理每个条码的数据
            foreach(var item in inOutList)
            {
                //获取工位类型
                WorkPosDto? curWorkPos = workPosList.Where(m => item.WorkPosNo == m.WorkPosNo).FirstOrDefault();
                if(curWorkPos == null)
                {
                    continue;
                }

                //基础数据
                MesOutDto mesDto = new MesOutDto();
                mesDto.Sfc = item.ProductNo;
                mesDto.ProcedureCode = item.ProcedureCode;
                mesDto.IsPassed = item.Result == Result_OK ? true : false;
                mesDto.Date = item.CreateTime;
                WorkOrderListDto? curProductOrder = productOrderList.Where(m => m.ProductNo == item.ProductNo).FirstOrDefault();
                if(curProductOrder != null)
                {
                    mesDto.OrderCode = curProductOrder.WorkNo;
                }

                //处理上料信息
                List<WorkItemInfoDto> curUpMatList = upMatList.Where(m => m.ProcessUID == item.ID).ToList();
                List<SfcUpMatDto> sfcUpList = new List<SfcUpMatDto>();
                if (curUpMatList != null && curUpMatList.Count > 0)
                {
                    foreach (var upItem in curUpMatList)
                    {
                        sfcUpList.Add(new SfcUpMatDto()
                        {
                            MatName = upItem.MatName,
                            MatValue = upItem.MatValue,
                            MatBatchCode = upItem.MatBatchCode,
                            BarCode = string.IsNullOrEmpty(upItem.MatValue) ? upItem.MatBatchCode : upItem.MatValue,
                            MatType = string.IsNullOrEmpty(upItem.MatValue) ? MatType_One : MatType_Batch,
                            MatNum = upItem.MatNum
                        });
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
                if ((curWorkPos.WorkPosType & 1) == 1 && item.ProductStatus == ProductStatus_In)
                {
                    mesDto.Type = 1;
                }
                //NG出站（中间工位可能会NG,NG在后续不会在有记录，当成出站处理）
                if (mesDto.IsPassed == false)
                {
                    mesDto.Type = 2;
                    mesDto.ParamToNgList();
                }
                //正常出站
                if ((curWorkPos.WorkPosType & 2) == 2 && item.ProductStatus == ProductStatus_Out && mesDto.IsPassed == true)
                {
                    //OP710工序在出站的时候把铁芯码和轴码绑定，把铁芯码当成轴码上料记录来处理
                    //if(mesDto.ProcedureCode == "OP710")
                    //{
                    //    List<WorkOrderRelationDto> op710List = bindList.Where(m => m.ProductNo == item.ProductNo).ToList();
                    //    if(op710List != null && op710List.Count > 0)
                    //    {
                    //        mesDto.UpMatList.AddRange(op710List.Select(m => new SfcUpMatDto()
                    //        {
                    //            BarCode = m.ProductNo,
                    //            MatType = 1,
                    //            MatNum = 1
                    //        }));
                    //    }
                    //}
                    mesDto.Type = 2;
                }

                mesList.Add(mesDto);
            }

            #endregion

            List<ManuSfcCirculationEntity> circulaList = new List<ManuSfcCirculationEntity>();
            List<ManuSfcStepEntity> stepList = new List<ManuSfcStepEntity>();

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
                var mesOrder = mesOrderList.Where(m => m.OrderCode == mesItem.OrderCode).FirstOrDefault();
                //写入到步骤表
                if(mesItem.Type == 1 || mesItem.Type == 2)
                {
                    ManuSfcStepEntity step = GetStepEntity(mesItem.Sfc, mesItem.Type, mesItem.ProcedureCode, mesOrder);
                    stepList.Add(step);
                }
                //写入到流转表ManuSfcCirculationEntity
                if (mesItem.UpMatList.Count > 0)
                {
                    List<ManuSfcCirculationEntity> curCirculaList = GetCirculaList(mesItem.Sfc, mesItem.UpMatList,
                        mesItem.ProcedureCode, mesOrder);
                    circulaList.AddRange(curCirculaList);
                }
                //写入到参数表
                if(mesItem.ParamList.Count > 0)
                {

                }
                //写入到NG表
                if(mesItem.NgList.Count > 0)
                {

                }
            }

            #endregion

            //MES数据入库
            using var trans = TransactionHelper.GetTransactionScope();

            await _manuSfcCirculationRepository.InsertRangeAsync(circulaList);
            await _manuSfcStepRepository.InsertRangeMavleAsync(stepList);
            DateTime maxCreateTime = inOutList.Max(x => x.CreateTime);
            string maxCreateTimeStr = maxCreateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            long timestamp = (DateTimeOffset.Parse(maxCreateTimeStr)).ToUnixTimeMilliseconds();
            rows += await _waterMarkService.RecordWaterMarkAsync(busKey, timestamp);

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
                AND CreateTime > '{start.ToString("yyyy-MM-dd HH:mm:ss.000")}'
                ORDER BY CreateTime ASC;
            ";

            List<WorkProcessDto> dbList = await _workProcessRepository.GetList(sql);
            if(dbList == null || dbList.Count == 0)
            {
                return resultList;
            }

            //resultList = dbList.Cast<WorkProcessDto>().ToList();
            //resultList = dbList.OfType<WorkProcessDto>().ToList();
            //resultList = dbList.Select(m => m as WorkProcessDto).ToList();

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

            //resultLit = allDbList.OfType<WorkProcessDataDto>().ToList();

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

            //resultLit = allDbList.OfType<WorkOrderRelationDto>().ToList();

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
        /// <returns></returns>
        private ManuSfcStepEntity GetStepEntity(string sfc, int type, string produceCode,
            PlanWorkOrderEntity ?mesOrder)
        {
            ManuSfcStepEntity step = new ManuSfcStepEntity();
            step.Id = IdGenProvider.Instance.CreateId();
            step.SiteId = SiteID;
            step.SFC = sfc;
            step.Qty = 1;
            step.Remark = produceCode; //备注字段存放工序，用于NIO直接取
            step.Operatetype = Core.Enums.Manufacture.ManuSfcStepTypeEnum.Receive;
            step.CurrentStatus = type == 1 ? Core.Enums.SfcStatusEnum.Activity : Core.Enums.SfcStatusEnum.lineUp;
            step.AfterOperationStatus = type == 1 ? Core.Enums.SfcStatusEnum.lineUp : Core.Enums.SfcStatusEnum.Activity;
            step.UpdatedBy = "";

            if(mesOrder != null)
            {
                step.ProductBOMId = mesOrder.ProductBOMId;
                step.ProcessRouteId = mesOrder.ProcessRouteId;
                step.ProductId = mesOrder.ProductId;
                step.WorkCenterId = mesOrder.WorkCenterId;
                step.WorkOrderId = mesOrder.Id;
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
        /// <returns></returns>
        private List<ManuSfcCirculationEntity> GetCirculaList(string sfc, List<SfcUpMatDto> upList,
            string produceCode, PlanWorkOrderEntity ?mesOrder)
        {
            List<ManuSfcCirculationEntity> list = new List<ManuSfcCirculationEntity>();

            foreach(var item in upList)
            {
                ManuSfcCirculationEntity model = new ManuSfcCirculationEntity();
                model.Id = IdGenProvider.Instance.CreateId();
                model.SiteId = SiteID;
                model.SFC = sfc;
                model.CirculationBarCode = item.BarCode;
                model.CirculationQty = item.MatNum;
                model.Location = produceCode;
                model.UpdatedBy = "";

                if (mesOrder != null)
                {
                    model.ProductId = mesOrder.ProductId;
                    model.WorkOrderId = mesOrder.Id;
                    model.CirculationWorkOrderId = mesOrder.Id;
                    model.CirculationMainProductId = mesOrder.ProductId;
                    model.CirculationProductId = mesOrder.ProductId;
                }

                list.Add(model);
            }

            return list;
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
    }
}
