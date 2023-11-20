/*
 *creator: Karl
 *
 *describe: 烘烤执行表    服务 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-28 05:42:41
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
    /// 烘烤执行表 服务
    /// </summary>
    public class ManuBakingRecordService : IManuBakingRecordService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 烘烤执行表 仓储
        /// </summary>
        private readonly IManuBakingRecordRepository _manuBakingRecordRepository;
        private readonly AbstractValidator<ManuBakingRecordCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuBakingRecordModifyDto> _validationModifyRules;

        public ManuBakingRecordService(ICurrentUser currentUser, ICurrentSite currentSite, IManuBakingRecordRepository manuBakingRecordRepository, AbstractValidator<ManuBakingRecordCreateDto> validationCreateRules, AbstractValidator<ManuBakingRecordModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuBakingRecordRepository = manuBakingRecordRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuBakingRecordCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuBakingRecordAsync(ManuBakingRecordCreateDto manuBakingRecordCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuBakingRecordCreateDto);

            //DTO转换实体
            var manuBakingRecordEntity = manuBakingRecordCreateDto.ToEntity<ManuBakingRecordEntity>();
            manuBakingRecordEntity.Id= IdGenProvider.Instance.CreateId();
            manuBakingRecordEntity.CreatedBy = _currentUser.UserName;
            manuBakingRecordEntity.UpdatedBy = _currentUser.UserName;
            manuBakingRecordEntity.CreatedOn = HymsonClock.Now();
            manuBakingRecordEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _manuBakingRecordRepository.InsertAsync(manuBakingRecordEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuBakingRecordAsync(long id)
        {
            await _manuBakingRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuBakingRecordAsync(long[] ids)
        {
            return await _manuBakingRecordRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuBakingRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuBakingRecordDto>> GetPagedListAsync(ManuBakingRecordPagedQueryDto manuBakingRecordPagedQueryDto)
        {
            var manuBakingRecordPagedQuery = manuBakingRecordPagedQueryDto.ToQuery<ManuBakingRecordPagedQuery>();
            var pagedInfo = await _manuBakingRecordRepository.GetPagedInfoAsync(manuBakingRecordPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuBakingRecordDto> manuBakingRecordDtos = PrepareManuBakingRecordDtos(pagedInfo);
            return new PagedInfo<ManuBakingRecordDto>(manuBakingRecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuBakingRecordDto> PrepareManuBakingRecordDtos(PagedInfo<ManuBakingRecordEntity>   pagedInfo)
        {
            var manuBakingRecordDtos = new List<ManuBakingRecordDto>();
            foreach (var manuBakingRecordEntity in pagedInfo.Data)
            {
                var manuBakingRecordDto = manuBakingRecordEntity.ToModel<ManuBakingRecordDto>();
                manuBakingRecordDtos.Add(manuBakingRecordDto);
            }

            return manuBakingRecordDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuBakingRecordModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuBakingRecordAsync(ManuBakingRecordModifyDto manuBakingRecordModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuBakingRecordModifyDto);

            //DTO转换实体
            var manuBakingRecordEntity = manuBakingRecordModifyDto.ToEntity<ManuBakingRecordEntity>();
            manuBakingRecordEntity.UpdatedBy = _currentUser.UserName;
            manuBakingRecordEntity.UpdatedOn = HymsonClock.Now();

            await _manuBakingRecordRepository.UpdateAsync(manuBakingRecordEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuBakingRecordDto> QueryManuBakingRecordByIdAsync(long id) 
        {
           var manuBakingRecordEntity = await _manuBakingRecordRepository.GetByIdAsync(id);
           if (manuBakingRecordEntity != null) 
           {
               return manuBakingRecordEntity.ToModel<ManuBakingRecordDto>();
           }
            return new ManuBakingRecordDto();
        }
    }
}
