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
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务（自定义字段） 
    /// </summary>
    public class InteCustomFieldService : IInteCustomFieldService
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
        private readonly AbstractValidator<InteCustomFieldSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（自定义字段）
        /// </summary>
        private readonly IInteCustomFieldRepository _inteCustomFieldRepository;

        private readonly IInteCustomFieldInternationalizationRepository _inteCustomFieldInternationalizationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="inteCustomFieldRepository"></param>
        /// <param name="inteCustomFieldInternationalizationRepository"></param>
        public InteCustomFieldService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<InteCustomFieldSaveDto> validationSaveRules, 
            IInteCustomFieldRepository inteCustomFieldRepository, IInteCustomFieldInternationalizationRepository inteCustomFieldInternationalizationRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _inteCustomFieldRepository = inteCustomFieldRepository;
            _inteCustomFieldInternationalizationRepository = inteCustomFieldInternationalizationRepository;
        }

        /// <summary>
        /// 创建或更新
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateAsync(InteCustomFieldSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<InteCustomFieldEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _inteCustomFieldRepository.InsertAsync(entity);
        }
    }
}
