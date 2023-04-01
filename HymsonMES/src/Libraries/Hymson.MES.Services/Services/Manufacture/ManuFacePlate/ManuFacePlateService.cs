/*
 *creator: Karl
 *
 *describe: 操作面板    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-04-01 01:56:57
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
    /// 操作面板 服务
    /// </summary>
    public class ManuFacePlateService : IManuFacePlateService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 操作面板 仓储
        /// </summary>
        private readonly IManuFacePlateRepository _manuFacePlateRepository;
        private readonly AbstractValidator<ManuFacePlateCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuFacePlateModifyDto> _validationModifyRules;

        public ManuFacePlateService(ICurrentUser currentUser, ICurrentSite currentSite, IManuFacePlateRepository manuFacePlateRepository, AbstractValidator<ManuFacePlateCreateDto> validationCreateRules, AbstractValidator<ManuFacePlateModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuFacePlateRepository = manuFacePlateRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuFacePlateCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuFacePlateAsync(ManuFacePlateCreateDto manuFacePlateCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuFacePlateCreateDto);

            //DTO转换实体
            var manuFacePlateEntity = manuFacePlateCreateDto.ToEntity<ManuFacePlateEntity>();
            manuFacePlateEntity.Id= IdGenProvider.Instance.CreateId();
            manuFacePlateEntity.CreatedBy = _currentUser.UserName;
            manuFacePlateEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateEntity.CreatedOn = HymsonClock.Now();
            manuFacePlateEntity.UpdatedOn = HymsonClock.Now();
            manuFacePlateEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _manuFacePlateRepository.InsertAsync(manuFacePlateEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuFacePlateAsync(long id)
        {
            await _manuFacePlateRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuFacePlateAsync(long[] ids)
        {
            return await _manuFacePlateRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuFacePlatePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFacePlateDto>> GetPagedListAsync(ManuFacePlatePagedQueryDto manuFacePlatePagedQueryDto)
        {
            var manuFacePlatePagedQuery = manuFacePlatePagedQueryDto.ToQuery<ManuFacePlatePagedQuery>();
            var pagedInfo = await _manuFacePlateRepository.GetPagedInfoAsync(manuFacePlatePagedQuery);

            //实体到DTO转换 装载数据
            List<ManuFacePlateDto> manuFacePlateDtos = PrepareManuFacePlateDtos(pagedInfo);
            return new PagedInfo<ManuFacePlateDto>(manuFacePlateDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuFacePlateDto> PrepareManuFacePlateDtos(PagedInfo<ManuFacePlateEntity>   pagedInfo)
        {
            var manuFacePlateDtos = new List<ManuFacePlateDto>();
            foreach (var manuFacePlateEntity in pagedInfo.Data)
            {
                var manuFacePlateDto = manuFacePlateEntity.ToModel<ManuFacePlateDto>();
                manuFacePlateDtos.Add(manuFacePlateDto);
            }

            return manuFacePlateDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuFacePlateDto"></param>
        /// <returns></returns>
        public async Task ModifyManuFacePlateAsync(ManuFacePlateModifyDto manuFacePlateModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuFacePlateModifyDto);

            //DTO转换实体
            var manuFacePlateEntity = manuFacePlateModifyDto.ToEntity<ManuFacePlateEntity>();
            manuFacePlateEntity.UpdatedBy = _currentUser.UserName;
            manuFacePlateEntity.UpdatedOn = HymsonClock.Now();

            await _manuFacePlateRepository.UpdateAsync(manuFacePlateEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuFacePlateDto> QueryManuFacePlateByIdAsync(long id) 
        {
           var manuFacePlateEntity = await _manuFacePlateRepository.GetByIdAsync(id);
           if (manuFacePlateEntity != null) 
           {
               return manuFacePlateEntity.ToModel<ManuFacePlateDto>();
           }
            return null;
        }
    }
}
