/*
 *creator: Karl
 *
 *describe: 容器装载表（物理删除）    服务 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:33:13
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
    /// 容器装载表（物理删除） 服务
    /// </summary>
    public class ManuContainerPackService : IManuContainerPackService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 容器装载表（物理删除） 仓储
        /// </summary>
        private readonly IManuContainerPackRepository _manuContainerPackRepository;
        private readonly AbstractValidator<ManuContainerPackCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuContainerPackModifyDto> _validationModifyRules;

        public ManuContainerPackService(ICurrentUser currentUser, ICurrentSite currentSite, IManuContainerPackRepository manuContainerPackRepository, AbstractValidator<ManuContainerPackCreateDto> validationCreateRules, AbstractValidator<ManuContainerPackModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuContainerPackRepository = manuContainerPackRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuContainerPackCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuContainerPackAsync(ManuContainerPackCreateDto manuContainerPackCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuContainerPackCreateDto);

            //DTO转换实体
            var manuContainerPackEntity = manuContainerPackCreateDto.ToEntity<ManuContainerPackEntity>();
            manuContainerPackEntity.Id= IdGenProvider.Instance.CreateId();
            manuContainerPackEntity.CreatedBy = _currentUser.UserName;
            manuContainerPackEntity.UpdatedBy = _currentUser.UserName;
            manuContainerPackEntity.CreatedOn = HymsonClock.Now();
            manuContainerPackEntity.UpdatedOn = HymsonClock.Now();
            manuContainerPackEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _manuContainerPackRepository.InsertAsync(manuContainerPackEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuContainerPackAsync(long id)
        {
            await _manuContainerPackRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuContainerPackAsync(long[] ids)
        {
            return await _manuContainerPackRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuContainerPackPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuContainerPackDto>> GetPagedListAsync(ManuContainerPackPagedQueryDto manuContainerPackPagedQueryDto)
        {
            var manuContainerPackPagedQuery = manuContainerPackPagedQueryDto.ToQuery<ManuContainerPackPagedQuery>();
            var pagedInfo = await _manuContainerPackRepository.GetPagedInfoAsync(manuContainerPackPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuContainerPackDto> manuContainerPackDtos = PrepareManuContainerPackDtos(pagedInfo);
            return new PagedInfo<ManuContainerPackDto>(manuContainerPackDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuContainerPackDto> PrepareManuContainerPackDtos(PagedInfo<ManuContainerPackEntity>   pagedInfo)
        {
            var manuContainerPackDtos = new List<ManuContainerPackDto>();
            foreach (var manuContainerPackEntity in pagedInfo.Data)
            {
                var manuContainerPackDto = manuContainerPackEntity.ToModel<ManuContainerPackDto>();
                manuContainerPackDtos.Add(manuContainerPackDto);
            }

            return manuContainerPackDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuContainerPackDto"></param>
        /// <returns></returns>
        public async Task ModifyManuContainerPackAsync(ManuContainerPackModifyDto manuContainerPackModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuContainerPackModifyDto);

            //DTO转换实体
            var manuContainerPackEntity = manuContainerPackModifyDto.ToEntity<ManuContainerPackEntity>();
            manuContainerPackEntity.UpdatedBy = _currentUser.UserName;
            manuContainerPackEntity.UpdatedOn = HymsonClock.Now();

            await _manuContainerPackRepository.UpdateAsync(manuContainerPackEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuContainerPackDto> QueryManuContainerPackByIdAsync(long id) 
        {
           var manuContainerPackEntity = await _manuContainerPackRepository.GetByIdAsync(id);
           if (manuContainerPackEntity != null) 
           {
               return manuContainerPackEntity.ToModel<ManuContainerPackDto>();
           }
            return null;
        }
    }
}
