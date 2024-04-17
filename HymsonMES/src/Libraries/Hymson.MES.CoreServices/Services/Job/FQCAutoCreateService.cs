using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Quality;
using Hymson.MES.CoreServices.Events.Quality;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.Snowflake;
using System.Data;

namespace Hymson.MES.CoreServices.Services.Job
{
    /// <summary>
    /// FQCAutoCreateJOB
    /// </summary>
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


            // 待执行的命令
            FQCOrderAutoCreateAutoResponse responseBo = new();
            responseBo.QualFinallyOutputRecordDetailEntities = new List<QualFinallyOutputRecordDetailEntity>();
            responseBo.QualFinallyOutputRecords = new List<QualFinallyOutputRecordEntity>();
            responseBo.FQCOrderAutoCreateIntegrationEvent = new FQCOrderAutoCreateIntegrationEvent();
            #region 先写入成品条码产出记录表

            foreach (var group in commonBo.OutStationRequestBos.GroupBy(g => g.VehicleCode))
            {
                var first = group.FirstOrDefault();
                if (first == null) continue;
                var sfcproduce = sfcProduceEntities.FirstOrDefault(s => s.SFC == first.SFC)
                      ?? throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", first.SFC);
                if(string.IsNullOrEmpty(group.Key)) //电芯出站
                {
                    var recordEntitys = group.Select(g=> new QualFinallyOutputRecordEntity
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
                        IsGenerated = TrueOrFalseEnum.No,
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
                responseBo.FQCOrderAutoCreateIntegrationEvent = new FQCOrderAutoCreateIntegrationEvent
                {
                    SiteId = commonBo.SiteId,
                    UserName = commonBo.UserName,
                    RecordDetails = responseBo.QualFinallyOutputRecords.Select(q=>new FQCOrderAutoCreateIntegration
                    {
                        Barcode = q.Barcode,
                        CodeType = q.CodeType,
                        Id = q.Id,
                        MaterialId = q.MaterialId,
                        Remark = q.Remark,
                        WorkCenterId= q.WorkCenterId,
                        WorkOrderId= q.WorkOrderId,
                    })
                };
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
            responseBo.Rows +=await _qualFinallyOutputRecordDetailRepository.InsertRangeAsync(data.QualFinallyOutputRecordDetailEntities);
            if(data.FQCOrderAutoCreateIntegrationEvent != null&&data.FQCOrderAutoCreateIntegrationEvent.RecordDetails.Any())
            {
                _eventBus.PublishDelay(data.FQCOrderAutoCreateIntegrationEvent,3);
            }

            return responseBo;
        }

        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            
        }
    }
}
