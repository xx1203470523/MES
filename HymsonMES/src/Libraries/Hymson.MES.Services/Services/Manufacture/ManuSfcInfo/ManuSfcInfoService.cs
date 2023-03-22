/*
 *creator: Karl
 *
 *describe: 条码信息表    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2023-03-21 04:00:29
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
//using Hymson.Utils.Extensions;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 条码信息表 服务
    /// </summary>
    public class ManuSfcInfoService : IManuSfcInfoService
    {
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly AbstractValidator<ManuSfcInfoCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuSfcInfoModifyDto> _validationModifyRules;

        public ManuSfcInfoService(ICurrentUser currentUser,IManuSfcInfoRepository manuSfcInfoRepository, AbstractValidator<ManuSfcInfoCreateDto> validationCreateRules, AbstractValidator<ManuSfcInfoModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuSfcInfoDto"></param>
        /// <returns></returns>
        public async Task CreateManuSfcInfoAsync(ManuSfcInfoCreateDto manuSfcInfoCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuSfcInfoCreateDto);

            //DTO转换实体
            var manuSfcInfoEntity = manuSfcInfoCreateDto.ToEntity<ManuSfcInfoEntity>();
            manuSfcInfoEntity.Id= IdGenProvider.Instance.CreateId();
            manuSfcInfoEntity.CreatedBy = _currentUser.UserName;
            manuSfcInfoEntity.UpdatedBy = _currentUser.UserName;
            manuSfcInfoEntity.CreatedOn = HymsonClock.Now();
            manuSfcInfoEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _manuSfcInfoRepository.InsertAsync(manuSfcInfoEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuSfcInfoAsync(long id)
        {
            await _manuSfcInfoRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuSfcInfoAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            return await _manuSfcInfoRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuSfcInfoPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcInfoDto>> GetPageListAsync(ManuSfcInfoPagedQueryDto manuSfcInfoPagedQueryDto)
        {
            var manuSfcInfoPagedQuery = manuSfcInfoPagedQueryDto.ToQuery<ManuSfcInfoPagedQuery>();
            var pagedInfo = await _manuSfcInfoRepository.GetPagedInfoAsync(manuSfcInfoPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuSfcInfoDto> manuSfcInfoDtos = PrepareManuSfcInfoDtos(pagedInfo);
            return new PagedInfo<ManuSfcInfoDto>(manuSfcInfoDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuSfcInfoDto> PrepareManuSfcInfoDtos(PagedInfo<ManuSfcInfoEntity>   pagedInfo)
        {
            var manuSfcInfoDtos = new List<ManuSfcInfoDto>();
            foreach (var manuSfcInfoEntity in pagedInfo.Data)
            {
                var manuSfcInfoDto = manuSfcInfoEntity.ToModel<ManuSfcInfoDto>();
                manuSfcInfoDtos.Add(manuSfcInfoDto);
            }

            return manuSfcInfoDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcInfoDto"></param>
        /// <returns></returns>
        public async Task ModifyManuSfcInfoAsync(ManuSfcInfoModifyDto manuSfcInfoModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuSfcInfoModifyDto);

            //DTO转换实体
            var manuSfcInfoEntity = manuSfcInfoModifyDto.ToEntity<ManuSfcInfoEntity>();
            manuSfcInfoEntity.UpdatedBy = _currentUser.UserName;
            manuSfcInfoEntity.UpdatedOn = HymsonClock.Now();

            await _manuSfcInfoRepository.UpdateAsync(manuSfcInfoEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcInfoDto> QueryManuSfcInfoByIdAsync(long id) 
        {
           var manuSfcInfoEntity = await _manuSfcInfoRepository.GetByIdAsync(id);
           if (manuSfcInfoEntity != null) 
           {
               return manuSfcInfoEntity.ToModel<ManuSfcInfoDto>();
           }
            return null;
        }
    }
}
