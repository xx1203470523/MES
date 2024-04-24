using FluentValidation;
using FluentValidation.Results;
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
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Dtos.Common;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.QualEnvOrderDetail;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Minio.DataModel;
using System;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Reactive;
using System.Reflection.Emit;
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

        private readonly IEquInspectionTaskSnapshootRepository _taskSnapshootRepository;
        private readonly IEquInspectionTaskDetailsSnapshootRepository _detailsSnapshootRepository;
        private readonly IEquInspectionRecordRepository _inspectionRecordRepository;
        private readonly IEquInspectionRecordDetailsRepository _recordDetailsRepository;

        private readonly IEquEquipmentRepository _equEquipmentRepository;
        private readonly IInteWorkCenterRepository _workCenterRepository;

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
            IEquInspectionTaskSnapshootRepository taskSnapshootRepository,
            IEquInspectionTaskDetailsSnapshootRepository detailsSnapshootRepository,
            IEquInspectionRecordRepository inspectionRecordRepository,
            IEquInspectionRecordDetailsRepository recordDetailsRepository,
            IEquEquipmentRepository equEquipmentRepository,
            IInteWorkCenterRepository workCenterRepository,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equInspectionTaskRepository = equInspectionTaskRepository;
            _taskDetailsRepository = taskDetailsRepository;
            _inspectionItemRepository = inspectionItemRepository;
            _taskSnapshootRepository = taskSnapshootRepository;
            _detailsSnapshootRepository = detailsSnapshootRepository;
            _inspectionRecordRepository = inspectionRecordRepository;
            _recordDetailsRepository = recordDetailsRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _workCenterRepository= workCenterRepository;
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
            if (saveDto.TaskDetailsSaveDtos != null && saveDto.TaskDetailsSaveDtos.Any()&& saveDto.TaskDetailsSaveDtos.GroupBy(x => x.InspectionItemId).Any(g => g.Count() >= 2))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15806));
            }
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
            //time格式化存储

            //判断关联项目
            var taskDetailsEntities = new List<EquInspectionTaskDetailsEntity>();
            if (saveDto.TaskDetailsSaveDtos != null && saveDto.TaskDetailsSaveDtos.Any())
            {
                int i = 0;
                foreach (var detail in saveDto.TaskDetailsSaveDtos)
                {
                    i++;
                    //校验是否重复
                    //校验上下限
                    if ((detail.MaxValue - detail.MinValue) < 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15730)).WithData("line", i);
                    }

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
           // entity.Time = saveDto.Time.ToString("HH:mm");
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            //验证关联项目不能重复
            if (saveDto.TaskDetailsSaveDtos != null && saveDto.TaskDetailsSaveDtos.Any() && saveDto.TaskDetailsSaveDtos.GroupBy(x => x.InspectionItemId).Any(g => g.Count() >= 2))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15806));
            }

            //判断关联项目
            var taskDetailsEntities = new List<EquInspectionTaskDetailsEntity>();
            if (saveDto.TaskDetailsSaveDtos != null && saveDto.TaskDetailsSaveDtos.Any())
            {
                int i = 0;
                foreach (var detail in saveDto.TaskDetailsSaveDtos)
                {
                    i++;
                    //校验上下限
                    if ((detail.MaxValue - detail.MinValue) < 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15730)).WithData("line", i);
                    }

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
            var inspectionTaskEntity = await _equInspectionTaskRepository.GetByIdAsync(id);
            if (inspectionTaskEntity == null)
            {
                return null;
            }

            var taskDto = inspectionTaskEntity.ToModel<EquInspectionTaskDto>();

            var equipmentEntity=await _equEquipmentRepository.GetByIdAsync(inspectionTaskEntity.EquipmentId);
            var workCenterEntity=await _workCenterRepository.GetByIdAsync(inspectionTaskEntity.WorkCenterId);

            taskDto.EquipmentCode= equipmentEntity?.EquipmentCode??"";
            taskDto.EquipmentName= equipmentEntity?.EquipmentName??"";
            taskDto.WorkCenterCode = workCenterEntity?.Code??"";
            return taskDto;
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
        public async Task<IEnumerable<EquInspectionTaskDetailDto>> QueryItemsByTaskIdAsync(long taskId)
        {
            var equInspectionTaskDetails = await _taskDetailsRepository.GetEntitiesAsync(new EquInspectionTaskDetailsQuery { InspectionTaskId = taskId });

            List<EquInspectionTaskDetailDto> taskDetailDtos = new List<EquInspectionTaskDetailDto>();
            if (equInspectionTaskDetails == null || !equInspectionTaskDetails.Any())
            {
                return taskDetailDtos;
            }

            var itemIds = equInspectionTaskDetails.Select(s => s.InspectionItemId).ToArray();
            var itemEntities = await _inspectionItemRepository.GetByIdsAsync(itemIds);

            foreach (var entity in equInspectionTaskDetails)
            {
                var item = itemEntities.FirstOrDefault(x => x.Id == entity.InspectionItemId);
                taskDetailDtos.Add(new EquInspectionTaskDetailDto
                {
                    Id = entity.Id,
                    InspectionItemId = entity.InspectionItemId,
                    InspectionItemCode = item?.Code ?? "",
                    InspectionItemName = item?.Name ?? "",
                    Code = item?.Code ?? "",
                    Name = item?.Name ??"",
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

        /// <summary>
        /// 生成录入任务
        /// </summary>
        /// <param name="recordDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<long> GeneratedTaskRecordAsync(GenerateInspectionRecordDto recordDto)
        {
            #region 验证

            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            // 点检任务
            var taskEntity = await _equInspectionTaskRepository.GetByIdAsync(recordDto.Id);
            if (taskEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15803));
            }

            //如果是周点检需要判断月份符合不然报错
            if (taskEntity.InspectionType == EquInspectionTypeEnum.WeeklyInspection)
            {
                //获取当前月份
                var month = DateTime.Now.Month;
                if ((!taskEntity.Month.HasValue || (int)taskEntity.Month != month))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15804));
                }
            }
            #endregion

            //日点检,当前日期+执行时间
            var execuTime = DateTime.Now.ToString("yyyy-MM-dd") +" "+ taskEntity.Time; 
            //周点检
            if (taskEntity.InspectionType == EquInspectionTypeEnum.WeeklyInspection)
            {
                //获取当前周拼成时间,当前年+当前周的执行日+执行时间
                DateTime today = DateTime.Today;
                int diff = (7 + (DateTime.Today.DayOfWeek - DayOfWeek.Monday)) % 7;
                var dayOfweek = taskEntity.Day.HasValue?(int)taskEntity.Day:0;
                var days = (-diff) + dayOfweek - 1;
                DateTime firstDayOfWeek = today.AddDays(days);
                execuTime= firstDayOfWeek.ToString("yyyy-MM-dd") + " " + taskEntity.Time;
            }

            //读取详情
            var equInspectionTasks = await _taskDetailsRepository.GetEntitiesAsync(new EquInspectionTaskDetailsQuery
            {
                InspectionTaskId = taskEntity.Id
            });

            var updatedBy = _currentUser.UserName;
            var siteId = _currentSite.SiteId ?? 0;
            var snapshootEntity = new EquInspectionTaskSnapshootEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                Code = taskEntity.Code,
                InspectionType = taskEntity.InspectionType,
                WorkCenterId = taskEntity.WorkCenterId,
                EquipmentId = taskEntity.EquipmentId,
                Month = taskEntity.Month,
                Day = taskEntity.Day,
                Time = taskEntity.Time,
                CompleteTime = taskEntity.CompleteTime,
                Version = taskEntity.Version,
                Status = taskEntity.Status,
                Type = taskEntity.Type,
                Remark = taskEntity.Remark??"",
                CreatedBy = updatedBy,
                UpdatedBy = updatedBy,
                SiteId = siteId
            };

            var startExecuTime = DateTime.Parse(execuTime);
            var recordEntity = new EquInspectionRecordEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                OrderCode = snapshootEntity.Code,
                InspectionTaskSnapshootId = snapshootEntity.Id,
                StartExecuTime = startExecuTime,
                Status = EquInspectionRecordStatusEnum.WaitInspect,
                CreatedBy = updatedBy,
                UpdatedBy = updatedBy,
                SiteId = siteId
            };

            var snapshootEntities = new List<EquInspectionTaskDetailsSnapshootEntity>();
            var recordDetailsEntities = new List<EquInspectionRecordDetailsEntity>();

            foreach (var detail in equInspectionTasks)
            {
                var detailSnapshootId = IdGenProvider.Instance.CreateId();
                snapshootEntities.Add(new EquInspectionTaskDetailsSnapshootEntity
                {
                    Id = detailSnapshootId,
                    InspectionTaskId = snapshootEntity.Id,
                    InspectionItemId = detail.InspectionItemId,
                    BaseValue = detail.BaseValue,
                    MaxValue = detail.MaxValue,
                    MinValue = detail.MinValue,
                    Unit = detail.Unit,
                    Remark = detail.Remark,
                    CreatedBy = updatedBy,
                    UpdatedBy = updatedBy,
                    SiteId = siteId
                });

                recordDetailsEntities.Add(new EquInspectionRecordDetailsEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    InspectionRecordId = recordEntity.Id,
                    InspectionTaskDetailSnapshootId = detailSnapshootId,
                    CreatedBy = updatedBy,
                    UpdatedBy = updatedBy,
                    SiteId = siteId
                });
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                // 任务
                await _taskSnapshootRepository.InsertAsync(snapshootEntity);

                //任务记录
                await _inspectionRecordRepository.InsertAsync(recordEntity);

                //任务详情
                if (snapshootEntities != null && snapshootEntities.Count > 0)
                {
                    await _detailsSnapshootRepository.InsertRangeAsync(snapshootEntities);

                    //任务记录详情
                    await _recordDetailsRepository.InsertRangeAsync(recordDetailsEntities);
                }
                ts.Complete();
            }
            return snapshootEntity.Id;
        }
    }
}
