using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Domain.Query;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.Resource;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Scriban.Runtime.Accessors;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 资源维护表Service业务层处理
    /// @tableName proc_resource
    /// @author zhaoqing
    /// @date 2023-02-08
    /// </summary>
    public class ProcResourceService : IProcResourceService
    {
        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 资源仓储
        /// </summary>
        private readonly IProcResourceRepository _resourceRepository;

        /// <summary>
        /// 资源类型仓储
        /// </summary>
        private readonly IProcResourceTypeRepository _resourceTypeRepository;

        /// <summary>
        /// 资源关联打印机仓储
        /// </summary>
        private readonly IProcResourceConfigPrintRepository _resourceConfigPrintRepository;

        /// <summary>
        /// 资源设置仓储
        /// </summary>
        private readonly IProcResourceConfigResRepository _procResourceConfigResRepository;

        /// <summary>
        /// 资源关联设备仓储
        /// </summary>
        private readonly IProcResourceEquipmentBindRepository _resourceEquipmentBindRepository;

        /// <summary>
        /// 工序配置作业表仓储
        /// </summary>
        private readonly IInteJobBusinessRelationRepository _jobBusinessRelationRepository;

        /// <summary>
        /// 作业表仓储
        /// </summary>
        private readonly IInteJobRepository _inteJobRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 产出设置
        /// </summary>
        private readonly IProcProductSetRepository _procProductSetRepository;

        /// <summary>
        /// 仓储接口（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 仓储接口（资质认证）
        /// </summary>
        private readonly IInteQualificationAuthenticationRepository _inteQualificationAuthenticationRepository;
        private readonly IProcResourceQualificationAuthenticationRelationRepository _authenticationRelationRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        private readonly AbstractValidator<ProcResourceCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcResourceModifyDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="resourceRepository"></param>
        /// <param name="resourceTypeRepository"></param>
        /// <param name="resourceConfigPrintRepository"></param>
        /// <param name="procResourceConfigResRepository"></param>
        /// <param name="resourceEquipmentBindRepository"></param>
        /// <param name="jobBusinessRelationRepository"></param>
        /// <param name="inteJobRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="procProductSetRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="authenticationRelationRepository"></param>
        /// <param name="inteQualificationAuthenticationRepository"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="localizationService"></param>
        public ProcResourceService(ICurrentUser currentUser, ICurrentSite currentSite,
                  IProcResourceRepository resourceRepository,
                  IProcResourceTypeRepository resourceTypeRepository,
                  IProcResourceConfigPrintRepository resourceConfigPrintRepository,
                  IProcResourceConfigResRepository procResourceConfigResRepository,
                  IProcResourceEquipmentBindRepository resourceEquipmentBindRepository,
                  IInteJobBusinessRelationRepository jobBusinessRelationRepository,
                  IInteJobRepository inteJobRepository,
                  IInteWorkCenterRepository inteWorkCenterRepository,
                  IPlanWorkOrderRepository planWorkOrderRepository,
                  IProcMaterialRepository procMaterialRepository,
                  IProcProductSetRepository procProductSetRepository,
                  IEquEquipmentRepository equEquipmentRepository,
                  IInteQualificationAuthenticationRepository inteQualificationAuthenticationRepository,
                  IProcResourceQualificationAuthenticationRelationRepository authenticationRelationRepository,
                  AbstractValidator<ProcResourceCreateDto> validationCreateRules,
                  AbstractValidator<ProcResourceModifyDto> validationModifyRules, ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _resourceRepository = resourceRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _resourceConfigPrintRepository = resourceConfigPrintRepository;
            _procResourceConfigResRepository = procResourceConfigResRepository;
            _resourceEquipmentBindRepository = resourceEquipmentBindRepository;
            _jobBusinessRelationRepository = jobBusinessRelationRepository;
            _inteJobRepository = inteJobRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProductSetRepository = procProductSetRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _inteQualificationAuthenticationRepository=inteQualificationAuthenticationRepository;
            _authenticationRelationRepository = authenticationRelationRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 查询资源类型维护表详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceViewDto> GetByIdAsync(long id)
        {
            var entity = await _resourceRepository.GetByIdAsync(id);
            return entity?.ToModel<ProcResourceViewDto>() ?? new ProcResourceViewDto();
        }

        /// <summary>
        /// 查询资源维护表列表(关联资源类型，展示资源类型名称)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceViewDto>> GetPageListAsync(ProcResourcePagedQueryDto query)
        {
            var resourcePagedQuery = query.ToQuery<ProcResourcePagedQuery>();
            resourcePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _resourceRepository.GetPageListAsync(resourcePagedQuery);

            //实体到DTO转换 装载数据
            var procResourceDtos = new List<ProcResourceViewDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceViewDto = entity.ToModel<ProcResourceViewDto>();
                procResourceDtos.Add(resourceViewDto);
            }
            return new PagedInfo<ProcResourceViewDto>(procResourceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取资源分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceDto>> GetListAsync(ProcResourcePagedQueryDto query)
        {
            var resourcePagedQuery = query.ToQuery<ProcResourcePagedQuery>();
            resourcePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _resourceRepository.GetListAsync(resourcePagedQuery);

            //实体到DTO转换 装载数据
            var procResourceDtos = new List<ProcResourceDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeDto = entity.ToModel<ProcResourceDto>();
                procResourceDtos.Add(resourceTypeDto);
            }
            return new PagedInfo<ProcResourceDto>(procResourceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 更具线体和工序查询资源
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceDto>> GetPageListBylineIdAndProcProcedureIdAsync(ProcResourcePagedlineIdAndProcProcedureIdDto query)
        {
            var resourcePagedQuery = query.ToQuery<ProcResourcePagedlineIdAndProcProcedureIdQuery>();
            if (!query.WorkCenterLineId.HasValue || !query.ResTypeId.HasValue)
            {
                return new PagedInfo<ProcResourceDto>(new List<ProcResourceDto>(), query.PageIndex, query.PageSize, 0);
            }
            var workCenters = await _inteWorkCenterRepository.GetInteWorkCenterRelationAsync(query.WorkCenterLineId ?? 0);
            if (workCenters != null && workCenters.Any())
            {
                resourcePagedQuery.WorkCenterLineIds = workCenters.Select(x => (long?)x.SubWorkCenterId);
            }
            else
            {
                resourcePagedQuery.WorkCenterLineIds = new List<long?> { query.WorkCenterLineId };
            }
            var pagedInfo = await _resourceRepository.GetPageListBylineIdAndProcProcedureIdAsync(resourcePagedQuery);

            //实体到DTO转换 装载数据
            var procResourceDtos = new List<ProcResourceDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeDto = entity.ToModel<ProcResourceDto>();
                procResourceDtos.Add(resourceTypeDto);
            }
            return new PagedInfo<ProcResourceDto>(procResourceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询资源类型下关联的资源(资源类型详情：可用资源，已分配资源)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<ProcResourceDto>> GetListForGroupAsync(ProcResourcePagedQueryDto query)
        {
            var resourcePagedQuery = query.ToQuery<ProcResourcePagedQuery>();
            resourcePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var list = await _resourceRepository.GetListForGroupAsync(resourcePagedQuery);

            //实体到DTO转换 装载数据
            var procResourceDtos = new List<ProcResourceDto>();
            foreach (var entity in list)
            {
                var resourceTypeDto = entity.ToModel<ProcResourceDto>();
                procResourceDtos.Add(resourceTypeDto);
            }
            return procResourceDtos;
        }

        /// <summary>
        /// 根据工序id查询资源列表(工序 > 资源类型 > 资源)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceViewDto>> GettPageListByProcedureIdAsync(ProcResourceProcedurePagedQueryDto query)
        {
            var resourcePagedQuery = query.ToQuery<ProcResourceProcedurePagedQuery>();
            resourcePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _resourceRepository.GettPageListByProcedureIdAsync(resourcePagedQuery);

            //实体到DTO转换 装载数据
            var procResourceDtos = new List<ProcResourceViewDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceDto = entity.ToModel<ProcResourceViewDto>();
                procResourceDtos.Add(resourceDto);
            }
            return new PagedInfo<ProcResourceViewDto>(procResourceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 资源关联打印机数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceConfigPrintViewDto>> GetcResourceConfigPrintAsync(ProcResourceConfigPrintPagedQueryDto query)
        {
            var printPagedQuery = query.ToQuery<ProcResourceConfigPrintPagedQuery>();
            var pagedInfo = await _resourceConfigPrintRepository.GetPagedInfoAsync(printPagedQuery);

            //实体到DTO转换 装载数据
            var procResourceConfigPrintViewDtos = new List<ProcResourceConfigPrintViewDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeDto = entity.ToModel<ProcResourceConfigPrintViewDto>();
                procResourceConfigPrintViewDtos.Add(resourceTypeDto);
            }
            return new PagedInfo<ProcResourceConfigPrintViewDto>(procResourceConfigPrintViewDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 资源设置数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceConfigResDto>> GetcResourceConfigResAsync(ProcResourceConfigResPagedQueryDto query)
        {
            var resPagedQuery = query.ToQuery<ProcResourceConfigResPagedQuery>();
            var pagedInfo = await _procResourceConfigResRepository.GetPagedInfoAsync(resPagedQuery);

            var pageData = pagedInfo.Data;
            var orderIds = pageData.Where(x => x.SetType == (int)ResourceSetTypeEnum.Workorder).Select(x => x.Value.ParseToLong()).Distinct().ToArray();
            var materialIds = pageData.Where(x => x.SetType == (int)ResourceSetTypeEnum.Material).Select(x => x.Value.ParseToLong()).Distinct().ToArray();
            var planWorkOrders = await _planWorkOrderRepository.GetByIdsAsync(orderIds);
            var procMaterials = await _procMaterialRepository.GetByIdsAsync(materialIds);

            //实体到DTO转换 装载数据
            var resourceConfigResDtos = new List<ProcResourceConfigResDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeDto = entity.ToModel<ProcResourceConfigResDto>();
                resourceTypeDto.MaterialCode = entity.SetType == (int)ResourceSetTypeEnum.Material ? procMaterials.FirstOrDefault(x => x.Id == entity.Value.ParseToLong())?.MaterialCode ?? "" : "";
                resourceTypeDto.OrderCode = entity.SetType == (int)ResourceSetTypeEnum.Workorder ? planWorkOrders.FirstOrDefault(x => x.Id == entity.Value.ParseToLong())?.OrderCode ?? "" : "";
                resourceConfigResDtos.Add(resourceTypeDto);
            }
            return new PagedInfo<ProcResourceConfigResDto>(resourceConfigResDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取资源关联设备数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceEquipmentBindViewDto>> GetcResourceConfigEquAsync(ProcResourceEquipmentBindPagedQueryDto query)
        {
            var bindPagedQuery = query.ToQuery<ProcResourceEquipmentBindPagedQuery>();
            var pagedInfo = await _resourceEquipmentBindRepository.GetPagedInfoAsync(bindPagedQuery);

            //实体到DTO转换 装载数据
            var procResourceEquipmentBinds = new List<ProcResourceEquipmentBindViewDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeDto = entity.ToModel<ProcResourceEquipmentBindViewDto>();
                procResourceEquipmentBinds.Add(resourceTypeDto);
            }
            return new PagedInfo<ProcResourceEquipmentBindViewDto>(procResourceEquipmentBinds, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取资源配置Job信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcedureJobReleationDto>> GetProcedureConfigJobListAsync(InteJobBusinessRelationPagedQueryDto queryDto)
        {
            var query = new InteJobBusinessRelationPagedQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                BusinessId = queryDto.BusinessId,
                BusinessType = (int)InteJobBusinessTypeEnum.Resource,
                PageIndex = queryDto.PageIndex,
                PageSize = queryDto.PageSize,
                Sorting = queryDto.Sorting
            };
            var pagedInfo = await _jobBusinessRelationRepository.GetPagedInfoAsync(query);


            //实体到DTO转换 装载数据
            var dtos = new List<ProcedureJobReleationDto>();
            if (pagedInfo.Data != null && pagedInfo.Data.Any())
            {
                var jobIds = pagedInfo.Data.Select(a => a.JobId).ToArray();
                var jobList = await _inteJobRepository.GetByIdsAsync(jobIds);

                foreach (var entity in pagedInfo.Data)
                {
                    var job = jobList.FirstOrDefault(a => a.Id == entity.JobId);
                    dtos.Add(new ProcedureJobReleationDto()
                    {
                        LinkPoint = entity.LinkPoint,
                        Parameter = entity.Parameter,
                        JobId = entity.JobId,
                        BusinessId = entity.BusinessId,
                        IsUse = entity.IsUse,
                        Code = job?.Code ?? "",
                        Name = job?.Name ?? "",
                        Remark = job?.Remark ?? ""
                    });
                }
            }

            return new PagedInfo<ProcedureJobReleationDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取资源产出设置
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProductSetDto>> GetResourceProductSetListAsync(ProcProductSetQueryDto queryDto)
        {
            var query = new ProcProductSetQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                SetPointId = queryDto.SetPointId
            };
            var procProductSetEntities = await _procProductSetRepository.GetProcProductSetEntitiesAsync(query);

            //实体到DTO转换 装载数据
            List<ProcProductSetDto> procProductSetDtos = new List<ProcProductSetDto>();
            if (procProductSetEntities != null && procProductSetEntities.Any())
            {
                var materialIds = new List<long> { };
                IEnumerable<ProcMaterialEntity> procMaterialList = new List<ProcMaterialEntity>();
                materialIds.AddRange(procProductSetEntities.Select(a => a.ProductId).ToArray());
                materialIds.AddRange(procProductSetEntities.Select(a => a.SemiProductId).ToArray());
                var materialIdList = materialIds.Distinct();
                if (materialIdList.Any())
                {
                    procMaterialList = await _procMaterialRepository.GetByIdsAsync(materialIdList);
                }

                foreach (var entity in procProductSetEntities)
                {
                    var product = procMaterialList.FirstOrDefault(x => x.Id == entity.ProductId);
                    var semiProduct = procMaterialList.FirstOrDefault(x => x.Id == entity.SemiProductId);
                    procProductSetDtos.Add(new ProcProductSetDto()
                    {
                        ProductId = entity.ProductId,
                        ProductCode = product?.MaterialCode ?? "",
                        MaterialName = product?.MaterialName ?? "",
                        Version = product?.Version ?? "",
                        SetPointId = entity.SetPointId,
                        SemiProductId = entity.SemiProductId,
                        SemiMaterialCode = semiProduct?.MaterialCode ?? "",
                        SemiMaterialName = semiProduct?.MaterialName ?? "",
                        SemiVersion = semiProduct?.Version ?? "",
                    });
                }
            }

            return procProductSetDtos;
        }

        /// <summary>
        /// 获取资质认证设置
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcQualificationAuthenticationDto>> GetResourceAuthSetListAsync(long resourceId)
        {
            var query = new ProcResourceQualificationAuthenticationRelationQuery()
            {
                ResourceId = resourceId
            };
            var relationEntities = await _authenticationRelationRepository.GetEntitiesAsync(query);

            //实体到DTO转换 装载数据
            List<ProcQualificationAuthenticationDto> authenticationDtos = new List<ProcQualificationAuthenticationDto>();
            if (relationEntities != null && relationEntities.Any())
            {
                IEnumerable<InteQualificationAuthenticationEntity> authenticationEntities = new List<InteQualificationAuthenticationEntity>();
                var authIds = relationEntities.Select(a => a.QualificationAuthenticationId).ToArray();
                if (authIds.Any())
                {
                    authenticationEntities = await _inteQualificationAuthenticationRepository.GetByIdsAsync(authIds);
                }

                foreach (var entity in relationEntities)
                {
                    var authenticationEntity = authenticationEntities.FirstOrDefault(x => x.Id == entity.QualificationAuthenticationId);
                    authenticationDtos.Add(new ProcQualificationAuthenticationDto()
                    {
                        AuthenticationId = entity.QualificationAuthenticationId,
                        IsEnable = entity.IsEnable,
                        Code = authenticationEntity?.Code ?? "",
                        Name = authenticationEntity?.Name ?? ""
                    });
                }
            }

            return authenticationDtos;
        }

        /// <summary>
        /// 添加资源维护表所有页签的数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> AddProcResourceAsync(ProcResourceCreateDto param)
        {
            #region 验证
            if (param == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            param.ResCode = param.ResCode.ToTrimSpace().ToUpperInvariant();
            param.ResName = param.ResName.Trim();
            param.Remark = param?.Remark ?? "".Trim();
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(param);

            var resCode = param.ResCode;
            var query = new ProcResourceQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ResCode = resCode
            };
            if (await _resourceRepository.IsExistsAsync(query))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10308)).WithData("ResCode", param.ResCode);
            }

            if (param.ResTypeId > 0)
            {
                var resourceType = await _resourceTypeRepository.GetByIdAsync(param.ResTypeId ?? 0);
                if (resourceType == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10310));
                }
            }

            //打印机验证
            //判断打印机是否重复配置  数据库中 已经存储的情况
            if (param.PrintList != null && param.PrintList.Count > 0 && param.PrintList.GroupBy(x => x.PrintId).Any(g => g.Count() >= 2))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10313));
            }

            //判断是否勾选了多个主设备，只能有一个主设备
            if (param.EquList != null && param.EquList.Count > 0)
            {
                var equNumber = param.EquList.ToLookup(w => w.EquipmentId).ToDictionary(d => d.Key, d => d);
                if (equNumber.Keys.Count < param.EquList.Count)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10314));
                }

                var ismianCount = param.EquList.Where(a => a.IsMain).ToList().Count;
                if (ismianCount > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10307));
                }
            }

            //判断资质是否重复配置  数据库中 已经存储的情况
            if (param.AuthSetList != null && param.AuthSetList.Count > 0 && param.AuthSetList.GroupBy(x => x.AuthenticationId).Any(g => g.Count() >= 2))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10389));
            }
            #endregion

            var siteId = _currentSite.SiteId ?? 0;
            var userName = _currentUser.UserName;

            #region 组装数据

            //DTO转换实体
            var entity = new ProcResourceEntity
            {
                Remark = param.Remark,
                ResName = param.ResName,
                ResTypeId = param.ResTypeId ?? 0,
                Status = (int)SysDataStatusEnum.Build,

                Id = IdGenProvider.Instance.CreateId(),
                SiteId = siteId,
                CreatedBy = userName,
                UpdatedBy = userName,
                ResCode = resCode
            };

            var validationFailures = new List<ValidationFailure>();

            //打印机数据
            List<ProcResourceConfigPrintEntity> printList = new List<ProcResourceConfigPrintEntity>();
            if (param.PrintList != null && param.PrintList.Count > 0)
            {
                int i = 0;
                foreach (var item in param.PrintList.Select(x => x.PrintId))
                {
                    i++;
                    if (item <= 0)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10381);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    ProcResourceConfigPrintEntity print = new ProcResourceConfigPrintEntity();
                    print = new ProcResourceConfigPrintEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ResourceId = entity.Id,
                        PrintId = item,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    printList.Add(print);
                }
            }

            //设备绑定设置数据
            List<ProcResourceEquipmentBindEntity> equList = new List<ProcResourceEquipmentBindEntity>();
            if (param.EquList != null && param.EquList.Count > 0)
            {
                int i = 0;
                foreach (var item in param.EquList)
                {
                    i++;
                    if (item.EquipmentId <= 0)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10382);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    ProcResourceEquipmentBindEntity equ = new ProcResourceEquipmentBindEntity();
                    equ = new ProcResourceEquipmentBindEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ResourceId = entity.Id,
                        EquipmentId = item.EquipmentId,
                        IsMain = item.IsMain,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    equList.Add(equ);
                }
            }

            //资源设置数据
            List<ProcResourceConfigResEntity> resSetList = new List<ProcResourceConfigResEntity>();
            if (param.ResList != null && param.ResList.Count > 0)
            {
                int i = 0;
                foreach (var item in param.ResList)
                {
                    i++;
                    if (item.SetType == null || !Enum.IsDefined(typeof(ResourceSetTypeEnum), item.SetType))
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10383);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    if (item.Value == null || item.Value == 0 || !long.TryParse(item.Value.ToString(), out long a))
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10384);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    ProcResourceConfigResEntity resSet = new ProcResourceConfigResEntity();
                    resSet = new ProcResourceConfigResEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ResourceId = entity.Id,
                        SetType = (int)item.SetType,
                        Value = item.Value.ToString()!,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    resSetList.Add(resSet);
                }
            }

            //作业设置数据
            List<InteJobBusinessRelationEntity> jobList = new List<InteJobBusinessRelationEntity>();
            if (param.JobList != null && param.JobList.Count > 0)
            {
                int i = 0;
                int index = 1;
                foreach (var item in param.JobList)
                {
                    i++;
                    if (item.LinkPoint == null || !Enum.IsDefined(typeof(ResourceJobLinkPointEnum), item.LinkPoint))
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10385);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    if (item.JobId <= 0)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10386);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    //if (string.IsNullOrWhiteSpace(item.Parameter.Trim()))
                    //{
                    //    var validationFailure = new ValidationFailure();
                    //    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    //    {
                    //        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                    //        { "CollectionIndex", i}
                    //    };
                    //    }
                    //    else
                    //    {
                    //        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                    //    }
                    //    validationFailure.ErrorCode = nameof(ErrorCode.MES10387);
                    //    validationFailures.Add(validationFailure);
                    //    continue;
                    //}

                    InteJobBusinessRelationEntity job = new InteJobBusinessRelationEntity();
                    job = new InteJobBusinessRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        BusinessId = entity.Id,
                        BusinessType = (int)InteJobBusinessTypeEnum.Resource,
                        OrderNumber = index,
                        LinkPoint = (int)item.LinkPoint,
                        JobId = item.JobId,
                        IsUse = item.IsUse,
                        Parameter = item.Parameter,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    index++;
                    jobList.Add(job);
                }
            }

            //productSet
            List<ProcProductSetEntity> productSetList = new List<ProcProductSetEntity>();
            if (param.ProductSetList != null && param.ProductSetList.Count > 0)
            {
                foreach (var item in param.ProductSetList)
                {
                    var relationEntity = new ProcProductSetEntity();
                    relationEntity.Id = IdGenProvider.Instance.CreateId();
                    relationEntity.ProductId = item.ProductId;
                    relationEntity.SetPointId = entity.Id;
                    relationEntity.SemiProductId = item.SemiProductId;
                    relationEntity.SiteId = siteId;
                    relationEntity.CreatedBy = userName;
                    relationEntity.UpdatedBy = userName;
                    productSetList.Add(relationEntity);
                }
            }

            //资质认证设置数据
            var authenticationRelationEntities = new List<ProcResourceQualificationAuthenticationRelationEntity>();
            if (param.AuthSetList != null && param.AuthSetList.Count > 0)
            {
                foreach (var item in param.AuthSetList)
                {
                    authenticationRelationEntities.Add(new ProcResourceQualificationAuthenticationRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ResourceId = entity.Id,
                        IsEnable = item.IsEnable,
                        QualificationAuthenticationId = item.AuthenticationId
                    });
                }
            }
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("MES10107"), validationFailures);
            }
            #endregion

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //入库
                await _resourceRepository.InsertAsync(entity);

                if (param.PrintList != null && param.PrintList.Count > 0)
                {
                    await _resourceConfigPrintRepository.InsertRangeAsync(printList);
                }
                if (param.EquList != null && param.EquList.Count > 0)
                {
                    await _resourceEquipmentBindRepository.InsertRangeAsync(equList);
                }
                if (param.ResList != null && param.ResList.Count > 0)
                {
                    await _procResourceConfigResRepository.InsertRangeAsync(resSetList);
                }
                if (param.JobList != null && param.JobList.Count > 0)
                {
                    await _jobBusinessRelationRepository.InsertRangeAsync(jobList);
                }
                if (productSetList != null && productSetList.Count > 0)
                {
                    await _procProductSetRepository.InsertsAsync(productSetList);
                }
                if (authenticationRelationEntities.Any())
                {
                    await _authenticationRelationRepository.InsertRangeAsync(authenticationRelationEntities);
                }

                ts.Complete();
            }
            return entity.Id;
        }

        /// <summary>
        /// 修改资源数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateProcResrouceAsync(ProcResourceModifyDto param)
        {
            string userName = _currentUser.UserName;
            #region 验证
            if (param == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            param.ResName = param.ResName.Trim();
            param.Remark = param.Remark.Trim();
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(param);

            var entityOld = await _resourceRepository.GetByIdAsync(param.Id)
                ?? throw new BusinessException(nameof(ErrorCode.MES10388));

            //验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => (int)x == entityOld.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            //资源类型在系统中不存在,请重新输入!
            if (param.ResTypeId > 0)
            {
                var resourceType = await _resourceTypeRepository.GetByIdAsync(param.ResTypeId);
                if (resourceType == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10310));
                }
            }

            if (param.PrintList != null && param.PrintList.Count > 0)
            {
                param.PrintList.ForEach(x =>
                {
                    if (x.PrintId == 0)
                    {
                        x.PrintId = x.Id == null ? 0 : x.Id.ParseToLong();
                    }
                });
                //判断打印机是否重复配置  数据库中 已经存储的情况
                if (param.PrintList.GroupBy(x => x.PrintId).Any(g => g.Count() >= 2))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10313));
                }
            }

            //判断是否勾选了多个主设备，只能有一个主设备
            if (param.EquList != null && param.EquList.Count > 0)
            {
                var equNumber = param.EquList.ToLookup(w => w.EquipmentId).ToDictionary(d => d.Key, d => d);
                if (equNumber.Keys.Count < param.EquList.Count)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10314));
                }

                //判断打印机是否重复配置  数据库中 已经存储的情况
                var parmEquIds = param.EquList.Select(x => x.EquipmentId).ToArray();

                var ismianList = param.EquList.Where(a => a.IsMain).ToList();
                if (ismianList.Count > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10307));
                }
            }

            //判断资质是否重复配置  数据库中 已经存储的情况
            if (param.AuthSetList != null && param.AuthSetList.Count > 0 && param.AuthSetList.GroupBy(x => x.AuthenticationId).Any(g => g.Count() >= 2))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10389));
            }
            #endregion

            //DTO转换实体
            var entity = new ProcResourceEntity
            {
                Id = param.Id,
                //Status = param.Status,
                ResName = param.ResName,
                ResTypeId = param.ResTypeId,
                Remark = param.Remark ?? "",
                UpdatedBy = userName
            };

            var validationFailures = new List<ValidationFailure>();

            //打印机数据
            List<ProcResourceConfigPrintEntity> addPrintList = new List<ProcResourceConfigPrintEntity>();
            if (param.PrintList != null && param.PrintList.Count > 0)
            {
                int i = 0;
                foreach (var item in param.PrintList.Select(x => x.PrintId))
                {
                    i++;
                    if (item <= 0)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10381);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    ProcResourceConfigPrintEntity print = new ProcResourceConfigPrintEntity();
                    print = new ProcResourceConfigPrintEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ResourceId = param.Id,
                        PrintId = item,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    addPrintList.Add(print);
                }
            }

            //设备绑定设置数据
            List<ProcResourceEquipmentBindEntity> addEquList = new List<ProcResourceEquipmentBindEntity>();
            if (param.EquList != null && param.EquList.Count > 0)
            {
                int i = 0;
                foreach (var item in param.EquList)
                {
                    i++;
                    if (item.EquipmentId <= 0)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10382);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    ProcResourceEquipmentBindEntity equ = new ProcResourceEquipmentBindEntity();
                    equ = new ProcResourceEquipmentBindEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ResourceId = param.Id,
                        EquipmentId = item.EquipmentId,
                        IsMain = item.IsMain,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    addEquList.Add(equ);
                }
            }

            //资源设置数据
            List<ProcResourceConfigResEntity> addResSetList = new List<ProcResourceConfigResEntity>();
            if (param.ResList != null && param.ResList.Count > 0)
            {
                int i = 0;
                foreach (var item in param.ResList)
                {
                    i++;
                    if (item.SetType == null || !Enum.IsDefined(typeof(ResourceSetTypeEnum), item.SetType))
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10383);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    if (item.Value == null || item.Value == 0 || !long.TryParse(item.Value.ToString(), out long a))
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10384);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    ProcResourceConfigResEntity resSet = new ProcResourceConfigResEntity();
                    resSet = new ProcResourceConfigResEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ResourceId = param.Id,
                        SetType = (int)item.SetType,
                        Value = item?.Value.ToString()!,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    addResSetList.Add(resSet);
                }
            }

            //作业设置数据
            List<InteJobBusinessRelationEntity> addJobList = new List<InteJobBusinessRelationEntity>();
            if (param.JobList != null && param.JobList.Count > 0)
            {
                int i = 0;
                int index = 1;
                foreach (var item in param.JobList)
                {

                    i++;
                    if (item.LinkPoint == null || !Enum.IsDefined(typeof(ResourceJobLinkPointEnum), item.LinkPoint))
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10385);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    if (item.JobId <= 0)
                    {
                        var validationFailure = new ValidationFailure();
                        if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                        {
                            validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", i}
                        };
                        }
                        else
                        {
                            validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                        }
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10386);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    //if (string.IsNullOrWhiteSpace(item.Parameter.Trim()))
                    //{
                    //    var validationFailure = new ValidationFailure();
                    //    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    //    {
                    //        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                    //        { "CollectionIndex", i}
                    //    };
                    //    }
                    //    else
                    //    {
                    //        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", i);
                    //    }
                    //    validationFailure.ErrorCode = nameof(ErrorCode.MES10387);
                    //    validationFailures.Add(validationFailure);
                    //    continue;
                    //}

                    InteJobBusinessRelationEntity job = new InteJobBusinessRelationEntity();
                    job = new InteJobBusinessRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        BusinessId = param.Id,
                        BusinessType = (int)InteJobBusinessTypeEnum.Resource,
                        OrderNumber = index,
                        LinkPoint = (int)item.LinkPoint,
                        JobId = item.JobId,
                        IsUse = item.IsUse,
                        Parameter = item.Parameter,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    index++;
                    addJobList.Add(job);
                }
            }

            //productSet
            List<ProcProductSetEntity> productSetList = new List<ProcProductSetEntity>();
            if (param.ProductSetList != null && param.ProductSetList.Count > 0)
            {
                foreach (var item in param.ProductSetList)
                {
                    var relationEntity = new ProcProductSetEntity();
                    relationEntity.Id = IdGenProvider.Instance.CreateId();
                    relationEntity.ProductId = item.ProductId;
                    relationEntity.SetPointId = param.Id;
                    relationEntity.SemiProductId = item.SemiProductId;
                    relationEntity.SiteId = _currentSite.SiteId ?? 0;
                    relationEntity.CreatedBy = userName;
                    relationEntity.UpdatedBy = userName;
                    productSetList.Add(relationEntity);
                }
            }

            //资质认证设置数据
            var authenticationRelationEntities = new List<ProcResourceQualificationAuthenticationRelationEntity>();
            if (param.AuthSetList != null && param.AuthSetList.Count > 0)
            {
                foreach (var item in param.AuthSetList)
                {
                    authenticationRelationEntities.Add(new ProcResourceQualificationAuthenticationRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ResourceId = entity.Id,
                        IsEnable = item.IsEnable,
                        QualificationAuthenticationId = item.AuthenticationId
                    });
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("MES10107"), validationFailures);
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //入库
                await _resourceRepository.UpdateAsync(entity);

                //删除之前的数据
                await _resourceConfigPrintRepository.DeleteByResourceIdAsync(param.Id);
                //打印机数据
                if (addPrintList != null && addPrintList.Count > 0)
                {
                    await _resourceConfigPrintRepository.InsertRangeAsync(addPrintList);
                }

                //删除之前的数据
                await _resourceEquipmentBindRepository.DeleteByResourceIdAsync(param.Id);
                //设备数据
                if (addEquList != null && addEquList.Count > 0)
                {
                    await _resourceEquipmentBindRepository.InsertRangeAsync(addEquList);
                }

                //删除之前的数据
                await _procResourceConfigResRepository.DeleteByResourceIdAsync(param.Id);
                //资源设置数据
                if (addResSetList != null && addResSetList.Count > 0)
                {
                    await _procResourceConfigResRepository.InsertRangeAsync(addResSetList);
                }

                //删除之前的数据
                await _jobBusinessRelationRepository.DeleteByBusinessIdAsync(param.Id);
                //作业设置数据
                if (addJobList != null && addJobList.Count > 0)
                {
                    await _jobBusinessRelationRepository.InsertRangeAsync(addJobList);
                }

                //产出设置
                await _procProductSetRepository.DeleteBySetPointIdAsync(param.Id);
                if (productSetList != null && productSetList.Count > 0)
                {
                    await _procProductSetRepository.InsertsAsync(productSetList);
                }

                //资质设置
                await _authenticationRelationRepository.DeleteByResourceIdAsync(param.Id);
                if (authenticationRelationEntities.Any())
                {
                    await _authenticationRelationRepository.InsertRangeAsync(authenticationRelationEntities);
                }
                ts.Complete();
            }
        }

        /// <summary>
        /// 批量删除资源类型数据
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcResourceAsync(long[] idsAr)
        {
            if (idsAr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            var entitys = await _resourceRepository.GetListByIdsAsync(idsAr);
            if (entitys != null && entitys.Any(a => a.Status != (int)SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            //资源被工作中心引用不能删除
            var workCenterIds = await _inteWorkCenterRepository.GetWorkCenterIdByResourceIdAsync(idsAr);
            if (workCenterIds != null && workCenterIds.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10355));
            }

            var command = new DeleteCommand
            {
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now(),
                Ids = idsAr
            };
            int rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                rows += await _resourceRepository.DeleteRangeAsync(command);
                rows += await _jobBusinessRelationRepository.DeleteByBusinessIdRangeAsync(idsAr);
                rows += await _authenticationRelationRepository.DeleteByResourceIdsAsync(idsAr);
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 查询资源绑定的设备
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SelectOptionDto>> QueryEquipmentsByResourceIdAsync(long resourceId)
        {
            var resourceEquipmentBindEntities = await _resourceEquipmentBindRepository.GetByResourceIdAsync(new ProcResourceEquipmentBindQuery { ResourceId = resourceId });

            List<SelectOptionDto> selectOptionDtos = new();
            foreach (var item in resourceEquipmentBindEntities.Select(x => x.EquipmentId))
            {
                var equipmentEntity = await _equEquipmentRepository.GetByIdAsync(item);
                if (equipmentEntity == null) continue;

                selectOptionDtos.Add(new SelectOptionDto
                {
                    Key = $"{item}",
                    Label = $"{equipmentEntity.EquipmentName}",
                    Value = $"{item}"
                });
            }

            return selectOptionDtos;
        }

        #region 状态变更
        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(ChangeStatusDto param)
        {
            #region 参数校验
            if (param.Id == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10125));
            }
            if (!Enum.IsDefined(typeof(SysDataStatusEnum), param.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10126));
            }
            if (param.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10128));
            }

            #endregion

            var changeStatusCommand = new ChangeStatusCommand()
            {
                Id = param.Id,
                Status = param.Status,

                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            #region 校验数据
            var entity = await _resourceRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10388));
            }
            if (entity.Status == (int)changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
            #endregion

            #region 操作数据库
            await _resourceRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion
    }
}
