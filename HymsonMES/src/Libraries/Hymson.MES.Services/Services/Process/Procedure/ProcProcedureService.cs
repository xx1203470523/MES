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
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Minio.DataModel;
using System.Transactions;
using System.Xml.Linq;

namespace Hymson.MES.Services.Services.Process.Procedure
{
    /// <summary>
    /// 工序表 服务
    /// </summary>
    public class ProcProcedureService : IProcProcedureService
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
        /// 工序表 仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;
        /// <summary>
        /// 资源类型仓储
        /// </summary>
        private readonly IProcResourceTypeRepository _resourceTypeRepository;
        /// <summary>
        /// 工序配置作业表仓储
        /// </summary>
        private readonly IInteJobBusinessRelationRepository _jobBusinessRelationRepository;
        /// <summary>
        /// 工序配置打印表仓储
        /// </summary>
        private readonly IProcProcedurePrintRelationRepository _procedurePrintRelationRepository;
        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        /// <summary>
        /// 作业表仓储
        /// </summary>
        private readonly IInteJobRepository _inteJobRepository;
        /// <summary>
        /// 仓库标签模板 仓储
        /// </summary>
        private readonly IProcLabelTemplateRepository _procLabelTemplateRepository;
        /// <summary>
        /// 产出设置
        /// </summary>
        private readonly IProcProductSetRepository _procProductSetRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        private readonly AbstractValidator<ProcProcedureCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcProcedureModifyDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcProcedureService(
            ICurrentUser currentUser, ICurrentSite currentSite,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceTypeRepository resourceTypeRepository,
            IInteJobBusinessRelationRepository jobBusinessRelationRepository,
            IProcProcedurePrintRelationRepository procedurePrintRelationRepository,
            IProcMaterialRepository procMaterialRepository,
            IInteJobRepository inteJobRepository,
            IProcLabelTemplateRepository procLabelTemplateRepository,
            IProcProductSetRepository procProductSetRepository,
            AbstractValidator<ProcProcedureCreateDto> validationCreateRules,
            AbstractValidator<ProcProcedureModifyDto> validationModifyRules, ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procProcedureRepository = procProcedureRepository;
            _resourceTypeRepository = resourceTypeRepository;
            _jobBusinessRelationRepository = jobBusinessRelationRepository;
            _procedurePrintRelationRepository = procedurePrintRelationRepository;
            _procMaterialRepository = procMaterialRepository;
            _inteJobRepository = inteJobRepository;
            _procLabelTemplateRepository = procLabelTemplateRepository;
            _procProductSetRepository = procProductSetRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procProcedurePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureViewDto>> GetPageListAsync(ProcProcedurePagedQueryDto procProcedurePagedQueryDto)
        {
            var procProcedurePagedQuery = procProcedurePagedQueryDto.ToQuery<ProcProcedurePagedQuery>();
            procProcedurePagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _procProcedureRepository.GetPagedInfoAsync(procProcedurePagedQuery);

            //实体到DTO转换 装载数据
            List<ProcProcedureViewDto> procProcedureDtos = PrepareProcProcedureDtos(pagedInfo);
            return new PagedInfo<ProcProcedureViewDto>(procProcedureDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// PDA获取指定工序
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureViewPDADto>> GetProcProcedurePDAAsync()
        {
            long site = _currentSite.SiteId ?? 123456;
            //OP13 模组上线
            //OP17 CCS焊接&焊中检测
            //OP23 模组入箱
            //OP12 模组挤压 
            //OP04 电芯扫码&OCVR测试
            //OP14 端板刻码-电芯扫码绑定
            //string[] query=  { "OP13", "OP17", "OP12", "OP04" };
            //目前只给此工序复投
            string[] query=  { "OP14" };
            var list = await _procProcedureRepository.GetByCodesAsync(query,site);
            var dto = new List<ProcProcedureViewPDADto>();
            foreach (var item in list)
            {
                dto.Add(new ProcProcedureViewPDADto()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                });
            }
            return dto;
        }

        /// <summary>
        /// 按编码取工序信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureViewPDADto>> GetProcProcedureByCodeAsync(string[] code)
        {
            long site = _currentSite.SiteId ?? 123456;
            var lst = await _procProcedureRepository.GetByCodesAsync(code, site);
            var dto = new List<ProcProcedureViewPDADto>();
            foreach (var item in lst)
            {
                dto.Add(new ProcProcedureViewPDADto()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                });
            }
            return dto;
        }

        /// <summary>
        /// 分页实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcProcedureViewDto> PrepareProcProcedureDtos(PagedInfo<ProcProcedureView> pagedInfo)
        {
            var procProcedureDtos = new List<ProcProcedureViewDto>();
            foreach (var procProcedureEntity in pagedInfo.Data)
            {
                var procProcedureDto = procProcedureEntity.ToModel<ProcProcedureViewDto>();
                procProcedureDtos.Add(procProcedureDto);
            }

            return procProcedureDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QueryProcProcedureDto> GetProcProcedureByIdAsync(long id)
        {
            QueryProcProcedureDto queryProcDto = new QueryProcProcedureDto();
            var procProcedureEntity = await _procProcedureRepository.GetByIdAsync(id);
            if (procProcedureEntity != null)
            {
                queryProcDto.Procedure = procProcedureEntity.ToModel<ProcProcedureDto>();
                if (procProcedureEntity.ResourceTypeId > 0)
                {
                    var resourceTypeId = procProcedureEntity.ResourceTypeId ?? 0;
                    var procResourceType = await _resourceTypeRepository.GetByIdAsync(resourceTypeId);
                    queryProcDto.ResourceType = new ProcResourceTypeDto
                    {
                        Id = procResourceType?.Id ?? 0,
                        ResType = procResourceType?.ResType ?? "",
                        ResTypeName = procResourceType?.ResTypeName ?? ""
                    };
                }
            }
            return queryProcDto;
        }

        /// <summary>
        /// 获取工序配置打印信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedurePrintReleationDto>> GetProcedureConfigPrintListAsync(ProcProcedurePrintReleationPagedQueryDto queryDto)
        {
            var query = new ProcProcedurePrintReleationPagedQuery()
            {
                SiteId = _currentSite.SiteId ?? 123456,
                ProcedureId = queryDto.ProcedureId,
                PageIndex = queryDto.PageIndex,
                PageSize = queryDto.PageSize,
                Sorting = queryDto.Sorting,
            };
            var pagedInfo = await _procedurePrintRelationRepository.GetPagedInfoAsync(query);

            //实体到DTO转换 装载数据
            var dtos = new List<ProcProcedurePrintReleationDto>();
            if (pagedInfo.Data != null && pagedInfo.Data.Any())
            {
                var materialIds = pagedInfo.Data.ToList().Select(a => a.MaterialId).ToArray();
                var materialLsit = await _procMaterialRepository.GetByIdsAsync(materialIds);

                var templateIds = pagedInfo.Data.ToList().Select(a => a.TemplateId).ToArray();
                var templateLsit = await _procLabelTemplateRepository.GetByIdsAsync(templateIds);
                foreach (var entity in pagedInfo.Data)
                {
                    var printReleationDto = entity.ToModel<ProcProcedurePrintRelationDto>();
                    var material = materialLsit.FirstOrDefault(a => a.Id == printReleationDto.MaterialId)?.ToModel<ProcMaterialDto>();
                    var template = templateLsit.FirstOrDefault(a => a.Id == printReleationDto.TemplateId)?.ToModel<ProcLabelTemplateDto>();
                    var queryEntity = new ProcProcedurePrintReleationDto
                    {
                        TemplateId = entity.TemplateId,
                        Copy = entity.Copy,
                        MaterialId = entity.MaterialId,
                        Version = entity.Version,
                        MaterialCode = material?.MaterialCode ?? "",
                        MaterialName = material?.MaterialName ?? "",
                        Name = template?.Name ?? ""
                    };
                    dtos.Add(queryEntity);
                }
            }

            return new PagedInfo<ProcProcedurePrintReleationDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取工序配置Job信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcedureJobReleationDto>> GetProcedureConfigJobListAsync(InteJobBusinessRelationPagedQueryDto queryDto)
        {
            var query = new InteJobBusinessRelationPagedQuery()
            {
                SiteId = _currentSite.SiteId ?? 123456,
                BusinessId = queryDto.BusinessId,
                BusinessType = (int)InteJobBusinessTypeEnum.Procedure,
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
        /// 获取工序产出设置
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProductSetDto>> GetProcedureProductSetListAsync(ProcProductSetQueryDto queryDto)
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
        /// 创建
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task AddProcProcedureAsync(AddProcProcedureDto parm)
        {
            #region 验证
            if (parm == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            if (parm.Procedure == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            var siteId = _currentSite.SiteId ?? 123456;
            var userName = _currentUser.UserName;
            parm.Procedure.Code = parm.Procedure.Code.ToTrimSpace().ToUpperInvariant();
            parm.Procedure.Name = parm.Procedure.Name.Trim();
            parm.Procedure.Remark = parm.Procedure.Remark.Trim();
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(parm.Procedure);

            var code = parm.Procedure.Code;
            var query = new ProcProcedureQuery
            {
                SiteId = siteId,
                Code = code
            };
            if (await _procProcedureRepository.IsExistsAsync(query))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10405)).WithData("Code", parm.Procedure.Code);
            }
            #endregion

            //DTO转换实体
            var procProcedureEntity = parm.Procedure.ToEntity<ProcProcedureEntity>();
            procProcedureEntity.Id = IdGenProvider.Instance.CreateId();
            procProcedureEntity.Code = code;
            procProcedureEntity.SiteId = siteId;
            procProcedureEntity.CreatedBy = userName;
            procProcedureEntity.UpdatedBy = userName;

            var validationFailures = new List<ValidationFailure>();

            //打印
            List<ProcProcedurePrintRelationEntity> procedureConfigPrintList = new List<ProcProcedurePrintRelationEntity>();
            if (parm.ProcedurePrintList != null && parm.ProcedurePrintList.Count > 0)
            {
                int i = 0;
                foreach (var item in parm.ProcedurePrintList)
                {
                    i++;
                    if (item.MaterialId <= 0)
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10411);
                        validationFailures.Add(validationFailure);
                        continue;
                    }

                    var releationEntity = item.ToEntity<ProcProcedurePrintRelationEntity>(); ;
                    releationEntity.Id = IdGenProvider.Instance.CreateId();
                    releationEntity.ProcedureId = procProcedureEntity.Id;
                    releationEntity.MaterialId = item.MaterialId;
                    releationEntity.Version = item.Version;
                    releationEntity.TemplateId = item.TemplateId;
                    releationEntity.Copy = item.Copy;
                    releationEntity.SiteId = siteId;
                    releationEntity.CreatedBy = userName;
                    releationEntity.UpdatedBy = userName;
                    procedureConfigPrintList.Add(releationEntity);
                }
            }

            //job
            List<InteJobBusinessRelationEntity> procedureConfigJobList = new List<InteJobBusinessRelationEntity>();
            if (parm.ProcedureJobList != null && parm.ProcedureJobList.Count > 0)
            {
                int i = 0;
                foreach (var item in parm.ProcedureJobList)
                {
                    i++;
                    if (item.LinkPoint <= 0 || !Enum.IsDefined(typeof(ResourceJobLinkPointEnum), item.LinkPoint))
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10410);
                        validationFailures.Add(validationFailure);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.Parameter))
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10412);
                        validationFailures.Add(validationFailure);
                        continue;
                    }

