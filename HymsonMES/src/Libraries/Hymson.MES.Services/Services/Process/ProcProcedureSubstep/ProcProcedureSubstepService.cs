using Elastic.Clients.Elasticsearch;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using FluentValidation.Results;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.Utils.Tools;
using System.Transactions;
using Hymson.Localization.Services;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteJob;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 服务（子步骤） 
    /// </summary>
    public class ProcProcedureSubstepService : IProcProcedureSubstepService
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
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<ProcProcedureSubstepSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（子步骤）
        /// </summary>
        private readonly IProcProcedureSubstepRepository _procProcedureSubstepRepository;

        /// <summary>
        /// 配置作业表仓储
        /// </summary>
        private readonly IInteJobBusinessRelationRepository _jobBusinessRelationRepository;

        private readonly IProcProcedureSubstepRelationRepository _configSubstepRepository;
        /// <summary>
        /// 作业表仓储
        /// </summary>
        private readonly IInteJobRepository _inteJobRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcProcedureSubstepService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<ProcProcedureSubstepSaveDto> validationSaveRules,
            IProcProcedureSubstepRepository procProcedureSubstepRepository,
            IInteJobBusinessRelationRepository jobBusinessRelationRepository,
            IInteJobRepository inteJobRepository,
            IProcProcedureSubstepRelationRepository configSubstepRepository,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _procProcedureSubstepRepository = procProcedureSubstepRepository;
            _jobBusinessRelationRepository = jobBusinessRelationRepository;
            _inteJobRepository = inteJobRepository;
            _configSubstepRepository = configSubstepRepository;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ProcProcedureSubstepSaveDto param)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(param);

            param.Code = param.Code.ToUpperInvariant();
            var siteId = _currentSite.SiteId ?? 0;

            //编码唯一性校验
            var inteJobEntity = await _procProcedureSubstepRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = siteId,
                Code = param.Code
            });
            if (inteJobEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17751)).WithData("code", param.Code);
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = param.ToEntity<ProcProcedureSubstepEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = siteId;

            var validationFailures = new List<ValidationFailure>();
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
                        BusinessType = (int)InteJobBusinessTypeEnum.Substep,
                        OrderNumber = index,
                        LinkPoint = (int)item.LinkPoint,
                        JobId = item.JobId,
                        IsUse = item.IsUse,
                        Parameter = item.Parameter,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = updatedBy,
                        UpdatedBy = updatedBy
                    };
                    index++;
                    jobList.Add(job);
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("MES10107"), validationFailures);
            }

            var rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                // 保存
                rows += await _procProcedureSubstepRepository.InsertAsync(entity);
                if (param.JobList != null && param.JobList.Count > 0)
                {
                    rows += await _jobBusinessRelationRepository.InsertRangeAsync(jobList);
                }
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ProcProcedureSubstepSaveDto param)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(param);

            var substepEntity = await _procProcedureSubstepRepository.GetByIdAsync(param.Id)
                                ?? throw new BusinessException(nameof(ErrorCode.MES17752));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            var validationFailures = new List<ValidationFailure>();
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
                        BusinessId = substepEntity.Id,
                        BusinessType = (int)InteJobBusinessTypeEnum.Substep,
                        OrderNumber = index,
                        LinkPoint = (int)item.LinkPoint,
                        JobId = item.JobId,
                        IsUse = item.IsUse,
                        Parameter = item.Parameter,
                        Remark = "",
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = updatedBy,
                        UpdatedBy = updatedBy
                    };
                    index++;
                    jobList.Add(job);
                }
            }

            if (validationFailures.Any())
            {
                throw new ValidationException(_localizationService.GetResource("MES10107"), validationFailures);
            }

            substepEntity.Remark = param.Remark ?? "";
            substepEntity.Type = param.Type;
            substepEntity.UpdatedBy = updatedBy;
            substepEntity.UpdatedOn = updatedOn;

            var rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                // 保存
                rows += await _procProcedureSubstepRepository.UpdateAsync(substepEntity);

                //删除之前的数据
                await _jobBusinessRelationRepository.DeleteByBusinessIdAsync(param.Id);
                if (param.JobList != null && param.JobList.Count > 0)
                {
                    rows += await _jobBusinessRelationRepository.InsertRangeAsync(jobList);
                }
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _procProcedureSubstepRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            if (ids.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            //查询是否被工序关联，关联了就不能删除
            var substepRelationEntities =await _configSubstepRepository.GetEntitiesAsync(new ProcProcedureSubstepRelationQuery
            {
                ProcedureSubstepIds = ids
            });
            if (substepRelationEntities != null && substepRelationEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17753));
            }

            return await _procProcedureSubstepRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcProcedureSubstepDto?> QueryByIdAsync(long id)
        {
            var procProcedureSubstepEntity = await _procProcedureSubstepRepository.GetByIdAsync(id);
            if (procProcedureSubstepEntity == null) return null;

            return procProcedureSubstepEntity.ToModel<ProcProcedureSubstepDto>();
        }

        /// <summary>
        /// 获取子步骤配置Job信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcedureJobReleationDto>> GetSubstepConfigJobListAsync(InteJobBusinessRelationPagedQueryDto queryDto)
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
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureSubstepDto>> GetPagedListAsync(ProcProcedureSubstepPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ProcProcedureSubstepPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procProcedureSubstepRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ProcProcedureSubstepDto>());
            return new PagedInfo<ProcProcedureSubstepDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
