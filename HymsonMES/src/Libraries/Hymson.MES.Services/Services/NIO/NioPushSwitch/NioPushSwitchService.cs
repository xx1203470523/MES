using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.NIO;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.NioPushSwitch;
using Hymson.MES.Data.Repositories.NioPushSwitch.Query;
using Hymson.MES.Services.Dtos.NioPushSwitch;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.NioPushSwitch
{
    /// <summary>
    /// 服务（蔚来推送开关） 
    /// </summary>
    public class NioPushSwitchService : INioPushSwitchService
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
        private readonly AbstractValidator<NioPushSwitchSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（蔚来推送开关）
        /// </summary>
        private readonly INioPushSwitchRepository _nioPushSwitchRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="nioPushSwitchRepository"></param>
        public NioPushSwitchService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<NioPushSwitchSaveDto> validationSaveRules, 
            INioPushSwitchRepository nioPushSwitchRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _nioPushSwitchRepository = nioPushSwitchRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(NioPushSwitchSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<NioPushSwitchEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.Method = 1;
            //entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _nioPushSwitchRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(NioPushSwitchSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            //if(saveDto.IsEnabled == Core.Enums.TrueOrFalseEnum.Yes)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES17770));
            //}

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<NioPushSwitchEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _nioPushSwitchRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyEnableAsync(NioPushSwitchModifyDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // DTO转换实体
            NioPushSwitchEntity modifyModel = new NioPushSwitchEntity();
            modifyModel.Id = saveDto.Id;
            modifyModel.IsEnabled = saveDto.IsEnabled;
            modifyModel.UpdatedBy = _currentUser.UserName;
            modifyModel.UpdatedOn = HymsonClock.Now();

            return await _nioPushSwitchRepository.UpdateEnableAsync(modifyModel);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _nioPushSwitchRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _nioPushSwitchRepository.DeletesAsync(new DeleteCommand
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
        public async Task<NioPushSwitchDto?> QueryByIdAsync(long id) 
        {
           var nioPushSwitchEntity = await _nioPushSwitchRepository.GetByIdAsync(id);
           if (nioPushSwitchEntity == null) return null;
           
           return nioPushSwitchEntity.ToModel<NioPushSwitchDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<NioPushSwitchDto>> GetPagedListAsync(NioPushSwitchPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<NioPushSwitchPagedQuery>();
            //pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _nioPushSwitchRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<NioPushSwitchDto>());
            return new PagedInfo<NioPushSwitchDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
