using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Bos.Quality;
using Hymson.MES.CoreServices.Events.Quality;
using Hymson.MES.CoreServices.Extension;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.Extensions.Logging;

namespace Hymson.MES.CoreServices.Services.Quality.QualFqcOrder
{
    /// <summary>
    /// FQC检验单创建服务
    /// </summary>
    public class FQCOrderCreateService : IFQCOrderCreateService
    {
        private readonly IQualFqcOrderRepository _qualFqcOrderRepository;
        private readonly IQualFqcOrderSfcRepository _qualFqcOrderSfcRepository;
        private readonly IQualFinallyOutputRecordRepository _qualFinallyOutputRecordRepository;
        private readonly IQualFinallyOutputRecordDetailRepository _qualFinallyOutputRecordDetailRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IQualFqcParameterGroupRepository _qualFqcParameterGroupRepository;
        private readonly IQualFqcParameterGroupDetailRepository _qualFqcParameterGroupDetailRepository;
        private readonly IQualFqcParameterGroupSnapshootRepository _qualFqcParameterGroupSnapshootRepository;
        private readonly IQualFqcParameterGroupDetailSnapshootRepository _qualFqcParameterGroupDetailSnapshootRepository;
        private readonly IProcParameterRepository _procParameterRepository;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;

        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        /// <summary>
        /// 日志对象
        /// </summary>
        private readonly ILogger<FQCOrderCreateService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="qualFqcOrderRepository"></param>
        /// <param name="qualFqcOrderSfcRepository"></param>
        /// <param name="qualFinallyOutputRecordRepository"></param>
        /// <param name="qualFinallyOutputRecordDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="qualFqcParameterGroupRepository"></param>
        /// <param name="qualFqcParameterGroupDetailRepository"></param>
        /// <param name="qualFqcParameterGroupSnapshootRepository"></param>
        /// <param name="qualFqcParameterGroupDetailSnapshootRepository"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="manuGenerateBarcodeService"></param>
        public FQCOrderCreateService(IQualFqcOrderRepository qualFqcOrderRepository,
            IQualFqcOrderSfcRepository qualFqcOrderSfcRepository,
            IQualFinallyOutputRecordRepository qualFinallyOutputRecordRepository,
            IQualFinallyOutputRecordDetailRepository qualFinallyOutputRecordDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IQualFqcParameterGroupRepository qualFqcParameterGroupRepository,
            IQualFqcParameterGroupDetailRepository qualFqcParameterGroupDetailRepository,
            IQualFqcParameterGroupSnapshootRepository qualFqcParameterGroupSnapshootRepository,
            IQualFqcParameterGroupDetailSnapshootRepository qualFqcParameterGroupDetailSnapshootRepository,
            IProcParameterRepository procParameterRepository,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IManuGenerateBarcodeService manuGenerateBarcodeService,
            ILogger<FQCOrderCreateService> logger)
        {
            _qualFqcOrderRepository = qualFqcOrderRepository;
            _qualFqcOrderSfcRepository = qualFqcOrderSfcRepository;
            _qualFinallyOutputRecordRepository = qualFinallyOutputRecordRepository;
            _qualFinallyOutputRecordDetailRepository = qualFinallyOutputRecordDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _qualFqcParameterGroupRepository = qualFqcParameterGroupRepository;
            _qualFqcParameterGroupDetailRepository = qualFqcParameterGroupDetailRepository;
            _qualFqcParameterGroupSnapshootRepository = qualFqcParameterGroupSnapshootRepository;
            _qualFqcParameterGroupDetailSnapshootRepository = qualFqcParameterGroupDetailSnapshootRepository;
            _procParameterRepository = procParameterRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _logger = logger;
        }


        /// <summary>
        /// 检验单手动生成
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<int> ManualCreateAsync(FQCOrderManualCreateBo bo)
        {
            if (bo == null) throw new CustomerValidationException(nameof(ErrorCode.MES10111));
            if (bo.RecordIds == null || !bo.RecordIds.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10111));
            }

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();

