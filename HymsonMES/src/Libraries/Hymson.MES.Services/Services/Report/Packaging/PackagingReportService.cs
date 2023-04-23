using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Report;
using Hymson.MES.Data.Repositories.Integrated.InteContainer;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Report;
using System.ComponentModel;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 包装报告 服务
    /// </summary>
    public class PackagingReportService : IPackagingReportService
    {
        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 容器维护 仓储
        /// </summary>
        private readonly IInteContainerRepository _inteContainerRepository;
        /// <summary>
        /// 容器条码表仓储接口
        /// </summary>
        private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;
        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        /// <summary>
        /// 容器装载表（物理删除） 仓储
        /// </summary>
        private readonly IManuContainerPackRepository _manuContainerPackRepository;

        /// <summary>
        /// 工单信息表 仓储
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        public PackagingReportService(ICurrentSite currentSite,
         IInteContainerRepository inteContainerRepository,
         IManuContainerBarcodeRepository manuContainerBarcodeRepository,
         IProcMaterialRepository procMaterialRepository,
         IManuContainerPackRepository manuContainerPackRepository,
         IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _currentSite = currentSite;
            _inteContainerRepository = inteContainerRepository;
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
            _procMaterialRepository = procMaterialRepository;
            _manuContainerPackRepository = manuContainerPackRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
        }

        /// <summary>
        /// 根据容器编码或者装载条码查询容器当前信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<ManuContainerBarcodeViewDto> QueryManuContainerByCodeAsync(PackagingQueryDto queryDto)
        {
            var barcodeViewDto = new ManuContainerBarcodeViewDto();

            var barcodeEntity = new ManuContainerBarcodeEntity();
            if (queryDto.Type == PackagingTypeEnum.Container)
            {
                var query = new ManuContainerBarcodeQuery { BarCode = queryDto.Code, SiteId = _currentSite.SiteId ?? 0 };
                barcodeEntity = await _manuContainerBarcodeRepository.GetByCodeAsync(query);
                if (barcodeEntity == null)
                {
                    return barcodeViewDto;
                }
            }
            else
            {
                //根据装载的条码获取到容器的id
                var query = new ManuContainerPackQuery
                {
                    LadeBarCode= queryDto.Code,
                    SiteId=_currentSite.SiteId ?? 0,
                };
                var containerPackEntity = await _manuContainerPackRepository.GetByLadeBarCodeAsync(query);
                if (containerPackEntity == null)
                {
                    return barcodeViewDto;
                }

                barcodeEntity = await _manuContainerBarcodeRepository.GetByIdAsync(containerPackEntity.Id);
                if (barcodeEntity == null)
                {
                    return barcodeViewDto;
                }
            }

            return await GetBarCodeViewAsync(barcodeEntity);
        }

        /// <summary>
        /// 查询工单包装信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PlanWorkPackDto> GetByWorkOrderCodeAsync(PackagingQueryDto queryDto)
        {
            var query = new PlanWorkOrderQuery
            {
                OrderCode = queryDto.Code,
                SiteId = _currentSite.SiteId ?? 0
            };

            var planWorkOrder = await _planWorkOrderRepository.GetByCodeAsync(query);
            if (planWorkOrder == null)
            {
                return new PlanWorkPackDto();
            }

            var procMaterial = await _procMaterialRepository.GetByIdAsync(planWorkOrder.ProductId);
            return new PlanWorkPackDto
            {
                WorkOrderId = planWorkOrder.Id,
                OrderCode = planWorkOrder.OrderCode,
                ProductCode = procMaterial.MaterialCode,
                ProductName = procMaterial.MaterialName,
                MaterialVersion = procMaterial.Version ?? "",
                Level = LevelEnum.One
            };
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkPackingDto>> GetPagedListAsync(ManuContainerBarcodePagedQueryDto queryDto)
        {
            var manuContainerBarcodePagedQuery = queryDto.ToQuery<ManuContainerBarcodePagedQuery>();
            manuContainerBarcodePagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuContainerBarcodeRepository.GetPagedListAsync(manuContainerBarcodePagedQuery);

            var list = pagedInfo.Data;
            var containerIds = list.Select(x => x.ContainerId).ToArray();
            var containerPackEntities = await _manuContainerPackRepository.GetByContainerBarCodeIdsAsync(containerIds);

            var workPackingDtos = new List<PlanWorkPackingDto>();
            foreach (var item in pagedInfo.Data)
            {
                workPackingDtos.Add(new PlanWorkPackingDto
                {
                    BarCode = item.BarCode,
                    ContainerId = item.ContainerId,
                    Status = item.Status,
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                    PackQuantity = containerPackEntities.Where(x => x.ContainerBarCodeId == item.Id).Count()
                });
            }
            return new PagedInfo<PlanWorkPackingDto>(workPackingDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<PlanWorkPackingDto>> GetPagedRecordListAsync(ManuContainerBarcodePagedQueryDto queryDto)
        {
            var manuContainerBarcodePagedQuery = queryDto.ToQuery<ManuContainerBarcodePagedQuery>();
            var pagedInfo = await _manuContainerBarcodeRepository.GetPagedListAsync(manuContainerBarcodePagedQuery);

            //实体到DTO转换 装载数据
            List<PlanWorkPackingDto> workPackingDtos = new List<PlanWorkPackingDto>();
            foreach (var item in pagedInfo.Data)
            {
                workPackingDtos.Add(new PlanWorkPackingDto
                {
                    BarCode = item.BarCode,
                    ContainerId = item.ContainerId,
                    Status = item.Status,
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn
                });
            }
            return new PagedInfo<PlanWorkPackingDto>(workPackingDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        private async Task<ManuContainerBarcodeViewDto> GetBarCodeViewAsync(ManuContainerBarcodeEntity barcodeEntity)
        {
            var barcodeViewDto = new ManuContainerBarcodeViewDto();

            //获取产品信息
            var materials = await _procMaterialRepository.GetByIdAsync(barcodeEntity.ProductId);
            barcodeViewDto.Id = barcodeEntity.Id;
            barcodeViewDto.ProductCode = materials?.MaterialCode ?? "";
            barcodeViewDto.ProductName = materials?.MaterialName ?? "";
            barcodeViewDto.Status = barcodeEntity.Status;

            var inteContainer = await _inteContainerRepository.GetByIdAsync(barcodeEntity.ContainerId);
            barcodeViewDto.Level = inteContainer?.Level;
            barcodeViewDto.PackQuantity = await _manuContainerPackRepository.GetCountByrBarCodeIdAsync(barcodeEntity.Id);

            return barcodeViewDto;
        }
    }
}
