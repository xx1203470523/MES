/*
 *creator: Karl
 *
 *describe: 工单信息表    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-20 10:07:17
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Plan
{
    /// <summary>
    /// 工单信息表 服务
    /// </summary>
    public class PlanWorkOrderService : IPlanWorkOrderService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly AbstractValidator<PlanWorkOrderCreateDto> _validationCreateRules;
        private readonly AbstractValidator<PlanWorkOrderModifyDto> _validationModifyRules;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcBomRepository _procBomRepository;
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        public PlanWorkOrderService(ICurrentUser currentUser, ICurrentSite currentSite, IPlanWorkOrderRepository planWorkOrderRepository, AbstractValidator<PlanWorkOrderCreateDto> validationCreateRules, AbstractValidator<PlanWorkOrderModifyDto> validationModifyRules, IProcMaterialRepository procMaterialRepository, IProcBomRepository procBomRepository, IProcProcessRouteRepository procProcessRouteRepository, IInteWorkCenterRepository inteWorkCenterRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _planWorkOrderRepository = planWorkOrderRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;

            _procMaterialRepository = procMaterialRepository;
            _procBomRepository = procBomRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="planWorkOrderCreateDto"></param>
        /// <returns></returns>
        public async Task CreatePlanWorkOrderAsync(PlanWorkOrderCreateDto planWorkOrderCreateDto)
        {
            //// 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(planWorkOrderCreateDto);

            planWorkOrderCreateDto.OrderCode = planWorkOrderCreateDto.OrderCode.ToUpper();
            //判断编号是否已存在
            var haveEntity = await _planWorkOrderRepository.GetEqualPlanWorkOrderEntitiesAsync(new PlanWorkOrderQuery()
            {
                SiteId = _currentSite.SiteId??0,
                OrderCode = planWorkOrderCreateDto.OrderCode
            });
            if (haveEntity != null&& haveEntity.Count()>0) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16001)).WithData("orderCode", planWorkOrderCreateDto.OrderCode);
            }

            //DTO转换实体
            var planWorkOrderEntity = planWorkOrderCreateDto.ToEntity<PlanWorkOrderEntity>();
            planWorkOrderEntity.Id= IdGenProvider.Instance.CreateId();
            planWorkOrderEntity.CreatedBy = _currentUser.UserName;
            planWorkOrderEntity.UpdatedBy = _currentUser.UserName;
            planWorkOrderEntity.CreatedOn = HymsonClock.Now();
            planWorkOrderEntity.UpdatedOn = HymsonClock.Now();
            planWorkOrderEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            var response = await _planWorkOrderRepository.InsertAsync(planWorkOrderEntity);

            if (response == 0)
            {
                throw new BusinessException(nameof(ErrorCode.MES16002));
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePlanWorkOrderAsync(long id)
        {
            await _planWorkOrderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesPlanWorkOrderAsync(long[] idsArr)
        {
            return await _planWorkOrderRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="planWorkOrderPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderDto>> GetPageListAsync(PlanWorkOrderPagedQueryDto planWorkOrderPagedQueryDto)
        {
            var planWorkOrderPagedQuery = planWorkOrderPagedQueryDto.ToQuery<PlanWorkOrderPagedQuery>();
            var pagedInfo = await _planWorkOrderRepository.GetPagedInfoAsync(planWorkOrderPagedQuery);

            //实体到DTO转换 装载数据
            List<PlanWorkOrderDto> planWorkOrderDtos = PreparePlanWorkOrderDtos(pagedInfo);
            return new PagedInfo<PlanWorkOrderDto>(planWorkOrderDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<PlanWorkOrderDto> PreparePlanWorkOrderDtos(PagedInfo<PlanWorkOrderEntity>   pagedInfo)
        {
            var planWorkOrderDtos = new List<PlanWorkOrderDto>();
            foreach (var planWorkOrderEntity in pagedInfo.Data)
            {
                var planWorkOrderDto = planWorkOrderEntity.ToModel<PlanWorkOrderDto>();
                planWorkOrderDtos.Add(planWorkOrderDto);
            }

            return planWorkOrderDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="planWorkOrderDto"></param>
        /// <returns></returns>
        public async Task ModifyPlanWorkOrderAsync(PlanWorkOrderModifyDto planWorkOrderModifyDto)
        {
            if (_currentSite.SiteId == 0) 
            {
                throw new BusinessException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(planWorkOrderModifyDto);

            //获取当前最新的数据  进行状态判断
            var current= _planWorkOrderRepository.GetByIdAsync(planWorkOrderModifyDto.Id);
            if (current != null)
            {
                //判断当前状态  TODO

            }
            else 
            {
                throw new BusinessException(nameof(ErrorCode.MES16003));
            }


            //DTO转换实体
            var planWorkOrderEntity = planWorkOrderModifyDto.ToEntity<PlanWorkOrderEntity>();
            planWorkOrderEntity.UpdatedBy = _currentUser.UserName;
            planWorkOrderEntity.UpdatedOn = HymsonClock.Now();

            var response= await _planWorkOrderRepository.UpdateAsync(planWorkOrderEntity);
            if (response == 0) 
            {
                throw new BusinessException(nameof(ErrorCode.MES16004));
            }
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderDetailView> QueryPlanWorkOrderByIdAsync(long id) 
        {
           var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(id);
           if (planWorkOrderEntity != null) 
           {
                var planWorkOrderDetailView= planWorkOrderEntity.ToModel<PlanWorkOrderDetailView>();

                //关联物料
                var material= await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId, planWorkOrderEntity.SiteId);
                if (material != null)
                {
                    planWorkOrderDetailView.MaterialCode = material.MaterialCode;
                    planWorkOrderDetailView.MaterialVersion= material.Version;
                }

                //关联BOM
                var bom = await _procBomRepository.GetByIdAsync(planWorkOrderEntity.ProductBOMId??0);
                if (bom != null)
                {
                    planWorkOrderDetailView.BomCode = bom.BomCode;
                    planWorkOrderDetailView.BomVersion = bom.Version;
                }

                //关联BOM
                var processRoute = await _procProcessRouteRepository.GetByIdAsync(planWorkOrderEntity.ProcessRouteId ?? 0);
                if (processRoute != null)
                {
                    planWorkOrderDetailView.ProcessRouteCode = processRoute.Code;
                    planWorkOrderDetailView.ProcessRouteVersion = processRoute.Version;
                }

                //关联工作中心
                var workCenter = await _inteWorkCenterRepository.GetByIdAsync(planWorkOrderEntity.WorkCenterId ?? 0);
                if (processRoute != null)
                {
                    planWorkOrderDetailView.WorkCenterCode = workCenter.Code;
                }

                return planWorkOrderDetailView;
           }
            return null;
        }
    }
}
