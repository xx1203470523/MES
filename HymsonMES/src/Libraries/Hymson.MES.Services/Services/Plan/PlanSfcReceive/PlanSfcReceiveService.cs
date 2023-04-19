/*
 *creator: Karl
 *
 *describe: 条码接收    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.WhMaterialInventory.Query;
using Hymson.MES.Services.Dtos.Plan;
using System.Security.Policy;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 条码接收 服务
    /// </summary>
    public class PlanSfcReceiveService : IPlanSfcReceiveService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        /// <summary>
        /// 条码接收 仓储
        /// </summary>
        private readonly IPlanSfcReceiveRepository _planSfcInfoRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;
        private readonly IWhMaterialInventoryRepository _whMaterialInventoryRepository;
        private readonly ILocalizationService _localizationService;
        private readonly AbstractValidator<PlanSfcReceiveCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanSfcReceiveModifyDto> _validationModifyRules;

        public PlanSfcReceiveService(ICurrentUser currentUser, ICurrentSite currentSite,
            IPlanSfcReceiveRepository planSfcInfoRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcRepository manuSfcRepository,
            IInteCodeRulesRepository inteCodeRulesRepository,
            IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
            IWhMaterialInventoryRepository whMaterialInventoryRepository,
            ILocalizationService localizationService,
        AbstractValidator<PlanSfcReceiveCreateDto> validationCreateRules, AbstractValidator<PlanSfcReceiveModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planSfcInfoRepository = planSfcInfoRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcRepository = manuSfcRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _whMaterialInventoryRepository = whMaterialInventoryRepository;
            _localizationService = localizationService;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 条码接收
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task CreatePlanSfcInfoAsync(PlanSfcReceiveCreateDto param)
        {
            //#region 验证与数据组装
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(param);
            var workOrdeEntity = await _planWorkOrderRepository.GetByIdAsync(param.WorkOrderId);
            if (workOrdeEntity.IsLocked == YesOrNoEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16106)).WithData("OrderCode", workOrdeEntity.OrderCode);
            }
            if (workOrdeEntity.Status == PlanWorkOrderStatusEnum.NotStarted)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16118)).WithData("OrderCode", workOrdeEntity.OrderCode);
            }
            if (workOrdeEntity.Status == PlanWorkOrderStatusEnum.Closed)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16119)).WithData("OrderCode", workOrdeEntity.OrderCode);
            }
            var validationFailures = new List<ValidationFailure>();
            if (param.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
            {
                var whMaterialInventoryList = await _whMaterialInventoryRepository.GetByBarCodesAsync(new WhMaterialInventoryBarcodeQuery
                {
                    BarCodes = param.SFCs,
                    SiteId = _currentSite.SiteId ?? 0
                });

                foreach (var sfc in param.SFCs)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", sfc}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", sfc);
                    }
                    if (!whMaterialInventoryList.Any(x => x.MaterialBarCode == sfc))
                    {
                        validationFailure.ErrorCode = nameof(ErrorCode.MES16120);
                        validationFailures.Add(validationFailure);
                    }
                }
            }
            else
            {
                foreach (var sfc in param.SFCs)
                {

                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("SFCError"), validationFailures);
            }
        }

        /// <summary>
        /// 验证条码规则
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="sfc"></param>
        /// <exception cref="BusinessException"></exception>
        private async Task<bool> VerifyCodeRule(long productId, string sfc)
        {

            var getCodeRulesTask = await _inteCodeRulesRepository.GetInteCodeRulesEntitiesEqualAsync(new InteCodeRulesQuery { ProductId = productId });
            if (getCodeRulesTask.Count() == 0)
            {
                throw new BusinessException(nameof(ErrorCode.MES16113));
            }
            var getCodeRules = getCodeRulesTask.FirstOrDefault();

            if (sfc.Length != getCodeRules.Base)
            {
                throw new BusinessException(nameof(ErrorCode.MES16114)).WithData("Base", getCodeRules.Base);
            }

            var getCodeRulesMakeListTask = _inteCodeRulesMakeRepository.GetInteCodeRulesMakeEntitiesAsync(new InteCodeRulesMakeQuery { CodeRulesId = getCodeRules.Id });
            var codeRulesMakeList = await getCodeRulesMakeListTask;
            foreach (var rule in codeRulesMakeList.OrderBy(x => x.Seq))
            {
                if (rule.ValueTakingType == CodeValueTakingTypeEnum.FixedValue)
                {
                    if (!sfc.Contains(rule.SegmentedValue))
                    {
                        throw new BusinessException(nameof(ErrorCode.MES16115)).WithData("ValuesType", CodeValueTakingTypeEnum.FixedValue.ToString()).WithData("SegmentedValue", rule.SegmentedValue);
                    };
                }
            }
            return true;
        }


        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="planSfcInfoPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanSfcReceiveDto>> GetPageListAsync(PlanSfcReceivePagedQueryDto planSfcInfoPagedQueryDto)
        {
            var planSfcInfoPagedQuery = planSfcInfoPagedQueryDto.ToQuery<PlanSfcReceivePagedQuery>();
            var pagedInfo = await _planSfcInfoRepository.GetPagedInfoAsync(planSfcInfoPagedQuery);

            //实体到DTO转换 装载数据
            List<PlanSfcReceiveDto> planSfcInfoDtos = PreparePlanSfcInfoDtos(pagedInfo);
            return new PagedInfo<PlanSfcReceiveDto>(planSfcInfoDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<PlanSfcReceiveDto> PreparePlanSfcInfoDtos(PagedInfo<PlanSfcReceiveView> pagedInfo)
        {
            var planSfcInfoDtos = new List<PlanSfcReceiveDto>();
            foreach (var planSfcInfoEntity in pagedInfo.Data)
            {
                var planSfcInfoDto = planSfcInfoEntity.ToModel<PlanSfcReceiveDto>();
                planSfcInfoDtos.Add(planSfcInfoDto);
            }

            return planSfcInfoDtos;
        }
    }
}
