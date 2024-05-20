using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Report;
using Hymson.Utils;

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

        private readonly IManuBarCodeRelationRepository _manuBarCodeRelationRepository;

        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

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
        /// <param name="manuBarCodeRelationRepository"></param>
        /// <param name="manuSfcRepository"></param>
        public OriginalSummaryReportService(ICurrentSite currentSite,
            IMasterDataService masterDataService,
            IProcBomRepository procBomRepository,
            IProcBomDetailRepository procBomDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceRepository resourceRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuBarCodeRelationRepository manuBarCodeRelationRepository,
            IManuSfcRepository manuSfcRepository)
        {
            _currentSite = currentSite;
            _masterDataService = masterDataService;
            _procBomRepository = procBomRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _resourceRepository = resourceRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuBarCodeRelationRepository = manuBarCodeRelationRepository;
            _manuSfcRepository = manuSfcRepository;
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

            //查询条码表信息
            var sfcList = await _manuSfcRepository.GetAllManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = new List<string> { queryDto.Sfc } });
            if (!sfcList.Any()) return bomDetailViews;

            // 查询组件信息
            var manuSfcCirculations = await _manuBarCodeRelationRepository.GetSfcMoudulesAsync(new ManuComponentBarcodeRelationQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfc = queryDto.Sfc,
                IsDisassemble = queryDto.Type
            });
            if (!manuSfcCirculations.Any()) return bomDetailViews;

            //// 拿到所有工单
            //var orderIds = manuSfcCirculations.Select(x => x.WorkOrderId);
            //var orders = await _planWorkOrderRepository.GetByIdsAsync(orderIds);
            //if (!orders.Any()) return bomDetailViews;

            // 查询bom
            var bomId = sfcList.Select(x => x.ProductBOMId);
            var bominfos = await _procBomRepository.GetByIdsAsync(bomId.OfType<long>());
            if (!bominfos.Any()) return bomDetailViews;

            // 物料Bom
            var sfcMaterialIds = sfcList.Select(x => x.ProductBOMId).Distinct();

            // 查询bom明细
            var bomDetails = await _procBomDetailRepository.GetByBomIdsAsync(sfcMaterialIds.OfType<long>());
            if (!bomDetails.Any()) return bomDetailViews;

            bomDetails = bomDetails.OrderBy(x => x.BomId).ToList();

            //组件物料
            var barCodeMaterialIds = manuSfcCirculations.Select(x => x.InputBarCodeMaterialId).Distinct();

            //bom物料
            var bomMaterialIds = bomDetails.Select(item => item.MaterialId).Distinct();

            //工单物料
           // var orderProducts = orders.Select(x => x.ProductId).Distinct();

            var materialIds = new List<long>();
            if (barCodeMaterialIds.Any())
            {
                materialIds.AddRange(barCodeMaterialIds);
            }
            if (bomMaterialIds.Any())
            {
                materialIds.AddRange(bomMaterialIds);
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
           // var procResources = await GetResourcesAsync(manuSfcCirculations);

            foreach (var detailEntity in bomDetails)
            {
                var material = procMaterials.FirstOrDefault(item => item.Id == detailEntity.MaterialId);
                var procedures = procProcedures.FirstOrDefault(item => item.Id == detailEntity.ProcedureId);
                var bom = bominfos.FirstOrDefault(item => item.Id == detailEntity.BomId);
                var bomDetail = new OriginalSummaryReportDto
                {
                    BomId = detailEntity.BomId,
                    BomDetailId = detailEntity.Id,
                    ProductRemark = material?.MaterialCode + "/" + material?.Version,
                    ProductName = material?.MaterialName ?? "",
                    BomRemark = bom?.BomCode + "/" + bom?.Version,
                    BomName = bom?.BomName ?? "",
                    CirculationBomRemark = material?.MaterialCode + "/" + material?.Version,
                    CirculationName = material?.MaterialName ?? "",
                    Usages = detailEntity.Usages,
                    AssembleCount = 0,
                    Code = procedures?.Code ?? "",
                    Name = procedures?.Name ?? "",
                    Children = new List<OriginalSummaryChildDto>()
                };
                bomDetailViews.Add(bomDetail);

                if (!manuSfcCirculations.Any())
                {
                    continue;
                }

                var assembleCount = 0M;

                var listCirculations = manuSfcCirculations.Where(a => a.OutputBarCode == queryDto.Sfc).ToList();

                foreach (var circulation in listCirculations)
                {
                    var inProductDismantleBusiness = circulation.BusinessContent.ToDeserialize<InProductDismantleBusinessContent>();
                    if (inProductDismantleBusiness == null)
                    {
                        continue;
                    }
                    if (inProductDismantleBusiness.BomMainMaterialId != detailEntity.Id)
                    {
                        continue;
                    }
                    var barcodeMaterial = procMaterials.FirstOrDefault(item => item.Id == circulation.InputBarCodeMaterialId);

                    var manuSfcChild = new OriginalSummaryChildDto
                    {
                        Id = circulation.Id,
                        BomDetailId = detailEntity.Id,
                        CirculationRemark = barcodeMaterial?.MaterialCode + "/" + barcodeMaterial?.Version ?? "",
                        CirculationName = barcodeMaterial?.MaterialName ?? "",
                        // ResCode = circulation.ResourceId.HasValue ? procResources.FirstOrDefault(x => x.Id == circulation.ResourceId.Value)?.ResCode ?? "" : "",
                        CirculationBarCode = circulation.InputBarCode,
                        CirculationQty = circulation.InputQty,
                        Status = circulation.IsDisassemble == TrueOrFalseEnum.Yes ? SFCCirculationReportTypeEnum.Remove : SFCCirculationReportTypeEnum.Activity,
                        UpdatedBy = circulation.UpdatedBy ?? "",
                        UpdatedOn = circulation.UpdatedOn
                    };
                    bomDetail.Children.Add(manuSfcChild);
                    assembleCount += manuSfcChild.Status == SFCCirculationReportTypeEnum.Activity ? circulation.InputQty : 0;
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
