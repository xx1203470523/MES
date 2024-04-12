using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Services.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Sequences;
using Hymson.Sequences.Enums;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Data;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 服务（消息管理） 
    /// </summary>
    public class InteMessageManageService : IInteMessageManageService
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
        /// 生成序列码
        /// </summary>
        private readonly ISequenceService _sequenceService;

        /// <summary>
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<InteMessageManageTriggerSaveDto> _validationSaveRules;

        /// <summary>
        /// 消息服务
        /// </summary>
        private readonly IMessagePushService _messagePushService;

        /// <summary>
        /// 仓储接口（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 仓储接口（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 仓储接口（工作中心）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

        /// <summary>
        /// 仓储接口（事件维护）
        /// </summary>
        private readonly IInteEventRepository _inteEventRepository;

        /// <summary>
        /// 仓储接口（附件）
        /// </summary>
        private readonly IInteAttachmentRepository _inteAttachmentRepository;

        /// <summary>
        /// 仓储接口（消息管理）
        /// </summary>
        private readonly IInteMessageManageRepository _inteMessageManageRepository;

        /// <summary>
        /// 仓储接口（消息管理分析报告附件）
        /// </summary>
        private readonly IInteMessageManageAnalysisReportAttachmentRepository _inteMessageManageAnalysisReportAttachmentRepository;

        /// <summary>
        /// 仓储接口（消息管理处理报告附件）
        /// </summary>
        private readonly IInteMessageManageHandleProgrammeAttachmentRepository _inteMessageManageHandleProgrammeAttachmentRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="sequenceService"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="messagePushService"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="equEquipmentRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="inteEventRepository"></param>
        /// <param name="inteAttachmentRepository"></param>
        /// <param name="inteMessageManageRepository"></param>
        /// <param name="inteMessageManageAnalysisReportAttachmentRepository"></param>
        /// <param name="inteMessageManageHandleProgrammeAttachmentRepository"></param>
        public InteMessageManageService(ICurrentUser currentUser, ICurrentSite currentSite, ISequenceService sequenceService,
            AbstractValidator<InteMessageManageTriggerSaveDto> validationSaveRules,
            IMessagePushService messagePushService,
            IProcResourceRepository procResourceRepository,
            IEquEquipmentRepository equEquipmentRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IInteEventRepository inteEventRepository,
            IInteAttachmentRepository inteAttachmentRepository,
            IInteMessageManageRepository inteMessageManageRepository,
            IInteMessageManageAnalysisReportAttachmentRepository inteMessageManageAnalysisReportAttachmentRepository,
            IInteMessageManageHandleProgrammeAttachmentRepository inteMessageManageHandleProgrammeAttachmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _sequenceService = sequenceService;
            _validationSaveRules = validationSaveRules;
            _messagePushService = messagePushService;
            _procResourceRepository = procResourceRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _inteEventRepository = inteEventRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
            _inteMessageManageRepository = inteMessageManageRepository;
            _inteMessageManageAnalysisReportAttachmentRepository = inteMessageManageAnalysisReportAttachmentRepository;
            _inteMessageManageHandleProgrammeAttachmentRepository = inteMessageManageHandleProgrammeAttachmentRepository;
        }


        /// <summary>
        /// 触发
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> TriggerAsync(InteMessageManageTriggerSaveDto dto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(dto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = dto.ToEntity<InteMessageManageEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.Status = MessageStatusEnum.Trigger;

            // 判断事件类型是否是自动关闭
            var eventEntity = await _inteEventRepository.GetByIdAsync(entity.EventId);
            if (eventEntity != null && eventEntity.IsAutoClose == DisableOrEnableEnum.Enable)
            {
                entity.Status = MessageStatusEnum.Close;
            }

            // 保存
            var rows = await _inteMessageManageRepository.InsertAsync(entity);
            if (rows > 0) await _messagePushService.Push(entity);

            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteMessageManageTriggerSaveDto dto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(dto);

            // DTO转换实体
            var entity = dto.ToEntity<InteMessageManageEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            // 保存
            return await _inteMessageManageRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 接收
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> ReceiveAsync(InteMessageManageReceiveSaveDto dto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 查询实体
            var entity = await _inteMessageManageRepository.GetByIdAsync(dto.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            // 状态校验
            if (entity.Status != MessageStatusEnum.Trigger) throw new CustomerValidationException(nameof(ErrorCode.MES10903));

            // 更新实体
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.ReceivedBy = updatedBy;
            entity.ReceivedOn = updatedOn;
            entity.Status = MessageStatusEnum.Receive;
            entity.ReceiveDuration = Math.Ceiling((updatedOn - entity.CreatedOn).TotalMinutes);

            var rows = await _inteMessageManageRepository.ReceiveAsync(entity);
            if (rows > 0) await _messagePushService.Push(entity);

            return rows;
        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> HandleAsync(InteMessageManageHandleSaveDto dto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 这里可以新建验证器去验证
            if (dto.ReasonAnalysis.Any(x => char.IsWhiteSpace(x))) throw new CustomerValidationException(nameof(ErrorCode.MES10904));
            if (dto.HandleSolution.Any(x => char.IsWhiteSpace(x))) throw new CustomerValidationException(nameof(ErrorCode.MES10905));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 查询实体
            var entity = await _inteMessageManageRepository.GetByIdAsync(dto.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            // 状态校验
            if (entity.Status != MessageStatusEnum.Receive) throw new CustomerValidationException(nameof(ErrorCode.MES10903));

            // 更新实体
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.HandledBy = updatedBy;
            entity.HandledOn = updatedOn;
            entity.Status = MessageStatusEnum.Handle;
            entity.DepartmentId = dto.DepartmentId;
            entity.ResponsibleBy = dto.ResponsibleBy;
            entity.ReasonAnalysis = dto.ReasonAnalysis;
            entity.HandleSolution = dto.HandleSolution;
            entity.HandleRemark = dto.HandleRemark;
            if (entity.ReceivedOn.HasValue)
            {
                entity.HandleDuration = Math.Ceiling((updatedOn - entity.ReceivedOn.Value).TotalMinutes);
            }

            // 附件处理
            List<InteAttachmentEntity> attachmentEntities = new();
            List<InteMessageManageAnalysisReportAttachmentEntity> messageManageAnalysisReportAttachmentEntities = new();
            List<InteMessageManageHandleProgrammeAttachmentEntity> messageManageHandleProgrammeAttachmentEntities = new();

            #region 附件
            // 原因分析（附件）
            if (dto.ReasonAttachments != null)
            {
                foreach (var item in dto.ReasonAttachments)
                {
                    var attachmentId = IdGenProvider.Instance.CreateId();
                    attachmentEntities.Add(new InteAttachmentEntity
                    {
                        Id = attachmentId,
                        Name = item.Name,
                        Path = item.Path,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        SiteId = entity.SiteId,
                    });

                    messageManageAnalysisReportAttachmentEntities.Add(new InteMessageManageAnalysisReportAttachmentEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        MessageManageId = entity.Id,
                        AttachmentId = attachmentId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        SiteId = entity.SiteId,
                    });
                }
            }

            // 处理方案（附件）
            if (dto.HandleAttachments != null)
            {
                foreach (var item in dto.HandleAttachments)
                {
                    var attachmentId = IdGenProvider.Instance.CreateId();
                    attachmentEntities.Add(new InteAttachmentEntity
                    {
                        Id = attachmentId,
                        Name = item.Name,
                        Path = item.Path,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        SiteId = entity.SiteId,
                    });

                    messageManageHandleProgrammeAttachmentEntities.Add(new InteMessageManageHandleProgrammeAttachmentEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        MessageManageId = entity.Id,
                        AttachmentId = attachmentId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        SiteId = entity.SiteId,
                    });
                }
            }
            #endregion

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _inteMessageManageRepository.HandleAsync(entity);

                if (attachmentEntities.Any())
                {
                    rows += await _inteAttachmentRepository.InsertRangeAsync(attachmentEntities);
                }

                if (messageManageAnalysisReportAttachmentEntities.Any())
                {
                    rows += await _inteMessageManageAnalysisReportAttachmentRepository.InsertRangeAsync(messageManageAnalysisReportAttachmentEntities);
                }

                if (messageManageHandleProgrammeAttachmentEntities.Any())
                {
                    rows += await _inteMessageManageHandleProgrammeAttachmentRepository.InsertRangeAsync(messageManageHandleProgrammeAttachmentEntities);
                }

                trans.Complete();
            }

            // 推送消息
            if (rows > 0) await _messagePushService.Push(entity);

            return rows;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> CloseAsync(InteMessageManageCloseSaveDto dto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 查询实体
            var entity = await _inteMessageManageRepository.GetByIdAsync(dto.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            // 状态校验
            if (entity.Status != MessageStatusEnum.Handle) throw new CustomerValidationException(nameof(ErrorCode.MES10903));

            // 更新实体
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.EvaluateBy = updatedBy;
            entity.EvaluateOn = $"{updatedOn:yyyy-MM-dd HH:mm:ss}";
            entity.Status = MessageStatusEnum.Close;
            entity.EvaluateRemark = dto.EvaluateRemark;

            var rows = await _inteMessageManageRepository.CloseAsync(entity);
            if (rows > 0) await _messagePushService.Push(entity);

            return rows;
        }

        /// <summary>
        /// 查询详情（消息管理）（触发）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteMessageManageTriggerDto?> QueryTriggerByIdAsync(long id)
        {
            var inteMessageManageEntity = await _inteMessageManageRepository.GetByIdAsync(id);
            if (inteMessageManageEntity == null) return null;

            var dto = inteMessageManageEntity.ToModel<InteMessageManageTriggerDto>();

            var eventEntity = await _inteEventRepository.GetByIdAsync(dto.EventId);
            if (eventEntity != null) dto.EventName = eventEntity.Name;

            if (dto.ResourceId.HasValue)
            {
                var resourceEntity = await _procResourceRepository.GetByIdAsync(dto.ResourceId.Value);
                if (resourceEntity != null) dto.ResourceCode = resourceEntity.ResCode;
            }
            return dto;
        }

        /// <summary>
        /// 查询详情（消息管理）（处理）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteMessageManageHandleDto?> QueryHandleByIdAsync(long id)
        {
            var inteMessageManageEntity = await _inteMessageManageRepository.GetByIdAsync(id);
            if (inteMessageManageEntity == null) return null;

            var dto = inteMessageManageEntity.ToModel<InteMessageManageHandleDto>();

            // 读取附件
            var messageManageAnalysisReportAttachmentEntities = await _inteMessageManageAnalysisReportAttachmentRepository.GetEntitiesAsync(new InteMessageManageAnalysisReportAttachmentQuery { MessageManageId = inteMessageManageEntity.Id });
            var messageManageHandleProgrammeAttachmentEntities = await _inteMessageManageHandleProgrammeAttachmentRepository.GetEntitiesAsync(new InteMessageManageHandleProgrammeAttachmentQuery { MessageManageId = inteMessageManageEntity.Id });

            var reasonAttachmentIds = messageManageAnalysisReportAttachmentEntities.Select(x => x.AttachmentId).ToList();
            var handleAttachmentIds = messageManageHandleProgrammeAttachmentEntities.Select(x => x.AttachmentId).ToList();

            var attachmentIds = reasonAttachmentIds.Union(handleAttachmentIds);
            var attachmentEntities = await _inteAttachmentRepository.GetByIdsAsync(attachmentIds);

            dto.ReasonAttachments = attachmentEntities.Where(x => reasonAttachmentIds.Contains(x.Id)).Select(x => x.ToModel<InteAttachmentBaseDto>());
            dto.HandleAttachments = attachmentEntities.Where(x => handleAttachmentIds.Contains(x.Id)).Select(x => x.ToModel<InteAttachmentBaseDto>());

            return dto;
        }

        /// <summary>
        /// 查询详情（消息管理）（关闭）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteMessageManageCloseDto?> QueryCloseByIdAsync(long id)
        {
            var inteMessageManageEntity = await _inteMessageManageRepository.GetByIdAsync(id);
            if (inteMessageManageEntity == null) return null;

            return inteMessageManageEntity.ToModel<InteMessageManageCloseDto>();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _inteMessageManageRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            if (ids.Length == 0) return 0;

            return await _inteMessageManageRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteMessageManageDto>> GetPagedListAsync(InteMessageManagePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<InteMessageManagePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteMessageManageRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            List<InteMessageManageDto> dtos = new();
            foreach (var item in pagedInfo.Data)
            {
                var dto = item.ToModel<InteMessageManageDto>();

                var eventEntity = await _inteEventRepository.GetByIdAsync(dto.EventId);
                if (eventEntity != null) dto.EventName = eventEntity.Name;

                var workShopEntity = await _inteWorkCenterRepository.GetByIdAsync(dto.WorkShopId);
                if (workShopEntity != null) dto.WorkShopName = workShopEntity.Code;

                var workLineEntity = await _inteWorkCenterRepository.GetByIdAsync(dto.LineId);
                if (workLineEntity != null) dto.LineName = workLineEntity.Code;

                if (dto.EquipmentId.HasValue)
                {
                    var equipmentEntity = await _equEquipmentRepository.GetByIdAsync(dto.EquipmentId.Value);
                    if (equipmentEntity != null) dto.EquipmentName = equipmentEntity.EquipmentName;
                }

                dtos.Add(dto);
            }

            return new PagedInfo<InteMessageManageDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取消息编号
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCodeAsync()
        {
            const string businessKey = "InteMessageManageCode";
            var serialNumbers = await _sequenceService.GetSerialNumberAsync(SerialNumberTypeEnum.ByDay, businessKey, 0, 1);

            var padNo = $"{serialNumbers}".PadLeft(4, '0');
            return $"EVENT{DateTime.Now:yyyyMMdd}{padNo}";
        }




    }
}
