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
using Hymson.MES.CoreServices.Services.Parameter;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Parameter;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.SqlActuator.Services;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

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
        /// 多语言服务
        /// </summary>
        private readonly ILocalizationService _localizationService;
        /// <summary>
        /// sql执行器
        /// </summary>
        private readonly ISqlExecuteTaskService _sqlExecuteTaskService;
        private readonly IManuProductParameterService _manuProductParameterService;

        private readonly AbstractValidator<ProcProcedureCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcProcedureModifyDto> _validationModifyRules;

        /// <summary>
        /// 仓储接口（工序复投设置）
        /// </summary>
        private readonly IProcProcedureRejudgeRepository _procProcedureRejudgeRepository;

        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        /// <summary>
        /// 参数收集仓储
        /// </summary>
        private readonly IManuProductParameterRepository _manuProductParameterRepository;

        /// <summary>
        /// 仓储接口（资质认证）
        /// </summary>
        private readonly IInteQualificationAuthenticationRepository _inteQualificationAuthenticationRepository;

        private readonly IProcProcedureQualificationAuthenticationRelationRepository _authenticationRelationRepository;
        private readonly IProcProcedureSubstepRelationRepository _configSubstepRepository;
        private readonly IProcProcedureSubstepRepository _procedureSubstepRepository;

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
            IManuProductParameterService manuProductParameterService,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IProcProcedureRejudgeRepository procProcedureRejudgeRepository,
            AbstractValidator<ProcProcedureCreateDto> validationCreateRules,
            AbstractValidator<ProcProcedureModifyDto> validationModifyRules, ILocalizationService localizationService, ISqlExecuteTaskService sqlExecuteTaskService, IManuProductParameterRepository manuProductParameterRepository,
            IInteQualificationAuthenticationRepository inteQualificationAuthenticationRepository,
            IProcProcedureQualificationAuthenticationRelationRepository authenticationRelationRepository,
            IProcProcedureSubstepRelationRepository configSubstepRepository,
            IProcProcedureSubstepRepository procedureSubstepRepository)
        {
            _manuProductParameterRepository = manuProductParameterRepository;
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
            _manuProductParameterService = manuProductParameterService;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _localizationService = localizationService;
            _sqlExecuteTaskService = sqlExecuteTaskService;
            _procProcedureRejudgeRepository = procProcedureRejudgeRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _authenticationRelationRepository = authenticationRelationRepository;
            _inteQualificationAuthenticationRepository = inteQualificationAuthenticationRepository;
            _configSubstepRepository = configSubstepRepository;
            _procedureSubstepRepository=procedureSubstepRepository;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procProcedurePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureViewDto>> GetPageListAsync(ProcProcedurePagedQueryDto procProcedurePagedQueryDto)
        {
            var procProcedurePagedQuery = procProcedurePagedQueryDto.ToQuery<ProcProcedurePagedQuery>();
            procProcedurePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procProcedureRepository.GetPagedInfoAsync(procProcedurePagedQuery);

            //实体到DTO转换 装载数据
            List<ProcProcedureViewDto> procProcedureDtos = PrepareProcProcedureDtos(pagedInfo);
            return new PagedInfo<ProcProcedureViewDto>(procProcedureDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 分页查询工艺路线的工序列表
        /// </summary>
        /// <param name="procProcedurePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureDto>> GetPagedInfoByProcessRouteIdAsync(ProcProcedurePagedQueryDto procProcedurePagedQueryDto)
        {
            var procProcedurePagedQuery = procProcedurePagedQueryDto.ToQuery<ProcProcedurePagedQuery>();
            procProcedurePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procProcedureRepository.GetPagedInfoByProcessRouteIdAsync(procProcedurePagedQuery);

            //实体到DTO转换 装载数据
            var procProcedureDtos = new List<ProcProcedureDto>();
            foreach (var procProcedureEntity in pagedInfo.Data)
            {
                var procProcedureDto = procProcedureEntity.ToModel<ProcProcedureDto>();
                procProcedureDtos.Add(procProcedureDto);
            }
            return new PagedInfo<ProcProcedureDto>(procProcedureDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
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
                if (procProcedureEntity.IsRejudge == TrueOrFalseEnum.Yes)
                {
                    queryProcDto.Procedure.IsRejudges = true;
                }
                else
                {
                    queryProcDto.Procedure.IsRejudges = false;
                }
                if (procProcedureEntity.IsValidNGCode == TrueOrFalseEnum.Yes)
                {
                    queryProcDto.Procedure.IsValidNGCodes = true;
                }
                else
                {
                    queryProcDto.Procedure.IsValidNGCodes = false;
                }
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
                var procProceduresList = await _procProcedureRejudgeRepository.GetEntitiesAsync(new Data.Repositories.Common.Query.EntityByParentIdQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    ParentId = id
                });
                if (procProceduresList != null && procProceduresList.Count() > 0)
                {
                    var markProcProcedure = procProceduresList.Where(x => x.ProcedureId == id && x.DefectType == RejudgeUnqualifiedCodeEnum.Mark).FirstOrDefault();
                    if (markProcProcedure != null)
                    {
                        var markQualUnqualifiedCode = await _qualUnqualifiedCodeRepository.GetByIdAsync(markProcProcedure.UnqualifiedCodeId);
                        queryProcDto.MarkQualUnqualifiedCode = new Dtos.Quality.QualUnqualifiedCodeDto
                        {
                            Id = markQualUnqualifiedCode.Id,
                            UnqualifiedCode = markQualUnqualifiedCode.UnqualifiedCode,
                            UnqualifiedCodeName = markQualUnqualifiedCode.UnqualifiedCodeName
                        };
                    }

                    var lastProcProcedure = procProceduresList.Where(x => x.ProcedureId == id && x.DefectType == RejudgeUnqualifiedCodeEnum.Last).FirstOrDefault();
                    if (lastProcProcedure != null)
                    {
                        var lastQualUnqualifiedCode = await _qualUnqualifiedCodeRepository.GetByIdAsync(lastProcProcedure.UnqualifiedCodeId);
                        queryProcDto.LastQualUnqualifiedCode = new Dtos.Quality.QualUnqualifiedCodeDto
                        {
                            Id = lastQualUnqualifiedCode.Id,
                            UnqualifiedCode = lastQualUnqualifiedCode.UnqualifiedCode,
                            UnqualifiedCodeName = lastQualUnqualifiedCode.UnqualifiedCodeName
                        };
                    }
                    List<QualUnqualifiedCodeDto> qualUnqualifiedsList = new List<QualUnqualifiedCodeDto>();
                    var blockProcProcedure = procProceduresList.Where(x => x.ProcedureId == id && x.DefectType == RejudgeUnqualifiedCodeEnum.Block).ToList();
                    if (blockProcProcedure != null)
                    {
                        foreach (var item in blockProcProcedure)
                        {
                            var blockQualUnqualifiedCode = await _qualUnqualifiedCodeRepository.GetByIdAsync(item.UnqualifiedCodeId);
                            QualUnqualifiedCodeDto qualUnqualified = new QualUnqualifiedCodeDto();
                            qualUnqualified.Id = blockQualUnqualifiedCode.Id;
                            qualUnqualified.UnqualifiedCode = blockQualUnqualifiedCode.UnqualifiedCode;
                            qualUnqualified.UnqualifiedCodeName = blockQualUnqualifiedCode.UnqualifiedCodeName;
                            qualUnqualifiedsList.Add(qualUnqualified);
                        }
                        queryProcDto.BlockQualUnqualifiedCode = qualUnqualifiedsList;
                    }
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
                SiteId = _currentSite.SiteId ?? 0,
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
                var materialIds = pagedInfo.Data.Select(a => a.MaterialId).ToArray();
                var materialLsit = await _procMaterialRepository.GetByIdsAsync(materialIds);

                var templateIds = pagedInfo.Data.Select(a => a.TemplateId).ToArray();
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
                SiteId = _currentSite.SiteId ?? 0,
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
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcQualificationAuthenticationDto>> GetProcedureAuthSetListAsync(long procedureId)
        {
            var query = new ProcProcedureQualificationAuthenticationRelationQuery()
            {
                ProcedureId = procedureId
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
        /// 获取子步骤设置
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcResourceConfigSubstepDto>> GetProcedureSubStepListAsync(long procedureId)
        {
            var query = new ProcProcedureSubstepRelationQuery()
            {
                ProcedureId = procedureId
            };
            var relationEntities = await _configSubstepRepository.GetEntitiesAsync(query);

            //实体到DTO转换 装载数据
            List<ProcResourceConfigSubstepDto> authenticationDtos = new List<ProcResourceConfigSubstepDto>();
            if (relationEntities != null && relationEntities.Any())
            {
                IEnumerable<ProcProcedureSubstepEntity> substepEntities = new List<ProcProcedureSubstepEntity>();
                var substepIds = relationEntities.Select(a => a.ProcedureSubstepId).ToArray();
                if (substepIds.Any())
                {
                    substepEntities = await _procedureSubstepRepository.GetByIdsAsync(substepIds);
                }

                foreach (var entity in relationEntities)
                {
                    var substepEntity = substepEntities.FirstOrDefault(x => x.Id == entity.ProcedureSubstepId);
                    authenticationDtos.Add(new ProcResourceConfigSubstepDto()
                    {
                        ProcedureSubstepId = entity.ProcedureSubstepId,
                        Code = substepEntity?.Code ?? "",
                        Name = substepEntity?.Name ?? ""
                    });
                }
            }

            return authenticationDtos;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procProcedureCreateDto"></param>
        /// <returns></returns>
        public async Task AddProcProcedureAsync(AddProcProcedureDto procProcedureCreateDto)
        {
            #region 验证
            if (procProcedureCreateDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            if (procProcedureCreateDto.Procedure == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            var siteId = _currentSite.SiteId ?? 0;
            var userName = _currentUser.UserName;
            procProcedureCreateDto.Procedure.Code = procProcedureCreateDto.Procedure.Code.ToTrimSpace().ToUpperInvariant();
            procProcedureCreateDto.Procedure.Name = procProcedureCreateDto.Procedure.Name.Trim();
            procProcedureCreateDto.Procedure.Remark = procProcedureCreateDto.Procedure.Remark.Trim();
            if (procProcedureCreateDto.Procedure.IsRejudges)
            {
                procProcedureCreateDto.Procedure.IsRejudge = TrueOrFalseEnum.Yes;
            }
            else
            {
                procProcedureCreateDto.Procedure.IsRejudge = TrueOrFalseEnum.No;
            }
            if (procProcedureCreateDto.Procedure.IsValidNGCodes)
            {
                procProcedureCreateDto.Procedure.IsValidNGCode = TrueOrFalseEnum.Yes;
            }
            else
            {
                procProcedureCreateDto.Procedure.IsValidNGCode = TrueOrFalseEnum.No;
            }
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procProcedureCreateDto.Procedure);

            var code = procProcedureCreateDto.Procedure.Code;
            var query = new ProcProcedureQuery
            {
                SiteId = siteId,
                Code = code
            };
            if (await _procProcedureRepository.IsExistsAsync(query))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10405)).WithData("Code", procProcedureCreateDto.Procedure.Code);
            }
            //资源类型验证 
            if (procProcedureCreateDto.Procedure.ResourceTypeId.HasValue && procProcedureCreateDto.Procedure.ResourceTypeId > 0)
            {
                var procProcedures = await _procProcedureRepository.GetEntitiesAsync(new ProcProcedureQuery { SiteId = siteId, ResourceTypeId = procProcedureCreateDto.Procedure.ResourceTypeId });
                if (procProcedures != null && procProcedures.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10413)).WithData("Code", procProcedures.FirstOrDefault()?.Code ?? "");
                }
            }

            //判断资质是否重复配置  数据库中 已经存储的情况
            if (procProcedureCreateDto.AuthSetList != null && procProcedureCreateDto.AuthSetList.Count > 0 && procProcedureCreateDto.AuthSetList.GroupBy(x => x.AuthenticationId).Any(g => g.Count() >= 2))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10389));
            }

            //判断子步骤是否重复配置  数据库中 已经存储的情况
            if (procProcedureCreateDto.SubstepList != null && procProcedureCreateDto.SubstepList.Count > 0 && procProcedureCreateDto.SubstepList.GroupBy(x => x.ProcedureSubstepId).Any(g => g.Count() >= 2))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10357));
            }
            #endregion

            //DTO转换实体
            var procProcedureEntity = procProcedureCreateDto.Procedure.ToEntity<ProcProcedureEntity>();
            procProcedureEntity.Id = IdGenProvider.Instance.CreateId();
            procProcedureEntity.Code = code;
            procProcedureEntity.SiteId = siteId;
            procProcedureEntity.CreatedBy = userName;
            procProcedureEntity.UpdatedBy = userName;
            procProcedureEntity.Status = SysDataStatusEnum.Build;

            //工序复判设置
            List<ProcProcedureRejudgeEntity> procProcedureRejudgeList = new List<ProcProcedureRejudgeEntity>();
            var firstUndesirableList = procProcedureCreateDto.Procedure.FirstUndesirableId;
            if (firstUndesirableList != null && firstUndesirableList.Count > 0)
            {
                foreach (var item in firstUndesirableList)
                {
                    ProcProcedureRejudgeEntity procedureRejudgeEntity = new ProcProcedureRejudgeEntity();
                    procedureRejudgeEntity.ProcedureId = procProcedureEntity.Id;
                    procedureRejudgeEntity.Id = IdGenProvider.Instance.CreateId();
                    procedureRejudgeEntity.UnqualifiedCodeId = item.Id;
                    procedureRejudgeEntity.SiteId = siteId;
                    procedureRejudgeEntity.DefectType = RejudgeUnqualifiedCodeEnum.Block;
                    procedureRejudgeEntity.CreatedBy = userName;
                    procedureRejudgeEntity.UpdatedBy = userName;
                    procProcedureRejudgeList.Add(procedureRejudgeEntity);
                }
            }
            if (procProcedureCreateDto.Procedure.MarkId.HasValue && procProcedureCreateDto.Procedure.MarkId > 0)
            {
                ProcProcedureRejudgeEntity procedureRejudgeEntity = new ProcProcedureRejudgeEntity();
                procedureRejudgeEntity.ProcedureId = procProcedureEntity.Id;
                procedureRejudgeEntity.Id = IdGenProvider.Instance.CreateId();
                procedureRejudgeEntity.UnqualifiedCodeId = procProcedureCreateDto.Procedure.MarkId.Value;
                procedureRejudgeEntity.SiteId = siteId;
                procedureRejudgeEntity.DefectType = RejudgeUnqualifiedCodeEnum.Mark;
                procedureRejudgeEntity.CreatedBy = userName;
                procedureRejudgeEntity.UpdatedBy = userName;
                procProcedureRejudgeList.Add(procedureRejudgeEntity);
            }
            if (procProcedureCreateDto.Procedure.DefectId.HasValue && procProcedureCreateDto.Procedure.DefectId > 0)
            {
                ProcProcedureRejudgeEntity procedureRejudgeEntity = new ProcProcedureRejudgeEntity();
                procedureRejudgeEntity.ProcedureId = procProcedureEntity.Id;
                procedureRejudgeEntity.Id = IdGenProvider.Instance.CreateId();
                procedureRejudgeEntity.UnqualifiedCodeId = procProcedureCreateDto.Procedure.DefectId.Value;
                procedureRejudgeEntity.SiteId = siteId;
                procedureRejudgeEntity.DefectType = RejudgeUnqualifiedCodeEnum.Last;
                procedureRejudgeEntity.CreatedBy = userName;
                procedureRejudgeEntity.UpdatedBy = userName;
                procProcedureRejudgeList.Add(procedureRejudgeEntity);
            }
            var validationFailures = new List<ValidationFailure>();
            //打印
            List<ProcProcedurePrintRelationEntity> procedureConfigPrintList = new List<ProcProcedurePrintRelationEntity>();
            if (procProcedureCreateDto.ProcedurePrintList != null && procProcedureCreateDto.ProcedurePrintList.Count > 0)
            {
                int i = 0;
                foreach (var item in procProcedureCreateDto.ProcedurePrintList)
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

                    var releationEntity = item.ToEntity<ProcProcedurePrintRelationEntity>();
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
            if (procProcedureCreateDto.ProcedureJobList != null && procProcedureCreateDto.ProcedureJobList.Count > 0)
            {
                int index = 1;
                int i = 0;
                foreach (var item in procProcedureCreateDto.ProcedureJobList)
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
                        OrderNumber = index,
                        LinkPoint = (int)item.LinkPoint,
                        JobId = item.JobId,
                        IsUse = item.IsUse,
                        Parameter = item.Parameter,
                        Remark = "",
                        SiteId = siteId,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    index++;
                    procedureConfigJobList.Add(relationEntity);
                }
            }

            //资质认证设置数据
            var authenticationRelationEntities = new List<ProcProcedureQualificationAuthenticationRelationEntity>();
            if (procProcedureCreateDto.AuthSetList != null && procProcedureCreateDto.AuthSetList.Count > 0)
            {
                foreach (var item in procProcedureCreateDto.AuthSetList)
                {
                    authenticationRelationEntities.Add(new ProcProcedureQualificationAuthenticationRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ProcedureId = procProcedureEntity.Id,
                        IsEnable = item.IsEnable,
                        QualificationAuthenticationId = item.AuthenticationId
                    });
                }
            }

            //子步骤设置数据
            var configSubstepEntities = new List<ProcProcedureSubstepRelationEntity>();
            if (procProcedureCreateDto.SubstepList != null && procProcedureCreateDto.SubstepList.Count > 0)
            {
                foreach (var item in procProcedureCreateDto.SubstepList)
                {
                    configSubstepEntities.Add(new ProcProcedureSubstepRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ProcedureId = procProcedureEntity.Id,
                        ProcedureSubstepId = item.ProcedureSubstepId
                    });
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource(nameof(ErrorCode.MES10107)), validationFailures);
            }

            //productSet
            List<ProcProductSetEntity> productSetList = new List<ProcProductSetEntity>();
            if (procProcedureCreateDto.ProductSetList != null && procProcedureCreateDto.ProductSetList.Count > 0)
            {
                foreach (var item in procProcedureCreateDto.ProductSetList)
                {
                    var relationEntity = new ProcProductSetEntity();
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
            var createProductParameterProcedureCodeTableSql = _manuProductParameterRepository.PrepareProductParameterProcedureIdTableSql(siteId, procProcedureEntity.Id);

            using (TransactionScope ts = TransactionHelper.GetTransactionScope(TransactionScopeOption.Required, IsolationLevel.ReadCommitted))
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

                if (procProcedureRejudgeList != null && procProcedureRejudgeList.Count > 0)
                {
                    await _procProcedureRejudgeRepository.InsertRangeAsync(procProcedureRejudgeList);
                }

                if (authenticationRelationEntities.Any())
                {
                    await _authenticationRelationRepository.InsertRangeAsync(authenticationRelationEntities);
                }

                if (configSubstepEntities.Any())
                {
                    await _configSubstepRepository.InsertRangeAsync(configSubstepEntities);
                }

                await _sqlExecuteTaskService.AddTaskAsync(DbName.MES_MASTER_PARAMETER, createProductParameterProcedureCodeTableSql, userName);
                //提交
                ts.Complete();
            }

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procProcedureModifyDto"></param>
        /// <returns></returns>
        public async Task UpdateProcProcedureAsync(UpdateProcProcedureDto procProcedureModifyDto)
        {
            #region
            if (procProcedureModifyDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }
            var siteId = _currentSite.SiteId ?? 0;
            var userName = _currentUser.UserName;

            procProcedureModifyDto.Procedure.Name = procProcedureModifyDto.Procedure.Name.Trim();
            procProcedureModifyDto.Procedure.Remark = procProcedureModifyDto.Procedure.Remark.Trim();
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procProcedureModifyDto.Procedure);

            var procProcedureEntityOld = await _procProcedureRepository.GetByIdAsync(procProcedureModifyDto.Procedure.Id) ?? throw new CustomerValidationException(nameof(ErrorCode.MES10406));
            //验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == procProcedureEntityOld.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            //资源类型验证  
            if (procProcedureModifyDto.Procedure.ResourceTypeId.HasValue && procProcedureModifyDto.Procedure.ResourceTypeId > 0)
            {
                var procProcedures = await _procProcedureRepository.GetEntitiesAsync(new ProcProcedureQuery { SiteId = siteId, ResourceTypeId = procProcedureModifyDto.Procedure.ResourceTypeId });
                if (procProcedures != null && procProcedures.Any(it => it.Id != procProcedureModifyDto.Procedure.Id))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10413)).WithData("Code", procProcedures.Where(it => it.Id != procProcedureModifyDto.Procedure.Id).FirstOrDefault()?.Code ?? "");
                }
            }

            //判断资质是否重复配置  数据库中 已经存储的情况
            if (procProcedureModifyDto.AuthSetList != null && procProcedureModifyDto.AuthSetList.Count > 0 && procProcedureModifyDto.AuthSetList.GroupBy(x => x.AuthenticationId).Any(g => g.Count() >= 2))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10389));
            }

            //判断子步骤是否重复配置  数据库中 已经存储的情况
            if (procProcedureModifyDto.SubstepList != null && procProcedureModifyDto.SubstepList.Count > 0 && procProcedureModifyDto.SubstepList.GroupBy(x => x.ProcedureSubstepId).Any(g => g.Count() >= 2))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10357));
            }
            #endregion

            if (procProcedureModifyDto.Procedure.IsRejudges)
            {
                procProcedureModifyDto.Procedure.IsRejudge = TrueOrFalseEnum.Yes;
            }
            else
            {
                procProcedureModifyDto.Procedure.IsRejudge = TrueOrFalseEnum.No;
            }
            if (procProcedureModifyDto.Procedure.IsValidNGCodes)
            {
                procProcedureModifyDto.Procedure.IsValidNGCode = TrueOrFalseEnum.Yes;
            }
            else
            {
                procProcedureModifyDto.Procedure.IsValidNGCode = TrueOrFalseEnum.No;
            }

            //DTO转换实体
            var procProcedureEntity = procProcedureModifyDto.Procedure.ToEntity<ProcProcedureEntity>();
            procProcedureEntity.UpdatedBy = _currentUser.UserName;

            //TODO 现在关联表批量删除批量新增，后面再修改
            //打印

            var validationFailures = new List<ValidationFailure>();
            List<ProcProcedurePrintRelationEntity> procedureConfigPrintList = new List<ProcProcedurePrintRelationEntity>();
            if (procProcedureModifyDto.ProcedurePrintList != null && procProcedureModifyDto.ProcedurePrintList.Count > 0)
            {
                int i = 0;
                foreach (var item in procProcedureModifyDto.ProcedurePrintList)
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
                    var releationEntity = item.ToEntity<ProcProcedurePrintRelationEntity>();
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
            if (procProcedureModifyDto.ProcedureJobList != null && procProcedureModifyDto.ProcedureJobList.Count > 0)
            {
                int index = 1;
                int i = 0;
                foreach (var item in procProcedureModifyDto.ProcedureJobList)
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
                        OrderNumber = index,
                        LinkPoint = (int)item.LinkPoint,
                        JobId = item.JobId,
                        IsUse = item.IsUse,
                        Parameter = item.Parameter,
                        Remark = "",
                        SiteId = siteId,
                        CreatedBy = userName,
                        UpdatedBy = userName
                    };
                    index++;
                    procedureConfigJobList.Add(relationEntity);
                }
            }
            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("MES10107"), validationFailures);
            }
            //productSet
            List<ProcProductSetEntity> productSetList = new List<ProcProductSetEntity>();
            if (procProcedureModifyDto.ProductSetList != null && procProcedureModifyDto.ProductSetList.Count > 0)
            {
                foreach (var item in procProcedureModifyDto.ProductSetList)
                {
                    var relationEntity = new ProcProductSetEntity();
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

            List<RejudgeUnqualifiedCodeEnum> ids = new List<RejudgeUnqualifiedCodeEnum>();
            //工序复判设置
            List<ProcProcedureRejudgeEntity> procProcedureRejudgeList = new List<ProcProcedureRejudgeEntity>();
            var firstUndesirableList = procProcedureModifyDto.Procedure.FirstUndesirableId;
            if (firstUndesirableList != null && firstUndesirableList.Count > 0)
            {
                foreach (var item in firstUndesirableList)
                {
                    ProcProcedureRejudgeEntity procedureRejudgeEntity = new ProcProcedureRejudgeEntity();
                    procedureRejudgeEntity.ProcedureId = procProcedureEntity.Id;
                    procedureRejudgeEntity.Id = IdGenProvider.Instance.CreateId();
                    procedureRejudgeEntity.UnqualifiedCodeId = item.Id;
                    procedureRejudgeEntity.SiteId = siteId;
                    procedureRejudgeEntity.DefectType = RejudgeUnqualifiedCodeEnum.Block;
                    procedureRejudgeEntity.CreatedBy = userName;
                    procedureRejudgeEntity.UpdatedBy = userName;
                    procProcedureRejudgeList.Add(procedureRejudgeEntity);
                }
                ids.Add(RejudgeUnqualifiedCodeEnum.Block);
            }
            if (procProcedureModifyDto.Procedure.MarkId.HasValue && procProcedureModifyDto.Procedure.MarkId > 0)
            {
                ProcProcedureRejudgeEntity procedureRejudgeEntity = new ProcProcedureRejudgeEntity();
                procedureRejudgeEntity.ProcedureId = procProcedureEntity.Id;
                procedureRejudgeEntity.Id = IdGenProvider.Instance.CreateId();
                procedureRejudgeEntity.UnqualifiedCodeId = procProcedureModifyDto.Procedure.MarkId.Value;
                procedureRejudgeEntity.SiteId = siteId;
                procedureRejudgeEntity.DefectType = RejudgeUnqualifiedCodeEnum.Mark;
                procedureRejudgeEntity.CreatedBy = userName;
                procedureRejudgeEntity.UpdatedBy = userName;
                procProcedureRejudgeList.Add(procedureRejudgeEntity);
                ids.Add(RejudgeUnqualifiedCodeEnum.Mark);
            }
            if (procProcedureModifyDto.Procedure.DefectId.HasValue && procProcedureModifyDto.Procedure.DefectId > 0)
            {
                ProcProcedureRejudgeEntity procedureRejudgeEntity = new ProcProcedureRejudgeEntity();
                procedureRejudgeEntity.ProcedureId = procProcedureEntity.Id;
                procedureRejudgeEntity.Id = IdGenProvider.Instance.CreateId();
                procedureRejudgeEntity.UnqualifiedCodeId = procProcedureModifyDto.Procedure.DefectId.Value;
                procedureRejudgeEntity.SiteId = siteId;
                procedureRejudgeEntity.DefectType = RejudgeUnqualifiedCodeEnum.Last;
                procedureRejudgeEntity.CreatedBy = userName;
                procedureRejudgeEntity.UpdatedBy = userName;
                procProcedureRejudgeList.Add(procedureRejudgeEntity);
                ids.Add(RejudgeUnqualifiedCodeEnum.Last);
            }

            //资质认证设置数据
            var authenticationRelationEntities = new List<ProcProcedureQualificationAuthenticationRelationEntity>();
            if (procProcedureModifyDto.AuthSetList != null && procProcedureModifyDto.AuthSetList.Count > 0)
            {
                foreach (var item in procProcedureModifyDto.AuthSetList)
                {
                    authenticationRelationEntities.Add(new ProcProcedureQualificationAuthenticationRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ProcedureId = procProcedureEntity.Id,
                        IsEnable = item.IsEnable,
                        QualificationAuthenticationId = item.AuthenticationId
                    });
                }
            }

            //子步骤设置数据
            var configSubstepEntities = new List<ProcProcedureSubstepRelationEntity>();
            if (procProcedureModifyDto.SubstepList != null && procProcedureModifyDto.SubstepList.Count > 0)
            {
                foreach (var item in procProcedureModifyDto.SubstepList)
                {
                    configSubstepEntities.Add(new ProcProcedureSubstepRelationEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        ProcedureId = procProcedureEntity.Id,
                        ProcedureSubstepId = item.ProcedureSubstepId
                    });
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

                await _authenticationRelationRepository.DeleteByProcedureIdAsync(procProcedureEntity.Id);
                if (authenticationRelationEntities.Any())
                {
                    await _authenticationRelationRepository.InsertRangeAsync(authenticationRelationEntities);
                }

                await _procProcedureRejudgeRepository.DeleteByParentIdAndDefectTypeAsync(procProcedureEntity.Id, ids.ToArray());
                if (procProcedureRejudgeList != null && procProcedureRejudgeList.Count > 0)
                {
                    await _procProcedureRejudgeRepository.InsertRangeAsync(procProcedureRejudgeList);
                }

                await _configSubstepRepository.DeleteByProcedureIdsAsync(new[] { procProcedureEntity.Id });
                if (configSubstepEntities.Any())
                {
                    await _configSubstepRepository.InsertRangeAsync(configSubstepEntities);
                }
                //提交
                ts.Complete();
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcProcedureAsync(long[] idsAr)
        {
            if (idsAr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            var entitys = await _procProcedureRepository.GetByIdsAsync(idsAr);
            if (entitys != null && entitys.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            int rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                rows += await _procProcedureRepository.DeleteRangeAsync(new DeleteCommand
                {
                    Ids = idsAr,
                    DeleteOn = HymsonClock.Now(),
                    UserId = _currentUser.UserName
                });
                rows += await _configSubstepRepository.DeleteByProcedureIdsAsync(idsAr);
                rows += await _procProcedureRejudgeRepository.DeleteByParentIdAsync(idsAr);
                rows += await _jobBusinessRelationRepository.DeleteByBusinessIdRangeAsync(idsAr);
                rows += await _authenticationRelationRepository.DeleteByProcedureIdsAsync(idsAr);
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 根据工序编码获取工序信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ProcProcedureCodeDto> GetByCodeAsync(string code)
        {
            var entitys = await _procProcedureRepository.GetProcProcedureEntitiesAsync(new ProcProcedureQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Code = code
            });
            if (entitys == null || !entitys.Any())
            {
                return new ProcProcedureCodeDto();
            }

            var model = entitys.ToList()[0];
            return new ProcProcedureCodeDto
            {
                Id = model.Id,
                Code = model.Code,
                Name = model.Name
            };
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
            var entity = await _procProcedureRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10406));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
            #endregion

            #region 操作数据库
            await _procProcedureRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion

        public async Task CreateProductParameterAsync()
        {
            for (int index = 1905; index < 2048; index++)
            {
                await _manuProductParameterRepository.PrepareProductParameterSFCTable(index);
            }

            var procProcedureEntities = await _procProcedureRepository.GetEntitiesAsync(new ProcProcedureQuery { });
            foreach (var item in procProcedureEntities)
            {
                await _manuProductParameterRepository.PrepareProductParameterProcedureldTable(item.SiteId, item.Id);
            }

        }

    }
}
