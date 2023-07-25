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
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
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
        private readonly AbstractValidator<ProcSortingRuleCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcSortingRuleModifyDto> _validationModifyRules;

        public ProcSortingRuleService(ICurrentUser currentUser, ICurrentSite currentSite, IProcSortingRuleRepository procSortingRuleRepository, AbstractValidator<ProcSortingRuleCreateDto> validationCreateRules, AbstractValidator<ProcSortingRuleModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procSortingRuleRepository = procSortingRuleRepository;
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
            procSortingRuleEntity.Id= IdGenProvider.Instance.CreateId();
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
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcSortingRuleDto> PrepareProcSortingRuleDtos(PagedInfo<ProcSortingRuleEntity>   pagedInfo)
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
        /// <param name="procSortingRuleDto"></param>
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
    }
}
