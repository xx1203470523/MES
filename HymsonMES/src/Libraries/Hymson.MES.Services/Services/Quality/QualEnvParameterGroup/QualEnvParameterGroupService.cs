using FluentValidation;
using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（环境检验参数表） 
    /// </summary>
    public class QualEnvParameterGroupService : IQualEnvParameterGroupService
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
        private readonly AbstractValidator<QualEnvParameterGroupSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（环境检验参数表）
        /// </summary>
        private readonly IQualEnvParameterGroupRepository _qualEnvParameterGroupRepository;

        /// <summary>
        /// 仓储接口（环境检验参数项目表）
        /// </summary>
        private readonly IQualEnvParameterGroupDetailRepository _qualEnvParameterGroupDetailRepository;

        /// <summary>
        /// 仓储接口（工作中心）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 仓储接口（工序）
        /// </summary>
        private readonly IProcProcedureRepository _procProcedureRepository;

        /// <summary>
        /// 仓储接口（标准参数）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 国际化服务
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="qualEnvParameterGroupRepository"></param>
        /// <param name="qualEnvParameterGroupDetailRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="procProcedureRepository"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="localizationService"></param>
        public QualEnvParameterGroupService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<QualEnvParameterGroupSaveDto> validationSaveRules,
            IQualEnvParameterGroupRepository qualEnvParameterGroupRepository,
            IQualEnvParameterGroupDetailRepository qualEnvParameterGroupDetailRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcParameterRepository procParameterRepository,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _qualEnvParameterGroupRepository = qualEnvParameterGroupRepository;
            _qualEnvParameterGroupDetailRepository = qualEnvParameterGroupDetailRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procProcedureRepository = procProcedureRepository;
            _procParameterRepository = procParameterRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(QualEnvParameterGroupSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<QualEnvParameterGroupEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            entity.Status = SysDataStatusEnum.Build;

            // 验证唯一性
            await CheckUniqueWorkCenterProcedureAsync(entity);

            // 验证参数项目
            CheckGroupDetails(saveDto.Details);

            var details = saveDto.Details.Select(s =>
            {
                var detailEntity = s.ToEntity<QualEnvParameterGroupDetailEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.ParameterGroupId = entity.Id;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;
                detailEntity.SiteId = entity.SiteId;

                return detailEntity;
            });

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows = await _qualEnvParameterGroupRepository.InsertAsync(entity);
                if (rows <= 0)
                {
                    trans.Dispose();
                }
                else
                {
                    rows += await _qualEnvParameterGroupDetailRepository.InsertRangeAsync(details);
                    trans.Complete();
                }
            }
            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(QualEnvParameterGroupSaveDto saveDto)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<QualEnvParameterGroupEntity>();
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 检查数据之前的状态是否允许修改
            var dbEntity = await _qualEnvParameterGroupRepository.GetByIdAsync(entity.Id) ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            //验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == dbEntity.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            // 验证唯一性
            await CheckUniqueWorkCenterProcedureAsync(entity);

            // 验证参数项目
            CheckGroupDetails(saveDto.Details);

            var details = saveDto.Details.Select(s =>
            {
                var detailEntity = s.ToEntity<QualEnvParameterGroupDetailEntity>();
                detailEntity.Id = IdGenProvider.Instance.CreateId();
                detailEntity.ParameterGroupId = entity.Id;
                detailEntity.CreatedBy = updatedBy;
                detailEntity.CreatedOn = updatedOn;
                detailEntity.UpdatedBy = updatedBy;
                detailEntity.UpdatedOn = updatedOn;
                detailEntity.SiteId = entity.SiteId;

                return detailEntity;
            });

            var command = new DeleteByParentIdCommand
            {
                ParentId = entity.Id,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualEnvParameterGroupRepository.UpdateAsync(entity);
                rows += await _qualEnvParameterGroupDetailRepository.DeleteByParentIdAsync(command);
                rows += await _qualEnvParameterGroupDetailRepository.InsertRangeAsync(details);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _qualEnvParameterGroupRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            var list = await _qualEnvParameterGroupRepository.GetByIdsAsync(ids);
            if (list != null && list.Any(x => x.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES12509));
            }

            return await _qualEnvParameterGroupRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualEnvParameterGroupInfoDto?> QueryByIdAsync(long id)
        {
            var qualEnvParameterGroupEntity = await _qualEnvParameterGroupRepository.GetByIdAsync(id);
            if (qualEnvParameterGroupEntity == null) return null;

            var dto = qualEnvParameterGroupEntity.ToModel<QualEnvParameterGroupInfoDto>();
            if (dto == null) return dto;

            var workCenterTask = _inteWorkCenterRepository.GetByIdAsync(dto.WorkCenterId);
            var procedureTask = _procProcedureRepository.GetByIdAsync(dto.ProcedureId);

            var workCenterEntity = await workCenterTask;
            var procedureEntity = await procedureTask;

            if (workCenterEntity != null) dto.WorkCenterCode = workCenterEntity.Code;
            if (procedureEntity != null) dto.ProcedureCode = procedureEntity.Code;

            return dto;
        }

        /// <summary>
        /// 根据ID获取关联明细列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualEnvParameterGroupDetailDto>> QueryDetailsByMainIdAsync(long id)
        {
            var details = await _qualEnvParameterGroupDetailRepository.GetEntitiesAsync(new QualEnvParameterGroupDetailQuery
            {
                ParameterGroupId = id
            });

            // 查询已经缓存的参数实体
            var parameterEntities = await _procParameterRepository.GetProcParameterEntitiesAsync(new ProcParameterQuery
            {
                SiteId = _currentSite.SiteId ?? 0
            });

            List<QualEnvParameterGroupDetailDto> dtos = new();
            foreach (var item in details)
            {
                var dto = item.ToModel<QualEnvParameterGroupDetailDto>();
                var parameterEntity = parameterEntities.FirstOrDefault(f => f.Id == item.ParameterId);
                if (dto == null) continue;

                if (parameterEntity != null)
                {
                    dto.Code = parameterEntity.ParameterCode;
                    dto.Name = parameterEntity.ParameterName;
                    dto.Unit = parameterEntity.ParameterUnit ?? "";
                    dto.DataType = parameterEntity.DataType;
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualEnvParameterGroupDto>> GetPagedListAsync(QualEnvParameterGroupPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualEnvParameterGroupPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualEnvParameterGroupRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<QualEnvParameterGroupDto>());
            return new PagedInfo<QualEnvParameterGroupDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }



        #region 内部方法
        /// <summary>
        /// 验证工作中心工序唯一性
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task CheckUniqueWorkCenterProcedureAsync(QualEnvParameterGroupEntity entity)
        {
            // 编码唯一性验证
            var checkEntity = await _qualEnvParameterGroupRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.Code,
                Version = entity.Version
            });
            if (checkEntity != null && checkEntity.Id != entity.Id) throw new CustomerValidationException(nameof(ErrorCode.MES10520))
                    .WithData("Code", entity.Code)
                    .WithData("Version", entity.Version);

            var workCenterEntity = await _inteWorkCenterRepository.GetByIdAsync(entity.WorkCenterId);
            var procedureEntity = await _procProcedureRepository.GetByIdAsync(entity.ProcedureId);
            if (workCenterEntity == null || procedureEntity == null) return;

            var checkUniqueWorkCenterProcedureEntities = await _qualEnvParameterGroupRepository.GetByWorkCenterProcedureListAsync(new EntityByWorkCenterProcedureQuery
            {
                SiteId = entity.SiteId,
                WorkCenterId = entity.WorkCenterId,
                ProcedureId = entity.ProcedureId
            });
            if (checkUniqueWorkCenterProcedureEntities == null || !checkUniqueWorkCenterProcedureEntities.Any()) return;

            // 校验工作中心编码+工序编码+版本是否唯一
            if (checkUniqueWorkCenterProcedureEntities.Any(a => a.WorkCenterId == entity.WorkCenterId
            && a.ProcedureId == entity.ProcedureId
            && a.Version == entity.Version
            && a.Id != entity.Id))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10525))
                    .WithData("WorkCenterCode", workCenterEntity.Code)
                    .WithData("ProcedureCode", $"{procedureEntity.Code}({procedureEntity.Name})")
                    .WithData("Version", entity.Version);
            }

            // 状态为启用时，校验启用状态的 工作中心编码+工序编码 唯一性
            if (entity.Status == SysDataStatusEnum.Enable && checkUniqueWorkCenterProcedureEntities.Any(a => a.WorkCenterId == entity.WorkCenterId
            && a.ProcedureId == entity.ProcedureId
            && a.Status == entity.Status
            && a.Id != entity.Id))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10524))
                    .WithData("WorkCenterCode", workCenterEntity.Code)
                    .WithData("ProcedureCode", $"{procedureEntity.Code}({procedureEntity.Name})");
            }
        }

        /// <summary>
        /// 验证参数项目
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        public static void CheckGroupDetails(IEnumerable<QualEnvParameterGroupDetailSaveDto> details)
        {
            // 判断规格上限和规格下限（数据类型为数值）
            List<ValidationFailure> validationFailures = new();
            foreach (var item in details)
            {
                // 如果参数类型为数值，则判断规格上限和规格下限
                if (item.DataType != DataTypeEnum.Numeric) continue;
                if (item.UpperLimit < item.LowerLimit)
                {
                    validationFailures.Add(new ValidationFailure
                    {
                        FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                        { "CollectionIndex", item.Code },
                        { "Code", item.Code }
                    },
                        ErrorCode = nameof(ErrorCode.MES10516)
                    });
                }
            }

            // 是否存在错误
            if (validationFailures.Any())
            {
                throw new ValidationException("", validationFailures);
            }
        }
        #endregion

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
            var entity = await _qualEnvParameterGroupRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }

            // 验证唯一性
            entity.Status = changeStatusCommand.Status;
            await CheckUniqueWorkCenterProcedureAsync(entity);
            #endregion

            #region 操作数据库
            await _qualEnvParameterGroupRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion
    }
}
