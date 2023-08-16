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
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProductSet.Query;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Components;
using Minio.DataModel;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Transactions;
using static Dapper.SqlMapper;

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

        public ProcSortingRuleService(ICurrentUser currentUser, ICurrentSite currentSite,
            IProcSortingRuleRepository procSortingRuleRepository,
            IProcSortingRuleDetailRepository sortingRuleDetailRepository,
            IProcSortingRuleGradeRepository sortingRuleGradeRepository,
            IProcSortingRuleGradeDetailsRepository ruleGradeDetailsRepository,
            IProcParameterRepository procParameterRepository,
            IProcProcedureRepository procProcedureRepository,
            AbstractValidator<ProcSortingRuleCreateDto> validationCreateRules,
            AbstractValidator<ProcSortingRuleModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procSortingRuleRepository = procSortingRuleRepository;
            _sortingRuleDetailRepository = sortingRuleDetailRepository;
            _sortingRuleGradeRepository = sortingRuleGradeRepository;
            _ruleGradeDetailsRepository = ruleGradeDetailsRepository;
            _procParameterRepository = procParameterRepository;
            _procProcedureRepository = procProcedureRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procSortingRuleCreateDto"></param>
        /// <returns></returns>
        public async Task CreateProcSortingRuleAsync(ProcSortingRuleCreateDto procSortingRuleCreateDto)
        {
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

            List<ProcSortingRuleDetailEntity> procSortingRuleDetailEntities = new();
            List<ProcSortingRuleGradeEntity> procSortingRuleGradeEntities = new();
            List<ProcSortingRuleGradeDetailsEntity> procSortingRuleGradeDetailsEntities = new();

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
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                });
            }

            foreach (var item in procSortingRuleCreateDto.SortingRuleGradeDtos)
            {
                var procSortingRuleGradeEntity = new ProcSortingRuleGradeEntity()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SortingRuleId = procSortingRuleEntity.Id,
                    Grade = item.Grade,
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

            using TransactionScope trans = TransactionHelper.GetTransactionScope();
            //入库
            await _procSortingRuleRepository.InsertAsync(procSortingRuleEntity);
            await _sortingRuleDetailRepository.InsertsAsync(procSortingRuleDetailEntities);
            await _sortingRuleGradeRepository.InsertsAsync(procSortingRuleGradeEntities);
            await _ruleGradeDetailsRepository.InsertsAsync(procSortingRuleGradeDetailsEntities);

            trans.Complete();
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
            return await _procSortingRuleRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procSortingRulePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcSortingRuleDto>> GetPagedListAsync(ProcSortingRulePagedQueryDto procSortingRulePagedQueryDto)
        {
            var procSortingRulePagedQuery = procSortingRulePagedQueryDto.ToQuery<ProcSortingRulePagedQuery>();
            var pagedInfo = await _procSortingRuleRepository.GetPagedInfoAsync(procSortingRulePagedQuery);

            //实体到DTO转换 装载数据
            List<ProcSortingRuleDto> procSortingRuleDtos = PrepareProcSortingRuleDtos(pagedInfo);
            return new PagedInfo<ProcSortingRuleDto>(procSortingRuleDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcSortingRuleDto> PrepareProcSortingRuleDtos(PagedInfo<ProcSortingRuleEntity> pagedInfo)
        {
            var procSortingRuleDtos = new List<ProcSortingRuleDto>();
            foreach (var procSortingRuleEntity in pagedInfo.Data)
            {
                var procSortingRuleDto = procSortingRuleEntity.ToModel<ProcSortingRuleDto>();
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
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procSortingRuleModifyDto);

            //DTO转换实体
            var procSortingRuleEntity = await _procSortingRuleRepository.GetByIdAsync(procSortingRuleModifyDto.Id);
            if (procSortingRuleEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11309));
            }
            procSortingRuleEntity.Name = procSortingRuleModifyDto.Name;
            procSortingRuleEntity.Remark = procSortingRuleModifyDto.Remark;
            procSortingRuleEntity.UpdatedBy = _currentUser.UserName;
            procSortingRuleEntity.UpdatedOn = HymsonClock.Now();


            List<ProcSortingRuleDetailEntity> procSortingRuleDetailEntities = new();
            List<ProcSortingRuleGradeEntity> procSortingRuleGradeEntities = new();
            List<ProcSortingRuleGradeDetailsEntity> procSortingRuleGradeDetailsEntities = new();

            foreach (var item in procSortingRuleModifyDto.SortingParamDtos)
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
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now()
                });
            }

            foreach (var item in procSortingRuleModifyDto.SortingRuleGradeDtos)
            {
                var procSortingRuleGradeEntity = new ProcSortingRuleGradeEntity()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SortingRuleId = procSortingRuleEntity.Id,
                    Grade = item.Grade,
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

            using TransactionScope trans = TransactionHelper.GetTransactionScope();

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
                return procSortingRuleEntity.ToModel<ProcSortingRuleDto>();
            }
            return null;
        }

        /// <summary>
        ///获取分选规则参数信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcSortingRuleDetailViewDto>> GetSortingRuleDetailListAsync(ProcSortingRuleDetailQueryDto queryDto)
        {
            if (!queryDto.SortingRuleId.HasValue)
            {
                return new List<ProcSortingRuleDetailViewDto>();
            }

            var query = new ProcSortingRuleDetailQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                SortingRuleId = queryDto.SortingRuleId.Value
            };

            var sortingRuleDetailEntities = await _sortingRuleDetailRepository.GetProcSortingRuleDetailEntitiesAsync(query);

            //实体到DTO转换 装载数据
            List<ProcSortingRuleDetailViewDto> ruleDetailViewDtos = new List<ProcSortingRuleDetailViewDto>();
            if (sortingRuleDetailEntities != null && sortingRuleDetailEntities.Any())
            {
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
                var procedureIdList = parameterIds.Distinct().ToArray();
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
                        ProcedureId = entity.ProcedureId,
                        Code = procedureEntity?.Code ?? "",
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
            }
            return ruleDetailViewDtos;
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
                var procParameterEntities = await _procParameterRepository.GetByIdsAsync(sortingRuleDetailEntits.Select(x => x.ProcedureId).Distinct());
                var procProcedureEntities = await _procProcedureRepository.GetByIdsAsync(sortingRuleDetailEntits.Select(x => x.ProcedureId).Distinct());
                foreach (var entity in sortingRuleDetailEntits)
                {
                    var procParameter = procParameterEntities.FirstOrDefault(x => x.Id == entity.ParameterId);
                    var procedureEntity = procProcedureEntities.FirstOrDefault(x => x.Id == entity.ProcedureId);
                    list.Add(new ProcSortingRuleDetailViewDto()
                    {
                        Id = entity.Id,
                        ProcedureId = entity.ProcedureId,
                        Code = procedureEntity?.Code ?? "",
                        ParameterId = entity.ParameterId,
                        ParameterCode = procParameter?.ParameterCode ?? "",
                        ParameterName = procParameter?.ParameterName ?? "",
                        ParameterUnit = procParameter?.ParameterUnit,
                        MinValue = entity.MinValue,
                        MinContainingType = entity.MinContainingType,
                        MaxValue = entity.MaxValue,
                        MaxContainingType = entity.MaxContainingType,
                        ParameterValue = entity.ParameterValue,
                        Rating = entity.Rating,
                    });
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
                Status = Core.Enums.SysDataStatusEnum.Enable
            };
            var procSortingRules = await _procSortingRuleRepository.GetProcSortingRuleEntitiesAsync(query);

            var ruleId = procSortingRules.FirstOrDefault()?.Id ?? 0;
            var detailQueryDto = new ProcSortingRuleDetailQueryDto
            {
                SortingRuleId = ruleId
            };
            return await GetSortingRuleDetailListAsync(detailQueryDto);
        }
    }
}
