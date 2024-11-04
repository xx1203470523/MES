/*
 *creator: Karl
 *
 *describe: 托盘条码记录表    服务 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:11:02
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
    /// 托盘条码记录表 服务
    /// </summary>
    public class ManuTraySfcRecordService : IManuTraySfcRecordService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 托盘条码记录表 仓储
        /// </summary>
        private readonly IManuTraySfcRecordRepository _manuTraySfcRecordRepository;
        private readonly AbstractValidator<ManuTraySfcRecordCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuTraySfcRecordModifyDto> _validationModifyRules;

        public ManuTraySfcRecordService(ICurrentUser currentUser, ICurrentSite currentSite, IManuTraySfcRecordRepository manuTraySfcRecordRepository, AbstractValidator<ManuTraySfcRecordCreateDto> validationCreateRules, AbstractValidator<ManuTraySfcRecordModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuTraySfcRecordRepository = manuTraySfcRecordRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuTraySfcRecordCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuTraySfcRecordAsync(ManuTraySfcRecordCreateDto manuTraySfcRecordCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuTraySfcRecordCreateDto);

            //DTO转换实体
            var manuTraySfcRecordEntity = manuTraySfcRecordCreateDto.ToEntity<ManuTraySfcRecordEntity>();
            manuTraySfcRecordEntity.Id= IdGenProvider.Instance.CreateId();
            manuTraySfcRecordEntity.CreatedBy = _currentUser.UserName;
            manuTraySfcRecordEntity.UpdatedBy = _currentUser.UserName;
            manuTraySfcRecordEntity.CreatedOn = HymsonClock.Now();
            manuTraySfcRecordEntity.UpdatedOn = HymsonClock.Now();
            manuTraySfcRecordEntity.SiteId = _currentSite.SiteId ?? 123456;

            //入库
            await _manuTraySfcRecordRepository.InsertAsync(manuTraySfcRecordEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuTraySfcRecordAsync(long id)
        {
            await _manuTraySfcRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuTraySfcRecordAsync(long[] ids)
        {
            return await _manuTraySfcRecordRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuTraySfcRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuTraySfcRecordDto>> GetPagedListAsync(ManuTraySfcRecordPagedQueryDto manuTraySfcRecordPagedQueryDto)
        {
            var manuTraySfcRecordPagedQuery = manuTraySfcRecordPagedQueryDto.ToQuery<ManuTraySfcRecordPagedQuery>();
            var pagedInfo = await _manuTraySfcRecordRepository.GetPagedInfoAsync(manuTraySfcRecordPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuTraySfcRecordDto> manuTraySfcRecordDtos = PrepareManuTraySfcRecordDtos(pagedInfo);
            return new PagedInfo<ManuTraySfcRecordDto>(manuTraySfcRecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuTraySfcRecordDto> PrepareManuTraySfcRecordDtos(PagedInfo<ManuTraySfcRecordEntity>   pagedInfo)
        {
            var manuTraySfcRecordDtos = new List<ManuTraySfcRecordDto>();
            foreach (var manuTraySfcRecordEntity in pagedInfo.Data)
            {
                var manuTraySfcRecordDto = manuTraySfcRecordEntity.ToModel<ManuTraySfcRecordDto>();
                manuTraySfcRecordDtos.Add(manuTraySfcRecordDto);
            }

            return manuTraySfcRecordDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuTraySfcRecordModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuTraySfcRecordAsync(ManuTraySfcRecordModifyDto manuTraySfcRecordModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuTraySfcRecordModifyDto);

            //DTO转换实体
            var manuTraySfcRecordEntity = manuTraySfcRecordModifyDto.ToEntity<ManuTraySfcRecordEntity>();
            manuTraySfcRecordEntity.UpdatedBy = _currentUser.UserName;
            manuTraySfcRecordEntity.UpdatedOn = HymsonClock.Now();

            await _manuTraySfcRecordRepository.UpdateAsync(manuTraySfcRecordEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuTraySfcRecordDto> QueryManuTraySfcRecordByIdAsync(long id) 
        {
           var manuTraySfcRecordEntity = await _manuTraySfcRecordRepository.GetByIdAsync(id);
           if (manuTraySfcRecordEntity != null) 
           {
               return manuTraySfcRecordEntity.ToModel<ManuTraySfcRecordDto>();
           }
            return null;
        }
    }
}
