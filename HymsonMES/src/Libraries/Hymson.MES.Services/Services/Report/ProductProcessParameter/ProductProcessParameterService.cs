using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Report;
using System.Collections.Generic;

namespace Hymson.MES.Services.Services.Report.ProductProcessParameter
{
    /// <summary>
    /// service（产品过程参数）
    /// @author zhaoqing
    /// @date 2023-10-13 10:14:17
    /// </summary>
    public class ProductProcessParameterService : IProductProcessParameterService
    {
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 产品过程参数
        /// </summary>
        private readonly IManuProductParameterRepository _productParameterRepository;

        /// <summary>
        /// 物料维护仓储接口
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 工序仓储接口
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 工作中心仓储接口
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 工序仓储接口
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcProductParameterGroupRepository _parameterGroupRepository;

        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IProcEquipmentGroupParamDetailRepository _groupParamDetailRepository;

        public ProductProcessParameterService(ICurrentSite currentSite,
        IManuProductParameterRepository productParameterRepository,
        IProcMaterialRepository procMaterialRepository,
        IProcProcedureRepository procProcedureRepository,
        IInteWorkCenterRepository inteWorkCenterRepository,
        IProcParameterRepository procParameterRepository,
        IManuSfcRepository manuSfcRepository,
        IProcResourceRepository procResourceRepository,
        IProcProductParameterGroupRepository parameterGroupRepository,
        IProcEquipmentGroupParamDetailRepository groupParamDetailRepository)
        {
            _currentSite = currentSite;
            _productParameterRepository = productParameterRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procParameterRepository = procParameterRepository;
            _manuSfcRepository = manuSfcRepository;
            _procResourceRepository = procResourceRepository;
            _parameterGroupRepository = parameterGroupRepository;
            _groupParamDetailRepository= groupParamDetailRepository;
        }

