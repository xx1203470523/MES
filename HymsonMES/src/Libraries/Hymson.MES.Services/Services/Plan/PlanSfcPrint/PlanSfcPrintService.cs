/*
 *creator: Karl
 *
 *describe: 条码打印    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:33:58
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 条码打印 服务
    /// </summary>
    public class PlanSfcPrintService : IPlanSfcPrintService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        /// <summary>
        /// 条码打印 仓储
        /// </summary>
        private readonly IPlanSfcPrintRepository _planSfcInfoRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IManuSfcRepository _manuSfcRepository;

        private readonly AbstractValidator<PlanSfcPrintCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanSfcPrintModifyDto> _validationModifyRules;

        public PlanSfcPrintService(ICurrentUser currentUser, ICurrentSite currentSite,
            IPlanSfcPrintRepository planSfcInfoRepository, IPlanWorkOrderRepository planWorkOrderRepository, IManuSfcRepository manuSfcRepository,
        AbstractValidator<PlanSfcPrintCreateDto> validationCreateRules, AbstractValidator<PlanSfcPrintModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planSfcInfoRepository = planSfcInfoRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcRepository = manuSfcRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="planSfcInfoCreateDto"></param>
        /// <returns></returns>
        public async Task CreatePlanSfcInfoAsync(PlanSfcPrintCreateDto planSfcInfoCreateDto)
        {
            #region 验证与数据组装
            //// 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(planSfcInfoCreateDto);

            //验证条码与工单




            //生成条码




            //条码打印




            #endregion

            #region 入库
            #endregion
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesPlanSfcInfoAsync(long[] idsArr)
        {

            var sfcList = await _manuSfcRepository.GetByIdsAsync(idsArr);
            //if (sfcList.Where(it => it.IsUsed > 0).Any())
            //{
            //    var msgSfcs = string.Join(",", sfcList.Where(it => it.IsUsed > 0).Select(it => it.SFC).ToArray());
            //    throw new BusinessException(nameof(ErrorCode.MES16111)).WithData("SFC", msgSfcs);
            //}
            //TODO 这里的验证不能只根据状态来验证 需要查看最新的maun_sfc_step表 是否是新建
            return await _manuSfcRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="planSfcInfoPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanSfcPrintDto>> GetPageListAsync(PlanSfcPrintPagedQueryDto planSfcInfoPagedQueryDto)
        {
            var planSfcInfoPagedQuery = planSfcInfoPagedQueryDto.ToQuery<PlanSfcPrintPagedQuery>();
            var pagedInfo = await _planSfcInfoRepository.GetPagedInfoAsync(planSfcInfoPagedQuery);

            //实体到DTO转换 装载数据
            List<PlanSfcPrintDto> planSfcInfoDtos = PreparePlanSfcInfoDtos(pagedInfo);
            return new PagedInfo<PlanSfcPrintDto>(planSfcInfoDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<PlanSfcPrintDto> PreparePlanSfcInfoDtos(PagedInfo<PlanSfcPrintView> pagedInfo)
        {
            var planSfcInfoDtos = new List<PlanSfcPrintDto>();
            foreach (var planSfcInfoEntity in pagedInfo.Data)
            {
                var planSfcInfoDto = planSfcInfoEntity.ToModel<PlanSfcPrintDto>();
                planSfcInfoDtos.Add(planSfcInfoDto);
            }

            return planSfcInfoDtos;
        }
    }
}
