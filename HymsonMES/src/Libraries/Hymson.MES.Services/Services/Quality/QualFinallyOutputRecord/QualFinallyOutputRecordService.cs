using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（成品条码产出记录(FQC生成使用)） 
    /// </summary>
    public class QualFinallyOutputRecordService : IQualFinallyOutputRecordService
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
        private readonly AbstractValidator<QualFinallyOutputRecordSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（成品条码产出记录(FQC生成使用)）
        /// </summary>
        private readonly IQualFinallyOutputRecordRepository _qualFinallyOutputRecordRepository;

        /// <summary>
        /// 物料维护仓储接口
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 工单信息表仓储接口
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        ///  工作中心表仓储
        /// </summary>
        private readonly IInteWorkCenterRepository _workCenterRepository;

        /// <summary>
        /// 仓储接口（FQC检验参数组）
        /// </summary>
        private readonly IQualFqcParameterGroupRepository _qualFqcParameterGroupRepository;

        /// <summary>
        /// 仓储接口（Fqc样本）
        /// </summary>
        private readonly IQualFqcOrderSampleRepository _qualFqcOrderSampleRepository;

        /// <summary>
        /// 条码表仓储接口
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;




        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="qualFinallyOutputRecordRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="inteWorkCenterRepository"></param>
        public QualFinallyOutputRecordService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<QualFinallyOutputRecordSaveDto> validationSaveRules,
            IQualFinallyOutputRecordRepository qualFinallyOutputRecordRepository,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IInteWorkCenterRepository inteWorkCenterRepository,
            IQualFqcParameterGroupRepository qualFqcParameterGroupRepository,
            IQualFqcOrderSampleRepository qualFqcOrderSampleRepository,
            IManuSfcRepository manuSfcRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _qualFinallyOutputRecordRepository = qualFinallyOutputRecordRepository;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _workCenterRepository = inteWorkCenterRepository;
            _qualFqcParameterGroupRepository = qualFqcParameterGroupRepository;
            _qualFqcOrderSampleRepository = qualFqcOrderSampleRepository;
            _manuSfcRepository = manuSfcRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(QualFinallyOutputRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<QualFinallyOutputRecordEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _qualFinallyOutputRecordRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(QualFinallyOutputRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<QualFinallyOutputRecordEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _qualFinallyOutputRecordRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _qualFinallyOutputRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _qualFinallyOutputRecordRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualFinallyOutputRecordDto?> QueryByIdAsync(long id)
        {
            var qualFinallyOutputRecordEntity = await _qualFinallyOutputRecordRepository.GetByIdAsync(id);
            if (qualFinallyOutputRecordEntity == null) return null;

            return qualFinallyOutputRecordEntity.ToModel<QualFinallyOutputRecordDto>();
        }

        /// <summary>
        /// 获取条码产了最终记录
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFinallyOutputRecordView>> QueryBySFCAsync(FQCInspectionSFCQueryDto queryDto)
        {
            var _siteId = _currentSite.SiteId ?? 0;

            long materialId = default, workOrderId = default, workCenterId = default;
            if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
            {
                var materialEntities = await _procMaterialRepository.GetByCodeAsync(new EntityByCodeQuery { Code = queryDto.MaterialCode });
                if (materialEntities != null)
                {
                    materialId = materialEntities.Id;
                }
                else { materialId = -1; }
            }

            if (!string.IsNullOrWhiteSpace(queryDto.WorkOrderCode))
            {
                var workOrderEntities = await _planWorkOrderRepository.GetByCodeAsync(new PlanWorkOrderQuery { SiteId = _siteId, OrderCode = queryDto.WorkOrderCode });
                if (workOrderEntities != null)
                {
                    workOrderId = workOrderEntities.Id;
                }
                else { workOrderId = -1; }
            }

            if (!string.IsNullOrWhiteSpace(queryDto.WorkCenterCode))
            {
                var entitity = await _workCenterRepository.GetByCodeAsync(new EntityByCodeQuery { Code = queryDto.WorkCenterCode });
                if (entitity != null)
                {
                    workCenterId = entitity.Id;
                }
                else { workCenterId = -1; }
            }

            var queryRsp = new QualFinallyOutputRecordPagedQuery
            {
                Barcode = queryDto.Barcode?.Trim(),
                MaterialId = materialId,
                WorkOrderId = workOrderId,
                WorkCenterId = workCenterId,
                PageIndex = queryDto.PageIndex,
                PageSize = queryDto.PageSize,
                IsGenerated = 0,
                SiteId = _siteId
            };

            var result = new PagedInfo<QualFinallyOutputRecordView>(Enumerable.Empty<QualFinallyOutputRecordView>(), queryRsp.PageIndex, queryRsp.PageSize);


            var pageResult = await _qualFinallyOutputRecordRepository.GetPagedListAsync(queryRsp);

            if (pageResult.Data != null && pageResult.Data.Any())
            {
                result.Data = pageResult.Data.Select(m => m.ToModel<QualFinallyOutputRecordView>());
                result.TotalCount = pageResult.TotalCount;

                var resultMaterialIds = result.Data.Select(m => m.MaterialId);
                var resultWorkOrderIds = result.Data.Select(m => m.WorkOrderId.GetValueOrDefault());
                var resultWorkCenterIdIds = result.Data.Select(m => m.WorkCenterId.GetValueOrDefault());
                var resultbarcodes = result.Data.Select(m => m.Barcode).Distinct();

                //获取检验项目
                var parameterGroupEntitys = await _qualFqcParameterGroupRepository.GetEntitiesAsync(new QualFqcParameterGroupQuery
                {
                    SiteId = _siteId,
                    MaterialIds = resultMaterialIds,
                    Status = Core.Enums.SysDataStatusEnum.Enable
                });

                //FQC检验单状态
                var fqcOrders = await _qualFqcOrderSampleRepository.GetEntitiesByDetailBacodeAsync(resultbarcodes);

                //条码信息
                var sfcs = await _manuSfcRepository.GetListAsync(new ManuSfcQuery { SFCs = resultbarcodes });

                try
                {
                    //物料
                    var materialEntities = await _procMaterialRepository.GetByIdsAsync(resultMaterialIds);
                    //工单
                    var planWorkEntities = await _planWorkOrderRepository.GetByIdsAsync(resultWorkOrderIds);
                    //工作中心
                    var workCenterEntities = await _workCenterRepository.GetByIdsAsync(resultWorkCenterIdIds);

                    result.Data = result.Data.Select(m =>
                    {
                        var materialEntity = materialEntities.FirstOrDefault(e => e.Id == m.MaterialId);
                        if (materialEntity != default)
                        {
                            m.MaterialCode = materialEntity.MaterialCode;
                            m.MaterialName = materialEntity.MaterialName;
                            m.MaterialUnit = materialEntity.Unit ?? string.Empty;
                            m.MaterialVersion = materialEntity.Version ?? string.Empty;
                        }

                        var workOrderEntity = planWorkEntities.FirstOrDefault(e => e.Id == m.WorkOrderId);
                        if (workOrderEntity != default)
                        {
                            m.WorkOrderCode = workOrderEntity.OrderCode;
                        }

                        var workCenterEntity = workCenterEntities.FirstOrDefault(e => e.Id == m.WorkCenterId);
                        if (workCenterEntity != default)
                        {
                            m.WorkCenterCode = workCenterEntity.Code;
                        }

                        var parameterGroupEntity = parameterGroupEntitys.FirstOrDefault(e => e.MaterialId == m.MaterialId);
                        if (parameterGroupEntity != default)
                        {
                            m.IsSameWorkCenter = parameterGroupEntity.IsSameWorkCenter;
                            m.IsSameWorkOrder = parameterGroupEntity.IsSameWorkOrder;
                        }

                        var fqcSatate = fqcOrders.FirstOrDefault(e => e.Barcode == m.Barcode);
                        if (fqcSatate != null)
                        {
                            m.FQCStatus = fqcSatate.Status;
                            m.FQCInspectionOrder = fqcSatate.InspectionOrder;
                        }

                        var sfc = sfcs.FirstOrDefault(e => e.SFC == m.Barcode);
                        if (sfc != null)
                        {
                            m.Qty = sfc.Qty;
                            m.BarcodeStatus = sfc.Status;
                        }


                        return m;
                    });

                }
                catch (Exception ex) { }


            }

            return result;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFinallyOutputRecordDto>> GetPagedListAsync(QualFinallyOutputRecordPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualFinallyOutputRecordPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualFinallyOutputRecordRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<QualFinallyOutputRecordDto>());
            return new PagedInfo<QualFinallyOutputRecordDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
