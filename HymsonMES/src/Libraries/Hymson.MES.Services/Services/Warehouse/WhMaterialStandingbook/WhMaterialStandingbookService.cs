/*
 *creator: Karl
 *
 *describe: 物料台账    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-13 10:03:29
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.Snowflake;
using Hymson.Utils;
//using Hymson.Utils.Extensions;
using System.Transactions;

namespace Hymson.MES.Services.Services.Warehouse
{
    /// <summary>
    /// 物料台账 服务
    /// </summary>
    public class WhMaterialStandingbookService : IWhMaterialStandingbookService
    {
        private readonly ICurrentUser _currentUser;

        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 物料台账 仓储
        /// </summary>
        private readonly IWhMaterialStandingbookRepository _whMaterialStandingbookRepository;

        private readonly AbstractValidator<WhMaterialStandingbookCreateDto> _validationCreateRules;
        private readonly AbstractValidator<WhMaterialStandingbookModifyDto> _validationModifyRules;

        public WhMaterialStandingbookService(ICurrentUser currentUser, ICurrentSite currentSite, IWhMaterialStandingbookRepository whMaterialStandingbookRepository, AbstractValidator<WhMaterialStandingbookCreateDto> validationCreateRules, AbstractValidator<WhMaterialStandingbookModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _whMaterialStandingbookRepository = whMaterialStandingbookRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _currentSite = currentSite;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="whMaterialStandingbookDto"></param>
        /// <returns></returns>
        public async Task CreateWhMaterialStandingbookAsync(WhMaterialStandingbookCreateDto whMaterialStandingbookCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(whMaterialStandingbookCreateDto);

            //DTO转换实体
            var whMaterialStandingbookEntity = whMaterialStandingbookCreateDto.ToEntity<WhMaterialStandingbookEntity>();
            whMaterialStandingbookEntity.Id = IdGenProvider.Instance.CreateId();
            whMaterialStandingbookEntity.CreatedBy = _currentUser.UserName;
            whMaterialStandingbookEntity.UpdatedBy = _currentUser.UserName;
            whMaterialStandingbookEntity.CreatedOn = HymsonClock.Now();
            whMaterialStandingbookEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _whMaterialStandingbookRepository.InsertAsync(whMaterialStandingbookEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteWhMaterialStandingbookAsync(long id)
        {
            await _whMaterialStandingbookRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesWhMaterialStandingbookAsync(string ids)
        {
            var idsArr = ids.ToSpitLongArray();
            return await _whMaterialStandingbookRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="whMaterialStandingbookPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhMaterialStandingbookDto>> GetPageListAsync(WhMaterialStandingbookPagedQueryDto whMaterialStandingbookPagedQueryDto)
        {
            var whMaterialStandingbookPagedQuery = whMaterialStandingbookPagedQueryDto.ToQuery<WhMaterialStandingbookPagedQuery>();
            whMaterialStandingbookPagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _whMaterialStandingbookRepository.GetPagedInfoAsync(whMaterialStandingbookPagedQuery);

            //实体到DTO转换 装载数据
            List<WhMaterialStandingbookDto> whMaterialStandingbookDtos = PrepareWhMaterialStandingbookDtos(pagedInfo);
            return new PagedInfo<WhMaterialStandingbookDto>(whMaterialStandingbookDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<WhMaterialStandingbookDto> PrepareWhMaterialStandingbookDtos(PagedInfo<WhMaterialStandingbookEntity> pagedInfo)
        {
            var whMaterialStandingbookDtos = new List<WhMaterialStandingbookDto>();
            foreach (var whMaterialStandingbookEntity in pagedInfo.Data)
            {
                var whMaterialStandingbookDto = whMaterialStandingbookEntity.ToModel<WhMaterialStandingbookDto>();
                whMaterialStandingbookDtos.Add(whMaterialStandingbookDto);
            }

            return whMaterialStandingbookDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="whMaterialStandingbookDto"></param>
        /// <returns></returns>
        public async Task ModifyWhMaterialStandingbookAsync(WhMaterialStandingbookModifyDto whMaterialStandingbookModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(whMaterialStandingbookModifyDto);

            //DTO转换实体
            var whMaterialStandingbookEntity = whMaterialStandingbookModifyDto.ToEntity<WhMaterialStandingbookEntity>();
            whMaterialStandingbookEntity.UpdatedBy = _currentUser.UserName;
            whMaterialStandingbookEntity.UpdatedOn = HymsonClock.Now();

            await _whMaterialStandingbookRepository.UpdateAsync(whMaterialStandingbookEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<WhMaterialStandingbookDto> QueryWhMaterialStandingbookByIdAsync(long id)
        {
            var whMaterialStandingbookEntity = await _whMaterialStandingbookRepository.GetByIdAsync(id);
            if (whMaterialStandingbookEntity != null)
            {
                return whMaterialStandingbookEntity.ToModel<WhMaterialStandingbookDto>();
            }
            return null;
        }
    }
}
