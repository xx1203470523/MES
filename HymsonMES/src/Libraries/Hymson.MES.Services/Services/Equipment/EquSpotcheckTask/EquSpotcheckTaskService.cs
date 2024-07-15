using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.CoreServices.Bos.Equment;
using Hymson.MES.Core.Domain.Equipment.EquSpotcheck;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Services.EquEquipmentRecord;
using Hymson.MES.Core.Domain.EquEquipmentRecord;
using Hymson.MES.Services.Dtos.EquEquipmentRecord;
using Hymson.MES.Data.Repositories.EquEquipmentRecord;
using Hymson.MES.Core.Enums.Common;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（点检任务） 
    /// </summary>
    public class EquSpotcheckTaskService : IEquSpotcheckTaskService
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
        private readonly AbstractValidator<EquSpotcheckTaskSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（点检任务）
        /// </summary>
        private readonly IEquSpotcheckTaskRepository _equSpotcheckTaskRepository;

        /// <summary>
        /// 仓储（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        /// <summary>
        /// 设备点检任务项目
        /// </summary>
        private readonly IEquSpotcheckTaskItemRepository _equSpotcheckTaskItemRepository;
        /// <summary>
        /// 设备点检快照任务项目
        /// </summary>
        private readonly IEquSpotcheckTaskSnapshotItemRepository _equSpotcheckTaskSnapshotItemRepository;

        /// <summary>
        /// 单位
        /// </summary>
        private readonly IInteUnitRepository _inteUnitRepository;

        /// <summary>
        /// 仓储接口（附件维护）
        /// </summary>
        private readonly IInteAttachmentRepository _inteAttachmentRepository;

        /// <summary>
        /// 设备点检任务项目附件
        /// </summary>
        private readonly IEquSpotcheckTaskItemAttachmentRepository _equSpotcheckTaskItemAttachmentRepository;

        /// <summary>
        /// 设备点检任务操作
        /// </summary>
        private readonly IEquSpotcheckTaskOperationRepository _equSpotcheckTaskOperationRepository;

        /// <summary>
        /// 设备点检任务结果处理
        /// </summary>
        private readonly IEquSpotcheckTaskProcessedRepository _equSpotcheckTaskProcessedRepository;

        /// <summary>
        /// 单据附件
        /// </summary>
        private readonly IEquSpotcheckTaskAttachmentRepository _equSpotcheckTaskAttachmentRepository;

        /// <summary>
        /// 快照计划
        /// </summary>
        private readonly IEquSpotcheckTaskSnapshotPlanRepository _equSpotcheckTaskSnapshotPlanRepository;

        /// <summary>
        /// 设备记录
        /// </summary>
        private readonly IEquEquipmentRecordService _equEquipmentRecordService;

        /// <summary>
        /// 设备记录(仓储)
        /// </summary>
        private readonly IEquEquipmentRecordRepository _equEquipmentRecordRepository;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equSpotcheckTaskRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="equSpotcheckTaskItemRepository"></param>
        /// <param name="equSpotcheckTaskSnapshotItemRepository"></param>
        /// <param name="inteUnitRepository"></param>
        /// <param name="inteAttachmentRepository"></param>
        /// <param name="equSpotcheckTaskItemAttachmentRepository"></param>
        /// <param name="equSpotcheckTaskOperationRepository"></param>
        /// <param name="equSpotcheckTaskProcessedRepository"></param>
        public EquSpotcheckTaskService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquSpotcheckTaskSaveDto> validationSaveRules,
            IEquSpotcheckTaskRepository equSpotcheckTaskRepository,
            IEquEquipmentRepository equEquipmentRepository,
            IEquSpotcheckTaskItemRepository equSpotcheckTaskItemRepository,
            IEquSpotcheckTaskSnapshotItemRepository equSpotcheckTaskSnapshotItemRepository,
            IInteUnitRepository inteUnitRepository, IInteAttachmentRepository inteAttachmentRepository,
            IEquSpotcheckTaskItemAttachmentRepository equSpotcheckTaskItemAttachmentRepository,
            IEquSpotcheckTaskOperationRepository equSpotcheckTaskOperationRepository,
            IEquSpotcheckTaskProcessedRepository equSpotcheckTaskProcessedRepository,
            IEquSpotcheckTaskAttachmentRepository equSpotcheckTaskAttachmentRepository,
            IEquSpotcheckTaskSnapshotPlanRepository equSpotcheckTaskSnapshotPlanRepository, IEquEquipmentRecordService equEquipmentRecordService, IEquEquipmentRecordRepository equEquipmentRecordRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equSpotcheckTaskRepository = equSpotcheckTaskRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _equSpotcheckTaskItemRepository = equSpotcheckTaskItemRepository;
            _equSpotcheckTaskSnapshotItemRepository = equSpotcheckTaskSnapshotItemRepository;
            _inteUnitRepository = inteUnitRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
            _equSpotcheckTaskItemAttachmentRepository = equSpotcheckTaskItemAttachmentRepository;
            _equSpotcheckTaskOperationRepository = equSpotcheckTaskOperationRepository;
            _equSpotcheckTaskProcessedRepository = equSpotcheckTaskProcessedRepository;
            _equSpotcheckTaskAttachmentRepository = equSpotcheckTaskAttachmentRepository;
            _equSpotcheckTaskSnapshotPlanRepository = equSpotcheckTaskSnapshotPlanRepository;
            _equEquipmentRecordService = equEquipmentRecordService;
            _equEquipmentRecordRepository = equEquipmentRecordRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquSpotcheckTaskSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSpotcheckTaskEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _equSpotcheckTaskRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquSpotcheckTaskSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSpotcheckTaskEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _equSpotcheckTaskRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equSpotcheckTaskRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            var entitys = await _equSpotcheckTaskRepository.GetByIdsAsync(ids);
            if (entitys != null)
            {
                var isDeleteEntitys = entitys.Where(x => x.Status != EquSpotcheckTaskStautusEnum.WaitInspect);
                if (isDeleteEntitys.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15904));
                }
            }

            return await _equSpotcheckTaskRepository.DeletesAsync(new DeleteCommand
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
        public async Task<EquSpotcheckTaskDto?> QueryByIdAsync(long id)
        {
            var equSpotcheckTaskEntity = await _equSpotcheckTaskRepository.GeUnionByIdAsync(id);
            if (equSpotcheckTaskEntity == null) return null;

            var result = equSpotcheckTaskEntity.ToModel<EquSpotcheckTaskDto>();

            var equipmenEntity = await _equEquipmentRepository.GetByIdAsync(result.EquipmentId.GetValueOrDefault());

            result.StatusText = result.Status?.GetDescription() ?? string.Empty;
            result.IsQualifiedText = result.IsQualified?.GetDescription() ?? string.Empty;
            result.EquipmentCode = equipmenEntity.EquipmentCode;
            result.Location = equipmenEntity.Location;
            result.ExecutorIds = _currentUser.UserName;

            result.PlanTypeText = result.PlanType?.GetDescription() ?? string.Empty;

            return result;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSpotcheckTaskDto>> GetPagedListAsync(EquSpotcheckTaskPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquSpotcheckTaskPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;

            // 转换设备编码变为taskid
            if (!string.IsNullOrWhiteSpace(pagedQuery.EquipmentCode))
            {
                var equipmenEntities = await _equEquipmentRepository.GetByCodeAsync(new Data.Repositories.Common.Query.EntityByCodeQuery
                {
                    Site = pagedQuery.SiteId,
                    Code = pagedQuery.EquipmentCode,
                });
                if (equipmenEntities != null) pagedQuery.EquipmentId = equipmenEntities.Id;
                else pagedQuery.EquipmentId = 0;
            }

            // 处理方式转换为任务单ID
            if (pagedQueryDto.HandMethod.HasValue)
            {
                var processedHandEntities = await _equSpotcheckTaskProcessedRepository.GetEntitiesAsync(new EquSpotcheckTaskProcessedQuery
                {
                    SiteId = pagedQuery.SiteId,
                    HandMethod = pagedQueryDto.HandMethod
                });
                if (processedHandEntities != null && processedHandEntities.Any()) pagedQuery.TaskIds = processedHandEntities.Select(s => s.SpotCheckTaskId);
                else pagedQuery.TaskIds = Array.Empty<long>();
            }

            var result = new PagedInfo<EquSpotcheckTaskDto>(Enumerable.Empty<EquSpotcheckTaskDto>(), pagedQuery.PageIndex, pagedQuery.PageSize);

            var pagedInfo = await _equSpotcheckTaskRepository.GetPagedListAsync(pagedQuery);

            if (pagedInfo.Data != null && pagedInfo.Data.Any())
            {
                result.Data = pagedInfo.Data.Select(s => s.ToModel<EquSpotcheckTaskDto>());
                result.TotalCount = pagedInfo.TotalCount;

                var resultEquipmentIds = result.Data.Select(m => m.EquipmentId.GetValueOrDefault());
                var resultIds = result.Data.Select(m => m.Id);

                try
                {
                    var equipmenEntities = await _equEquipmentRepository.GetByIdAsync(resultEquipmentIds);

                    var processEntities = await _equSpotcheckTaskProcessedRepository.GetEntitiesAsync(new EquSpotcheckTaskProcessedQuery { SpotCheckTaskIds = resultIds });

                    result.Data = result.Data.Select(m =>
                    {
                        //设备处理
                        var equipmentEntity = equipmenEntities.FirstOrDefault(e => e.Id == m.EquipmentId);
                        if (equipmentEntity != null)
                        {
                            m.EquipmentCode = equipmentEntity.EquipmentCode;
                            m.EquipmentName = equipmentEntity.EquipmentName;
                            m.Location = equipmentEntity.Location;
                        }

                        //处理结果
                        var processEntity = processEntities.FirstOrDefault(e => e.SpotCheckTaskId == m.Id);
                        if (processEntity != null)
                        {
                            m.HandMethod = processEntity.HandMethod;
                            m.ProcessedBy = processEntity.ProcessedBy;
                        }

                        m.PlanTypeText = m.PlanType == 0 ? string.Empty : m.PlanType?.GetDescription();

                        return m;
                    });
                }
                catch (Exception ex) { }

            }

            return result;
        }



        /// <summary>
        /// 更改检验单状态（点击执行检验）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> OperationOrderAsync(EquSpotcheckTaskOrderOperationStatusDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            //单据状态
            var entity = await _equSpotcheckTaskRepository.GetByIdAsync(requestDto.OrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 检查当前操作类型是否已经执行过
            if (entity.Status != EquSpotcheckTaskStautusEnum.WaitInspect) return default;
            switch (entity.Status)
            {
                case EquSpotcheckTaskStautusEnum.WaitInspect:
                    // 继续接下来的操作
                    break;
                case EquSpotcheckTaskStautusEnum.Completed:
                case EquSpotcheckTaskStautusEnum.Closed:
                    throw new CustomerValidationException(nameof(ErrorCode.MES11914))
                        .WithData("Status", $"{InspectionStatusEnum.Completed.GetDescription()}/{InspectionStatusEnum.Closed.GetDescription()}");
                case EquSpotcheckTaskStautusEnum.Inspecting:
                default: return default;
            }

            EquEquipmentRecordEntity? equEquipmentRecordEntity = null;

            // 更改状态
            switch (requestDto.OperationType)
            {
                case EquSpotcheckOperationTypeEnum.Start:
                    entity.Status = EquSpotcheckTaskStautusEnum.Inspecting;
                    var equSpotcheckTaskSnapshotPlan = await _equSpotcheckTaskSnapshotPlanRepository.GetByTaskIdAsync(entity.Id);
                    equEquipmentRecordEntity = await _equEquipmentRecordService.GetAddEquRecordByEquEquipmentAsync(new GetAddEquRecordByEquEquipmentDto { EquipmentId = equSpotcheckTaskSnapshotPlan.EquipmentId, operationType = EquEquipmentRecordOperationTypeEnum.Inspection });

                    break;
                case EquSpotcheckOperationTypeEnum.Complete:
                    entity.Status = entity.IsQualified == TrueOrFalseEnum.Yes ? EquSpotcheckTaskStautusEnum.Closed : EquSpotcheckTaskStautusEnum.Completed;
                    break;
                case EquSpotcheckOperationTypeEnum.Close:
                    entity.Status = EquSpotcheckTaskStautusEnum.Closed;
                    break;
                default:
                    break;
            }

            // 保存
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await CommonOperationAsync(entity, EquSpotcheckOperationTypeEnum.Start);
            if (equEquipmentRecordEntity != null)
            {
                await _equEquipmentRecordRepository.InsertAsync(equEquipmentRecordEntity);
            }
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 查询点检单明细项数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskItemUnionSnapshotView>> querySnapshotItemAsync(SpotcheckTaskSnapshotItemQueryDto requestDto)
        {
            var taskitem = await _equSpotcheckTaskItemRepository.GetEntitiesAsync(new EquSpotcheckTaskItemQuery { SpotCheckTaskId = requestDto.SpotCheckTaskId });
            var spotCheckItemSnapshotIds = taskitem.Select(e => e.SpotCheckItemSnapshotId);
            if (!spotCheckItemSnapshotIds.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }
            var taskitemSnap = await _equSpotcheckTaskSnapshotItemRepository.GetEntitiesAsync(new EquSpotcheckTaskSnapshotItemQuery { Ids = spotCheckItemSnapshotIds });
            //单位
            var unitIds = taskitemSnap.Where(x => x.UnitId.HasValue).Select(x => x.UnitId!.Value);
            var inteUnitEntitys = await _inteUnitRepository.GetByIdsAsync(unitIds);
            var unitDic = inteUnitEntitys.ToDictionary(x => x.Id, x => x.Code);

            // 查询检验单下面的所有样本附件
            var itemAttachmentEntities = await _equSpotcheckTaskItemAttachmentRepository.GetEntitiesAsync(new EquSpotcheckTaskItemAttachmentQuery
            {
                SpotCheckTaskId = requestDto.SpotCheckTaskId
            });

            // 附件集合
            Dictionary<long, IGrouping<long, EquSpotcheckTaskItemAttachmentEntity>> itemAttachmentDic = new();
            IEnumerable<InteAttachmentEntity> interAttachmentEntities = Array.Empty<InteAttachmentEntity>();

            if (itemAttachmentEntities.Any())
            {
                itemAttachmentDic = itemAttachmentEntities.ToLookup(w => w.SpotCheckTaskItemId).ToDictionary(d => d.Key, d => d);
                interAttachmentEntities = await _inteAttachmentRepository.GetByIdsAsync(itemAttachmentEntities.Select(s => s.AttachmentId));
            }


            List<TaskItemUnionSnapshotView> outViews = new();

            try
            {
                foreach (var item in taskitem)
                {
                    var snap = taskitemSnap.Where(x => x.Id == item.SpotCheckItemSnapshotId).FirstOrDefault();
                    if (snap != null)
                    {
                        var oneView = snap.ToModel<TaskItemUnionSnapshotView>();
                        oneView = item.ToCombineMap(oneView);

                        //处理单位
                        if (snap.UnitId.HasValue)
                        {
                            unitDic.TryGetValue(snap.UnitId ?? 0, out var unitCode);
                            if (unitCode != null)
                            {
                                oneView.Unit = unitCode;
                            }

                        }

                        // 填充附件
                        if (interAttachmentEntities != null && itemAttachmentDic.TryGetValue(item.Id, out var detailAttachmentEntities))
                        {
                            oneView.Attachments = PrepareAttachmentBaseDtos(detailAttachmentEntities, interAttachmentEntities);
                        }
                        else oneView.Attachments = Array.Empty<InteAttachmentBaseDto>();


                        outViews.Add(oneView);
                    }

                }
            }
            catch (Exception ex) { }

            return outViews;
        }

        public async Task<PagedInfo<TaskItemUnionSnapshotView>> QueryItemPagedListAsync(SpotcheckTaskItemPagedQueryDto dto)
        {
            var result = new PagedInfo<TaskItemUnionSnapshotView>(Enumerable.Empty<TaskItemUnionSnapshotView>(), dto.PageIndex, dto.PageSize);
            result.Data = await querySnapshotItemAsync(new SpotcheckTaskSnapshotItemQueryDto { SpotCheckTaskId = dto.SpotCheckTaskId });
            result.TotalCount = result.Data.Count();
            return result;
        }


        /// <summary>
        /// 保存点检
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SaveAndUpdateTaskItemAsync(SpotcheckTaskItemSaveDto requestDto)
        {
            var taskItemids = requestDto.Details.Select(x => x.Id);
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            //单据状态
            var taskEntity = await _equSpotcheckTaskRepository.GetByIdAsync(requestDto.SpotCheckTaskId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES15910));

            //单据快照 更新执行人
            var snapshotPlanEntitys = await _equSpotcheckTaskSnapshotPlanRepository.GetByTaskIdAsync(requestDto.SpotCheckTaskId);

            //快照项目
            var snapshotItemEntitys = await _equSpotcheckTaskSnapshotItemRepository.GetByIdsAsync(taskItemids.ToArray());

            taskEntity.UpdatedBy = updatedBy;
            taskEntity.UpdatedOn = updatedOn;

            if (snapshotPlanEntitys is not null)
            {
                snapshotPlanEntitys.ExecutorIds = updatedBy;
                snapshotPlanEntitys.UpdatedBy = updatedBy;
                snapshotPlanEntitys.UpdatedOn = updatedOn;

                if (updatedOn < snapshotPlanEntitys.BeginTime)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15912)).WithData("time", snapshotPlanEntitys.BeginTime);
                }
                if (updatedOn > snapshotPlanEntitys.EndTime)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15913)).WithData("time", snapshotPlanEntitys.EndTime);
                }
            }

            var entitys = await _equSpotcheckTaskItemRepository.GetByIdsAsync(taskItemids.ToArray());
            if (!entitys.Any()) return 0;

            var site = entitys.FirstOrDefault()?.SiteId ?? 0;

            // 样本附件
            List<InteAttachmentEntity> attachmentEntities = new();
            List<EquSpotcheckTaskItemAttachmentEntity> sampleDetailAttachmentEntities = new();

            foreach (var entity in entitys)
            {
                var transItem = requestDto.Details.Where(x => x.Id == entity.Id).FirstOrDefault();

                if (transItem == null) continue;

                entity.IsQualified = transItem.IsQualified;
                entity.InspectionValue = transItem.InspectionValue ?? "";
                entity.Remark = transItem.Remark;
                entity.UpdatedBy = updatedBy;
                entity.UpdatedOn = updatedOn;

                var oneDetail = requestDto.Details.Where(x => x.Id == entity.Id).FirstOrDefault();
                if (oneDetail == null) continue;

                var requestAttachments = oneDetail.Attachments;

                var currentDataType = snapshotItemEntitys.FirstOrDefault(x => x.Id == entity.SpotCheckItemSnapshotId)?.DataType;
                if (currentDataType != null && currentDataType == EquSpotcheckDataTypeEnum.Numeric)
                {
                    if (!decimal.TryParse(entity.InspectionValue, out var v))
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES15905));
                    }
                }


                if (requestAttachments != null && requestAttachments.Any())
                {
                    foreach (var attachment in requestAttachments)
                    {
                        // 附件
                        var attachmentId = IdGenProvider.Instance.CreateId();
                        attachmentEntities.Add(new InteAttachmentEntity
                        {
                            Id = attachmentId,
                            Name = attachment.Name,
                            Path = attachment.Path,
                            CreatedBy = updatedBy,
                            CreatedOn = updatedOn,
                            UpdatedBy = updatedBy,
                            UpdatedOn = updatedOn,
                            SiteId = entity.SiteId,
                        });

                        // 样本附件
                        sampleDetailAttachmentEntities.Add(new EquSpotcheckTaskItemAttachmentEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = entity.SiteId,
                            SpotCheckTaskId = requestDto.SpotCheckTaskId,
                            SpotCheckTaskItemId = entity.Id,
                            AttachmentId = attachmentId,
                            CreatedBy = updatedBy,
                            CreatedOn = updatedOn,
                            UpdatedBy = updatedBy,
                            UpdatedOn = updatedOn
                        });
                    }
                }
            }


            // 之前的附件
            var beforeAttachments = await _equSpotcheckTaskItemAttachmentRepository.GetEntitiesAsync(new EquSpotcheckTaskItemAttachmentQuery
            {
                SiteId = site,
                SpotCheckTaskId = requestDto.SpotCheckTaskId,
            });

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _equSpotcheckTaskItemRepository.UpdateRangeAsync(entitys);

            // 先删除再添加
            if (beforeAttachments != null && beforeAttachments.Any())
            {
                rows += await _equSpotcheckTaskItemAttachmentRepository.DeletesAsync(new DeleteCommand
                {
                    UserId = updatedBy,
                    DeleteOn = updatedOn,
                    Ids = beforeAttachments.Select(s => s.Id)
                });

                rows += await _inteAttachmentRepository.DeletesAsync(new DeleteCommand
                {
                    UserId = updatedBy,
                    DeleteOn = updatedOn,
                    Ids = beforeAttachments.Select(s => s.AttachmentId)
                });
            }

            //修改检验状态
            rows += await OperationOrderAsync(new EquSpotcheckTaskOrderOperationStatusDto
            {
                OrderId = requestDto.SpotCheckTaskId,
                OperationType = EquSpotcheckOperationTypeEnum.Start
            });

            //更新task操作时间
            rows += await _equSpotcheckTaskRepository.UpdateAsync(taskEntity);
            if (snapshotPlanEntitys != null)
            {
                rows += await _equSpotcheckTaskSnapshotPlanRepository.UpdateAsync(snapshotPlanEntitys);
            }
            if (attachmentEntities.Any())
            {
                rows += await _inteAttachmentRepository.InsertRangeAsync(attachmentEntities);
                rows += await _equSpotcheckTaskItemAttachmentRepository.InsertRangeAsync(sampleDetailAttachmentEntities);
            }
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 点检完成
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<int> CompleteOrderAsync(SpotcheckTaskCompleteDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            //单据
            var entity = await _equSpotcheckTaskRepository.GetByIdAsync(requestDto.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 只有"检验中"的状态才允许点击"完成"
            if (entity.Status != EquSpotcheckTaskStautusEnum.Inspecting)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11912))
                    .WithData("Before", EquSpotcheckTaskStautusEnum.Inspecting.GetDescription())
                    .WithData("After", EquSpotcheckTaskStautusEnum.Completed.GetDescription());
            }
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            //校验是否超期
            var snapshotPlanEntitys = await _equSpotcheckTaskSnapshotPlanRepository.GetByTaskIdAsync(requestDto.Id);
            if (snapshotPlanEntitys is not null)
            {
                if (updatedOn < snapshotPlanEntitys.BeginTime)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15912)).WithData("time", snapshotPlanEntitys.BeginTime);
                }
                if (updatedOn > snapshotPlanEntitys.EndTime)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15913)).WithData("time", snapshotPlanEntitys.EndTime);
                }
            }

            // 读取所有明细参数
            var sampleDetailEntities = await _equSpotcheckTaskItemRepository.GetEntitiesAsync(new EquSpotcheckTaskItemQuery
            {
                SiteId = entity.SiteId,
                SpotCheckTaskId = entity.Id
            });

            var snapshotItem = await _equSpotcheckTaskSnapshotItemRepository.GetByIdsAsync(sampleDetailEntities.Select(x => x.SpotCheckItemSnapshotId).ToArray());

            //检验值是否为空
            if (sampleDetailEntities.Any(x => string.IsNullOrEmpty(x.InspectionValue)))
            {
                var isEmptyValueList = sampleDetailEntities.Where(x => string.IsNullOrWhiteSpace(x.InspectionValue));
                if (isEmptyValueList.Any())
                {
                    foreach (var item in isEmptyValueList)
                    {
                        var emptyValueSnapshotItem = snapshotItem.Where(x => x.Id == item.SpotCheckItemSnapshotId).FirstOrDefault();
                        if (emptyValueSnapshotItem != null && emptyValueSnapshotItem.DataType == EquSpotcheckDataTypeEnum.Numeric)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES15911));
                        }
                    }
                }
            }

            var operationType = EquSpotcheckOperationTypeEnum.Complete;

            //有任一不合格，完成
            if (sampleDetailEntities.Any(X => X.IsQualified == TrueFalseEmptyEnum.No))
            {
                entity.Status = EquSpotcheckTaskStautusEnum.Completed;
                entity.IsQualified = TrueOrFalseEnum.No;
                operationType = EquSpotcheckOperationTypeEnum.Complete;
            }
            else
            {
                // 默认是关闭
                entity.Status = EquSpotcheckTaskStautusEnum.Closed;
                entity.IsQualified = TrueOrFalseEnum.Yes;
                operationType = EquSpotcheckOperationTypeEnum.Close;
            }

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await CommonOperationAsync(entity, operationType);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 关闭检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> CloseOrderAsync(SpotcheckTaskCloseDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            //点检单
            var entity = await _equSpotcheckTaskRepository.GetByIdAsync(requestDto.SpotCheckTaskId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 只有"已检验"的状态才允许"关闭"
            if (entity.Status != EquSpotcheckTaskStautusEnum.Completed)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11912))
                    .WithData("Before", InspectionStatusEnum.Completed.GetDescription())
                    .WithData("After", InspectionStatusEnum.Closed.GetDescription());
            }

            // 不合格处理完成之后直接关闭（无需变为合格）
            entity.Status = EquSpotcheckTaskStautusEnum.Closed;

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await CommonOperationAsync(entity, EquSpotcheckOperationTypeEnum.Close, new SpotTaskHandleBo { HandMethod = requestDto.HandMethod, Remark = requestDto.Remark });
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 通用操作（未加事务）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operationType"></param>
        /// <param name="handleBo"></param>
        /// <returns></returns>
        private async Task<int> CommonOperationAsync(EquSpotcheckTaskEntity entity, EquSpotcheckOperationTypeEnum operationType, SpotTaskHandleBo? handleBo = null)
        {
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 更新
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            if (operationType == EquSpotcheckOperationTypeEnum.Start)
            {
                entity.BeginTime = updatedOn;
            }
            else
            {
                entity.EndTime = updatedOn;
            }


            var rows = 0;
            rows += await _equSpotcheckTaskRepository.UpdateAsync(entity);
            rows += await _equSpotcheckTaskOperationRepository.InsertAsync(new EquSpotcheckTaskOperationEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                SpotCheckTaskId = entity.Id,
                OperationType = operationType,
                OperationBy = updatedBy,
                OperationOn = updatedOn,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            });

            if (handleBo != null) rows += await _equSpotcheckTaskProcessedRepository.InsertAsync(new EquSpotcheckTaskProcessedEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                SpotCheckTaskId = entity.Id,
                HandMethod = handleBo.HandMethod,
                Remark = handleBo.Remark ?? "",
                ProcessedBy = updatedBy,
                ProcessedOn = updatedOn,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            });
            return rows;
        }

        /// <summary>
        /// 保存检验单附件
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SaveAttachmentAsync(SpotcheckTaskSaveAttachmentDto requestDto)
        {
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            //单据
            var entity = await _equSpotcheckTaskRepository.GetByIdAsync(requestDto.OrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES11714));

            if (!requestDto.Attachments.Any()) return 0;

            List<InteAttachmentEntity> inteAttachmentEntities = new();
            List<EquSpotcheckTaskAttachmentEntity> orderAttachmentEntities = new();
            foreach (var attachment in requestDto.Attachments)
            {
                var attachmentId = IdGenProvider.Instance.CreateId();
                inteAttachmentEntities.Add(new InteAttachmentEntity
                {
                    Id = attachmentId,
                    Name = attachment.Name,
                    Path = attachment.Path,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn,
                    SiteId = entity.SiteId,
                });

                orderAttachmentEntities.Add(new EquSpotcheckTaskAttachmentEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = entity.SiteId,
                    SpotCheckTaskId = requestDto.OrderId,
                    AttachmentId = attachmentId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });
            }

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _inteAttachmentRepository.InsertRangeAsync(inteAttachmentEntities);
            rows += await _equSpotcheckTaskAttachmentRepository.InsertRangeAsync(orderAttachmentEntities);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 删除检验单附件
        /// </summary>
        /// <param name="orderAttachmentId"></param>
        /// <returns></returns>
        public async Task<int> DeleteAttachmentByIdAsync(long orderAttachmentId)
        {
            var attachmentEntity = await _equSpotcheckTaskAttachmentRepository.GetByIdAsync(orderAttachmentId);
            if (attachmentEntity == null) return default;

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _inteAttachmentRepository.DeleteAsync(attachmentEntity.AttachmentId);
            rows += await _equSpotcheckTaskAttachmentRepository.DeleteAsync(attachmentEntity.Id);
            trans.Complete();
            return rows;
        }


        /// <summary>
        /// 查询单据附件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryOrderAttachmentListByIdAsync(long orderId)
        {
            var orderAttachments = await _equSpotcheckTaskAttachmentRepository.GetByOrderIdAsync(orderId);
            if (orderAttachments == null) return Array.Empty<InteAttachmentBaseDto>();

            var attachmentEntities = await _inteAttachmentRepository.GetByIdsAsync(orderAttachments.Select(s => s.AttachmentId));
            if (attachmentEntities == null) return Array.Empty<InteAttachmentBaseDto>();

            return PrepareAttachmentBaseDtos(orderAttachments, attachmentEntities);
        }

        /// <summary>
        /// 转换成附近DTO
        /// </summary>
        /// <param name="linkAttachments"></param>
        /// <param name="attachmentEntities"></param>
        /// <returns></returns>
        private static IEnumerable<InteAttachmentBaseDto> PrepareAttachmentBaseDtos(IEnumerable<dynamic> linkAttachments, IEnumerable<InteAttachmentEntity> attachmentEntities)
        {
            List<InteAttachmentBaseDto> dtos = new();
            foreach (var item in linkAttachments)
            {
                var dto = new InteAttachmentBaseDto
                {
                    Id = item.Id,
                    AttachmentId = item.AttachmentId
                };

                var attachmentEntity = attachmentEntities.FirstOrDefault(f => f.Id == item.AttachmentId);
                if (attachmentEntity == null)
                {
                    dto.Name = "附件不存在";
                    dto.Path = "";
                    dto.Url = "";
                    dtos.Add(dto);
                    continue;
                }

                dto.Id = item.Id;
                dto.Name = attachmentEntity.Name;
                dto.Path = attachmentEntity.Path;
                dto.Url = attachmentEntity.Path;
                dtos.Add(dto);
            }

            return dtos;
        }



    }
}
