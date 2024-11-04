/*
 *creator: Karl
 *
 *describe: 托盘条码关系    服务 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:13
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
    /// 托盘条码关系 服务
    /// </summary>
    public class ManuTraySfcRelationService : IManuTraySfcRelationService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 托盘条码关系 仓储
        /// </summary>
        private readonly IManuTraySfcRelationRepository _manuTraySfcRelationRepository;
        private readonly AbstractValidator<ManuTraySfcRelationCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuTraySfcRelationModifyDto> _validationModifyRules;

        public ManuTraySfcRelationService(ICurrentUser currentUser, ICurrentSite currentSite, IManuTraySfcRelationRepository manuTraySfcRelationRepository, AbstractValidator<ManuTraySfcRelationCreateDto> validationCreateRules, AbstractValidator<ManuTraySfcRelationModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuTraySfcRelationRepository = manuTraySfcRelationRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuTraySfcRelationCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuTraySfcRelationAsync(ManuTraySfcRelationCreateDto manuTraySfcRelationCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuTraySfcRelationCreateDto);

            //DTO转换实体
            var manuTraySfcRelationEntity = manuTraySfcRelationCreateDto.ToEntity<ManuTraySfcRelationEntity>();
            manuTraySfcRelationEntity.Id= IdGenProvider.Instance.CreateId();
            manuTraySfcRelationEntity.CreatedBy = _currentUser.UserName;
            manuTraySfcRelationEntity.UpdatedBy = _currentUser.UserName;
            manuTraySfcRelationEntity.CreatedOn = HymsonClock.Now();
            manuTraySfcRelationEntity.UpdatedOn = HymsonClock.Now();
            manuTraySfcRelationEntity.SiteId = _currentSite.SiteId ?? 123456;

            //入库
            await _manuTraySfcRelationRepository.InsertAsync(manuTraySfcRelationEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuTraySfcRelationAsync(long id)
        {
            await _manuTraySfcRelationRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuTraySfcRelationAsync(long[] ids)
        {
            return await _manuTraySfcRelationRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuTraySfcRelationPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuTraySfcRelationDto>> GetPagedListAsync(ManuTraySfcRelationPagedQueryDto manuTraySfcRelationPagedQueryDto)
        {
            var manuTraySfcRelationPagedQuery = manuTraySfcRelationPagedQueryDto.ToQuery<ManuTraySfcRelationPagedQuery>();
            var pagedInfo = await _manuTraySfcRelationRepository.GetPagedInfoAsync(manuTraySfcRelationPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuTraySfcRelationDto> manuTraySfcRelationDtos = PrepareManuTraySfcRelationDtos(pagedInfo);
            return new PagedInfo<ManuTraySfcRelationDto>(manuTraySfcRelationDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuTraySfcRelationDto> PrepareManuTraySfcRelationDtos(PagedInfo<ManuTraySfcRelationEntity>   pagedInfo)
        {
            var manuTraySfcRelationDtos = new List<ManuTraySfcRelationDto>();
            foreach (var manuTraySfcRelationEntity in pagedInfo.Data)
            {
                var manuTraySfcRelationDto = manuTraySfcRelationEntity.ToModel<ManuTraySfcRelationDto>();
                manuTraySfcRelationDtos.Add(manuTraySfcRelationDto);
            }

            return manuTraySfcRelationDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuTraySfcRelationModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuTraySfcRelationAsync(ManuTraySfcRelationModifyDto manuTraySfcRelationModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuTraySfcRelationModifyDto);

            //DTO转换实体
            var manuTraySfcRelationEntity = manuTraySfcRelationModifyDto.ToEntity<ManuTraySfcRelationEntity>();
            manuTraySfcRelationEntity.UpdatedBy = _currentUser.UserName;
            manuTraySfcRelationEntity.UpdatedOn = HymsonClock.Now();

            await _manuTraySfcRelationRepository.UpdateAsync(manuTraySfcRelationEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuTraySfcRelationDto> QueryManuTraySfcRelationByIdAsync(long id) 
        {
           var manuTraySfcRelationEntity = await _manuTraySfcRelationRepository.GetByIdAsync(id);
           if (manuTraySfcRelationEntity != null) 
           {
               return manuTraySfcRelationEntity.ToModel<ManuTraySfcRelationDto>();
           }
            return null;
        }
    }
}