        /// <summary>
        /// 产品过程参数报表分页查询
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProductProcessParameterReportDto>> GetPageListAsync(ProductProcessParameterReportPagedQueryDto pagedQueryDto)
        {
            //工序和产品序列码必须至少选择一个
            if (string.IsNullOrWhiteSpace(pagedQueryDto.Sfc) && !pagedQueryDto.ProcedureId.HasValue)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45221));
            }

            var siteId = _currentSite.SiteId ?? 0;
            PagedInfo<ManuProductParameterEntity> pagedInfo = null;
            IEnumerable<ManuSfcProduceOrderView> produceOrderViews = new List<ManuSfcProduceOrderView>();

            //产品参数查询条件
            var pagedQuery = new ManuProductParameterPagedQuery
            {
                SiteId = siteId,
                Sfc = pagedQueryDto.Sfc,
                ProcedureId = pagedQueryDto.ProcedureId ?? 0,
                ParameterId = pagedQueryDto.ParameterId,
                CollectionTimeRange = pagedQueryDto.CollectionTimeRange,
                PageIndex = pagedQueryDto.PageIndex,
                PageSize = pagedQueryDto.PageSize
            };

            //有查询条件,过滤出条码
            if (pagedQueryDto.WorkCenterId.HasValue || pagedQueryDto.ProductId.HasValue
                || !string.IsNullOrWhiteSpace(pagedQueryDto.OrderCode) || (!string.IsNullOrWhiteSpace(pagedQueryDto.Sfc)))
            {
                var produceQuery = new ManuSfcProduceQuery
                {
                    SiteId = siteId,
                    WorkCenterId = pagedQueryDto.WorkCenterId,
                    OrderCode = pagedQueryDto.OrderCode,
                    ProductId = pagedQueryDto.ProductId
                };
                if (!string.IsNullOrWhiteSpace(pagedQueryDto.Sfc))
                {
                    produceQuery.Sfcs = new[] { pagedQueryDto.Sfc };
                }

                produceOrderViews = await _manuSfcRepository.GetSfcsEntitiesAsync(produceQuery);
                var sfcs = produceOrderViews.Select(x => x.Sfc).Distinct().ToList();
                if (sfcs.Any())
                {
                    pagedQuery.Sfcs = sfcs;
                    pagedInfo = await _productParameterRepository.GetParametesEntitiesAsync(pagedQuery);
                }
            }
            else
            {
                //如果没有查询条件，条码会分页查出来的条码
                pagedInfo = await _productParameterRepository.GetParametesEntitiesAsync(pagedQuery);
                var sfcs = pagedInfo.Data.Select(x => x.SFC).Distinct().ToList();
                if (sfcs.Any())
                {
                    var produceQuery = new ManuSfcProduceQuery
                    {
                        SiteId = siteId,
                        Sfcs = sfcs
                    };
                    produceOrderViews = await _manuSfcRepository.GetSfcsEntitiesAsync(produceQuery);
                }
            }

            //分页查询
            var reportDtos = new List<ProductProcessParameterReportDto>();
            if (pagedInfo == null)
            {
                return new PagedInfo<ProductProcessParameterReportDto>(reportDtos, pagedQueryDto.PageIndex, pagedQueryDto.PageSize, 0);
            }

            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<ProductProcessParameterReportDto>(reportDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }

            //查询关联信息
            var materialIds = produceOrderViews.Select(x => x.ProductId).Distinct().ToList();
            var procMaterialsTask = _procMaterialRepository.GetByIdsAsync(materialIds);

            var workCenterIds = produceOrderViews.Select(x => x.WorkCenterId.GetValueOrDefault()).Distinct().ToArray();
            var inteWorkCentersTask = _inteWorkCenterRepository.GetByIdsAsync(workCenterIds);

            var procedureIds = pagedInfo.Data.Select(x => x.ProcedureId).Distinct().ToList();
            var procProceduresTask = _procProcedureRepository.GetByIdsAsync(procedureIds);

            var parameterIds = pagedInfo.Data.Select(x => x.ParameterId).Distinct().ToList();
            var procParametersTask = _procParameterRepository.GetByIdsAsync(parameterIds);

            var resourceId = pagedInfo.Data.Select(x => x.ResourceId).Distinct().ToList();
            var  procResourceTask= _procResourceRepository.GetResByIdsAsync(resourceId);

            var groupIds = pagedInfo.Data.Select(x => x.ParameterGroupId).Distinct().ToArray();
            var parameterGroupTask = _parameterGroupRepository.GetByIdsAsync(groupIds);

           // await _groupParamDetailRepository.GetEntitiesByRecipeIdsAsync(groupIds);
            var procMaterials = await procMaterialsTask;
            var inteWorkCenters = await inteWorkCentersTask;
            var procProcedures = await procProceduresTask;
            var procParameters = await procParametersTask;
            var procResources = await procResourceTask;
            var parameterGroups = await parameterGroupTask;

            //组合数据返回
            foreach (var item in pagedInfo.Data)
            {
                var produceOrderView = produceOrderViews.FirstOrDefault(x => x.Sfc == item.SFC);
                var procMaterial = procMaterials.FirstOrDefault(x => x.Id == produceOrderView?.ProductId);
                var procedure = procProcedures.FirstOrDefault(x => x.Id == item.ProcedureId);
                var workCenter = inteWorkCenters.FirstOrDefault(x => x.Id == produceOrderView?.WorkCenterId.GetValueOrDefault());
                var parameter = procParameters.FirstOrDefault(x => x.Id == item.ParameterId);
                var resource = procResources.FirstOrDefault(x => x.Id == item.ResourceId);
                var group = parameterGroups.FirstOrDefault(x => x.Id == item.ParameterGroupId);

                reportDtos.Add(new ProductProcessParameterReportDto
                {
                    Sfc = item.SFC,
                    ParameterGroupCode= group?.Code??"",
                    ParameterGroupVersion=group?.Version??"",
                    ResCode = resource?.ResCode??"",
                    ProductCode = procMaterial?.MaterialCode ?? "",
                    ProductName = procMaterial?.MaterialName ?? "",
                    OrderCode = produceOrderView?.OrderCode ?? "",
                    WorkCenterCode = workCenter?.Code ?? "",
                    ProcedureCode = procedure?.Code ?? "",
                    ProcedureName = procedure?.Name ?? "",
                    ParameterCode = parameter?.ParameterCode ?? "",
                    ParameterName = parameter?.ParameterName ?? "",
                    ParameterUnit = parameter?.ParameterUnit ?? "",
                    ParameterValue = item.ParameterValue,
                    CollectionTime = item.CollectionTime
                });
            }
            return new PagedInfo<ProductProcessParameterReportDto>(reportDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }
    }
}
