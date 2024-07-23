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
using Hymson.MES.CoreServices.Services.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.HttpClients;
using Hymson.MES.HttpClients.Requests.WMS;
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
        /// 仓储接口（生产退料单）
        /// </summary>
        private readonly IManuReturnOrderRepository _manuReturnOrderRepository;

        /// <summary>
        /// 仓储接口（生产退料单详情）
        /// </summary>
        private readonly IManuReturnOrderDetailRepository _manuReturnOrderDetailRepository;

        /// <summary>
        /// 仓储接口（物料维护）
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;

        /// <summary>
        /// 仓储接口（生产工单）
        /// </summary>
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;

        /// <summary>
        /// 仓储接口（附件维护）
        /// </summary>
        private readonly IInteAttachmentRepository _inteAttachmentRepository;

        /// <summary>
        /// 服务接口（检验单生成）
        /// </summary>
        private readonly IIQCOrderCreateService _iqcOrderCreateService;

        /// <summary>
        /// 服务接口（WMS）
        /// </summary>
        private readonly IWMSApiClient _wmsApiClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="qualIqcOrderReturnRepository"></param>
        /// <param name="qualIqcOrderReturnDetailRepository"></param>
        /// <param name="qualIqcOrderOperateRepository"></param>
        /// <param name="qualIqcOrderAnnexRepository"></param>
        /// <param name="manuReturnOrderRepository"></param>
        /// <param name="manuReturnOrderDetailRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="inteAttachmentRepository"></param>
        /// <param name="iqcOrderCreateService"></param>
        /// <param name="wmsApiClient"></param>
        public QualIqcOrderReturnService(ICurrentUser currentUser, ICurrentSite currentSite,
            IQualIqcOrderReturnRepository qualIqcOrderReturnRepository,
            IQualIqcOrderReturnDetailRepository qualIqcOrderReturnDetailRepository,
            IQualIqcOrderOperateRepository qualIqcOrderOperateRepository,
            IQualIqcOrderAnnexRepository qualIqcOrderAnnexRepository,
            IManuReturnOrderRepository manuReturnOrderRepository,
            IManuReturnOrderDetailRepository manuReturnOrderDetailRepository,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IInteAttachmentRepository inteAttachmentRepository,
            IIQCOrderCreateService iqcOrderCreateService,
            IWMSApiClient wmsApiClient)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _qualIqcOrderReturnRepository = qualIqcOrderReturnRepository;
            _qualIqcOrderReturnDetailRepository = qualIqcOrderReturnDetailRepository;
            _qualIqcOrderOperateRepository = qualIqcOrderOperateRepository;
            _qualIqcOrderAnnexRepository = qualIqcOrderAnnexRepository;
            _manuReturnOrderRepository = manuReturnOrderRepository;
            _manuReturnOrderDetailRepository = manuReturnOrderDetailRepository;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
            _iqcOrderCreateService = iqcOrderCreateService;
            _wmsApiClient = wmsApiClient;
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

            // 读取退料单
            var returnEntity = await _manuReturnOrderRepository.GetByIdAsync(requestDto.OrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES11901));

            // 校验是否已生成过检验单
            var orderEntities = await _qualIqcOrderReturnRepository.GetEntitiesAsync(new QualIqcOrderReturnQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ReturnOrderId = returnEntity.Id
            });
            if (orderEntities != null && orderEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11996)).WithData("Code", returnEntity.ReturnOrderCode);
            }

            // 当前信息
            var user = _currentUser.UserName;
            var time = HymsonClock.Now();

            // 读取退料单明细
            var returnDetailEntities = await _manuReturnOrderDetailRepository.GetEntitiesAsync(new ManuReturnOrderDetailQuery
            {
                SiteId = returnEntity.SiteId,
                ReturnOrderId = returnEntity.Id
            });

            // 生成检验单号
            var inspectionOrder = await _iqcOrderCreateService.GenerateCommonIQCOrderCodeAsync(new CoreServices.Bos.Common.BaseBo
            {
                SiteId = returnEntity.SiteId,
                User = user
            });

            /*
            // 根据工单Code读取工单信息
            var workOrderEntities = await _planWorkOrderRepository.GetEntitiesAsync(new PlanWorkOrderNewQuery
            {
                SiteId = returnEntity.SiteId,
                OrderCode = returnEntity.SourceWorkOrderCode
            });

            var workOrderEntity = workOrderEntities.FirstOrDefault()
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES13221));
            */

            // 检验单
            var orderEntity = new QualIqcOrderReturnEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = returnEntity.SiteId,
                InspectionOrder = inspectionOrder,
                ReturnOrderId = returnEntity.Id,
                WorkOrderId = returnEntity.SourceWorkOrderId,
                Status = IQCLiteStatusEnum.WaitInspect,
                IsQualified = null,
                CreatedBy = user,
                CreatedOn = time
            };

            /*
            // 根据物料Code读取物料信息
            var materialEntities = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery
            {
                SiteId = returnEntity.SiteId,
                MaterialCodes = returnDetailEntities.Select(s => s.MaterialCode)
            });
            */

            // 检验单明细
            List<QualIqcOrderReturnDetailEntity> orderDetailEntities = new();
            foreach (var item in returnDetailEntities)
            {
                //var materialEntity = materialEntities.FirstOrDefault(f => f.MaterialCode == item.MaterialCode);
                var orderDetailEntity = new QualIqcOrderReturnDetailEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = returnEntity.SiteId,
                    IQCOrderId = orderEntity.Id,
                    ReturnOrderDetailId = item.Id,
                    BarCode = item.MaterialBarCode,
                    MaterialId = item.MaterialId, //materialEntity?.Id ?? 0,
                    IsQualified = null,
                    CreatedBy = user,
                    CreatedOn = time
                };

                orderDetailEntities.Add(orderDetailEntity);
            }

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
            if (entity.Status != IQCLiteStatusEnum.WaitInspect) return default;
            switch (entity.Status)
            {
                case IQCLiteStatusEnum.WaitInspect:
                    // 继续接下来的操作
                    break;
                case IQCLiteStatusEnum.Completed:
                //case InspectionStatusEnum.Closed:
                //    throw new CustomerValidationException(nameof(ErrorCode.MES11914))
                //        .WithData("Status", $"{InspectionStatusEnum.Completed.GetDescription()}/{InspectionStatusEnum.Closed.GetDescription()}");
                case IQCLiteStatusEnum.Inspecting:
                default: return default;
            }

            // 更改状态
            switch (requestDto.OperationType)
            {
                case OrderOperateTypeEnum.Start:
                    entity.Status = IQCLiteStatusEnum.Inspecting;
                    break;
                case OrderOperateTypeEnum.Complete:
                    entity.Status = IQCLiteStatusEnum.Completed;
                    //entity.Status = entity.IsQualified == TrueOrFalseEnum.Yes ? InspectionStatusEnum.Closed : InspectionStatusEnum.Completed;
                    break;
                case OrderOperateTypeEnum.Close:
                    //entity.Status = IQCLiteStatusEnum.Closed;
                    //entity.Status = InspectionStatusEnum.Closed;
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
            orderEntity.Status = IQCLiteStatusEnum.Completed;
            orderEntity.UpdatedBy = user;
            orderEntity.UpdatedOn = time;

            // 回调WMS
            await IQCReturnCallBackAsync(orderEntity, updateDetailEntities);

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
            if (entities != null && entities.Any(a => a.Status != IQCLiteStatusEnum.WaitInspect))
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
            dto.CreatedOn = entity.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");
            dto.UpdatedOn = entity.UpdatedOn?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";

            // 读取退料单
            var returnEntity = await _manuReturnOrderRepository.GetByIdAsync(entity.ReturnOrderId);
            if (returnEntity == null) return dto;

            dto.ReqOrderCode = returnEntity.ReturnOrderCode;
            dto.ReturnUser = returnEntity.CreatedBy;
            dto.ReturnTime = returnEntity.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss");

            // 读取工单
            if (entity.WorkOrderId.HasValue)
            {
                var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(entity.WorkOrderId.Value);
                if (workOrderEntity != null)
                {
                    dto.WorkOrderCode = workOrderEntity.OrderCode;
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

            // 退料单明细
            var returnDetailEntities = await _manuReturnOrderDetailRepository.GetEntitiesAsync(new ManuReturnOrderDetailQuery
            {
                SiteId = orderEntity.SiteId,
                ReturnOrderId = orderEntity.ReturnOrderId
            });

            // 读取产品
            var materialEntities = await _procMaterialRepository.GetByIdsAsync(detailEntities.Where(w => w.MaterialId.HasValue).Select(x => x.MaterialId!.Value));
            var materialDic = materialEntities.ToDictionary(x => x.Id, x => x);

            // 遍历
            foreach (var entity in detailEntities)
            {
                var dto = entity.ToModel<QualIqcOrderReturnDetailDto>();

                // 退料单明细
                var materialReturnDetailEntity = returnDetailEntities.FirstOrDefault(f => f.Id == entity.ReturnOrderDetailId);
                if (materialReturnDetailEntity != null)
                {
                    dto.MaterialBarCode = materialReturnDetailEntity.MaterialBarCode;
                    dto.Qty = materialReturnDetailEntity.Qty;
                }

                // 产品
                if (entity.MaterialId.HasValue && materialDic.ContainsKey(entity.MaterialId.Value))
                {
                    var materialEntity = materialDic[entity.MaterialId.Value];
                    if (materialEntity != null)
                    {
                        //dto.IsFree = TrueOrFalseEnum.Yes;   // TODO
                        dto.MaterialCode = materialEntity.MaterialCode;
                        dto.MaterialName = materialEntity.MaterialName;
                        dto.MaterialVersion = materialEntity.Version ?? "";
                        //dto.Unit = materialEntity.Unit ?? "";
                    }
                }
                else
                {
                    dto.MaterialCode = "-";
                    dto.MaterialName = "-";
                    dto.MaterialVersion = "-";
                }

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

            /*
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
            */

            // 将退料单号变为退料单ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.ReqOrderCode))
            {
                var returnEntities = await _manuReturnOrderRepository.GetEntitiesAsync(new ManuReturnOrderQuery
                {
                    SiteId = pagedQuery.SiteId,
                    ReqOrderCode = pagedQueryDto.ReqOrderCode
                });
                if (returnEntities != null && returnEntities.Any()) pagedQuery.ReturnOrderIds = returnEntities.Select(s => s.Id);
                else pagedQuery.ReturnOrderIds = Array.Empty<long>();
            }

            // 转换工单编码变为工单ID
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.WorkOrderCode))
            {
                var workOrderEntities = await _planWorkOrderRepository.GetEntitiesAsync(new PlanWorkOrderNewQuery
                {
                    SiteId = pagedQuery.SiteId,
                    OrderCode = pagedQueryDto.WorkOrderCode
                });
                if (workOrderEntities != null && workOrderEntities.Any()) pagedQuery.WorkOrderIds = workOrderEntities.Select(s => s.Id);
                else pagedQuery.WorkOrderIds = Array.Empty<long>();
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
        /// <returns></returns>
        private async Task<int> CommonOperationAsync(QualIqcOrderReturnEntity entity, OrderOperateTypeEnum operationType)
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

            // 读取退料单
            var returnEntities = await _manuReturnOrderRepository.GetByIdsAsync(entities.Select(x => x.ReturnOrderId));
            var returnDic = returnEntities.ToDictionary(x => x.Id, x => x);

            // 检验单操作
            var orderOperationEntities = await _qualIqcOrderOperateRepository.GetEntitiesAsync(new QualIqcOrderOperateQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                IQCOrderIds = entities.Select(s => s.Id)
            });

            // 读取生产工单
            var workOrderEntities = await _planWorkOrderRepository.GetByIdsAsync(entities.Where(w => w.WorkOrderId.HasValue).Select(x => x.WorkOrderId!.Value));
            var workOrderDic = workOrderEntities.ToDictionary(x => x.Id, x => x);

            foreach (var entity in entities)
            {
                var dto = entity.ToModel<QualIqcOrderReturnDto>();
                if (dto == null) continue;

                // 退料单
                var receiptEntity = returnDic[entity.ReturnOrderId];
                if (receiptEntity != null)
                {
                    dto.ReqOrderCode = receiptEntity.ReturnOrderCode;
                }

                // 检验人
                var inspectionEntity = orderOperationEntities.FirstOrDefault(f => f.OperationType == OrderOperateTypeEnum.Start);
                if (inspectionEntity != null)
                {
                    dto.InspectionBy = inspectionEntity.CreatedBy;
                    dto.InspectionOn = inspectionEntity.CreatedOn;
                }

                // 生产工单
                if (entity.WorkOrderId.HasValue)
                {
                    var workOrderEntity = workOrderDic[entity.WorkOrderId.Value];
                    if (workOrderEntity != null)
                    {
                        dto.WorkOrderCode = workOrderEntity.OrderCode;
                    }
                }
                else
                {
                    dto.WorkOrderCode = "-";
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

        /// <summary>
        /// IQC回调（退料）
        /// </summary>
        /// <param name="orderEntity"></param>
        /// <param name="updateDetailEntities"></param>
        /// <returns></returns>
        private async Task IQCReturnCallBackAsync(QualIqcOrderReturnEntity orderEntity, List<QualIqcOrderReturnDetailEntity> updateDetailEntities)
        {
            // 读取退料单
            var returnEntity = await _manuReturnOrderRepository.GetByIdAsync(orderEntity.ReturnOrderId);

            // 读取退料单明细
            var returnDetailEntities = await _manuReturnOrderDetailRepository.GetEntitiesAsync(new ManuReturnOrderDetailQuery
            {
                SiteId = orderEntity.SiteId,
                ReturnOrderId = orderEntity.ReturnOrderId
            });

            if (returnEntity == null) return;
            if (!orderEntity.WorkOrderId.HasValue) return;

            // 读取工单
            var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(orderEntity.WorkOrderId.Value);

            List<IQCReturnMaterialResultDto> details = new();
            foreach (var item in updateDetailEntities)
            {
                var returnDetailEntity = returnDetailEntities.FirstOrDefault(f => f.Id == item.ReturnOrderDetailId);
                if (returnDetailEntity == null) continue;

                details.Add(new IQCReturnMaterialResultDto
                {
                    MaterialCode = returnDetailEntity.MaterialBarCode,
                    PlanQty = returnDetailEntity.Qty,
                    Qty = returnDetailEntity.Qty,
                    IsQualified = item.IsQualified ?? TrueOrFalseEnum.No
                });
            }

            // 将结果推送给WMS
            await _wmsApiClient.IQCReturnCallBackAsync(new IQCReturnResultDto
            {
                ReturnOrderCode = returnEntity.ReturnOrderCode,
                WorkOrderCode = workOrderEntity?.OrderCode ?? "",
                Details = details
            });
        }
        #endregion

    }
}
