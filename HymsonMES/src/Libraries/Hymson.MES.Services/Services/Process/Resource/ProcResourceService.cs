using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.Resource;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Minio.DataModel;
using System.Security.Policy;
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
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 产出设置
        /// </summary>
        private readonly IProcProductSetRepository _procProductSetRepository;


        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        private readonly AbstractValidator<ProcResourceCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcResourceModifyDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
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
            resourcePagedQuery.SiteId = _currentSite.SiteId ?? 123456;
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
            resourcePagedQuery.SiteId = _currentSite.SiteId ?? 123456;
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
        /// 查询资源类型下关联的资源(资源类型详情：可用资源，已分配资源)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<List<ProcResourceDto>> GetListForGroupAsync(ProcResourcePagedQueryDto query)
        {
            var resourcePagedQuery = query.ToQuery<ProcResourcePagedQuery>();
            resourcePagedQuery.SiteId = _currentSite.SiteId ?? 123456;
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
        public async Task<PagedInfo<ProcResourceDto>> GettPageListByProcedureIdAsync(ProcResourceProcedurePagedQueryDto query)
        {
            var resourcePagedQuery = query.ToQuery<ProcResourceProcedurePagedQuery>();
            resourcePagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _resourceRepository.GettPageListByProcedureIdAsync(resourcePagedQuery);

            //实体到DTO转换 装载数据
            var procResourceDtos = new List<ProcResourceDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceDto = entity.ToModel<ProcResourceDto>();
                procResourceDtos.Add(resourceDto);
            }
            return new PagedInfo<ProcResourceDto>(procResourceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
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
        /// 获取资源关联作业数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        //public async Task<PagedInfo<ProcResourceConfigJobViewDto>> GetcResourceConfigJoAsync(ProcResourceConfigJobPagedQueryDto query)
        //{
        //    var jobPagedQuery = query.ToQuery<ProcResourceConfigJobPagedQuery>();
        //    var pagedInfo = await _jobBusinessRelationRepository.GetPagedInfoAsync(jobPagedQuery);

        //    //实体到DTO转换 装载数据
        //    var procResourceConfigJobViews = new List<ProcResourceConfigJobViewDto>();
        //    foreach (var entity in pagedInfo.Data)
        //    {
        //        var resourceTypeDto = entity.ToModel<ProcResourceConfigJobViewDto>();
        //        procResourceConfigJobViews.Add(resourceTypeDto);
        //    }
        //    return new PagedInfo<ProcResourceConfigJobViewDto>(procResourceConfigJobViews, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        //}

        /// <summary>
        /// 获取资源配置Job信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcedureJobReleationDto>> GetProcedureConfigJobListAsync(InteJobBusinessRelationPagedQueryDto queryDto)
        {
            var query = new InteJobBusinessRelationPagedQuery()
            {
                SiteId = _currentSite.SiteId ?? 123456,
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
                SiteId = _currentSite.SiteId ?? 123456,
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
                    }); ;
                }
            }

            return procProductSetDtos;
        }

        /// <summary>
        /// 添加资源维护表所有页签的数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task AddProcResourceAsync(ProcResourceCreateDto parm)
        {
            #region 验证
            if (parm == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            parm.ResCode = parm.ResCode.ToTrimSpace().ToUpperInvariant();
            parm.ResName = parm.ResName.Trim();
            parm.Remark = parm.Remark.Trim();
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(parm);

            var resCode = parm.ResCode;
            var query = new ProcResourceQuery
            {
                SiteId = _currentSite.SiteId ?? 123456,
                ResCode = resCode
            };
            if (await _resourceRepository.IsExistsAsync(query))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10308)).WithData("ResCode", parm.ResCode);
            }

            if (parm.ResTypeId > 0)
            {
                var resourceType = await _resourceTypeRepository.GetByIdAsync(parm.ResTypeId ?? 0);
                if (resourceType == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10310));
                }
            }

            //打印机验证
            if (parm.PrintList != null && parm.PrintList.Count > 0)
            {
                //判断打印机是否重复配置  数据库中 已经存储的情况
                if (parm.PrintList.GroupBy(x => x.PrintId).Where(g => g.Count() >= 2).Count() >= 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10313));
                }
            }

            //判断是否勾选了多个主设备，只能有一个主设备
            if (parm.EquList != null && parm.EquList.Count > 0)
            {
                var equNumber = parm.EquList.ToLookup(w => w.EquipmentId).ToDictionary(d => d.Key, d => d);
                if (equNumber.Keys.Count < parm.EquList.Count)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10314));
                }

                /*
                if (parm.EquList.GroupBy(x => x.EquipmentId).Where(g => g.Count() >= 2).Count() >= 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10306));
                }
                */

                var ismianCount = parm.EquList.Where(a => a.IsMain == true).ToList().Count;
                if (ismianCount > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10307));
                }
            }
            #endregion

            var siteId = _currentSite.SiteId ?? 123456;
            var userName = _currentUser.UserName;

            #region 组装数据

            //DTO转换实体
            var entity = new ProcResourceEntity
            {
                Remark = parm.Remark,
                ResName = parm.ResName,
                ResTypeId = parm.ResTypeId ?? 0,
                Status = (int)parm.Status,

                Id = IdGenProvider.Instance.CreateId(),
                SiteId = siteId,
                CreatedBy = userName,
                UpdatedBy = userName,
                ResCode = resCode
            };

            var validationFailures = new List<ValidationFailure>();

            //打印机数据
            List<ProcResourceConfigPrintEntity> printList = new List<ProcResourceConfigPrintEntity>();
            if (parm.PrintList != null && parm.PrintList.Count > 0)
            {
                int i = 0;
                foreach (var item in parm.PrintList)
                {
                    i++;
                    if (item.PrintId <= 0)
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
                        PrintId = item.PrintId,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 123456,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    printList.Add(print);
                }
            }

            //设备绑定设置数据
            List<ProcResourceEquipmentBindEntity> equList = new List<ProcResourceEquipmentBindEntity>();
            if (parm.EquList != null && parm.EquList.Count > 0)
            {
                int i = 0;
                foreach (var item in parm.EquList)
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
                        SiteId = _currentSite.SiteId ?? 123456,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    equList.Add(equ);
                }
            }

            //资源设置数据
            List<ProcResourceConfigResEntity> resSetList = new List<ProcResourceConfigResEntity>();
            if (parm.ResList != null && parm.ResList.Count > 0)
            {
                int i = 0;
                foreach (var item in parm.ResList)
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
                        Value = item.Value.ToString(),
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 123456,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    resSetList.Add(resSet);
                }
            }

            //作业设置数据
            List<InteJobBusinessRelationEntity> jobList = new List<InteJobBusinessRelationEntity>();
            if (parm.JobList != null && parm.JobList.Count > 0)
            {
                int i = 0;
                foreach (var item in parm.JobList)
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
                    if (string.IsNullOrWhiteSpace(item.Parameter.Trim()))
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10387);
                        validationFailures.Add(validationFailure);
                        continue;
                    }

                    InteJobBusinessRelationEntity job = new InteJobBusinessRelationEntity();
                    job = new InteJobBusinessRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        BusinessId = entity.Id,
                        BusinessType = (int)InteJobBusinessTypeEnum.Resource,
                        OrderNumber = "",
                        LinkPoint = (int)item.LinkPoint,
                        JobId = item.JobId,
                        IsUse = item.IsUse,
                        Parameter = item.Parameter,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 123456,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    jobList.Add(job);
                }
            }

            //productSet
            List<ProcProductSetEntity> productSetList = new List<ProcProductSetEntity>();
            if (parm.ProductSetList != null && parm.ProductSetList.Count > 0)
            {
                foreach (var item in parm.ProductSetList)
                {
                    var relationEntity = new ProcProductSetEntity(); ;
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
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("MES10107"), validationFailures);
            }
            #endregion

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //入库
                await _resourceRepository.InsertAsync(entity);

                if (parm.PrintList != null && parm.PrintList.Count > 0)
                {
                    await _resourceConfigPrintRepository.InsertRangeAsync(printList);
                }
                if (parm.EquList != null && parm.EquList.Count > 0)
                {
                    await _resourceEquipmentBindRepository.InsertRangeAsync(equList);
                }
                if (parm.ResList != null && parm.ResList.Count > 0)
                {
                    await _procResourceConfigResRepository.InsertRangeAsync(resSetList);
                }
                if (parm.JobList != null && parm.JobList.Count > 0)
                {
                    await _jobBusinessRelationRepository.InsertRangeAsync(jobList);
                }
                if (productSetList != null && productSetList.Count > 0)
                {
                    await _procProductSetRepository.InsertsAsync(productSetList);
                }
                ts.Complete();
            }
        }

        /// <summary>
        /// 修改资源数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateProcResrouceAsync(ProcResourceModifyDto param)
        {
            string userName = _currentUser.UserName;
            var siteCode = _currentSite.SiteId ?? 123456;
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

            if (entityOld.Status != (int)SysDataStatusEnum.Build && param.Status == (int)SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10108));
            }
            if (!Enum.IsDefined(typeof(SysDataStatusEnum), (SysDataStatusEnum)param.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10380));
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
                        x.PrintId = x.Id.ParseToLong();
                    }
                });
                //判断打印机是否重复配置  数据库中 已经存储的情况
                if (param.PrintList.GroupBy(x => x.PrintId).Where(g => g.Count() >= 2).Count() >= 1)
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

                /*
                if (param.EquList.GroupBy(x => x.EquipmentId).Where(g => g.Count() >= 2).Count() >= 1)
                {
                    throw new Exception(nameof(ErrorCode.MES10314));
                }
                */

                //判断打印机是否重复配置  数据库中 已经存储的情况
                var parmEquIds = param.EquList.Select(x => x.EquipmentId).ToArray();
                var equQuery = new ProcResourceEquipmentBindQuery
                {
                    ResourceId = param.Id,
                    Ids = parmEquIds
                };

                var ismianList = param.EquList.Where(a => a.IsMain == true).ToList();
                if (ismianList.Count > 1)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10307));
                }
            }
            #endregion

            //DTO转换实体
            var entity = new ProcResourceEntity
            {
                Id = param.Id,
                Status = param.Status,
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
                foreach (var item in param.PrintList)
                {
                    i++;
                    if (item.PrintId <= 0)
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
                        PrintId = item.PrintId,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 123456,
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
                        SiteId = _currentSite.SiteId ?? 123456,
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
                        Value = item?.Value.ToString(),
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 123456,
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
                    if (string.IsNullOrWhiteSpace(item.Parameter.Trim()))
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10387);
                        validationFailures.Add(validationFailure);
                        continue;
                    }

                    InteJobBusinessRelationEntity job = new InteJobBusinessRelationEntity();
                    job = new InteJobBusinessRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        BusinessId = param.Id,
                        BusinessType = (int)InteJobBusinessTypeEnum.Resource,
                        OrderNumber = "",
                        LinkPoint = (int)item.LinkPoint,
                        JobId = item.JobId,
                        IsUse = item.IsUse,
                        Parameter = item.Parameter,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 123456,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    addJobList.Add(job);
                }
            }

            //productSet
            List<ProcProductSetEntity> productSetList = new List<ProcProductSetEntity>();
            if (param.ProductSetList != null && param.ProductSetList.Count > 0)
            {
                foreach (var item in param.ProductSetList)
                {
                    var relationEntity = new ProcProductSetEntity(); ;
                    relationEntity.Id = IdGenProvider.Instance.CreateId();
                    relationEntity.ProductId = item.ProductId;
                    relationEntity.SetPointId = param.Id;
                    relationEntity.SemiProductId = item.SemiProductId;
                    relationEntity.SiteId = _currentSite.SiteId ?? 123456;
                    relationEntity.CreatedBy = userName;
                    relationEntity.UpdatedBy = userName;
                    productSetList.Add(relationEntity);
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
                    // return Error(ResultCode.FAIL, "插入资源关联打印表失败");
                }

                //删除之前的数据
                await _resourceEquipmentBindRepository.DeleteByResourceIdAsync(param.Id);
                //设备数据
                if (addEquList != null && addEquList.Count > 0)
                {
                    await _resourceEquipmentBindRepository.InsertRangeAsync(addEquList);
                    //  return Error(ResultCode.FAIL, "插入资源关联设备失败");
                }

                //删除之前的数据
                await _procResourceConfigResRepository.DeleteByResourceIdAsync(param.Id);
                //资源设置数据
                if (addResSetList != null && addResSetList.Count > 0)
                {
                    await _procResourceConfigResRepository.InsertRangeAsync(addResSetList);
                    // return Error(ResultCode.FAIL, "插入工序关联作业表失败");
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

                ts.Complete();
            }
        }

        /// <summary>
        /// 批量删除资源类型数据
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcResourceAsync(long[] idsArr)
        {
            if (idsArr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            ////不能删除启用状态的资源
            //var query = new ProcResourceQuery
            //{
            //    IdsArr = idsArr,
            //    Status = (int)SysDataStatusEnum.Enable
            //};
            //var resourceList = await _resourceRepository.GetByIdsAsync(query);
            //if (resourceList != null && resourceList.Any())
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES10319));
            //}
            var entitys = await _resourceRepository.GetListByIdsAsync(idsArr);
            if (entitys != null && entitys.Any(a => a.Status != (int)SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            //资源被工作中心引用不能删除
            var workCenterIds = await _inteWorkCenterRepository.GetWorkCenterIdByResourceIdAsync(idsArr);
            if (workCenterIds != null && workCenterIds.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10355));
            }

            var command = new DeleteCommand
            {
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now(),
                Ids = idsArr
            };
            int rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                rows += await _resourceRepository.DeleteRangeAsync(command);
                rows += await _jobBusinessRelationRepository.DeleteByBusinessIdRangeAsync(idsArr);
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改资源数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        //public async Task UpdateProcResrouceAsync(ProcResourceModifyDto param)
        //{
        //    string userName = _currentUser.UserName;
        //    var siteCode = _currentSite.SiteId??0;
        //    #region 验证
        //    if (param == null)
        //    {
        //        throw new ValidationException(ErrorCode.MES10100);
        //    }

        //    //验证DTO
        //    await _validationModifyRules.ValidateAndThrowAsync(param);

        //    //资源类型在系统中不存在,请重新输入!
        //    if (param.ResTypeId > 0)
        //    {
        //        var resourceType = await _resourceTypeRepository.GetByIdAsync(param.ResTypeId);
        //        if (resourceType == null)
        //        {
        //            throw new ValidationException(ErrorCode.MES10310);
        //        }
        //    }

        //    if (param.PrintList != null && param.PrintList.Count > 0)
        //    {
        //        if (param.PrintList.Where(x => x.OperationType != 3).GroupBy(x => x.PrintId).Where(g => g.Count() >= 2).Count() >= 1)
        //        {
        //            throw new ValidationException(ErrorCode.MES10313);
        //        }

        //        //判断打印机是否重复配置  数据库中 已经存储的情况

        //        var parmPrintIds = param.PrintList.Select(x => x.PrintId);
        //        var printQuery = new ProcResourceConfigPrintQuery
        //        {
        //            ResourceId = param.Id,
        //            Ids = parmPrintIds.ToArray()
        //        };
        //        var resourcePrintList = await _resourceConfigPrintRepository.GetByResourceIdAsync(printQuery);
        //        if (resourcePrintList != null && resourcePrintList.Any())
        //        {
        //            foreach (var item in resourcePrintList)
        //            {
        //                var parmPrint = param.PrintList.Where(x => x.PrintId == item.PrintId);
        //                //  判断参数中打印机数据情况
        //                //     传入一条 只能为修改或者删除
        //                //     传入两条 只能为删除和新增
        //                //     其他则为异常
        //                if (parmPrint.Count() == 1)
        //                {
        //                    if (parmPrint.FirstOrDefault()?.OperationType == 1)
        //                        throw new ValidationException(ErrorCode.MES10313);
        //                }
        //                else if (parmPrint.Count() == 2)
        //                {
        //                    if (!(parmPrint.Where(x => x.OperationType == 3).Count() == 1 && parmPrint.Where(x => x.OperationType == 1).Count() == 1))
        //                    {
        //                        throw new ValidationException(ErrorCode.MES10313);
        //                    }
        //                }
        //                else
        //                {
        //                    throw new ValidationException(ErrorCode.MES10313);
        //                }
        //            }
        //        }
        //    }

        //    //判断是否勾选了多个主设备，只能有一个主设备
        //    if (param.EquList != null && param.EquList.Count > 0)
        //    {
        //        if (param.EquList.Where(x => x.OperationType != 3).GroupBy(x => x.EquipmentId).Where(g => g.Count() >= 2).Count() >= 1)
        //        {
        //            throw new Exception(ErrorCode.MES10314);
        //        }

        //        //判断打印机是否重复配置  数据库中 已经存储的情况
        //        var parmEquIds = param.EquList.Select(x => x.EquipmentId).ToArray();
        //        var equQuery = new ProcResourceEquipmentBindQuery
        //        {
        //            ResourceId = param.Id,
        //            Ids = parmEquIds
        //        };
        //        var resourceEquList = await _resourceEquipmentBindRepository.GetByResourceIdAsync(equQuery);
        //        if (resourceEquList != null && resourceEquList.Any())
        //        {
        //            foreach (var item in resourceEquList)
        //            {
        //                var parmEqu = param?.PrintList?.Where(x => x.PrintId == item.EquipmentId);
        //                //  判断参数中打印机数据情况
        //                //     传入一条 只能为修改或者删除
        //                //     传入两条 只能为删除和新增
        //                //     其他则为异常
        //                if (parmEqu?.Count() == 1)
        //                {
        //                    if (parmEqu.FirstOrDefault()?.OperationType == 1)
        //                        throw new ValidationException(ErrorCode.MES10314);
        //                }
        //                else if (parmEqu?.Count() == 2)
        //                {
        //                    if (!(parmEqu.Where(x => x.OperationType == 3).Count() == 1 && parmEqu.Where(x => x.OperationType == 1).Count() == 1))
        //                    {
        //                        throw new ValidationException(ErrorCode.MES10314);
        //                    }
        //                }
        //                else
        //                {
        //                    throw new ValidationException(ErrorCode.MES10314);
        //                }
        //            }
        //        }

        //        var ismianList = param.EquList.Where(a => a.IsMain == true && a.OperationType != 3).ToList();
        //        if (ismianList.Count > 1)
        //        {
        //            throw new ValidationException(ErrorCode.MES10307);
        //        }
        //        else
        //        {
        //            //判断当前存储与参数主设备是否一致
        //            //        不一致 则判断 存储设备是否设置成非主设备
        //            var equQueryMain = new ProcResourceEquipmentBindQuery
        //            {
        //                ResourceId = param.Id,
        //                IsMain = true
        //            };
        //            var mainList = await _resourceEquipmentBindRepository.GetByResourceIdAsync(equQueryMain);
        //            if (mainList != null)
        //            {
        //                var isMain = mainList.FirstOrDefault();
        //                if (ismianList.FirstOrDefault()?.EquipmentId != isMain?.EquipmentId)
        //                {
        //                    if (param.EquList.Where(x => x.EquipmentId == isMain?.EquipmentId).FirstOrDefault() == null)
        //                    {
        //                        throw new CustomerValidationException(ErrorCode.MES10307);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    #endregion

        //    //DTO转换实体
        //    var entity = new ProcResourceEntity
        //    {
        //        Id = param.Id,
        //        Status = param.Status,
        //        ResName = param.ResName,
        //        ResTypeId = param.ResTypeId,
        //        Remark = param.Remark ?? "",
        //        UpdatedBy = userName
        //    };

        //    //打印机数据
        //    List<ProcResourceConfigPrintEntity> addPrintList = new List<ProcResourceConfigPrintEntity>();
        //    List<ProcResourceConfigPrintEntity> updaterPintList = new List<ProcResourceConfigPrintEntity>();
        //    List<long> deletePintIds = new List<long>();
        //    if (param.PrintList != null && param.PrintList.Count > 0)
        //    {
        //        foreach (var item in param.PrintList)
        //        {
        //            ProcResourceConfigPrintEntity print = new ProcResourceConfigPrintEntity();
        //            switch (item.OperationType)
        //            {
        //                case 1:
        //                    print = new ProcResourceConfigPrintEntity
        //                    {
        //                        Id = IdGenProvider.Instance.CreateId(),
        //                        ResourceId = param.Id,
        //                        PrintId = item.PrintId,
        //                        Remark = "",
        //                        SiteId = _currentSite.SiteId??0,
        //                        CreatedBy = userName,
        //                        UpdatedBy = userName
        //                    };
        //                    addPrintList.Add(print);
        //                    break;
        //                case 2:
        //                    print = print = new ProcResourceConfigPrintEntity
        //                    {
        //                        Id = item.Id ?? 0,
        //                        PrintId = item.PrintId,
        //                        UpdatedBy = userName
        //                    };
        //                    updaterPintList.Add(print);
        //                    break;
        //                case 3:
        //                    if (item.Id != null && item.Id > 0)
        //                    {
        //                        deletePintIds.Add(item.Id ?? 0);
        //                    }
        //                    break;
        //                default:
        //                    throw new BusinessException(ErrorCode.MES10315).WithData("OperationType", item.OperationType);
        //            }
        //        }
        //    }

        //    //设备绑定设置数据
        //    List<ProcResourceEquipmentBindEntity> addEquList = new List<ProcResourceEquipmentBindEntity>();
        //    List<ProcResourceEquipmentBindEntity> updateEquListt = new List<ProcResourceEquipmentBindEntity>();
        //    List<long> deleteEquIds = new List<long>();
        //    if (param.EquList != null && param.EquList.Count > 0)
        //    {
        //        foreach (var item in param.EquList)
        //        {
        //            ProcResourceEquipmentBindEntity equ = new ProcResourceEquipmentBindEntity();
        //            switch (item.OperationType)
        //            {
        //                case 1:
        //                    equ = new ProcResourceEquipmentBindEntity
        //                    {
        //                        Id = IdGenProvider.Instance.CreateId(),
        //                        ResourceId = param.Id,
        //                        EquipmentId = item.EquipmentId,
        //                        IsMain = item.IsMain ?? false,
        //                        Remark = "",
        //                        SiteId = _currentSite.SiteId??0,
        //                        CreatedBy = userName,
        //                        UpdatedBy = userName
        //                    };
        //                    addEquList.Add(equ);
        //                    break;
        //                case 2:
        //                    equ = new ProcResourceEquipmentBindEntity
        //                    {
        //                        Id = item.Id ?? 0,
        //                        EquipmentId = item.EquipmentId,
        //                        IsMain = item.IsMain ?? false,
        //                        UpdatedBy = userName
        //                    };
        //                    updateEquListt.Add(equ);
        //                    break;
        //                case 3:
        //                    if (item.Id != null && item.Id > 0)
        //                    {
        //                        deleteEquIds.Add(item.Id ?? 0);
        //                    }
        //                    break;
        //                default:
        //                    throw new BusinessException(ErrorCode.MES10316).WithData("OperationType", item.OperationType);
        //            }
        //        }
        //    }

        //    //资源设置数据
        //    List<ProcResourceConfigResEntity> addResSetList = new List<ProcResourceConfigResEntity>();
        //    List<ProcResourceConfigResEntity> updateSetListt = new List<ProcResourceConfigResEntity>();
        //    List<long> deleteSetIds = new List<long>();
        //    if (param.ResList != null && param.ResList.Count > 0)
        //    {
        //        foreach (var item in param.ResList)
        //        {
        //            ProcResourceConfigResEntity resSet = new ProcResourceConfigResEntity();
        //            switch (item.OperationType)
        //            {
        //                case 1:
        //                    resSet = new ProcResourceConfigResEntity
        //                    {
        //                        Id = IdGenProvider.Instance.CreateId(),
        //                        ResourceId = param.Id,
        //                        SetType = item.SetType,
        //                        Value = item.Value,
        //                        Remark = "",
        //                        SiteId = _currentSite.SiteId??0,
        //                        CreatedBy = userName,
        //                        UpdatedBy = userName
        //                    };
        //                    addResSetList.Add(resSet);
        //                    break;
        //                case 2:
        //                    resSet = new ProcResourceConfigResEntity
        //                    {
        //                        Id = item.Id ?? 0,
        //                        SetType = item.SetType,
        //                        Value = item.Value,
        //                        UpdatedBy = userName
        //                    };
        //                    updateSetListt.Add(resSet);
        //                    break;
        //                case 3:
        //                    if (item.Id != null && item.Id > 0)
        //                    {
        //                        deleteSetIds.Add(item.Id ?? 0);
        //                    }
        //                    break;
        //                default:
        //                    throw new BusinessException(ErrorCode.MES10317).WithData("OperationType", item.OperationType);
        //            }
        //        }
        //    }

        //    //作业设置数据
        //    List<ProcResourceConfigJobEntity> addJobList = new List<ProcResourceConfigJobEntity>();
        //    List<ProcResourceConfigJobEntity> updateJobList = new List<ProcResourceConfigJobEntity>();
        //    List<long> deleteJobIds = new List<long>();
        //    if (param.JobList != null && param.JobList.Count > 0)
        //    {
        //        foreach (var item in param.JobList)
        //        {
        //            ProcResourceConfigJobEntity job = new ProcResourceConfigJobEntity();
        //            switch (item.OperationType)
        //            {
        //                case 1:
        //                    job = new ProcResourceConfigJobEntity
        //                    {
        //                        Id = IdGenProvider.Instance.CreateId(),
        //                        ResourceId = param.Id,
        //                        LinkPoint = item.LinkPoint,
        //                        JobId = item.JobId,
        //                        IsUse = item.IsUse,
        //                        Parameter = item.Parameter,
        //                        Remark = item.Remark,
        //                        SiteId = _currentSite.SiteId??0,
        //                        CreatedBy = userName,
        //                        UpdatedBy = userName
        //                    };
        //                    addJobList.Add(job);
        //                    break;
        //                case 2:
        //                    job = new ProcResourceConfigJobEntity
        //                    {
        //                        Id = item.Id ?? 0,
        //                        LinkPoint = item.LinkPoint,
        //                        JobId = item.JobId,
        //                        IsUse = item.IsUse,
        //                        Remark=item.Remark,
        //                        Parameter = item.Parameter,
        //                        UpdatedBy = userName,
        //                    };
        //                    updateJobList.Add(job);
        //                    break;
        //                case 3:
        //                    if (item.Id != null && item.Id > 0)
        //                    {
        //                        deleteJobIds.Add(item.Id ?? 0);
        //                    }
        //                    break;
        //                default:
        //                    throw new BusinessException(ErrorCode.MES10318).WithData("OperationType", item.OperationType);
        //            }
        //        }
        //    }

        //    using (TransactionScope ts = TransactionHelper.GetTransactionScope())
        //    {
        //        //入库
        //        await _resourceRepository.UpdateAsync(entity);

        //        //打印机数据
        //        if (addPrintList != null && addPrintList.Count > 0)
        //        {
        //            await _resourceConfigPrintRepository.InsertRangeAsync(addPrintList);
        //            // return Error(ResultCode.FAIL, "插入资源关联打印表失败");
        //        }
        //        if (updaterPintList != null && updaterPintList.Count > 0)
        //        {
        //            // return Error(ResultCode.FAIL, "修改资源关联打印表失败");
        //            await _resourceConfigPrintRepository.UpdateRangeAsync(updaterPintList);
        //        }
        //        if (deletePintIds != null && deletePintIds.Count > 0)
        //        {
        //            // return Error(ResultCode.FAIL, "删除资源关联打印表失败");
        //            await _resourceConfigPrintRepository.DeleteRangeAsync(deletePintIds.ToArray());
        //        }

        //        //设备数据
        //        if (addEquList != null && addEquList.Count > 0)
        //        {
        //            //  return Error(ResultCode.FAIL, "插入资源关联设备失败");
        //            await _resourceEquipmentBindRepository.InsertRangeAsync(addEquList);
        //        }
        //        if (updateEquListt != null && updateEquListt.Count > 0)
        //        {
        //            //  return Error(ResultCode.FAIL, "修改资源关联设备失败");
        //            await _resourceEquipmentBindRepository.UpdateRangeAsync(updateEquListt);
        //        }
        //        if (deleteEquIds != null && deleteEquIds.Count > 0)
        //        {
        //            // return Error(ResultCode.FAIL, "删除资源关联设备失败");
        //            await _resourceEquipmentBindRepository.DeletesRangeAsync(deleteEquIds.ToArray());
        //        }

        //        //资源设置数据
        //        if (addResSetList != null && addResSetList.Count > 0)
        //        {
        //            // return Error(ResultCode.FAIL, "插入工序关联作业表失败");
        //            await _procResourceConfigResRepository.InsertRangeAsync(addResSetList);
        //        }
        //        if (updateSetListt != null && updateSetListt.Count > 0)
        //        {
        //            // return Error(ResultCode.FAIL, "修改工序关联作业表失败");
        //            await _procResourceConfigResRepository.UpdateRangeAsync(updateSetListt);
        //        }
        //        if (deleteSetIds != null && deleteSetIds.Count > 0)
        //        {
        //            // return Error(ResultCode.FAIL, "删除工序关联作业表失败");
        //            await _procResourceConfigResRepository.DeletesRangeAsync(deleteSetIds.ToArray());
        //        }

        //        //TODO作业设置数据
        //        //if (addJobList != null && addJobList.Count > 0)
        //        //{
        //        //    // return Error(ResultCode.FAIL, "插入工序关联作业表失败");
        //        //    await _resourceConfigJobRepository.InsertRangeAsync(addJobList);
        //        //}
        //        //if (updateJobList != null && updateJobList.Count > 0)
        //        //{
        //        //    // return Error(ResultCode.FAIL, "修改工序关联作业表失败");
        //        //    await _resourceConfigJobRepository.InsertRangeAsync(updateJobList);
        //        //}
        //        //if (deleteJobIds != null && deleteJobIds.Count > 0)
        //        //{
        //        //    // return Error(ResultCode.FAIL, "删除工序关联作业表失败");
        //        //    await _resourceConfigJobRepository.DeletesRangeAsync(deleteJobIds.ToArray());
        //        //}
        //        ts.Complete();
        //    }
        //}
    }
}
