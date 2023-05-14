using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 条码打印 服务
    /// </summary>
    public class PlanSfcPrintService : IPlanSfcPrintService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<PlanSfcPrintCreateDto> _validationCreateRules;

        /// <summary>
        /// 仓储（条码）
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储（条码信息）
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 仓储（条码在制品）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 仓储（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 仓储（工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        public PlanSfcPrintService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<PlanSfcPrintCreateDto> validationCreateRules,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuSfcStepRepository manuSfcStepRepository,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationCreateRules = validationCreateRules;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task CreateAsync(PlanSfcPrintCreateDto createDto)
        {
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(createDto);

        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            var sfcEntities = await _manuSfcRepository.GetByIdsAsync(idsArr);
            if (sfcEntities.Any(it => it.IsUsed == YesOrNoEnum.Yes) == true) throw new CustomerValidationException(nameof(ErrorCode.MES16116));
            if (sfcEntities.Any(it => it.Status == SfcStatusEnum.Scrapping) == true) throw new CustomerValidationException(nameof(ErrorCode.MES16116));
            // 条码集合
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(s => s.Id));

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _manuSfcRepository.DeletesAsync(new DeleteCommand
                {
                    Ids = idsArr,
                    UserId = _currentUser.UserName,
                    DeleteOn = HymsonClock.Now()
                });
                rows += await _manuSfcProduceRepository.DeletePhysicalRangeAsync(sfcEntities.Select(s => s.SFC).ToArray());
                rows += await _manuSfcStepRepository.InsertRangeAsync(sfcEntities.Select(s => new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = s.SFC,
                    Qty = s.Qty,
                    ProductId = sfcInfoEntities.FirstOrDefault(f => f.SfcId == s.Id)!.ProductId,
                    WorkOrderId = sfcInfoEntities.FirstOrDefault(f => f.SfcId == s.Id)!.WorkOrderId,
                    //ProductBOMId = planWorkOrderEntity.ProductBOMId,
                    //WorkCenterId = planWorkOrderEntity.WorkCenterId ?? 0,
                    //ProcedureId = processRouteFirstProcedure.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.Delete,
                    CurrentStatus = SfcProduceStatusEnum.Complete,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                }));

                /*
                 * 禅道 1159 反映不能删除
                rows += await _planWorkOrderRepository.DeletesAsync(new DeleteCommand
                {
                    Ids = sfcInfoEntities.Select(s => s.WorkOrderId).ToArray(),
                    UserId = _currentUser.UserName,
                    DeleteOn = HymsonClock.Now()
                });
                */

                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 分页查询列表（条码打印）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanSfcPrintDto>> GetPagedListAsync(PlanSfcPrintPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuSfcPassDownPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuSfcRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => new PlanSfcPrintDto
            {
                Id = s.Id,
                SFC = s.SFC,
                IsUsed = s.IsUsed,
                UpdatedOn = s.UpdatedOn,
                OrderCode = s.OrderCode,
                MaterialCode = s.MaterialCode,
                MaterialName = s.MaterialName,
                BuyType = s.BuyType
            });
            return new PagedInfo<PlanSfcPrintDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }


    }
}
