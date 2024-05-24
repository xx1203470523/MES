using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Report.MaterialDeductionRecord
{
    public class MaterialDeductionRecordService : IMaterialDeductionRecordService
    {
        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 工单表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 流转表 仓储
        /// </summary>
        private readonly IManuBarCodeRelationRepository _manuBarCodeRelationRepository;

        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 资源仓储
        /// </summary>
        private readonly IProcResourceRepository _resourceRepository;

        /// <summary>
        /// 工序表 仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// BOM详情
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="manuBarCodeRelationRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="resourceRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        public MaterialDeductionRecordService(ICurrentSite currentSite, IManuBarCodeRelationRepository  manuBarCodeRelationRepository, IPlanWorkOrderRepository planWorkOrderRepository, IProcMaterialRepository procMaterialRepository, IProcResourceRepository resourceRepository, IProcProcedureRepository procProcedureRepository, IProcBomDetailRepository procBomDetailRepository)
        {
            _currentSite = currentSite;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuBarCodeRelationRepository = manuBarCodeRelationRepository;
            _procMaterialRepository = procMaterialRepository;
            _resourceRepository = resourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _procBomDetailRepository = procBomDetailRepository;
        }

        /// <summary>
        /// 扣料分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<MaterialDeductionRecordResultDto>> GetMaterialDeductionRecorPageListAsync(ComUsageReportPagedQueryDto param)
        {
            if (param == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var pagedQuery = param.ToQuery<ComUsageReportPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            pagedQuery.CirculationType = ManuBarCodeRelationTypeEnum.SFC_Consumption;
            var pagedInfo = await _manuBarCodeRelationRepository.GetReportPagedInfoAsync(pagedQuery);

            List<MaterialDeductionRecordResultDto> listDto = new List<MaterialDeductionRecordResultDto>();

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<MaterialDeductionRecordResultDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }

            // 获取工单
            var workOrderIds = pagedInfo.Data.Select(x => x.OutputBarCodeWorkOrderId).Distinct().ToArray();
            var workOrders = await _planWorkOrderRepository.GetByIdsAsync(workOrderIds.OfType<long>());

            // 获取资源
            var resourceIds = pagedInfo.Data.Select(x => x.ResourceId).Distinct().ToArray();
            var resources = await _resourceRepository.GetResByIdsAsync(resourceIds.OfType<long>());

            // 获取工序
            var procedureIds = pagedInfo.Data.Select(x => x.ProcedureId).Distinct().ToArray();
            var procedures = await _procProcedureRepository.GetByIdsAsync(procedureIds.OfType<long>());

            // 获取物料
            var productIds = pagedInfo.Data.Select(x => x.OutputBarCodeMaterialId).Distinct().ToArray();
            var products = await _procMaterialRepository.GetByIdsAsync(productIds.OfType<long>());

            //查询Bom详情
            var bomDetails = await _procBomDetailRepository.GetProcBomDetailEntitiesAsync(
                new ProcBomDetailQuery
                {
                    SiteId = _currentSite.SiteId ?? 123456,
                    MaterialIds = productIds.OfType<long>(),
                    ProcedureIds = procedureIds.OfType<long>(),
                }
                );

            foreach (var item in pagedInfo.Data)
            {
                var product = products.FirstOrDefault(x => x.Id == item.OutputBarCodeMaterialId);
                var workOrder = workOrders.FirstOrDefault(x => x.Id == item.OutputBarCodeWorkOrderId);
                var procedure = procedures.FirstOrDefault(x => x.Id == item.ProcedureId);
                var resource = resources.FirstOrDefault(x => x.Id == item.ResourceId);
                var bomUsgQty = bomDetails.FirstOrDefault(x => x.MaterialId == item.OutputBarCodeMaterialId && x.ProcedureId == item.ProcedureId);
                listDto.Add(new MaterialDeductionRecordResultDto()
                {
                    Sfc = item.OutputBarCode,
                    MaterialRemark = product != null ? product.MaterialCode + "/" + product.Version : "",
                    WorkOrder = workOrder != null ? workOrder.OrderCode : "",
                    CirculationBarCode = item.InputBarCode,
                    CirculationQty = item.InputQty,
                    ProcedureName = procedure != null ? procedure.Code : "",
                    ResourceCode = resource != null ? resource.ResCode : "",
                    ResourceName = resource != null ? resource.ResName : "",
                    BomUsages = bomUsgQty != null ? bomUsgQty.Usages : 0,
                    CreateBy = item.CreatedBy,
                    CreateOn = item.CreatedOn,
                });
            }
            return new PagedInfo<MaterialDeductionRecordResultDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}