            //查询产出记录
            var outputRecords = await _qualFinallyOutputRecordRepository.GetByIdsAsync(bo.RecordIds.ToArray());
            if (outputRecords == null || !outputRecords.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11700));
            }

            //校验是否属于同一物料
            if (outputRecords.Select(x => x.MaterialId).Distinct().Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11701));
            }
            //校验是否已生成过检验单
            if (outputRecords.Any(x => x.IsGenerated == TrueOrFalseEnum.Yes))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11702)).WithData("SFC", string.Join(',', outputRecords.Where(x => x.IsGenerated == TrueOrFalseEnum.Yes).Select(x => x.Barcode)));
            }

            //物料
            var materialEntity = await _procMaterialRepository.GetByIdAsync(outputRecords.First().MaterialId);
            var materialCode = materialEntity?.MaterialCode ?? string.Empty;

            //获取检验项目
            var parameterGroupEntity = await _qualFqcParameterGroupRepository.GetEntityAsync(new QualFqcParameterGroupQuery
            {
                SiteId = bo.SiteId,
                MaterialId = outputRecords.First().MaterialId,
                Status = Core.Enums.SysDataStatusEnum.Enable
            });
            if (parameterGroupEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11705)).WithData("MaterialCode", materialCode);
            }

            //校验是否为同一工单
            if (parameterGroupEntity.IsSameWorkOrder == Core.Enums.TrueOrFalseEnum.Yes)
            {
                //TODO 
                if (outputRecords.Select(x => x.WorkCenterId).Distinct().Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES11703));
                }
            }
            //校验是否为同一产线
            if (parameterGroupEntity.IsSameWorkCenter == Core.Enums.TrueOrFalseEnum.Yes)
            {
                if (outputRecords.Select(x => x.WorkCenterId).Distinct().Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES11704));
                }
            }

            //获取检验项目明细
            var parameterGroupDetails = await _qualFqcParameterGroupDetailRepository.GetEntitiesAsync(new QualFqcParameterGroupDetailQuery
            {
                SiteId = bo.SiteId,
                ParameterGroupId = parameterGroupEntity.Id
            });
            if (parameterGroupDetails == null || !parameterGroupDetails.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11707)).WithData("ParameterGroupCode", parameterGroupEntity.Code);
            }

            #region 组装检验单数据

            //检验项目快照
            var parameterGroupSnapshoot = parameterGroupEntity.ToEntity<QualFqcParameterGroupSnapshootEntity>();
            parameterGroupSnapshoot.Id = IdGenProvider.Instance.CreateId();
            //检验项目明细快照
            var parameterGroupDetailSnapshootList = new List<QualFqcParameterGroupDetailSnapshootEntity>();
            var parameters = await _procParameterRepository.GetByIdsAsync(parameterGroupDetails.Select(x => x.ParameterId).Distinct());
            if (!parameters.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES11709)).WithData("Id", parameterGroupEntity.Code);

            foreach (var parameterGroupDetail in parameterGroupDetails)
            {
                var parameterEntity = parameters.First(x => x.Id == parameterGroupDetail.ParameterId);

                var parameterGroupDetailSnapshoot = parameterGroupDetail.ToEntity<QualFqcParameterGroupDetailSnapshootEntity>();
                parameterGroupDetailSnapshoot.Id = IdGenProvider.Instance.CreateId();
                parameterGroupDetailSnapshoot.ParameterGroupId = parameterGroupSnapshoot.Id;
                parameterGroupDetailSnapshoot.ParameterCode = parameterEntity.ParameterCode;
                parameterGroupDetailSnapshoot.ParameterName = parameterEntity.ParameterName;
                parameterGroupDetailSnapshoot.ParameterDataType = parameterEntity.DataType;
                parameterGroupDetailSnapshoot.ParameterUnit = parameterEntity.ParameterUnit ?? "";
                parameterGroupDetailSnapshootList.Add(parameterGroupDetailSnapshoot);
            }
            //检验单
            var orderEntity = new QualFqcOrderEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = bo.SiteId,
                InspectionOrder = await GenerateFQCOrderCodeAsync(bo.SiteId, bo.UserName),
                GroupSnapshootId = parameterGroupSnapshoot.Id,
                WorkOrderId = outputRecords.First().WorkOrderId,
                MaterialId = parameterGroupEntity.MaterialId,
                SampleQty = parameterGroupEntity.SampleQty,
                Status = InspectionStatusEnum.WaitInspect,
                IsPreGenerated = TrueOrFalseEnum.No,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };
            //检验单包含条码
            List<QualFqcOrderSfcEntity> orderSfcList = new();
            if (outputRecords.Any(x => x.CodeType == FQCLotUnitEnum.EA))
            {
                orderSfcList.AddRange(outputRecords.Where(x => x.CodeType == FQCLotUnitEnum.EA).Select(x => new QualFqcOrderSfcEntity
                {
                    Id = x.Id,
                    SiteId = bo.SiteId,
                    FQCOrderId = orderEntity.Id,
                    WorkOrderId = x.WorkOrderId.GetValueOrDefault(),
                    SFC = x.Barcode,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                }));
            }
            else
            {
                var recordDetails = await _qualFinallyOutputRecordDetailRepository.GetEntitiesAsync(new QualFinallyOutputRecordDetailQuery
                {
                    SiteId = bo.SiteId,
                    OutputRecordIds = outputRecords.Where(x => x.CodeType != FQCLotUnitEnum.EA).Select(x => x.Id)
                });
                orderSfcList.AddRange(recordDetails.Select(x => new QualFqcOrderSfcEntity
                {
                    Id = x.Id,
                    SiteId = bo.SiteId,
                    FQCOrderId = orderEntity.Id,
                    WorkOrderId = x.WorkOrderId,
                    SFC = x.Barcode,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                }));
            }

            //标记为已生成过检验单
            foreach (var record in outputRecords)
            {
                record.IsGenerated = TrueOrFalseEnum.Yes;
                record.UpdatedBy = updatedBy;
                record.UpdatedOn = updatedOn;
            }

            #endregion
            //样本数量
            var sfccout = orderSfcList.Count;
            if (sfccout > 0)
            {
                if (sfccout < parameterGroupEntity.SampleQty)
                {
                    orderEntity.SampleQty = sfccout;
                }
            }
            // 保存
            var rows = 0;
            try
            {
                using (var trans = TransactionHelper.GetTransactionScope())
                {
                    rows += await _qualFqcOrderRepository.InsertAsync(orderEntity);
                    rows += await _qualFqcOrderSfcRepository.InsertRangeAsync(orderSfcList);
                    rows += await _qualFqcParameterGroupSnapshootRepository.InsertAsync(parameterGroupSnapshoot);
                    rows += await _qualFqcParameterGroupDetailSnapshootRepository.InsertRangeAsync(parameterGroupDetailSnapshootList);

                    //更新条码产出记录表
                    rows += await _qualFinallyOutputRecordRepository.UpdateRangeAsync(outputRecords);

                    trans.Complete();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Duplicate"))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES11720));
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES11721), ex.Message);
                }

            }

            return rows;
        }

        /// <summary>
        /// 检验单自动生成
        /// </summary>
        /// <param name="bo"></param>
        /// <returns>是否要做FQC检验</returns>
        public async Task<bool> AutoCreateAsync(FQCOrderAutoCreateAutoBo bo)
        {
            var isNeedFQC = false;

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();

            #region 先写入成品条码产出记录表

            var recordEntity = new QualFinallyOutputRecordEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = bo.SiteId,
                MaterialId = bo.MaterialId,
                WorkOrderId = bo.WorkOrderId,
                WorkCenterId = bo.WorkCenterId,
                Barcode = bo.Barcode,
                CodeType = bo.CodeType,
                IsGenerated = TrueOrFalseEnum.No,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };
            var recordDetailEntities = new List<QualFinallyOutputRecordDetailEntity>();
            if (bo.RecordDetails != null && bo.RecordDetails.Any())
            {
                recordDetailEntities = bo.RecordDetails.Select(x => new QualFinallyOutputRecordDetailEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    OutputRecordId = recordEntity.Id,
                    Barcode = x.Barcode,
                    WorkOrderId = x.WorkOrderId,
                    WorkCenterId = x.WorkCenterId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                }).ToList();
            }

            await _qualFinallyOutputRecordRepository.InsertAsync(recordEntity);
            await _qualFinallyOutputRecordDetailRepository.InsertRangeAsync(recordDetailEntities);

            #endregion

            //获取检验项目
            var parameterGroupEntity = await _qualFqcParameterGroupRepository.GetEntityAsync(new QualFqcParameterGroupQuery
            {
                SiteId = bo.SiteId,
                MaterialId = bo.MaterialId,
                Status = SysDataStatusEnum.Enable
            });
            if (parameterGroupEntity == null)
            {
                //TODO 待确认没有维护检验项目时是否要抛出异常
                //var materialCode = (await _procMaterialRepository.GetByIdAsync(bo.MaterialId))?.MaterialCode ?? "";
                //throw new CustomerValidationException(nameof(ErrorCode.MES11706)).WithData("MaterialCode", materialCode);
                return isNeedFQC;
            }

            //获取检验项目明细
            var parameterGroupDetails = await _qualFqcParameterGroupDetailRepository.GetEntitiesAsync(new QualFqcParameterGroupDetailQuery
            {
                SiteId = bo.SiteId,
                ParameterGroupId = parameterGroupEntity.Id
            });

            //判定是否需要生成FQC
            var queryParam = new QualFinallyOutputRecordQuery
            {
                SiteId = bo.SiteId,
                MaterialId = bo.MaterialId,
                IsGenerated = TrueOrFalseEnum.No
            };
            if (parameterGroupEntity.IsSameWorkOrder == TrueOrFalseEnum.Yes)
            {
                queryParam.WorkOrderId = bo.WorkOrderId;
            }
            if (parameterGroupEntity.IsSameWorkCenter == TrueOrFalseEnum.Yes)
            {
                queryParam.WorkCenterId = bo.WorkCenterId;
            }
            var outputRecords = await _qualFinallyOutputRecordRepository.GetEntitiesAsync(queryParam);
            if (outputRecords != null && outputRecords.Count() >= parameterGroupEntity.LotSize)
            {
                isNeedFQC = true;
            }
            else
            {
                isNeedFQC = false;
                return isNeedFQC;
            }

            #region 组装检验单数据

            //检验项目快照
            var parameterGroupSnapshoot = parameterGroupEntity.ToEntity<QualFqcParameterGroupSnapshootEntity>();
            parameterGroupSnapshoot.Id = IdGenProvider.Instance.CreateId();
            //检验项目明细快照
            var parameterGroupDetailSnapshootList = new List<QualFqcParameterGroupDetailSnapshootEntity>();
            var parameters = await _procParameterRepository.GetByIdsAsync(parameterGroupDetails.Select(x => x.ParameterId).Distinct());
            foreach (var parameterGroupDetail in parameterGroupDetails)
            {
                var parameterEntity = parameters.First(x => x.Id == parameterGroupDetail.ParameterId);

                var parameterGroupDetailSnapshoot = parameterGroupDetail.ToEntity<QualFqcParameterGroupDetailSnapshootEntity>();
                parameterGroupDetailSnapshoot.Id = IdGenProvider.Instance.CreateId();
                parameterGroupDetailSnapshoot.ParameterGroupId = parameterGroupSnapshoot.Id;
                parameterGroupDetailSnapshoot.ParameterCode = parameterEntity.ParameterCode;
                parameterGroupDetailSnapshoot.ParameterName = parameterEntity.ParameterName;
                parameterGroupDetailSnapshoot.ParameterDataType = parameterEntity.DataType;
                parameterGroupDetailSnapshoot.ParameterUnit = parameterEntity.ParameterUnit ?? "";
                parameterGroupDetailSnapshootList.Add(parameterGroupDetailSnapshoot);
            }
            //检验单
            var orderEntity = new QualFqcOrderEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = bo.SiteId,
                InspectionOrder = await GenerateFQCOrderCodeAsync(bo.SiteId, bo.UserName),
                GroupSnapshootId = parameterGroupSnapshoot.Id,
                WorkOrderId = bo.WorkOrderId,
                MaterialId = parameterGroupEntity.MaterialId,
                SampleQty = parameterGroupEntity.SampleQty,
                Status = InspectionStatusEnum.WaitInspect,
                IsPreGenerated = TrueOrFalseEnum.No,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };
            //检验单包含条码
            List<QualFqcOrderSfcEntity> orderSfcList = new();
            if (outputRecords.Any(x => x.CodeType == FQCLotUnitEnum.EA))
            {
                orderSfcList.AddRange(outputRecords.Where(x => x.CodeType == FQCLotUnitEnum.EA).Select(x => new QualFqcOrderSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    FQCOrderId = orderEntity.Id,
                    WorkOrderId = x.WorkOrderId.GetValueOrDefault(),
                    SFC = x.Barcode,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                }));
            }
            else
            {
                var recordDetails = await _qualFinallyOutputRecordDetailRepository.GetEntitiesAsync(new QualFinallyOutputRecordDetailQuery
                {
                    SiteId = bo.SiteId,
                    OutputRecordIds = outputRecords.Where(x => x.CodeType != FQCLotUnitEnum.EA).Select(x => x.Id)
                });
                orderSfcList.AddRange(recordDetails.Select(x => new QualFqcOrderSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    FQCOrderId = orderEntity.Id,
                    WorkOrderId = x.WorkOrderId,
                    SFC = x.Barcode,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                }));
            }

            //标记为已生成过检验单
            foreach (var record in outputRecords)
            {
                record.IsGenerated = TrueOrFalseEnum.Yes;
                record.UpdatedBy = updatedBy;
                record.UpdatedOn = updatedOn;
            }

            #endregion

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualFqcOrderRepository.InsertAsync(orderEntity);
                rows += await _qualFqcOrderSfcRepository.InsertRangeAsync(orderSfcList);
                rows += await _qualFqcParameterGroupSnapshootRepository.InsertAsync(parameterGroupSnapshoot);
                rows += await _qualFqcParameterGroupDetailSnapshootRepository.InsertRangeAsync(parameterGroupDetailSnapshootList);

                //更新条码产出记录表
                rows += await _qualFinallyOutputRecordRepository.UpdateRangeAsync(outputRecords);

                trans.Complete();
            }

            return isNeedFQC;
        }

        /// <summary>
        /// 创建Fqc
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public async Task<bool> CreateFqcAsync(FQCOrderAutoCreateIntegrationEvent bo)
        {
            var isNeedFQC = false;
            if (bo.RecordDetails == null)
            {
                _logger.LogError("FQC:检验单明细为空");
                throw new CustomerValidationException(nameof(ErrorCode.MES11717));
            }

            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();
            string inspectionOrder = await GenerateFQCOrderCodeAsync(bo.SiteId, bo.UserName);
            var materialId = bo.RecordDetails.Select(x => x.MaterialId).FirstOrDefault();
            var workOrderId = bo.RecordDetails.Select(x => x.WorkOrderId).FirstOrDefault();

            //获取所有检验项目
            var parameterGroupEntity = await _qualFqcParameterGroupRepository.GetEntityAsync(new QualFqcParameterGroupQuery
            {
                SiteId = bo.SiteId,
                MaterialId = materialId,
                Status = SysDataStatusEnum.Enable
            });

            if (parameterGroupEntity == null)
            {
                _logger.LogError($"qual_fqc_parameter_group,FQC:物料ID={materialId}参数项目为空");
                throw new CustomerValidationException(nameof(ErrorCode.MES11718)).WithData("materialId", materialId);
            }

            //获取所有检验项目明细
            var parameterGroupDetails = await _qualFqcParameterGroupDetailRepository.GetEntitiesAsync(new QualFqcParameterGroupDetailQuery
            {
                SiteId = bo.SiteId,
                ParameterGroupId = parameterGroupEntity.Id
            });

            if (parameterGroupDetails == null)
            {
                _logger.LogError($"qual_fqc_parameter_group,FQC:{parameterGroupEntity.Id}参数项目明细为空");
                throw new CustomerValidationException(nameof(ErrorCode.MES11718)).WithData("materialId", materialId);
            }

            //判定是否需要生成FQC
            //var queryParam = new QualFinallyOutputRecordQuery
            //{
            //    SiteId = bo.SiteId,
            //    MaterialId = materialId,
            //    IsGenerated = TrueOrFalseEnum.No
            //};

            //if (parameterGroupEntity.IsSameWorkOrder == TrueOrFalseEnum.Yes)
            //{
            //    queryParam.WorkOrderId = workOrderId;
            //}
            //if (parameterGroupEntity.IsSameWorkCenter == TrueOrFalseEnum.Yes)
            //{
            //    queryParam.WorkCenterId = bo.WorkCenterId;
            //}

            //IEnumerable<QualFinallyOutputRecordEntity> recordList = null;
            //long[] recrodids = bo.RecordDetails.Select(x => x.Id).Where(id => id.HasValue).Select(id => id.Value).ToArray();
            //if (recrodids != null)
            //{
            //    recordList = await _qualFinallyOutputRecordRepository.GetByIdsAsync(recrodids);
            //}

            var outputRecords = bo.RecordDetails;
            //if (outputRecords != null && outputRecords.Count() >= parameterGroupEntity.LotSize)
            //{
            //    isNeedFQC = true;
            //}
            //else
            //{
            //    _logger.LogError($"FQC记录数量小于批次数量，不予生成");
            //    isNeedFQC = false;
            //    return isNeedFQC;
            //}

            //检验项目快照
            var parameterGroupSnapshoot = parameterGroupEntity.ToEntity<QualFqcParameterGroupSnapshootEntity>();
            parameterGroupSnapshoot.Id = IdGenProvider.Instance.CreateId();

            //检验项目明细快照
            var parameterGroupDetailSnapshootList = new List<QualFqcParameterGroupDetailSnapshootEntity>();
            var parameters = await _procParameterRepository.GetByIdsAsync(parameterGroupDetails.Select(x => x.ParameterId).Distinct());

            foreach (var parameterGroupDetail in parameterGroupDetails)
            {
                var parameterEntity = parameters.First(x => x.Id == parameterGroupDetail.ParameterId);

                var parameterGroupDetailSnapshoot = parameterGroupDetail.ToEntity<QualFqcParameterGroupDetailSnapshootEntity>();
                parameterGroupDetailSnapshoot.Id = IdGenProvider.Instance.CreateId();
                parameterGroupDetailSnapshoot.ParameterGroupId = parameterGroupSnapshoot.Id;
                parameterGroupDetailSnapshoot.ParameterCode = parameterEntity.ParameterCode;
                parameterGroupDetailSnapshoot.ParameterName = parameterEntity.ParameterName;
                parameterGroupDetailSnapshoot.ParameterDataType = parameterEntity.DataType;
                parameterGroupDetailSnapshoot.ParameterUnit = parameterEntity.ParameterUnit ?? "";
                parameterGroupDetailSnapshootList.Add(parameterGroupDetailSnapshoot);
            }


            //检验单
            var orderEntity = new QualFqcOrderEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = bo.SiteId,
                InspectionOrder = inspectionOrder,
                GroupSnapshootId = parameterGroupSnapshoot.Id,
                WorkOrderId = workOrderId,
                MaterialId = parameterGroupEntity.MaterialId,
                SampleQty = parameterGroupEntity.SampleQty,
                Status = InspectionStatusEnum.WaitInspect,
                IsPreGenerated = TrueOrFalseEnum.No,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };

            //检验单包含条码
            List<QualFqcOrderSfcEntity> orderSfcList = new();
            if (outputRecords.Any(x => x.CodeType == FQCLotUnitEnum.EA))
            {
                orderSfcList.AddRange(outputRecords.Where(x => x.CodeType == FQCLotUnitEnum.EA).Select(x => new QualFqcOrderSfcEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    FQCOrderId = orderEntity.Id,
                    WorkOrderId = x.WorkOrderId.GetValueOrDefault(),
                    SFC = x.Barcode,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                }));

            }
            else
            {
                var recordIds = outputRecords.Where(x => x.CodeType != FQCLotUnitEnum.EA).Select(x => x.Id);
                if (recordIds != null)
                {
                    //get from detail
                    var recordDetails = await _qualFinallyOutputRecordDetailRepository.GetEntitiesAsync(new QualFinallyOutputRecordDetailQuery
                    {
                        SiteId = bo.SiteId,
                        OutputRecordIds = (IEnumerable<long>)recordIds
                    });

                    orderSfcList.AddRange(recordDetails.Select(x => new QualFqcOrderSfcEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = bo.SiteId,
                        FQCOrderId = orderEntity.Id,
                        WorkOrderId = x.WorkOrderId,
                        SFC = x.Barcode,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    }));
                }

            } 
 

            //样本数量
            var sfccout = orderSfcList.Count;
            if (sfccout > 0)
            {
                if (sfccout < parameterGroupEntity.SampleQty)
                {
                    orderEntity.SampleQty = sfccout;
                }
            }

            // 保存
            var rows = 0;
            try
            {
                using (var trans = TransactionHelper.GetTransactionScope())
                {
                    rows += await _qualFqcOrderRepository.InsertAsync(orderEntity);
                    rows += await _qualFqcOrderSfcRepository.InsertRangeAsync(orderSfcList);
                    rows += await _qualFqcParameterGroupSnapshootRepository.InsertAsync(parameterGroupSnapshoot);
                    rows += await _qualFqcParameterGroupDetailSnapshootRepository.InsertRangeAsync(parameterGroupDetailSnapshootList);

                    //更新条码产出记录表
                    //if (recordList != null)
                    //{
                    //    //标记为已生成过检验单
                    //    foreach (var record in recordList)
                    //    {
                    //        record.IsGenerated = TrueOrFalseEnum.Yes;
                    //        record.UpdatedBy = updatedBy;
                    //        record.UpdatedOn = updatedOn;
                    //    }
                    //    rows += await _qualFinallyOutputRecordRepository.UpdateRangeAsync(recordList);
                    //}

                    trans.Complete();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FQC生成失败！");
            }

            return isNeedFQC;
        }

        /// <summary>
        /// 检验单号生成
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<string> GenerateFQCOrderCodeAsync(long siteId, string userName)
        {
            _logger.LogError($"SiteId={siteId},FQC检验单号开始生成");

            var codeRules = await _inteCodeRulesRepository.GetListAsync(new InteCodeRulesReQuery
            {
                SiteId = siteId,
                CodeType = Core.Enums.Integrated.CodeRuleCodeTypeEnum.FQC
            });
            if (codeRules == null || !codeRules.Any())
            {
                _logger.LogError($"{siteId},FQC检验单号生成失败：FQC类型编码规则未维护，前往综合-编码规则维护");
                throw new CustomerValidationException(nameof(ErrorCode.MES11710));
            }
            if (codeRules.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11711));
            }

            var orderCodes = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new Bos.Manufacture.ManuGenerateBarcode.GenerateBarcodeBo
            {
                SiteId = siteId,
                UserName = userName,
                CodeRuleId = codeRules.First().Id,
                Count = 1
            });

            return orderCodes.First();
        }
    }
}
