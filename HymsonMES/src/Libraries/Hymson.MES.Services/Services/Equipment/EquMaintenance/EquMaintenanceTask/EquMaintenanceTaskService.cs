using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Equipment.EquMaintenance;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Equment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Services;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Equipment.EquMaintenance;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Core.Enums.Equipment.EquMaintenance;

namespace Hymson.MES.Services.Services.Equipment.EquMaintenance.EquMaintenanceTask
{
    /// <summary>
    /// 服务（设备保养任务） 
    /// </summary>
    public class EquMaintenanceTaskService : IEquMaintenanceTaskService
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
        private readonly AbstractValidator<EquMaintenanceTaskSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（设备保养任务）
        /// </summary>
        private readonly IEquMaintenanceTaskRepository _equMaintenanceTaskRepository;

        /// <summary>
        /// 仓储（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        /// <summary>
        /// 设备点检任务项目
        /// </summary>
        private readonly IEquMaintenanceTaskItemRepository _equMaintenanceTaskItemRepository;
        /// <summary>
        /// 设备点检快照任务项目
        /// </summary>
        private readonly IEquMaintenanceTaskSnapshotItemRepository _equMaintenanceTaskSnapshotItemRepository;

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
        private readonly IEquMaintenanceTaskItemAttachmentRepository _equMaintenanceTaskItemAttachmentRepository;

        /// <summary>
        /// 设备点检任务操作
        /// </summary>
        private readonly IEquMaintenanceTaskOperationRepository _equMaintenanceTaskOperationRepository;

        /// <summary>
        /// 设备点检任务结果处理
        /// </summary>
        private readonly IEquMaintenanceTaskProcessedRepository _equMaintenanceTaskProcessedRepository;

