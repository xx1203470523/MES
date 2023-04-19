/*
 *creator: Karl
 *
 *describe: 编码规则组成    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-03-17 05:02:19
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
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 编码规则组成 服务
    /// </summary>
    public class InteCodeRulesMakeService : IInteCodeRulesMakeService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 编码规则组成 仓储
        /// </summary>
        private readonly IInteCodeRulesMakeRepository _inteCodeRulesMakeRepository;
        private readonly AbstractValidator<InteCodeRulesMakeCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteCodeRulesMakeModifyDto> _validationModifyRules;

        public InteCodeRulesMakeService(ICurrentUser currentUser, ICurrentSite currentSite, IInteCodeRulesMakeRepository inteCodeRulesMakeRepository, AbstractValidator<InteCodeRulesMakeCreateDto> validationCreateRules, AbstractValidator<InteCodeRulesMakeModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;

            _inteCodeRulesMakeRepository = inteCodeRulesMakeRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="inteCodeRulesMakeDto"></param>
        /// <returns></returns>
        public async Task CreateInteCodeRulesMakeAsync(InteCodeRulesMakeCreateDto inteCodeRulesMakeCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(inteCodeRulesMakeCreateDto);

            //DTO转换实体
            var inteCodeRulesMakeEntity = inteCodeRulesMakeCreateDto.ToEntity<InteCodeRulesMakeEntity>();
            inteCodeRulesMakeEntity.Id= IdGenProvider.Instance.CreateId();
            inteCodeRulesMakeEntity.CreatedBy = _currentUser.UserName;
            inteCodeRulesMakeEntity.UpdatedBy = _currentUser.UserName;
            inteCodeRulesMakeEntity.CreatedOn = HymsonClock.Now();
            inteCodeRulesMakeEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _inteCodeRulesMakeRepository.InsertAsync(inteCodeRulesMakeEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteInteCodeRulesMakeAsync(long id)
        {
            await _inteCodeRulesMakeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteCodeRulesMakeAsync(string ids)
        {
            var idsArr = ids.ToSpitLongArray();
            return await _inteCodeRulesMakeRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="inteCodeRulesMakePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCodeRulesMakeDto>> GetPageListAsync(InteCodeRulesMakePagedQueryDto inteCodeRulesMakePagedQueryDto)
        {
            var inteCodeRulesMakePagedQuery = inteCodeRulesMakePagedQueryDto.ToQuery<InteCodeRulesMakePagedQuery>();
            var pagedInfo = await _inteCodeRulesMakeRepository.GetPagedInfoAsync(inteCodeRulesMakePagedQuery);

            //实体到DTO转换 装载数据
            List<InteCodeRulesMakeDto> inteCodeRulesMakeDtos = PrepareInteCodeRulesMakeDtos(pagedInfo);
            return new PagedInfo<InteCodeRulesMakeDto>(inteCodeRulesMakeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteCodeRulesMakeDto> PrepareInteCodeRulesMakeDtos(PagedInfo<InteCodeRulesMakeEntity>   pagedInfo)
        {
            var inteCodeRulesMakeDtos = new List<InteCodeRulesMakeDto>();
            foreach (var inteCodeRulesMakeEntity in pagedInfo.Data)
            {
                var inteCodeRulesMakeDto = inteCodeRulesMakeEntity.ToModel<InteCodeRulesMakeDto>();
                inteCodeRulesMakeDtos.Add(inteCodeRulesMakeDto);
            }

            return inteCodeRulesMakeDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteCodeRulesMakeDto"></param>
        /// <returns></returns>
        public async Task ModifyInteCodeRulesMakeAsync(InteCodeRulesMakeModifyDto inteCodeRulesMakeModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(inteCodeRulesMakeModifyDto);

            //DTO转换实体
            var inteCodeRulesMakeEntity = inteCodeRulesMakeModifyDto.ToEntity<InteCodeRulesMakeEntity>();
            inteCodeRulesMakeEntity.UpdatedBy = _currentUser.UserName;
            inteCodeRulesMakeEntity.UpdatedOn = HymsonClock.Now();

            await _inteCodeRulesMakeRepository.UpdateAsync(inteCodeRulesMakeEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteCodeRulesMakeDto> QueryInteCodeRulesMakeByIdAsync(long id) 
        {
           var inteCodeRulesMakeEntity = await _inteCodeRulesMakeRepository.GetByIdAsync(id);
           if (inteCodeRulesMakeEntity != null) 
           {
               return inteCodeRulesMakeEntity.ToModel<InteCodeRulesMakeDto>();
           }
            return null;
        }
    }
}
