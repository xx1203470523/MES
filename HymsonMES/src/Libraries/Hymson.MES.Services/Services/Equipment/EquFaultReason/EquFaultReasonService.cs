using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 设备故障原因表 服务
    /// </summary>
    public class EquFaultReasonService : IEquFaultReasonService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备故障原因表 仓储
        /// </summary>
        private readonly IEquFaultReasonRepository _equFaultReasonRepository;
        private readonly AbstractValidator<EquFaultReasonSaveDto> _validationSaveRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equFaultReasonRepository"></param>
        public EquFaultReasonService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<EquFaultReasonSaveDto> validationSaveRules,
            IEquFaultReasonRepository equFaultReasonRepository)
        {
            _currentSite = currentSite;
            _currentUser = currentUser;
            _validationSaveRules = validationSaveRules;
            _equFaultReasonRepository = equFaultReasonRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateEquFaultReasonAsync(EquFaultReasonSaveDto createDto)
        {
            // 验证DTO
            createDto.FaultReasonCode = createDto.FaultReasonCode.ToTrimSpace();
            createDto.FaultReasonCode = createDto.FaultReasonCode.ToUpperInvariant();
            await _validationSaveRules.ValidateAndThrowAsync(createDto);

            // DTO转换实体
            var entity = createDto.ToEntity<EquFaultReasonEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId;

            // 编码唯一性验证
            var checkEntity = await _equFaultReasonRepository.GetByCodeAsync(new EntityByCodeQuery { Site = entity.SiteId, Code = entity.FaultReasonCode });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES13011)).WithData("Code", entity.FaultReasonCode);

            // 保存实体
            return await _equFaultReasonRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task ModifyEquFaultReasonAsync(EquFaultReasonSaveDto modifyDto)
        {
            if (modifyDto == null) throw new ValidationException(ErrorCode.MES13003);

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(modifyDto);

            // DTO转换实体
            var entity = modifyDto.ToEntity<EquFaultReasonEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            entity.SiteId = _currentSite.SiteId;

            // 更新实体
            await _equFaultReasonRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEquFaultReasonAsync(long id)
        {
            await _equFaultReasonRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquFaultReasonAsync(long[] ids)
        {
            if (ids == null || ids.Any() == false)
            {
                throw new ValidationException(ErrorCode.MES13005);
            }

            return await _equFaultReasonRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="EquFaultReasonPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquFaultReasonDto>> GetPageListAsync(EquFaultReasonPagedQueryDto EquFaultReasonPagedQueryDto)
        {
            var EquFaultReasonPagedQuery = EquFaultReasonPagedQueryDto.ToQuery<EquFaultReasonPagedQuery>();
            EquFaultReasonPagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _equFaultReasonRepository.GetPagedInfoAsync(EquFaultReasonPagedQuery);

            //实体到DTO转换 装载数据
            List<EquFaultReasonDto> EquFaultReasonDtos = PrepareEquFaultReasonDtos(pagedInfo).ToList();
            return new PagedInfo<EquFaultReasonDto>(EquFaultReasonDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static IEnumerable<EquFaultReasonDto> PrepareEquFaultReasonDtos(PagedInfo<EquFaultReasonEntity> pagedInfo)
        {
            var EquFaultReasonDtos = new List<EquFaultReasonDto>();
            foreach (var EquFaultReasonEntity in pagedInfo.Data)
            {
                var EquFaultReasonDto = EquFaultReasonEntity.ToModel<EquFaultReasonDto>();
                EquFaultReasonDtos.Add(EquFaultReasonDto);
            }

            return EquFaultReasonDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquFaultReasonDto> QueryEquFaultReasonByIdAsync(long id)
        {
            var EquFaultReasonEntity = await _equFaultReasonRepository.GetByIdAsync(id);
            var dto = EquFaultReasonEntity.ToModel<CustomEquFaultReasonDto>();
            return dto;
        }
    }
}
