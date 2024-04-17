using FluentValidation;
using FluentValidation.Validators;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（点检记录表） 
    /// </summary>
    public class EquInspectionRecordService : IEquInspectionRecordService
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
        private readonly AbstractValidator<EquInspectionRecordSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（点检记录表）
        /// </summary>
        private readonly IEquInspectionRecordRepository _equInspectionRecordRepository;
        private readonly IEquInspectionRecordDetailsRepository _recordDetailsRepository;
        private readonly IEquInspectionRecordOperateRepository _recordOperateRepository;
        private readonly IEquInspectionTaskSnapshootRepository _taskSnapshootRepository;
        private readonly IEquInspectionTaskDetailsSnapshootRepository _detailsSnapshootRepository;
        private readonly IEquInspectionItemRepository _inspectionItemRepository;

        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IEquEquipmentRepository _equipmentRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquInspectionRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<EquInspectionRecordSaveDto> validationSaveRules,
            IEquInspectionRecordRepository equInspectionRecordRepository, IEquInspectionRecordDetailsRepository recordDetailsRepository,
            IEquInspectionRecordOperateRepository recordOperateRepository, IEquInspectionTaskSnapshootRepository taskSnapshootRepository,
            IEquInspectionTaskDetailsSnapshootRepository detailsSnapshootRepository, IEquInspectionItemRepository inspectionItemRepository,
            IInteWorkCenterRepository inteWorkCenterRepository, IEquEquipmentRepository equipmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equInspectionRecordRepository = equInspectionRecordRepository;
            _recordDetailsRepository = recordDetailsRepository;
            _recordOperateRepository = recordOperateRepository;
            _taskSnapshootRepository = taskSnapshootRepository;
            _detailsSnapshootRepository = detailsSnapshootRepository;
            _inspectionItemRepository = inspectionItemRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _equipmentRepository = equipmentRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquInspectionRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquInspectionRecordEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _equInspectionRecordRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquInspectionRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquInspectionRecordEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _equInspectionRecordRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equInspectionRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _equInspectionRecordRepository.DeletesAsync(new DeleteCommand
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
        public async Task<EquInspectionRecordDto?> QueryByIdAsync(long id)
        {
            var equInspectionRecordEntity = await _equInspectionRecordRepository.GetByIdAsync(id);
            if (equInspectionRecordEntity == null) return null;

            return equInspectionRecordEntity.ToModel<EquInspectionRecordDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquInspectionRecordDto>> GetPagedListAsync(EquInspectionRecordPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquInspectionRecordPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equInspectionRecordRepository.GetPagedListAsync(pagedQuery);

            List<EquInspectionRecordDto> recordDtos = new List<EquInspectionRecordDto>();
            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return new PagedInfo<EquInspectionRecordDto>(recordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
            }

            var recordList = pagedInfo.Data.Select(s => s.ToModel<EquInspectionRecordDto>());
            //var workCenterIds = pagedInfo.Data.Select(x => x.WorkCenterId).ToArray();
            //var workCenterEntities = await _inteWorkCenterRepository.GetByIdsAsync(workCenterIds);
            //foreach (var data in pagedInfo.Data)
            //{
            //    var model = data.ToModel<EquInspectionRecordDto>();
            //    model.WorkCenterCode = workCenterEntities.FirstOrDefault(x => x.Id == data.WorkCenterId)?.Code ?? "";
            //    recordDtos.Add(model);
            //}

            return new PagedInfo<EquInspectionRecordDto>(recordList, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquInspectionOperateDto?> QueryByRecordIdAsync(long id)
        {
            var entity = await _equInspectionRecordRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return null;
            }

            //任务是否在校验中
            if (entity.Status != EquInspectionRecordStatusEnum.Inspecting)
            {
                await StartVerificationAsync(entity);
            }

            var recordId = entity.Id;
            var taskItemDtos = new List<EquInspectioTaskItemDto>();
            var operateDto = new EquInspectionOperateDto
            {
                Id = entity.Id,
                OrderCode = entity.OrderCode,
                Remark = entity.Remark,
                IsQualified=entity.IsQualified??false,
                IsNoticeRepair=entity.IsNoticeRepair??false,
                TaskItemDtos = taskItemDtos
            };

            var taskSnapshootId = entity.InspectionTaskSnapshootId;
            //查询任务
            var taskSnapshootEntity = await _taskSnapshootRepository.GetByIdAsync(taskSnapshootId);
            if (taskSnapshootEntity == null)
            {
                return operateDto;
            }

            //读取设备信息
            var equEquipmentEntity = await _equipmentRepository.GetByIdAsync(taskSnapshootEntity.EquipmentId);
            operateDto.EquipmentCode = equEquipmentEntity?.EquipmentCode ?? "";
            operateDto.EquipmentName = equEquipmentEntity?.EquipmentName ?? "";

            //查询操作信息
            var recordOperateEntities = await _recordOperateRepository.GetEntitiesAsync(new EquInspectionRecordOperateQuery
            {
                InspectionRecordId = recordId
            });
            if (recordOperateEntities != null && recordOperateEntities.Any())
            {
                operateDto.OperateBy = recordOperateEntities.FirstOrDefault()?.OperateBy ?? "";
            }

            //查询任务详情
            var equInspectionTaskDetails = await _detailsSnapshootRepository.GetEntitiesAsync(new EquInspectionTaskDetailsSnapshootQuery
            {
                InspectionTaskId = taskSnapshootEntity.Id
            });

            //查询项目信息
            var itemIds = equInspectionTaskDetails.Select(x => x.InspectionItemId).ToArray();
            var equInspectionItems = await _inspectionItemRepository.GetByIdsAsync(itemIds);

            //查询记录详情信息
            var equInspectionRecords = await _recordDetailsRepository.GetEntitiesAsync(new EquInspectionRecordDetailsQuery
            {
                InspectionRecordId = recordId
            });
            foreach (var item in equInspectionItems)
            {
                var inspectionTaskDetailSnapshootId = equInspectionTaskDetails.FirstOrDefault(x => x.InspectionItemId == item.Id)?.Id ?? 0;
                var operation = equInspectionRecords.FirstOrDefault(x => x.InspectionTaskDetailSnapshootId == inspectionTaskDetailSnapshootId);
                taskItemDtos.Add(new EquInspectioTaskItemDto
                {
                    InspectionRecordDetailId = operation?.Id ?? 0,
                    Code = item.Code,
                    Name = item.Name,
                    OperationMethod = item.OperationMethod,
                    OperationContent = item.OperationContent,
                    InspectionResult = operation?.InspectionResult ?? "",
                    IsQualified = operation?.IsQualified,
                    Remark = operation?.Remark ?? ""
                });
            }
            return operateDto;
        }

        /// <summary>
        /// 开始校验
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> StartVerificationAsync(EquInspectionCompleteDto saveDto)
        {
            // 判断是否有获取到站点码
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            var recordId = saveDto.Id;
            //找不到记录
            var recordEntity = await _equInspectionRecordRepository.GetByIdAsync(recordId);
            if (recordEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            return await StartVerificationAsync(recordEntity);
        }

        private async Task<int> StartVerificationAsync(EquInspectionRecordEntity recordEntity)
        {
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            //操作记录校验中
            recordEntity.UpdatedOn = updatedOn;
            recordEntity.UpdatedBy = updatedBy;
            recordEntity.Status = Core.Enums.EquInspectionRecordStatusEnum.Inspecting;

            //插入开始校验操作
            var recordOperateEntity = new EquInspectionRecordOperateEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                InspectionRecordId = recordEntity.Id,
                OperateType = OrderOperateTypeEnum.Start,
                OperateBy = updatedBy,
                OperateOn = updatedOn,
                Remark = "",
                CreatedBy = updatedBy,
                UpdatedBy = updatedBy,
                SiteId = _currentSite.SiteId ?? 0,
            };

            var rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                rows += await _equInspectionRecordRepository.UpdateAsync(recordEntity);
                rows += await _recordOperateRepository.InsertAsync(recordOperateEntity);
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 保存校验
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveVerificationnAsync(EquInspectionSaveDto saveDto)
        {
            // 判断是否有获取到站点码
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            var recordId = saveDto.Id;
            //找不到记录
            var recordEntity = await _equInspectionRecordRepository.GetByIdAsync(recordId);
            if (recordEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            var isQualifiedCount = saveDto.TaskItemDtos.Count(x => x.IsQualified == false);
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            recordEntity.IsNoticeRepair = saveDto.IsNoticeRepair;
            recordEntity.IsQualified = isQualifiedCount>0? false:true;
            recordEntity.Remark = saveDto.Remark;
            recordEntity.UpdatedOn = updatedOn;
            recordEntity.UpdatedBy = updatedBy;

            var updateRecordDetails = new List<EquInspectionRecordDetailsEntity>();
            foreach (var item in saveDto.TaskItemDtos)
            {
                updateRecordDetails.Add(new EquInspectionRecordDetailsEntity
                {
                    Id = item.InspectionRecordDetailId,
                    InspectionResult = item.InspectionResult,
                    IsQualified = item.IsQualified,
                    Remark = item.Remark,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });
            }

            var rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                rows += await _equInspectionRecordRepository.UpdateAsync(recordEntity);
                rows += await _recordDetailsRepository.UpdateRangeAsync(updateRecordDetails);
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 完成检验单
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CompleteVerificationAsync(EquInspectionCompleteDto saveDto)
        {
            // 判断是否有获取到站点码
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            var recordId = saveDto.Id;
            //找不到记录
            var recordEntity = await _equInspectionRecordRepository.GetByIdAsync(recordId);
            if (recordEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            if (!recordEntity.IsQualified.HasValue)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15805));
            }

            // 只有"检验中"的状态才允许点击"完成"
            if (recordEntity.Status != EquInspectionRecordStatusEnum.Inspecting)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11912))
                    .WithData("Before", EquInspectionRecordStatusEnum.Inspecting.GetDescription())
                    .WithData("After", EquInspectionRecordStatusEnum.Completed.GetDescription());
            }


            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            //记录表的更新时间
            recordEntity.Status = EquInspectionRecordStatusEnum.Completed;
            recordEntity.UpdatedOn = updatedOn;
            recordEntity.UpdatedBy = updatedBy;

            //插入完成校验操作
            var recordOperateEntity = new EquInspectionRecordOperateEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                InspectionRecordId = recordEntity.Id,
                OperateType = OrderOperateTypeEnum.Complete,
                OperateBy = updatedBy,
                OperateOn = updatedOn,
                Remark = "",
                CreatedBy = updatedBy,
                UpdatedBy = updatedBy,
                SiteId = _currentSite.SiteId ?? 0,
            };

            var rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                rows += await _equInspectionRecordRepository.UpdateAsync(recordEntity);
                rows += await _recordOperateRepository.InsertAsync(recordOperateEntity);
                ts.Complete();
            }
            return rows;
        }
    }
}
