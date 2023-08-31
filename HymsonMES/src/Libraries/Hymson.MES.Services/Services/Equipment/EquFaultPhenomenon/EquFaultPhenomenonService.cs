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
using Hymson.MES.Data.Repositories.Equipment.EquFaultPhenomenon;
using Hymson.MES.Data.Repositories.Equipment.EquFaultPhenomenon.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Equipment.EquFaultPhenomenon
{
    /// <summary>
    /// 服务（设备故障现象）
    /// </summary>
    public class EquFaultPhenomenonService : IEquFaultPhenomenonService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<EquFaultPhenomenonSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储（设备故障现象）
        /// </summary>
        private readonly IEquFaultPhenomenonRepository _equFaultPhenomenonRepository;

        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equFaultPhenomenonRepository"></param>
        /// <param name="localizationService"></param>
        public EquFaultPhenomenonService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<EquFaultPhenomenonSaveDto> validationSaveRules,
            IEquFaultPhenomenonRepository equFaultPhenomenonRepository, ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equFaultPhenomenonRepository = equFaultPhenomenonRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 添加（设备故障现象）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquFaultPhenomenonSaveDto parm)
        {
            await _validationSaveRules.ValidateAndThrowAsync(parm);

            // 验证DTO
            parm.FaultPhenomenonCode = parm.FaultPhenomenonCode.ToTrimSpace();
            parm.FaultPhenomenonCode = parm.FaultPhenomenonCode.ToUpperInvariant();

            if (parm.EquipmentGroupId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12904));
            }

            // DTO转换实体
            var entity = parm.ToEntity<EquFaultPhenomenonEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId;

            entity.UseStatus = SysDataStatusEnum.Build;

            // 编码唯一性验证
            var checkEntity = await _equFaultPhenomenonRepository.GetByCodeAsync(new EntityByCodeQuery { Site = entity.SiteId, Code = entity.FaultPhenomenonCode });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES12900)).WithData("Code", entity.FaultPhenomenonCode);

            // 保存实体
            return await _equFaultPhenomenonRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改（设备故障现象）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquFaultPhenomenonSaveDto parm)
        {
            await _validationSaveRules.ValidateAndThrowAsync(parm);

            // 验证DTO
            var entityOld = await _equFaultPhenomenonRepository.GetByIdAsync(parm.Id.Value)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES12905));

            //if (entityOld.UseStatus != SysDataStatusEnum.Build && parm.UseStatus == SysDataStatusEnum.Build)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES10108));
            //}
            //验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == entityOld.UseStatus))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            // DTO转换实体
            var entity = parm.ToEntity<EquFaultPhenomenonEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            return await _equFaultPhenomenonRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除（设备故障现象）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            var entities = await _equFaultPhenomenonRepository.GetByIdsAsync(idsArr);
            if (entities != null && entities.Any(a => a.UseStatus != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            return await _equFaultPhenomenonRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 查询列表（设备故障现象）
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquFaultPhenomenonDto>> GetPagedListAsync(EquFaultPhenomenonPagedQueryDto parm)
        {
            var pagedQuery = parm.ToQuery<EquFaultPhenomenonPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _equFaultPhenomenonRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquFaultPhenomenonDto>());
            return new PagedInfo<EquFaultPhenomenonDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquFaultPhenomenonDto> GetDetailAsync(long id)
        {
            return (await _equFaultPhenomenonRepository.GetViewByIdAsync(id)).ToModel<EquFaultPhenomenonDto>();
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
            var entity = await _equFaultPhenomenonRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12905));
            }
            if (entity.UseStatus == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.UseStatus)}"));
            }
            #endregion

            #region 操作数据库
            await _equFaultPhenomenonRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion
    }
}
