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
using Hymson.MES.CoreServices.Bos.Quality;
using Hymson.MES.CoreServices.Services.Quality;
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
    public class QualIqcOrderReturnService : IQualIqcOrderReturnService
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
        /// 仓储接口（iqc检验单）
        /// </summary>
        private readonly IQualIqcOrderReturnRepository _qualIqcOrderReturnRepository;

        /// <summary>
        /// 仓储接口（iqc检验单明细）
        /// </summary>
        private readonly IQualIqcOrderReturnDetailRepository _qualIqcOrderReturnDetailRepository;

        /// <summary>
        /// 仓储接口（iqc检验单操作）
        /// </summary>
        private readonly IQualIqcOrderOperateRepository _qualIqcOrderOperateRepository;

        /// <summary>
        /// 仓储接口（iqc检验样本附件）
        /// </summary>
        private readonly IQualIqcOrderAnnexRepository _qualIqcOrderAnnexRepository;

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
        /// 服务接口（检验单生成）
        /// </summary>
        private readonly IIQCOrderCreateService _iqcOrderCreateService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="qualIqcOrderReturnRepository"></param>
        /// <param name="qualIqcOrderReturnDetailRepository"></param>
        /// <param name="qualIqcOrderOperateRepository"></param>
        /// <param name="qualIqcOrderAnnexRepository"></param>
        /// <param name="whMaterialReceiptRepository"></param>
        /// <param name="whMaterialReceiptDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whSupplierRepository"></param>
        /// <param name="inteAttachmentRepository"></param>
        /// <param name="iqcOrderCreateService"></param>
        public QualIqcOrderReturnService(ICurrentUser currentUser, ICurrentSite currentSite,
            IQualIqcOrderReturnRepository qualIqcOrderReturnRepository,
            IQualIqcOrderReturnDetailRepository qualIqcOrderReturnDetailRepository,
            IQualIqcOrderOperateRepository qualIqcOrderOperateRepository,
            IQualIqcOrderAnnexRepository qualIqcOrderAnnexRepository,
            IWhMaterialReceiptRepository whMaterialReceiptRepository,
            IWhMaterialReceiptDetailRepository whMaterialReceiptDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IWhSupplierRepository whSupplierRepository,
            IInteAttachmentRepository inteAttachmentRepository,
            IIQCOrderCreateService iqcOrderCreateService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _qualIqcOrderReturnRepository = qualIqcOrderReturnRepository;
            _qualIqcOrderReturnDetailRepository = qualIqcOrderReturnDetailRepository;
            _qualIqcOrderOperateRepository = qualIqcOrderOperateRepository;
            _qualIqcOrderAnnexRepository = qualIqcOrderAnnexRepository;
            _whMaterialReceiptRepository = whMaterialReceiptRepository;
            _whMaterialReceiptDetailRepository = whMaterialReceiptDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _whSupplierRepository = whSupplierRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
            _iqcOrderCreateService = iqcOrderCreateService;
        }


        /// <summary>
        /// 生成IQC检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<long> GeneratedOrderAsync(GenerateOrderReturnDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 读取收货单
            var receiptEntity = await _whMaterialReceiptRepository.GetByIdAsync(requestDto.ReceiptId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES11901));

            // 校验是否已生成过检验单
            var orderEntities = await _qualIqcOrderReturnRepository.GetEntitiesAsync(new QualIqcOrderReturnQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                MaterialReceiptId = receiptEntity.Id
            });
            if (orderEntities != null && orderEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11993))
                    .WithData("ReceiptNum", receiptEntity.ReceiptNum);
            }

            // 当前信息
            var user = _currentUser.UserName;
            var time = HymsonClock.Now();

            // 读取收货单明细
            var receiptDetailEntities = await _whMaterialReceiptDetailRepository.GetEntitiesAsync(new WhMaterialReceiptDetailQuery
            {
                SiteId = receiptEntity.SiteId,
                MaterialReceiptId = receiptEntity.Id
            });

            // 生成检验单号
            var inspectionOrder = await _iqcOrderCreateService.GenerateIQCOrderCodeAsync(new IQCOrderCreateBo
            {
                SiteId = receiptEntity.SiteId,
                UserName = user,
                MaterialReceiptEntity = receiptEntity,
                MaterialReceiptDetailEntities = receiptDetailEntities
            });

            // 检验单
            var orderEntity = new QualIqcOrderReturnEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = receiptEntity.SiteId,
                InspectionOrder = inspectionOrder,
                MaterialReceiptId = receiptEntity.Id,
                SupplierId = receiptEntity.SupplierId,
                Status = InspectionStatusEnum.WaitInspect,
                IsQualified = null,
                CreatedBy = user,
                CreatedOn = time
            };

            // 检验单明细
            var orderDetailEntities = receiptDetailEntities.Select(s => new QualIqcOrderReturnDetailEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = receiptEntity.SiteId,
                IQCOrderId = orderEntity.Id,
                MaterialReceiptDetailId = s.Id,
                MaterialId = s.MaterialId,
                IsQualified = null,
                CreatedBy = user,
                CreatedOn = time
            });

            // 保存
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _qualIqcOrderReturnRepository.InsertAsync(orderEntity);
            rows += await _qualIqcOrderReturnDetailRepository.InsertRangeAsync(orderDetailEntities);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 更改检验单状态（点击执行检验）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> OperationOrderAsync(QualOrderReturnOperationStatusDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // IQC检验单
            var entity = await _qualIqcOrderReturnRepository.GetByIdAsync(requestDto.IQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 检查当前操作类型是否已经执行过
            if (entity.Status != InspectionStatusEnum.WaitInspect) return default;
            switch (entity.Status)
            {
                case InspectionStatusEnum.WaitInspect:
                    // 继续接下来的操作
                    break;
                case InspectionStatusEnum.Completed:
                case InspectionStatusEnum.Closed:
                    throw new CustomerValidationException(nameof(ErrorCode.MES11914))
                        .WithData("Status", $"{InspectionStatusEnum.Completed.GetDescription()}/{InspectionStatusEnum.Closed.GetDescription()}");
                case InspectionStatusEnum.Inspecting:
                default: return default;
            }

            // 更改状态
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
            rows += await CommonOperationAsync(entity, OrderOperateTypeEnum.Start);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 保存样品数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SaveOrderAsync(QualIqcOrderReturnSaveDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // IQC检验单
            var orderEntity = await _qualIqcOrderReturnRepository.GetByIdAsync(requestDto.IQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 检验单明细
            var detailEntities = await _qualIqcOrderReturnDetailRepository.GetEntitiesAsync(new QualIqcOrderReturnDetailQuery
            {
                SiteId = orderEntity.SiteId,
                IQCOrderId = orderEntity.Id
            });
            if (detailEntities == null || !detailEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11994)).WithData("Code", orderEntity.InspectionOrder);
            }

            // 更新时间
            var user = _currentUser.UserName;
            var time = HymsonClock.Now();

            var isEqual = requestDto.Details.Count() == detailEntities.Count();
            if (!isEqual) throw new CustomerValidationException(nameof(ErrorCode.MES11995));

            // 遍历样本参数
            List<QualIqcOrderReturnDetailEntity> updateDetailEntities = new();
            foreach (var detailEntity in detailEntities)
            {
                var dto = requestDto.Details.FirstOrDefault(f => f.Id == detailEntity.Id);
                if (dto == null) continue;

                detailEntity.IsQualified = dto.IsQualified;
                detailEntity.Remark = dto.Remark ?? "";
                detailEntity.HandMethod = dto.HandMethod;
                detailEntity.ProcessedBy = user;
                detailEntity.ProcessedOn = time;
                detailEntity.UpdatedBy = user;
                detailEntity.UpdatedOn = time;

                updateDetailEntities.Add(detailEntity);
            }

            // 更新检验单状态
            orderEntity.Status = InspectionStatusEnum.Completed;
            orderEntity.UpdatedBy = user;
            orderEntity.UpdatedOn = time;

            // 保存
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _qualIqcOrderReturnRepository.UpdateAsync(orderEntity);
            rows += await _qualIqcOrderReturnDetailRepository.UpdateRangeAsync(updateDetailEntities);
            trans.Complete();
            return rows;
        }


        /// <summary>
        /// 保存检验单附件
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SaveAttachmentAsync(QualIqcOrderSaveAttachmentDto requestDto)
        {
            // 更新时间
            var user = _currentUser.UserName;
            var time = HymsonClock.Now();

            // IQC检验单
            var entity = await _qualIqcOrderReturnRepository.GetByIdAsync(requestDto.IQCOrderId)
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
                    CreatedBy = user,
                    CreatedOn = time,
                    UpdatedBy = user,
                    UpdatedOn = time,
                    SiteId = entity.SiteId,
                });

                orderAnnexEntities.Add(new QualIqcOrderAnnexEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = entity.SiteId,
                    IQCOrderId = requestDto.IQCOrderId,
                    AnnexId = attachmentId,
                    CreatedBy = user,
                    CreatedOn = time,
                    UpdatedBy = user,
                    UpdatedOn = time
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
            trans.Complete();
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

            var entities = await _qualIqcOrderReturnRepository.GetByIdsAsync(ids);
            if (entities != null && entities.Any(a => a.Status != InspectionStatusEnum.WaitInspect))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10137));
            }

            return await _qualIqcOrderReturnRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualIqcOrderReturnBaseDto?> QueryByIdAsync(long id)
        {
            var entity = await _qualIqcOrderReturnRepository.GetByIdAsync(id);
            if (entity == null) return null;

            // 实体到DTO转换
            var dto = entity.ToModel<QualIqcOrderReturnBaseDto>();
            dto.StatusText = dto.Status.GetDescription();
            dto.InspectionTime = entity.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");

            // 读取收货单
            var receiptEntity = await _whMaterialReceiptRepository.GetByIdAsync(entity.MaterialReceiptId);
            if (receiptEntity == null) return dto;

            dto.ReceiptNum = receiptEntity.ReceiptNum;
            dto.ReceiptTime = receiptEntity.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");

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
        /// 查询检验单明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualIqcOrderReturnDetailDto>?> QueryOrderDetailAsync(long id)
        {
            List<QualIqcOrderReturnDetailDto> dtos = new();

            // 检验单
            var orderEntity = await _qualIqcOrderReturnRepository.GetByIdAsync(id);
            if (orderEntity == null) return dtos;

            // 检验单明细
            var detailEntities = await _qualIqcOrderReturnDetailRepository.GetEntitiesAsync(new QualIqcOrderReturnDetailQuery
            {
                SiteId = orderEntity.SiteId,
                IQCOrderId = orderEntity.Id
            });
            if (detailEntities == null || !detailEntities.Any()) return dtos;

            // 收货单明细
            var receiptDetailEntities = await _whMaterialReceiptDetailRepository.GetEntitiesAsync(new WhMaterialReceiptDetailQuery
            {
                SiteId = orderEntity.SiteId,
                MaterialReceiptId = orderEntity.MaterialReceiptId
            });

            // 读取产品
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(detailEntities.Where(w => w.MaterialId.HasValue).Select(x => x.MaterialId!.Value));
            var materialDic = materialEntities.ToDictionary(x => x.Id, x => x);

            // 遍历
            foreach (var entity in detailEntities)
            {
                var dto = entity.ToModel<QualIqcOrderReturnDetailDto>();

                // 收货单明细
                var materialReceiptDetailEntity = receiptDetailEntities.FirstOrDefault(f => f.Id == entity.MaterialReceiptDetailId);
                if (materialReceiptDetailEntity != null)
                {
                    dto.InternalBatch = materialReceiptDetailEntity.InternalBatch;
                    dto.Qty = materialReceiptDetailEntity.Qty;
                }

                // 产品
                if (entity.MaterialId.HasValue)
                {
                    var materialEntity = materialDic[entity.MaterialId.Value];
                    if (materialEntity != null)
                    {
                        //dto.IsFree = TrueOrFalseEnum.Yes;   // TODO
                        dto.MaterialCode = materialEntity.MaterialCode;
                        dto.MaterialName = materialEntity.MaterialName;
                        dto.MaterialVersion = materialEntity.Version ?? "";
                        dto.Specifications = materialEntity.Specifications ?? "";
                        //dto.Unit = materialEntity.Unit ?? "";
                    }
                }
                else
                {
                    dto.MaterialCode = "-";
                    dto.MaterialName = "-";
                    dto.MaterialVersion = "-";
                }

                // 如果是免检
                if (dto.IsFree == TrueOrFalseEnum.Yes)
                {
                    dto.IsQualified = TrueOrFalseEnum.Yes;
                    dto.Remark = "";
                }

                // 输出显示文本
                dto.IsFreeText = dto.IsFree.GetDescription();

                if (dto.IsQualified.HasValue) dto.IsQualifiedText = dto.IsQualified.GetDescription();
                else dto.IsQualifiedText = "";

                if (dto.HandMethod.HasValue) dto.HandMethodText = dto.HandMethod.GetDescription();
                else dto.HandMethodText = "";

                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualIqcOrderReturnDto>> GetPagedListAsync(QualIqcOrderReturnPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualIqcOrderReturnPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;

            // 转换产品编码/版本变为产品ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.MaterialCode)
                || !string.IsNullOrWhiteSpace(pagedQueryDto.MaterialName)
                || !string.IsNullOrWhiteSpace(pagedQueryDto.MaterialVersion))
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
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.SupplierCode)
                || !string.IsNullOrWhiteSpace(pagedQueryDto.SupplierName))
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
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.SupplierBatch)
                || !string.IsNullOrWhiteSpace(pagedQueryDto.InternalBatch))
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
            var pagedInfo = await _qualIqcOrderReturnRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = await PrepareOrderDtosAsync(pagedInfo.Data);
            return new PagedInfo<QualIqcOrderReturnDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
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


        #region 内部方法
        /// <summary>
        /// 通用操作（未加事务）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operationType"></param>
        /// <param name="handleBo"></param>
        /// <returns></returns>
        private async Task<int> CommonOperationAsync(QualIqcOrderReturnEntity entity, OrderOperateTypeEnum operationType, QCHandleBo? handleBo = null)
        {
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 更新检验单
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            var rows = 0;
            rows += await _qualIqcOrderReturnRepository.UpdateAsync(entity);
            rows += await _qualIqcOrderOperateRepository.InsertAsync(new QualIqcOrderOperateEntity
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

            return rows;
        }

        /// <summary>
        /// 转换为Dto对象
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private async Task<IEnumerable<QualIqcOrderReturnDto>> PrepareOrderDtosAsync(IEnumerable<QualIqcOrderReturnEntity> entities)
        {
            List<QualIqcOrderReturnDto> dtos = new();

            // 读取收货单
            var receiptEntities = await _whMaterialReceiptRepository.GetByIdsAsync(entities.Select(x => x.MaterialReceiptId));
            var receiptDic = receiptEntities.ToDictionary(x => x.Id, x => x);

            // 检验单操作
            var orderOperationEntities = await _qualIqcOrderOperateRepository.GetEntitiesAsync(new QualIqcOrderOperateQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                IQCOrderIds = entities.Select(s => s.Id)
            });

            // 读取供应商
            var supplierEntities = await _whSupplierRepository.GetByIdsAsync(entities.Where(w => w.SupplierId.HasValue).Select(x => x.SupplierId!.Value));
            var supplierDic = supplierEntities.ToDictionary(x => x.Id, x => x);

            foreach (var entity in entities)
            {
                var dto = entity.ToModel<QualIqcOrderReturnDto>();
                if (dto == null) continue;

                // 收货单
                var receiptEntity = receiptDic[entity.MaterialReceiptId];
                if (receiptEntity != null)
                {
                    dto.ReceiptNum = receiptEntity.ReceiptNum;
                }

                // 检验人
                var inspectionEntity = orderOperationEntities.FirstOrDefault(f => f.OperationType == OrderOperateTypeEnum.Start);
                if (inspectionEntity != null)
                {
                    dto.InspectionBy = inspectionEntity.CreatedBy;
                    dto.InspectionOn = inspectionEntity.CreatedOn;
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

        #endregion

    }
}
