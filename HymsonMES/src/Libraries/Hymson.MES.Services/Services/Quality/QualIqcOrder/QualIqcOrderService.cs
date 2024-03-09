using Elastic.Transport;
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.WHMaterialReceipt;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Minio.DataModel;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（iqc检验单） 
    /// </summary>
    public class QualIqcOrderService : IQualIqcOrderService
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
        private readonly AbstractValidator<QualIqcOrderSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（iqc检验项目快照）
        /// </summary>
        private readonly IQualIqcInspectionItemSnapshotRepository _qualIqcInspectionItemSnapshotRepository;

        /// <summary>
        /// 仓储接口（iqc检验项目快照明细）
        /// </summary>
        private readonly IQualIqcInspectionItemDetailSnapshotRepository _qualIqcInspectionItemDetailSnapshotRepository;

        /// <summary>
        /// 仓储接口（iqc检验单）
        /// </summary>
        private readonly IQualIqcOrderRepository _qualIqcOrderRepository;

        /// <summary>
        /// 仓储接口（iqc检验单操作）
        /// </summary>
        private readonly IQualIqcOrderOperateRepository _qualIqcOrderOperateRepository;

        /// <summary>
        /// 仓储接口（iqc检验类型）
        /// </summary>
        private readonly IQualIqcOrderTypeRepository _qualIqcOrderTypeRepository;

        /// <summary>
        /// 仓储接口（iqc检验样本附件）
        /// </summary>
        private readonly IQualIqcOrderAnnexRepository _qualIqcOrderAnnexRepository;

        /// <summary>
        /// 仓储接口（Iqc样本）
        /// </summary>
        private readonly IQualIqcOrderSampleRepository _qualIqcOrderSampleRepository;

        /// <summary>
        /// 仓储接口（iqc检验样本）
        /// </summary>
        private readonly IQualIqcOrderSampleDetailRepository _qualIqcOrderSampleDetailRepository;

        /// <summary>
        /// 仓储接口（iqc检验样本附件）
        /// </summary>
        private readonly IQualIqcOrderSampleDetailAnnexRepository _qualIqcOrderSampleDetailAnnexRepository;

        /// <summary>
        /// 仓储接口（Iqc不合格处理）
        /// </summary>
        private readonly IQualIqcOrderUnqualifiedHandRepository _qualIqcOrderUnqualifiedHandRepository;

        /// <summary>
        /// 仓储接口（收货单）
        /// </summary>
        private readonly IWhMaterialReceiptRepository _whMaterialReceiptRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（供应商维护）
        /// </summary>
        private readonly IWhSupplierRepository _whSupplierRepository;

        /// <summary>
        /// 仓储接口（附件维护）
        /// </summary>
        private readonly IInteAttachmentRepository _inteAttachmentRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="qualIqcInspectionItemSnapshotRepository"></param>
        /// <param name="qualIqcInspectionItemDetailSnapshotRepository"></param>
        /// <param name="qualIqcOrderRepository"></param>
        /// <param name="qualIqcOrderOperateRepository"></param>
        /// <param name="qualIqcOrderTypeRepository"></param>
        /// <param name="qualIqcOrderAnnexRepository"></param>
        /// <param name="qualIqcOrderSampleRepository"></param>
        /// <param name="qualIqcOrderSampleDetailRepository"></param>
        /// <param name="qualIqcOrderSampleDetailAnnexRepository"></param>
        /// <param name="qualIqcOrderUnqualifiedHandRepository"></param>
        /// <param name="whMaterialReceiptRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whSupplierRepository"></param>
        /// <param name="inteAttachmentRepository"></param>
        public QualIqcOrderService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<QualIqcOrderSaveDto> validationSaveRules,
            IQualIqcInspectionItemSnapshotRepository qualIqcInspectionItemSnapshotRepository,
            IQualIqcInspectionItemDetailSnapshotRepository qualIqcInspectionItemDetailSnapshotRepository,
            IQualIqcOrderRepository qualIqcOrderRepository,
            IQualIqcOrderOperateRepository qualIqcOrderOperateRepository,
            IQualIqcOrderTypeRepository qualIqcOrderTypeRepository,
            IQualIqcOrderAnnexRepository qualIqcOrderAnnexRepository,
            IQualIqcOrderSampleRepository qualIqcOrderSampleRepository,
            IQualIqcOrderSampleDetailRepository qualIqcOrderSampleDetailRepository,
            IQualIqcOrderSampleDetailAnnexRepository qualIqcOrderSampleDetailAnnexRepository,
            IQualIqcOrderUnqualifiedHandRepository qualIqcOrderUnqualifiedHandRepository,
            IWhMaterialReceiptRepository whMaterialReceiptRepository,
            IProcMaterialRepository procMaterialRepository,
            IWhSupplierRepository whSupplierRepository,
            IInteAttachmentRepository inteAttachmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _qualIqcInspectionItemSnapshotRepository = qualIqcInspectionItemSnapshotRepository;
            _qualIqcInspectionItemDetailSnapshotRepository = qualIqcInspectionItemDetailSnapshotRepository;
            _qualIqcOrderRepository = qualIqcOrderRepository;
            _qualIqcOrderOperateRepository = qualIqcOrderOperateRepository;
            _qualIqcOrderTypeRepository = qualIqcOrderTypeRepository;
            _qualIqcOrderAnnexRepository = qualIqcOrderAnnexRepository;
            _qualIqcOrderSampleRepository = qualIqcOrderSampleRepository;
            _qualIqcOrderSampleDetailRepository = qualIqcOrderSampleDetailRepository;
            _qualIqcOrderSampleDetailAnnexRepository = qualIqcOrderSampleDetailAnnexRepository;
            _qualIqcOrderUnqualifiedHandRepository = qualIqcOrderUnqualifiedHandRepository;
            _whMaterialReceiptRepository = whMaterialReceiptRepository;
            _procMaterialRepository = procMaterialRepository;
            _whSupplierRepository = whSupplierRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(QualIqcOrderSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<QualIqcOrderEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _qualIqcOrderRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(QualIqcOrderSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<QualIqcOrderEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _qualIqcOrderRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除检验单附件
        /// </summary>
        /// <param name="orderAnnexId"></param>
        /// <returns></returns>
        public async Task<int> DeleteAttachmentByIdAsync(long orderAnnexId)
        {
            var attachmentEntity = await _qualIqcOrderAnnexRepository.GetByIdAsync(orderAnnexId);
            if (attachmentEntity == null) return default;

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _inteAttachmentRepository.DeleteAsync(attachmentEntity.AnnexId);
            rows += await _qualIqcOrderAnnexRepository.DeleteAsync(attachmentEntity.Id);
            return rows;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            if (!ids.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10213));

            var entities = await _qualIqcOrderRepository.GetByIdsAsync(ids);
            if (entities != null && entities.Any(a => a.Status != InspectionStatusEnum.WaitInspect))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10137));
            }

            return await _qualIqcOrderRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualIqcOrderDto?> QueryByIdAsync(long id)
        {
            var entity = await _qualIqcOrderRepository.GetByIdAsync(id);
            if (entity == null) return null;

            // 实体到DTO转换
            var dto = entity.ToModel<QualIqcOrderDto>();
            dto.StatusText = dto.Status.GetDescription();
            dto.IsQualifiedText = dto.IsQualified.GetDescription();

            // 读取收货单
            var receiptDetailEntity = await _whMaterialReceiptRepository.GetDetailByIdAsync(entity.MaterialReceiptDetailId);
            if (receiptDetailEntity == null) return dto;

            dto.SupplierBatch = receiptDetailEntity.SupplierBatch;
            dto.InternalBatch = receiptDetailEntity.InternalBatch;
            dto.PlanQty = receiptDetailEntity.PlanQty;

            // 读取收货单
            var receiptEntity = await _whMaterialReceiptRepository.GetByIdAsync(receiptDetailEntity.MaterialReceiptId);
            if (receiptEntity == null) return dto;

            dto.ReceiptNum = receiptEntity.ReceiptNum;

            // TODO 规格型号
            dto.Specifications = "-";

            // 读取产品
            if (entity.MaterialId.HasValue)
            {
                var materialEntity = await _procMaterialRepository.GetByIdAsync(entity.MaterialId.Value);
                if (materialEntity != null)
                {
                    dto.MaterialCode = materialEntity.MaterialCode;
                    dto.MaterialName = materialEntity.MaterialName;
                    dto.MaterialVersion = materialEntity.Version ?? "";
                    dto.Unit = materialEntity.Unit ?? "";
                }
            }

            // 读取供应商
            if (entity.SupplierId.HasValue)
            {
                var supplierEntity = await _whSupplierRepository.GetByIdAsync(entity.SupplierId.Value);
                if (supplierEntity != null)
                {
                    dto.SupplierCode = supplierEntity.Code;
                    dto.SupplierName = supplierEntity.Name;
                }
            }

            return dto;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIqcOrderDto>> GetPagedListAsync(QualIqcOrderPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualIqcOrderPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualIqcOrderRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = await PrepareDtos(pagedInfo.Data);
            return new PagedInfo<QualIqcOrderDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询类型
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcOrderTypeBaseDto>> QueryOrderTypeListByIdAsync(long orderId)
        {
            var entities = await _qualIqcOrderTypeRepository.GetByOrderIdAsync(orderId);
            return entities.Select(s => s.ToModel<QualIqcOrderTypeBaseDto>());
        }

        /// <summary>
        /// 根据ID查询附件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryOrderAttachmentListByIdAsync(long orderId)
        {
            var iqcOrderAnnexs = await _qualIqcOrderAnnexRepository.GetByOrderIdAsync(orderId);
            if (iqcOrderAnnexs == null) return Array.Empty<InteAttachmentBaseDto>();

            var attachmentEntities = await _inteAttachmentRepository.GetByIdsAsync(iqcOrderAnnexs.Select(s => s.AnnexId));
            if (attachmentEntities == null) return Array.Empty<InteAttachmentBaseDto>();

            return PrepareAttachmentBaseDtos(iqcOrderAnnexs, attachmentEntities);
        }

        /// <summary>
        /// 查询检验单快照数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OrderParameterDetailDto>> QueryDetailSnapshotByIdAsync(OrderParameterDetailQueryDto requestDto)
        {
            var entity = await _qualIqcOrderRepository.GetByIdAsync(requestDto.OrderId);
            if (entity == null) return Array.Empty<OrderParameterDetailDto>();

            var orderTypeEntity = await _qualIqcOrderTypeRepository.GetByIdAsync(requestDto.IQCOrderTypeId);
            if (orderTypeEntity == null) return Array.Empty<OrderParameterDetailDto>();

            // 检查该类型是否已经录入
            var sampleEntities = await _qualIqcOrderSampleRepository.GetEntitiesAsync(new QualIqcOrderSampleQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                IQCOrderId = entity.Id,
                IQCOrderTypeId = orderTypeEntity.Id
            });
            if (sampleEntities != null && sampleEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19905)).WithData("Type", orderTypeEntity.Type.GetDescription());
            }

            var snapshotEntity = await _qualIqcInspectionItemSnapshotRepository.GetByIdAsync(entity.IqcInspectionItemSnapshotId);
            if (snapshotEntity == null) return Array.Empty<OrderParameterDetailDto>();

            var detailEntities = await _qualIqcInspectionItemDetailSnapshotRepository.GetEntitiesAsync(new QualIqcInspectionItemDetailSnapshotQuery
            {
                IqcInspectionItemSnapshotId = snapshotEntity.Id,
                InspectionType = orderTypeEntity.Type
            });
            if (detailEntities == null) return Array.Empty<OrderParameterDetailDto>();

            return detailEntities.Select(s => s.ToModel<OrderParameterDetailDto>());
        }

        /// <summary>
        /// 查询检验单样本数据
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OrderParameterDetailDto>> QueryDetailSampleByIdAsync(long orderId)
        {
            var entity = await _qualIqcOrderRepository.GetByIdAsync(orderId);
            if (entity == null) return Array.Empty<OrderParameterDetailDto>();

            // 站点数据
            var siteId = _currentSite.SiteId ?? 0;

            // 查询检验单下面的所有样本
            var sampleEntities = await _qualIqcOrderSampleRepository.GetEntitiesAsync(new QualIqcOrderSampleQuery
            {
                SiteId = siteId,
                IQCOrderId = orderId
            });
            if (sampleEntities == null) return Array.Empty<OrderParameterDetailDto>();

            // 查询检验单下面的所有样本明细
            var sampleDetailEntities = await _qualIqcOrderSampleDetailRepository.GetEntitiesAsync(new QualIqcOrderSampleDetailQuery
            {
                SiteId = siteId,
                IQCOrderId = orderId
            });
            if (sampleDetailEntities == null) return Array.Empty<OrderParameterDetailDto>();

            // 查询检验单下面的所有样本附件
            var sampleAttachmentEntities = await _qualIqcOrderSampleDetailAnnexRepository.GetEntitiesAsync(new QualIqcOrderSampleDetailAnnexQuery
            {
                SiteId = siteId,
                IQCOrderId = orderId
            });

            // 附件集合
            Dictionary<long, IGrouping<long, QualIqcOrderSampleDetailAnnexEntity>> sampleAttachmentDic = new();
            IEnumerable<InteAttachmentEntity> attachmentEntities = Array.Empty<InteAttachmentEntity>();
            if (sampleAttachmentEntities.Any())
            {
                sampleAttachmentDic = sampleAttachmentEntities.ToLookup(w => w.IQCOrderSampleDetailId).ToDictionary(d => d.Key, d => d);
                attachmentEntities = await _inteAttachmentRepository.GetByIdsAsync(sampleAttachmentEntities.Select(s => s.AnnexId));
            }

            List<OrderParameterDetailDto> dtos = new();
            foreach (var sampleDetailEntity in sampleDetailEntities)
            {
                var dto = sampleDetailEntity.ToModel<OrderParameterDetailDto>();

                // 填充条码
                var sampleEntity = sampleEntities.FirstOrDefault(f => f.Id == sampleDetailEntity.IQCOrderSampleId);
                if (sampleEntity == null) continue;
                dto.Barcode = sampleEntity.Barcode;

                // 填充附件
                if (attachmentEntities != null && sampleAttachmentDic.TryGetValue(sampleDetailEntity.Id, out var detailAttachmentEntities))
                {
                    dto.Attachments = PrepareAttachmentBaseDtos(detailAttachmentEntities, attachmentEntities);
                }

                dtos.Add(dto);
            }

            return dtos;
        }




        #region 内部方法
        /// <summary>
        /// 转换为Dto对象
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private async Task<IEnumerable<QualIqcOrderDto>> PrepareDtos(IEnumerable<QualIqcOrderEntity> entities)
        {
            List<QualIqcOrderDto> dtos = new();

            // 读取收货单明细
            var receiptDetailEntities = await _whMaterialReceiptRepository.GetDetailsByIdsAsync(entities.Select(x => x.MaterialReceiptDetailId));
            var receiptDetailDic = receiptDetailEntities.ToDictionary(x => x.Id, x => x);

            // 读取收货单
            var receiptEntities = await _whMaterialReceiptRepository.GetByIdsAsync(receiptDetailEntities.Select(x => x.MaterialReceiptId));
            var receiptDic = receiptEntities.ToDictionary(x => x.Id, x => x);

            // 读取产品
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(entities.Where(w => w.MaterialId.HasValue).Select(x => x.MaterialId!.Value));
            var materialDic = materialEntities.ToDictionary(x => x.Id, x => x);

            // 读取供应商
            var supplierEntities = await _whSupplierRepository.GetByIdsAsync(entities.Where(w => w.SupplierId.HasValue).Select(x => x.SupplierId!.Value));
            var supplierDic = supplierEntities.ToDictionary(x => x.Id, x => x);

            foreach (var entity in entities)
            {
                var dto = entity.ToModel<QualIqcOrderDto>();
                if (dto == null) continue;

                // 收货单明细
                var receiptDetailEntity = receiptDetailDic[entity.MaterialReceiptDetailId];
                if (receiptDetailEntity != null)
                {
                    dto.SupplierBatch = receiptDetailEntity.SupplierBatch;
                    dto.InternalBatch = receiptDetailEntity.InternalBatch;
                    dto.PlanQty = receiptDetailEntity.PlanQty;

                    // 收货单
                    var receiptEntity = receiptDic[receiptDetailEntity.MaterialReceiptId];
                    if (receiptEntity != null) dto.ReceiptNum = receiptEntity.ReceiptNum;
                }

                // TODO 规格型号
                dto.Specifications = "-";

                // 产品
                if (entity.MaterialId.HasValue)
                {
                    var materialEntity = materialDic[entity.MaterialId.Value];
                    if (materialEntity != null)
                    {
                        dto.MaterialCode = materialEntity.MaterialCode;
                        dto.MaterialName = materialEntity.MaterialName;
                        dto.MaterialVersion = materialEntity.Version ?? "";
                        dto.Unit = materialEntity.Unit ?? "";
                    }
                }
                else
                {
                    dto.MaterialCode = "-";
                    dto.MaterialName = "-";
                    dto.MaterialVersion = "-";
                    dto.SupplierCode = "-";
                    dto.SupplierName = "-";
                }

                // 供应商
                if (entity.SupplierId.HasValue)
                {
                    var supplierEntity = supplierDic[entity.SupplierId.Value];
                    if (supplierEntity != null)
                    {
                        dto.SupplierCode = supplierEntity.Code;
                        dto.SupplierName = supplierEntity.Name;
                    }
                }
                else
                {
                    dto.SupplierCode = "-";
                    dto.SupplierName = "-";
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 根据关联附件ID获取附件Dto
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
                    AttachmentId = item.AnnexId
                };

                var attachmentEntity = attachmentEntities.FirstOrDefault(f => f.Id == item.AnnexId);
                if (attachmentEntity == null)
                {
                    dto.Name = "附件不存在";
                    dto.Path = "";
                    dtos.Add(dto);
                    continue;
                }

                dto.Name = attachmentEntity.Name;
                dto.Path = attachmentEntity.Path;
                dtos.Add(dto);
            }

            return dtos;
        }
        #endregion


    }
}