        /// <summary>
        /// 单据附件
        /// </summary>
        private readonly IEquMaintenanceTaskAttachmentRepository _equMaintenanceTaskAttachmentRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equMaintenanceTaskRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="equMaintenanceTaskItemRepository"></param>
        /// <param name="equMaintenanceTaskSnapshotItemRepository"></param>
        /// <param name="inteUnitRepository"></param>
        /// <param name="inteAttachmentRepository"></param>
        /// <param name="equMaintenanceTaskItemAttachmentRepository"></param>
        /// <param name="equMaintenanceTaskOperationRepository"></param>
        /// <param name="equMaintenanceTaskProcessedRepository"></param>
        /// <param name="equMaintenanceTaskAttachmentRepository"></param>
        public EquMaintenanceTaskService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquMaintenanceTaskSaveDto> validationSaveRules,
            IEquMaintenanceTaskRepository equMaintenanceTaskRepository,
               IEquEquipmentRepository equEquipmentRepository,
            IEquMaintenanceTaskItemRepository equMaintenanceTaskItemRepository,
            IEquMaintenanceTaskSnapshotItemRepository equMaintenanceTaskSnapshotItemRepository,
            IInteUnitRepository inteUnitRepository, IInteAttachmentRepository inteAttachmentRepository,
            IEquMaintenanceTaskItemAttachmentRepository equMaintenanceTaskItemAttachmentRepository,
            IEquMaintenanceTaskOperationRepository equMaintenanceTaskOperationRepository,
            IEquMaintenanceTaskProcessedRepository equMaintenanceTaskProcessedRepository,
            IEquMaintenanceTaskAttachmentRepository equMaintenanceTaskAttachmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equMaintenanceTaskRepository = equMaintenanceTaskRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _equMaintenanceTaskItemRepository = equMaintenanceTaskItemRepository;
            _equMaintenanceTaskSnapshotItemRepository = equMaintenanceTaskSnapshotItemRepository;
            _inteUnitRepository = inteUnitRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
            _equMaintenanceTaskItemAttachmentRepository = equMaintenanceTaskItemAttachmentRepository;
            _equMaintenanceTaskOperationRepository = equMaintenanceTaskOperationRepository;
            _equMaintenanceTaskProcessedRepository = equMaintenanceTaskProcessedRepository;
            _equMaintenanceTaskAttachmentRepository = equMaintenanceTaskAttachmentRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquMaintenanceTaskSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquMaintenanceTaskEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _equMaintenanceTaskRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquMaintenanceTaskSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquMaintenanceTaskEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _equMaintenanceTaskRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equMaintenanceTaskRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _equMaintenanceTaskRepository.DeletesAsync(new DeleteCommand
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
        public async Task<EquMaintenanceTaskDto?> QueryByIdAsync(long id)
        {
            var taskEntity = await _equMaintenanceTaskRepository.GeUnionByIdAsync(id);
            if (taskEntity == null) return null;

            var result = taskEntity.ToModel<EquMaintenanceTaskDto>();

            var equipmenEntity = await _equEquipmentRepository.GetByIdAsync(result.EquipmentId.GetValueOrDefault());

            result.StatusText = result.Status?.GetDescription() ?? string.Empty;
            result.IsQualifiedText = result.IsQualified?.GetDescription() ?? string.Empty;
            result.EquipmentCode = equipmenEntity?.EquipmentCode ?? string.Empty;
            result.Location = equipmenEntity?.Location ?? string.Empty;
            result.PlanTypeText = result.PlanType?.GetDescription() ?? string.Empty;

            return result;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquMaintenanceTaskDto>> GetPagedListAsync(EquMaintenanceTaskPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquMaintenanceTaskPagedQuery>();
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
                else pagedQuery.EquipmentId = default;
            }

            // 处理方式转换为任务单ID
            if (pagedQueryDto.HandMethod.HasValue)
            {
                var processedHandEntities = await _equMaintenanceTaskProcessedRepository.GetEntitiesAsync(new EquMaintenanceTaskProcessedQuery
                {
                    SiteId = pagedQuery.SiteId,
                    HandMethod = pagedQueryDto.HandMethod
                });
                if (processedHandEntities != null && processedHandEntities.Any()) pagedQuery.TaskIds = processedHandEntities.Select(s => s.MaintenanceTaskId);
                else pagedQuery.TaskIds = Array.Empty<long>();
            }

            var result = new PagedInfo<EquMaintenanceTaskDto>(Enumerable.Empty<EquMaintenanceTaskDto>(), pagedQuery.PageIndex, pagedQuery.PageSize);

            var pagedInfo = await _equMaintenanceTaskRepository.GetPagedListAsync(pagedQuery);

            if (pagedInfo.Data != null && pagedInfo.Data.Any())
            {
                result.Data = pagedInfo.Data.Select(s => s.ToModel<EquMaintenanceTaskDto>());
                result.TotalCount = pagedInfo.TotalCount;

                var resultEquipmentIds = result.Data.Select(m => m.EquipmentId.GetValueOrDefault());
                var resultIds = result.Data.Select(m => m.Id);

                try
                {
                    var equipmenEntities = await _equEquipmentRepository.GetByIdAsync(resultEquipmentIds);

                    var processEntities = await _equMaintenanceTaskProcessedRepository.GetEntitiesAsync(new EquMaintenanceTaskProcessedQuery { MaintenanceTaskIds = resultIds });

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
                        var processEntity = processEntities.FirstOrDefault(e => e.MaintenanceTaskId == m.Id);
                        if (processEntity != null)
                        {
                            m.HandMethod = processEntity.HandMethod;
                            m.ProcessedBy = processEntity.ProcessedBy;
                        }

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
        public async Task<int> OperationOrderAsync(EquMaintenanceTaskOrderOperationStatusDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            //单据状态
            var entity = await _equMaintenanceTaskRepository.GetByIdAsync(requestDto.OrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 检查当前操作类型是否已经执行过
            if (entity.Status != EquMaintenanceTaskStautusEnum.WaitInspect) return default;
            switch (entity.Status)
            {
                case EquMaintenanceTaskStautusEnum.WaitInspect:
                    // 继续接下来的操作
                    break;
                case EquMaintenanceTaskStautusEnum.Completed:
                case EquMaintenanceTaskStautusEnum.Closed:
                    throw new CustomerValidationException(nameof(ErrorCode.MES11914))
                        .WithData("Status", $"{InspectionStatusEnum.Completed.GetDescription()}/{InspectionStatusEnum.Closed.GetDescription()}");
                case EquMaintenanceTaskStautusEnum.Inspecting:
                default: return default;
            }

            // 更改状态
            switch (requestDto.OperationType)
            {
                case EquMaintenanceOperationTypeEnum.Start:
                    entity.Status = EquMaintenanceTaskStautusEnum.Inspecting;
                    break;
                case EquMaintenanceOperationTypeEnum.Complete:
                    entity.Status = entity.IsQualified == TrueOrFalseEnum.Yes ? EquMaintenanceTaskStautusEnum.Closed : EquMaintenanceTaskStautusEnum.Completed;
                    break;
                case EquMaintenanceOperationTypeEnum.Close:
                    entity.Status = EquMaintenanceTaskStautusEnum.Closed;
                    break;
                default:
                    break;
            }

            // 保存
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await CommonOperationAsync(entity, EquMaintenanceOperationTypeEnum.Start);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 查询点检单明细项数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquMaintenanceTaskItemUnionSnapshotView>> querySnapshotItemAsync(EquMaintenanceTaskSnapshotItemQueryDto requestDto)
        {
            var taskitem = await _equMaintenanceTaskItemRepository.GetEntitiesAsync(new EquMaintenanceTaskItemQuery { MaintenanceTaskId = requestDto.MaintenanceTaskId });
            var MaintenanceItemSnapshotIds = taskitem.Select(e => e.MaintenanceItemSnapshotId);
            if (!MaintenanceItemSnapshotIds.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }
            var taskitemSnap = await _equMaintenanceTaskSnapshotItemRepository.GetEntitiesAsync(new EquMaintenanceTaskSnapshotItemQuery { Ids = MaintenanceItemSnapshotIds });
            //单位
            var unitIds = taskitemSnap.Where(x => x.UnitId.HasValue).Select(x => x.UnitId!.Value);
            var inteUnitEntitys = await _inteUnitRepository.GetByIdsAsync(unitIds);
            var unitDic = inteUnitEntitys.ToDictionary(x => x.Id, x => x.Code);

            // 查询检验单下面的所有样本附件
            var itemAttachmentEntities = await _equMaintenanceTaskItemAttachmentRepository.GetEntitiesAsync(new EquMaintenanceTaskItemAttachmentQuery
            {
                MaintenanceTaskId = requestDto.MaintenanceTaskId
            });

            // 附件集合
            Dictionary<long, IGrouping<long, EquMaintenanceTaskItemAttachmentEntity>> itemAttachmentDic = new();
            IEnumerable<InteAttachmentEntity> interAttachmentEntities = Array.Empty<InteAttachmentEntity>();

            if (itemAttachmentEntities.Any())
            {
                itemAttachmentDic = itemAttachmentEntities.ToLookup(w => w.MaintenanceTaskItemId).ToDictionary(d => d.Key, d => d);
                interAttachmentEntities = await _inteAttachmentRepository.GetByIdsAsync(itemAttachmentEntities.Select(s => s.AttachmentId));
            }

            List<EquMaintenanceTaskItemUnionSnapshotView> outViews = new();

            try
            {
                foreach (var item in taskitem)
                {
                    var snap = taskitemSnap.Where(x => x.Id == item.MaintenanceItemSnapshotId).FirstOrDefault();
                    if (snap != null)
                    {
                        var oneView = snap.ToModel<EquMaintenanceTaskItemUnionSnapshotView>();
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

        /// <summary>
        /// 查询明细数据(结果处理)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquMaintenanceTaskItemUnionSnapshotView>> QueryItemPagedListAsync(EquMaintenanceTaskItemPagedQueryDto dto)
        {
            var result = new PagedInfo<EquMaintenanceTaskItemUnionSnapshotView>(Enumerable.Empty<EquMaintenanceTaskItemUnionSnapshotView>(), dto.PageIndex, dto.PageSize);
            result.Data = await querySnapshotItemAsync(new EquMaintenanceTaskSnapshotItemQueryDto { MaintenanceTaskId = dto.MaintenanceTaskId });
            return result;
        }


        /// <summary>
        /// 保存点检
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SaveAndUpdateTaskItemAsync(EquMaintenanceTaskItemSaveDto requestDto)
        {
            var taskItemids = requestDto.Details.Select(x => x.Id);

            var entitys = await _equMaintenanceTaskItemRepository.GetByIdsAsync(taskItemids.ToArray());
            if (!entitys.Any()) return 0;

            var site = entitys.FirstOrDefault()?.SiteId ?? 0;

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 样本附件
            List<InteAttachmentEntity> attachmentEntities = new();
            List<EquMaintenanceTaskItemAttachmentEntity> sampleDetailAttachmentEntities = new();

            foreach (var entity in entitys)
            {
                var transItem = requestDto.Details.Where(x => x.Id == entity.Id).FirstOrDefault();

                if (transItem == null) continue;

                entity.IsQualified = transItem.IsQualified;
                entity.InspectionValue = transItem.InspectionValue ?? "";
                entity.Remark = transItem.Remark ?? string.Empty;
                entity.UpdatedBy = updatedBy;
                entity.UpdatedOn = updatedOn;

                var oneDetail = requestDto.Details.Where(x => x.Id == entity.Id).FirstOrDefault();
                if (oneDetail == null) continue;

                var requestAttachments = oneDetail.Attachments;


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
                        sampleDetailAttachmentEntities.Add(new EquMaintenanceTaskItemAttachmentEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = entity.SiteId,
                            MaintenanceTaskId = requestDto.MaintenanceTaskId,
                            MaintenanceTaskItemId = entity.Id,
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
            var beforeAttachments = await _equMaintenanceTaskItemAttachmentRepository.GetEntitiesAsync(new EquMaintenanceTaskItemAttachmentQuery
            {
                SiteId = site,
                MaintenanceTaskId = requestDto.MaintenanceTaskId,
            });

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _equMaintenanceTaskItemRepository.UpdateRangeAsync(entitys);

            // 先删除再添加
            if (beforeAttachments != null && beforeAttachments.Any())
            {
                rows += await _equMaintenanceTaskItemAttachmentRepository.DeletesAsync(new DeleteCommand
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

            if (attachmentEntities.Any())
            {
                rows += await _inteAttachmentRepository.InsertRangeAsync(attachmentEntities);
                rows += await _equMaintenanceTaskItemAttachmentRepository.InsertRangeAsync(sampleDetailAttachmentEntities);
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
        public async Task<int> CompleteOrderAsync(EquMaintenanceTaskCompleteDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            //单据
            var entity = await _equMaintenanceTaskRepository.GetByIdAsync(requestDto.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 只有"检验中"的状态才允许点击"完成"
            if (entity.Status != EquMaintenanceTaskStautusEnum.Inspecting)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11912))
                    .WithData("Before", EquMaintenanceTaskStautusEnum.Inspecting.GetDescription())
                    .WithData("After", EquMaintenanceTaskStautusEnum.Completed.GetDescription());
            }
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 检查每种类型是否已经录入足够
            //var sampleEntities = await _equMaintenanceTaskItemRepository.GetEntitiesAsync(new QualFqcOrderSampleQuery
            //{
            //    SiteId = entity.SiteId,
            //    FQCOrderId = entity.Id
            //});

            ////校验已检数量

            //if (sampleEntities.Count() < entity.SampleQty)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES11716)).WithData("CheckedQty", sampleEntities.Count()).WithData("SampleQty", entity.SampleQty);
            //}

            // 读取所有明细参数
            var sampleDetailEntities = await _equMaintenanceTaskItemRepository.GetEntitiesAsync(new EquMaintenanceTaskItemQuery
            {
                SiteId = entity.SiteId,
                MaintenanceTaskId = entity.Id
            });

            var operationType = EquMaintenanceOperationTypeEnum.Complete;

            //检验值是否为空
            if (sampleDetailEntities.Any(x => string.IsNullOrEmpty(x.InspectionValue)))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15902));
            }

            //有任一不合格，完成
            if (sampleDetailEntities.Any(X => X.IsQualified == TrueOrFalseEnum.No))
            {
                entity.Status = EquMaintenanceTaskStautusEnum.Completed;
                entity.IsQualified = TrueOrFalseEnum.No;
                operationType = EquMaintenanceOperationTypeEnum.Complete;
            }
            else
            {
                // 默认是关闭
                entity.Status = EquMaintenanceTaskStautusEnum.Closed;
                entity.IsQualified = TrueOrFalseEnum.Yes;
                operationType = EquMaintenanceOperationTypeEnum.Close;
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
        public async Task<int> CloseOrderAsync(EquMaintenanceTaskCloseDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            //点检单
            var entity = await _equMaintenanceTaskRepository.GetByIdAsync(requestDto.MaintenanceTaskId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 只有"已检验"的状态才允许"关闭"
            if (entity.Status != EquMaintenanceTaskStautusEnum.Completed)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11912))
                    .WithData("Before", EquMaintenanceTaskStautusEnum.Completed.GetDescription())
                    .WithData("After", "结果处理");
                    //.WithData("After", EquMaintenanceTaskStautusEnum.Closed.GetDescription());
            }

            // 不合格处理完成之后直接关闭（无需变为合格）
            entity.Status = EquMaintenanceTaskStautusEnum.Closed;

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await CommonOperationAsync(entity, EquMaintenanceOperationTypeEnum.Close, new MaintananceTaskHandleBo { HandMethod = requestDto.HandMethod, Remark = requestDto.Remark });
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
        private async Task<int> CommonOperationAsync(EquMaintenanceTaskEntity entity, EquMaintenanceOperationTypeEnum operationType, MaintananceTaskHandleBo? handleBo = null)
        {
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 更新
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            if (operationType == EquMaintenanceOperationTypeEnum.Start)
            {
                entity.BeginTime = updatedOn;
            }
            else
            {
                entity.EndTime = updatedOn;
            }


            var rows = 0;
            rows += await _equMaintenanceTaskRepository.UpdateAsync(entity);
            rows += await _equMaintenanceTaskOperationRepository.InsertAsync(new EquMaintenanceTaskOperationEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                MaintenanceTaskId = entity.Id,
                OperationType = operationType,
                OperationBy = updatedBy,
                OperationOn = updatedOn,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            });

            if (handleBo != null) rows += await _equMaintenanceTaskProcessedRepository.InsertAsync(new EquMaintenanceTaskProcessedEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                MaintenanceTaskId = entity.Id,
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
        public async Task<int> SaveAttachmentAsync(EquMaintenanceTaskSaveAttachmentDto requestDto)
        {
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            //单据
            var entity = await _equMaintenanceTaskRepository.GetByIdAsync(requestDto.OrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES11714));

            if (!requestDto.Attachments.Any()) return 0;

            List<InteAttachmentEntity> inteAttachmentEntities = new();
            List<EquMaintenanceTaskAttachmentEntity> orderAttachmentEntities = new();
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

                orderAttachmentEntities.Add(new EquMaintenanceTaskAttachmentEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = entity.SiteId,
                    MaintenanceTaskId = requestDto.OrderId,
                    AttachmentId = attachmentId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });
            }

            var rows = 0;
            try
            {
                using var trans = TransactionHelper.GetTransactionScope();
                rows += await _inteAttachmentRepository.InsertRangeAsync(inteAttachmentEntities);
                rows += await _equMaintenanceTaskAttachmentRepository.InsertRangeAsync(orderAttachmentEntities);
                trans.Complete();
            }catch(Exception ex) { }
            return rows;
        }

        /// <summary>
        /// 删除检验单附件
        /// </summary>
        /// <param name="orderAttachmentId"></param>
        /// <returns></returns>
        public async Task<int> DeleteAttachmentByIdAsync(long orderAttachmentId)
        {
            var attachmentEntity = await _equMaintenanceTaskAttachmentRepository.GetByIdAsync(orderAttachmentId);
            if (attachmentEntity == null) return default;

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _inteAttachmentRepository.DeleteAsync(attachmentEntity.AttachmentId);
            rows += await _equMaintenanceTaskAttachmentRepository.DeleteAsync(attachmentEntity.Id);
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
            var orderAttachments = await _equMaintenanceTaskAttachmentRepository.GetByOrderIdAsync(orderId);
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
