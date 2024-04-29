using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Report;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 
    /// </summary>
    public class OriginalSummaryReportService : IOriginalSummaryReportService
    {
        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 服务接口（主数据）
        /// </summary>
        private readonly IMasterDataService _masterDataService;

        /// <summary>
        /// BOM表仓储接口
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;
        /// <summary>
        /// BOM明细表仓储接口
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;
        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        /// <summary>
        /// 工序表 仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;
        /// <summary>
        /// 资源仓储
        /// </summary>
        private readonly IProcResourceRepository _resourceRepository;
        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="masterDataService"></param>
        /// <param name="procBomRepository"></param>
        /// <param name="procBomDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="resourceRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        public OriginalSummaryReportService(ICurrentSite currentSite,
            IMasterDataService masterDataService,
            IProcBomRepository procBomRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceRepository resourceRepository,
            IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _currentSite = currentSite;
            _masterDataService = masterDataService;
            _procBomRepository = procBomRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _resourceRepository = resourceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
        }

        /// <summary>
        /// 根据ID查询Bom 主物料以及组件信息详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<List<OriginalSummaryReportDto>> GetOriginalSummaryAsync(OriginalSummaryQueryDto queryDto)
        {
            if (queryDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            var bomDetailViews = new List<OriginalSummaryReportDto>();

            // 查询组件信息
            var manuSfcCirculations = await _masterDataService.GetSFCCirculationEntitiesByTypesAsync(new SFCCirculationBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = queryDto.Sfc,
                Type = queryDto.Type
            });
            if (!manuSfcCirculations.Any()) return bomDetailViews;

            // 拿到所有工单
            var orderIds = manuSfcCirculations.Select(x => x.WorkOrderId);
            var orders = await _planWorkOrderRepository.GetByIdsAsync(orderIds);
            if (!orders.Any()) return bomDetailViews;

            //根据工单拿到所有bom信息
            var bomIds = orders.Select(x => x.ProductBOMId).ToList();
            var boms = await _procBomRepository.GetByIdsAsync(bomIds.Distinct().ToArray());
            if (!boms.Any())
            {
                return bomDetailViews;
            }

            //查询bom明细
            var bomDetails = await _procBomDetailRepository.GetByBomIdsAsync(bomIds);
            if (!bomDetails.Any())
            {
                return bomDetailViews;
            }
            bomDetails = bomDetails.OrderBy(x => x.BomId).ToList();

            //组件物料
            var barCodeMaterialIds = manuSfcCirculations.Select(x => x.CirculationProductId).Distinct();

            //bom物料
            var bomMaterialIds = bomDetails.Select(item => item.MaterialId).Distinct();
            //工单物料
            var orderProducts = orders.Select(x => x.ProductId).Distinct();

            var materialIds = new List<long>();
            if (barCodeMaterialIds.Any())
            {
                materialIds.AddRange(barCodeMaterialIds);
            }
            if (bomMaterialIds.Any())
            {
                materialIds.AddRange(bomMaterialIds);
            }
            if (orderProducts.Any())
            {
                materialIds.AddRange(orderProducts);
            }

            //查询物料信息
            var procMaterials = new List<ProcMaterialEntity>();
            var materials = materialIds.Distinct();
            if (materials.Any())
            {
                procMaterials = (await _procMaterialRepository.GetByIdsAsync(materials.ToArray())).ToList();
            }

            //查询工序信息
            var procedureIds = bomDetails.Select(item => item.ProcedureId).ToArray();
            var procProcedures = new List<ProcProcedureEntity>();
            if (procedureIds.Any())
            {
                procProcedures = (await _procProcedureRepository.GetByIdsAsync(procedureIds)).ToList();
            }

            //查询资源信息
            var procResources = await GetResourcesAsync(manuSfcCirculations);

            foreach (var detailEntity in bomDetails)
            {
                //查询有没有挂载组件
                //var listCirculations = manuSfcCirculations.Where(a => a.ProcedureId == detailEntity.ProcedureId
                //                                && a.CirculationMainProductId == detailEntity.MaterialId).OrderByDescending(x => x.UpdatedOn).ToList();
                //查询有没有挂载组件
                var bomId = detailEntity.BomId;
                var orderId = orders.FirstOrDefault(x => x.ProductBOMId == bomId)?.Id ?? 0;
                var listCirculations = manuSfcCirculations.Where(a => a.WorkOrderId == orderId && a.CirculationMainProductId == detailEntity.MaterialId).OrderByDescending(x => x.UpdatedOn).ToList();
                if (!listCirculations.Any())
                {
                    continue;
                }

                var order = orders.FirstOrDefault(x => x.ProductBOMId == detailEntity.BomId);
                var product = procMaterials.FirstOrDefault(item => item.Id == order?.ProductId);
                var productBom = boms.FirstOrDefault(item => item.Id == order?.ProductBOMId);

                var material = procMaterials.FirstOrDefault(item => item.Id == detailEntity.MaterialId);
                var procedures = procProcedures.FirstOrDefault(item => item.Id == detailEntity.ProcedureId);

                var bomDetail = new OriginalSummaryReportDto
                {
                    BomId = detailEntity.BomId,
                    BomDetailId = detailEntity.Id,
                    ProductRemark = product?.MaterialCode + "/" + product?.Version,
                    ProductName = product?.MaterialName ?? "",
                    BomRemark = productBom?.BomCode + "/" + productBom?.Version,
                    BomName = productBom?.BomName ?? "",
                    CirculationBomRemark = material?.MaterialCode + "/" + material?.Version,
                    CirculationName = material?.MaterialName ?? "",
                    Usages = detailEntity.Usages,
                    AssembleCount = 0,
                    Code = procedures?.Code ?? "",
                    Name = procedures?.Name ?? "",
                    Children = new List<OriginalSummaryChildDto>()
                };
                bomDetailViews.Add(bomDetail);

                var assembleCount = 0M;
                foreach (var circulation in listCirculations)
                {
                    var barcodeMaterial = procMaterials.FirstOrDefault(item => item.Id == circulation.CirculationProductId);
                    var manuSfcChild = new OriginalSummaryChildDto
                    {
                        Id = circulation.Id,
                        BomDetailId = detailEntity.Id,
                        CirculationRemark = barcodeMaterial?.MaterialCode + "/" + barcodeMaterial?.Version ?? "",
                        CirculationName = barcodeMaterial?.MaterialName ?? "",
                        ResCode = circulation.ResourceId.HasValue ? procResources.FirstOrDefault(x => x.Id == circulation.ResourceId.Value)?.ResCode ?? "" : "",
                        CirculationBarCode = circulation.CirculationBarCode,
                        CirculationQty = circulation.CirculationQty ?? 0,
                        Status = circulation.IsDisassemble == TrueOrFalseEnum.Yes ? SFCCirculationReportTypeEnum.Remove : SFCCirculationReportTypeEnum.Activity,
                        UpdatedBy = circulation.UpdatedBy ?? "",
                        UpdatedOn = circulation.UpdatedOn
                    };
                    bomDetail.Children.Add(manuSfcChild);
                    assembleCount += manuSfcChild.Status == SFCCirculationReportTypeEnum.Activity ? circulation.CirculationQty ?? 0 : 0;
                }
                bomDetail.AssembleCount = assembleCount;
            }

            //查询子组件
            return bomDetailViews;
        }

        /// <summary>
        /// 获取资源信息
        /// </summary>
        /// <param name="manuSfcCirculations"></param>
        /// <returns></returns>
        private async Task<List<ProcResourceEntity>> GetResourcesAsync(IEnumerable<ManuSfcCirculationEntity> manuSfcCirculations)
        {
            var resourceIds = new List<long>();
            foreach (var item in manuSfcCirculations.Select(x=>x.ResourceId))
            {
                if (item.HasValue && item.Value > 0 && !resourceIds.Contains(item.Value))
                {
                    resourceIds.Add(item.Value);
                }
            }
            var procResources = new List<ProcResourceEntity>();
            if (resourceIds.Any())
            {
                procResources = (await _resourceRepository.GetListByIdsAsync(resourceIds.ToArray())).ToList();
            }
            return procResources;
        }
    }
}
