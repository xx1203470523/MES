/*
 *creator: Karl
 *
 *describe: 系统Token    服务 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-06-15 02:09:57
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Microsoft.Extensions.Options;
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

        private readonly JwtOptions _jwtOptions;

        public InteSystemTokenService(ICurrentUser currentUser, ICurrentSite currentSite,
            IInteSystemTokenRepository inteSystemTokenRepository, IOptions<JwtOptions> jwtOptions,
            AbstractValidator<InteSystemTokenCreateDto> validationCreateRules,
            AbstractValidator<InteSystemTokenModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteSystemTokenRepository = inteSystemTokenRepository;
            _jwtOptions = jwtOptions.Value;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="inteSystemTokenPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteSystemTokenDto>> GetPagedListAsync(InteSystemTokenPagedQueryDto inteSystemTokenPagedQueryDto)
        {
            var inteSystemTokenPagedQuery = inteSystemTokenPagedQueryDto.ToQuery<InteSystemTokenPagedQuery>();
            inteSystemTokenPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _inteSystemTokenRepository.GetPagedInfoAsync(inteSystemTokenPagedQuery);

            //实体到DTO转换 装载数据
            List<InteSystemTokenDto> inteSystemTokenDtos = PrepareInteSystemTokenDtos(pagedInfo);
            return new PagedInfo<InteSystemTokenDto>(inteSystemTokenDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteSystemTokenDto> PrepareInteSystemTokenDtos(PagedInfo<InteSystemTokenEntity> pagedInfo)
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
            if (inteSystemTokenCreateDto == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(inteSystemTokenCreateDto);

            //inteSystemTokenCreateDto.SystemCode = inteSystemTokenCreateDto.SystemCode.ToUpperInvariant();
            var systemCode = inteSystemTokenCreateDto.SystemCode;
            var inteSystemTokenEntity = await _inteSystemTokenRepository.GetByCodeAsync(new InteSystemTokenQuery
            {
                SiteId = _currentSite.SiteId ?? 123456,
                SystemCode = systemCode
            });
            if (inteSystemTokenEntity != null)
            {
                throw new BusinessException(nameof(ErrorCode.MES18300)).WithData("code", systemCode);
            }

            //DTO转换实体
            inteSystemTokenEntity = inteSystemTokenCreateDto.ToEntity<InteSystemTokenEntity>();
            inteSystemTokenEntity.Id = IdGenProvider.Instance.CreateId();
            inteSystemTokenEntity.CreatedBy = _currentUser.UserName;
            inteSystemTokenEntity.UpdatedBy = _currentUser.UserName;
            inteSystemTokenEntity.SiteId = _currentSite.SiteId ?? 123456;
            //生成Token
            inteSystemTokenEntity.Token = CreateSystemTokenAsync(inteSystemTokenEntity);
            inteSystemTokenEntity.ExpirationTime = HymsonClock.Now().AddMinutes(_jwtOptions.ExpiresMinutes);

            //入库
            await _inteSystemTokenRepository.InsertAsync(inteSystemTokenEntity);
        }

        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="inteSystemToken"></param>
        /// <returns></returns>
        public string CreateSystemTokenAsync(InteSystemTokenEntity inteSystemToken)
        {
            var systemModel = new SystemModel
            {
                FactoryId = _currentSite.SiteId ?? 123456,
                Id = inteSystemToken.Id,
                Name = inteSystemToken.SystemCode,
                SiteId = _currentSite.SiteId ?? 123456,
            };
            return JwtHelper.GenerateJwtToken(systemModel, _jwtOptions);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="systemId"></param>
        /// <returns></returns>
        public async Task<string> RefreshSystemTokenAsync(long systemId)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            var inteSystemTokenEntity = await _inteSystemTokenRepository.GetByIdAsync(systemId);
            if (inteSystemTokenEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            var systemCode = inteSystemTokenEntity.SystemCode;
            var systemModel = new SystemModel
            {
                FactoryId = _currentSite.SiteId ?? 123456,
                Id = inteSystemTokenEntity.Id,
                Name = systemCode,
                SiteId = _currentSite.SiteId ?? 123456,
            };
            var token = JwtHelper.GenerateJwtToken(systemModel, _jwtOptions);
            var expirationTime = HymsonClock.Now().AddMinutes(_jwtOptions.ExpiresMinutes);

            inteSystemTokenEntity.UpdatedBy = _currentUser.UserName;
            inteSystemTokenEntity.UpdatedOn = HymsonClock.Now();
            inteSystemTokenEntity.Token = token;
            inteSystemTokenEntity.ExpirationTime = expirationTime;
            await _inteSystemTokenRepository.UpdateTokenAsync(inteSystemTokenEntity);
            return inteSystemTokenEntity.Token;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteSystemTokenModifyDto"></param>
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
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteSystemTokenAsync(long[] ids)
        {
            return await _inteSystemTokenRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }
    }
}