                    var relationEntity = new InteJobBusinessRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        BusinessType = (int)InteJobBusinessTypeEnum.Procedure,
                        BusinessId = procProcedureEntity.Id,
                        OrderNumber = "",
                        LinkPoint = (int)item.LinkPoint,
                        JobId = item.JobId,
                        IsUse = item.IsUse,
                        Parameter = item.Parameter,
                        Remark = "",
                        SiteId = siteId,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    procedureConfigJobList.Add(relationEntity);
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("MES10107"), validationFailures);
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
                    relationEntity.SetPointId = procProcedureEntity.Id;
                    relationEntity.SemiProductId = item.SemiProductId;
                    relationEntity.SiteId = siteId;
                    relationEntity.CreatedBy = userName;
                    relationEntity.UpdatedBy = userName;
                    productSetList.Add(relationEntity);
                }
            }



            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //入库
                await _procProcedureRepository.InsertAsync(procProcedureEntity);

                if (procedureConfigPrintList != null && procedureConfigPrintList.Count > 0)
                {
                    await _procedurePrintRelationRepository.InsertRangeAsync(procedureConfigPrintList);
                }

                if (procedureConfigJobList != null && procedureConfigJobList.Count > 0)
                {
                    await _jobBusinessRelationRepository.InsertRangeAsync(procedureConfigJobList);
                }

                if (productSetList != null && productSetList.Count > 0)
                {
                    await _procProductSetRepository.InsertsAsync(productSetList);
                }

                //提交
                ts.Complete();
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task UpdateProcProcedureAsync(UpdateProcProcedureDto parm)
        {
            #region
            if (parm == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            var siteId = _currentSite.SiteId ?? 123456;
            var userName = _currentUser.UserName;

            parm.Procedure.Name = parm.Procedure.Name.Trim();
            parm.Procedure.Remark = parm.Procedure.Remark.Trim();
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(parm.Procedure);

            var procProcedureEntityOld = await _procProcedureRepository.GetByIdAsync(parm.Procedure.Id) ?? throw new BusinessException(nameof(ErrorCode.MES10406));
            if (procProcedureEntityOld.Status != SysDataStatusEnum.Build && parm.Procedure.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10108));
            }
            #endregion

            //DTO转换实体
            var procProcedureEntity = parm.Procedure.ToEntity<ProcProcedureEntity>();
            procProcedureEntity.UpdatedBy = _currentUser.UserName;

            //TODO 现在关联表批量删除批量新增，后面再修改
            //打印

            var validationFailures = new List<ValidationFailure>();
            List<ProcProcedurePrintRelationEntity> procedureConfigPrintList = new List<ProcProcedurePrintRelationEntity>();
            if (parm.ProcedurePrintList != null && parm.ProcedurePrintList.Count > 0)
            {
                int i = 0;
                foreach (var item in parm.ProcedurePrintList)
                {
                    i++;
                    if (item.MaterialId <= 0)
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10411);
                        validationFailures.Add(validationFailure);
                        continue;
                    }
                    var releationEntity = item.ToEntity<ProcProcedurePrintRelationEntity>(); ;
                    releationEntity.Id = IdGenProvider.Instance.CreateId();
                    releationEntity.ProcedureId = procProcedureEntity.Id;
                    releationEntity.MaterialId = item.MaterialId;
                    releationEntity.Version = item.Version;
                    releationEntity.TemplateId = item.TemplateId;
                    releationEntity.Copy = item.Copy;
                    releationEntity.SiteId = siteId;
                    releationEntity.CreatedBy = userName;
                    releationEntity.UpdatedBy = userName;
                    procedureConfigPrintList.Add(releationEntity);
                }
            }

            //job
            List<InteJobBusinessRelationEntity> procedureConfigJobList = new List<InteJobBusinessRelationEntity>();
            if (parm.ProcedureJobList != null && parm.ProcedureJobList.Count > 0)
            {
                int i = 0;
                foreach (var item in parm.ProcedureJobList)
                {
                    i++;
                    if (item.LinkPoint <= 0 || !Enum.IsDefined(typeof(ResourceJobLinkPointEnum), item.LinkPoint))
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10410);
                        validationFailures.Add(validationFailure);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(item.Parameter))
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
                        validationFailure.ErrorCode = nameof(ErrorCode.MES10412);
                        validationFailures.Add(validationFailure);
                        continue;
                    }


                    var relationEntity = new InteJobBusinessRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        BusinessType = (int)InteJobBusinessTypeEnum.Procedure,
                        BusinessId = procProcedureEntity.Id,
                        OrderNumber = "",
                        LinkPoint = (int)item.LinkPoint,
                        JobId = item.JobId,
                        IsUse = item.IsUse,
                        Parameter = item.Parameter,
                        Remark = "",
                        SiteId = siteId,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    procedureConfigJobList.Add(relationEntity);
                }
            }
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("MES10107"), validationFailures);
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
                    relationEntity.SetPointId = procProcedureEntity.Id;
                    relationEntity.SemiProductId = item.SemiProductId;
                    relationEntity.SiteId = siteId;
                    relationEntity.CreatedBy = userName;
                    relationEntity.UpdatedBy = userName;
                    productSetList.Add(relationEntity);
                }
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                //入库
                await _procProcedureRepository.UpdateAsync(procProcedureEntity);

                await _procedurePrintRelationRepository.DeleteByProcedureIdAsync(procProcedureEntity.Id);
                if (procedureConfigPrintList != null && procedureConfigPrintList.Count > 0)
                {
                    await _procedurePrintRelationRepository.InsertRangeAsync(procedureConfigPrintList);
                }

                await _jobBusinessRelationRepository.DeleteByBusinessIdAsync(procProcedureEntity.Id);
                if (procedureConfigJobList != null && procedureConfigJobList.Count > 0)
                {
                    await _jobBusinessRelationRepository.InsertRangeAsync(procedureConfigJobList);
                }

                await _procProductSetRepository.DeleteBySetPointIdAsync(procProcedureEntity.Id);
                if (productSetList != null && productSetList.Count > 0)
                {
                    await _procProductSetRepository.InsertsAsync(productSetList);
                }

                //提交
                ts.Complete();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcProcedureAsync(long[] idsArr)
        {
            if (idsArr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            var entitys = await _procProcedureRepository.GetByIdsAsync(idsArr);
            //if (entitys.Any(a => a.Status == SysDataStatusEnum.Enable
            //|| a.Status == SysDataStatusEnum.Retain) == true)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES10443));
            //}
            if (entitys != null && entitys.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            int rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                rows += await _procProcedureRepository.DeleteRangeAsync(new DeleteCommand
                {
                    Ids = idsArr,
                    DeleteOn = HymsonClock.Now(),
                    UserId = _currentUser.UserName
                });
                rows += await _jobBusinessRelationRepository.DeleteByBusinessIdRangeAsync(idsArr);
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 获取所有工序
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcedureSelectDto>> GetAllProcProcedurePDAAsync()
        {
            long site = _currentSite.SiteId ?? 123456;
            var list = await _procProcedureRepository.GetAllAsync( site);
            var dto = new List<ProcProcedureSelectDto>();
            foreach (var item in list)
            {
                dto.Add(new ProcProcedureSelectDto()
                {
                    Text = item.Name,
                    Value = item.Id,
                });
            }
            return dto;
        }
    }
}
