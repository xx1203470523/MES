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
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

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

        /// <summary>
        /// 多语言服务
        /// </summary>
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
            IEquFaultPhenomenonRepository equFaultPhenomenonRepository,
            ILocalizationService localizationService)
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
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquFaultPhenomenonSaveDto saveDto)
        {
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 验证DTO
            saveDto.Code = saveDto.Code.ToTrimSpace();
            saveDto.Code = saveDto.Code.ToUpperInvariant();

            //if (parm.EquipmentGroupId == 0)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES12904));
            //}

            // DTO转换实体
            var entity = saveDto.ToEntity<EquFaultPhenomenonEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId;
            entity.Status = SysDataStatusEnum.Build;

            //增加设备故障原因
            var equFaultReasonPhenomenonEntities = new List<EquFaultPhenomenonReasonRelationEntity>();
            if (saveDto.EquFaultReasonList != null && saveDto.EquFaultReasonList.Any())
            {
                foreach (var item in saveDto.EquFaultReasonList)
                {
                    var model = new EquFaultPhenomenonReasonRelationEntity();
                    model.Id = IdGenProvider.Instance.CreateId();
                    model.CreatedBy = _currentUser.UserName;
                    model.UpdatedBy = _currentUser.UserName;
                    //model.SiteId = _currentSite.SiteId;
                    model.FaultReasonId = item;
                    model.FaultPhenomenonId = entity.Id;

                    equFaultReasonPhenomenonEntities.Add(model);
                }
            }


            // 编码唯一性验证
            var checkEntity = await _equFaultPhenomenonRepository.GetByCodeAsync(new EntityByCodeQuery { Site = entity.SiteId, Code = entity.Code });
            if (checkEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12900)).WithData("Code", entity.Code);
            }

            using var scope = TransactionHelper.GetTransactionScope();

            if (equFaultReasonPhenomenonEntities.Any())
            {
                var insertFaultReasonPhenomenonResult = await _equFaultPhenomenonRepository.InsertFaultReasonAsync(equFaultReasonPhenomenonEntities);
                if (insertFaultReasonPhenomenonResult != equFaultReasonPhenomenonEntities.Count)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12908));
                }
            }

            var insertFaultPhenomenon = await _equFaultPhenomenonRepository.InsertAsync(entity);
            if (insertFaultPhenomenon == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12908));
            }

            scope.Complete();

            return insertFaultPhenomenon;
        }

        /// <summary>
        /// 修改（设备故障现象）
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquFaultPhenomenonSaveDto saveDto)
        {
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 验证DTO
            var entityOld = await _equFaultPhenomenonRepository.GetByIdAsync(saveDto.Id.Value)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES12905));

            //验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == entityOld.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            //处理设备故障原因
            var equFaultReasonPhenomenonEntities = new List<EquFaultPhenomenonReasonRelationEntity>();
            if (saveDto.EquFaultReasonList != null && saveDto.EquFaultReasonList.Any())
            {
                //增加设备故障原因

                foreach (var item in saveDto.EquFaultReasonList)
                {
                    var model = new EquFaultPhenomenonReasonRelationEntity();
                    model.Id = IdGenProvider.Instance.CreateId();
                    model.CreatedBy = _currentUser.UserName;
                    model.UpdatedBy = _currentUser.UserName;
                    //model.SiteId = _currentSite.SiteId;
                    model.FaultReasonId = item;
                    model.FaultPhenomenonId = saveDto.Id.GetValueOrDefault();

                    equFaultReasonPhenomenonEntities.Add(model);
                }

            }

            using var scope = TransactionHelper.GetTransactionScope();

            if (saveDto.EquFaultReasonList != null && saveDto.EquFaultReasonList.Any())
            {
                var delreult = await _equFaultPhenomenonRepository.DeleteEquFaultReasonPhenomenonRelationsAsync(new DeleteCommand { Ids = new List<long> { saveDto.Id.Value } });

                if (delreult == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12909));
                }
            }
            if (equFaultReasonPhenomenonEntities.Any())
            {
                var insertFaultReasonPhenomenonResult = await _equFaultPhenomenonRepository.InsertFaultReasonAsync(equFaultReasonPhenomenonEntities);
                if (insertFaultReasonPhenomenonResult != equFaultReasonPhenomenonEntities.Count)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES12909));
                }
            }

            var entity = saveDto.ToEntity<EquFaultPhenomenonEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            var updateResult = await _equFaultPhenomenonRepository.UpdateAsync(entity);
            if (updateResult == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12909));
            }

            scope.Complete();
            //// DTO转换实体
            //var entity = parm.ToEntity<EquFaultPhenomenonEntity>();
            //entity.UpdatedBy = _currentUser.UserName;

            return updateResult;
        }

        /// <summary>
        /// 删除（设备故障现象）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            var entities = await _equFaultPhenomenonRepository.GetByIdsAsync(idsArr);
            if (entities != null && entities.Any(a => a.Status != SysDataStatusEnum.Build))
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
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquFaultPhenomenonDto>> GetPagedListAsync(EquFaultPhenomenonPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquFaultPhenomenonPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equFaultPhenomenonRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquFaultPhenomenonDto>());
            return new PagedInfo<EquFaultPhenomenonDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquFaultPhenomenonDto> QueryByIdAsync(long id)
        {
            var date = await _equFaultPhenomenonRepository.GetByIdAsync(id);
            var result = date.ToModel<EquFaultPhenomenonDto>();

            //获取已分配故障原因
            var changedFaultReasonList = await _equFaultPhenomenonRepository.GetEquFaultReasonListAsync(new EquFaultPhenomenonQuery { Id = id });
            result.EquFaultReasonList = changedFaultReasonList?.Select(a => a.FaultReasonId);

            return result;
        }


        #region 状态变更
        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="statusDto"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(ChangeStatusDto statusDto)
        {
            if (statusDto.Id == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10125));

            // 校验状态是否在枚举里面
            if (!Enum.IsDefined(typeof(SysDataStatusEnum), statusDto.Status)) throw new CustomerValidationException(nameof(ErrorCode.MES10126));
            // 不能重新设置为新建
            if (statusDto.Status == SysDataStatusEnum.Build) throw new CustomerValidationException(nameof(ErrorCode.MES10128));

            // 读取当前实时信息
            var entity = await _equFaultPhenomenonRepository.GetByIdAsync(statusDto.Id);
            if (entity == null || entity.IsDeleted != 0) throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            if (entity.Status == statusDto.Status) throw new CustomerValidationException(nameof(ErrorCode.MES10127))
                    .WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));

            await _equFaultPhenomenonRepository.UpdateStatusAsync(new ChangeStatusCommand()
            {
                Id = statusDto.Id,
                Status = statusDto.Status,

                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            });
        }

        #endregion

    }
}
