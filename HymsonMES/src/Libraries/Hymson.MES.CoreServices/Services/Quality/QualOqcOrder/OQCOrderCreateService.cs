using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Bos.Quality;
using Hymson.MES.CoreServices.Extension;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.WhShipment;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.CoreServices.Services.Quality
{
    /// <summary>
    /// OQC检验单创建服务
    /// </summary>
    public class OQCOrderCreateService : IOQCOrderCreateService
    {
        private readonly IQualOqcOrderRepository _qualOqcOrderRepository;
        private readonly IQualOqcOrderTypeRepository _qualOqcOrderTypeRepository;
        private readonly IWhShipmentRepository _whShipmentRepository;
        private readonly IWhShipmentMaterialRepository _whShipmentMaterialRepository;
        private readonly IInteCustomRepository _inteCustomRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IQualOqcParameterGroupRepository _qualOqcParameterGroupRepository;
        private readonly IQualOqcParameterGroupDetailRepository _qualOqcParameterGroupDetailRepository;
        private readonly IProcParameterRepository _procParameterRepository;
        private readonly IQualOqcLevelRepository _qualOqcLevelRepository;
        private readonly IQualOqcLevelDetailRepository _qualOqcLevelDetailRepository;
        private readonly IQualOqcParameterGroupSnapshootRepository _qualOqcParameterGroupSnapshootRepository;
        private readonly IQualOqcParameterGroupDetailSnapshootRepository _qualOqcParameterGroupDetailSnapshootRepository;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;

        private readonly IAQLLevelQueryService _aqlLevelQueryService;
        private readonly IAQLPlanQueryService _aqlPlanQueryService;
        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        private readonly ISequenceService _sequenceService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="qualOqcOrderRepository"></param>
        /// <param name="qualOqcOrderTypeRepository"></param>
        /// <param name="whShipmentRepository"></param>
        /// <param name="whShipmentMaterialRepository"></param>
        /// <param name="inteCustomRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="qualOqcParameterGroupRepository"></param>
        /// <param name="qualOqcParameterGroupDetailRepository"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="qualOqcLevelRepository"></param>
        /// <param name="qualOqcLevelDetailRepository"></param>
        /// <param name="qualOqcParameterGroupSnapshootRepository"></param>
        /// <param name="qualOqcParameterGroupDetailSnapshootRepository"></param>
        /// <param name="inteCodeRulesRepository"></param>
        /// <param name="aqlLevelQueryService"></param>
        /// <param name="aqlPlanQueryService"></param>
        /// <param name="manuGenerateBarcodeService"></param>
        /// <param name="sequenceService"></param>
        public OQCOrderCreateService(IQualOqcOrderRepository qualOqcOrderRepository,
            IQualOqcOrderTypeRepository qualOqcOrderTypeRepository,
            IWhShipmentRepository whShipmentRepository,
            IWhShipmentMaterialRepository whShipmentMaterialRepository,
            IInteCustomRepository inteCustomRepository,
            IProcMaterialRepository procMaterialRepository,
            IQualOqcParameterGroupRepository qualOqcParameterGroupRepository,
            IQualOqcParameterGroupDetailRepository qualOqcParameterGroupDetailRepository,
            IProcParameterRepository procParameterRepository,
            IQualOqcLevelRepository qualOqcLevelRepository,
            IQualOqcLevelDetailRepository qualOqcLevelDetailRepository,
            IQualOqcParameterGroupSnapshootRepository qualOqcParameterGroupSnapshootRepository,
            IQualOqcParameterGroupDetailSnapshootRepository qualOqcParameterGroupDetailSnapshootRepository,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IAQLLevelQueryService aqlLevelQueryService,
            IAQLPlanQueryService aqlPlanQueryService,
            IManuGenerateBarcodeService manuGenerateBarcodeService,
            ISequenceService sequenceService)
        {
            _qualOqcOrderRepository = qualOqcOrderRepository;
            _qualOqcOrderTypeRepository = qualOqcOrderTypeRepository;
            _whShipmentRepository = whShipmentRepository;
            _whShipmentMaterialRepository = whShipmentMaterialRepository;
            _inteCustomRepository = inteCustomRepository;
            _procMaterialRepository = procMaterialRepository;
            _qualOqcParameterGroupRepository = qualOqcParameterGroupRepository;
            _qualOqcParameterGroupDetailRepository = qualOqcParameterGroupDetailRepository;
            _procParameterRepository = procParameterRepository;
            _qualOqcLevelRepository = qualOqcLevelRepository;
            _qualOqcLevelDetailRepository = qualOqcLevelDetailRepository;
            _qualOqcParameterGroupSnapshootRepository = qualOqcParameterGroupSnapshootRepository;
            _qualOqcParameterGroupDetailSnapshootRepository = qualOqcParameterGroupDetailSnapshootRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _aqlLevelQueryService = aqlLevelQueryService;
            _aqlPlanQueryService = aqlPlanQueryService;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _sequenceService = sequenceService;
        }

        /// <summary>
        /// OQC检验单创建
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<int> CreateAsync(OQCOrderCreateBo bo)
        {
            if (bo.ShipmentEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10111));
            }
            if (bo.ShipmentMaterialEntities == null || !bo.ShipmentMaterialEntities.Any())
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
            var orderList = new List<QualOqcOrderEntity>();
            var orderTypeList = new List<QualOqcOrderTypeEntity>();
            var parameterGroupSnapshootList = new List<QualOqcParameterGroupSnapshootEntity>();
            var parameterGroupDetailSnapshootList = new List<QualOqcParameterGroupDetailSnapshootEntity>();

            //查询客户、物料
            var customer = await _inteCustomRepository.GetByIdAsync(bo.ShipmentEntity.CustomerId);
            var customerCode = customer == null ? "" : customer.Code;
            var materials = await _procMaterialRepository.GetByIdsAsync(bo.ShipmentMaterialEntities.Select(x => x.MaterialId).Distinct());

            //每个出货单明细生成一个检验单
            foreach (var item in bo.ShipmentMaterialEntities)
            {
                var materialCode = materials.FirstOrDefault(x => x.Id == item.MaterialId)?.MaterialCode ?? "";
                //查询对应OQC检验参数组
                var parameterGroupEntity = await _qualOqcParameterGroupRepository.GetEntityAsync(new QualOqcParameterGroupQuery
                {
                    MaterialId = item.MaterialId,
                    CustomerId = bo.ShipmentEntity.CustomerId,
                    Status = Core.Enums.SysDataStatusEnum.Enable,
                    SiteId = bo.SiteId
                });
                if (parameterGroupEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES11804)).WithData("MaterialCode", materialCode).WithData("CustomerCode", customerCode);
                }
                //OQC检验参数组明细
                var parameterGroupDetails = await _qualOqcParameterGroupDetailRepository.GetEntitiesAsync(new QualOqcParameterGroupDetailQuery { ParameterGroupId = parameterGroupEntity.Id });
                if (parameterGroupDetails == null || !parameterGroupDetails.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES11805)).WithData("MaterialCode", materialCode).WithData("CustomerCode", customerCode);
                }
                //检验类型列表
                var inspectionTypes = parameterGroupDetails.Select(x => x.InspectionType).Distinct().ToList();

                //获取客户物料检验水平
                var oqcLevels = await _qualOqcLevelRepository.GetEntitiesAsync(new QualOqcLevelQuery
                {
                    SiteId = bo.SiteId,
                    MaterialId = parameterGroupEntity.MaterialId,
                    CustomId = parameterGroupEntity.CustomerId,
                    Type = Core.Enums.Quality.QCMaterialTypeEnum.Material,
                    Status = Core.Enums.DisableOrEnableEnum.Enable
                });
                if (oqcLevels == null || !oqcLevels.Any())
                {
                    //客户物料检验水平为空则使用通用检验水平
                    oqcLevels = await _qualOqcLevelRepository.GetEntitiesAsync(new QualOqcLevelQuery
                    {
                        SiteId = bo.SiteId,
                        Type = Core.Enums.Quality.QCMaterialTypeEnum.General,
                        Status = Core.Enums.DisableOrEnableEnum.Enable
                    });
                    if (oqcLevels == null || !oqcLevels.Any())
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES11806)).WithData("MaterialCode", materialCode).WithData("CustomerCode", customerCode);
                    }
                }
                var oqcLevel = oqcLevels.First();

                //检验水平详情
                var oqcLevelDetails = await _qualOqcLevelDetailRepository.GetEntitiesAsync(new QualOqcLevelDetailQuery { OqcLevelIds = new[] { oqcLevel.Id } });
                if (oqcLevelDetails == null || !oqcLevelDetails.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES11807)).WithData("MaterialCode", materialCode).WithData("CustomerCode", customerCode);
                }

                //获取抽样计划
                AQLPlanBo? aqlPlan = null;
                if (item.Qty >= 2)
                {
                    //AQL检验水平
                    var aqlLevel = aqlLevels.First(x => x.Min <= item.Qty && x.Max >= item.Qty);
                    //样本代字
                    var samplingCode = aqlLevel.GetType().GetProperty(oqcLevel.Level.GetDescription())?.GetValue(aqlLevel)?.ParseToString();
                    //抽样计划
                    aqlPlan = aqlPlans.First(x => x.Code == samplingCode);
                }

                //计算样品数量
                var sampleQtyDic = new Dictionary<OQCInspectionTypeEnum, int>();
                foreach (var inspectionType in inspectionTypes)
                {
                    var oqcLevelDetail = oqcLevelDetails.FirstOrDefault(x => x.Type == inspectionType);
                    if (oqcLevelDetail == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES11808)).WithData("MaterialCode", materialCode).WithData("CustomerCode", customerCode).WithData("InspectionType", inspectionType.GetDescription());
                    }
                    if (item.Qty < 2)
                    {
                        sampleQtyDic.Add(inspectionType, 1);
                        continue;
                    }
                    //样品数量
                    var sampleQty = 1;
                    if (aqlPlan != null)
                    {
                        sampleQty = aqlPlan.GetType().GetProperty(oqcLevelDetail.VerificationLevel.GetDescription())?.GetValue(aqlPlan)?.ParseToInt() ?? 1;
                    }
                    //计算出的样品数量大于出货数量，则样品数量修改为出货数量
                    if (sampleQty > item.Qty)
                    {
                        sampleQty = (int)item.Qty;
                    }
                    sampleQtyDic.Add(inspectionType, sampleQty);
                }

                #region 组装检验单数据

                //检验项目快照
                var parameterGroupSnapshoot = parameterGroupEntity.ToEntity<QualOqcParameterGroupSnapshootEntity>();
                parameterGroupSnapshoot.Id = IdGenProvider.Instance.CreateId();
                parameterGroupSnapshootList.Add(parameterGroupSnapshoot);
                //检验项目明细快照
                var parameters = await _procParameterRepository.GetByIdsAsync(parameterGroupDetails.Select(x => x.ParameterId).Distinct());
                foreach (var parameterGroupDetail in parameterGroupDetails)
                {
                    var parameterEntity = parameters.First(x => x.Id == parameterGroupDetail.ParameterId);

                    var parameterGroupDetailSnapshoot = parameterGroupDetail.ToEntity<QualOqcParameterGroupDetailSnapshootEntity>();
                    parameterGroupDetailSnapshoot.Id = IdGenProvider.Instance.CreateId();
                    parameterGroupDetailSnapshoot.ParameterGroupId = parameterGroupSnapshoot.Id;
                    parameterGroupDetailSnapshoot.ParameterCode = parameterEntity.ParameterCode;
                    parameterGroupDetailSnapshoot.ParameterName = parameterEntity.ParameterName;
                    parameterGroupDetailSnapshoot.ParameterDataType = parameterEntity.DataType;
                    parameterGroupDetailSnapshoot.ParameterUnit = parameterEntity.ParameterUnit ?? "";
                    parameterGroupDetailSnapshootList.Add(parameterGroupDetailSnapshoot);
                }
                //检验单
                //var sequence = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.ByDay, "OQC");  //流水号
                var orderEntity = new QualOqcOrderEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = bo.SiteId,
                    //InspectionOrder = $"OQC{updatedOn.ToString("yyyyMMdd")}{sequence.ToString().PadLeft(4, '0')}",
                    InspectionOrder = await GenerateOQCOrderCodeAsync(bo),
                    GroupSnapshootId = parameterGroupSnapshoot.Id,
                    MaterialId = parameterGroupEntity.MaterialId,
                    CustomerId = parameterGroupEntity.CustomerId,
                    ShipmentMaterialId = item.Id,
                    ShipmentQty = item.Qty,
                    AcceptanceLevel = oqcLevel.AcceptanceLevel,
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
                    var oqcLevelDetail = oqcLevelDetails.First(x => x.Type == inspectionType);

                    orderTypeList.Add(new QualOqcOrderTypeEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = orderEntity.SiteId,
                        OQCOrderId = orderEntity.Id,
                        InspectionType = inspectionType,
                        VerificationLevel = oqcLevelDetail.VerificationLevel,
                        AcceptanceLevel = oqcLevelDetail.AcceptanceLevel,
                        SampleQty = sampleQtyDic[inspectionType],
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
                rows += await _qualOqcOrderRepository.InsertRangeAsync(orderList);
                rows += await _qualOqcOrderTypeRepository.InsertRangeAsync(orderTypeList);
                rows += await _qualOqcParameterGroupSnapshootRepository.InsertRangeAsync(parameterGroupSnapshootList);
                rows += await _qualOqcParameterGroupDetailSnapshootRepository.InsertRangeAsync(parameterGroupDetailSnapshootList);
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
        private async Task<string> GenerateOQCOrderCodeAsync(OQCOrderCreateBo bo)
        {
            var codeRules = await _inteCodeRulesRepository.GetListAsync(new InteCodeRulesReQuery
            {
                SiteId = bo.SiteId,
                CodeType = Core.Enums.Integrated.CodeRuleCodeTypeEnum.OQC
            });
            if (codeRules == null || !codeRules.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11810));
            }
            if (codeRules.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11811));
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
