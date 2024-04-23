using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Job;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Bos.Quality;
using Hymson.MES.CoreServices.Events.Quality;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.Snowflake;
using Hymson.Utils;
using Mysqlx.Crud;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// FQCAutoCreateJOB
    /// </summary>
    [Job("FQC自动创建检验单", JobTypeEnum.Standard)]
    public class FQCAutoCreateService : IJobService
    {
        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;
        private readonly IQualFinallyOutputRecordRepository _qualFinallyOutputRecordRepository;
        private readonly IQualFinallyOutputRecordDetailRepository _qualFinallyOutputRecordDetailRepository;
        private readonly IQualFqcParameterGroupDetailRepository _qualFqcParameterGroupDetailRepository;
        private readonly IQualFqcParameterGroupRepository _qualFqcParameterGroupRepository;
        /// <summary>
        /// 事件总线
        /// </summary>
        private readonly IEventBus<EventBusInstance2> _eventBus;

        public FQCAutoCreateService(IQualFinallyOutputRecordRepository qualFinallyOutputRecordRepository
            , IQualFinallyOutputRecordDetailRepository qualFinallyOutputRecordDetailRepository
            , IQualFqcParameterGroupDetailRepository qualFqcParameterGroupDetailRepository
            , IQualFqcParameterGroupRepository qualFqcParameterGroupRepository
            , IEventBus<EventBusInstance2> eventBus
            , IMasterDataService masterDataService)
        {
            _qualFinallyOutputRecordDetailRepository = qualFinallyOutputRecordDetailRepository;
            _qualFinallyOutputRecordRepository = qualFinallyOutputRecordRepository;
            _masterDataService = masterDataService;
            _qualFqcParameterGroupDetailRepository = qualFqcParameterGroupDetailRepository;
            _qualFqcParameterGroupRepository = qualFqcParameterGroupRepository;
            _eventBus = eventBus;
        }

        public async Task<IEnumerable<JobBo>?> AfterExecuteAsync<T>(T param) where T : JobBaseBo
        {
            return null;
        }

        public async Task<IEnumerable<JobBo>?> BeforeExecuteAsync<T>(T param) where T : JobBaseBo
        {

            return null;
        }

        public async Task<object?> DataAssemblingAsync<T>(T param) where T : JobBaseBo
        {

            if (param is not JobRequestBo commonBo) return default;
            if (commonBo == null) return default;
            if (commonBo.OutStationRequestBos == null || !commonBo.OutStationRequestBos.Any()) return default;

            // 临时中转变量
            var multiSFCBo = new MultiSFCBo { SiteId = commonBo.SiteId, SFCs = commonBo.OutStationRequestBos.Select(s => s.SFC) };

            // 获取生产条码信息
            var sfcProduceEntities = await commonBo.Proxy!.GetDataBaseValueAsync(_masterDataService.GetProduceEntitiesBySFCsAsync, multiSFCBo);
            if (sfcProduceEntities == null || !sfcProduceEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", string.Join(',', multiSFCBo.SFCs));
            }

            //未生成
            var waitSendRecordList = new List<QualFinallyOutputRecordEntity>();
            //生成容量
            var lotSizeDict = new Dictionary<long, int>();
            //同工单
            var _isSameWorkOrder = false;
            //同产线
            var _isSameWorkCenter = false;
            //产品分组
            var resultsfcProduceEntitys = sfcProduceEntities
                                            .Select(x => new MaterialBo { ProductId = x.ProductId, WorkOrderId = x.WorkOrderId, WorkCenterId = x.WorkCenterId })
                                            .GroupBy(g => g.ProductId)
                                            .Select(group => group.First()); // 这里选择每个分组中的第一个元素，以消除重复项
            //循环取MATERAIL
            foreach (var item in resultsfcProduceEntitys)
            {
                var parameterGroupEntity = await _qualFqcParameterGroupRepository.GetEntityAsync(new QualFqcParameterGroupQuery
                {
                    SiteId = commonBo.SiteId,
                    MaterialId = item.ProductId,
                    Status = SysDataStatusEnum.Enable
                });

                if (parameterGroupEntity == null)
                {
                    continue;
                }

                //获取所有检验项目明细
                var parameterGroupDetails = await _qualFqcParameterGroupDetailRepository.GetEntitiesAsync(new QualFqcParameterGroupDetailQuery
                {
                    SiteId = commonBo.SiteId,
                    ParameterGroupId = parameterGroupEntity.Id
                });

                if (parameterGroupDetails == null)
                {
                    continue;
                }

                // 判定是否需要生成FQC
                var queryParam = new QualFinallyOutputRecordQuery
                {
                    SiteId = commonBo.SiteId,
                    MaterialId = item.ProductId,
                    IsGenerated = TrueOrFalseEnum.No
                };

                if (parameterGroupEntity.IsSameWorkOrder == TrueOrFalseEnum.Yes)
                {
                    queryParam.WorkOrderId = item.WorkOrderId;
                    _isSameWorkOrder = true;
                }
                if (parameterGroupEntity.IsSameWorkCenter == TrueOrFalseEnum.Yes)
                {
                    queryParam.WorkCenterId = item.WorkCenterId;
                    _isSameWorkCenter = true;
                }
                var recordList = await _qualFinallyOutputRecordRepository.GetEntitiesAsync(queryParam);
                if (recordList != null)
                {
                    waitSendRecordList.AddRange(recordList);
                    lotSizeDict.Add(item.ProductId, parameterGroupEntity.LotSize);
                }

            }


            // 待执行的命令
            FQCOrderAutoCreateAutoResponse responseBo = new();
            responseBo.QualFinallyOutputRecordDetailEntities = new List<QualFinallyOutputRecordDetailEntity>();
            responseBo.QualFinallyOutputRecords = new List<QualFinallyOutputRecordEntity>();
            responseBo.FQCOrderAutoCreateIntegrationEvent = new FQCOrderAutoCreateIntegrationEvent();
            responseBo.FQCOrderAutoCreateIntegrationEvents = new List<FQCOrderAutoCreateIntegrationEvent>();
            #region 先写入成品条码产出记录表

            foreach (var group in commonBo.OutStationRequestBos.GroupBy(g => g.VehicleCode))
            {
                var first = group.FirstOrDefault();
                if (first == null) continue;
                var sfcproduce = sfcProduceEntities.FirstOrDefault(s => s.SFC == first.SFC)
                      ?? throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", first.SFC);
                if (string.IsNullOrEmpty(group.Key)) //电芯出站
                {
                    var recordEntitys = group.Select(g => new QualFinallyOutputRecordEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = commonBo.SiteId,
                        MaterialId = sfcproduce.ProductId,
                        WorkOrderId = sfcproduce.WorkOrderId,
                        WorkCenterId = sfcproduce.WorkCenterId,
                        Barcode = g.SFC,
                        CodeType = FQCLotUnitEnum.EA,
                        IsGenerated = TrueOrFalseEnum.No,
                        CreatedBy = commonBo.UserName,
                        CreatedOn = commonBo.Time,
                        UpdatedBy = commonBo.UserName,
                        UpdatedOn = commonBo.Time
                    });

                    responseBo.QualFinallyOutputRecords.AddRange(recordEntitys);
                }
                else //包装出站
                {
                    var recordEntity = new QualFinallyOutputRecordEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = commonBo.SiteId,
                        MaterialId = sfcproduce.ProductId,
                        WorkOrderId = sfcproduce.WorkOrderId,
                        WorkCenterId = sfcproduce.WorkCenterId,
                        Barcode = group.Key ?? string.Empty,
                        CodeType = FQCLotUnitEnum.Tray,
                        IsGenerated = TrueOrFalseEnum.Yes,
                        CreatedBy = commonBo.UserName,
                        CreatedOn = commonBo.Time,
                        UpdatedBy = commonBo.UserName,
                        UpdatedOn = commonBo.Time
                    };

                    responseBo.QualFinallyOutputRecords.Add(recordEntity);
                    var recordDetailEntities = group.Select(x => new QualFinallyOutputRecordDetailEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = commonBo.SiteId,
                        OutputRecordId = recordEntity.Id,
                        Barcode = x.SFC,
                        WorkOrderId = sfcproduce.WorkOrderId,
                        WorkCenterId = sfcproduce.WorkCenterId,
                        CreatedBy = commonBo.UserName,
                        CreatedOn = commonBo.Time,
                        UpdatedBy = commonBo.UserName,
                        UpdatedOn = commonBo.Time
                    }).ToList();
                    responseBo.QualFinallyOutputRecordDetailEntities.AddRange(recordDetailEntities);

                }

            }
            if (responseBo.QualFinallyOutputRecords != null && responseBo.QualFinallyOutputRecords.Any())
            {
                // 使用LINQ查询获取每个MaterialId的数量
                var materialIdCounts = responseBo.QualFinallyOutputRecords
                    .GroupBy(record => record.MaterialId)
                    .Select(group => new { MaterialId = group.Key, Count = group.Count() });


                //每个MaterialId的数量
                foreach (var item in materialIdCounts)
                {
                    //当前产品容量
                    lotSizeDict.TryGetValue(item.MaterialId, out var lotsize);
                    //记录数量
                    var currentMaterial = waitSendRecordList.Where(x => x.MaterialId == item.MaterialId);

                    var fqcFromData = waitSendRecordList.Select(q => new FQCOrderAutoCreateIntegration
                    {
                        Barcode = q.Barcode,
                        CodeType = q.CodeType,
                        Id = q.Id,
                        MaterialId = q.MaterialId,
                        Remark = q.Remark,
                        WorkCenterId = q.WorkCenterId,
                        WorkOrderId = q.WorkOrderId,
                    });

                    var fqcDataFromJob = responseBo.QualFinallyOutputRecords.Select(q => new FQCOrderAutoCreateIntegration
                    {
                        Barcode = q.Barcode,
                        CodeType = q.CodeType,
                        Id = q.Id,
                        MaterialId = q.MaterialId,
                        Remark = q.Remark,
                        WorkCenterId = q.WorkCenterId,
                        WorkOrderId = q.WorkOrderId,
                    });


                    if (fqcFromData != null)
                    {
                        fqcDataFromJob = fqcDataFromJob.Concat(fqcFromData);
                    }

                    //同工单 同产线校验
                    if (_isSameWorkOrder && _isSameWorkCenter)
                    {
                       var groupedOrders = fqcDataFromJob.GroupBy(order => new  { order.WorkOrderId,order.WorkCenterId });
                        foreach (var groupedOrder in groupedOrders)
                        {
                            if (groupedOrder.Count() > lotsize)
                            {
                                var fqcevent = new FQCOrderAutoCreateIntegrationEvent
                                {
                                    SiteId = commonBo.SiteId,
                                    UserName = commonBo.UserName,
                                    RecordDetails = fqcDataFromJob,
                                };

                                responseBo.FQCOrderAutoCreateIntegrationEvents.Add(fqcevent);
                            }
                        }

                    }

                    //同工单校验
                    if (_isSameWorkOrder && !_isSameWorkCenter)
                    {

                        var groupedOrders = fqcDataFromJob.GroupBy(order => new { order.WorkOrderId });
                        foreach (var groupedOrder in groupedOrders)
                        {
                            if (groupedOrder.Count() > lotsize)
                            {
                                var fqcevent = new FQCOrderAutoCreateIntegrationEvent
                                {
                                    SiteId = commonBo.SiteId,
                                    UserName = commonBo.UserName,
                                    RecordDetails = fqcDataFromJob,
                                };

                                responseBo.FQCOrderAutoCreateIntegrationEvents.Add(fqcevent);
                            }
                        }
                    }

                    //同产线
                    if (!_isSameWorkOrder & _isSameWorkCenter)
                    {
                        var groupedOrders = fqcDataFromJob.GroupBy(order => new  { order.WorkCenterId });
                        foreach (var groupedOrder in groupedOrders)
                        {
                            if (groupedOrder.Count() > lotsize)
                            {
                                var fqcevent = new FQCOrderAutoCreateIntegrationEvent
                                {
                                    SiteId = commonBo.SiteId,
                                    UserName = commonBo.UserName,
                                    RecordDetails = fqcDataFromJob,
                                };

                                responseBo.FQCOrderAutoCreateIntegrationEvents.Add(fqcevent);
                            }
                        }

                    }


                    //混线
                    if (!_isSameWorkOrder & !_isSameWorkCenter)
                    {
                        if (fqcDataFromJob.Count() > lotsize)
                        {
                            var fqcevent = new FQCOrderAutoCreateIntegrationEvent
                            {
                                SiteId = commonBo.SiteId,
                                UserName = commonBo.UserName,
                                RecordDetails = fqcDataFromJob,
                            };

                            responseBo.FQCOrderAutoCreateIntegrationEvents.Add(fqcevent);
                        }
                    }

                    //判断容量是否生成
                    //if ((currentMaterial.Count() + item.Count) >= lotsize)
                    //{
                    //    var fqcFromData = waitSendRecordList.Select(q => new FQCOrderAutoCreateIntegration
                    //    {
                    //        Barcode = q.Barcode,
                    //        CodeType = q.CodeType,
                    //        Id = q.Id,
                    //        MaterialId = q.MaterialId,
                    //        Remark = q.Remark,
                    //        WorkCenterId = q.WorkCenterId,
                    //        WorkOrderId = q.WorkOrderId,
                    //    });

                    //    var fqcDataFromJob = responseBo.QualFinallyOutputRecords.Select(q => new FQCOrderAutoCreateIntegration
                    //    {
                    //        Barcode = q.Barcode,
                    //        CodeType = q.CodeType,
                    //        Id = q.Id,
                    //        MaterialId = q.MaterialId,
                    //        Remark = q.Remark,
                    //        WorkCenterId = q.WorkCenterId,
                    //        WorkOrderId = q.WorkOrderId,
                    //    });


                    //    if (fqcFromData != null)
                    //    {
                    //        fqcDataFromJob = fqcDataFromJob.Concat(fqcFromData);
                    //    }

                    //    //同工单校验
                    //    if (_isSameWorkOrder)
                    //    {
                    //        //是否存在具有不同 WorkOrderId 的元素
                    //        bool hasDiffWorkOrderId = fqcDataFromJob.Select(order => order.WorkOrderId)
                    //                                                .GroupBy(id => id)
                    //                                                .Any(group => group.Count() > 1);
                    //        if (hasDiffWorkOrderId) continue;

                    //    }

                    //    //同产线校验
                    //    if (_isSameWorkCenter)
                    //    {
                    //        //是否存在具有不同 WorkCenterId 的元素
                    //        bool hasDiffWorkCenterId = fqcDataFromJob.Select(order => order.WorkCenterId)
                    //                                                .GroupBy(id => id)
                    //                                                .Any(group => group.Count() > 1);
                    //        if (hasDiffWorkCenterId) continue;

                    //    }

                    //var fqcevent = new FQCOrderAutoCreateIntegrationEvent
                    //{
                    //    SiteId = commonBo.SiteId,
                    //    UserName = commonBo.UserName,
                    //    RecordDetails = fqcDataFromJob,
                    //};

                    //responseBo.FQCOrderAutoCreateIntegrationEvents.Add(fqcevent);
                    //}

                }

            }


            #endregion
            return responseBo;
        }

        public async Task<JobResponseBo> ExecuteAsync(object obj)
        {
            JobResponseBo responseBo = new();
            if (obj is not FQCOrderAutoCreateAutoResponse data)
            {
                return responseBo;
            }

            responseBo.Rows += await _qualFinallyOutputRecordRepository.InsertRangeAsync(data.QualFinallyOutputRecords);
            responseBo.Rows += await _qualFinallyOutputRecordDetailRepository.InsertRangeAsync(data.QualFinallyOutputRecordDetailEntities);

            if (data.FQCOrderAutoCreateIntegrationEvents != null && data.FQCOrderAutoCreateIntegrationEvents.Any())
            {

                foreach (var item in data.FQCOrderAutoCreateIntegrationEvents)
                {
                    _eventBus.Publish(item);
                }

                #region 更新FinallyOutputRecord
                // 从 FQCOrderAutoCreateIntegrationEvents 中获取所有 FQCOrderAutoCreateIntegration 的 Id 并去重
                var distinctIds = data.FQCOrderAutoCreateIntegrationEvents
                    // 选择所有的 RecordDetails
                    .SelectMany(eventObj => eventObj.RecordDetails ?? Enumerable.Empty<FQCOrderAutoCreateIntegration>())
                    // 选择所有的 Id
                    .Select(detail => detail.Id)
                    // 去除空的 Id
                    .Where(id => id.HasValue)
                    // 去重
                    .Distinct();

                //long?[]转long[]
                long[] recrodids = distinctIds.Where(x => x.HasValue).Select(x => x.Value).ToArray();

                IEnumerable<QualFinallyOutputRecordEntity> recordList = null;

                if (recrodids != null)
                {
                    recordList = await _qualFinallyOutputRecordRepository.GetByIdsAsync(recrodids);
                }

                //更新条码产出记录表
                if (recordList != null)
                {
                    //标记为已生成过检验单
                    foreach (var record in recordList)
                    {
                        record.IsGenerated = TrueOrFalseEnum.Yes;
                        record.UpdatedBy = "FQCOrderAutoJob";
                        record.UpdatedOn = HymsonClock.Now();
                    }
                    await _qualFinallyOutputRecordRepository.UpdateRangeAsync(recordList);
                }
                #endregion
            }


            return responseBo;
        }

        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {

        }
    }

    public class MaterialBo
    {
        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工单id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 工作中心
        /// </summary>
        public long WorkCenterId { get; set; }

    }
}
