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
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;

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
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

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

            //入库
            await _procSortingRuleRepository.InsertAsync(procSortingRuleEntity);
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
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procSortingRuleModifyDto);

            //DTO转换实体
            var procSortingRuleEntity = procSortingRuleModifyDto.ToEntity<ProcSortingRuleEntity>();
            procSortingRuleEntity.UpdatedBy = _currentUser.UserName;
            procSortingRuleEntity.UpdatedOn = HymsonClock.Now();

            await _procSortingRuleRepository.UpdateAsync(procSortingRuleEntity);
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
        /// 读取分选规则列表信息
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
            return ruleDetailViewDtos;
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
