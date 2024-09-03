using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.NIO;
using Hymson.MES.Data.NIO;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.NIO;
using Hymson.MES.Data.Repositories.NIO.Query;
using Hymson.MES.Services.Dtos.NIO;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.NIO
{
    /// <summary>
    /// 服务（合作伙伴精益与生产能力） 
    /// </summary>
    public class NioPushProductioncapacityService : INioPushProductioncapacityService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 参数验证器
        /// </summary>
        //private readonly AbstractValidator<NioPushProductioncapacitySaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（合作伙伴精益与生产能力）
        /// </summary>
        private readonly INioPushProductioncapacityRepository _nioPushProductioncapacityRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly INioPushRepository _nioPushRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NioPushProductioncapacityService(ICurrentUser currentUser, ICurrentSite currentSite, 
            INioPushProductioncapacityRepository nioPushProductioncapacityRepository,
            INioPushRepository nioPushRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _nioPushProductioncapacityRepository = nioPushProductioncapacityRepository;
            _nioPushRepository = nioPushRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(NioPushProductioncapacitySaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            //await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<NioPushProductioncapacityEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            //entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _nioPushProductioncapacityRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(NioPushProductioncapacitySaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            if (string.IsNullOrEmpty(saveDto.Date) == false)
            {
                bool result = DateTime.TryParse(saveDto.Date, out _);
                if(result == false)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17771));
                }
            }

            if(saveDto.Efficiency < 0 || saveDto.Efficiency > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17772));
            }

            var dbModel = await _nioPushRepository.GetByIdAsync(saveDto.NioPushId);
            if (dbModel.Status == Core.Enums.Plan.PushStatusEnum.Success)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17773));
            }

            // 验证DTO
            //await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<NioPushProductioncapacityEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _nioPushProductioncapacityRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _nioPushProductioncapacityRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _nioPushProductioncapacityRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<NioPushProductioncapacityDto?> QueryByIdAsync(long id) 
        {
           var nioPushProductioncapacityEntity = await _nioPushProductioncapacityRepository.GetByIdAsync(id);
           if (nioPushProductioncapacityEntity == null) return null;
           
           return nioPushProductioncapacityEntity.ToModel<NioPushProductioncapacityDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<NioPushProductioncapacityDto>> GetPagedListAsync(NioPushProductioncapacityPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<NioPushProductioncapacityPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _nioPushProductioncapacityRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<NioPushProductioncapacityDto>());
            return new PagedInfo<NioPushProductioncapacityDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
