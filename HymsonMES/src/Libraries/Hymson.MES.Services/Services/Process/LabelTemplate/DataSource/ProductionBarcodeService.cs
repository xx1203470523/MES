using Hymson.Authentication.JwtBearer;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process.LabelTemplate.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process.LabelTemplate.DataSource
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductionBarcodeService : IBarcodeDataSourceService
    {
        /// <summary>
        /// 条码仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 条码信息仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 仓储接口（条码生产信息）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 工单
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 仓储接口（工序维护）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;


        /// <summary>
        /// 获取生产执行类
        /// </summary>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        public ProductionBarcodeService(IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository, IPlanWorkOrderRepository planWorkOrderRepository,
            IProcMaterialRepository procMaterialRepository, IProcResourceRepository procResourceRepository,
            IProcProcedureRepository procProcedureRepository, IEquEquipmentRepository equEquipmentRepository
            )
        {
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _equEquipmentRepository = equEquipmentRepository;
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<string> GetLabelTemplateData(BaseLabelTemplateDataDto param)
        {
            var manuSfcEntitiesTask = _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SiteId = param.SiteId,
                SFCs = param.BarCodes,
                Type = SfcTypeEnum.Produce
            });
            var manuSfcProduceEntitiesTask = _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery { SiteId = param.SiteId, Sfcs = param.BarCodes });

            var manuSfcEntities = await manuSfcEntitiesTask;
            var manuSfcProduceEntities = await manuSfcProduceEntitiesTask;

            var manuSfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(manuSfcEntities.Select(x => x.Id));

            var planWorkOrderEntitiesTask = _planWorkOrderRepository.GetByIdsAsync(manuSfcInfoEntities.Select(x => x.WorkOrderId ?? 0));
            var procMaterialEntitiesTask = _procMaterialRepository.GetByIdsAsync(manuSfcInfoEntities.Select(x => x.ProductId));
            var procProcedureEntitiesTask = _procProcedureRepository.GetByIdsAsync(manuSfcProduceEntities.Select(x => x.ProcedureId));
            var equEquipmentEntitiesTask = _equEquipmentRepository.GetByIdAsync(manuSfcProduceEntities.Select(x => x.EquipmentId ?? 0));
            var procResourceEntitiesTask = _procResourceRepository.GetListByIdsAsync(manuSfcProduceEntities.Select(x => x.ResourceId ?? 0));

            var planWorkOrderEntities = await planWorkOrderEntitiesTask;
            var procMaterialEntities = await procMaterialEntitiesTask;
            var procProcedureEntities = await procProcedureEntitiesTask;
            var equEquipmentEntities = await equEquipmentEntitiesTask;
            var procResourceEntities = await procResourceEntitiesTask;

            var productionBarcodeList = new List<ProductionBarcodeDto>();
            foreach (var item in manuSfcEntities)
            {
                var manuSfcInfoEntity = manuSfcInfoEntities.FirstOrDefault(x => x.SfcId == item.Id);

                var procMaterialEntity = procMaterialEntities.FirstOrDefault(x => x.Id == manuSfcInfoEntity?.ProductId);
                var planWorkOrderEntity = planWorkOrderEntities.FirstOrDefault(x => x.Id == manuSfcInfoEntity?.WorkOrderId);

                var manuSfcProduceEntity = manuSfcProduceEntities.FirstOrDefault(x => x.SFC == item.SFC);
                var procProcedureEntity = procProcedureEntities.FirstOrDefault(x => x.Id == manuSfcProduceEntity?.ProcedureId);
                var procResourceEntity = procResourceEntities.FirstOrDefault(x => x.Id == manuSfcProduceEntity?.ResourceId);
                var equEquipmentEntity = equEquipmentEntities.FirstOrDefault(x => x.Id == manuSfcProduceEntity?.EquipmentId);

                productionBarcodeList.Add(new ProductionBarcodeDto
                {
                    SFC = item.SFC,
                    WorkOrderCode = planWorkOrderEntity?.OrderCode ?? "",
                    ProductCode = procMaterialEntity?.MaterialCode ?? "",
                    ProductName = procMaterialEntity?.MaterialName ?? "",
                    Qty = item.Qty,
                    ProcedureCode = procProcedureEntity?.Code,
                    ProcedureName = procProcedureEntity?.Name,
                    ResourceCode = procResourceEntity?.ResCode,
                    ResourceName = procResourceEntity?.ResName,
                    EquipmentCode = equEquipmentEntity?.EquipmentCode,
                    EquipmentName = equEquipmentEntity?.EquipmentName,
                    Status = ""
                });
            }
            await Task.CompletedTask;
            return null;
        }
    }
}
