using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Report;
using Hymson.MES.Services.Services.Equipment.EquEquipment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Hymson.MES.Services.Services.Report
{
    /// <summary>
    /// 服务（设备过程参数报表） 
    /// </summary>
    public class EquProcessParameterReportService : IEquProcessParameterReportService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 工作中心仓储
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 工序仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（设备过程参数报表）
        /// </summary>

        private readonly IManuEquipmentParameterRepository _equipmentParameterRepository;

        private readonly IEquEquipmentRepository _equipmentRepository;
        private readonly IProcParameterRepository _parameterRepository;
        private readonly IProcResourceRepository _resourceRepository;
        private readonly IProcResourceEquipmentBindRepository _equipmentBindRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquProcessParameterReportService(ICurrentUser currentUser, ICurrentSite currentSite,
               IInteWorkCenterRepository inteWorkCenterRepository,
               IProcProcedureRepository procProcedureRepository,
               IManuEquipmentParameterRepository equipmentParameterRepository,
               IEquEquipmentRepository equipmentRepository,
               IProcParameterRepository parameterRepository,
               IProcResourceRepository resourceRepository,
               IProcResourceEquipmentBindRepository equipmentBindRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procProcedureRepository = procProcedureRepository;
            _equipmentParameterRepository = equipmentParameterRepository;
            _equipmentRepository = equipmentRepository;
            _parameterRepository = parameterRepository;
            _resourceRepository = resourceRepository;
            _equipmentBindRepository = equipmentBindRepository;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquProcessParameterReportDto>> GetPagedListAsync(EquProcessParameterReportPagedQueryDto pagedQueryDto)
        {
            //选择时间与设备编号显示数据
            if (pagedQueryDto.CreatedOn == null || pagedQueryDto.CreatedOn.Length < 2)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES14953));
            }
            if (pagedQueryDto.EquipmentId <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12601));
            }

            var siteId = _currentSite.SiteId ?? 0;
            //获取设备的参数信息
            var parameterQuery = new ManuEquipmentParameterPagedQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                CreatedOn = pagedQueryDto.CreatedOn,
                ParameterId = pagedQueryDto.ParameterId,
                EquipmentId = pagedQueryDto.EquipmentId,
                PageIndex = pagedQueryDto.PageIndex,
                PageSize = pagedQueryDto.PageSize
            };
            var pagedInfo = await _equipmentParameterRepository.GetParametesByEqumentIdEntitiesAsync(parameterQuery);

            var listDto = new List<EquProcessParameterReportDto>();
            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<EquProcessParameterReportDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }

            var equipmentId = pagedQueryDto.EquipmentId;
            var parameterIds = pagedInfo.Data.Select(x => x.ParameterId).Distinct().ToList();

            //设备
            var equipmentEntity = await _equipmentRepository.GetByIdAsync(equipmentId);

            //参数
            var parameterEntities = await _parameterRepository.GetByIdsAsync(parameterIds);

            //资源
            var resourceEntity = new ProcResourceEntity();
            var workCenterEntity = new InteWorkCenterEntity();
            IEnumerable<ProcProcedureEntity> procedureEntities = new List<ProcProcedureEntity>();
            var equipmentBindEntities = await _equipmentBindRepository.GetByResourceIdAsync(new ProcResourceEquipmentBindQuery
            {
                Ids = new long[] { equipmentId }
            });
            var resourceIds = equipmentBindEntities.Select(x => x.ResourceId).Distinct().ToList();
            if (resourceIds.Any())
            {
                var resourceId = resourceIds[0];
                resourceEntity = await _resourceRepository.GetResByIdAsync(resourceId);
                if (resourceEntity != null)
                {
                    //工作中心
                    workCenterEntity = await _inteWorkCenterRepository.GetByResourceIdAsync(resourceId);

                    //工序
                    procedureEntities = await _procProcedureRepository.GetProcProcedureByResourceTypeIdAsync(resourceEntity.ResTypeId);
                }
            }

            foreach (var item in pagedInfo.Data)
            {
                var resTypeId = resourceEntity?.ResTypeId ?? 0;
                var procedureEntity = procedureEntities.FirstOrDefault(y => y.ResourceTypeId == resTypeId);
                var parameterEntity = parameterEntities.FirstOrDefault(x => x.Id == item.ParameterId);
                var equParameterDto = new EquProcessParameterReportDto
                {
                    WorkCenterName = workCenterEntity?.Name ?? "",
                    ProcedureCode = procedureEntity?.Code ?? "",
                    ProcedureName = procedureEntity?.Name ?? "",
                    ResCode = resourceEntity?.ResCode ?? "",
                    ResName = resourceEntity?.ResName ?? "",
                    EquipmentCode = equipmentEntity?.EquipmentCode ?? "",
                    EquipmentName = equipmentEntity?.EquipmentName ?? "",
                    ParameterCode = parameterEntity?.ParameterCode ?? "",
                    ParameterName = parameterEntity?.ParameterName ?? "",
                    ParameterUnit = parameterEntity?.ParameterUnit ?? "",
                    ParameterValue = item.ParameterValue,
                    CollectionTime = item.CollectionTime,
                    CreatedOn = item.CreatedOn
                };

                listDto.Add(equParameterDto);
            }
            return new PagedInfo<EquProcessParameterReportDto>(listDto, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
