using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Sequences;
using Hymson.Sequences.Enums;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

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
        private readonly AbstractValidator<InteMessageManageTriggerDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（工作中心）
        /// </summary>
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;

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
        /// <param name="inteWorkCenterRepository"></param>
        /// <param name="inteAttachmentRepository"></param>
        /// <param name="inteMessageManageRepository"></param>
        /// <param name="inteMessageManageAnalysisReportAttachmentRepository"></param>
        /// <param name="inteMessageManageHandleProgrammeAttachmentRepository"></param>
        public InteMessageManageService(ICurrentUser currentUser, ICurrentSite currentSite, ISequenceService sequenceService,
            AbstractValidator<InteMessageManageTriggerDto> validationSaveRules,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IInteAttachmentRepository inteAttachmentRepository,
            IInteMessageManageRepository inteMessageManageRepository,
            IInteMessageManageAnalysisReportAttachmentRepository inteMessageManageAnalysisReportAttachmentRepository,
            IInteMessageManageHandleProgrammeAttachmentRepository inteMessageManageHandleProgrammeAttachmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _sequenceService = sequenceService;
            _validationSaveRules = validationSaveRules;
            _inteWorkCenterRepository = inteWorkCenterRepository;
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
        public async Task<int> TriggerAsync(InteMessageManageTriggerDto dto)
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
            entity.Status = MessageStatusEnum.Trigger;
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _inteMessageManageRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 接收
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> ReceiveAsync(InteMessageManageReceiveDto dto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // DTO转换实体
            var entity = dto.ToEntity<InteMessageManageEntity>();
            entity.Status = MessageStatusEnum.Receive;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _inteMessageManageRepository.ReceiveAsync(entity);
        }

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> HandleAsync(InteMessageManageHandleDto dto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = dto.ToEntity<InteMessageManageEntity>();
            entity.Status = MessageStatusEnum.Handle;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            // 附件处理
            List<InteAttachmentEntity> attachmentEntities = new();
            List<InteMessageManageAnalysisReportAttachmentEntity> InteMessageManageAnalysisReportAttachmentEntities = new();
            List<InteMessageManageHandleProgrammeAttachmentEntity> InteMessageManageHandleProgrammeAttachmentEntities = new();

            // 原因分析（附件）
            //dto.ReasonAttachments ??= new List<string>();
            // TODO

            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                var rowArray = await Task.WhenAll(new List<Task<int>>()
                {
                    _inteMessageManageRepository.HandleAsync(entity),
                    _inteAttachmentRepository.InsertRangeAsync(attachmentEntities),
                    _inteMessageManageAnalysisReportAttachmentRepository.InsertRangeAsync(InteMessageManageAnalysisReportAttachmentEntities),
                    _inteMessageManageHandleProgrammeAttachmentRepository.InsertRangeAsync(InteMessageManageHandleProgrammeAttachmentEntities)
                });
                rows += rowArray.Sum();
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> CloseAsync(InteMessageManageCloseDto dto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = dto.ToEntity<InteMessageManageEntity>();
            entity.Status = MessageStatusEnum.Close;
            entity.EvaluateBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            return await _inteMessageManageRepository.CloseAsync(entity); ;
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
            return await _inteMessageManageRepository.DeletesAsync(new DeleteCommand
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
        public async Task<InteMessageManageDto?> QueryByIdAsync(long id)
        {
            var inteMessageManageEntity = await _inteMessageManageRepository.GetByIdAsync(id);
            if (inteMessageManageEntity == null) return null;

            return inteMessageManageEntity.ToModel<InteMessageManageDto>();
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

                var workShopEntity = await _inteWorkCenterRepository.GetByIdAsync(dto.WorkShopId);
                if (workShopEntity != null) dto.WorkShopName = workShopEntity.Name;

                var workLineEntity = await _inteWorkCenterRepository.GetByIdAsync(dto.LineId);
                if (workLineEntity != null) dto.LineName = workLineEntity.Name;

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
