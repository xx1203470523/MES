using FluentValidation;
using Hymson.EventBus.Abstractions;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Quality;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility;
using Hymson.MES.CoreServices.Events.ProcessEvents.PrintEvents;
using Hymson.MES.CoreServices.Services.Common;

using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.Snowflake;
using Hymson.Utils;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        public FQCAutoCreateService(IQualFinallyOutputRecordRepository qualFinallyOutputRecordRepository
            , IQualFinallyOutputRecordDetailRepository qualFinallyOutputRecordDetailRepository
            , IQualFqcParameterGroupDetailRepository qualFqcParameterGroupDetailRepository
            , IQualFqcParameterGroupRepository qualFqcParameterGroupRepository
            , IMasterDataService masterDataService)
        {
            _qualFinallyOutputRecordDetailRepository = qualFinallyOutputRecordDetailRepository;
            _qualFinallyOutputRecordRepository = qualFinallyOutputRecordRepository;
            _masterDataService = masterDataService; 
            _qualFqcParameterGroupDetailRepository = qualFqcParameterGroupDetailRepository;
            _qualFqcParameterGroupRepository = qualFqcParameterGroupRepository;
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
            #region 先写入成品条码产出记录表
           
            foreach (var group in commonBo.OutStationRequestBos.GroupBy(g=>g.VehicleCode))
            {
                var first = group.FirstOrDefault();
                if(first == null) continue;
                var sfcproduce = sfcProduceEntities.FirstOrDefault(s => s.SFC == first.SFC)
                      ?? throw new CustomerValidationException(nameof(ErrorCode.MES17415)).WithData("SFC", first.SFC); ;
                var recordEntity = new QualFinallyOutputRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = commonBo.SiteId,
                    MaterialId = sfcproduce.ProductId,
                    WorkOrderId = sfcproduce.WorkOrderId,
                    WorkCenterId = sfcproduce.WorkCenterId,
                    Barcode = group.Key??string.Empty,
                    CodeType = commonBo.Type switch
                    {
                        ManuFacePlateBarcodeTypeEnum.Vehicle => FQCLotUnitEnum.Tray,
                        ManuFacePlateBarcodeTypeEnum.Product => FQCLotUnitEnum.EA

                    },

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


            return responseBo;
        }

        public async Task VerifyParamAsync<T>(T param) where T : JobBaseBo
        {
            var jobRequestBo = param as JobRequestBo;
            if (jobRequestBo == null)
            {
                return;
            }
            var fQCOrderAutoCreateAutoBo = jobRequestBo.FQCOrderAutoCreateAutoBo;
            if (fQCOrderAutoCreateAutoBo == null) return;

        }
    }
}
