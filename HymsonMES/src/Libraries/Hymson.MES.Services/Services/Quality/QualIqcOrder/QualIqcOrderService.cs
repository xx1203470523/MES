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
using Hymson.MES.Data.Repositories.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.WHMaterialReceipt;
using Hymson.MES.Data.Repositories.WhMaterialReceiptDetail;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

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
        /// 仓储接口（收货单详情）
        /// </summary>
        private readonly IWhMaterialReceiptDetailRepository _whMaterialReceiptDetailRepository;

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
        /// <param name="whMaterialReceiptDetailRepository"></param>
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
            IWhMaterialReceiptDetailRepository whMaterialReceiptDetailRepository,
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
            _whMaterialReceiptDetailRepository = whMaterialReceiptDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _whSupplierRepository = whSupplierRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
        }


        /// <summary>
        /// 更改检验单状态
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> OperationOrderAsync(QualOrderOperationStatusDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // IQC检验单
            var entity = await _qualIqcOrderRepository.GetByIdAsync(requestDto.IQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 检查类型是否已经存在
            var orderOperationEntities = await _qualIqcOrderOperateRepository.GetEntitiesAsync(new QualIqcOrderOperateQuery
            {
                SiteId = entity.SiteId,
                IQCOrderId = entity.Id,
                OperationType = requestDto.OperationType
            });
            if (orderOperationEntities != null && orderOperationEntities.Any()) return 0;

            switch (requestDto.OperationType)
            {
                case OrderOperateTypeEnum.Start:
                    entity.Status = InspectionStatusEnum.Inspecting;
                    break;
                case OrderOperateTypeEnum.Complete:
                    entity.Status = entity.IsQualified == TrueOrFalseEnum.Yes ? InspectionStatusEnum.Closed : InspectionStatusEnum.Completed;
                    break;
                case OrderOperateTypeEnum.Close:
                    entity.Status = InspectionStatusEnum.Closed;
                    break;
                default:
                    break;
            }

            // 保存
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _qualIqcOrderOperateRepository.InsertAsync(new QualIqcOrderOperateEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                IQCOrderId = entity.Id,
                OperationType = requestDto.OperationType,
                OperateBy = updatedBy,
                OperateOn = updatedOn,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            });
            rows += await _qualIqcOrderRepository.UpdateAsync(entity);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 保存样品数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SaveOrderAsync(QualIqcOrderSaveDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // IQC检验单
            var entity = await _qualIqcOrderRepository.GetByIdAsync(requestDto.IQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // IQC检验类型
            var orderTypeEntity = await _qualIqcOrderTypeRepository.GetByIdAsync(requestDto.IQCOrderTypeId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 检查该类型是否已经录入
            var sampleQuery = requestDto.ToQuery<QualIqcOrderSampleQuery>();
            sampleQuery.SiteId = entity.SiteId;
            var sampleEntities = await _qualIqcOrderSampleRepository.GetEntitiesAsync(sampleQuery);
            if (sampleEntities != null && sampleEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19905))
                    .WithData("Code", requestDto.Barcode)
                    .WithData("Type", orderTypeEntity.Type.GetDescription());
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // TODO 更新检验单类型数量
            //orderTypeEntity.CheckedQty

            // 样本
            var sampleId = IdGenProvider.Instance.CreateId();
            var sampleEntity = new QualIqcOrderSampleEntity
            {
                Id = sampleId,
                SiteId = entity.SiteId,
                IQCOrderId = entity.Id,
                IQCOrderTypeId = orderTypeEntity.Id,
                Barcode = requestDto.Barcode,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };

            // 遍历样本参数
            List<QualIqcOrderSampleDetailEntity> sampleDetailEntities = new();
            List<InteAttachmentEntity> attachmentEntities = new();
            List<QualIqcOrderSampleDetailAnnexEntity> sampleDetailAttachmentEntities = new();
            foreach (var item in requestDto.Details)
            {
                var sampleDetailId = IdGenProvider.Instance.CreateId();
                sampleDetailEntities.Add(new QualIqcOrderSampleDetailEntity
                {
                    Id = sampleDetailId,
                    SiteId = entity.SiteId,
                    IQCOrderId = entity.Id,
                    IQCOrderSampleId = sampleId,
                    IQCInspectionDetailSnapshotId = item.Id,
                    InspectionValue = item.InspectionValue ?? "",
                    IsQualified = item.IsQualified,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });

                if (item.Attachments != null && item.Attachments.Any())
                {
                    foreach (var attachment in item.Attachments)
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
                        sampleDetailAttachmentEntities.Add(new QualIqcOrderSampleDetailAnnexEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = entity.SiteId,
                            IQCOrderId = requestDto.IQCOrderId,
                            AnnexId = attachmentId,
                            CreatedBy = updatedBy,
                            CreatedOn = updatedOn,
                            UpdatedBy = updatedBy,
                            UpdatedOn = updatedOn
                        });
                    }
                }
            }

            // 保存
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _qualIqcOrderSampleRepository.InsertAsync(sampleEntity);
            rows += await _qualIqcOrderSampleDetailRepository.InsertRangeAsync(sampleDetailEntities);
            if (attachmentEntities.Any())
            {
                rows += await _inteAttachmentRepository.InsertRangeAsync(attachmentEntities);
                rows += await _qualIqcOrderSampleDetailAnnexRepository.InsertRangeAsync(sampleDetailAttachmentEntities);
            }
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 完成检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> CompleteOrderAsync(QualIqcOrderCompleteDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // IQC检验单
            var entity = await _qualIqcOrderRepository.GetByIdAsync(requestDto.IQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // TODO 只有检验中的状态才允许"完成"

            /*
            // 检查每种类型是否已经录入
            var orderTypeEntities = await _qualIqcOrderTypeRepository.GetByOrderIdAsync(entity.Id);
            if (orderTypeEntities.Any(a => a.SampleQty > a.CheckedQty))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19908))
                    .WithData("Type", "")
                    .WithData("CheckedQty", "")
                    .WithData("SampleQty", "");
            }
            */

            // 检查类型是否已经存在
            var operationType = OrderOperateTypeEnum.Complete;
            var orderOperationEntities = await _qualIqcOrderOperateRepository.GetEntitiesAsync(new QualIqcOrderOperateQuery
            {
                SiteId = entity.SiteId,
                IQCOrderId = entity.Id,
                OperationType = operationType
            });
            if (orderOperationEntities != null && orderOperationEntities.Any()) return 0;

            // TODO 检验是否样本数量已经足够

            // 插入检验单状态操作记录
            return await _qualIqcOrderOperateRepository.InsertAsync(new QualIqcOrderOperateEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                IQCOrderId = entity.Id,
                OperationType = operationType,
                OperateBy = updatedBy,
                OperateOn = updatedOn,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            });
        }

        /// <summary>
        /// 关闭检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> CloseOrderAsync(QualIqcOrderCloseDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // IQC检验单
            var entity = await _qualIqcOrderRepository.GetByIdAsync(requestDto.IQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // TODO 只有检验中的状态才允许"关闭"

            // 检查类型是否已经存在
            var operationType = OrderOperateTypeEnum.Close;
            var orderOperationEntities = await _qualIqcOrderOperateRepository.GetEntitiesAsync(new QualIqcOrderOperateQuery
            {
                SiteId = entity.SiteId,
                IQCOrderId = entity.Id,
                OperationType = operationType
            });
            if (orderOperationEntities != null && orderOperationEntities.Any()) return 0;

            // 插入检验单状态操作记录
            return await _qualIqcOrderOperateRepository.InsertAsync(new QualIqcOrderOperateEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                IQCOrderId = entity.Id,
                OperationType = operationType,
                OperateBy = updatedBy,
                OperateOn = updatedOn,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            });
        }


        /// <summary>
        /// 保存检验单附件
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SaveAttachmentAsync(QualIqcOrderSaveAttachmentDto requestDto)
        {
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // IQC检验单
            var entity = await _qualIqcOrderRepository.GetByIdAsync(requestDto.IQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            if (!requestDto.Attachments.Any()) return 0;

            List<InteAttachmentEntity> attachmentEntities = new();
            List<QualIqcOrderAnnexEntity> orderAnnexEntities = new();
            foreach (var attachment in requestDto.Attachments)
            {
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

                orderAnnexEntities.Add(new QualIqcOrderAnnexEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = entity.SiteId,
                    IQCOrderId = requestDto.IQCOrderId,
                    AnnexId = attachmentId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });
            }

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _inteAttachmentRepository.InsertRangeAsync(attachmentEntities);
            rows += await _qualIqcOrderAnnexRepository.InsertRangeAsync(orderAnnexEntities);
            trans.Complete();
            return rows;
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
        public async Task<int> DeleteOrdersAsync(long[] ids)
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
            var receiptDetailEntity = await _whMaterialReceiptDetailRepository.GetByIdAsync(entity.MaterialReceiptDetailId);
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

            // 转换产品编码/版本变为产品ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.MaterialCode) || !string.IsNullOrWhiteSpace(pagedQueryDto.MaterialName))
            {
                var procMaterialEntities = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery
                {
                    SiteId = pagedQuery.SiteId,
                    MaterialCode = pagedQueryDto.MaterialCode,
                    MaterialName = pagedQueryDto.MaterialName,
                    Version = pagedQueryDto.MaterialVersion
                });
                if (procMaterialEntities != null && procMaterialEntities.Any()) pagedQuery.MaterialIds = procMaterialEntities.Select(s => s.Id);
                else pagedQuery.MaterialIds = Array.Empty<long>();
            }

            // 转换供应商编码变为供应商ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.SupplierCode))
            {
                var whSupplierEntities = await _whSupplierRepository.GetWhSupplierEntitiesAsync(new WhSupplierQuery
                {
                    SiteId = pagedQuery.SiteId,
                    Code = pagedQueryDto.SupplierCode,
                    Name = pagedQueryDto.SupplierName
                });
                if (whSupplierEntities != null && whSupplierEntities.Any()) pagedQuery.SupplierIds = whSupplierEntities.Select(s => s.Id);
                else pagedQuery.SupplierIds = Array.Empty<long>();
            }

            // 将供应商批次/内部批次转换为收货单详情ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.SupplierBatch) || !string.IsNullOrWhiteSpace(pagedQueryDto.InternalBatch))
            {
                var receiptDetailEntities = await _whMaterialReceiptDetailRepository.GetEntitiesAsync(new WhMaterialReceiptDetailQuery
                {
                    SiteId = pagedQuery.SiteId,
                    SupplierBatch = pagedQueryDto.SupplierBatch,
                    InternalBatch = pagedQueryDto.InternalBatch
                });
                if (receiptDetailEntities != null && receiptDetailEntities.Any()) pagedQuery.MaterialReceiptDetailIds = receiptDetailEntities.Select(s => s.Id);
                else pagedQuery.MaterialReceiptDetailIds = Array.Empty<long>();
            }

            // 查询数据
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
        public async Task<IEnumerable<OrderParameterDetailDto>> QueryDetailSnapshotAsync(OrderParameterDetailQueryDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.Barcode)) throw new CustomerValidationException(nameof(ErrorCode.MES19906));
            if (!requestDto.IQCOrderTypeId.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES19907));

            var entity = await _qualIqcOrderRepository.GetByIdAsync(requestDto.IQCOrderId);
            if (entity == null) return Array.Empty<OrderParameterDetailDto>();

            var orderTypeEntity = await _qualIqcOrderTypeRepository.GetByIdAsync(requestDto.IQCOrderTypeId.Value);
            if (orderTypeEntity == null) return Array.Empty<OrderParameterDetailDto>();

            // 检查该类型是否已经录入
            var sampleQuery = requestDto.ToQuery<QualIqcOrderSampleQuery>();
            sampleQuery.SiteId = entity.SiteId;
            var sampleEntities = await _qualIqcOrderSampleRepository.GetEntitiesAsync(sampleQuery);
            if (sampleEntities != null && sampleEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19905))
                    .WithData("Code", requestDto.Barcode)
                    .WithData("Type", orderTypeEntity.Type.GetDescription());
            }

            var snapshotEntity = await _qualIqcInspectionItemSnapshotRepository.GetByIdAsync(entity.IqcInspectionItemSnapshotId);
            if (snapshotEntity == null) return Array.Empty<OrderParameterDetailDto>();

            var snapshotDetailEntities = await _qualIqcInspectionItemDetailSnapshotRepository.GetEntitiesAsync(new QualIqcInspectionItemDetailSnapshotQuery
            {
                SiteId = entity.SiteId,
                IqcInspectionItemSnapshotId = snapshotEntity.Id,
                InspectionType = orderTypeEntity.Type
            });
            if (snapshotDetailEntities == null) return Array.Empty<OrderParameterDetailDto>();

            return snapshotDetailEntities.Select(s => s.ToModel<OrderParameterDetailDto>());
        }

        /// <summary>
        /// 查询检验单样本数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OrderParameterDetailDto>> QueryDetailSampleAsync(OrderParameterDetailQueryDto requestDto)
        {
            var entity = await _qualIqcOrderRepository.GetByIdAsync(requestDto.IQCOrderId);
            if (entity == null) return Array.Empty<OrderParameterDetailDto>();

            // 查询检验单下面的所有样本
            var sampleQuery = requestDto.ToQuery<QualIqcOrderSampleQuery>();
            sampleQuery.SiteId = entity.SiteId;
            var sampleEntities = await _qualIqcOrderSampleRepository.GetEntitiesAsync(sampleQuery);
            if (sampleEntities == null) return Array.Empty<OrderParameterDetailDto>();

            // 查询检验单下面的所有样本明细
            var sampleDetailEntities = await _qualIqcOrderSampleDetailRepository.GetEntitiesAsync(new QualIqcOrderSampleDetailQuery
            {
                SiteId = entity.SiteId,
                IQCOrderId = entity.Id
            });
            if (sampleDetailEntities == null) return Array.Empty<OrderParameterDetailDto>();

            // 查询样品明细对应的快照明细
            var snapshotDetailEntities = await _qualIqcInspectionItemDetailSnapshotRepository.GetByIdsAsync(sampleDetailEntities.Select(s => s.IQCInspectionDetailSnapshotId));

            // 查询检验单下面的所有样本附件
            var sampleAttachmentEntities = await _qualIqcOrderSampleDetailAnnexRepository.GetEntitiesAsync(new QualIqcOrderSampleDetailAnnexQuery
            {
                SiteId = entity.SiteId,
                IQCOrderId = entity.Id
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
                // 快照数据
                var snapshotDetailEntity = snapshotDetailEntities.FirstOrDefault(f => f.Id == sampleDetailEntity.IQCInspectionDetailSnapshotId);
                if (snapshotDetailEntity == null) continue;

                var dto = snapshotDetailEntity.ToModel<OrderParameterDetailDto>();
                dto.Id = sampleDetailEntity.Id;

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
            var receiptDetailEntities = await _whMaterialReceiptDetailRepository.GetByIdsAsync(entities.Select(x => x.MaterialReceiptDetailId));
            var receiptDetailDic = receiptDetailEntities.ToDictionary(x => x.Id, x => x);

            // 读取收货单
            var receiptEntities = await _whMaterialReceiptRepository.GetByIdsAsync(receiptDetailEntities.Select(x => x.MaterialReceiptId));
            var receiptDic = receiptEntities.ToDictionary(x => x.Id, x => x);

            // 检验单操作
            var orderOperationEntities = await _qualIqcOrderOperateRepository.GetEntitiesAsync(new QualIqcOrderOperateQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                IQCOrderIds = entities.Select(s => s.Id)
            });

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

                // 检验人
                var inspectionEntity = orderOperationEntities.FirstOrDefault(f => f.OperationType == OrderOperateTypeEnum.Complete);
                if (inspectionEntity != null)
                {
                    dto.InspectionBy = inspectionEntity.CreatedBy;
                    dto.InspectionOn = inspectionEntity.CreatedOn;
                }

                // 处理人
                var handleEntity = orderOperationEntities.FirstOrDefault(f => f.OperationType == OrderOperateTypeEnum.Close);
                if (handleEntity != null)
                {
                    dto.HandledBy = handleEntity.CreatedBy;
                    dto.HandledOn = handleEntity.CreatedOn;
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
        /// 转换附件实体为附件Dto
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
