/*
 *creator: Karl
 *
 *describe: 分选规则    服务 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-07-25 03:24:54
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Org.BouncyCastle.Crypto;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 分选规则 服务
    /// </summary>
    public class ProcSortingRuleService : IProcSortingRuleService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 分选规则 仓储
        /// </summary>
        private readonly IProcSortingRuleRepository _procSortingRuleRepository;
        private readonly IProcSortingRuleDetailRepository _sortingRuleDetailRepository;
        private readonly IProcSortingRuleGradeRepository _sortingRuleGradeRepository;
        private readonly IProcSortingRuleGradeDetailsRepository _ruleGradeDetailsRepository;

        private readonly IProcMaterialRepository _procMaterialRepository;
        /// <summary>
        /// 标准参数表 仓储
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;
        /// <summary>
        /// 工序表 仓储
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        private readonly AbstractValidator<ProcSortingRuleCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcSortingRuleModifyDto> _validationModifyRules;

        private readonly ILocalizationService _localizationService;

        public ProcSortingRuleService(ICurrentUser currentUser, ICurrentSite currentSite,
            IProcSortingRuleRepository procSortingRuleRepository,
            IProcSortingRuleDetailRepository sortingRuleDetailRepository,
            IProcSortingRuleGradeRepository sortingRuleGradeRepository,
            IProcSortingRuleGradeDetailsRepository ruleGradeDetailsRepository,
            IProcParameterRepository procParameterRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcMaterialRepository procMaterialRepository,
            AbstractValidator<ProcSortingRuleCreateDto> validationCreateRules,
            AbstractValidator<ProcSortingRuleModifyDto> validationModifyRules, ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procSortingRuleRepository = procSortingRuleRepository;
            _sortingRuleDetailRepository = sortingRuleDetailRepository;
            _sortingRuleGradeRepository = sortingRuleGradeRepository;
            _ruleGradeDetailsRepository = ruleGradeDetailsRepository;
            _procParameterRepository = procParameterRepository;
            _procProcedureRepository = procProcedureRepository;
            _procMaterialRepository = procMaterialRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procSortingRuleCreateDto"></param>
        /// <returns></returns>
        public async Task<long> CreateProcSortingRuleAsync(ProcSortingRuleCreateDto procSortingRuleCreateDto)
        {
            procSortingRuleCreateDto.Code = procSortingRuleCreateDto.Code.ToTrimSpace();
            procSortingRuleCreateDto.Name = procSortingRuleCreateDto.Name.Trim();
            procSortingRuleCreateDto.Version = procSortingRuleCreateDto.Version.ToTrimSpace();
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procSortingRuleCreateDto);

            //DTO转换实体
            var procSortingRuleEntity = procSortingRuleCreateDto.ToEntity<ProcSortingRuleEntity>();

            procSortingRuleEntity.Id = IdGenProvider.Instance.CreateId();
            procSortingRuleEntity.CreatedBy = _currentUser.UserName;
            procSortingRuleEntity.UpdatedBy = _currentUser.UserName;
            procSortingRuleEntity.CreatedOn = HymsonClock.Now();
            procSortingRuleEntity.UpdatedOn = HymsonClock.Now();
            procSortingRuleEntity.SiteId = _currentSite.SiteId ?? 0;

            procSortingRuleEntity.Status = SysDataStatusEnum.Build;

            ProcSortingRuleEntity? procSortingRuleDefaultEntity = null;
            if (procSortingRuleEntity.IsDefaultVersion ?? false)
            {
                procSortingRuleDefaultEntity = await _procSortingRuleRepository.GetByDefaultVersion(new Data.Repositories.Process.ProcSortingRule.Query.ProcSortingRuleByDefaultVersionQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Code = procSortingRuleCreateDto.Code
                });
                if (procSortingRuleDefaultEntity != null)
                {
                    procSortingRuleDefaultEntity.IsDefaultVersion = false;
                    procSortingRuleDefaultEntity.UpdatedOn = HymsonClock.Now();
                    procSortingRuleDefaultEntity.UpdatedBy = _currentUser.UserName;
                }
            }

            List<ProcSortingRuleDetailEntity> procSortingRuleDetailEntities = new();
            List<ProcSortingRuleGradeEntity> procSortingRuleGradeEntities = new();
            List<ProcSortingRuleGradeDetailsEntity> procSortingRuleGradeDetailsEntities = new();

            if (procSortingRuleCreateDto.SortingParamDtos != null && procSortingRuleCreateDto.SortingParamDtos.Any())
            {
                foreach (var item in procSortingRuleCreateDto.SortingParamDtos)
                {
                    procSortingRuleDetailEntities.Add(new ProcSortingRuleDetailEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        SortingRuleId = procSortingRuleEntity.Id,
                        ProcedureId = item.ProcedureId,
                        ParameterId = item.ParameterId,
                        MinValue = item.MinValue,
                        MinContainingType = item.MinContainingType,
                        MaxValue = item.MaxValue,
                        MaxContainingType = item.MaxContainingType,
                        ParameterValue = item.ParameterValue,
                        Rating = item.Rating,
                        Serial = item.Serial,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now()
                    });
                }
            }

            if (procSortingRuleCreateDto.SortingRuleGradeDtos != null && procSortingRuleCreateDto.SortingRuleGradeDtos.Any())
            {
                foreach (var item in procSortingRuleCreateDto.SortingRuleGradeDtos)
                {
                    var procSortingRuleGradeEntity = new ProcSortingRuleGradeEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        SortingRuleId = procSortingRuleEntity.Id,
                        Grade = item.Grade,
                        Priority = item.Priority,
                        Remark = item.Remark,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now()
                    };
                    procSortingRuleGradeEntities.Add(procSortingRuleGradeEntity);

                    var procSortingRuleDetails = procSortingRuleDetailEntities.Where(x => item.Ratings.Contains(x.Rating));

                    foreach (var SortingRule in procSortingRuleDetails)
                    {
                        procSortingRuleGradeDetailsEntities.Add(new ProcSortingRuleGradeDetailsEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SortingRuleId = procSortingRuleEntity.Id,
                            SiteId = _currentSite.SiteId ?? 0,
                            SortingRuleGradeId = procSortingRuleGradeEntity.Id,
                            SortingRuleDetailId = SortingRule.Id,
                            CreatedBy = _currentUser.UserName,
                            UpdatedBy = _currentUser.UserName,
                            CreatedOn = HymsonClock.Now(),
                            UpdatedOn = HymsonClock.Now()
                        });
                    }
                }
            }

            using TransactionScope trans = TransactionHelper.GetTransactionScope();
            //入库
            if (procSortingRuleDefaultEntity != null)
            {
                await _procSortingRuleRepository.UpdateAsync(procSortingRuleDefaultEntity);
            }
            await _procSortingRuleRepository.InsertAsync(procSortingRuleEntity);
            await _sortingRuleDetailRepository.InsertsAsync(procSortingRuleDetailEntities);
            await _sortingRuleGradeRepository.InsertsAsync(procSortingRuleGradeEntities);
            await _ruleGradeDetailsRepository.InsertsAsync(procSortingRuleGradeDetailsEntities);

            trans.Complete();
            return procSortingRuleEntity.Id;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcSortingRuleAsync(long id)
        {
            await _procSortingRuleRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcSortingRuleAsync(long[] ids)
        {
            if (ids.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            var entities = await _procSortingRuleRepository.GetByIdsAsync(ids);
            if (entities != null && entities.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            return await _procSortingRuleRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procSortingRulePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcSortingRuleDto>> GetPagedListAsync(ProcSortingRulePagedQueryDto procSortingRulePagedQueryDto)
        {
            var procMaterials = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery { SiteId = _currentSite.SiteId ?? 0, MaterialCode = procSortingRulePagedQueryDto.MaterialCode, Version = procSortingRulePagedQueryDto.MaterialVersion });
            if ((procMaterials == null || !procMaterials.Any()) && (!string.IsNullOrEmpty(procSortingRulePagedQueryDto.MaterialCode) || !string.IsNullOrEmpty(procSortingRulePagedQueryDto.MaterialVersion)))
            {
                return new PagedInfo<ProcSortingRuleDto>(new List<ProcSortingRuleDto> { }, 1, 0, 0);
            }
            var procSortingRulePagedQuery = procSortingRulePagedQueryDto.ToQuery<ProcSortingRulePagedQuery>();
            procSortingRulePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            procSortingRulePagedQuery.MaterialIds = procMaterials!.Select(m => m.Id).ToList();
            var pagedInfo = await _procSortingRuleRepository.GetPagedInfoAsync(procSortingRulePagedQuery);

            //实体到DTO转换 装载数据
            List<ProcSortingRuleDto> procSortingRuleDtos = await PrepareProcSortingRuleDtos(pagedInfo);
            return new PagedInfo<ProcSortingRuleDto>(procSortingRuleDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private async Task<List<ProcSortingRuleDto>> PrepareProcSortingRuleDtos(PagedInfo<ProcSortingRuleEntity> pagedInfo)
        {
            var procSortingRuleDtos = new List<ProcSortingRuleDto>();
            var materials = await _procMaterialRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.MaterialId));
            foreach (var procSortingRuleEntity in pagedInfo.Data)
            {
                var procSortingRuleDto = procSortingRuleEntity.ToModel<ProcSortingRuleDto>();
                var material = materials.FirstOrDefault(x => x.Id == procSortingRuleEntity.MaterialId);
                if (material != null)
                {
                    procSortingRuleDto.MaterialCode = material.MaterialCode;
                    procSortingRuleDto.MaterialName = material.MaterialName;
                    procSortingRuleDto.MaterialVersion = material.Version;
                }
                procSortingRuleDtos.Add(procSortingRuleDto);
            }

            return procSortingRuleDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procSortingRuleModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcSortingRuleAsync(ProcSortingRuleModifyDto procSortingRuleModifyDto)
        {
            procSortingRuleModifyDto.Name = procSortingRuleModifyDto.Name.Trim();
            procSortingRuleModifyDto.Remark = procSortingRuleModifyDto.Remark?.Trim();
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procSortingRuleModifyDto);

            //DTO转换实体
            var procSortingRuleEntity = await _procSortingRuleRepository.GetByIdAsync(procSortingRuleModifyDto.Id);
            if (procSortingRuleEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11309));
            }
            //验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == procSortingRuleEntity.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            procSortingRuleEntity.Name = procSortingRuleModifyDto.Name.Trim();
            procSortingRuleEntity.Remark = procSortingRuleModifyDto.Remark;
            procSortingRuleEntity.UpdatedBy = _currentUser.UserName;
            procSortingRuleEntity.UpdatedOn = HymsonClock.Now();
            procSortingRuleEntity.IsDefaultVersion = procSortingRuleModifyDto.IsDefaultVersion;
            ProcSortingRuleEntity? procSortingRuleDefaultEntity = null;
            if (procSortingRuleEntity.IsDefaultVersion ?? false)
            {
                procSortingRuleDefaultEntity = await _procSortingRuleRepository.GetByDefaultVersion(new Data.Repositories.Process.ProcSortingRule.Query.ProcSortingRuleByDefaultVersionQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Code = procSortingRuleEntity.Code
                });
                if (procSortingRuleDefaultEntity != null)
                {
                    procSortingRuleDefaultEntity.IsDefaultVersion = false;
                    procSortingRuleDefaultEntity.UpdatedOn = HymsonClock.Now();
                    procSortingRuleDefaultEntity.UpdatedBy = _currentUser.UserName;
                }
            }

            List<ProcSortingRuleDetailEntity> procSortingRuleDetailEntities = new();
            List<ProcSortingRuleGradeEntity> procSortingRuleGradeEntities = new();
            List<ProcSortingRuleGradeDetailsEntity> procSortingRuleGradeDetailsEntities = new();

            if (procSortingRuleModifyDto.SortingParamDtos != null && procSortingRuleModifyDto.SortingParamDtos.Any())
            {
                foreach (var item in procSortingRuleModifyDto.SortingParamDtos)
                {
                    procSortingRuleDetailEntities.Add(new ProcSortingRuleDetailEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        Serial = item.Serial,
                        SortingRuleId = procSortingRuleEntity.Id,
                        ProcedureId = item.ProcedureId,
                        ParameterId = item.ParameterId,
                        MinValue = item.MinValue,
                        MinContainingType = item.MinContainingType,
                        MaxValue = item.MaxValue,
                        MaxContainingType = item.MaxContainingType,
                        ParameterValue = item.ParameterValue,
                        Rating = item.Rating,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now()
                    });
                }
            }

            if (procSortingRuleModifyDto.SortingRuleGradeDtos != null && procSortingRuleModifyDto.SortingRuleGradeDtos.Any())
            {
                foreach (var item in procSortingRuleModifyDto.SortingRuleGradeDtos)
                {
                    var procSortingRuleGradeEntity = new ProcSortingRuleGradeEntity()
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        SortingRuleId = procSortingRuleEntity.Id,
                        Grade = item.Grade,
                        Priority = item.Priority,
                        Remark = item.Remark,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now()
                    };
                    procSortingRuleGradeEntities.Add(procSortingRuleGradeEntity);

                    var procSortingRuleDetails = procSortingRuleDetailEntities.Where(x => item.Ratings.Contains(x.Rating));

                    foreach (var SortingRule in procSortingRuleDetails)
                    {
                        procSortingRuleGradeDetailsEntities.Add(new ProcSortingRuleGradeDetailsEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SortingRuleId = procSortingRuleEntity.Id,
                            SiteId = _currentSite.SiteId ?? 0,
                            SortingRuleGradeId = procSortingRuleGradeEntity.Id,
                            SortingRuleDetailId = SortingRule.Id,
                            CreatedBy = _currentUser.UserName,
                            UpdatedBy = _currentUser.UserName,
                            CreatedOn = HymsonClock.Now(),
                            UpdatedOn = HymsonClock.Now()
                        });
                    }
                }
            }

            using TransactionScope trans = TransactionHelper.GetTransactionScope();
            if (procSortingRuleDefaultEntity != null)
            {
                await _procSortingRuleRepository.UpdateAsync(procSortingRuleDefaultEntity);
            }
            await _sortingRuleDetailRepository.DeleteSortingRuleDetailByIdAsync(procSortingRuleModifyDto.Id);
            await _sortingRuleGradeRepository.DeleteSortingRuleGradeByIdAsync(procSortingRuleModifyDto.Id);
            await _ruleGradeDetailsRepository.DeleteSortingRuleGradeByIdAsync(procSortingRuleModifyDto.Id);

            //入库
            await _procSortingRuleRepository.UpdateAsync(procSortingRuleEntity);
            await _sortingRuleDetailRepository.InsertsAsync(procSortingRuleDetailEntities);
            await _sortingRuleGradeRepository.InsertsAsync(procSortingRuleGradeEntities);
            await _ruleGradeDetailsRepository.InsertsAsync(procSortingRuleGradeDetailsEntities);

            trans.Complete();
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcSortingRuleDto> QueryProcSortingRuleByIdAsync(long id)
        {
            var procSortingRuleEntity = await _procSortingRuleRepository.GetByIdAsync(id);
            if (procSortingRuleEntity != null)
            {
                var procSortingRuleDto = procSortingRuleEntity.ToModel<ProcSortingRuleDto>();
                var material = await _procMaterialRepository.GetByIdAsync(procSortingRuleEntity.MaterialId);
                if (material != null)
                {
                    procSortingRuleDto.MaterialCode = material.MaterialCode;
                    procSortingRuleDto.MaterialName = material.MaterialName;
                    procSortingRuleDto.MaterialVersion = material.Version;
                }

                return procSortingRuleDto;
            }
            return new ProcSortingRuleDto();
        }

        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleDetailViewDto>> GetProcSortingRuleGradeRuleDetailsAsync(long id)
        {
            List<ProcSortingRuleDetailViewDto> list = new();
            var sortingRuleDetailEntits = await _sortingRuleDetailRepository.GetSortingRuleDetailByIdAsync(id);
            if (sortingRuleDetailEntits != null && sortingRuleDetailEntits.Any())
            {
                var procParameterEntities = await _procParameterRepository.GetByIdsAsync(sortingRuleDetailEntits.Select(x => x.ParameterId).Distinct());
                var procProcedureEntities = await _procProcedureRepository.GetByIdsAsync(sortingRuleDetailEntits.Select(x => x.ProcedureId).Distinct());
                foreach (var entity in sortingRuleDetailEntits)
                {
                    var procParameter = procParameterEntities.FirstOrDefault(x => x.Id == entity.ParameterId);
                    var procedureEntity = procProcedureEntities.FirstOrDefault(x => x.Id == entity.ProcedureId);
                    if (procParameter != null && procedureEntity != null)
                    {
                        list.Add(new ProcSortingRuleDetailViewDto()
                        {
                            Id = entity.Id,
                            ProcedureId = entity.ProcedureId,
                            Code = procedureEntity.Code,
                            ProcedureCode = procedureEntity.Code,
                            ProcedureName = procedureEntity.Name,
                            ParameterId = entity.ParameterId,
                            ParameterCode = procParameter.ParameterCode,
                            ParameterName = procParameter.ParameterName,
                            ParameterUnit = procParameter?.ParameterUnit??"",
                            MinValue = entity.MinValue,
                            MinContainingType = entity.MinContainingType,
                            MaxValue = entity.MaxValue,
                            MaxContainingType = entity.MaxContainingType,
                            ParameterValue = entity.ParameterValue,
                            Rating = entity.Rating,
                            Serial=entity.Serial,
                            SortingRuleId = entity.SortingRuleId

                        });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取档位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SortingRuleGradeDto>> GetProcSortingRuleGradesAsync(long id)
        {
            List<SortingRuleGradeDto> list = new();
            var sortingRuleDetailTask = _sortingRuleDetailRepository.GetSortingRuleDetailByIdAsync(id);
            var sortingRuleGradesTask = _sortingRuleGradeRepository.GetSortingRuleGradesByIdAsync(id);
            var sortingRuleGradeeDetailsTask = _ruleGradeDetailsRepository.GetSortingRuleGradeeDetailsByIdAsync(id);

            var sortingRuleDetails = await sortingRuleDetailTask;

            var sortingRuleGrades = await sortingRuleGradesTask;

            var sortingRuleGradeeDetails = await sortingRuleGradeeDetailsTask;

            if (sortingRuleDetails != null && sortingRuleDetails.Any())
            {
                foreach (var item in sortingRuleGrades)
                {
                    var gradeDetails = sortingRuleGradeeDetails.Where(x => x.SortingRuleGradeId == item.Id);
                    list.Add(new SortingRuleGradeDto
                    {
                        Grade = item.Grade,
                        Remark = item.Remark,
                        Priority = item.Priority,
                        Ratings = sortingRuleDetails.Where(o => gradeDetails.Select(x => x.SortingRuleDetailId).Contains(o.Id)).Select(k => k.Rating)
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// 根据物料读取分选规则列表信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleDetailViewDto>> GetSortingRuleDetailListByMaterialIdAsync(ProcSortingRuleDetailQueryDto queryDto)
        {
            if (!queryDto.MaterialId.HasValue)
            {
                return new List<ProcSortingRuleDetailViewDto>();
            }

            //根据物料找到分选规则
            var query = new ProcSortingRuleQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                MaterialId = queryDto.MaterialId.Value,
                Status = SysDataStatusEnum.Enable,
                IsDefaultVersion = true
            };
            var procSortingRules = await _procSortingRuleRepository.GetProcSortingRuleEntitiesAsync(query);

            var procSortingRule = procSortingRules.LastOrDefault();
            var ruleId = procSortingRule?.Id ?? 0;
            if (ruleId <= 0)
            {
                return new List<ProcSortingRuleDetailViewDto>();
            }

            var sortingRuleDetailEntities = await _sortingRuleDetailRepository.GetSortingRuleDetailByIdAsync(ruleId);

            //实体到DTO转换 装载数据
            List<ProcSortingRuleDetailViewDto> ruleDetailViewDtos = new List<ProcSortingRuleDetailViewDto>();
            if (sortingRuleDetailEntities == null || !sortingRuleDetailEntities.Any())
            {
                return new List<ProcSortingRuleDetailViewDto>();
            }
            var parameterIds = new List<long> { };
            IEnumerable<ProcParameterEntity> procParameterEntities = new List<ProcParameterEntity>();
            parameterIds.AddRange(sortingRuleDetailEntities.Select(a => a.ParameterId).ToArray());
            var parameterIdList = parameterIds.Distinct().ToArray();
            if (parameterIdList.Any())
            {
                procParameterEntities = await _procParameterRepository.GetByIdsAsync(parameterIdList);
            }

            var procedureIds = new List<long> { };
            IEnumerable<ProcProcedureEntity> procProcedureEntities = new List<ProcProcedureEntity>();
            procedureIds.AddRange(sortingRuleDetailEntities.Select(a => a.ProcedureId).ToArray());
            var procedureIdList = procedureIds.Distinct().ToArray();
            if (procedureIdList.Any())
            {
                procProcedureEntities = await _procProcedureRepository.GetByIdsAsync(procedureIdList);
            }

            foreach (var entity in sortingRuleDetailEntities)
            {
                var procParameter = procParameterEntities.FirstOrDefault(x => x.Id == entity.ParameterId);
                var procedureEntity = procProcedureEntities.FirstOrDefault(x => x.Id == entity.ProcedureId);
                ruleDetailViewDtos.Add(new ProcSortingRuleDetailViewDto()
                {
                    Id = entity.Id,
                    SortingRuleId = entity.SortingRuleId,
                    ProcedureId = entity.ProcedureId,
                    Code = procedureEntity?.Code ?? "",
                    Version = procSortingRule?.Version ?? "",
                    ParameterId = entity.ParameterId,
                    ParameterCode = procParameter?.ParameterCode ?? "",
                    ParameterName = procParameter?.ParameterName ?? "",
                    ParameterUnit = procParameter?.ParameterUnit ?? "",
                    MinValue = entity.MinValue,
                    MinContainingType = entity.MinContainingType,
                    MaxValue = entity.MaxValue,
                    MaxContainingType = entity.MaxContainingType,
                    ParameterValue = entity.ParameterValue,
                    Rating = entity.Rating,
                });
            }

            return ruleDetailViewDtos;
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
            var entity = await _procSortingRuleRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10705));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
            #endregion

            #region 操作数据库
            await _procSortingRuleRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion
    }
}
