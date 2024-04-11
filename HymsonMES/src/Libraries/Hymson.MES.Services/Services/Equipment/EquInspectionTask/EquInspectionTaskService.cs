using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Reactive;
using System.Transactions;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（点检任务） 
    /// </summary>
    public class EquInspectionTaskService : IEquInspectionTaskService
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
        private readonly AbstractValidator<EquInspectionTaskSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（点检任务）
        /// </summary>
        private readonly IEquInspectionTaskRepository _equInspectionTaskRepository;

        /// <summary>
        /// 仓储接口（点检任务）
        /// </summary>
        private readonly IEquInspectionTaskDetailsRepository _taskDetailsRepository;

        /// <summary>
        /// 仓储接口（点检项目）
        /// </summary>
        private readonly IEquInspectionItemRepository _inspectionItemRepository;

        /// <summary>
        /// 多语言服务
        /// </summary>
        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquInspectionTaskService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<EquInspectionTaskSaveDto> validationSaveRules,
            IEquInspectionTaskRepository equInspectionTaskRepository,
            IEquInspectionTaskDetailsRepository taskDetailsRepository,
            IEquInspectionItemRepository inspectionItemRepository,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equInspectionTaskRepository = equInspectionTaskRepository;
            _taskDetailsRepository = taskDetailsRepository;
            _inspectionItemRepository = inspectionItemRepository;
            _localizationService = localizationService;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<long> CreateAsync(EquInspectionTaskSaveDto saveDto)
        {
            #region 验证
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO
            saveDto.Code = saveDto.Code.ToTrimSpace().ToUpperInvariant();
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            var siteId = _currentSite.SiteId ?? 0;
            //判断设备点检项目在系统中是否已经存在
            var inspectionItemQuery = new EquInspectionTaskQuery { SiteId = siteId, Code = saveDto.Code };
            var equInspectionItems = await _equInspectionTaskRepository.GetEntitiesAsync(inspectionItemQuery);
            if (equInspectionItems != null && equInspectionItems.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15802)).WithData("code", saveDto.Code);
            }

            //验证关联项目不能重复
            //上下限验证
            #endregion

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquInspectionTaskEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.Status = Core.Enums.SysDataStatusEnum.Build;
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            //判断关联项目
            List<EquInspectionTaskDetailsEntity> taskDetailsEntities = new List<EquInspectionTaskDetailsEntity>();
            foreach (var detail in saveDto.TaskDetailsSaveDtos)
            {
                taskDetailsEntities.Add(new EquInspectionTaskDetailsEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    InspectionTaskId = entity.Id,
                    InspectionItemId = detail.InspectionItemId,
                    BaseValue = detail.BaseValue,
                    MaxValue = detail.MaxValue,
                    MinValue = detail.MinValue,
                    Unit = detail.Unit ?? "",
                    Remark = detail.Remark ?? "",
                    CreatedBy = updatedBy,
                    UpdatedBy = updatedBy,
                    SiteId = _currentSite.SiteId ?? 0
                });
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                // 保存
                await _equInspectionTaskRepository.InsertAsync(entity);

                if (taskDetailsEntities != null && taskDetailsEntities.Count > 0)
                {
                    await _taskDetailsRepository.InsertRangeAsync(taskDetailsEntities);
                }

                ts.Complete();
            }

            return entity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<long> ModifyAsync(EquInspectionTaskSaveDto saveDto)
        {
            #region 验证

            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);
            #endregion

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            // DTO转换实体
            var entity = saveDto.ToEntity<EquInspectionTaskEntity>();
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            //判断关联项目
            List<EquInspectionTaskDetailsEntity> taskDetailsEntities = new List<EquInspectionTaskDetailsEntity>();
            foreach (var detail in saveDto.TaskDetailsSaveDtos)
            {
                taskDetailsEntities.Add(new EquInspectionTaskDetailsEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    InspectionTaskId = entity.Id,
                    InspectionItemId = detail.InspectionItemId,
                    BaseValue = detail.BaseValue,
                    MaxValue = detail.MaxValue,
                    MinValue = detail.MinValue,
                    Unit = detail.Unit ?? "",
                    Remark = detail.Remark ?? "",
                    CreatedBy = updatedBy,
                    UpdatedBy = updatedBy,
                    SiteId = _currentSite.SiteId ?? 0
                });
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                // 保存
                await _equInspectionTaskRepository.UpdateAsync(entity);

                //删除之前的数据
                await _taskDetailsRepository.DeleteByTaskIdAsync(saveDto.Id);
                if (taskDetailsEntities != null && taskDetailsEntities.Count > 0)
                {
                    await _taskDetailsRepository.InsertRangeAsync(taskDetailsEntities);
                }

                ts.Complete();
            }
            return entity.Id;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equInspectionTaskRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            if (idsArr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            var entitys = await _equInspectionTaskRepository.GetByIdsAsync(idsArr);
            if (entitys != null && entitys.Any(a => a.Status != (int)SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            }

            var command = new DeleteCommand
            {
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now(),
                Ids = idsArr
            };
            int rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                rows += await _equInspectionTaskRepository.DeletesAsync(command);
                rows += await _taskDetailsRepository.DeleteByTaskIdRangeAsync(idsArr);
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquInspectionTaskDto?> QueryByIdAsync(long id)
        {
            var equInspectionTaskEntity = await _equInspectionTaskRepository.GetByIdAsync(id);
            if (equInspectionTaskEntity == null) return null;

            return equInspectionTaskEntity.ToModel<EquInspectionTaskDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquInspectionTaskDto>> GetPagedListAsync(EquInspectionTaskPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquInspectionTaskPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equInspectionTaskRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquInspectionTaskDto>());
            return new PagedInfo<EquInspectionTaskDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询点检任务详情
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquInspectionTaskDetailDto>> QueryEquipmentsByResourceIdAsync(long taskId)
        {
            var equInspectionTaskDetails = await _taskDetailsRepository.GetEntitiesAsync(new EquInspectionTaskDetailsQuery { InspectionTaskId = taskId });

            List<EquInspectionTaskDetailDto> taskDetailDtos = new List<EquInspectionTaskDetailDto>();
            if (equInspectionTaskDetails == null || !equInspectionTaskDetails.Any())
            {
                return taskDetailDtos;
            }

            var itemIds = equInspectionTaskDetails.Select(s => s.InspectionTaskId.GetValueOrDefault()).ToArray();
            var itemEntities = await _inspectionItemRepository.GetByIdsAsync(itemIds);

            foreach (var entity in equInspectionTaskDetails)
            {
                var item = itemEntities.FirstOrDefault(x => x.Id == entity.InspectionTaskId);
                taskDetailDtos.Add(new EquInspectionTaskDetailDto
                {
                    Id = entity.Id,
                    InspectionItemCode = item?.Code ?? "",
                    InspectionItemName = item?.Name ?? "",
                    BaseValue = entity.BaseValue,
                    MaxValue = entity.MaxValue,
                    MinValue = entity.MinValue,
                    Unit = entity.Unit,
                    Remark = entity.Remark
                });
            }

            return taskDetailDtos;
        }

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
            var entity = await _equInspectionTaskRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15803));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
            #endregion

            #region 操作数据库
            await _equInspectionTaskRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }
    }
}
