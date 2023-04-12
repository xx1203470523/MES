/*
 *creator: Karl
 *
 *describe: 条码接收    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Services.Dtos.Plan;

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

        private readonly AbstractValidator<PlanSfcReceiveCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanSfcReceiveModifyDto> _validationModifyRules;

        public PlanSfcReceiveService(ICurrentUser currentUser, ICurrentSite currentSite,
            IPlanSfcReceiveRepository planSfcInfoRepository, IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcRepository manuSfcRepository, IInteCodeRulesRepository inteCodeRulesRepository, IInteCodeRulesMakeRepository inteCodeRulesMakeRepository,
        AbstractValidator<PlanSfcReceiveCreateDto> validationCreateRules, AbstractValidator<PlanSfcReceiveModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planSfcInfoRepository = planSfcInfoRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcRepository = manuSfcRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="planSfcInfoCreateDto"></param>
        /// <returns></returns>
        public async Task CreatePlanSfcInfoAsync(PlanSfcReceiveCreateDto planSfcInfoCreateDto)
        {
            //TODO  逻辑异常 重新写
            //#region 验证与数据组装
            ////// 判断是否有获取到站点码 
            //if (_currentSite.SiteId == 0)
            //{
            //    throw new ValidationException(nameof(ErrorCode.MES10101));
            //}
            ////验证DTO
            //await _validationCreateRules.ValidateAndThrowAsync(planSfcInfoCreateDto);


            ////验证条码
            ////var sfcInfo = await _planSfcInfoRepository.GetBySFCAsync(planSfcInfoCreateDto.SFC);
            ////if (sfcInfo != null)
            ////{
            ////    throw new BusinessException(nameof(ErrorCode.MES16105)).WithData("SFC", planSfcInfoCreateDto.SFC);
            ////}
            ////验证工单
            //var workOrderInfo = await _planWorkOrderRepository.GetByIdAsync(planSfcInfoCreateDto.WorkOrderId);
            //if (workOrderInfo.Status != PlanWorkOrderStatusEnum.NotStarted)
            //{
            //    throw new BusinessException(nameof(ErrorCode.MES16106)).WithData("OrderCode", workOrderInfo.OrderCode);
            //}

            ////验证条码规则
            //await VerifyCodeRule(workOrderInfo.ProductId, planSfcInfoCreateDto.SFC);

            //var manuSfcInfoCreate = new ManuSfcInfoEntity();
            //var manuSfcInfoUpdate = new ManuSfcInfoEntity();
            ////物料/供应商条码接收？
            //if (planSfcInfoCreateDto.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
            //{
            //    #region 物料条码接收 处理
            //    if (planSfcInfoCreateDto.WorkOrderId == planSfcInfoCreateDto.RelevanceWorkOrderId)
            //    {
            //        throw new BusinessException(nameof(ErrorCode.MES16109)).WithData("OrderCode", workOrderInfo.OrderCode);
            //    }

            //    if (planSfcInfoCreateDto.RelevanceWorkOrderId <= 0)
            //    {
            //        throw new BusinessException(nameof(ErrorCode.MES16103));
            //    }

            //    var relevanceWorkOrderInfo = await _planWorkOrderRepository.GetByIdAsync(planSfcInfoCreateDto.RelevanceWorkOrderId);
            //    if (relevanceWorkOrderInfo.Status != PlanWorkOrderStatusEnum.Closed)
            //    {
            //        throw new BusinessException(nameof(ErrorCode.MES16106)).WithData("OrderCode", relevanceWorkOrderInfo.OrderCode);
            //    }

            //    var planSfcInfo = await _planSfcInfoRepository.GetPlanSfcInfoAsync(new PlanSfcReceiveQuery { SFC = planSfcInfoCreateDto.SFC, WorkOrderId = planSfcInfoCreateDto.RelevanceWorkOrderId });
            //    if (planSfcInfo == null)
            //    {
            //        throw new BusinessException(nameof(ErrorCode.MES16112));
            //    }
            //    if (planSfcInfo.ProductId != workOrderInfo.ProductId)
            //    {
            //        throw new BusinessException(nameof(ErrorCode.MES16108)).WithData("OrderCode", workOrderInfo.OrderCode);
            //    }
            //    //创建
            //    manuSfcInfoCreate.SFC = planSfcInfo.SFC;
            //    manuSfcInfoCreate.WorkOrderId = planSfcInfoCreateDto.WorkOrderId;
            //    manuSfcInfoCreate.ProductId = planSfcInfo.ProductId;
            //    manuSfcInfoCreate.Qty = 1;// workOrderInfo.Qty;
            //    manuSfcInfoCreate.Status = SfcStatusEnum.InProcess;
            //    manuSfcInfoCreate.IsUsed = 0;
            //    manuSfcInfoCreate.RelevanceWorkOrderId = planSfcInfo.WorkOrderId;
            //    manuSfcInfoCreate.SiteId = _currentSite.SiteId ?? 0;

            //    manuSfcInfoCreate.Id = IdGenProvider.Instance.CreateId();
            //    manuSfcInfoCreate.CreatedBy = _currentUser.UserName;
            //    manuSfcInfoCreate.UpdatedBy = _currentUser.UserName;
            //    manuSfcInfoCreate.CreatedOn = HymsonClock.Now();
            //    manuSfcInfoCreate.UpdatedOn = HymsonClock.Now();

            //    //修改
            //    manuSfcInfoUpdate = planSfcInfo;
            //    manuSfcInfoUpdate.Status = SfcStatusEnum.Received;
            //    manuSfcInfoUpdate.UpdatedBy = _currentUser.UserName;
            //    manuSfcInfoUpdate.UpdatedOn = HymsonClock.Now();
            //    #endregion
            //}
            //else
            //{
            //    #region 供应商条码接收处理
            //    //验证条码
            //    var sfcInfo = await _planSfcInfoRepository.GetBySFCAsync(planSfcInfoCreateDto.SFC);
            //    if (sfcInfo != null)
            //    {
            //        throw new BusinessException(nameof(ErrorCode.MES16105)).WithData("SFC", planSfcInfoCreateDto.SFC);
            //    }
            //    manuSfcInfoCreate.SFC = planSfcInfoCreateDto.SFC;
            //    manuSfcInfoCreate.WorkOrderId = planSfcInfoCreateDto.WorkOrderId;
            //    manuSfcInfoCreate.ProductId = workOrderInfo.ProductId;
            //    manuSfcInfoCreate.Qty = 1;// workOrderInfo.Qty;
            //    manuSfcInfoCreate.Status = SfcStatusEnum.InProcess;
            //    manuSfcInfoCreate.IsUsed = 0;
            //    manuSfcInfoCreate.RelevanceWorkOrderId = 0;
            //    manuSfcInfoCreate.SiteId = _currentSite.SiteId ?? 0;

            //    manuSfcInfoCreate.Id = IdGenProvider.Instance.CreateId();
            //    manuSfcInfoCreate.CreatedBy = _currentUser.UserName;
            //    manuSfcInfoCreate.UpdatedBy = _currentUser.UserName;
            //    manuSfcInfoCreate.CreatedOn = HymsonClock.Now();
            //    manuSfcInfoCreate.UpdatedOn = HymsonClock.Now();
            //    #endregion
            //}

            //#endregion

            //#region 入库
            //var response = 0;
            //if (planSfcInfoCreateDto.ReceiveType == PlanSFCReceiveTypeEnum.MaterialSfc)
            //{
            //    using (var trans = TransactionHelper.GetTransactionScope())
            //    {
            //        response += await _planSfcInfoRepository.InsertAsync(manuSfcInfoCreate);
            //        response += await _planSfcInfoRepository.UpdateAsync(manuSfcInfoUpdate);
            //        trans.Complete();
            //    }
            //}
            //else
            //{
            //    response = await _planSfcInfoRepository.InsertAsync(manuSfcInfoCreate);
            //}
            //if (response == 0)
            //{
            //    throw new BusinessException(nameof(ErrorCode.MES16110));
            //}
            //#endregion

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
