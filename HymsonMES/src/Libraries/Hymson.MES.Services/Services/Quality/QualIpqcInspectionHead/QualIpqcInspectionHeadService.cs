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
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Plan.PlanWorkOrder.Query;
using Hymson.MES.Data.Repositories.Process;
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
    /// 服务（首检检验单） 
    /// </summary>
    public class QualIpqcInspectionHeadService : IQualIpqcInspectionHeadService
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
        private readonly AbstractValidator<QualIpqcInspectionHeadSaveDto> _validationSaveRules;
        private readonly AbstractValidator<List<QualIpqcInspectionHeadSampleCreateDto>> _validationSampleAddRules;

        /// <summary>
        /// 仓储接口（首检检验单）
        /// </summary>
        private readonly IQualIpqcInspectionHeadRepository _qualIpqcInspectionHeadRepository;
        private readonly IQualIpqcInspectionHeadResultRepository _qualIpqcInspectionHeadResultRepository;
        private readonly IQualIpqcInspectionHeadSampleRepository _qualIpqcInspectionHeadSampleRepository;
        private readonly IQualIpqcInspectionRepository _qualIpqcInspectionRepository;
        private readonly IQualIpqcInspectionRuleRepository _qualIpqcInspectionRuleRepository;
        private readonly IQualIpqcInspectionRuleResourceRelationRepository _qualIpqcInspectionRuleResourceRelationRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteDetailNodeRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcResourceEquipmentBindRepository _procResourceEquipmentBindRepository;
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;

        private readonly ISequenceService _sequenceService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QualIpqcInspectionHeadService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<QualIpqcInspectionHeadSaveDto> validationSaveRules,
            AbstractValidator<List<QualIpqcInspectionHeadSampleCreateDto>> validationSampleAddRules,
            IQualIpqcInspectionHeadRepository qualIpqcInspectionHeadRepository,
            IQualIpqcInspectionHeadResultRepository qualIpqcInspectionHeadResultRepository,
            IQualIpqcInspectionHeadSampleRepository qualIpqcInspectionHeadSampleRepository,
            IQualIpqcInspectionRepository qualIpqcInspectionRepository,
            IQualIpqcInspectionRuleRepository qualIpqcInspectionRuleRepository,
            IQualIpqcInspectionRuleResourceRelationRepository qualIpqcInspectionRuleResourceRelationRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IProcProcessRouteRepository procProcessRouteRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteDetailNodeRepository,
            IProcProcedureRepository procProcedureRepository,
            IProcResourceRepository procResourceRepository,
            IProcResourceEquipmentBindRepository procResourceEquipmentBindRepository,
            IEquEquipmentRepository equEquipmentRepository,
            IProcMaterialRepository procMaterialRepository,
            ISequenceService sequenceService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _validationSampleAddRules = validationSampleAddRules;
            _qualIpqcInspectionHeadRepository = qualIpqcInspectionHeadRepository;
            _qualIpqcInspectionHeadResultRepository = qualIpqcInspectionHeadResultRepository;
            _qualIpqcInspectionHeadSampleRepository = qualIpqcInspectionHeadSampleRepository;
            _qualIpqcInspectionRepository = qualIpqcInspectionRepository;
            _qualIpqcInspectionRuleRepository = qualIpqcInspectionRuleRepository;
            _qualIpqcInspectionRuleResourceRelationRepository = qualIpqcInspectionRuleResourceRelationRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procProcessRouteDetailNodeRepository = procProcessRouteDetailNodeRepository;
            _procProcedureRepository = procProcedureRepository;
            _procResourceRepository = procResourceRepository;
            _procResourceEquipmentBindRepository = procResourceEquipmentBindRepository;
            _equEquipmentRepository = equEquipmentRepository;
            _procMaterialRepository = procMaterialRepository;

            _sequenceService = sequenceService;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(QualIpqcInspectionHeadSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            //开班检
            if (saveDto.TriggerCondition == TriggerConditionEnum.Shift)
            {
                return await CreateIpqcInspectionHeadByShiftAsync();
            }

            var workOrderId = saveDto.WorkOrderId.GetValueOrDefault();
            var procedureId = saveDto.ProcedureId.GetValueOrDefault();
            var resourceId = saveDto.ResourceId.GetValueOrDefault();

            //工单
            var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(workOrderId);
            if (workOrderEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13221));
            }
            ////工序
            //var procedureEntity = await _procProcedureRepository.GetByIdAsync(procedureId);
            //if (procedureEntity == null)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            //}
            //校验工序、资源是否匹配
            var procedureResourceList = await _procResourceRepository.GetProcResourceListByProcedureIdAsync(procedureId);
            if (procedureResourceList.IsNullOrEmpty() || !procedureResourceList.Any(x => x.Id == resourceId))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13222));
            }
            //获取首件检验项目
            var ipqcInspectionList = await _qualIpqcInspectionRepository.GetEntitiesAsync(new QualIpqcInspectionQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Type = IPQCTypeEnum.FAI,
                MaterialId = workOrderEntity.ProductId,
                ProcedureId = procedureId,
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
            ////获取检验项目关联资源信息
            //var ruleResourceEntities = await _qualIpqcInspectionRuleResourceRelationRepository.GetEntitiesAsync(new QualIpqcInspectionRuleResourceRelationQuery
            //{
            //    SiteId = _currentSite.SiteId ?? 0,
            //    IpqcInspectionId = ipqcInspection.Id
            //});
            //if (ruleResourceEntities.IsNullOrEmpty() || !ruleResourceEntities.Any(x => x.ResourceId == resourceId))
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES13225));
            //}
            ////检验规则
            //var ipqcInspectRule = await _qualIpqcInspectionRuleRepository.GetByIdAsync(ruleResourceEntities.First().IpqcInspectionRuleId);
            //主设备Id
            var equipmentId = (await _procResourceEquipmentBindRepository.GetByResourceIdAsync(new ProcResourceEquipmentBindQuery
            {
                ResourceId = resourceId,
                IsMain = true
            }))
            .FirstOrDefault()?.EquipmentId ?? 0;
            //流水号
            var sequence = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.ByDay, "FAI");

            var entity = new QualIpqcInspectionHeadEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                InspectionOrder = $"FAI{updatedOn.ToString("yyyy-MM-dd")}{sequence.ToString().PadLeft(4, '0')}",
                IpqcInspectionId = ipqcInspection.Id,
                WorkOrderId = workOrderEntity.Id,
                MaterialId = workOrderEntity.ProductId,
                ProcedureId = procedureId,
                ResourceId = resourceId,
                EquipmentId = equipmentId,
                TriggerCondition = saveDto.TriggerCondition,
                IsStop = TrueOrFalseEnum.No, //ipqcInspectRule?.Way == IPQCRuleWayEnum.Stop ? TrueOrFalseEnum.Yes : TrueOrFalseEnum.No,
                ControlTime = ipqcInspection.ControlTime,
                ControlTimeUnit = ipqcInspection.ControlTimeUnit,
                SampleQty = ipqcInspection.SampleQty,
                IsQualified = null,
                Status = InspectionStatusEnum.WaitInspect,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };
            var resultEntity = new QualIpqcInspectionHeadResultEntity
            {
                Id = entity.Id,
                SiteId = entity.SiteId,
                IpqcInspectionHeadId = entity.Id,
                IsQualified = null,
                Status = InspectionStatusEnum.WaitInspect,
                InspectionBy = entity.CreatedBy,
                InspectionOn = entity.CreatedOn,
                IsCurrent = TrueOrFalseEnum.Yes,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn
            };

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualIpqcInspectionHeadRepository.InsertAsync(entity);
                rows += await _qualIpqcInspectionHeadResultRepository.InsertAsync(resultEntity);
            }
            return rows;
        }

        /// <summary>
        /// 开班首检单生成
        /// </summary>
        /// <returns></returns>
        public async Task<int> CreateIpqcInspectionHeadByShiftAsync()
        {
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            var entities = new List<QualIpqcInspectionHeadEntity>();
            var resultEntites = new List<QualIpqcInspectionHeadResultEntity>();

            var ipqcInspectionList = await _qualIpqcInspectionRepository.GetEntitiesAsync(new QualIpqcInspectionQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Type = IPQCTypeEnum.FAI,
                Status = SysDataStatusEnum.Enable
            });
            if (ipqcInspectionList.IsNullOrEmpty())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13226));
            }
            var workOrderList = await _planWorkOrderRepository.GetEqualPlanWorkOrderEntitiesAsync(new PlanWorkOrderQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                StatusList = new List<PlanWorkOrderStatusEnum> { PlanWorkOrderStatusEnum.SendDown, PlanWorkOrderStatusEnum.InProduction }
            });
            if (workOrderList.IsNullOrEmpty())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13227));
            }
            foreach (var workOrder in workOrderList)
            {
                //获取工单工序列表
                var procedureList = await _procProcessRouteDetailNodeRepository.GetProcessRouteDetailNodesByProcessRouteIdAsync(workOrder.ProcessRouteId);
                if (procedureList.IsNullOrEmpty())
                {
                    continue;
                }
                foreach (var procedure in procedureList)
                {
                    //获取首件检验项目
                    var ipqcInspection = ipqcInspectionList.Where(x => x.MaterialId == workOrder.ProductId && x.ProcedureId == procedure.Id).FirstOrDefault();
                    if (ipqcInspection == null)
                    {
                        continue;
                    }
                    //获取应检资源列表
                    var procedureResourceList = await _procResourceRepository.GetProcResourceListByProcedureIdAsync(procedure.Id);
                    if (procedureResourceList.IsNullOrEmpty())
                    {
                        continue;
                    }
                    foreach (var resource in procedureResourceList)
                    {
                        //主设备Id
                        var equipmentId = (await _procResourceEquipmentBindRepository.GetByResourceIdAsync(new ProcResourceEquipmentBindQuery
                        {
                            ResourceId = resource.Id,
                            IsMain = true
                        }))
                        .FirstOrDefault()?.EquipmentId ?? 0;
                        //流水号
                        var sequence = await _sequenceService.GetSerialNumberAsync(Sequences.Enums.SerialNumberTypeEnum.ByDay, "FAI");

                        var entity = new QualIpqcInspectionHeadEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = _currentSite.SiteId ?? 0,
                            InspectionOrder = $"FAI{updatedOn.ToString("yyyy-MM-dd")}{sequence.ToString().PadLeft(4, '0')}",
                            IpqcInspectionId = ipqcInspection.Id,
                            WorkOrderId = workOrder.Id,
                            MaterialId = workOrder.ProductId,
                            ProcedureId = procedure.Id,
                            ResourceId = resource.Id,
                            EquipmentId = equipmentId,
                            TriggerCondition = TriggerConditionEnum.Shift,
                            IsStop = TrueOrFalseEnum.No,
                            ControlTime = ipqcInspection.ControlTime,
                            ControlTimeUnit = ipqcInspection.ControlTimeUnit,
                            SampleQty = ipqcInspection.SampleQty,
                            IsQualified = null,
                            Status = InspectionStatusEnum.WaitInspect,
                            CreatedBy = updatedBy,
                            UpdatedBy = updatedBy
                        };
                        var resultEntity = new QualIpqcInspectionHeadResultEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = entity.SiteId,
                            IpqcInspectionHeadId = entity.Id,
                            IsQualified = null,
                            Status = InspectionStatusEnum.WaitInspect,
                            InspectionBy = entity.CreatedBy,
                            InspectionOn = entity.CreatedOn,
                            IsCurrent = TrueOrFalseEnum.Yes,
                            CreatedBy = entity.CreatedBy,
                            UpdatedBy = entity.UpdatedBy
                        };

                        entities.Add(entity);
                        resultEntites.Add(resultEntity);
                    }
                }

            }

            var rows = 0;
            if (entities.Any() && resultEntites.Any())
            {
                using (var trans = TransactionHelper.GetTransactionScope())
                {
                    rows += await _qualIpqcInspectionHeadRepository.InsertRangeAsync(entities);
                    rows += await _qualIpqcInspectionHeadResultRepository.InsertRangeAsync(resultEntites);
                }
            }
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13228));
            }

            return rows;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(QualIpqcInspectionHeadSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<QualIpqcInspectionHeadEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _qualIpqcInspectionHeadRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _qualIpqcInspectionHeadRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            var entities = await _qualIpqcInspectionHeadRepository.GetByIdsAsync(ids);
            if (entities != null && entities.Any(x => x.Status != InspectionStatusEnum.WaitInspect))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13233));
            }
            return await _qualIpqcInspectionHeadRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualIpqcInspectionHeadDto?> QueryByIdAsync(long id)
        {
            var qualIpqcInspectionHeadEntity = await _qualIpqcInspectionHeadRepository.GetByIdAsync(id);
            if (qualIpqcInspectionHeadEntity == null) return null;

            return qualIpqcInspectionHeadEntity.ToModel<QualIpqcInspectionHeadDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIpqcInspectionHeadDto>> GetPagedListAsync(QualIpqcInspectionHeadPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualIpqcInspectionHeadPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualIpqcInspectionHeadRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<QualIpqcInspectionHeadDto>());
            return new PagedInfo<QualIpqcInspectionHeadDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 执行检验
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(StatusChangeDto dto)
        {
            var entity = await _qualIpqcInspectionHeadRepository.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }
            if (entity.Status != InspectionStatusEnum.WaitInspect)
            {
                return 1;
            }

            var resultEntity = await _qualIpqcInspectionHeadResultRepository.GetCurrentEntityByMainIdAsync(dto.Id);
            if (resultEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }
            if (resultEntity.Status != InspectionStatusEnum.WaitInspect)
            {
                return 1;
            }

            entity.Status = InspectionStatusEnum.Inspecting;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            resultEntity.Status = entity.Status;
            resultEntity.UpdatedBy = entity.UpdatedBy;
            resultEntity.UpdatedOn = entity.UpdatedOn;

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualIpqcInspectionHeadRepository.UpdateAsync(entity);
                rows += await _qualIpqcInspectionHeadResultRepository.UpdateAsync(resultEntity);
            }

            return rows;
        }

        /// <summary>
        /// 样品检验数据录入
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public async Task<int> InsertSampleDataAsync(List<QualIpqcInspectionHeadSampleCreateDto> dataList)
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
                var entity = item.ToEntity<QualIpqcInspectionHeadSampleEntity>();
                entity.Id = IdGenProvider.Instance.CreateId();
                entity.SiteId = _currentSite.SiteId ?? 0;
                entity.CreatedBy = updatedBy;
                entity.CreatedOn = updatedOn;
                entity.UpdatedBy = updatedBy;
                entity.UpdatedOn = updatedOn;
                return entity;
            });

            return await _qualIpqcInspectionHeadSampleRepository.InsertRangeAsync(entitys);
        }

        /// <summary>
        /// 样品检验数据修改
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateSampleDataAsync(QualIpqcInspectionHeadSampleUpdateDto param)
        {
            var entity = await _qualIpqcInspectionHeadSampleRepository.GetByIdAsync(param.Id);
            if (entity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }
            //校验检验单状态
            var mainEntity = await _qualIpqcInspectionHeadRepository.GetByIdAsync(entity.IpqcInspectionHeadId);
            if (mainEntity != null && (mainEntity.Status == InspectionStatusEnum.Completed || mainEntity.Status == InspectionStatusEnum.Closed))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13229));
            }

            entity.InspectionValue = param.InspectionValue;
            entity.IsQualified = param.IsQualified;
            entity.Remark = param.Remark;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _qualIpqcInspectionHeadSampleRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 检验完成
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> CompleteAsync(StatusChangeDto dto)
        {
            var entityTask = _qualIpqcInspectionHeadRepository.GetByIdAsync(dto.Id);
            var resultEntityTask = _qualIpqcInspectionHeadResultRepository.GetCurrentEntityByMainIdAsync(dto.Id);
            var entity = await entityTask;
            var resultEntity = await resultEntityTask;
            if (entity == null || resultEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }
            if (entity.Status != InspectionStatusEnum.Inspecting || resultEntity.Status != InspectionStatusEnum.Inspecting)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13230));
            }
            //获取已检样本数据
            var samples = await _qualIpqcInspectionHeadSampleRepository.GetEntitiesAsync(new QualIpqcInspectionHeadSampleQuery { IpqcInspectionHeadId = dto.Id });
            if (samples.IsNullOrEmpty() || samples.Select(x => x.Barcode).Distinct().Count() < entity.SampleQty)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13231));
            }

            entity.IsQualified = samples.Any(x => x.IsQualified != TrueOrFalseEnum.Yes) ? TrueOrFalseEnum.No : TrueOrFalseEnum.Yes;
            entity.Status = entity.IsQualified == TrueOrFalseEnum.Yes ? InspectionStatusEnum.Closed : InspectionStatusEnum.Completed;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            resultEntity.IsQualified = entity.IsQualified;
            resultEntity.Status = entity.Status;
            resultEntity.UpdatedBy = entity.UpdatedBy;
            resultEntity.UpdatedOn = entity.UpdatedOn;

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualIpqcInspectionHeadRepository.UpdateAsync(entity);
                rows += await _qualIpqcInspectionHeadResultRepository.UpdateAsync(resultEntity);
            }

            return rows;
        }

        /// <summary>
        /// 不合格处理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> UnqualifiedHandleAsync(UnqualifiedHandleDto dto)
        {
            var entityTask = _qualIpqcInspectionHeadRepository.GetByIdAsync(dto.Id);
            var resultEntityTask = _qualIpqcInspectionHeadResultRepository.GetCurrentEntityByMainIdAsync(dto.Id);
            var entity = await entityTask;
            var resultEntity = await resultEntityTask;
            if (entity == null || resultEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }
            if (entity.Status != InspectionStatusEnum.Completed || resultEntity.Status != InspectionStatusEnum.Completed)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES13232));
            }

            entity.Status = InspectionStatusEnum.Closed;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            resultEntity.Status = entity.Status;
            resultEntity.HandMethod = dto.HandMethod;
            resultEntity.ProcessedBy = _currentUser.UserName;
            resultEntity.ProcessedOn = entity.UpdatedOn;
            resultEntity.UpdatedBy = entity.UpdatedBy;
            resultEntity.UpdatedOn = entity.UpdatedOn;

            // 保存
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _qualIpqcInspectionHeadRepository.UpdateAsync(entity);
                rows += await _qualIpqcInspectionHeadResultRepository.UpdateAsync(resultEntity);
            }

            return rows;
        }
    }
}
