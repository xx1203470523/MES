using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Services.Qkny.PlanWorkOrder
{
    /// <summary>
    /// 工单服务
    /// </summary>
    public class PlanWorkOrderService : IPlanWorkOrderService
    {
        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 工单激活表
        /// </summary>
        private readonly IPlanWorkOrderActivationRepository _planWorkOrderActivationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PlanWorkOrderService(IPlanWorkOrderRepository planWorkOrderRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository)
        {
            _planWorkOrderRepository = planWorkOrderRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
        }

        /// <summary>
        /// 根据产线ID获取工单数据（激活的工单）
        /// </summary>
        /// <param name="workLineId"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderEntity> GetByWorkLineIdAsync(long workLineId)
        {
            //获取线体对应工单
            var orderList = await _planWorkOrderRepository.GetByWorkLineIdAsync(workLineId);
            //1. 校验是否存在工单
            if(orderList == null || orderList.Any() == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45030));
            }
            //2. 查询激活工单
            orderList = orderList.Where(m => m.Status == PlanWorkOrderStatusEnum.InProduction).ToList(); //激活后会变成生产中
            List<long> orderIdList = orderList.Select(m => m.Id).ToList();
            var activeList = await _planWorkOrderActivationRepository.GetByIdsAsync(orderIdList.ToArray());
            if(activeList == null || activeList.Any() != false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45030));
            }
            //3. 校验是否激活大于一个工单
            if(activeList.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45031));
            }

            //获取激活工单
            var activeIdList = activeList.Select(m => m.WorkOrderId).ToList();
            var curOrder = orderList.Where(m => activeIdList.Contains(m.Id)).FirstOrDefault();
            return curOrder;
        }
    }
}
