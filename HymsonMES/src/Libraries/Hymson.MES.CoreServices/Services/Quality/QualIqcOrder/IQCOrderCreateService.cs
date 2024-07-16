using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Bos.Quality;
using Hymson.MES.CoreServices.Extension;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Qual;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.CoreServices.Services.Quality
{
    /// <summary>
    /// IQC检验单创建服务
    /// </summary>
    public class IQCOrderCreateService : IIQCOrderCreateService
    {
        private readonly IQualIqcOrderRepository _qualIqcOrderRepository;
        private readonly IQualIqcOrderTypeRepository _qualIqcOrderTypeRepository;
        private readonly IWhSupplierRepository _whSupplierRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IQualIqcInspectionItemRepository _qualIqcInspectionItemRepository;
        private readonly IQualIqcInspectionItemDetailRepository _qualIqcInspectionItemDetailRepository;
        private readonly IQualIqcInspectionItemSnapshotRepository _qualIqcInspectionItemSnapshotRepository;
        private readonly IQualIqcInspectionItemDetailSnapshotRepository _qualIqcInspectionItemDetailSnapshotRepository;
        private readonly IProcParameterRepository _procParameterRepository;
        private readonly IQualIqcLevelRepository _qualIqcLevelRepository;
        private readonly IQualIqcLevelDetailRepository _qualIqcLevelDetailRepository;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;

        private readonly IAQLLevelQueryService _aqlLevelQueryService;
        private readonly IAQLPlanQueryService _aqlPlanQueryService;
        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        private readonly ISequenceService _sequenceService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="qualIqcOrderRepository"></param>
        /// <param name="qualIqcOrderTypeRepository"></param>
        /// <param name="whSupplierRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="qualIqcInspectionItemRepository"></param>
        /// <param name="qualIqcInspectionItemDetailRepository"></param>
        /// <param name="qualIqcInspectionItemSnapshotRepository"></param>
        /// <param name="qualIqcInspectionItemDetailSnapshotRepository"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="qualIqcLevelRepository"></param>
        /// <param name="qualIqcLevelDetailRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="aqlLevelQueryService"></param>
        /// <param name="aqlPlanQueryService"></param>
        /// <param name="manuGenerateBarcodeService"></param>
        /// <param name="sequenceService"></param>
        public IQCOrderCreateService(IQualIqcOrderRepository qualIqcOrderRepository,
            IQualIqcOrderTypeRepository qualIqcOrderTypeRepository,
            IWhSupplierRepository whSupplierRepository,
            IProcMaterialRepository procMaterialRepository,
            IQualIqcInspectionItemRepository qualIqcInspectionItemRepository,
            IQualIqcInspectionItemDetailRepository qualIqcInspectionItemDetailRepository,
            IQualIqcInspectionItemSnapshotRepository qualIqcInspectionItemSnapshotRepository,
            IQualIqcInspectionItemDetailSnapshotRepository qualIqcInspectionItemDetailSnapshotRepository,
            IProcParameterRepository procParameterRepository,
            IQualIqcLevelRepository qualIqcLevelRepository,
            IQualIqcLevelDetailRepository qualIqcLevelDetailRepository,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IAQLLevelQueryService aqlLevelQueryService,
            IAQLPlanQueryService aqlPlanQueryService,
            IManuGenerateBarcodeService manuGenerateBarcodeService,
            ISequenceService sequenceService)
        {
            _qualIqcOrderRepository = qualIqcOrderRepository;
            _qualIqcOrderTypeRepository = qualIqcOrderTypeRepository;
            _whSupplierRepository = whSupplierRepository;
            _procMaterialRepository = procMaterialRepository;
            _qualIqcInspectionItemRepository = qualIqcInspectionItemRepository;
            _qualIqcInspectionItemDetailRepository = qualIqcInspectionItemDetailRepository;
            _qualIqcInspectionItemSnapshotRepository = qualIqcInspectionItemSnapshotRepository;
            _qualIqcInspectionItemDetailSnapshotRepository = qualIqcInspectionItemDetailSnapshotRepository;
            _procParameterRepository = procParameterRepository;
            _qualIqcLevelRepository = qualIqcLevelRepository;
            _qualIqcLevelDetailRepository = qualIqcLevelDetailRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _aqlLevelQueryService = aqlLevelQueryService;
            _aqlPlanQueryService = aqlPlanQueryService;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _sequenceService = sequenceService;
        }

        /// <summary>
        /// IQC检验单创建
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<int> CreateAsync(IQCOrderCreateBo bo)
        {
            if (bo == null) throw new CustomerValidationException(nameof(ErrorCode.MES10111));
            if (bo.MaterialReceiptEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10111));
            }
            if (bo.MaterialReceiptDetailEntities == null || !bo.MaterialReceiptDetailEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10111));
            }

            // 更新时间
            var updatedBy = bo.UserName;
            var updatedOn = HymsonClock.Now();

            //AQL检验水平
            var aqlLevels = await _aqlLevelQueryService.QueryListAsync(bo.SiteId);
            if (aqlLevels == null || !aqlLevels.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11802));
            }
            var aqlPlans = await _aqlPlanQueryService.QueryListAsync(bo.SiteId);
            if (aqlPlans == null || !aqlPlans.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11803));
            }

            //待写入数据
            var orderList = new List<QualIqcOrderEntity>();
            var orderTypeList = new List<QualIqcOrderTypeEntity>();
            var inspectionItemSnapshootList = new List<QualIqcInspectionItemSnapshotEntity>();
            var inspectionItemDetailSnapshootList = new List<QualIqcInspectionItemDetailSnapshotEntity>();

            //查询供应商、物料
            var supplier = await _whSupplierRepository.GetByIdAsync(bo.MaterialReceiptEntity.SupplierId);
            var supplierCode = supplier == null ? "" : supplier.Code;
            var materials = await _procMaterialRepository.GetByIdsAsync(bo.MaterialReceiptDetailEntities.Select(x => x.MaterialId).Distinct());

            //每个出货单明细生成一个检验单
            foreach (var item in bo.MaterialReceiptDetailEntities)
            {
                var materialCode = materials.FirstOrDefault(x => x.Id == item.MaterialId)?.MaterialCode ?? "";
                //查询对应IQC检验项目
                var inspectionItem = await _qualIqcInspectionItemRepository.GetOneAsync(new QualIqcInspectionItemQuery
                {
                    MaterialId = item.MaterialId,
                    SupplierId = bo.MaterialReceiptEntity.SupplierId,
                    Status = Core.Enums.DisableOrEnableEnum.Enable,
                    SiteId = bo.SiteId,
                });
                if (inspectionItem == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES11902)).WithData("MaterialCode", materialCode).WithData("SupplierCode", supplierCode);
                }
                //IQC检验项目明细
                var inspectionItemDetails = await _qualIqcInspectionItemDetailRepository.GetListAsync(new QualIqcInspectionItemDetailQuery
                {
                    QualIqcInspectionItemId = inspectionItem.Id
                });
                if (inspectionItemDetails == null || !inspectionItemDetails.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES11903)).WithData("MaterialCode", materialCode).WithData("SupplierCode", supplierCode).WithData("InspectionItemCode", inspectionItem.Code ?? "");
                }
                if (inspectionItemDetails.Any(x => x.InspectionType == null))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES11904)).WithData("MaterialCode", materialCode).WithData("SupplierCode", supplierCode).WithData("InspectionItemCode", inspectionItem.Code ?? "");
                }

                //检验类型列表
                var inspectionTypes = inspectionItemDetails.Select(x => x.InspectionType).Distinct().ToList();

                //获取供应商物料检验水平
                var iqcLevels = await _qualIqcLevelRepository.GetEntitiesAsync(new QualIqcLevelQuery
                {
                    SiteId = bo.SiteId,
                    MaterialId = inspectionItem.MaterialId,
                    SupplierId = inspectionItem.SupplierId,
                    Type = QCMaterialTypeEnum.Material,
                    Status = Core.Enums.DisableOrEnableEnum.Enable
                });
                if (iqcLevels == null || !iqcLevels.Any())
                {
                    //供应商物料检验水平为空则使用通用检验水平
                    iqcLevels = await _qualIqcLevelRepository.GetEntitiesAsync(new QualIqcLevelQuery
                    {
                        SiteId = bo.SiteId,
                        Type = QCMaterialTypeEnum.General,
                        Status = Core.Enums.DisableOrEnableEnum.Enable
                    });
                    if (iqcLevels == null || !iqcLevels.Any())
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES11905)).WithData("MaterialCode", materialCode).WithData("SupplierCode", supplierCode);
                    }
                }
                var iqcLevel = iqcLevels.First();

                // 检验水平详情
                var iqcLevelDetails = await _qualIqcLevelDetailRepository.GetEntitiesAsync(new QualIqcLevelDetailQuery
                {
                    IqcLevelIds = new[] { iqcLevel.Id }
                });
                if (iqcLevelDetails == null || !iqcLevelDetails.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES11906)).WithData("MaterialCode", materialCode).WithData("SupplierCode", supplierCode);
                }

                //获取抽样计划
                AQLPlanBo? aqlPlan = null;
                if (item.Qty > 1)
                {
                    //AQL检验水平
                    var aqlLevel = aqlLevels.First(x => x.Min <= item.Qty && x.Max >= item.Qty);
                    //样本代字
                    var samplingCode = aqlLevel.GetType().GetProperty(iqcLevel.Level.GetDescription())?.GetValue(aqlLevel)?.ParseToString();
                    //抽样计划
                    aqlPlan = aqlPlans.First(x => x.Code == samplingCode);
                }

                #region 检验等级(加严、正常、放宽)判定

                var inspectionGrade = InspectionGradeEnum.Normal;
                //上一次检验结果
                var preTask = await _qualIqcOrderRepository.GetEntityAsync(new QualIqcOrderQuery
                {
                    SiteId = bo.SiteId,
                    MaterialId = inspectionItem.MaterialId,
                    SupplierId = inspectionItem.SupplierId,
                    StatusArr = new[] { InspectionStatusEnum.Completed, InspectionStatusEnum.Closed }
                });
                if (preTask != null)
                {
                    inspectionGrade = preTask.InspectionGrade;

                    if (preTask.InspectionGrade == InspectionGradeEnum.Normal) //上一次检验等级为正常
                    {
                        var list = await _qualIqcOrderRepository.GetEntitiesAsync(new QualIqcOrderQuery
                        {
                            SiteId = bo.SiteId,
                            MaterialId = inspectionItem.MaterialId,
                            SupplierId = inspectionItem.SupplierId,
                            StatusArr = new[] { InspectionStatusEnum.Completed, InspectionStatusEnum.Closed },
                            MaxRows = 10
                        });
                        //连续5批或少于5批中有2批不合格，正常==>加严
                        if (list.Take(5).Count(x => x.IsQualified == Core.Enums.TrueOrFalseEnum.No) >= 2)
                        {
                            inspectionGrade = InspectionGradeEnum.Tighter;
                        }
                        //连续10次检验都合格，正常==>放宽
                        else if (list.Count(x => x.IsQualified == Core.Enums.TrueOrFalseEnum.Yes) == 10)
                        {
                            inspectionGrade = InspectionGradeEnum.Relax;
                        }
                    }
                    else if (preTask.InspectionGrade == InspectionGradeEnum.Tighter) //上一次检验等级为加严
                    {
                        //连续3次检验都合格，加严==>正常
                        var list = await _qualIqcOrderRepository.GetEntitiesAsync(new QualIqcOrderQuery
                        {
                            SiteId = bo.SiteId,
                            MaterialId = inspectionItem.MaterialId,
                            SupplierId = inspectionItem.SupplierId,
                            StatusArr = new[] { InspectionStatusEnum.Completed, InspectionStatusEnum.Closed },
                            MaxRows = 3
                        });
                        if (list.Count(x => x.IsQualified == Core.Enums.TrueOrFalseEnum.Yes) == 3)
                        {
                            inspectionGrade = InspectionGradeEnum.Normal;
                        }
                    }
                    else if (preTask.InspectionGrade == InspectionGradeEnum.Relax) //上一次检验等级为放宽
                    {
                        //上一次检验不合格，放宽==>正常
                        if (preTask.IsQualified == Core.Enums.TrueOrFalseEnum.No)
                        {
                            inspectionGrade = InspectionGradeEnum.Normal;
                        }
                    }
                }

                #endregion

                //计算样品数量
                var sampleQtyDic = new Dictionary<IQCInspectionTypeEnum, int>();
                foreach (var inspectionType in inspectionTypes)
                {
                    if (item.Qty < 2)
                    {
                        sampleQtyDic.Add(inspectionType.GetValueOrDefault(), 1);
                        continue;
                    }
                    //验证水准
                    var iqcLevelDetail = iqcLevelDetails.FirstOrDefault(x => x.Type == inspectionType);
                    if (iqcLevelDetail == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES11907)).WithData("MaterialCode", materialCode).WithData("SupplierCode", supplierCode).WithData("InspectionType", inspectionType?.GetDescription() ?? "");
                    }
                    var verificationLevelArr = new[] { VerificationLevelEnum.R, VerificationLevelEnum.I, VerificationLevelEnum.II, VerificationLevelEnum.III,
                        VerificationLevelEnum.IV, VerificationLevelEnum.V, VerificationLevelEnum.VI, VerificationLevelEnum.VII, VerificationLevelEnum.T };
                    var verificationLevel = iqcLevelDetail.VerificationLevel;
                    if (inspectionGrade == InspectionGradeEnum.Tighter)
                    {
                        verificationLevel = verificationLevel == VerificationLevelEnum.T ? VerificationLevelEnum.T : verificationLevelArr[Array.IndexOf(verificationLevelArr, verificationLevel) + 1];
                    }
                    else if (inspectionGrade == InspectionGradeEnum.Relax)
                    {
                        verificationLevel = verificationLevel == VerificationLevelEnum.R ? VerificationLevelEnum.R : verificationLevelArr[Array.IndexOf(verificationLevelArr, verificationLevel) - 1];
                    }
                    //样品数量
                    var sampleQty = 1;
                    if (aqlPlan != null)
                    {
                        sampleQty = aqlPlan.GetType().GetProperty(verificationLevel.GetDescription())?.GetValue(aqlPlan)?.ParseToInt() ?? 1;
                    }
                    //计算出的样品数量大于出货数量，则样品数量修改为出货数量
                    if (sampleQty > item.Qty)
                    {
                        sampleQty = (int)item.Qty;
                    }
                    sampleQtyDic.Add(inspectionType.GetValueOrDefault(), sampleQty);
                }

                #region 组装检验单数据

                //检验项目快照
                var inspectionItemSnapshoot = inspectionItem.ToEntity<QualIqcInspectionItemSnapshotEntity>();
                inspectionItemSnapshoot.Id = IdGenProvider.Instance.CreateId();
                inspectionItemSnapshootList.Add(inspectionItemSnapshoot);
                //检验项目明细快照
                var parameters = await _procParameterRepository.GetByIdsAsync(inspectionItemDetails.Select(x => x.ParameterId).Distinct());
                foreach (var inspectionItemDetail in inspectionItemDetails)
                {
                    var parameterEntity = parameters.First(x => x.Id == inspectionItemDetail.ParameterId);

                    var inspectionItemDetailSnapshoot = inspectionItemDetail.ToEntity<QualIqcInspectionItemDetailSnapshotEntity>();
                    inspectionItemDetailSnapshoot.Id = IdGenProvider.Instance.CreateId();
                    inspectionItemDetailSnapshoot.IqcInspectionItemSnapshotId = inspectionItemSnapshoot.Id;
                    inspectionItemDetailSnapshoot.ParameterCode = parameterEntity.ParameterCode;
                    inspectionItemDetailSnapshoot.ParameterName = parameterEntity.ParameterName;
                    inspectionItemDetailSnapshoot.ParameterDataType = parameterEntity.DataType;
                    inspectionItemDetailSnapshoot.ParameterUnit = parameterEntity.ParameterUnit ?? "";
                    inspectionItemDetailSnapshootList.Add(inspectionItemDetailSnapshoot);
                }
                //检验单
                //var sequence = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.ByDay, "IQC");  //流水号
                var orderEntity = new QualIqcOrderEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    //InspectionOrder = $"IQC{updatedOn.ToString("yyyyMMdd")}{sequence.ToString().PadLeft(4, '0')}",
                    InspectionOrder = await GenerateIQCOrderCodeAsync(bo),
                    MaterialId = inspectionItem.MaterialId,
                    SupplierId = inspectionItem.SupplierId,
                    IqcInspectionItemSnapshotId = inspectionItemSnapshoot.Id,
                    MaterialReceiptDetailId = item.Id,
                    InspectionGrade = inspectionGrade,
                    AcceptanceLevel = iqcLevel.AcceptanceLevel,
                    Status = InspectionStatusEnum.WaitInspect,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                };
                orderList.Add(orderEntity);
                //检验单检验类型
                foreach (var inspectionType in inspectionTypes)
                {
                    var iqcLevelDetail = iqcLevelDetails.First(x => x.Type == inspectionType);

                    orderTypeList.Add(new QualIqcOrderTypeEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = orderEntity.SiteId,
                        IQCOrderId = orderEntity.Id,
                        Type = inspectionType.GetValueOrDefault(),
                        VerificationLevel = iqcLevelDetail.VerificationLevel,
                        AcceptanceLevel = iqcLevelDetail.AcceptanceLevel,
                        SampleQty = sampleQtyDic[inspectionType.GetValueOrDefault()],
                        CheckedQty = 0,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });
                }

                #endregion
            }

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualIqcOrderRepository.InsertRangeAsync(orderList);
                rows += await _qualIqcOrderTypeRepository.InsertRangeAsync(orderTypeList);
                rows += await _qualIqcInspectionItemSnapshotRepository.InsertRangeAsync(inspectionItemSnapshootList);
                rows += await _qualIqcInspectionItemDetailSnapshotRepository.InsertRangeAsync(inspectionItemDetailSnapshootList);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 检验单号生成
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<string> GenerateIQCOrderCodeAsync(IQCOrderCreateBo bo)
        {
            var codeRules = await _inteCodeRulesRepository.GetListAsync(new InteCodeRulesReQuery
            {
                SiteId = bo.SiteId,
                CodeType = Core.Enums.Integrated.CodeRuleCodeTypeEnum.IQC
            });
            if (codeRules == null || !codeRules.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11991));
            }
            if (codeRules.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11992));
            }

            var orderCodes = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new Bos.Manufacture.ManuGenerateBarcode.GenerateBarcodeBo
            {
                SiteId = bo.SiteId,
                UserName = bo.UserName,
                CodeRuleId = codeRules.First().Id,
                Count = 1
            });

            return orderCodes.First();
        }

    }
}
