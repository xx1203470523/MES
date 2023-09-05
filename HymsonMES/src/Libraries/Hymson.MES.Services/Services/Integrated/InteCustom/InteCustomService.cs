/*
 *creator: Karl
 *
 *describe: 客户维护    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-11 09:33:26
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
    /// 客户维护 服务
    /// </summary>
    public class InteCustomService : IInteCustomService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 客户维护 仓储
        /// </summary>
        private readonly IInteCustomRepository _inteCustomRepository;
        private readonly AbstractValidator<InteCustomCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteCustomModifyDto> _validationModifyRules;

        public InteCustomService(ICurrentUser currentUser, ICurrentSite currentSite, IInteCustomRepository inteCustomRepository, AbstractValidator<InteCustomCreateDto> validationCreateRules, AbstractValidator<InteCustomModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteCustomRepository = inteCustomRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="inteCustomCreateDto"></param>
        /// <returns></returns>
        public async Task CreateInteCustomAsync(InteCustomCreateDto inteCustomCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(inteCustomCreateDto);

            //DTO转换实体
            var inteCustomEntity = inteCustomCreateDto.ToEntity<InteCustomEntity>();
            inteCustomEntity.Id= IdGenProvider.Instance.CreateId();
            inteCustomEntity.CreatedBy = _currentUser.UserName;
            inteCustomEntity.UpdatedBy = _currentUser.UserName;
            inteCustomEntity.CreatedOn = HymsonClock.Now();
            inteCustomEntity.UpdatedOn = HymsonClock.Now();
            inteCustomEntity.SiteId = _currentSite.SiteId ?? 0;

            //验证是否编码唯一
            var entity= await _inteCustomRepository.GetByCodeAsync(inteCustomEntity.Code.Trim());
            if (entity != null) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18402));
            }

            //入库
            await _inteCustomRepository.InsertAsync(inteCustomEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteInteCustomAsync(long id)
        {
            await _inteCustomRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteCustomAsync(long[] ids)
        {
            return await _inteCustomRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="inteCustomPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteCustomDto>> GetPagedListAsync(InteCustomPagedQueryDto inteCustomPagedQueryDto)
        {
            var inteCustomPagedQuery = inteCustomPagedQueryDto.ToQuery<InteCustomPagedQuery>();
            inteCustomPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteCustomRepository.GetPagedInfoAsync(inteCustomPagedQuery);

            //实体到DTO转换 装载数据
            List<InteCustomDto> inteCustomDtos = PrepareInteCustomDtos(pagedInfo);
            return new PagedInfo<InteCustomDto>(inteCustomDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteCustomDto> PrepareInteCustomDtos(PagedInfo<InteCustomEntity>   pagedInfo)
        {
            var inteCustomDtos = new List<InteCustomDto>();
            foreach (var inteCustomEntity in pagedInfo.Data)
            {
                var inteCustomDto = inteCustomEntity.ToModel<InteCustomDto>();
                inteCustomDtos.Add(inteCustomDto);
            }

            return inteCustomDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteCustomModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyInteCustomAsync(InteCustomModifyDto inteCustomModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(inteCustomModifyDto);

            //DTO转换实体
            var inteCustomEntity = inteCustomModifyDto.ToEntity<InteCustomEntity>();
            inteCustomEntity.UpdatedBy = _currentUser.UserName;
            inteCustomEntity.UpdatedOn = HymsonClock.Now();

            await _inteCustomRepository.UpdateAsync(inteCustomEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteCustomDto> QueryInteCustomByIdAsync(long id) 
        {
           var inteCustomEntity = await _inteCustomRepository.GetByIdAsync(id);
           if (inteCustomEntity != null) 
           {
               return inteCustomEntity.ToModel<InteCustomDto>();
           }
           throw new CustomerValidationException(nameof(ErrorCode.MES18401));
        }
    }
}
