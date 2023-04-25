using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;

namespace Hymson.MES.Services.Services.Report
{
    public class OriginalSummaryReportService: IOriginalSummaryReportService
    {
        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;
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
        /// 条码流转表仓储
        /// </summary>
        private readonly IManuSfcCirculationRepository _circulationRepository;
        /// <summary>
        /// 资源仓储
        /// </summary>
        private readonly IProcResourceRepository _resourceRepository;
        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OriginalSummaryReportService(ICurrentSite currentSite,
         IProcBomRepository procBomRepository,
        IProcBomDetailRepository procBomDetailRepository,
        IProcMaterialRepository procMaterialRepository,
        IProcProcedureRepository procProcedureRepository,
        IProcResourceRepository resourceRepository,
        IManuSfcCirculationRepository circulationRepository,
        IManuSfcProduceRepository manuSfcProduceRepository)
        {
            _currentSite = currentSite;
            _procBomRepository = procBomRepository;
            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _circulationRepository = circulationRepository;
            _resourceRepository = resourceRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
        }

        /// <summary>
        /// 根据ID查询Bom 主物料以及组件信息详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<List<InProductDismantleDto>> GetOriginalSummaryAsync(InProductDismantleQueryDto queryDto)
        {
            var bomDetailViews = new List<InProductDismantleDto>();

            if (queryDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            //查询bom
            var bom = await _procBomRepository.GetByIdAsync(queryDto.BomId);
            if (bom == null)
            {
                return bomDetailViews;
            }

            //查询bom明细
            var bomDetails = await _procBomDetailRepository.GetByBomIdAsync(queryDto.BomId);
            if (!bomDetails.Any())
            {
                return bomDetailViews;
            }

            //查询组件信息
            var manuSfcCirculations = await GetCirculationsBySfcAsync(queryDto);

            //组件物料
            var barCodeMaterialIds = manuSfcCirculations.Select(x => x.CirculationProductId).ToArray().Distinct();

            //bom物料
            var bomMaterialIds = bomDetails.Select(item => item.MaterialId).ToArray().Distinct();

            var materialIds = new List<long>();
            if (barCodeMaterialIds.Any())
            {
                materialIds.AddRange(barCodeMaterialIds);
            }
            if (bomMaterialIds.Any())
            {
                materialIds.AddRange(bomMaterialIds);
            }

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
                var material = procMaterials.FirstOrDefault(item => item.Id == detailEntity.MaterialId);
                var procedures = procProcedures.FirstOrDefault(item => item.Id == detailEntity.ProcedureId);

                var bomDetail = new InProductDismantleDto
                {
                    BomDetailId = detailEntity.Id,
                    Usages = detailEntity.Usages,
                    MaterialId = detailEntity.MaterialId,
                    ProcedureId = detailEntity.ProcedureId,
                    MaterialCode = material?.MaterialCode ?? "",
                    MaterialName = material?.MaterialName ?? "",
                    Version = material?.Version ?? "",
                    SerialNumber = detailEntity.DataCollectionWay.HasValue == true ? detailEntity.DataCollectionWay.Value : material?.SerialNumber,
                    Code = procedures?.Code ?? "",
                    Name = procedures?.Name ?? "",
                    BomRemark = bom.BomCode + "/" + bom.Version,
                    AssembleCount = 0,
                    Children = new List<ManuSfcChildCirculationDto>()
                };
                bomDetailViews.Add(bomDetail);

                if (!manuSfcCirculations.Any())
                {
                    continue;
                }

                var assembleCount = 0M;
                var listCirculations = manuSfcCirculations.Where(a => a.ProcedureId == bomDetail.ProcedureId && a.CirculationMainProductId == bomDetail.MaterialId).ToList();
                foreach (var circulation in listCirculations)
                {
                    var barcodeMaterial = procMaterials.FirstOrDefault(item => item.Id == circulation.CirculationProductId);

                    var manuSfcChild = new ManuSfcChildCirculationDto
                    {
                        Id = circulation.Id,
                        BomDetailId = detailEntity.Id,
                        ProcedureId = bomDetail.ProcedureId,
                        ProductId = bomDetail.MaterialId,
                        CirculationBarCode = circulation.CirculationBarCode,
                        CirculationQty = circulation.CirculationQty ?? 0,
                        MaterialRemark = barcodeMaterial?.MaterialName ?? "" + "/" + barcodeMaterial?.Version ?? "",
                        ResCode = circulation.ResourceId.HasValue == true ? procResources.FirstOrDefault(x => x.Id == circulation.ResourceId.Value)?.ResCode ?? "" : "",
                        Status = circulation.IsDisassemble == TrueOrFalseEnum.Yes ? InProductDismantleTypeEnum.Remove : InProductDismantleTypeEnum.Activity,
                        UpdatedBy = circulation.UpdatedBy ?? "",
                        UpdatedOn = circulation.UpdatedOn
                    };
                    bomDetail.Children.Add(manuSfcChild);
                    assembleCount += manuSfcChild.Status == InProductDismantleTypeEnum.Activity ? circulation.CirculationQty ?? 0 : 0;
                }
                bomDetail.AssembleCount = assembleCount;
            }

            //查询子组件
            return bomDetailViews;
        }

        /// <summary>
        /// 获取sfc组件信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ManuSfcCirculationEntity>> GetCirculationsBySfcAsync(InProductDismantleQueryDto queryDto)
        {
            var types = new List<SfcCirculationTypeEnum>();
            if (queryDto.Type == InProductDismantleTypeEnum.Remove
                || queryDto.Type == InProductDismantleTypeEnum.Whole)
            {
                types.Add(SfcCirculationTypeEnum.Disassembly);
            }

            if (queryDto.Type == InProductDismantleTypeEnum.Activity
              || queryDto.Type == InProductDismantleTypeEnum.Whole)
            {
                types.Add(SfcCirculationTypeEnum.Consume);
                types.Add(SfcCirculationTypeEnum.ModuleAdd);
                types.Add(SfcCirculationTypeEnum.ModuleReplace);
            }

            var query = new ManuSfcCirculationQuery { Sfc = queryDto.Sfc, SiteId = _currentSite.SiteId ?? 0, CirculationTypes = types.ToArray() };

            if (queryDto.Type == InProductDismantleTypeEnum.Remove)
            {
                query.IsDisassemble = TrueOrFalseEnum.Yes;
            }

            if (queryDto.Type == InProductDismantleTypeEnum.Activity)
            {
                query.IsDisassemble = TrueOrFalseEnum.No;
            }

            return await _circulationRepository.GetSfcMoudulesAsync(query);
        }

        /// <summary>
        /// 获取资源信息
        /// </summary>
        /// <param name="manuSfcCirculations"></param>
        /// <returns></returns>
        private async Task<List<ProcResourceEntity>> GetResourcesAsync(IEnumerable<ManuSfcCirculationEntity> manuSfcCirculations)
        {
            var resourceIds = new List<long>();
            foreach (var item in manuSfcCirculations)
            {
                if (item.ResourceId.HasValue && item.ResourceId.Value > 0)
                {
                    if (!resourceIds.Contains(item.ResourceId.Value))
                    {
                        resourceIds.Add(item.ResourceId.Value);
                    }
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
