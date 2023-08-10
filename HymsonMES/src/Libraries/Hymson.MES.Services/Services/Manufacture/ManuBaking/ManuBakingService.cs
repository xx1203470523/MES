/*
 *creator: Karl
 *
 *describe: 烘烤工序    服务 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-28 05:41:12
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 烘烤工序 服务
    /// </summary>
    public class ManuBakingService : IManuBakingService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 烘烤工序 仓储
        /// </summary>
        private readonly IManuBakingRepository _manuBakingRepository;
        private readonly AbstractValidator<ManuBakingCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuBakingModifyDto> _validationModifyRules;

        public ManuBakingService(ICurrentUser currentUser, ICurrentSite currentSite, IManuBakingRepository manuBakingRepository, AbstractValidator<ManuBakingCreateDto> validationCreateRules, AbstractValidator<ManuBakingModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuBakingRepository = manuBakingRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuBakingCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuBakingAsync(ManuBakingCreateDto manuBakingCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuBakingCreateDto);

            //DTO转换实体
            var manuBakingEntity = manuBakingCreateDto.ToEntity<ManuBakingEntity>();
            manuBakingEntity.Id= IdGenProvider.Instance.CreateId();
            manuBakingEntity.CreatedBy = _currentUser.UserName;
            manuBakingEntity.UpdatedBy = _currentUser.UserName;
            manuBakingEntity.CreatedOn = HymsonClock.Now();
            manuBakingEntity.UpdatedOn = HymsonClock.Now();
            manuBakingEntity.SiteId = _currentSite.SiteId ?? 123456;

            //入库
            await _manuBakingRepository.InsertAsync(manuBakingEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuBakingAsync(long id)
        {
            await _manuBakingRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuBakingAsync(long[] ids)
        {
            return await _manuBakingRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuBakingPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuBakingDto>> GetPagedListAsync(ManuBakingPagedQueryDto manuBakingPagedQueryDto)
        {
            var manuBakingPagedQuery = manuBakingPagedQueryDto.ToQuery<ManuBakingPagedQuery>();
            var pagedInfo = await _manuBakingRepository.GetPagedInfoAsync(manuBakingPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuBakingDto> manuBakingDtos = PrepareManuBakingDtos(pagedInfo);
            return new PagedInfo<ManuBakingDto>(manuBakingDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuBakingDto> PrepareManuBakingDtos(PagedInfo<ManuBakingEntity>   pagedInfo)
        {
            var manuBakingDtos = new List<ManuBakingDto>();
            foreach (var manuBakingEntity in pagedInfo.Data)
            {
                var manuBakingDto = manuBakingEntity.ToModel<ManuBakingDto>();
                manuBakingDtos.Add(manuBakingDto);
            }

            return manuBakingDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuBakingModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuBakingAsync(ManuBakingModifyDto manuBakingModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuBakingModifyDto);

            //DTO转换实体
            var manuBakingEntity = manuBakingModifyDto.ToEntity<ManuBakingEntity>();
            manuBakingEntity.UpdatedBy = _currentUser.UserName;
            manuBakingEntity.UpdatedOn = HymsonClock.Now();

            await _manuBakingRepository.UpdateAsync(manuBakingEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuBakingDto> QueryManuBakingByIdAsync(long id) 
        {
           var manuBakingEntity = await _manuBakingRepository.GetByIdAsync(id);
           if (manuBakingEntity != null) 
           {
               return manuBakingEntity.ToModel<ManuBakingDto>();
           }
            return null;
        }
    }
}
