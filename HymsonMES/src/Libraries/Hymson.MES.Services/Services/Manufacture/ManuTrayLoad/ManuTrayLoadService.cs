/*
 *creator: Karl
 *
 *describe: 托盘装载信息表    服务 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-16 11:10:43
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
    /// 托盘装载信息表 服务
    /// </summary>
    public class ManuTrayLoadService : IManuTrayLoadService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 托盘装载信息表 仓储
        /// </summary>
        private readonly IManuTrayLoadRepository _manuTrayLoadRepository;
        private readonly AbstractValidator<ManuTrayLoadCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuTrayLoadModifyDto> _validationModifyRules;

        public ManuTrayLoadService(ICurrentUser currentUser, ICurrentSite currentSite, IManuTrayLoadRepository manuTrayLoadRepository, AbstractValidator<ManuTrayLoadCreateDto> validationCreateRules, AbstractValidator<ManuTrayLoadModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuTrayLoadRepository = manuTrayLoadRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuTrayLoadCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuTrayLoadAsync(ManuTrayLoadCreateDto manuTrayLoadCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuTrayLoadCreateDto);

            //DTO转换实体
            var manuTrayLoadEntity = manuTrayLoadCreateDto.ToEntity<ManuTrayLoadEntity>();
            manuTrayLoadEntity.Id= IdGenProvider.Instance.CreateId();
            manuTrayLoadEntity.CreatedBy = _currentUser.UserName;
            manuTrayLoadEntity.UpdatedBy = _currentUser.UserName;
            manuTrayLoadEntity.CreatedOn = HymsonClock.Now();
            manuTrayLoadEntity.UpdatedOn = HymsonClock.Now();
            manuTrayLoadEntity.SiteId = _currentSite.SiteId ?? 123456;

            //入库
            await _manuTrayLoadRepository.InsertAsync(manuTrayLoadEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuTrayLoadAsync(long id)
        {
            await _manuTrayLoadRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuTrayLoadAsync(long[] ids)
        {
            return await _manuTrayLoadRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuTrayLoadPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuTrayLoadDto>> GetPagedListAsync(ManuTrayLoadPagedQueryDto manuTrayLoadPagedQueryDto)
        {
            var manuTrayLoadPagedQuery = manuTrayLoadPagedQueryDto.ToQuery<ManuTrayLoadPagedQuery>();
            var pagedInfo = await _manuTrayLoadRepository.GetPagedInfoAsync(manuTrayLoadPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuTrayLoadDto> manuTrayLoadDtos = PrepareManuTrayLoadDtos(pagedInfo);
            return new PagedInfo<ManuTrayLoadDto>(manuTrayLoadDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuTrayLoadDto> PrepareManuTrayLoadDtos(PagedInfo<ManuTrayLoadEntity>   pagedInfo)
        {
            var manuTrayLoadDtos = new List<ManuTrayLoadDto>();
            foreach (var manuTrayLoadEntity in pagedInfo.Data)
            {
                var manuTrayLoadDto = manuTrayLoadEntity.ToModel<ManuTrayLoadDto>();
                manuTrayLoadDtos.Add(manuTrayLoadDto);
            }

            return manuTrayLoadDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuTrayLoadModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuTrayLoadAsync(ManuTrayLoadModifyDto manuTrayLoadModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuTrayLoadModifyDto);

            //DTO转换实体
            var manuTrayLoadEntity = manuTrayLoadModifyDto.ToEntity<ManuTrayLoadEntity>();
            manuTrayLoadEntity.UpdatedBy = _currentUser.UserName;
            manuTrayLoadEntity.UpdatedOn = HymsonClock.Now();

            await _manuTrayLoadRepository.UpdateAsync(manuTrayLoadEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuTrayLoadDto> QueryManuTrayLoadByIdAsync(long id) 
        {
           var manuTrayLoadEntity = await _manuTrayLoadRepository.GetByIdAsync(id);
           if (manuTrayLoadEntity != null) 
           {
               return manuTrayLoadEntity.ToModel<ManuTrayLoadDto>();
           }
            return null;
        }
    }
}
