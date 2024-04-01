using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Parameter;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.CoreServices.Services.Parameter;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.IdentityModel.Tokens;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（尾检检验单） 
    /// </summary>
    public class QualIpqcInspectionTailService : IQualIpqcInspectionTailService
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
        private readonly AbstractValidator<QualIpqcInspectionTailSaveDto> _validationSaveRules;
        private readonly AbstractValidator<List<QualIpqcInspectionTailSampleCreateDto>> _validationSampleAddRules;

        /// <summary>
        /// 仓储接口（尾检检验单）
        /// </summary>
        private readonly IQualIpqcInspectionTailRepository _qualIpqcInspectionTailRepository;
        private readonly IQualIpqcInspectionTailSampleRepository _qualIpqcInspectionTailSampleRepository;
        private readonly IQualIpqcInspectionTailAnnexRepository _qualIpqcInspectionTailAnnexRepository;
        private readonly IQualIpqcInspectionRepository _qualIpqcInspectionRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        private readonly IInteAttachmentRepository _inteAttachmentRepository;
        private readonly IQualIpqcInspectionParameterRepository _qualIpqcInspectionParameterRepository;
        private readonly IProcParameterRepository _procParameterRepository;
        private readonly IManuProductParameterService _manuProductParameterService;
        private readonly ISequenceService _sequenceService;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QualIpqcInspectionTailService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<QualIpqcInspectionTailSaveDto> validationSaveRules,
            AbstractValidator<List<QualIpqcInspectionTailSampleCreateDto>> validationSampleAddRules,
            IQualIpqcInspectionTailRepository qualIpqcInspectionTailRepository,
            IQualIpqcInspectionTailSampleRepository qualIpqcInspectionTailSampleRepository,
            IQualIpqcInspectionTailAnnexRepository qualIpqcInspectionTailAnnexRepository,
            IQualIpqcInspectionRepository qualIpqcInspectionRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceRepository procResourceRepository,
            IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository,
            IEquEquipmentRepository equEquipmentRepository,
            IInteAttachmentRepository inteAttachmentRepository,
            IQualIpqcInspectionParameterRepository qualIpqcInspectionParameterRepository,
            IProcParameterRepository procParameterRepository,
            IManuProductParameterService manuProductParameterService,
            ISequenceService sequenceService, IManuSfcInfoRepository manuSfcInfoRepository,
            IManuSfcRepository manuSfcRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _validationSampleAddRules = validationSampleAddRules;
            _qualIpqcInspectionTailRepository = qualIpqcInspectionTailRepository;
            _qualIpqcInspectionTailSampleRepository = qualIpqcInspectionTailSampleRepository;
            _qualIpqcInspectionTailAnnexRepository = qualIpqcInspectionTailAnnexRepository;
            _qualIpqcInspectionRepository = qualIpqcInspectionRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _procResourceRepository = procResourceRepository;
            _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
            _qualIpqcInspectionParameterRepository = qualIpqcInspectionParameterRepository;
            _procParameterRepository = procParameterRepository;
            _manuProductParameterService = manuProductParameterService;
            _sequenceService = sequenceService;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(QualIpqcInspectionTailSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            //工单
            var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(saveDto.WorkOrderId);
            if (workOrderEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13221));
            }
            //校验工序、资源是否匹配
            var procedureResourceList = await _procResourceRepository.GetProcResourceListByProcedureIdAsync(saveDto.ProcedureId);
            if (procedureResourceList.IsNullOrEmpty() || !procedureResourceList.Any(x => x.Id == saveDto.ResourceId))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13222));
            }
            //获取检验项目
            var ipqcInspectionList = await _qualIpqcInspectionRepository.GetEntitiesAsync(new QualIpqcInspectionQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Type = IPQCTypeEnum.QTI,
                MaterialId = workOrderEntity.ProductId,
                ProcedureId = saveDto.ProcedureId,
                GenerateConditionUnit = saveDto.TriggerCondition,
                Status = SysDataStatusEnum.Enable
            });
            if (ipqcInspectionList.IsNullOrEmpty())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13223));
            }
            if (ipqcInspectionList.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13224));
            }
            var ipqcInspection = ipqcInspectionList.First();

            //主设备Id
            var equipmentId = (await _procResourceEquipmentBindRepository.GetByResourceIdAsync(new ProcResourceEquipmentBindQuery
            {
                ResourceId = saveDto.ResourceId,
                IsMain = true
            }))
            .FirstOrDefault()?.EquipmentId ?? 0;
            //流水号
            var sequence = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.ByDay, "QTI");

            // DTO转换实体
            var entity = new QualIpqcInspectionTailEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                InspectionOrder = $"QTI{updatedOn.ToString("yyyy-MM-dd")}{sequence.ToString().PadLeft(4, '0')}",
                IpqcInspectionId = ipqcInspection.Id,
                WorkOrderId = workOrderEntity.Id,
                MaterialId = workOrderEntity.ProductId,
                ProcedureId = saveDto.ProcedureId,
                ResourceId = saveDto.ResourceId,
                EquipmentId = equipmentId,
                SampleQty = ipqcInspection.SampleQty,
                Status = InspectionStatusEnum.WaitInspect,
                InspectionBy = updatedBy,
                InspectionOn = updatedOn,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };

            // 保存
            return await _qualIpqcInspectionTailRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            var entities = await _qualIpqcInspectionTailRepository.GetByIdsAsync(ids);
            if (entities != null && entities.Any(x => x.Status != InspectionStatusEnum.WaitInspect))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13233));
            }
            return await _qualIpqcInspectionTailRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualIpqcInspectionTailDto?> QueryByIdAsync(long id)
        {
            var entity = await _qualIpqcInspectionTailRepository.GetByIdAsync(id);
            if (entity == null) return null;

            var dto = entity.ToModel<QualIpqcInspectionTailDto>();
            if (dto == null) return null;

            var workOrderTask = _planWorkOrderRepository.GetByIdAsync(entity.WorkOrderId);
            var materialTask = _procMaterialRepository.GetByIdAsync(entity.MaterialId);
            var procedureTask = _procProcedureRepository.GetByIdAsync(entity.ProcedureId);
            var resourceTask = _procResourceRepository.GetByIdAsync(entity.ResourceId);
            var equipmentTask = _equEquipmentRepository.GetByIdAsync(entity.EquipmentId);
            var qualIpqcInspectionTask = _qualIpqcInspectionRepository.GetByIdAsync(entity.IpqcInspectionId);
            var workOrder = await workOrderTask;
            var material = await materialTask;
            var procedure = await procedureTask;
            var resource = await resourceTask;
            var equipment = await equipmentTask;
            var qualIpqcInspection = await qualIpqcInspectionTask;
            if (workOrder != null)
            {
                dto.WorkOrderCode = workOrder.OrderCode;
                dto.WorkCenterCode = (await _inteWorkCenterRepository.GetByIdAsync(workOrder.WorkCenterId.GetValueOrDefault()))?.Code ?? "";
            }
            if (material != null)
            {
                dto.MaterialCode = material.MaterialCode;
                dto.MaterialName = material.MaterialName;
            }
            if (procedure != null)
            {
                dto.ProcedureCode = procedure.Code;
                dto.ProcedureName = procedure.Name;
            }
            if (resource != null)
            {
                dto.ResourceCode = resource.ResCode;
                dto.ResourceName = resource.ResName;
            }
            if (equipment != null)
            {
                dto.EquipmentCode = equipment.EquipmentCode;
                dto.EquipmentName = equipment.EquipmentName;
            }
            dto.InspectCount = await _qualIpqcInspectionTailSampleRepository.GetCountByIpqcInspectionId(id);
            if (qualIpqcInspection != null)
            {
                dto.GenerateConditionUnit = qualIpqcInspection.GenerateConditionUnit;
            }
            return dto;
        }

        /// <summary>
        /// 获取检验单已检样本列表
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIpqcInspectionTailSampleDto>> GetPagedSampleListAsync(QualIpqcInspectionTailSamplePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualIpqcInspectionTailSamplePagedQuery>();

            var pagedInfo = await _qualIpqcInspectionTailSampleRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<QualIpqcInspectionTailSampleDto>());
            return new PagedInfo<QualIpqcInspectionTailSampleDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据检验单ID获取检验单附件列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIpqcInspectionTailAnnexDto>?> GetAttachmentListAsync(long id)
        {
            var entities = await _qualIpqcInspectionTailAnnexRepository.GetEntitiesAsync(new QualIpqcInspectionTailAnnexQuery { InspectionOrderId = id });
            if (entities == null) return null;

            var attachments = await _inteAttachmentRepository.GetByIdsAsync(entities.Select(x => x.AnnexId));

            var dtos = entities.Select(item =>
            {
                var dto = item.ToModel<QualIpqcInspectionTailAnnexDto>();
                var attachment = attachments.FirstOrDefault(x => x.Id == item.AnnexId);
                if (attachment != null)
                {
                    dto.Name = attachment.Name;
                    dto.Path = attachment.Path;
                }
                return dto;
            });

            return dtos;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIpqcInspectionTailDto>> GetPagedListAsync(QualIpqcInspectionTailPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualIpqcInspectionTailPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualIpqcInspectionTailRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<QualIpqcInspectionTailDto>());
            return new PagedInfo<QualIpqcInspectionTailDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 执行检验
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(StatusChangeDto dto)
        {
            var entity = await _qualIpqcInspectionTailRepository.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }
            if (entity.Status == InspectionStatusEnum.Inspecting)
            {
                return 1;
            }
            if (entity.Status != InspectionStatusEnum.WaitInspect)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13229));
            }

            entity.Status = InspectionStatusEnum.Inspecting;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            entity.ExecuteBy = _currentUser.UserName;
            entity.ExecuteOn = HymsonClock.Now();

            // 保存
            return await _qualIpqcInspectionTailRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 样品检验数据录入
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public async Task<int> InsertSampleDataAsync(List<QualIpqcInspectionTailSampleCreateDto> dataList)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSampleAddRules.ValidateAndThrowAsync(dataList);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            var entitys = dataList.Select(item =>
            {
                var entity = item.ToEntity<QualIpqcInspectionTailSampleEntity>();
                entity.Id = IdGenProvider.Instance.CreateId();
                entity.IpqcInspectionTailId = item.IpqcInspectionPatrolId;
                entity.SiteId = _currentSite.SiteId ?? 0;
                entity.CreatedBy = updatedBy;
                entity.CreatedOn = updatedOn;
                entity.UpdatedBy = updatedBy;
                entity.UpdatedOn = updatedOn;
                return entity;
            });

            return await _qualIpqcInspectionTailSampleRepository.InsertRangeAsync(entitys);
        }

        /// <summary>
        /// 样品检验数据修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateSampleDataAsync(QualIpqcInspectionTailSampleUpdateDto param)
        {
            var entity = await _qualIpqcInspectionTailSampleRepository.GetByIdAsync(param.Id);
            if (entity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }
            //校验检验单状态
            var mainEntity = await _qualIpqcInspectionTailRepository.GetByIdAsync(entity.IpqcInspectionTailId);
            if (mainEntity != null && (mainEntity.Status == InspectionStatusEnum.Completed || mainEntity.Status == InspectionStatusEnum.Closed))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13229));
            }

            entity.InspectionValue = param.InspectionValue;
            entity.IsQualified = param.IsQualified;
            entity.Remark = param.Remark;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _qualIpqcInspectionTailSampleRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 检验完成
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> CompleteAsync(StatusChangeDto dto)
        {
            var entity = await _qualIpqcInspectionTailRepository.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }
            if (entity.Status != InspectionStatusEnum.Inspecting)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13230));
            }
            //获取已检样本数据
            var samples = await _qualIpqcInspectionTailSampleRepository.GetEntitiesAsync(new QualIpqcInspectionTailSampleQuery { InspectionOrderId = dto.Id });
            if (samples.IsNullOrEmpty() || samples.Select(x => x.Barcode).Distinct().Count() < entity.SampleQty)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13231));
            }

            entity.IsQualified = samples.Any(x => x.IsQualified != TrueOrFalseEnum.Yes) ? TrueOrFalseEnum.No : TrueOrFalseEnum.Yes;
            entity.Status = entity.IsQualified == TrueOrFalseEnum.Yes ? InspectionStatusEnum.Closed : InspectionStatusEnum.Completed;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            entity.CompleteOn = HymsonClock.Now();
            if (entity.IsQualified == TrueOrFalseEnum.Yes)
            {
                entity.CloseOn = HymsonClock.Now();
            }
            // 保存
            return await _qualIpqcInspectionTailRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 不合格处理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> UnqualifiedHandleAsync(UnqualifiedHandleDto dto)
        {
            var entity = await _qualIpqcInspectionTailRepository.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }
            if (entity.Status != InspectionStatusEnum.Completed)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13232));
            }

            entity.Status = InspectionStatusEnum.Closed;
            entity.HandMethod = dto.HandMethod;
            entity.ProcessedBy = _currentUser.UserName;
            entity.ProcessedOn = HymsonClock.Now();
            entity.Remark = dto.Remark;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();
            entity.CloseOn = HymsonClock.Now();
            // 保存
            return await _qualIpqcInspectionTailRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> AttachmentAddAsync(AttachmentAddDto dto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            List<InteAttachmentEntity> attachments = new();
            List<QualIpqcInspectionTailAnnexEntity> annexs = new();

            foreach (var item in dto.Attachments)
            {
                var attachment = new InteAttachmentEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    Name = item.Name,
                    Path = item.Path,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                };
                attachments.Add(attachment);
                var annex = new QualIpqcInspectionTailAnnexEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    IpqcInspectionTailId = dto.Id,
                    AnnexId = attachment.Id,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                };
                annexs.Add(annex);
            }

            int rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _inteAttachmentRepository.InsertRangeAsync(attachments);
                rows += await _qualIpqcInspectionTailAnnexRepository.InsertRangeAsync(annexs);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 附件删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> AttachmentDeleteAsync(long[] ids)
        {
            return await _qualIpqcInspectionTailAnnexRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }

        /// <summary>
        /// 查询检验单样品应检参数并校验
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SampleShouldInspectItemsDto>?> GetSampleShouldInspectItemsAsync(SampleShouldInspectItemsQueryDto query)
        {
            var entity = await _qualIpqcInspectionTailRepository.GetByIdAsync(query.Id);
            if (entity == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10104));
            }

            var manuSfcEntity = await _manuSfcRepository.GetBySFCAsync(new EntityBySFCQuery { SFC = query.SampleCode, SiteId = _currentSite.SiteId ?? 0 });
            if (manuSfcEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13235)).WithData("SampleCode", query.SampleCode);
            }

            if (manuSfcEntity.Status == SfcStatusEnum.Scrapping)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13236)).WithData("SampleCode", query.SampleCode);
            }
            var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdWithIsUseAsync(manuSfcEntity.Id);
            if (manuSfcInfoEntity != null && entity.WorkOrderId != manuSfcInfoEntity.WorkOrderId)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13237));
            }


            //校验样品条码是否已检验
            var samples = await _qualIpqcInspectionTailSampleRepository.GetEntitiesAsync(new QualIpqcInspectionTailSampleQuery { InspectionOrderId = query.Id });
            if (samples != null && samples.Any(x => x.Barcode.ToUpper() == query.SampleCode.ToUpper()))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13234)).WithData("SampleCode", query.SampleCode);
            }

            //IPQC检验项目参数
            var ipqcParameters = await _qualIpqcInspectionParameterRepository.GetEntitiesAsync(new QualIpqcInspectionParameterQuery { IpqcInspectionId = entity.IpqcInspectionId });

            // 查询已经缓存的参数实体
            var parameterEntities = await _procParameterRepository.GetProcParameterEntitiesAsync(new ProcParameterQuery
            {
                SiteId = _currentSite.SiteId ?? 0
            });

            //设备采集参数值获取
            IEnumerable<ManuProductParameterEntity>? collectParameterValues = null;
            if (ipqcParameters.Any(x => x.IsDeviceCollect == YesOrNoEnum.Yes))
            {
                collectParameterValues = await _manuProductParameterService.GetProductParameterListByProcedureAsync(new CoreServices.Dtos.Parameter.QueryParameterByProcedureDto
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    ProcedureId = entity.ProcedureId,
                    SFCs = new[] { query.SampleCode }
                });
            }

            List<SampleShouldInspectItemsDto> dtos = new();
            foreach (var item in ipqcParameters)
            {
                var dto = item.ToModel<SampleShouldInspectItemsDto>();

                var parameterEntity = parameterEntities.FirstOrDefault(f => f.Id == item.ParameterId);
                if (parameterEntity != null)
                {
                    dto.ParameterCode = parameterEntity.ParameterCode;
                    dto.ParameterName = parameterEntity.ParameterName;
                    dto.ParameterUnit = parameterEntity.ParameterUnit ?? "";
                    dto.DataType = parameterEntity.DataType;
                }

                //设备采集参数值
                if (item.IsDeviceCollect == YesOrNoEnum.Yes)
                {
                    dto.InspectionValue = collectParameterValues?.FirstOrDefault(x => x.ParameterId == item.ParameterId)?.ParameterValue ?? "";
                }

                dtos.Add(dto);
            }

            return dtos;
        }

    }
}
