using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Data.Repositories.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.MES.Data.Repositories.Integrated;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（FQC检验单） 
    /// </summary>
    public class QualFqcOrderService : IQualFqcOrderService
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
        private readonly AbstractValidator<QualFqcOrderSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（FQC检验单）
        /// </summary>
        private readonly IQualFqcOrderRepository _qualFqcOrderRepository;

        /// <summary>
        /// 仓储接口（FQC检验单附件）
        /// </summary>
        private readonly IQualFqcOrderAttachmentRepository _qualFqcOrderAttachmentRepository;

        /// <summary>
        /// 仓储接口（Fqc样本）
        /// </summary>
        private readonly IQualFqcOrderSampleRepository _qualFqcOrderSampleRepository;

        /// <summary>
        /// 仓储接口（FQC检验单样品检验详情）
        /// </summary>
        private readonly IQualFqcOrderSampleDetailRepository _qualFqcOrderSampleDetailRepository;

        /// <summary>
        /// 仓储接口（FQC检验单样本明细附件）
        /// </summary>
        private readonly IQualFqcOrderSampleDetailAttachmentRepository _qualFqcOrderSampleDetailAttachmentRepository;

        /// <summary>
        /// 仓储接口（附件维护）
        /// </summary>
        private readonly IInteAttachmentRepository _inteAttachmentRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（FQC不合格处理结果）
        /// </summary>
        private readonly IQualFqcOrderUnqualifiedHandleRepository _qualFqcOrderUnqualifiedHandleRepository;

        /// <summary>
        /// 仓储接口（FQC检验单操作记录）
        /// </summary>
        private readonly IQualFqcOrderOperateRepository _qualFqcOrderOperateRepository;

        /// <summary>
        /// 仓储接口（FQC检验参数组明细快照）
        /// </summary>
        private readonly IQualFqcParameterGroupDetailSnapshootRepository _qualFqcParameterGroupDetailSnapshootRepository;

        /// <summary>
        /// 仓储接口（FQC检验参数组快照）
        /// </summary>
        private readonly IQualFqcParameterGroupSnapshootRepository _qualFqcParameterGroupSnapshootRepository;

        



        /// <summary>
        /// struct
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="qualFqcOrderRepository"></param>
        /// <param name="qualFqcOrderAttachmentRepository"></param>
        /// <param name="qualFqcOrderSampleRepository"></param>
        /// <param name="qualFqcOrderSampleDetailRepository"></param>
        /// <param name="qualFqcOrderSampleDetailAttachmentRepository"></param>
        /// <param name="inteAttachmentRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="qualFqcOrderUnqualifiedHandleRepository"></param>
        /// <param name="qualFqcOrderOperateRepository"></param>
        /// <param name="qualFqcParameterGroupDetailSnapshootRepository"></param>
        /// <param name="qualFqcParameterGroupSnapshootRepository"></param>
        public QualFqcOrderService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<QualFqcOrderSaveDto> validationSaveRules, 
            IQualFqcOrderRepository qualFqcOrderRepository,
            IQualFqcOrderAttachmentRepository qualFqcOrderAttachmentRepository,
            IQualFqcOrderSampleRepository qualFqcOrderSampleRepository, 
            IQualFqcOrderSampleDetailRepository qualFqcOrderSampleDetailRepository,
            IQualFqcOrderSampleDetailAttachmentRepository qualFqcOrderSampleDetailAttachmentRepository,
            IInteAttachmentRepository inteAttachmentRepository,
            IProcMaterialRepository procMaterialRepository,
            IQualFqcOrderUnqualifiedHandleRepository qualFqcOrderUnqualifiedHandleRepository,
            IQualFqcOrderOperateRepository qualFqcOrderOperateRepository,
            IQualFqcParameterGroupDetailSnapshootRepository qualFqcParameterGroupDetailSnapshootRepository,
            IQualFqcParameterGroupSnapshootRepository qualFqcParameterGroupSnapshootRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _qualFqcOrderRepository = qualFqcOrderRepository;
            _qualFqcOrderAttachmentRepository= qualFqcOrderAttachmentRepository;
            _qualFqcOrderSampleRepository = qualFqcOrderSampleRepository;
            _qualFqcOrderSampleDetailRepository= qualFqcOrderSampleDetailRepository;
            _qualFqcOrderSampleDetailAttachmentRepository= qualFqcOrderSampleDetailAttachmentRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
            _procMaterialRepository= procMaterialRepository;
            _qualFqcOrderUnqualifiedHandleRepository= qualFqcOrderUnqualifiedHandleRepository;
            _qualFqcOrderOperateRepository = qualFqcOrderOperateRepository;
            _qualFqcParameterGroupDetailSnapshootRepository = qualFqcParameterGroupDetailSnapshootRepository;
            _qualFqcParameterGroupSnapshootRepository= qualFqcParameterGroupSnapshootRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(QualFqcOrderSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<QualFqcOrderEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _qualFqcOrderRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(QualFqcOrderSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<QualFqcOrderEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _qualFqcOrderRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _qualFqcOrderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _qualFqcOrderRepository.DeletesAsync(new DeleteCommand
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
        //public async Task<QualFqcOrderDto?> QueryByIdAsync(long id) 
        //{
        //   var qualFqcOrderEntity = await _qualFqcOrderRepository.GetByIdAsync(id);
        //   if (qualFqcOrderEntity == null) return null;
           
        //   return qualFqcOrderEntity.ToModel<QualFqcOrderDto>();
        //}

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        //public async Task<PagedInfo<QualFqcOrderDto>> GetPagedListAsync(QualFqcOrderPagedQueryDto pagedQueryDto)
        //{
        //    var pagedQuery = pagedQueryDto.ToQuery<QualFqcOrderPagedQuery>();
        //    pagedQuery.SiteId = _currentSite.SiteId ?? 0;
        //    var pagedInfo = await _qualFqcOrderRepository.GetPagedListAsync(pagedQuery);

        //    // 实体到DTO转换 装载数据
        //    var dtos = pagedInfo.Data.Select(s => s.ToModel<QualFqcOrderDto>());
        //    return new PagedInfo<QualFqcOrderDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        //}

        /// <summary>
        /// 生成IQC检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<long> GeneratedOrderAsync(GenerateInspectionDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 收货单明细
            //var receiptDetails = await _whMaterialReceiptDetailRepository.GetByIdsAsync(requestDto.Details);
            //if (receiptDetails == null || !receiptDetails.Any())
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            //}
            //校验是否属于同一收货单
            //if (receiptDetails.Select(x => x.MaterialReceiptId).Distinct().Count() > 1)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES11900));
            //}
            // 查询收货单
            //var receiptEntity = await _whMaterialReceiptRepository.GetByIdAsync(requestDto.ReceiptId)
            //    ?? throw new CustomerValidationException(nameof(ErrorCode.MES11901));

            //校验是否已生成过检验单
            var orderList = await _qualFqcOrderRepository.GetEntitiesAsync(new QualFqcOrderQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                MaterialReceiptDetailIds = requestDto.Details
            });
            if (orderList != null && orderList.Any())
            {
                //throw new CustomerValidationException(nameof(ErrorCode.MES11990)).WithData("ReceiptNum", receiptEntity.ReceiptNum).WithData("MaterialReceiptDetailIds", string.Join(',', orderList.Select(x => x.MaterialReceiptDetailId).Distinct()));
            }

            var bo = new CoreServices.Bos.Quality.IQCOrderCreateBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                //MaterialReceiptEntity = receiptEntity,
                //MaterialReceiptDetailEntities = receiptDetails,
            };

            //return await _iqcOrderCreateService.CreateAsync(bo);
            return 1;
        }

        /// <summary>
        /// 更改检验单状态（点击执行检验）
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> OperationOrderAsync(QualOrderOperationStatusDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // IQC检验单
            var entity = await _qualFqcOrderRepository.GetByIdAsync(requestDto.IQCOrderId)
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
        public async Task<int> SaveOrderAsync(QualIqcOrderSaveDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // IQC检验单
            var entity = await _qualFqcOrderRepository.GetByIdAsync(requestDto.IQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // IQC检验类型
            //var orderTypeEntity = await _qualIqcOrderTypeRepository.GetByIdAsync(requestDto.IQCOrderTypeId)
            //    ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 检查样品条码在该类型是否已经录入
            var sampleQuery = requestDto.ToQuery<QualFqcOrderSampleQuery>();
            sampleQuery.SiteId = entity.SiteId;
            var sampleEntities = await _qualFqcOrderSampleRepository.GetEntitiesAsync(sampleQuery);
            if (sampleEntities != null && sampleEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11908))
                    .WithData("Code", requestDto.Barcode);
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 样本
            var sampleId = IdGenProvider.Instance.CreateId();
            var sampleEntity = new QualFqcOrderSampleEntity
            {
                Id = sampleId,
                SiteId = entity.SiteId,
                FQCOrderId = entity.Id,
                Barcode = requestDto.Barcode,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };

            // 遍历样本参数
            List<InteAttachmentEntity> attachmentEntities = new();
            List<QualFqcOrderSampleDetailEntity> sampleDetailEntities = new();
            List<QualFqcOrderSampleDetailAttachmentEntity> sampleDetailAttachmentEntities = new();
            foreach (var item in requestDto.Details)
            {
                var sampleDetailId = IdGenProvider.Instance.CreateId();
                sampleDetailEntities.Add(new QualFqcOrderSampleDetailEntity
                {
                    Id = sampleDetailId,
                    SiteId = entity.SiteId,
                    FQCOrderId = entity.Id,
                    FQCOrderSampleId = sampleId,
                    GroupDetailSnapshootId = item.Id,
                    InspectionValue = item.InspectionValue ?? "",
                    IsQualified = item.IsQualified,
                    Remark = item.Remark,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });

                // 样本附件
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
                        sampleDetailAttachmentEntities.Add(new QualFqcOrderSampleDetailAttachmentEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = entity.SiteId,
                            FQCOrderId = requestDto.IQCOrderId,
                            SampleDetailId = sampleDetailId,
                            AttachmentId = attachmentId,
                            CreatedBy = updatedBy,
                            CreatedOn = updatedOn,
                            UpdatedBy = updatedBy,
                            UpdatedOn = updatedOn
                        });
                    }
                }
            }

            // 更新已检数量
            //orderTypeEntity.CheckedQty += 1;
            //orderTypeEntity.UpdatedBy = updatedBy;
            //orderTypeEntity.UpdatedOn = updatedOn;

            // 保存
            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _qualFqcOrderSampleRepository.InsertAsync(sampleEntity);
            rows += await _qualFqcOrderSampleDetailRepository.InsertRangeAsync(sampleDetailEntities);
            if (attachmentEntities.Any())
            {
                rows += await _inteAttachmentRepository.InsertRangeAsync(attachmentEntities);
                rows += await _qualFqcOrderSampleDetailAttachmentRepository.InsertRangeAsync(sampleDetailAttachmentEntities);
            }
            //rows += await _qualIqcOrderTypeRepository.UpdateAsync(orderTypeEntity);
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

            // FQC检验单
            var entity = await _qualFqcOrderRepository.GetByIdAsync(requestDto.IQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 只有"检验中"的状态才允许点击"完成"
            if (entity.Status != InspectionStatusEnum.Inspecting)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11912))
                    .WithData("Before", InspectionStatusEnum.Inspecting.GetDescription())
                    .WithData("After", InspectionStatusEnum.Completed.GetDescription());
            }
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 检查每种类型是否已经录入足够
            //var orderTypeEntities = await _qualIqcOrderTypeRepository.GetByOrderIdAsync(entity.Id);

            //// 读取一个未录入完整的类型
            //var orderTypeEntity = orderTypeEntities.FirstOrDefault(f => f.SampleQty > f.CheckedQty);
            //if (orderTypeEntity != null)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES11911))
            //        .WithData("Type", orderTypeEntity.Type.GetDescription())
            //        .WithData("CheckedQty", orderTypeEntity.CheckedQty)
            //        .WithData("SampleQty", orderTypeEntity.SampleQty);
            //}

            // 读取所有明细参数
            var sampleDetailEntities = await _qualFqcOrderSampleDetailRepository.GetEntitiesAsync(new QualFqcOrderSampleDetailQuery
            {
                SiteId = entity.SiteId,
                FQCOrderId = entity.Id
            });

            var operationType = OrderOperateTypeEnum.Complete;
            //entity.IsQualified = TrueOrFalseEnum.Yes;

            // 如果不合格数超过接收水准，则设置为"完成"
            //if (sampleDetailEntities.Count(c => c.IsQualified == TrueOrFalseEnum.No) > entity.AcceptanceLevel)
            //{
            //    entity.Status = InspectionStatusEnum.Completed;
            //    entity.IsQualified = TrueOrFalseEnum.No;
            //    operationType = OrderOperateTypeEnum.Complete;
            //}
            //else
            //{
            //    // 默认是关闭
            //    entity.Status = InspectionStatusEnum.Closed;
            //    operationType = OrderOperateTypeEnum.Close;
            //}

            //有任一不合格，完成
            if(sampleDetailEntities.Any(X=>X.IsQualified== TrueOrFalseEnum.No))
            {
                entity.Status = InspectionStatusEnum.Completed;
                entity.IsQualified = TrueOrFalseEnum.No;
                operationType = OrderOperateTypeEnum.Complete;
            }
            else
            {
                // 默认是关闭
                entity.Status = InspectionStatusEnum.Closed;
                operationType = OrderOperateTypeEnum.Close;
            }

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await CommonOperationAsync(entity, operationType);
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 免检
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        //public async Task<int> FreeOrderAsync(QualIqcOrderFreeDto requestDto)
        //{
        //    // 判断是否有获取到站点码 
        //    if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

        //    // IQC检验单
        //    var entity = await _qualIqcOrderRepository.GetByIdAsync(requestDto.IQCOrderId)
        //        ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

        //    /*
        //    // 只有"检验中"的状态才允许点击"免检"
        //    if (entity.Status != InspectionStatusEnum.Inspecting)
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES11912))
        //            .WithData("Before", InspectionStatusEnum.Inspecting.GetDescription())
        //            .WithData("After", "免检");   // InspectionStatusEnum.Closed.GetDescription()
        //    }

        //    // 检查当前操作类型是否已经执行过
        //    await CheckOperationAsync(entity, OrderOperateTypeEnum.Close);
        //    */

        //    // 关闭检验单
        //    entity.IsExemptInspection = TrueOrFalseEnum.Yes;
        //    entity.IsQualified = TrueOrFalseEnum.Yes;
        //    entity.Status = InspectionStatusEnum.Closed;

        //    var rows = 0;
        //    using var trans = TransactionHelper.GetTransactionScope();
        //    rows += await CommonOperationAsync(entity, OrderOperateTypeEnum.Close);
        //    trans.Complete();
        //    return rows;
        //}

        /// <summary>
        /// 关闭检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> CloseOrderAsync(QualFqcOrderCloseDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // IQC检验单
            var entity = await _qualFqcOrderRepository.GetByIdAsync(requestDto.FQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 只有"已检验"的状态才允许"关闭"
            if (entity.Status != InspectionStatusEnum.Completed)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11912))
                    .WithData("Before", InspectionStatusEnum.Completed.GetDescription())
                    .WithData("After", InspectionStatusEnum.Closed.GetDescription());
            }

            // 不合格处理完成之后直接关闭（无需变为合格）
            //entity.IsQualified = TrueOrFalseEnum.Yes;
            entity.Status = InspectionStatusEnum.Closed;

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await CommonOperationAsync(entity, OrderOperateTypeEnum.Close, new QCHandleBo { HandMethod = requestDto.HandMethod, Remark = requestDto.Remark });
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
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // FQC检验单
            var entity = await _qualFqcOrderRepository.GetByIdAsync(requestDto.IQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            if (!requestDto.Attachments.Any()) return 0;

            List<InteAttachmentEntity> attachmentEntities = new();
            List<QualFqcOrderAttachmentEntity> orderAnnexEntities = new();
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

                orderAnnexEntities.Add(new QualFqcOrderAttachmentEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = entity.SiteId,
                    FQCOrderId = requestDto.IQCOrderId,
                    AttachmentId = attachmentId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });
            }

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _inteAttachmentRepository.InsertRangeAsync(attachmentEntities);
            rows += await _qualFqcOrderAttachmentRepository.InsertRangeAsync(orderAnnexEntities);
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
            var attachmentEntity = await _qualFqcOrderAttachmentRepository.GetByIdAsync(orderAnnexId);
            if (attachmentEntity == null) return default;

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _inteAttachmentRepository.DeleteAsync(attachmentEntity.AttachmentId);
            rows += await _qualFqcOrderAttachmentRepository.DeleteAsync(attachmentEntity.Id);
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

            var entities = await _qualFqcOrderRepository.GetByIdsAsync(ids);
            if (entities != null && entities.Any(a => a.Status != InspectionStatusEnum.WaitInspect))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10137));
            }

            return await _qualFqcOrderRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualFqcOrderDto?> QueryByIdAsync(long id)
        {
            var entity = await _qualFqcOrderRepository.GetByIdAsync(id);
            if (entity == null) return null;

            // 实体到DTO转换
            var dto = entity.ToModel<QualFqcOrderDto>();
            //dto.InspectionGradeText = dto.InspectionGrade.GetDescription();
            //dto.StatusText = dto.Status.GetDescription();

            //if (dto.IsQualified.HasValue) dto.IsQualifiedText = dto.IsQualified.GetDescription();
            //else dto.IsQualifiedText = "";

            // 读取收货单
            //var receiptDetailEntity = await _whMaterialReceiptDetailRepository.GetByIdAsync(entity.MaterialReceiptDetailId);
            //if (receiptDetailEntity == null) return dto;

            //dto.SupplierBatch = receiptDetailEntity.SupplierBatch;
            //dto.InternalBatch = receiptDetailEntity.InternalBatch;
            //dto.PlanQty = receiptDetailEntity.PlanQty;

            //// 读取收货单
            //var receiptEntity = await _whMaterialReceiptRepository.GetByIdAsync(receiptDetailEntity.MaterialReceiptId);
            //if (receiptEntity == null) return dto;

            //dto.ReceiptNum = receiptEntity.ReceiptNum;

            // TODO 规格型号
            //dto.Specifications = "-";

            //// 读取产品
            //if (entity.MaterialId.HasValue)
            //{
            //    var materialEntity = await _procMaterialRepository.GetByIdAsync(entity.MaterialId.Value);
            //    if (materialEntity != null)
            //    {
            //        dto.MaterialCode = materialEntity.MaterialCode;
            //        dto.MaterialName = materialEntity.MaterialName;
            //        dto.MaterialVersion = materialEntity.Version ?? "";
            //        dto.Unit = materialEntity.Unit ?? "";
            //    }
            //}

            //// 读取供应商
            //if (entity.SupplierId.HasValue)
            //{
            //    var supplierEntity = await _whSupplierRepository.GetByIdAsync(entity.SupplierId.Value);
            //    if (supplierEntity != null)
            //    {
            //        dto.SupplierCode = supplierEntity.Code;
            //        dto.SupplierName = supplierEntity.Name;
            //    }
            //}

            return dto;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> UpdateOrderAsync(OrderParameterDetailSaveDto requestDto)
        {
            var entity = await _qualFqcOrderSampleDetailRepository.GetByIdAsync(requestDto.Id);
            if (entity == null) return 0;

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            entity.IsQualified = requestDto.IsQualified;
            entity.InspectionValue = requestDto.InspectionValue ?? "";
            entity.Remark = requestDto.Remark;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            // 样本附件
            List<InteAttachmentEntity> attachmentEntities = new();
            List<QualFqcOrderSampleDetailAttachmentEntity> sampleDetailAttachmentEntities = new();
            if (requestDto.Attachments != null && requestDto.Attachments.Any())
            {
                foreach (var attachment in requestDto.Attachments)
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
                    sampleDetailAttachmentEntities.Add(new QualFqcOrderSampleDetailAttachmentEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = entity.SiteId,
                        FQCOrderId = entity.FQCOrderId,
                        SampleDetailId = entity.Id,
                        AttachmentId = attachmentId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });
                }
            }

            // 之前的附件
            var beforeAttachments = await _qualFqcOrderSampleDetailAttachmentRepository.GetEntitiesAsync(new QualFqcOrderSampleDetailAttachmentQuery
            {
                SiteId = entity.SiteId,
                FQCOrderId = entity.FQCOrderId,
            });

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _qualFqcOrderSampleDetailRepository.UpdateAsync(entity);

            // 先删除再添加
            if (beforeAttachments != null && beforeAttachments.Any())
            {
                rows += await _qualFqcOrderSampleDetailAttachmentRepository.DeletesAsync(new DeleteCommand
                {
                    UserId = updatedBy,
                    DeleteOn = updatedOn,
                    Ids = beforeAttachments.Select(s => s.Id)
                });

                rows += await _inteAttachmentRepository.DeletesAsync(new DeleteCommand
                {
                    UserId = updatedBy,
                    DeleteOn = updatedOn,
                    Ids = beforeAttachments.Select(s => s.AttachmentId)
                });
            }

            if (attachmentEntities.Any())
            {
                rows += await _inteAttachmentRepository.InsertRangeAsync(attachmentEntities);
                rows += await _qualFqcOrderSampleDetailAttachmentRepository.InsertRangeAsync(sampleDetailAttachmentEntities);
            }
            trans.Complete();
            return rows;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFqcOrderDto>> GetPagedListAsync(QualFqcOrderPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<QualFqcOrderPagedQuery>();
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
            //if (!string.IsNullOrWhiteSpace(pagedQueryDto.SupplierCode)
            //    || !string.IsNullOrWhiteSpace(pagedQueryDto.SupplierName))
            //{
            //    var whSupplierEntities = await _whSupplierRepository.GetWhSupplierEntitiesAsync(new WhSupplierQuery
            //    {
            //        SiteId = pagedQuery.SiteId,
            //        Code = pagedQueryDto.SupplierCode,
            //        Name = pagedQueryDto.SupplierName
            //    });
            //    if (whSupplierEntities != null && whSupplierEntities.Any()) pagedQuery.SupplierIds = whSupplierEntities.Select(s => s.Id);
            //    else pagedQuery.SupplierIds = Array.Empty<long>();
            //}

            // 将供应商批次/内部批次转换为收货单详情ID
            //if (!string.IsNullOrWhiteSpace(pagedQueryDto.SupplierBatch)
            //    || !string.IsNullOrWhiteSpace(pagedQueryDto.InternalBatch))
            //{
            //    var receiptDetailEntities = await _whMaterialReceiptDetailRepository.GetEntitiesAsync(new WhMaterialReceiptDetailQuery
            //    {
            //        SiteId = pagedQuery.SiteId,
            //        SupplierBatch = pagedQueryDto.SupplierBatch,
            //        InternalBatch = pagedQueryDto.InternalBatch
            //    });
            //    if (receiptDetailEntities != null && receiptDetailEntities.Any()) pagedQuery.MaterialReceiptDetailIds = receiptDetailEntities.Select(s => s.Id);
            //    else pagedQuery.MaterialReceiptDetailIds = Array.Empty<long>();
            //}

            // 将不合格处理方式转换为检验单ID
            if (pagedQueryDto.HandMethod.HasValue)
            {
                var unqualifiedHandEntities = await _qualFqcOrderUnqualifiedHandleRepository.GetEntitiesAsync(new QualFqcOrderUnqualifiedHandleQuery
                {
                    SiteId = pagedQuery.SiteId,
                    HandMethod = pagedQueryDto.HandMethod
                });
                if (unqualifiedHandEntities != null && unqualifiedHandEntities.Any()) pagedQuery.FQCOrderIds = unqualifiedHandEntities.Select(s => s.FQCOrderId);
                else pagedQuery.FQCOrderIds = Array.Empty<long>();
            }

            // 查询数据
            var pagedInfo = await _qualFqcOrderRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = await PrepareOrderDtosAsync(pagedInfo.Data);
            return new PagedInfo<QualFqcOrderDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询类型
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        //public async Task<IEnumerable<QualIqcOrderTypeBaseDto>> QueryOrderTypeListByIdAsync(long orderId)
        //{
        //    var entities = await _qualIqcOrderTypeRepository.GetByOrderIdAsync(orderId);
        //    return entities.Select(s => s.ToModel<QualIqcOrderTypeBaseDto>());
        //}

        /// <summary>
        /// 根据ID查询附件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryOrderAttachmentListByIdAsync(long orderId)
        {
            var iqcOrderAnnexs = await _qualFqcOrderAttachmentRepository.GetByOrderIdAsync(orderId);
            if (iqcOrderAnnexs == null) return Array.Empty<InteAttachmentBaseDto>();

            var attachmentEntities = await _inteAttachmentRepository.GetByIdsAsync(iqcOrderAnnexs.Select(s => s.AttachmentId));
            if (attachmentEntities == null) return Array.Empty<InteAttachmentBaseDto>();

            return PrepareAttachmentBaseDtos(iqcOrderAnnexs, attachmentEntities);
        }

        /// <summary>
        /// 查询检验单快照数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<FQCParameterDetailDto>> QueryDetailSnapshotAsync(FQCParameterDetailQueryDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.Barcode)) throw new CustomerValidationException(nameof(ErrorCode.MES11909));
            //if (!requestDto.IQCOrderTypeId.HasValue) throw new CustomerValidationException(nameof(ErrorCode.MES11910));

            var entity = await _qualFqcOrderRepository.GetByIdAsync(requestDto.FQCOrderId);
            if (entity == null) return Array.Empty<FQCParameterDetailDto>();

            //var orderTypeEntity = await _qualIqcOrderTypeRepository.GetByIdAsync(requestDto.IQCOrderTypeId.Value);
            //if (orderTypeEntity == null) return Array.Empty<FQCParameterDetailDto>();

            // 检查该类型是否已经录入
            //var sampleQuery = requestDto.ToQuery<QualIqcOrderSampleQuery>();
            //sampleQuery.SiteId = entity.SiteId;
            //var sampleEntities = await _qualIqcOrderSampleRepository.GetEntitiesAsync(sampleQuery);
            //if (sampleEntities != null && sampleEntities.Any())
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES11908))
            //        .WithData("Code", requestDto.Barcode)
            //        .WithData("Type", orderTypeEntity.Type.GetDescription());
            //}

            var snapshotEntity = await _qualFqcParameterGroupSnapshootRepository.GetByIdAsync(entity.GroupSnapshootId);
            if (snapshotEntity == null) return Array.Empty<FQCParameterDetailDto>();

            var snapshotDetailEntities = await _qualFqcParameterGroupDetailSnapshootRepository.GetEntitiesAsync(new QualFqcParameterGroupDetailSnapshootQuery
            {
                SiteId = entity.SiteId,
                ParameterGroupId = snapshotEntity.Id,
            });
            if (snapshotDetailEntities == null) return Array.Empty<FQCParameterDetailDto>();

            return snapshotDetailEntities.Select(s => s.ToModel<FQCParameterDetailDto>());
        }

        /// <summary>
        /// 查询检验单样本数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<FQCParameterDetailDto>> QueryDetailSampleAsync(FQCParameterDetailQueryDto requestDto)
        {
            var entity = await _qualFqcOrderRepository.GetByIdAsync(requestDto.FQCOrderId);
            if (entity == null) return Array.Empty<FQCParameterDetailDto>();

            // 查询检验单下面的所有样本明细
            var sampleDetailEntities = await _qualFqcOrderSampleDetailRepository.GetEntitiesAsync(new QualFqcOrderSampleDetailQuery
            {
                SiteId = entity.SiteId,
                FQCOrderId = entity.Id
            });
            if (sampleDetailEntities == null) return Array.Empty<FQCParameterDetailDto>();

            return await PrepareSampleDetailDtosAsync(entity, sampleDetailEntities);
        }

        /// <summary>
        /// 查询检验单样本数据（分页）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<FQCParameterDetailDto>> QueryDetailSamplePagedListAsync(FQCParameterDetailPagedQueryDto pagedQueryDto)
        {
            // 初始化集合
            var defaultResult = new PagedInfo<FQCParameterDetailDto>(Array.Empty<FQCParameterDetailDto>(), pagedQueryDto.PageIndex, pagedQueryDto.PageSize, 0);

            var entity = await _qualFqcOrderRepository.GetByIdAsync(pagedQueryDto.FQCOrderId);
            if (entity == null) return defaultResult;

            // 查询检验单下面的所有样本
            var pagedQuery = pagedQueryDto.ToQuery<QualFqcOrderSampleDetailPagedQuery>();
            pagedQuery.SiteId = entity.SiteId;

            // 转换产品编码/版本变为产品ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.Barcode))
            {
                // 查询检验单下面的所有样本
                var sampleEntities = await _qualFqcOrderSampleRepository.GetEntitiesAsync(new QualFqcOrderSampleQuery
                {
                    SiteId = entity.SiteId,
                    FQCOrderId = entity.Id,
                    Barcode = pagedQueryDto.Barcode
                });
                if (sampleEntities != null && sampleEntities.Any()) pagedQuery.FQCOrderSampleIds = sampleEntities.Select(s => s.Id);
                else pagedQuery.FQCOrderSampleIds = Array.Empty<long>();
            }

            // 转换项目编码变为快照明细ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.ParameterCode))
            {
                var snapshotDetailEntities = await _qualFqcParameterGroupDetailSnapshootRepository.GetEntitiesAsync(new QualFqcParameterGroupDetailSnapshootQuery
                {
                    SiteId = entity.SiteId,
                    ParameterCode = pagedQueryDto.ParameterCode
                });
                if (snapshotDetailEntities != null && snapshotDetailEntities.Any()) pagedQuery.FQCParameterGroupDetailSnapshootIds = snapshotDetailEntities.Select(s => s.Id);
                else pagedQuery.FQCParameterGroupDetailSnapshootIds = Array.Empty<long>();
            }

            // 查询数据
            var pagedInfo = await _qualFqcOrderSampleDetailRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = await PrepareSampleDetailDtosAsync(entity, pagedInfo.Data);
            return new PagedInfo<FQCParameterDetailDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }



        #region 内部方法
        /// <summary>
        /// 检查当前操作类型是否已经执行过
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="orderOperateType"></param>
        /// <returns></returns>
        private async Task CheckOperationAsync(QualIqcOrderEntity entity, OrderOperateTypeEnum orderOperateType)
        {
            if (entity == null) return;

            var orderOperationEntities = await _qualFqcOrderOperateRepository.GetEntitiesAsync(new QualFqcOrderOperateQuery
            {
                SiteId = entity.SiteId,
                FQCOrderId = entity.Id,
                OperationType = orderOperateType
            });
            if (orderOperationEntities != null && orderOperationEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11913))
                    .WithData("Code", entity.InspectionOrder)
                    .WithData("Operation", orderOperateType.GetDescription());
            }
        }

        /// <summary>
        /// 通用操作（未加事务）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="operationType"></param>
        /// <param name="handleBo"></param>
        /// <returns></returns>
        private async Task<int> CommonOperationAsync(QualFqcOrderEntity entity, OrderOperateTypeEnum operationType, QCHandleBo? handleBo = null)
        {
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 更新检验单
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;

            var rows = 0;
            rows += await _qualFqcOrderRepository.UpdateAsync(entity);
            rows += await _qualFqcOrderOperateRepository.InsertAsync(new QualFqcOrderOperateEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                FQCOrderId = entity.Id,
                //OperationType = operationType,
                OperateBy = updatedBy,
                OperateOn = updatedOn,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            });

            if (handleBo != null) rows += await _qualFqcOrderUnqualifiedHandleRepository.InsertAsync(new QualFqcOrderUnqualifiedHandleEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                FQCOrderId = entity.Id,
                SourceSystem = 1,   // 来源系统;1、本系统 2、OA  (需要加一个外部来源的通用枚举)
                HandMethod = handleBo.HandMethod,
                Remark = handleBo.Remark ?? "",
                ProcessedBy = updatedBy,
                ProcessedOn = updatedOn,
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
        private async Task<IEnumerable<QualFqcOrderDto>> PrepareOrderDtosAsync(IEnumerable<QualFqcOrderEntity> entities)
        {
            List<QualFqcOrderDto> dtos = new();

            // 读取收货单明细
            //var receiptDetailEntities = await _whMaterialReceiptDetailRepository.GetByIdsAsync(entities.Select(x => x.MaterialReceiptDetailId));
            //var receiptDetailDic = receiptDetailEntities.ToDictionary(x => x.Id, x => x);

            //// 读取收货单
            //var receiptEntities = await _whMaterialReceiptRepository.GetByIdsAsync(receiptDetailEntities.Select(x => x.MaterialReceiptId));
            //var receiptDic = receiptEntities.ToDictionary(x => x.Id, x => x);

            // 检验单操作
            var orderOperationEntities = await _qualFqcOrderOperateRepository.GetEntitiesAsync(new QualFqcOrderOperateQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                FQCOrderIds = entities.Select(s => s.Id)
            });

            // 读取产品
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(entities.Where(w => w.MaterialId.HasValue).Select(x => x.MaterialId!.Value));
            var materialDic = materialEntities.ToDictionary(x => x.Id, x => x);

            // 读取供应商
            //var supplierEntities = await _whSupplierRepository.GetByIdsAsync(entities.Where(w => w.SupplierId.HasValue).Select(x => x.SupplierId!.Value));
            //var supplierDic = supplierEntities.ToDictionary(x => x.Id, x => x);

            // 查询不合格处理数据
            var unqualifiedHandEntities = await _qualFqcOrderUnqualifiedHandleRepository.GetEntitiesAsync(new QualFqcOrderUnqualifiedHandleQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                FQCOrderIds = entities.Select(s => s.Id)
            });

            foreach (var entity in entities)
            {
                var dto = entity.ToModel<QualFqcOrderDto>();
                if (dto == null) continue;

                // 收货单明细
                //var receiptDetailEntity = receiptDetailDic[entity.MaterialReceiptDetailId];
                //if (receiptDetailEntity != null)
                //{
                //    dto.SupplierBatch = receiptDetailEntity.SupplierBatch;
                //    dto.InternalBatch = receiptDetailEntity.InternalBatch;
                //    dto.PlanQty = receiptDetailEntity.PlanQty;

                //    // 收货单
                //    var receiptEntity = receiptDic[receiptDetailEntity.MaterialReceiptId];
                //    if (receiptEntity != null) dto.ReceiptNum = receiptEntity.ReceiptNum;
                //}

                // 检验人
                var inspectionEntity = orderOperationEntities.FirstOrDefault(f => f.OperateType == OrderOperateTypeEnum.Start);
                if (inspectionEntity != null)
                {
                    dto.InspectionBy = inspectionEntity.CreatedBy;
                    dto.InspectionOn = inspectionEntity.CreatedOn;
                }

                // 处理人
                var handleEntity = unqualifiedHandEntities.FirstOrDefault(f => f.FQCOrderId == entity.Id);
                if (handleEntity != null)
                {
                    dto.HandMethod = handleEntity.HandMethod;
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
                }

                //// 供应商
                //if (entity.SupplierId.HasValue)
                //{
                //    var supplierEntity = supplierDic[entity.SupplierId.Value];
                //    if (supplierEntity != null)
                //    {
                //        dto.SupplierCode = supplierEntity.Code;
                //        dto.SupplierName = supplierEntity.Name;
                //    }
                //}
                //else
                //{
                //    dto.SupplierCode = "-";
                //    dto.SupplierName = "-";
                //}

                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 转换为Dto对象
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sampleDetailEntities"></param>
        /// <returns></returns>
        private async Task<IEnumerable<FQCParameterDetailDto>> PrepareSampleDetailDtosAsync(QualFqcOrderEntity entity, IEnumerable<QualFqcOrderSampleDetailEntity> sampleDetailEntities)
        {
            // 查询样品明细对应的快照明细
            var snapshotDetailEntities = await _qualFqcParameterGroupDetailSnapshootRepository.GetByIdsAsync(sampleDetailEntities.Select(s => s.GroupDetailSnapshootId));

            // 查询检验单下面的所有样本附件
            var sampleAttachmentEntities = await _qualFqcOrderSampleDetailAttachmentRepository.GetEntitiesAsync(new QualFqcOrderSampleDetailAttachmentQuery
            {
                SiteId = entity.SiteId,
                FQCOrderId = entity.Id
            });

            // 所有样品明细对应的样品集合
            var sampleEntities = await _qualFqcOrderSampleRepository.GetByIdsAsync(sampleDetailEntities.Select(s => s.FQCOrderSampleId));

            // 附件集合
            Dictionary<long, IGrouping<long, QualFqcOrderSampleDetailAttachmentEntity>> sampleAttachmentDic = new();
            IEnumerable<InteAttachmentEntity> attachmentEntities = Array.Empty<InteAttachmentEntity>();
            if (sampleAttachmentEntities.Any())
            {
                sampleAttachmentDic = sampleAttachmentEntities.ToLookup(w => w.SampleDetailId).ToDictionary(d => d.Key, d => d);
                attachmentEntities = await _inteAttachmentRepository.GetByIdsAsync(sampleAttachmentEntities.Select(s => s.AttachmentId));
            }

            List<FQCParameterDetailDto> dtos = new();
            foreach (var sampleDetailEntity in sampleDetailEntities)
            {
                // 快照数据
                var snapshotDetailEntity = snapshotDetailEntities.FirstOrDefault(f => f.Id == sampleDetailEntity.GroupDetailSnapshootId);
                if (snapshotDetailEntity == null) continue;

                var dto = snapshotDetailEntity.ToModel<FQCParameterDetailDto>();
                dto.Id = sampleDetailEntity.Id;
                dto.InspectionValue = sampleDetailEntity.InspectionValue;
                dto.IsQualified = sampleDetailEntity.IsQualified;
                dto.Remark = sampleDetailEntity.Remark;
                //dto.Scale = snapshotDetailEntity.Scale;

                // 填充条码
                var sampleEntity = sampleEntities.FirstOrDefault(f => f.Id == sampleDetailEntity.FQCOrderSampleId);
                if (sampleEntity == null) continue;
                dto.Barcode = sampleEntity.Barcode;

                // 填充附件
                if (attachmentEntities != null && sampleAttachmentDic.TryGetValue(sampleDetailEntity.Id, out var detailAttachmentEntities))
                {
                    dto.Attachments = PrepareAttachmentBaseDtos(detailAttachmentEntities, attachmentEntities);
                }
                else dto.Attachments = Array.Empty<InteAttachmentBaseDto>();

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
