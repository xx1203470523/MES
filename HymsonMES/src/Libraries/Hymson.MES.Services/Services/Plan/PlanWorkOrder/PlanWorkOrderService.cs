using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Warehouse;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuRequistionOrder;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Command;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Minio.DataModel;
using System.Transactions;

namespace Hymson.MES.Services.Services.Plan.PlanWorkOrder
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
        private readonly IPlanWorkOrderStatusRecordRepository _planWorkOrderStatusRecordRepository;
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;
        private readonly IPlanWorkOrderActivationRecordRepository _planWorkOrderActivationRecordRepository;

        private readonly AbstractValidator<PlanWorkOrderChangeStatusDto> _validationChangeStatusRules;
        private readonly IManuRequistionOrderRepository _manuRequistionOrderRepository;
        private readonly IManuProductReceiptOrderService _manuProductReceiptOrderService; 
        private readonly IManuRequistionOrderDetailRepository _manuRequistionOrderDetailRepository;
        private readonly Data.Repositories.Manufacture.IManuReturnOrderRepository _manuReturnOrderRepository;
        /// <summary>
        /// 仓储接口（生产计划产品）
        /// </summary>
        private readonly IPlanWorkPlanProductRepository _planWorkPlanProductRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="manuRequistionOrderRepository"></param>
        /// <param name="planWorkOrderStatusRecordRepository"></param>
        /// <param name="planWorkOrderActivationRecordRepository"></param>
        /// <param name="planWorkOrderActivationRepository"></param>
        /// <param name="manuRequistionOrderDetailRepository"></param>
        /// <param name="manuReturnOrderRepository"></param>
        /// <param name="validationChangeStatusRules"></param>
        /// <param name="planWorkPlanProductRepository"></param>
        public PlanWorkOrderService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<PlanWorkOrderCreateDto> validationCreateRules,
            AbstractValidator<PlanWorkOrderModifyDto> validationModifyRules,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcBomRepository procBomRepository,
            IProcProcessRouteRepository procProcessRouteRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IManuRequistionOrderRepository manuRequistionOrderRepository,
            IPlanWorkOrderStatusRecordRepository planWorkOrderStatusRecordRepository,
            IPlanWorkOrderActivationRecordRepository planWorkOrderActivationRecordRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IManuRequistionOrderDetailRepository manuRequistionOrderDetailRepository,
            Data.Repositories.Manufacture.IManuReturnOrderRepository manuReturnOrderRepository,
            AbstractValidator<PlanWorkOrderChangeStatusDto> validationChangeStatusRules,
            IPlanWorkPlanProductRepository planWorkPlanProductRepository,
            IManuProductReceiptOrderService manuProductReceiptOrderService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _planWorkOrderRepository = planWorkOrderRepository;

            _procMaterialRepository = procMaterialRepository;
            _procBomRepository = procBomRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _planWorkOrderStatusRecordRepository = planWorkOrderStatusRecordRepository;
            _planWorkOrderActivationRecordRepository = planWorkOrderActivationRecordRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _validationChangeStatusRules = validationChangeStatusRules;
            _manuRequistionOrderRepository = manuRequistionOrderRepository;
            _manuRequistionOrderDetailRepository = manuRequistionOrderDetailRepository;
            _manuReturnOrderRepository = manuReturnOrderRepository;
            _planWorkPlanProductRepository = planWorkPlanProductRepository;
            _manuProductReceiptOrderService = manuProductReceiptOrderService;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="planWorkOrderCreateDto"></param>
        /// <returns></returns>
        public async Task<long> CreatePlanWorkOrderAsync(PlanWorkOrderCreateDto planWorkOrderCreateDto)
        {
            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(planWorkOrderCreateDto);

            planWorkOrderCreateDto.OrderCode = planWorkOrderCreateDto.OrderCode.ToUpper();
            // 判断编号是否已存在
            var haveEntities = await _planWorkOrderRepository.GetEqualPlanWorkOrderEntitiesAsync(new PlanWorkOrderQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                OrderCode = planWorkOrderCreateDto.OrderCode
            });
            if (haveEntities != null && haveEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16001)).WithData("orderCode", planWorkOrderCreateDto.OrderCode);
            }

            // DTO转换实体
            var planWorkOrderEntity = planWorkOrderCreateDto.ToEntity<PlanWorkOrderEntity>();
            planWorkOrderEntity.Id = IdGenProvider.Instance.CreateId();
            planWorkOrderEntity.CreatedBy = _currentUser.UserName;
            planWorkOrderEntity.UpdatedBy = _currentUser.UserName;
            planWorkOrderEntity.CreatedOn = HymsonClock.Now();
            planWorkOrderEntity.UpdatedOn = HymsonClock.Now();
            planWorkOrderEntity.SiteId = _currentSite.SiteId ?? 0;

            var planWorkOrderRecordEntity = new PlanWorkOrderRecordEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                UpdatedBy = _currentUser.UserName,
                CreatedBy = _currentUser.UserName,
                SiteId = _currentSite.SiteId ?? 0,
                WorkOrderId = planWorkOrderEntity.Id,
                InputQty = 0,
                UnqualifiedQuantity = 0,
                FinishProductQuantity = 0,
                PassDownQuantity = 0,
            };

            // 检查工艺路线
            var procProcessRouteEntity = await _procProcessRouteRepository.GetByIdAsync(planWorkOrderEntity.ProcessRouteId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10438));

            // 工艺路线状态校验
            if (procProcessRouteEntity.Status != SysDataStatusEnum.Enable && procProcessRouteEntity.Status != SysDataStatusEnum.Retain)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10448));
            }

            // 入库
            using var ts = TransactionHelper.GetTransactionScope();
            var response = await _planWorkOrderRepository.InsertAsync(planWorkOrderEntity);

            if (response == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16002));
            }
            await _planWorkOrderRepository.InsertPlanWorkOrderRecordAsync(planWorkOrderRecordEntity);
            ts.Complete();
            return planWorkOrderEntity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="planWorkOrderModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyPlanWorkOrderAsync(PlanWorkOrderModifyDto planWorkOrderModifyDto)
        {
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(planWorkOrderModifyDto);

            //获取当前最新的数据  进行状态判断
            var current = await _planWorkOrderRepository.GetByIdAsync(planWorkOrderModifyDto.Id);
            if (current != null)
            {
                //判断当前状态  
                if (current.Status != PlanWorkOrderStatusEnum.NotStarted)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16046));
                }
            }
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16003));
            }


            // DTO转换实体
            var planWorkOrderEntity = planWorkOrderModifyDto.ToEntity<PlanWorkOrderEntity>();
            planWorkOrderEntity.UpdatedBy = _currentUser.UserName;
            planWorkOrderEntity.UpdatedOn = HymsonClock.Now();

            var response = await _planWorkOrderRepository.UpdateAsync(planWorkOrderEntity);
            if (response == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16004));
            }
        }

        /// <summary>
        /// 修改工单状态
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public async Task ModifyWorkOrderStatusAsync(List<PlanWorkOrderChangeStatusDto> parms)
        {
            if (parms == null || !parms.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            foreach (var item in parms)
            {
                //校验
                await _validationChangeStatusRules.ValidateAndThrowAsync(item);
            }

            //查询需要改变的工单
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(parms.Select(x => x.Id).ToArray());
            if (workOrders == null || !workOrders.Any() || workOrders.Any(x => x.IsDeleted > 0) || workOrders.Count() != parms.Count)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16014));
            }

            #region//判断订单是否可以继续修改状态 

            if (parms.First().Status == PlanWorkOrderStatusEnum.SendDown) //需要修改为已下达的
            {
                foreach (var item in workOrders)
                {
                    if (item.Status != PlanWorkOrderStatusEnum.NotStarted)//判断是否有不是未开始的则无法更改状态
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16006));
                    }
                }
            }
            if (parms.First().Status == PlanWorkOrderStatusEnum.Finish) //需要修改为完工的
            {
                foreach (var item in workOrders)
                {
                    if (item.Status != PlanWorkOrderStatusEnum.InProduction)//判断是否有不是生产中的则无法更改状态
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16011));
                    }
                }
            }
            if (parms.First().Status == PlanWorkOrderStatusEnum.Closed) //需要修改为关闭的
            {
                foreach (var item in workOrders)
                {
                    if (item.Status != PlanWorkOrderStatusEnum.Finish)//判断是否有不是完工的则无法更改状态
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES16012));
                    }
                }
            }

            #endregion

            List<UpdateStatusNewCommand> planWorkOrderEntities = new List<UpdateStatusNewCommand>();
            List<long> updateWorkOrderRealEndList = new List<long>();
            List<long> deleteActivationWorkOrderIds = new List<long>();//需要取消激活工单

            foreach (var item in parms)
            {
                var workOrder = workOrders.FirstOrDefault(x => x.Id == item.Id);
                if (workOrder != null)
                {
                    planWorkOrderEntities.Add(new UpdateStatusNewCommand()
                    {
                        Id = item.Id,
                        Status = item.Status,
                        BeforeStatus = workOrder.Status,
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    });
                }

                //对是需要修改为关闭状态的做特殊处理： 给 工单记录表 更新 真实结束时间
                if (item.Status == PlanWorkOrderStatusEnum.Closed)
                {
                    updateWorkOrderRealEndList.Add(item.Id);
                }

                //对是需要修改为关闭状态的做特殊处理： 取消掉 对应工单激活的信息
                if (item.Status == PlanWorkOrderStatusEnum.Closed)
                {
                    deleteActivationWorkOrderIds.Add(item.Id);
                }
            }

            List<PlanWorkOrderActivationRecordEntity> planWorkOrderActivationRecordEntitys = new List<PlanWorkOrderActivationRecordEntity>();//对取消激活的做记录
            if (deleteActivationWorkOrderIds.Any())
            {
                var deleteActivationWorkOrders = await _planWorkOrderActivationRepository.GetByWorkOrderIdsAsync(deleteActivationWorkOrderIds.ToArray());
                foreach (var item in deleteActivationWorkOrders)
                {
                    planWorkOrderActivationRecordEntitys.Add(new PlanWorkOrderActivationRecordEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0,

                        WorkOrderId = item.WorkOrderId,
                        LineId = item.LineId,

                        ActivateType = PlanWorkOrderActivateTypeEnum.CancelActivate
                    });
                }
            }


            //组装工单状态变化记录
            List<PlanWorkOrderStatusRecordEntity> planWorkOrderStatusRecordEntities = new List<PlanWorkOrderStatusRecordEntity>();
            foreach (var item in workOrders)
            {
                var record = AutoMapperConfiguration.Mapper.Map<PlanWorkOrderStatusRecordEntity>(item);
                record.Id = IdGenProvider.Instance.CreateId();
                record.CreatedBy = _currentUser.UserName;
                record.UpdatedBy = _currentUser.UserName;
                record.CreatedOn = HymsonClock.Now();
                record.UpdatedOn = HymsonClock.Now();
                record.SiteId = _currentSite.SiteId ?? 0;
                record.IsDeleted = 0;

                planWorkOrderStatusRecordEntities.Add(record);
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                var response = await _planWorkOrderRepository.ModifyWorkOrderStatusAsync(planWorkOrderEntities);
                if (response != planWorkOrderEntities.Count)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16037));
                }

                if (updateWorkOrderRealEndList.Any()) //对是需要修改为关闭状态的做特殊处理： 给 工单记录表 更新 真实结束时间
                {
                    UpdateWorkOrderRealTimeCommand command = new UpdateWorkOrderRealTimeCommand()
                    {
                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        WorkOrderIds = updateWorkOrderRealEndList.ToArray()
                    };
                    await _planWorkOrderRepository.UpdatePlanWorkOrderRealEndByWorkOrderIdAsync(command);
                }

                //对是需要修改为关闭状态的做特殊处理： 取消掉 对应工单激活的信息
                if (deleteActivationWorkOrderIds.Any())
                {
                    await _planWorkOrderActivationRepository.DeletesTrueByWorkOrderIdsAsync(deleteActivationWorkOrderIds.ToArray());
                    if (planWorkOrderActivationRecordEntitys.Any())
                    {
                        await _planWorkOrderActivationRecordRepository.InsertsAsync(planWorkOrderActivationRecordEntitys);
                    }
                }

                await _planWorkOrderStatusRecordRepository.InsertsAsync(planWorkOrderStatusRecordEntities);

                ts.Complete();

            }
        }

        /// <summary>
        /// 修改工单是否锁定
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public async Task ModifyWorkOrderLockedAsync(List<PlanWorkOrderLockedDto> parms)
        {
            if (parms == null || parms.Count == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            #region//判断订单是否可以继续修改为锁定/解锁  且组装数据
            //查询需要改变的工单
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(parms.Select(x => x.Id).ToArray());
            if (workOrders == null || !workOrders.Any() || workOrders.Any(x => x.IsDeleted > 0) || workOrders.Count() != parms.Count)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16014));
            }

            List<UpdateLockedCommand> updateLockedCommands = new();

            if (parms.First().IsLocked == YesOrNoEnum.Yes) //需要修改为锁定
            {
                if (workOrders.Any(x => x.Status != PlanWorkOrderStatusEnum.InProduction))//判断是否有不是生产中的则无法更改为锁定
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16007));
                }

                foreach (var item in workOrders)
                {
                    updateLockedCommands.Add(new UpdateLockedCommand()
                    {
                        Id = item.Id,
                        Status = PlanWorkOrderStatusEnum.Pending,
                        LockedStatus = item.Status,

                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    });
                }
            }
            else  //解锁操作
            {
                if (workOrders.Any(x => x.Status != PlanWorkOrderStatusEnum.Pending))//判断是否是锁定中
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16008));
                }

                if (workOrders.Any(x => !x.LockedStatus.HasValue))//判断是否锁定前是否有值
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16015));
                }

                foreach (var item in workOrders)
                {
                    updateLockedCommands.Add(new UpdateLockedCommand()
                    {
                        Id = item.Id,
                        Status = item.LockedStatus!.Value,
                        LockedStatus = null,

                        UpdatedBy = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now()
                    });
                }
            }

            #endregion

            //组装工单状态变化记录
            List<PlanWorkOrderStatusRecordEntity> planWorkOrderStatusRecordEntities = new List<PlanWorkOrderStatusRecordEntity>();
            foreach (var item in workOrders)
            {
                var record = AutoMapperConfiguration.Mapper.Map<PlanWorkOrderStatusRecordEntity>(item);
                record.Id = IdGenProvider.Instance.CreateId();
                record.CreatedBy = _currentUser.UserName;
                record.UpdatedBy = _currentUser.UserName;
                record.CreatedOn = HymsonClock.Now();
                record.UpdatedOn = HymsonClock.Now();
                record.SiteId = _currentSite.SiteId ?? 0;
                record.IsDeleted = 0;

                planWorkOrderStatusRecordEntities.Add(record);
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                var response = await _planWorkOrderRepository.ModifyWorkOrderLockedAsync(updateLockedCommands);

                await _planWorkOrderStatusRecordRepository.InsertsAsync(planWorkOrderStatusRecordEntities);//新增工单变化记录表

                if (response == parms.Count)
                {
                    ts.Complete();
                }
                else
                {
                    var errCode = parms.First().IsLocked == YesOrNoEnum.Yes ? ErrorCode.MES16009 : ErrorCode.MES16010;
                    throw new CustomerValidationException(nameof(errCode));
                }
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
            //检查工单状态
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(idsArr);
            foreach (var item in workOrders)
            {
                if (item.Status != PlanWorkOrderStatusEnum.NotStarted)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16013));
                }
            }

            return await _planWorkOrderRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据：生产工单，列表查询，所调接口
        /// </summary>
        /// <param name="planWorkOrderPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkOrderListDetailViewDto>> GetPageListAsync(PlanWorkOrderPagedQueryDto planWorkOrderPagedQueryDto)
        {
            List<PlanWorkOrderListDetailViewDto> viewDtos = new List<PlanWorkOrderListDetailViewDto>();
            IEnumerable<ManuRequistionOrderEntity> requistionOrderEntities = new List<ManuRequistionOrderEntity>();

            var siteId = _currentSite.SiteId ?? 0;
            if (!string.IsNullOrWhiteSpace(planWorkOrderPagedQueryDto.PickCode))
            {
                //PickCode可能是领料单或者退料单，先查询领料单,查不到就去查退料单
                var pickCode = planWorkOrderPagedQueryDto.PickCode.Trim();
                var orderIds = new List<long>();

                requistionOrderEntities = await _manuRequistionOrderRepository.GetManuRequistionOrderEntitiesAsync(new ManuRequistionQueryByWorkOrders
                {
                    SiteId = siteId,
                    ReqOrderCode = pickCode,
                });
                if (requistionOrderEntities == null || !requistionOrderEntities.Any())
                {
                    var returnOrderEntities = await _manuReturnOrderRepository.GetEntitiesAsync(new Data.Repositories.Manufacture.Query.ManuReturnOrderQuery
                    {
                        SiteId = siteId,
                        ReturnOrderCodeValue = pickCode
                    });
                    if (returnOrderEntities == null || !returnOrderEntities.Any())
                    {
                        return new PagedInfo<PlanWorkOrderListDetailViewDto>(viewDtos, planWorkOrderPagedQueryDto.PageIndex, planWorkOrderPagedQueryDto.PageSize, 0);
                    }
                    orderIds = returnOrderEntities.Select(x => x.WorkOrderId).ToList();
                }
                else
                {
                    orderIds = requistionOrderEntities.Select(x => x.WorkOrderId).ToList();
                }

                var pagedQuery = planWorkOrderPagedQueryDto.ToQuery<PlanWorkOrderPagedQuery>();
                pagedQuery.SiteId = _currentSite.SiteId;
                pagedQuery.WorkOrderIds = orderIds.Distinct().ToList();
                var pagedInfo = await _planWorkOrderRepository.GetPagedInfoWithPickAsync(pagedQuery);

                // 实体到DTO转换 装载数据
                var dtos = pagedInfo.Data.Select(s => s.ToModel<PlanWorkOrderListDetailViewDto>());

                List<PlanWorkOrderListDetailViewDto> dtolist = dtos.ToList();
                // TODO: 由于工单没有领料状态字段，所以根据领料记录判断工单领料状态。。。
                dtolist.ForEach(d =>
                {
                    // 现在不按照工单生产数量进行领料，只标记未领料和已领料状态
                    d.PickStatus = requistionOrderEntities != null && requistionOrderEntities.Any(x => x.Status != WhMaterialPickingStatusEnum.CancelMaterialReturn && x.WorkOrderId == d.Id) ? PlanWorkOrderPickStatusEnum.FinishPicked : PlanWorkOrderPickStatusEnum.NotPicked;
                    d.PassDownQuantity = d.Qty;
                });
                return new PagedInfo<PlanWorkOrderListDetailViewDto>(dtolist, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }
            else
            {
                var pagedQuery = planWorkOrderPagedQueryDto.ToQuery<PlanWorkOrderPagedQuery>();
                pagedQuery.SiteId = _currentSite.SiteId;
                var pagedInfo = await _planWorkOrderRepository.GetPagedInfoWithPickAsync(pagedQuery);

                // 实体到DTO转换 装载数据
                var dtos = pagedInfo.Data.Select(s => s.ToModel<PlanWorkOrderListDetailViewDto>());
                // 根据工单查找领料记录，汇总已领料数量，根据已领料数量和工单数量 计算出领料状态
                requistionOrderEntities = await _manuRequistionOrderRepository.GetManuRequistionOrderEntitiesAsync(new ManuRequistionQueryByWorkOrders
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    WorkOrderIds = dtos.Select(d => d.Id).Distinct(),
                });

                List<PlanWorkOrderListDetailViewDto> dtolist = dtos.ToList();
                // TODO: 由于工单没有领料状态字段，所以根据领料记录判断工单领料状态。。。
                dtolist.ForEach(async d =>
                {
                    // 现在不按照工单生产数量进行领料，只标记未领料和已领料状态
                    //var qty = requistiongroup.FirstOrDefault(r => r.Key == d.Id)?.Count() ?? 0;
                    d.PickStatus = requistionOrderEntities.Any(x => x.Status != WhMaterialPickingStatusEnum.CancelMaterialReturn && x.WorkOrderId == d.Id) ? PlanWorkOrderPickStatusEnum.FinishPicked : PlanWorkOrderPickStatusEnum.NotPicked;
                    d.PassDownQuantity = d.Qty;
                    var result = await _manuProductReceiptOrderService.QueryByWorkIdByScwAsync(d.Id);
                    if (result != null)
                    {
                        d.FinishProductQuantity = result.SumQty;
                    }
                });
                return new PagedInfo<PlanWorkOrderListDetailViewDto>(dtolist, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }
        }

        /// <summary>
        /// 查询剩余可下单条码数量
        /// </summary>
        /// <param name="workOrderCode"></param>
        /// <returns></returns>
        public async Task<decimal> GetPlanWorkOrderByWorkOrderCodeAsync(string workOrderCode)
        {
            var query = new PlanWorkOrderQuery
            {
                OrderCode = workOrderCode,
                SiteId = _currentSite.SiteId ?? 0
            };
            var workOrderEntity = await _planWorkOrderRepository.GetByCodeAsync(query)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16003));

            // 应下达数量
            var residue = Math.Ceiling(workOrderEntity.Qty * (1 + workOrderEntity.OverScale / 100));

            // 查询已下发数量
            var workOrderRecordEntity = await _planWorkOrderRepository.GetByWorkOrderIdAsync(workOrderEntity.Id);
            if (workOrderRecordEntity != null && workOrderRecordEntity.PassDownQuantity.HasValue)
            {
                // 减掉已下达数量
                residue -= workOrderRecordEntity.PassDownQuantity.Value;
            }

            if (residue < 0) residue = 0;
            return residue;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderDetailViewDto> QueryPlanWorkOrderByIdAsync(long id)
        {
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(id);
            if (planWorkOrderEntity != null)
            {
                var planWorkOrderDetailView = planWorkOrderEntity.ToModel<PlanWorkOrderDetailViewDto>();

                //关联物料
                var material = await _procMaterialRepository.GetByIdAsync(planWorkOrderEntity.ProductId);
                if (material != null)
                {
                    planWorkOrderDetailView.MaterialCode = material.MaterialCode;
                    planWorkOrderDetailView.MaterialVersion = material.Version!;
                    planWorkOrderDetailView.MaterialName = material.MaterialName;
                    planWorkOrderDetailView.MaterialUnit = material.Unit;
                }

                //关联BOM
                var bom = await _procBomRepository.GetByIdAsync(planWorkOrderEntity.ProductBOMId);
                if (bom != null)
                {
                    planWorkOrderDetailView.BomCode = bom.BomCode;
                    planWorkOrderDetailView.BomVersion = bom.Version;
                }

                //关联工艺路线
                var processRoute = await _procProcessRouteRepository.GetByIdAsync(planWorkOrderEntity.ProcessRouteId);
                if (processRoute != null)
                {
                    planWorkOrderDetailView.ProcessRouteCode = processRoute.Code;
                    planWorkOrderDetailView.ProcessRouteVersion = processRoute.Version;
                }

                //关联工作中心
                var workCenter = await _inteWorkCenterRepository.GetByIdAsync(planWorkOrderEntity.WorkCenterId ?? 0);
                if (workCenter != null)
                {
                    planWorkOrderDetailView.WorkCenterCode = workCenter.Code;
                }

                //计算汇总数量和入库数量
                var result = await _manuProductReceiptOrderService.QueryByWorkIdByScwAsync(id);
                if (result != null)
                {
                    planWorkOrderDetailView.SumQty = result.SumQty;
                    planWorkOrderDetailView.ToBeTestQty = result.ToBeTestQty;
                    planWorkOrderDetailView.FinishQty = result.FinishQty;
                    planWorkOrderDetailView.BadQty = result.BadQty;
                }

                return planWorkOrderDetailView;
            }
            return new PlanWorkOrderDetailViewDto();
        }

        public async Task<List<ManuRequistionOrderEntity>> GetPickHistoryByWorkOrderIdAsync(long workOrderId)
        {
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(workOrderId);
            if (planWorkOrderEntity != null)
            {
                var requistionOrderEntities = await _manuRequistionOrderRepository.GetByOrderCodeAsync(planWorkOrderEntity.Id, planWorkOrderEntity.SiteId);
                var lst = requistionOrderEntities.ToList();
                foreach (var item in lst)
                {
                    item.ReqOrderCode = $"{planWorkOrderEntity.OrderCode}_{item.Id}";
                }
                return lst;
            }
            return new List<ManuRequistionOrderEntity>();
        }

        /// <summary>
        /// 根据工单查询领料明细
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<List<ManuRequistionOrderDetailDto>> GetPickDetailByOrderIdAsync(long workOrderId)
        {
            var details = new List<ManuRequistionOrderDetailDto>();
            //根据工单ID，查询工单信息（plan_work_order）
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(workOrderId);
            if (planWorkOrderEntity == null)
            {
                return details;
            }

            //根据工单ID，查询生产领料单信息（manu_requistion_order）
            var requistionOrderEntities = await _manuRequistionOrderRepository.GetByOrderCodeAsync(planWorkOrderEntity.Id, planWorkOrderEntity.SiteId);
            if (requistionOrderEntities == null || !requistionOrderEntities.Any())
            {
                return details;
            }

            //组装生产领料单ID集合
            var requistionOrderIds = requistionOrderEntities.Select(x => x.Id).ToArray();
            //根据生产领料单ID，查询生产领料单明细信息（manu_requistion_order_detail）
            var manuRequistionOrderDetails = await _manuRequistionOrderDetailRepository.GetManuRequistionOrderDetailEntitiesAsync(new ManuRequistionOrderDetailQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                RequistionOrderIds = requistionOrderIds
            });

            //组装物料ID集合
            var materialIds = manuRequistionOrderDetails.Select(x => x.MaterialId).ToArray();
            //根据物料ID集合，获取物料列表
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(materialIds);

            foreach (var item in manuRequistionOrderDetails)
            {
                var requistionOrder = requistionOrderEntities.FirstOrDefault(x => x.Id == item.RequistionOrderId);
                var material = procMaterials.FirstOrDefault(x => x.Id == item.MaterialId);
                details.Add(new ManuRequistionOrderDetailDto
                {
                    ReqOrderCode = requistionOrder?.ReqOrderCode ?? "",
                    MaterialCode = material?.MaterialCode ?? "",
                    Version = material?.Version ?? "",
                    MaterialName = material?.MaterialName ?? "",
                    //MaterialBarCode = material?.MaterialBarCode??"",
                    //Batch = item.Batch,
                    Qty = item.Qty,
                    PickTime = requistionOrder?.CreatedOn ?? item.CreatedOn,
                    Status = requistionOrder?.Status,
                    CreatedBy = requistionOrder?.CreatedBy ?? "-"
                });
            }
            return details;
        }


        /// <summary>
        /// 根据工单查询领料明细
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public async Task<List<ManuRequistionOrderDetailByScwDto>> GetPickDetailByOrderIdByScwAsync(long workOrderId)
        {
            var details = new List<ManuRequistionOrderDetailByScwDto>();
            //根据工单ID，查询工单信息（plan_work_order）
            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(workOrderId);
            if (planWorkOrderEntity == null)
            {
                return details;
            }

            //根据工单ID，查询生产领料单信息（manu_requistion_order），领料状态需除去【5-申请取消】的
            var requistionOrderEntities = await _manuRequistionOrderRepository.GetByOrderCodeByScwAsync(planWorkOrderEntity.Id, planWorkOrderEntity.SiteId);
            if (requistionOrderEntities == null || !requistionOrderEntities.Any())
            {
                return details;
            }

            //组装生产领料单ID集合
            var requistionOrderIds = requistionOrderEntities.Select(x => x.Id).ToArray();
            //根据生产领料单ID，查询生产领料单明细信息（manu_requistion_order_detail）
            var manuRequistionOrderDetails = await _manuRequistionOrderDetailRepository.GetManuRequistionOrderDetailEntitiesAsync(new ManuRequistionOrderDetailQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                RequistionOrderIds = requistionOrderIds
            });

            //组装物料ID集合
            var materialIds = manuRequistionOrderDetails.Select(x => x.MaterialId).ToArray();
            //根据物料ID集合，获取物料列表
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(materialIds);

            //遍历工单Bom领料的物料列表
            foreach (var procMaterial in procMaterials)
            {
                //已领料数量初始化
                var detailQty = 0M;

                //领料中数量初始化
                var detailPickingQty = 0M;

                //遍历工单领料记录的物料记录
                foreach (var item in manuRequistionOrderDetails)
                {
                    var material = procMaterials.FirstOrDefault(x => x.Id == item.MaterialId);

                    //计算已领料数量的和【已领料数量：取值工单领料记录中领料状态为“发料完成”的数量】
                    if (material != null && procMaterial.MaterialCode == material.MaterialCode && procMaterial.Id == item.MaterialId)
                    {
                        foreach (var requistionOrderEntity in requistionOrderEntities)
                        {
                            //计算领料中数量的和，领料状态为：发料完成
                            if (requistionOrderEntity.Id == item.RequistionOrderId && requistionOrderEntity.Status == WhMaterialPickingStatusEnum.Completed)
                            {
                                detailQty += item.Qty;
                                break;
                            }
                        }
                    }

                    //2024.12.5号，新增的需求：计算领料中数量的和【领料中数量：取值工单领料记录中领料状态为“申请成功待发料” + “发料中”的数量】
                    if (material != null && procMaterial.MaterialCode == material.MaterialCode && procMaterial.Id == item.MaterialId)
                    {
                        foreach (var requistionOrderEntity in requistionOrderEntities)
                        {
                            //计算领料中数量的和，领料状态为：“申请成功待发料” + “发料中”
                            if (requistionOrderEntity.Id == item.RequistionOrderId 
                                && (requistionOrderEntity.Status == WhMaterialPickingStatusEnum.ApplicationSuccessful 
                                || requistionOrderEntity.Status == WhMaterialPickingStatusEnum.Inspectioning))
                            {
                                detailPickingQty += item.Qty;
                                break;
                            }
                        }
                    }
                }

                details.Add(new ManuRequistionOrderDetailByScwDto
                {
                    MaterialCode = procMaterial.MaterialCode,
                    Qty = detailQty,
                    PickingQty = detailPickingQty
                });
            }
            return details;
        }

        /// <summary>
        /// 根据工单id修改计划数
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> EditWorderPlanQtyByIdAsync(EditPlanWorkOrderDto dto)
        {
            if (dto.PlanQty == 0) throw new CustomerValidationException(nameof(ErrorCode.MES16022));

            var planWorkOrderEntity = await _planWorkOrderRepository.GetByIdAsync(dto.Id);
            if (planWorkOrderEntity == null) return false;

            var planProductEntity = await _planWorkPlanProductRepository.GetByIdAsync(planWorkOrderEntity.WorkPlanProductId ?? 0)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES16018));

            var workOrderEntities = await _planWorkOrderRepository.GetByPlanProductIdAsync(planProductEntity.Id);
            var sumWorkOrderNum = workOrderEntities.Sum(x => x.Qty);
            if ((dto.PlanQty + sumWorkOrderNum - planWorkOrderEntity.Qty) > planProductEntity.Qty)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16058));
            }

            // 修改工单计划数
            var rows = await _planWorkOrderRepository.UpdateQtyAsync(new UpdateQtyByWorkOrderIdCommand
            {
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = DateTime.UtcNow,
                WorkOrderId = planWorkOrderEntity.Id,
                Qty = dto.PlanQty,
            });

            return rows > 0;
        }

    }
}
