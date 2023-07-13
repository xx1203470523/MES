/*
 *creator: Karl
 *
 *describe: 载具类型维护    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-12 10:37:17
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 载具类型维护 服务
    /// </summary>
    public class InteVehicleTypeService : IInteVehicleTypeService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 载具类型维护 仓储
        /// </summary>
        private readonly IInteVehicleTypeRepository _inteVehicleTypeRepository;
        private readonly AbstractValidator<InteVehicleTypeCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteVehicleTypeModifyDto> _validationModifyRules;

        public InteVehicleTypeService(ICurrentUser currentUser, ICurrentSite currentSite, IInteVehicleTypeRepository inteVehicleTypeRepository, AbstractValidator<InteVehicleTypeCreateDto> validationCreateRules, AbstractValidator<InteVehicleTypeModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteVehicleTypeRepository = inteVehicleTypeRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="inteVehicleTypeCreateDto"></param>
        /// <returns></returns>
        public async Task CreateInteVehicleTypeAsync(InteVehicleTypeCreateDto inteVehicleTypeCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(inteVehicleTypeCreateDto);

            //DTO转换实体
            var inteVehicleTypeEntity = inteVehicleTypeCreateDto.ToEntity<InteVehicleTypeEntity>();
            inteVehicleTypeEntity.Id= IdGenProvider.Instance.CreateId();
            inteVehicleTypeEntity.CreatedBy = _currentUser.UserName;
            inteVehicleTypeEntity.UpdatedBy = _currentUser.UserName;
            inteVehicleTypeEntity.CreatedOn = HymsonClock.Now();
            inteVehicleTypeEntity.UpdatedOn = HymsonClock.Now();
            inteVehicleTypeEntity.SiteId = _currentSite.SiteId ?? 0;

            //验证是否编码唯一
            var entity = await _inteVehicleTypeRepository.GetByCodeAsync( new InteVehicleTypeCodeQuery 
            { 
                Code =inteVehicleTypeEntity.Code.Trim(),
                SiteId=_currentSite.SiteId??0 
            });
            if (entity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18502));
            }

            //入库
            await _inteVehicleTypeRepository.InsertAsync(inteVehicleTypeEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteInteVehicleTypeAsync(long id)
        {
            await _inteVehicleTypeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteVehicleTypeAsync(long[] ids)
        {
            return await _inteVehicleTypeRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="inteVehicleTypePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteVehicleTypeDto>> GetPagedListAsync(InteVehicleTypePagedQueryDto inteVehicleTypePagedQueryDto)
        {
            var inteVehicleTypePagedQuery = inteVehicleTypePagedQueryDto.ToQuery<InteVehicleTypePagedQuery>();
            inteVehicleTypePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteVehicleTypeRepository.GetPagedInfoAsync(inteVehicleTypePagedQuery);

            //实体到DTO转换 装载数据
            List<InteVehicleTypeDto> inteVehicleTypeDtos = PrepareInteVehicleTypeDtos(pagedInfo);
            return new PagedInfo<InteVehicleTypeDto>(inteVehicleTypeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteVehicleTypeDto> PrepareInteVehicleTypeDtos(PagedInfo<InteVehicleTypeEntity>   pagedInfo)
        {
            var inteVehicleTypeDtos = new List<InteVehicleTypeDto>();
            foreach (var inteVehicleTypeEntity in pagedInfo.Data)
            {
                var inteVehicleTypeDto = inteVehicleTypeEntity.ToModel<InteVehicleTypeDto>();
                inteVehicleTypeDtos.Add(inteVehicleTypeDto);
            }

            return inteVehicleTypeDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteVehicleTypeModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyInteVehicleTypeAsync(InteVehicleTypeModifyDto inteVehicleTypeModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(inteVehicleTypeModifyDto);

            //DTO转换实体
            var inteVehicleTypeEntity = inteVehicleTypeModifyDto.ToEntity<InteVehicleTypeEntity>();
            inteVehicleTypeEntity.UpdatedBy = _currentUser.UserName;
            inteVehicleTypeEntity.UpdatedOn = HymsonClock.Now();

            await _inteVehicleTypeRepository.UpdateAsync(inteVehicleTypeEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteVehicleTypeDto> QueryInteVehicleTypeByIdAsync(long id) 
        {
           var inteVehicleTypeEntity = await _inteVehicleTypeRepository.GetByIdAsync(id);
           if (inteVehicleTypeEntity != null) 
           {
               return inteVehicleTypeEntity.ToModel<InteVehicleTypeDto>();
           }
           throw new CustomerValidationException(nameof(ErrorCode.MES18501));
        }
    }
}
