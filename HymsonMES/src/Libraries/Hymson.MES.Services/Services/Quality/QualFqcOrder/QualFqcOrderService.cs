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
using Hymson.MES.CoreServices.Services.Quality.QualFqcOrder;
using Hymson.MES.Data.Repositories.Plan;
using static Hymson.MES.Services.Dtos.Quality.QualFqcParameterGroup;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System.Security.Policy;
using System.Linq;

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
        /// FQC检验单创建服务
        /// </summary>
        private readonly IFQCOrderCreateService _qualFqcOrderCreateService;


        /// <summary>
        /// 工单信息表仓储接口
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（FQC检验单条码记录）
        /// </summary>
        private readonly IQualFqcOrderSfcRepository _qualFqcOrderSfcRepository;

        /// <summary>
        /// 仓储接口（成品条码产出记录(FQC生成使用)）
        /// </summary>
        private readonly IQualFinallyOutputRecordRepository _qualFinallyOutputRecordRepository;


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
        /// <param name="qualFqcOrderCreateService"></param>
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
            IQualFqcParameterGroupSnapshootRepository qualFqcParameterGroupSnapshootRepository,
            IFQCOrderCreateService qualFqcOrderCreateService,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IQualFqcOrderSfcRepository qualFqcOrderSfcRepository, IQualFinallyOutputRecordRepository qualFinallyOutputRecordRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _qualFqcOrderRepository = qualFqcOrderRepository;
            _qualFqcOrderAttachmentRepository = qualFqcOrderAttachmentRepository;
            _qualFqcOrderSampleRepository = qualFqcOrderSampleRepository;
            _qualFqcOrderSampleDetailRepository = qualFqcOrderSampleDetailRepository;
            _qualFqcOrderSampleDetailAttachmentRepository = qualFqcOrderSampleDetailAttachmentRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
            _procMaterialRepository = procMaterialRepository;
            _qualFqcOrderUnqualifiedHandleRepository = qualFqcOrderUnqualifiedHandleRepository;
            _qualFqcOrderOperateRepository = qualFqcOrderOperateRepository;
            _qualFqcParameterGroupDetailSnapshootRepository = qualFqcParameterGroupDetailSnapshootRepository;
            _qualFqcParameterGroupSnapshootRepository = qualFqcParameterGroupSnapshootRepository;
            _qualFqcOrderCreateService = qualFqcOrderCreateService;
            _planWorkOrderRepository = planWorkOrderRepository;
            _qualFqcOrderSfcRepository = qualFqcOrderSfcRepository;
            _qualFinallyOutputRecordRepository = qualFinallyOutputRecordRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(QualFqcOrderCreateDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            var bo = new FQCOrderManualCreateBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                RecordIds = saveDto.OutputRecordIds
            };

            return await _qualFqcOrderCreateService.ManualCreateAsync(bo);
        }

        /// <summary>
        /// 创建(测试条码产出时自动生成功能)
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(QualFqcOrderCreateTestDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            var bo = new FQCOrderAutoCreateAutoBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                MaterialId = saveDto.MaterialId,
                Barcode = saveDto.Barcode,
                WorkOrderId = saveDto.WorkOrderId,
                WorkCenterId = saveDto.WorkCenterId,
                CodeType = saveDto.CodeType,
            };

            return await _qualFqcOrderCreateService.AutoCreateAsync(bo);
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
            var entitys = await _qualFqcOrderRepository.GetByIdsAsync(ids);
            if (entitys != null)
            {
                if (!entitys.Any(x => x.Status == InspectionStatusEnum.WaitInspect))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES11715));
                }
            }

            //还原条码生成状态
            //OrderSFC 查询
            var recordEntitys = Enumerable.Empty<QualFinallyOutputRecordEntity>();
            var fqcid = entitys?.Select(x => x.Id);
            var site = entitys?.Select(x => x.SiteId).FirstOrDefault() ?? 0;
            if (fqcid != null)
            {
                var orderSFCEntitys = await _qualFqcOrderSfcRepository.GetEntitiesAsync(new QualFqcOrderSfcQuery { FQCOrderIds = fqcid, SiteId = site });
                var sfcs = orderSFCEntitys.Select(x => x.SFC);
                if (sfcs != null)
                {
                    recordEntitys = await _qualFinallyOutputRecordRepository.GetEntitiesAsync(new QualFinallyOutputRecordQuery { Barcodes = sfcs, SiteId = site });
                }
            }

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();

            if (recordEntitys != null)
            {
                //更新qual_fqc_order
                foreach (var item in recordEntitys)
                {
                    item.IsGenerated = TrueOrFalseEnum.No;
                    item.UpdatedOn = DateTime.Now;
                    item.Remark = "检验单删除回滚";
                }
                await _qualFinallyOutputRecordRepository.UpdateRangeAsync(recordEntitys);

            }
            rows = await _qualFqcOrderRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });

            trans.Complete();
            return rows;

        }

        /// <summary>
        /// 生成FQC检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        public async Task<long> GeneratedOrderAsync(GenerateInspectionDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

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
        public async Task<int> OperationOrderAsync(OrderOperationStatusDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // FQC检验单
            var entity = await _qualFqcOrderRepository.GetByIdAsync(requestDto.OrderId)
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
        public async Task<int> SaveOrderAsync(QualFqcOrderSampleSaveDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            //检验单
            var entity = await _qualFqcOrderRepository.GetByIdAsync(requestDto.FQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 检查样品条码在该类型是否已经录入
            var sampleQuery = requestDto.ToQuery<QualFqcOrderSampleQuery>();
            sampleQuery.SiteId = entity.SiteId;
            var sampleEntities = await _qualFqcOrderSampleRepository.GetEntitiesAsync(sampleQuery);
            if (sampleEntities != null && sampleEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11708))
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
                            FQCOrderId = requestDto.FQCOrderId,
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
        public async Task<int> CompleteOrderAsync(QualFqcOrderCompleteDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // FQC检验单
            var entity = await _qualFqcOrderRepository.GetByIdAsync(requestDto.FQCOrderId)
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
            var sampleEntities = await _qualFqcOrderSampleRepository.GetEntitiesAsync(new QualFqcOrderSampleQuery
            {
                SiteId = entity.SiteId,
                FQCOrderId = entity.Id
            });

            //校验已检数量

            if (sampleEntities.Count() < entity.SampleQty)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11716)).WithData("CheckedQty", sampleEntities.Count()).WithData("SampleQty", entity.SampleQty);
            }
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
            if (sampleDetailEntities.Any(X => X.IsQualified == TrueOrFalseEnum.No))
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
        public async Task<int> SaveAttachmentAsync(QualFqcOrderSaveAttachmentDto requestDto)
        {
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // FQC检验单
            var entity = await _qualFqcOrderRepository.GetByIdAsync(requestDto.FQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES11714));

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
                    FQCOrderId = requestDto.FQCOrderId,
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
        //public async Task<int> DeleteOrdersAsync(long[] ids)
        //{
        //    if (!ids.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES10213));
        //    //只能删除待检验
        //    var entities = await _qualFqcOrderRepository.GetByIdsAsync(ids);
        //    if (entities != null && entities.Any(a => a.Status != InspectionStatusEnum.WaitInspect))
        //    {
        //        throw new CustomerValidationException(nameof(ErrorCode.MES10137));
        //    }        

        //    return await _qualFqcOrderRepository.DeletesAsync(new DeleteCommand
        //    {
        //        Ids = ids,
        //        DeleteOn = HymsonClock.Now(),
        //        UserId = _currentUser.UserName
        //    });
        //}

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
            dto.StatusText = dto.Status.GetDescription();

            if (dto.IsQualified.HasValue) dto.IsQualifiedText = dto.IsQualified.GetDescription();
            else dto.IsQualifiedText = "";

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
                    dto.Specifications = materialEntity.ProductModel ?? "-";
                }
            }

            //工单
            if (entity.WorkOrderId.HasValue)
            {
                var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(entity.WorkOrderId.Value);
                if (workOrderEntity != null)
                {
                    dto.OrderCode = workOrderEntity.OrderCode;
                }
            }

            return dto;
        }

        /// <summary>
        /// 更新已检明细 根据ID查询
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> UpdateOrderAsync(FQCParameterDetailSaveDto requestDto)
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

            // 转换工单编码变为WorkOrderId
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.OrderCode))
            {
                var planWorkOrderEntities = await _planWorkOrderRepository.GetByCodeAsync(new PlanWorkOrderQuery
                {
                    SiteId = pagedQuery.SiteId,
                    OrderCode = pagedQueryDto.OrderCode,
                });
                if (planWorkOrderEntities != null) pagedQuery.WorkOrderId = planWorkOrderEntities.Id;
                else pagedQuery.WorkOrderId = default;
            }

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
        /// 根据ID查询附件
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryOrderAttachmentListByIdAsync(long orderId)
        {
            var orderAttachments = await _qualFqcOrderAttachmentRepository.GetByOrderIdAsync(orderId);
            if (orderAttachments == null) return Array.Empty<InteAttachmentBaseDto>();

            var attachmentEntities = await _inteAttachmentRepository.GetByIdsAsync(orderAttachments.Select(s => s.AttachmentId));
            if (attachmentEntities == null) return Array.Empty<InteAttachmentBaseDto>();

            return PrepareAttachmentBaseDtos(orderAttachments, attachmentEntities);
        }

        /// <summary>
        /// 查询检验单快照数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<FQCParameterDetailDto>> QueryDetailSnapshotAsync(FQCParameterDetailQueryDto query)
        {
            if (string.IsNullOrWhiteSpace(query.Barcode)) throw new CustomerValidationException(nameof(ErrorCode.MES11909));
            var site = _currentSite.SiteId ?? 0;
            //OrderSFC 查询
            var orderSFCEntity = await _qualFqcOrderSfcRepository.GetEntityAsync(new QualFqcOrderSfcQuery { BarCode = query.Barcode, FQCOrderId = query.FQCOrderId, SiteId = site });
            if (orderSFCEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11712)).WithData("SFC", query.Barcode);
            }
            //FQCOrder
            var entity = await _qualFqcOrderRepository.GetByIdAsync(query.FQCOrderId);
            if (entity == null) return Array.Empty<FQCParameterDetailDto>();
            //SnapShoot
            var snapshotEntity = await _qualFqcParameterGroupSnapshootRepository.GetByIdAsync(entity.GroupSnapshootId);
            if (snapshotEntity == null) return Array.Empty<FQCParameterDetailDto>();

            var snapshotDetailEntities = await _qualFqcParameterGroupDetailSnapshootRepository.GetEntitiesAsync(new QualFqcParameterGroupDetailSnapshootQuery
            {
                SiteId = entity.SiteId,
                ParameterGroupId = snapshotEntity.Id,
            });
            if (snapshotDetailEntities == null) return Array.Empty<FQCParameterDetailDto>();
            //SnapShootDetail
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
        /// 条码校验FQC，返回FQC参数项目
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualFqcParameterGroupEntity>> VerificationParametergroupAsync(ParameterGroupQuery query)
        {

            // 转换产品编码/版本变为产品ID
            if (!string.IsNullOrWhiteSpace(query.SFC))
            {
                // 查询检验单下面的所有样本
                var sampleEntities = await _qualFqcOrderSampleRepository.GetEntitiesAsync(new QualFqcOrderSampleQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Barcode = query.SFC
                });
                if (sampleEntities != null && sampleEntities.Any())
                {
                    var fqcOrderList = await _qualFqcOrderRepository.GetByIdsAsync(sampleEntities.Select(x => x.FQCOrderId).Distinct().ToArray());
                    if (fqcOrderList.Any(x => x.Status != InspectionStatusEnum.Closed))
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES11713)).WithData("sfc", query.SFC).WithData("fqcOrder", fqcOrderList.Where(x => x.Status != InspectionStatusEnum.Closed));
                    }
                }
            }

            //查FQC参数项目
            IEnumerable<QualFqcParameterGroupEntity> parm = null;
            return parm;

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
            IEnumerable<FQCParameterDetailDto>? dtos = null;
            if (pagedInfo.Data.Count() > 0)
            {
                // 实体到DTO转换 装载数据
                dtos = await PrepareSampleDetailDtosAsync(entity, pagedInfo.Data);
            }
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
                OperateType = operationType,
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

            //检验人： 检验单操作
            var orderOperationEntities = await _qualFqcOrderOperateRepository.GetEntitiesAsync(new QualFqcOrderOperateQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                FQCOrderIds = entities.Select(s => s.Id)
            });

            // 读取产品
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(entities.Where(w => w.MaterialId.HasValue).Select(x => x.MaterialId!.Value));
            var materialDic = materialEntities.ToDictionary(x => x.Id, x => x);

            // 处理人：查询不合格处理数据
            var unqualifiedHandEntities = await _qualFqcOrderUnqualifiedHandleRepository.GetEntitiesAsync(new QualFqcOrderUnqualifiedHandleQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                FQCOrderIds = entities.Select(s => s.Id)
            });

            //工单
            var workids = entities.Where(w => w.WorkOrderId.HasValue).Select(x => x.WorkOrderId!.Value).Distinct();
            var planworkEntitys = await _planWorkOrderRepository.GetByIdsAsync(workids);
            var planworkDic = planworkEntitys.ToDictionary(x => x.Id, x => x);

            //参数快照
            var parashoots = entities.Select(s => s.GroupSnapshootId).Distinct().ToArray();
            var snapshotEntitys = await _qualFqcParameterGroupSnapshootRepository.GetByIdsAsync(parashoots);
            var snapshotDic = snapshotEntitys.ToDictionary(x => x.Id, x => x);

            foreach (var entity in entities)
            {
                var dto = entity.ToModel<QualFqcOrderDto>();
                if (dto == null) continue;

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
                    materialDic.TryGetValue(entity.MaterialId.Value, out var materialEntity);
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

                //参数快照表
                snapshotDic.TryGetValue(entity.GroupSnapshootId, out var snaphot);
                if (snaphot != null)
                {
                    //允许混线，列表不显示工单
                    if (snaphot.IsSameWorkOrder == TrueOrFalseEnum.No)
                    {
                        dto.OrderCode = "-";
                    }
                    else
                    {
                        //否则取工单号
                        if (entity.WorkOrderId.HasValue)
                        {
                            planworkDic.TryGetValue(entity.WorkOrderId.Value, out var workorder);
                            if (workorder != null)
                                dto.OrderCode = workorder.OrderCode;
                        }
                    }
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        /// <summary>
        /// 根据ID查询类型
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<QualFqcParameterGroupSnapshootOut> QuerySnapshootByIdAsync(long orderId)
        {
            var rsp = new QualFqcParameterGroupSnapshootOut();
            //FQCOrder
            var entity = await _qualFqcOrderRepository.GetByIdAsync(orderId);
            if (entity == null) return rsp;
            //SnapShoot
            //var snapshotEntity = await _qualFqcParameterGroupSnapshootRepository.GetByIdAsync(entity.GroupSnapshootId);
            //if (snapshotEntity == null) return rsp;

            rsp = new QualFqcParameterGroupSnapshootOut
            {
                Code = entity.InspectionOrder,
                MaterialId = entity.MaterialId ?? 0,
                SampleQty = entity.SampleQty
            };
            return rsp;
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
                    AttachmentId = item.AttachmentId
                };

                var attachmentEntity = attachmentEntities.FirstOrDefault(f => f.Id == item.AttachmentId);
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
