using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Services.Dtos.Common;
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

        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equFaultReasonRepository"></param>
        /// <param name="localizationService"></param>
        public EquFaultReasonService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<EquFaultReasonSaveDto> validationSaveRules,
            IEquFaultReasonRepository equFaultReasonRepository, ILocalizationService localizationService)
        {
            _currentSite = currentSite;
            _currentUser = currentUser;
            _validationSaveRules = validationSaveRules;
            _equFaultReasonRepository = equFaultReasonRepository;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="EquFaultReasonCreateDto"></param>
        /// <returns></returns>
        public async Task<int> CreateEquFaultReasonAsync(EquFaultReasonSaveDto EquFaultReasonCreateDto)
        {
            // 验证DTO
            EquFaultReasonCreateDto.FaultReasonCode = EquFaultReasonCreateDto.FaultReasonCode.ToTrimSpace();
            EquFaultReasonCreateDto.FaultReasonCode = EquFaultReasonCreateDto.FaultReasonCode.ToUpperInvariant();
            await _validationSaveRules.ValidateAndThrowAsync(EquFaultReasonCreateDto);

            // DTO转换实体
            var entity = EquFaultReasonCreateDto.ToEntity<EquFaultReasonEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId;

            entity.UseStatus = SysDataStatusEnum.Build;

            // 编码唯一性验证
            var checkEntity = await _equFaultReasonRepository.GetByCodeAsync(new EntityByCodeQuery { Site = entity.SiteId, Code = entity.FaultReasonCode });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES13011)).WithData("Code", entity.FaultReasonCode);

            // 保存实体
            return await _equFaultReasonRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="EquFaultReasonModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyEquFaultReasonAsync(EquFaultReasonSaveDto EquFaultReasonModifyDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(EquFaultReasonModifyDto);

            var entityOld = await _equFaultReasonRepository.GetByIdAsync(EquFaultReasonModifyDto.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES13013));
            //验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == entityOld.UseStatus))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            // DTO转换实体
            var entity = EquFaultReasonModifyDto.ToEntity<EquFaultReasonEntity>();
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
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquFaultReasonAsync(long[] idsArr)
        {
            var entities = await _equFaultReasonRepository.GetByIdsAsync(idsArr);
            if (entities != null && entities.Any(a => a.UseStatus != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            return await _equFaultReasonRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
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

        #region 状态变更
        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(ChangeStatusDto param)
        {
            #region 参数校验
            if (param.Id == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10125));
            }
            if (!Enum.IsDefined(typeof(SysDataStatusEnum), param.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10126));
            }
            if (param.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10128));
            }

            #endregion

            var changeStatusCommand = new ChangeStatusCommand()
            {
                Id = param.Id,
                Status = param.Status,

                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            #region 校验数据
            var entity = await _equFaultReasonRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13013));
            }
            if (entity.UseStatus == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.UseStatus)}"));
            }
            #endregion

            #region 操作数据库
            await _equFaultReasonRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion
    }
}
