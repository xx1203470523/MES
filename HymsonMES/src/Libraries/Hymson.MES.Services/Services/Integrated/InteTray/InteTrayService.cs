/*
 *creator: Karl
 *
 *describe: 托盘信息    服务 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 10:57:03
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
using Hymson.MES.Data.Repositories.Integrated.InteTray.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 托盘信息 服务
    /// </summary>
    public class InteTrayService : IInteTrayService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 托盘信息 仓储
        /// </summary>
        private readonly IInteTrayRepository _inteTrayRepository;
        private readonly AbstractValidator<InteTrayCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteTrayModifyDto> _validationModifyRules;

        public InteTrayService(ICurrentUser currentUser, ICurrentSite currentSite, IInteTrayRepository inteTrayRepository, AbstractValidator<InteTrayCreateDto> validationCreateRules, AbstractValidator<InteTrayModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteTrayRepository = inteTrayRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="inteTrayCreateDto"></param>
        /// <returns></returns>
        public async Task CreateInteTrayAsync(InteTrayCreateDto inteTrayCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(inteTrayCreateDto);

            //DTO转换实体
            var inteTrayEntity = inteTrayCreateDto.ToEntity<InteTrayEntity>();
            inteTrayEntity.Id= IdGenProvider.Instance.CreateId();
            inteTrayEntity.CreatedBy = _currentUser.UserName;
            inteTrayEntity.UpdatedBy = _currentUser.UserName;
            inteTrayEntity.CreatedOn = HymsonClock.Now();
            inteTrayEntity.UpdatedOn = HymsonClock.Now();
            inteTrayEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _inteTrayRepository.InsertAsync(inteTrayEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteInteTrayAsync(long id)
        {
            await _inteTrayRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteTrayAsync(long[] ids)
        {
            return await _inteTrayRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="inteTrayPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteTrayDto>> GetPagedListAsync(InteTrayPagedQueryDto inteTrayPagedQueryDto)
        {
            var inteTrayPagedQuery = inteTrayPagedQueryDto.ToQuery<InteTrayPagedQuery>();
            var pagedInfo = await _inteTrayRepository.GetPagedInfoAsync(inteTrayPagedQuery);

            //实体到DTO转换 装载数据
            List<InteTrayDto> inteTrayDtos = PrepareInteTrayDtos(pagedInfo);
            return new PagedInfo<InteTrayDto>(inteTrayDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteTrayDto> PrepareInteTrayDtos(PagedInfo<InteTrayEntity>   pagedInfo)
        {
            var inteTrayDtos = new List<InteTrayDto>();
            foreach (var inteTrayEntity in pagedInfo.Data)
            {
                var inteTrayDto = inteTrayEntity.ToModel<InteTrayDto>();
                inteTrayDtos.Add(inteTrayDto);
            }

            return inteTrayDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteTrayModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyInteTrayAsync(InteTrayModifyDto inteTrayModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(inteTrayModifyDto);

            //DTO转换实体
            var inteTrayEntity = inteTrayModifyDto.ToEntity<InteTrayEntity>();
            inteTrayEntity.UpdatedBy = _currentUser.UserName;
            inteTrayEntity.UpdatedOn = HymsonClock.Now();

            await _inteTrayRepository.UpdateAsync(inteTrayEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteTrayDto> QueryInteTrayByIdAsync(long id) 
        {
           var inteTrayEntity = await _inteTrayRepository.GetByIdAsync(id);
           if (inteTrayEntity != null) 
           {
               return inteTrayEntity.ToModel<InteTrayDto>();
           }
            return null;
        }
    }
}
