using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Integrated.InteContainer;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Report;
using Minio.DataModel;
using System.Collections.Generic;
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
         IManuContainerBarcodeRepository manuContainerBarcodeRepository,
         IProcMaterialRepository procMaterialRepository,
         IManuContainerPackRepository manuContainerPackRepository,
         IPlanWorkOrderRepository planWorkOrderRepository)
        {
            _currentSite = currentSite;
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
                var query = new ManuContainerBarcodeQuery { BarCode = queryDto.Code, SiteId = _currentSite.SiteId ?? 123456 };
                barcodeEntity = await _manuContainerBarcodeRepository.GetByCodeAsync(query);
                if (barcodeEntity == null)
                {
                    return null;
                }
            }
            else
            {
                //根据装载的条码获取到容器的id
                var query = new ManuContainerPackQuery
                {
                    LadeBarCode = queryDto.Code,
                    SiteId = _currentSite.SiteId ?? 123456,
                };
                var containerPackEntity = await _manuContainerPackRepository.GetByLadeBarCodeAsync(query);
                if (containerPackEntity == null)
                {
                    return null;
                }

                barcodeEntity = await _manuContainerBarcodeRepository.GetByIdAsync(containerPackEntity.ContainerBarCodeId);
                if (barcodeEntity == null)
                {
                    return null;
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
                SiteId = _currentSite.SiteId ?? 123456
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
        /// <param name="manuContainerPackPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuContainerPackDto>> GetContainerPackPagedListAsync(ManuContainerPackPagedQueryDto manuContainerPackPagedQueryDto)
        {
            var manuContainerPackPagedQuery = manuContainerPackPagedQueryDto.ToQuery<ManuContainerPackPagedQuery>();
            manuContainerPackPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _manuContainerPackRepository.GetPagedInfoAsync(manuContainerPackPagedQuery);

            //实体到DTO转换 装载数据
            var manuContainerPackDtos = new List<ManuContainerPackDto>();
            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<ManuContainerPackDto>(manuContainerPackDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }

            //查询容器的父容器信息
            var ladeBarCodes = pagedInfo.Data.Select(X => X.BarCode).Distinct().ToArray();
            var query = new ManuContainerPackQuery
            {
                SiteId = _currentSite.SiteId ?? 123456,
                LadeBarCodes = ladeBarCodes
            };
            var manuContainerPacks = await _manuContainerPackRepository.GetByLadeBarCodesAsync(query);
            IEnumerable<ManuContainerBarcodeEntity> parentContainers = new List<ManuContainerBarcodeEntity>();
            if (manuContainerPacks.Any())
            {
                //父容器id列表
                var parentContainerIds = manuContainerPacks.Select(x => x.ContainerBarCodeId).ToArray();
                parentContainers = await _manuContainerBarcodeRepository.GetByIdsAsync(parentContainerIds);
            }

            foreach (var item in pagedInfo.Data)
            {
                var pack = manuContainerPacks.FirstOrDefault(x => x.LadeBarCode == item.BarCode);
                manuContainerPackDtos.Add(new ManuContainerPackDto
                {
                    BarCode = item.BarCode,
                    LadeBarCode = item.LadeBarCode,
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                    ParentContainerCode = parentContainers.FirstOrDefault(x => x.Id == pack?.ContainerBarCodeId)?.BarCode ?? ""
                });
            }
            return new PagedInfo<ManuContainerPackDto>(manuContainerPackDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
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
            manuContainerBarcodePagedQuery.PackLevel = (int)LevelEnum.One;
            var pagedInfo = await _manuContainerBarcodeRepository.GetPagedListAsync(manuContainerBarcodePagedQuery);

            var workPackingDtos = new List<PlanWorkPackingDto>();
            var list = pagedInfo.Data;
            if (list == null || !list.Any())
            {
                return new PagedInfo<PlanWorkPackingDto>(workPackingDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }

            //查询容器的父容器信息
            var ladeBarCodes = pagedInfo.Data.Select(X => X.BarCode).ToArray();
            var query = new ManuContainerPackQuery
            {
                SiteId = _currentSite.SiteId ?? 123456,
                LadeBarCodes = ladeBarCodes
            };
            var manuContainerPacks = await _manuContainerPackRepository.GetByLadeBarCodesAsync(query);
            IEnumerable<ManuContainerBarcodeEntity> parentContainers = new List<ManuContainerBarcodeEntity>();
            if (manuContainerPacks.Any())
            {
                //父容器id列表
                var parentContainerIds = manuContainerPacks.Select(x => x.ContainerBarCodeId).ToArray();
                parentContainers = await _manuContainerBarcodeRepository.GetByIdsAsync(parentContainerIds);
            }

            //查询当前容器的子容器信息
            var containerIds = list.Select(x => x.Id).ToArray();
            var containerPackEntities = await _manuContainerPackRepository.GetByContainerBarCodeIdsAsync(containerIds, _currentSite.SiteId ?? 123456);

            foreach (var item in pagedInfo.Data)
            {
                var pack = manuContainerPacks.FirstOrDefault(x => x.LadeBarCode == item.BarCode);
                workPackingDtos.Add(new PlanWorkPackingDto
                {
                    BarCode = item.BarCode,
                    ContainerBarCodeId = item.ContainerId,
                    Status = item.Status,
                    PackLevel = item.PackLevel,
                    CreatedBy = item.CreatedBy,
                    CreatedOn = item.CreatedOn,
                    ParentContainerCode = parentContainers.FirstOrDefault(x => x.Id == pack?.ContainerBarCodeId)?.BarCode ?? "",
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
            manuContainerBarcodePagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            manuContainerBarcodePagedQuery.PackLevel = (int)LevelEnum.One;
            var pagedInfo = await _manuContainerBarcodeRepository.GetPagedListAsync(manuContainerBarcodePagedQuery);

            //实体到DTO转换 装载数据
            List<PlanWorkPackingDto> workPackingDtos = new List<PlanWorkPackingDto>();
            foreach (var item in pagedInfo.Data)
            {
                workPackingDtos.Add(new PlanWorkPackingDto
                {
                    BarCode = item.BarCode,
                    ContainerBarCodeId = item.Id,
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

            barcodeViewDto.Level = barcodeEntity.PackLevel;
            barcodeViewDto.PackQuantity = await _manuContainerPackRepository.GetCountByrBarCodeIdAsync(barcodeEntity.Id);

            return barcodeViewDto;
        }
    }
}
