/*
 *creator: Karl
 *
 *describe: 系统Token    服务 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-15 02:09:57
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
    /// 系统Token 服务
    /// </summary>
    public class InteSystemTokenService : IInteSystemTokenService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 系统Token 仓储
        /// </summary>
        private readonly IInteSystemTokenRepository _inteSystemTokenRepository;
        private readonly AbstractValidator<InteSystemTokenCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteSystemTokenModifyDto> _validationModifyRules;

        public InteSystemTokenService(ICurrentUser currentUser, ICurrentSite currentSite, IInteSystemTokenRepository inteSystemTokenRepository, AbstractValidator<InteSystemTokenCreateDto> validationCreateRules, AbstractValidator<InteSystemTokenModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteSystemTokenRepository = inteSystemTokenRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="inteSystemTokenCreateDto"></param>
        /// <returns></returns>
        public async Task CreateInteSystemTokenAsync(InteSystemTokenCreateDto inteSystemTokenCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(inteSystemTokenCreateDto);

            //DTO转换实体
            var inteSystemTokenEntity = inteSystemTokenCreateDto.ToEntity<InteSystemTokenEntity>();
            inteSystemTokenEntity.Id= IdGenProvider.Instance.CreateId();
            inteSystemTokenEntity.CreatedBy = _currentUser.UserName;
            inteSystemTokenEntity.UpdatedBy = _currentUser.UserName;
            inteSystemTokenEntity.CreatedOn = HymsonClock.Now();
            inteSystemTokenEntity.UpdatedOn = HymsonClock.Now();
            inteSystemTokenEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _inteSystemTokenRepository.InsertAsync(inteSystemTokenEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteInteSystemTokenAsync(long id)
        {
            await _inteSystemTokenRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteSystemTokenAsync(long[] ids)
        {
            return await _inteSystemTokenRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="inteSystemTokenPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteSystemTokenDto>> GetPagedListAsync(InteSystemTokenPagedQueryDto inteSystemTokenPagedQueryDto)
        {
            var inteSystemTokenPagedQuery = inteSystemTokenPagedQueryDto.ToQuery<InteSystemTokenPagedQuery>();
            var pagedInfo = await _inteSystemTokenRepository.GetPagedInfoAsync(inteSystemTokenPagedQuery);

            //实体到DTO转换 装载数据
            List<InteSystemTokenDto> inteSystemTokenDtos = PrepareInteSystemTokenDtos(pagedInfo);
            return new PagedInfo<InteSystemTokenDto>(inteSystemTokenDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteSystemTokenDto> PrepareInteSystemTokenDtos(PagedInfo<InteSystemTokenEntity>   pagedInfo)
        {
            var inteSystemTokenDtos = new List<InteSystemTokenDto>();
            foreach (var inteSystemTokenEntity in pagedInfo.Data)
            {
                var inteSystemTokenDto = inteSystemTokenEntity.ToModel<InteSystemTokenDto>();
                inteSystemTokenDtos.Add(inteSystemTokenDto);
            }

            return inteSystemTokenDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteSystemTokenDto"></param>
        /// <returns></returns>
        public async Task ModifyInteSystemTokenAsync(InteSystemTokenModifyDto inteSystemTokenModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(inteSystemTokenModifyDto);

            //DTO转换实体
            var inteSystemTokenEntity = inteSystemTokenModifyDto.ToEntity<InteSystemTokenEntity>();
            inteSystemTokenEntity.UpdatedBy = _currentUser.UserName;
            inteSystemTokenEntity.UpdatedOn = HymsonClock.Now();

            await _inteSystemTokenRepository.UpdateAsync(inteSystemTokenEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteSystemTokenDto> QueryInteSystemTokenByIdAsync(long id) 
        {
           var inteSystemTokenEntity = await _inteSystemTokenRepository.GetByIdAsync(id);
           if (inteSystemTokenEntity != null) 
           {
               return inteSystemTokenEntity.ToModel<InteSystemTokenDto>();
           }
            return null;
        }
    }
}
