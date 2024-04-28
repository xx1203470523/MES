using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Microsoft.IdentityModel.Tokens;
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
        /// BOM
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 物料替代料
        /// </summary>
        private readonly IProcReplaceMaterialRepository _procReplaceMaterialRepository;

        /// <summary>
        /// 设备工单绑定仓储
        /// </summary>
        private readonly IPlanWorkOrderBindRepository _planWorkOrderBindRepository;

        /// <summary>
        /// 工作中心仓储
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PlanWorkOrderService(IPlanWorkOrderRepository planWorkOrderRepository,
            IPlanWorkOrderActivationRepository planWorkOrderActivationRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcReplaceMaterialRepository procReplaceMaterialRepository,
            IPlanWorkOrderBindRepository planWorkOrderBindRepository,
            IInteWorkCenterRepository inteWorkCenterRepository)
        {
            _planWorkOrderRepository = planWorkOrderRepository;
            _planWorkOrderActivationRepository = planWorkOrderActivationRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procReplaceMaterialRepository = procReplaceMaterialRepository;
            _planWorkOrderBindRepository = planWorkOrderBindRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
        }

        /// <summary>
        /// 根据产线ID、资源ID获取工单数据（激活的工单）
        /// </summary>
        /// <param name="workLineId"></param>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<PlanWorkOrderEntity> GetByWorkLineIdAsync(long workLineId, long resourceId)
        {
            PlanWorkOrderEntity? orderEntity = null;
            //查询产线信息
            var workLineEntity = await _inteWorkCenterRepository.GetByIdAsync(workLineId);
            if (workLineEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12125));
            }
            //获取线体激活工单
            var activeOrderList = await _planWorkOrderRepository.GetByWorkLineIdAsync(workLineId);
            if (activeOrderList == null || !activeOrderList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45030));
            }
            if (activeOrderList.Count() == 1)
            {
                orderEntity = activeOrderList.First();
                return orderEntity;
            }
            if (workLineEntity.IsMixLine != true)
            {
                //不混线时 校验是否激活大于一个工单
                if (activeOrderList.Count() > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45031));
                }
                orderEntity = activeOrderList.First();
            }
            else
            {
                //获取资源绑定的工单
                var workOrderBindEntities = await _planWorkOrderBindRepository.GetPlanWorkOrderBindEntitiesAsync(new PlanWorkOrderBindQuery
                {
                    SiteId = workLineEntity.SiteId ?? 0,
                    ResourceId = resourceId
                });
                if (workOrderBindEntities == null || !workOrderBindEntities.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45034));
                }
                var tempOrderList = activeOrderList.Where(x => workOrderBindEntities.Select(z => z.WorkOrderId).Contains(x.Id));
                if (tempOrderList == null || tempOrderList.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES45035));
                }
                orderEntity = tempOrderList.First();
            }

            return orderEntity;
        }

        /// <summary>
        /// 获取工单对应的物料id
        /// 包括BOM替代料和物料自身的替代料
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        public async Task<List<long>> GetWorkOrderMaterialAsync(long bomId)
        {
            var matList = (await _procBomDetailRepository.GetListMainAsync(bomId)).ToList();
            var bomMatReplaceList = (await _procBomDetailRepository.GetListReplaceAsync(bomId)).ToList();

            if (matList.IsNullOrEmpty() == true && bomMatReplaceList.IsNullOrEmpty() == true)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45033));
            }

            //工单BOM的物料(包括替代料)
            List<ProcBomDetailView> resultList = new List<ProcBomDetailView>();
            resultList.AddRange(matList.ToList());
            resultList.AddRange(bomMatReplaceList.ToList());

            var matIdList = resultList.Select(m => m.MaterialId).ToList();

            //查询所有的替代料
            var matReplaceList = await _procReplaceMaterialRepository.GetListByMaterialIdAsync(matIdList);
            if (matReplaceList.IsNullOrEmpty() == false)
            {
                matIdList.AddRange(matReplaceList.Select(m => m.MaterialId).ToList());
            }

            return matIdList;
        }
    }
}
