using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductBadRecord.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Org.BouncyCastle.Crypto;
using System.Security.Policy;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 在制品拆解服务类
    /// </summary>
    public class InProductDismantleService : IInProductDismantleService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

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
        /// 构造函数
        /// </summary>
        public InProductDismantleService(ICurrentUser currentUser, ICurrentSite currentSite,
        IProcBomDetailRepository procBomDetailRepository,
        IProcMaterialRepository procMaterialRepository,
        IProcProcedureRepository procProcedureRepository,
        IProcResourceRepository resourceRepository,
        IManuSfcCirculationRepository circulationRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _procBomDetailRepository = procBomDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _circulationRepository = circulationRepository;
            _resourceRepository = resourceRepository;
        }

        /// <summary>
        /// 根据ID查询Bom 详情
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<List<InProductDismantleDto>> GetProcBomDetailAsync(InProductDismantleQueryDto queryDto)
        {
            if (queryDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            var bomDetailViews = await GetBomDetailViewsAsync(queryDto);

            var manuSfcCirculations = await GetCirculationsBySfcAsync(queryDto);
            if (!manuSfcCirculations.Any())
            {
                return bomDetailViews;
            }

            //查询资源信息
            var procResources = await GetResourcesAsync(manuSfcCirculations);

            foreach (var item in bomDetailViews)
            {
                var listCirculations = manuSfcCirculations.Where(a => a.ProcedureId == item.ProcedureId && a.ProductId == item.MaterialId).ToList();
                foreach (var circulation in listCirculations)
                {
                    item.Children.Add(new ManuSfcCirculationDto
                    {
                        ProcedureId = item.ProcedureId,
                        ProductId = item.MaterialId,
                        CirculationBarCode = circulation.CirculationBarCode,
                        ResCode = circulation.ResourceId.HasValue==true? procResources.FirstOrDefault(x=>x.Id==circulation.ResourceId.Value)?.ResCode??"":"",
                        Status= circulation.CirculationType== SfcCirculationTypeEnum.Disassembly? InProductDismantleTypeEnum.Remove: InProductDismantleTypeEnum.Activity,
                        UpdatedBy= circulation.UpdatedBy??"",
                        UpdatedOn= circulation.UpdatedOn
                    });
                }
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

        /// <summary>
        /// 组装主物料信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        private async Task<List<InProductDismantleDto>> GetBomDetailViewsAsync(InProductDismantleQueryDto queryDto)
        {
            var bomDetailViews = new List<InProductDismantleDto>();
            var bomDetails = await _procBomDetailRepository.GetByBomIdAsync(queryDto.BomId);
            if (!bomDetails.Any())
            {
                return bomDetailViews;
            }

            var materialIds = bomDetails.Select(item => item.MaterialId).ToArray();
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(materialIds);

            var procedureIds = bomDetails.Select(item => item.ProcedureId).ToArray();
            var procProcedures = await _procProcedureRepository.GetByIdsAsync(procedureIds);

            foreach (var detailEntity in bomDetails)
            {
                var material = procMaterials.FirstOrDefault(item => item.Id == detailEntity.MaterialId);
                var procedures = procProcedures.FirstOrDefault(item => item.Id == detailEntity.ProcedureId);

                bomDetailViews.Add(new InProductDismantleDto
                {
                    BomDetailId = detailEntity.Id,
                    Usages = detailEntity.Usages,
                    MaterialId = detailEntity.MaterialId,
                    ProcedureId = detailEntity.ProcedureId,
                    MaterialCode = material?.MaterialCode ?? "",
                    MaterialName = material?.MaterialName ?? "",
                    Version = material?.Version ?? "",
                    SerialNumber = material?.SerialNumber,
                    Code = procedures?.Code ?? "",
                    Name = procedures?.Name ?? "",
                    Children = new List<ManuSfcCirculationDto>()
                });
            }
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
            return await _circulationRepository.GetBySfcAsync(query);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="removeDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task RemoveModuleAsync(InProductDismantleRemoveDto removeDto)
        {
            if (removeDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
 
        }
    }
}
