/*
 *creator: Karl
 *
 *describe: 条码绑定关系表    服务 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-17 10:09:11
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
    /// 条码绑定关系表 服务
    /// </summary>
    public class ManuSfcBindService : IManuSfcBindService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 条码绑定关系表 仓储
        /// </summary>
        private readonly IManuSfcBindRepository _manuSfcBindRepository;
        private readonly AbstractValidator<ManuSfcBindCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuSfcBindModifyDto> _validationModifyRules;

        public ManuSfcBindService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcBindRepository manuSfcBindRepository, AbstractValidator<ManuSfcBindCreateDto> validationCreateRules, AbstractValidator<ManuSfcBindModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcBindRepository = manuSfcBindRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuSfcBindCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuSfcBindAsync(ManuSfcBindCreateDto manuSfcBindCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuSfcBindCreateDto);

            //DTO转换实体
            var manuSfcBindEntity = manuSfcBindCreateDto.ToEntity<ManuSfcBindEntity>();
            manuSfcBindEntity.Id= IdGenProvider.Instance.CreateId();
            manuSfcBindEntity.CreatedBy = _currentUser.UserName;
            manuSfcBindEntity.UpdatedBy = _currentUser.UserName;
            manuSfcBindEntity.CreatedOn = HymsonClock.Now();
            manuSfcBindEntity.UpdatedOn = HymsonClock.Now();
            manuSfcBindEntity.SiteId = _currentSite.SiteId ?? 123456;

            //入库
            await _manuSfcBindRepository.InsertAsync(manuSfcBindEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuSfcBindAsync(long id)
        {
            await _manuSfcBindRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuSfcBindAsync(long[] ids)
        {
            return await _manuSfcBindRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuSfcBindPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcBindDto>> GetPagedListAsync(ManuSfcBindPagedQueryDto manuSfcBindPagedQueryDto)
        {
            var manuSfcBindPagedQuery = manuSfcBindPagedQueryDto.ToQuery<ManuSfcBindPagedQuery>();
            var pagedInfo = await _manuSfcBindRepository.GetPagedInfoAsync(manuSfcBindPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuSfcBindDto> manuSfcBindDtos = PrepareManuSfcBindDtos(pagedInfo);
            return new PagedInfo<ManuSfcBindDto>(manuSfcBindDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuSfcBindDto> PrepareManuSfcBindDtos(PagedInfo<ManuSfcBindEntity>   pagedInfo)
        {
            var manuSfcBindDtos = new List<ManuSfcBindDto>();
            foreach (var manuSfcBindEntity in pagedInfo.Data)
            {
                var manuSfcBindDto = manuSfcBindEntity.ToModel<ManuSfcBindDto>();
                manuSfcBindDtos.Add(manuSfcBindDto);
            }

            return manuSfcBindDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcBindDto"></param>
        /// <returns></returns>
        public async Task ModifyManuSfcBindAsync(ManuSfcBindModifyDto manuSfcBindModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuSfcBindModifyDto);

            //DTO转换实体
            var manuSfcBindEntity = manuSfcBindModifyDto.ToEntity<ManuSfcBindEntity>();
            manuSfcBindEntity.UpdatedBy = _currentUser.UserName;
            manuSfcBindEntity.UpdatedOn = HymsonClock.Now();

            await _manuSfcBindRepository.UpdateAsync(manuSfcBindEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcBindDto> QueryManuSfcBindByIdAsync(long id) 
        {
           var manuSfcBindEntity = await _manuSfcBindRepository.GetByIdAsync(id);
           if (manuSfcBindEntity != null) 
           {
               return manuSfcBindEntity.ToModel<ManuSfcBindDto>();
           }
            return null;
        }
    }
}
