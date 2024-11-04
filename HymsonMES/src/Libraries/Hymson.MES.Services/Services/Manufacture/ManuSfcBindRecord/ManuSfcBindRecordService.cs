/*
 *creator: Karl
 *
 *describe: 条码绑定解绑记录表    服务 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-17 10:09:25
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
    /// 条码绑定解绑记录表 服务
    /// </summary>
    public class ManuSfcBindRecordService : IManuSfcBindRecordService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 条码绑定解绑记录表 仓储
        /// </summary>
        private readonly IManuSfcBindRecordRepository _manuSfcBindRecordRepository;
        private readonly AbstractValidator<ManuSfcBindRecordCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuSfcBindRecordModifyDto> _validationModifyRules;

        public ManuSfcBindRecordService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcBindRecordRepository manuSfcBindRecordRepository, AbstractValidator<ManuSfcBindRecordCreateDto> validationCreateRules, AbstractValidator<ManuSfcBindRecordModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcBindRecordRepository = manuSfcBindRecordRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuSfcBindRecordCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuSfcBindRecordAsync(ManuSfcBindRecordCreateDto manuSfcBindRecordCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuSfcBindRecordCreateDto);

            //DTO转换实体
            var manuSfcBindRecordEntity = manuSfcBindRecordCreateDto.ToEntity<ManuSfcBindRecordEntity>();
            manuSfcBindRecordEntity.Id= IdGenProvider.Instance.CreateId();
            manuSfcBindRecordEntity.CreatedBy = _currentUser.UserName;
            manuSfcBindRecordEntity.UpdatedBy = _currentUser.UserName;
            manuSfcBindRecordEntity.CreatedOn = HymsonClock.Now();
            manuSfcBindRecordEntity.UpdatedOn = HymsonClock.Now();
            manuSfcBindRecordEntity.SiteId = _currentSite.SiteId ?? 123456;

            //入库
            await _manuSfcBindRecordRepository.InsertAsync(manuSfcBindRecordEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuSfcBindRecordAsync(long id)
        {
            await _manuSfcBindRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuSfcBindRecordAsync(long[] ids)
        {
            return await _manuSfcBindRecordRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuSfcBindRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcBindRecordDto>> GetPagedListAsync(ManuSfcBindRecordPagedQueryDto manuSfcBindRecordPagedQueryDto)
        {
            var manuSfcBindRecordPagedQuery = manuSfcBindRecordPagedQueryDto.ToQuery<ManuSfcBindRecordPagedQuery>();
            var pagedInfo = await _manuSfcBindRecordRepository.GetPagedInfoAsync(manuSfcBindRecordPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuSfcBindRecordDto> manuSfcBindRecordDtos = PrepareManuSfcBindRecordDtos(pagedInfo);
            return new PagedInfo<ManuSfcBindRecordDto>(manuSfcBindRecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuSfcBindRecordDto> PrepareManuSfcBindRecordDtos(PagedInfo<ManuSfcBindRecordEntity>   pagedInfo)
        {
            var manuSfcBindRecordDtos = new List<ManuSfcBindRecordDto>();
            foreach (var manuSfcBindRecordEntity in pagedInfo.Data)
            {
                var manuSfcBindRecordDto = manuSfcBindRecordEntity.ToModel<ManuSfcBindRecordDto>();
                manuSfcBindRecordDtos.Add(manuSfcBindRecordDto);
            }

            return manuSfcBindRecordDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcBindRecordDto"></param>
        /// <returns></returns>
        public async Task ModifyManuSfcBindRecordAsync(ManuSfcBindRecordModifyDto manuSfcBindRecordModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuSfcBindRecordModifyDto);

            //DTO转换实体
            var manuSfcBindRecordEntity = manuSfcBindRecordModifyDto.ToEntity<ManuSfcBindRecordEntity>();
            manuSfcBindRecordEntity.UpdatedBy = _currentUser.UserName;
            manuSfcBindRecordEntity.UpdatedOn = HymsonClock.Now();

            await _manuSfcBindRecordRepository.UpdateAsync(manuSfcBindRecordEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcBindRecordDto> QueryManuSfcBindRecordByIdAsync(long id) 
        {
           var manuSfcBindRecordEntity = await _manuSfcBindRecordRepository.GetByIdAsync(id);
           if (manuSfcBindRecordEntity != null) 
           {
               return manuSfcBindRecordEntity.ToModel<ManuSfcBindRecordDto>();
           }
            return null;
        }
    }
}
