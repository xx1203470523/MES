using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.Utils;

namespace Hymson.MES.BackgroundServices.Stator.Services
{
    /// <summary>
    /// 服务
    /// </summary>
    public class BaseService
    {
        /// <summary>
        /// 工序前缀
        /// </summary>
        protected const string PRODUCRE_PREFIX = "S01";

        /// <summary>
        /// 仓储接口（系统配置）
        /// </summary>
        private readonly ISysConfigRepository _sysConfigRepository;

        /// <summary>
        /// 仓储接口（工作中心）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 仓储接口（生产工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sysConfigRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        public BaseService(ISysConfigRepository sysConfigRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _sysConfigRepository = sysConfigRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
        }

        /// <summary>
        /// 获取基础配置（定子）
        /// </summary>
        /// <returns></returns>
        protected async Task<BaseStatorBo> GetStatorBaseConfigAsync()
        {
            // 初始化对象
            var baseDto = new BaseStatorBo
            {
                User = "StatorTask",
                Time = HymsonClock.Now()
            };

            // 读取站点配置
            var siteConfigEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.MainSite });
            if (siteConfigEntities == null || !siteConfigEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139))
                    .WithData("name", SysConfigEnum.MainSite.GetDescription());
            }

            // 站点配置
            var siteConfigEntity = siteConfigEntities.FirstOrDefault(f => f.Code == "MainSite")
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES12842)).WithData("Msg", "站点配置不存在！");

            // 站点赋值
            baseDto.SiteId = siteConfigEntity.Value.ParseToLong();

            // 读取产线配置
            var lineConfigEntities = await _sysConfigRepository.GetEntitiesAsync(new SysConfigQuery { Type = SysConfigEnum.NioBaseConfig });
            if (lineConfigEntities == null || !lineConfigEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10139))
                    .WithData("name", SysConfigEnum.NioBaseConfig.GetDescription());
            }

            // 定子线配置
            var lineConfigEntity = lineConfigEntities.FirstOrDefault(f => f.Code == "NioStatorConfig")
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES12842)).WithData("Msg", "定子线配置不存在！");

            // 定子线赋值
            var configBo = lineConfigEntity.Value.ToDeserialize<NIOConfigBaseDto>();
            if (configBo != null)
            {
                // 读取产线
                var workLineEntity = await _inteWorkCenterRepository.GetByCodeAsync(new EntityByCodeQuery
                {
                    Site = baseDto.SiteId,
                    Code = configBo.ProductionLineId
                });
                if (workLineEntity == null) return baseDto;

                baseDto.WorkLineId = workLineEntity.Id;

                // 读取产线当前激活的工单
                var workOrderEntities = await _planWorkOrderRepository.GetByWorkLineIdAsync(workLineEntity.Id);
                if (workOrderEntities == null || !workOrderEntities.Any()) return baseDto;

                // 这里激活的工单应该只能有一个
                var workOrderEntity = workOrderEntities.FirstOrDefault();
                if (workOrderEntity == null) return baseDto;

                // 填充信息
                baseDto.WorkOrderId = workOrderEntity.Id;
                baseDto.ProductBOMId = workOrderEntity.ProductBOMId;
                baseDto.ProcessRouteId = workOrderEntity.ProcessRouteId;
                baseDto.ProductId = workOrderEntity.ProductId;
            }

            return baseDto;
        }

    }
}
