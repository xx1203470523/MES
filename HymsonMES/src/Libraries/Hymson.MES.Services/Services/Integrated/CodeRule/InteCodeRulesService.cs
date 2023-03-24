/*
 *creator: Karl
 *
 *describe: 编码规则    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:26
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 编码规则 服务
    /// </summary>
    public class InteCodeRulesService : IInteCodeRulesService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 编码规则 仓储
        /// </summary>
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;
        private readonly AbstractValidator<InteCodeRulesCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteCodeRulesModifyDto> _validationModifyRules;

        private readonly IProcMaterialRepository _procMaterialRepository;

        public InteCodeRulesService(ICurrentUser currentUser, ICurrentSite currentSite, IInteCodeRulesRepository inteCodeRulesRepository, AbstractValidator<InteCodeRulesCreateDto> validationCreateRules, AbstractValidator<InteCodeRulesModifyDto> validationModifyRules, IProcMaterialRepository procMaterialRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _inteCodeRulesRepository = inteCodeRulesRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;

            _procMaterialRepository = procMaterialRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="inteCodeRulesDto"></param>
        /// <returns></returns>
        public async Task CreateInteCodeRulesAsync(InteCodeRulesCreateDto inteCodeRulesCreateDto)
        {
            //// 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new BusinessException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(inteCodeRulesCreateDto);

            //DTO转换实体
            var inteCodeRulesEntity = inteCodeRulesCreateDto.ToEntity<InteCodeRulesEntity>();
            inteCodeRulesEntity.Id= IdGenProvider.Instance.CreateId();
            inteCodeRulesEntity.CreatedBy = _currentUser.UserName;
            inteCodeRulesEntity.UpdatedBy = _currentUser.UserName;
            inteCodeRulesEntity.CreatedOn = HymsonClock.Now();
            inteCodeRulesEntity.UpdatedOn = HymsonClock.Now();
            inteCodeRulesEntity.SiteId = _currentSite.SiteId ?? 0;

            //判断是否已经存在该物料数据
            var hasCodeRulesEntities= await _inteCodeRulesRepository.GetInteCodeRulesEntitiesEqualAsync(new InteCodeRulesQuery { ProductId= inteCodeRulesCreateDto.ProductId });
            if (hasCodeRulesEntities != null && hasCodeRulesEntities.Any()) 
            {
                throw new BusinessException(nameof(ErrorCode.MES12401)).WithData("productId", inteCodeRulesCreateDto.ProductId);
            }

            //入库
            await _inteCodeRulesRepository.InsertAsync(inteCodeRulesEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteInteCodeRulesAsync(long id)
        {
            await _inteCodeRulesRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteCodeRulesAsync(long[] idsArr)
        {
            return await _inteCodeRulesRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="inteCodeRulesPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCodeRulesPageViewDto>> GetPageListAsync(InteCodeRulesPagedQueryDto inteCodeRulesPagedQueryDto)
        {
            var inteCodeRulesPagedQuery = inteCodeRulesPagedQueryDto.ToQuery<InteCodeRulesPagedQuery>();
            var pagedInfo = await _inteCodeRulesRepository.GetPagedInfoAsync(inteCodeRulesPagedQuery);

            //实体到DTO转换 装载数据
            List<InteCodeRulesPageViewDto> inteCodeRulesDtos = PrepareInteCodeRulesDtos(pagedInfo);
            return new PagedInfo<InteCodeRulesPageViewDto>(inteCodeRulesDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteCodeRulesPageViewDto> PrepareInteCodeRulesDtos(PagedInfo<InteCodeRulesPageView>   pagedInfo)
        {
            var inteCodeRulesDtos = new List<InteCodeRulesPageViewDto>();
            foreach (var inteCodeRulesEntity in pagedInfo.Data)
            {
                var inteCodeRulesDto = inteCodeRulesEntity.ToModel<InteCodeRulesPageViewDto>();
                inteCodeRulesDtos.Add(inteCodeRulesDto);
            }

            return inteCodeRulesDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteCodeRulesDto"></param>
        /// <returns></returns>
        public async Task ModifyInteCodeRulesAsync(InteCodeRulesModifyDto inteCodeRulesModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(inteCodeRulesModifyDto);

            //DTO转换实体
            var inteCodeRulesEntity = inteCodeRulesModifyDto.ToEntity<InteCodeRulesEntity>();
            inteCodeRulesEntity.UpdatedBy = _currentUser.UserName;
            inteCodeRulesEntity.UpdatedOn = HymsonClock.Now();

            await _inteCodeRulesRepository.UpdateAsync(inteCodeRulesEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteCodeRulesDetailViewDto> QueryInteCodeRulesByIdAsync(long id) 
        {
           var inteCodeRulesEntity = await _inteCodeRulesRepository.GetByIdAsync(id);
           if (inteCodeRulesEntity != null) 
           {
                var inteCodeRulesDetailViewDto = inteCodeRulesEntity.ToModel<InteCodeRulesDetailViewDto>();
                //查询关联数据
                var material= await _procMaterialRepository.GetByIdAsync(inteCodeRulesEntity.ProductId, _currentSite.SiteId??0);
                if (material != null) 
                {
                    inteCodeRulesDetailViewDto.MaterialCode=material.MaterialCode;
                    inteCodeRulesDetailViewDto.MaterialName = material.MaterialName;
                    inteCodeRulesDetailViewDto.MaterialVersion = material.Version;
                }

                return inteCodeRulesDetailViewDto;
           }
            return null;
        }
    }
}
