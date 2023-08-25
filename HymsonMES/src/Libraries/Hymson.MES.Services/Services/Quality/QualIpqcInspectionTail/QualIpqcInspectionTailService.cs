using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Sequences;
using Hymson.Snowflake;
using Hymson.Utils;
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
        private readonly IQualIpqcInspectionRepository _qualIpqcInspectionRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;
        private readonly ISequenceService _sequenceService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QualIpqcInspectionTailService(ICurrentUser currentUser, ICurrentSite currentSite, 
            AbstractValidator<QualIpqcInspectionTailSaveDto> validationSaveRules,
            AbstractValidator<List<QualIpqcInspectionTailSampleCreateDto>> validationSampleAddRules,
            IQualIpqcInspectionTailRepository qualIpqcInspectionTailRepository,
            IQualIpqcInspectionTailSampleRepository qualIpqcInspectionTailSampleRepository,
            IQualIpqcInspectionRepository qualIpqcInspectionRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcResourceRepository procResourceRepository,
            IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository,
            ISequenceService sequenceService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _validationSampleAddRules = validationSampleAddRules;
            _qualIpqcInspectionTailRepository = qualIpqcInspectionTailRepository;
            _qualIpqcInspectionTailSampleRepository = qualIpqcInspectionTailSampleRepository;
            _qualIpqcInspectionRepository = qualIpqcInspectionRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procResourceRepository = procResourceRepository;
            _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
            _sequenceService = sequenceService;
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
            var procedureResourceList = await _procResourceRepository.GetProcResourceListByProcedureIdAsync(saveDto.ResourceId);
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
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _qualIpqcInspectionTailRepository.DeleteAsync(id);
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
           var qualIpqcInspectionTailEntity = await _qualIpqcInspectionTailRepository.GetByIdAsync(id);
           if (qualIpqcInspectionTailEntity == null) return null;
           
           return qualIpqcInspectionTailEntity.ToModel<QualIpqcInspectionTailDto>();
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
            var samples = await _qualIpqcInspectionTailSampleRepository.GetEntitiesAsync(new QualIpqcInspectionTailSampleQuery { IpqcInspectionTailId = dto.Id });
            if (samples.IsNullOrEmpty() || samples.Select(x => x.Barcode).Distinct().Count() < entity.SampleQty)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13231));
            }

            entity.IsQualified = samples.Any(x => x.IsQualified != TrueOrFalseEnum.Yes) ? TrueOrFalseEnum.No : TrueOrFalseEnum.Yes;
            entity.Status = entity.IsQualified == TrueOrFalseEnum.Yes ? InspectionStatusEnum.Closed : InspectionStatusEnum.Completed;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

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
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            // 保存
            return await _qualIpqcInspectionTailRepository.UpdateAsync(entity);
        }

    }
}
