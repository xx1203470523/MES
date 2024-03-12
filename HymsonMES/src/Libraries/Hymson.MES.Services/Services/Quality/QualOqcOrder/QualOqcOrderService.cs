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
using Hymson.MES.CoreServices.Services.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.Query;
using Hymson.MES.Data.Repositories.WhShipment;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Quality
{
    /// <summary>
    /// 服务（OQC检验单） 
    /// </summary>
    public class QualOqcOrderService : IQualOqcOrderService
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
        private readonly AbstractValidator<QualOqcOrderSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（OQC检验单）
        /// </summary>
        private readonly IQualOqcOrderRepository _qualOqcOrderRepository;
        private readonly IQualOqcOrderTypeRepository _qualOqcOrderTypeRepository;
        private readonly IWhShipmentRepository _whShipmentRepository;
        private readonly IWhShipmentMaterialRepository _whShipmentMaterialRepository;
        private readonly IQualOqcParameterGroupSnapshootRepository _qualOqcParameterGroupSnapshootRepository;
        private readonly IQualOqcParameterGroupDetailSnapshootRepository _qualOqcParameterGroupDetailSnapshootRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IWhSupplierRepository _whSupplierRepository;
        private readonly IQualOqcOrderOperateRepository _qualOqcOrderOperateRepository;
        private readonly IQualOqcOrderUnqualifiedHandleRepository _qualOqcOrderUnqualifiedHandleRepository;
        private readonly IWhShipmentBarcodeRepository _whShipmentBarcodeRepository;
        private readonly IQualOqcParameterGroupRepository _qualOqcParameterGroupRepository;
        private readonly IQualOqcOrderSampleRepository _qualOqcOrderSampleRepository;
        private readonly IQualOqcOrderSampleDetailRepository _qualOqcOrderSampleDetailRepository;
        private readonly IInteAttachmentRepository _inteAttachmentRepository;
        private readonly IQualOqcOrderSampleDetailAnnexRepository _qualOqcOrderSampleDetailAnnexRepository;

        private readonly IOQCOrderCreateService _oqcOrderCreateService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="qualOqcOrderRepository"></param>
        /// <param name="qualOqcOrderTypeRepository"></param>
        /// <param name="whShipmentRepository"></param>
        /// <param name="whShipmentMaterialRepository"></param>
        /// <param name="qualOqcParameterGroupSnapshootRepository"></param>
        /// <param name="qualOqcParameterGroupDetailSnapshootRepository"></param>
        /// <param name="oqcOrderCreateService"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="whSupplierRepository"></param>
        /// <param name="qualOqcOrderOperateRepository"></param>
        /// <param name="qualOqcOrderUnqualifiedHandleRepository"></param>
        /// <param name="whShipmentBarcodeRepository"></param>
        /// <param name="qualOqcParameterGroupRepository"></param>
        /// <param name="qualOqcOrderSampleRepository"></param>
        /// <param name="qualOqcOrderSampleDetailRepository"></param>
        /// <param name="inteAttachmentRepository"></param>
        /// <param name="qualOqcOrderSampleDetailAnnexRepository"></param>
        public QualOqcOrderService(ICurrentUser currentUser,
            ICurrentSite currentSite,
            AbstractValidator<QualOqcOrderSaveDto> validationSaveRules,
            IQualOqcOrderRepository qualOqcOrderRepository,
            IQualOqcOrderTypeRepository qualOqcOrderTypeRepository,
            IWhShipmentRepository whShipmentRepository,
            IWhShipmentMaterialRepository whShipmentMaterialRepository,
            IQualOqcParameterGroupSnapshootRepository qualOqcParameterGroupSnapshootRepository,
            IQualOqcParameterGroupDetailSnapshootRepository qualOqcParameterGroupDetailSnapshootRepository,
            IOQCOrderCreateService oqcOrderCreateService,
            IProcMaterialRepository procMaterialRepository,
            IWhSupplierRepository whSupplierRepository,
            IQualOqcOrderOperateRepository qualOqcOrderOperateRepository,
            IQualOqcOrderUnqualifiedHandleRepository qualOqcOrderUnqualifiedHandleRepository,
            IWhShipmentBarcodeRepository whShipmentBarcodeRepository,
            IQualOqcParameterGroupRepository qualOqcParameterGroupRepository,
            IQualOqcOrderSampleRepository qualOqcOrderSampleRepository,
            IQualOqcOrderSampleDetailRepository qualOqcOrderSampleDetailRepository,
            IInteAttachmentRepository inteAttachmentRepository,
            IQualOqcOrderSampleDetailAnnexRepository qualOqcOrderSampleDetailAnnexRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _qualOqcOrderRepository = qualOqcOrderRepository;
            _qualOqcOrderTypeRepository = qualOqcOrderTypeRepository;
            _whShipmentRepository = whShipmentRepository;
            _whShipmentMaterialRepository = whShipmentMaterialRepository;
            _qualOqcParameterGroupSnapshootRepository = qualOqcParameterGroupSnapshootRepository;
            _qualOqcParameterGroupDetailSnapshootRepository = qualOqcParameterGroupDetailSnapshootRepository;
            _oqcOrderCreateService = oqcOrderCreateService;
            _procMaterialRepository = procMaterialRepository;
            _whSupplierRepository = whSupplierRepository;
            _qualOqcOrderOperateRepository = qualOqcOrderOperateRepository;
            _qualOqcOrderUnqualifiedHandleRepository = qualOqcOrderUnqualifiedHandleRepository;
            _whShipmentBarcodeRepository = whShipmentBarcodeRepository;
            _qualOqcParameterGroupRepository = qualOqcParameterGroupRepository;
            _qualOqcOrderSampleRepository = qualOqcOrderSampleRepository;
            _qualOqcOrderSampleDetailRepository = qualOqcOrderSampleDetailRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
            _qualOqcOrderSampleDetailAnnexRepository = qualOqcOrderSampleDetailAnnexRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(QualOqcOrderSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            //出货单明细
            var shipmentMaterialList = await _whShipmentMaterialRepository.GetByIdsAsync(saveDto.ShipmentDetailIds.ToArray());
            if (shipmentMaterialList == null || !shipmentMaterialList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }
            //校验是否属于同一出货单
            if (shipmentMaterialList.Select(x => x.ShipmentId).Distinct().Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11800));
            }
            //查询出货单
            var shipmentEntity = await _whShipmentRepository.GetByIdAsync(shipmentMaterialList.First().ShipmentId);
            if (shipmentEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11801));
            }

            var bo = new CoreServices.Bos.Quality.OQCOrderCreateBo
            {
                ShipmentEntity = shipmentEntity,
                ShipmentMaterialEntities = shipmentMaterialList,
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
            };

            return await _oqcOrderCreateService.CreateAsync(bo);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(QualOqcOrderSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<QualOqcOrderEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _qualOqcOrderRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _qualOqcOrderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _qualOqcOrderRepository.DeletesAsync(new DeleteCommand
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
        public async Task<QualOqcOrderDto?> QueryByIdAsync(long id)
        {
            var qualOqcOrderEntity = await _qualOqcOrderRepository.GetByIdAsync(id);
            if (qualOqcOrderEntity == null) return null;

            return qualOqcOrderEntity.ToModel<QualOqcOrderDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualOqcOrderDto>> GetPagedListAsync(QualOqcOrderPagedQueryDto pagedQueryDto)
        {
            var resultData=new PagedInfo<QualOqcOrderDto>(Enumerable.Empty<QualOqcOrderDto>(), 0, 0, 0);
            var pagedQuery = pagedQueryDto.ToQuery<QualOqcOrderPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;

            var pagedInfo = await _qualOqcOrderRepository.GetPagedListAsync(pagedQuery);
            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return resultData;
            }

            //获取出货单明细
            var shipmentMaterialIds = pagedInfo.Data.Select(a => a.ShipmentMaterialId).Distinct();
            var shipmentMaterialEntities = await _whShipmentMaterialRepository.GetByIdsAsync(shipmentMaterialIds);
            if (shipmentMaterialEntities == null|| !shipmentMaterialEntities.Any()) return resultData;

            //获取出货单
            var shipmentOrderIds = pagedInfo.Data.Select(a => a.ShipmentMaterialId).Distinct();
            var shipmentEntity = await _whShipmentRepository.GetByIdAsync(shipmentMaterialEntities.First().ShipmentId);
            if (shipmentMaterialEntities == null) return resultData;

            //获取物料
            var materialIds = pagedInfo.Data.Select(a => a.MaterialId).Distinct();
            var materialEntities=await _procMaterialRepository.GetByIdsAsync(materialIds);

            //获取供应商
            var supplierIds = pagedInfo.Data.Select(a => a.CustomerId).Distinct();
            var supplierEntities = await _whSupplierRepository.GetByIdsAsync(supplierIds);

            //获取OQC检验记录
            var oqcOrderIds = pagedInfo.Data.Select(a => a.Id);
            var oqcOrderOperateEntities = await _qualOqcOrderOperateRepository.GetEntitiesAsync(new QualOqcOrderOperateQuery { OQCOrderIds = oqcOrderIds, SiteId = _currentSite.SiteId ?? 0 });

            //获取OQC不合格处理结果
            var oqcOrderUnqualifiedHandleEntities = await _qualOqcOrderUnqualifiedHandleRepository.GetEntitiesAsync(new QualOqcOrderUnqualifiedHandleQuery {OQCOrderIds= oqcOrderIds, SiteId = _currentSite.SiteId ?? 0 });

            //TODO 型号规则暂不确定是那个字段

            var dtos = new List<QualOqcOrderDto>();
            foreach (var item in pagedInfo.Data) {
                var model = item.ToModel<QualOqcOrderDto>();

                //var shipmentEntity = shipmentEntities.FirstOrDefault(a => a.Id == item.ShipmentMaterialId);
                model.ShipmentNum = shipmentEntity.ShipmentNum;
                model.ShipmentOrderId = shipmentEntity.Id;

                var materialEntitiy = materialEntities.FirstOrDefault(a => a.Id == item.MaterialId);
                model.MaterialCode = materialEntitiy?.MaterialCode;
                model.MaterialName = materialEntitiy?.MaterialName;
                model.Version = materialEntitiy?.Version;
                model.Unit = materialEntitiy?.Unit;

                var supplierEntity = supplierEntities.FirstOrDefault(a => a.Id == item.CustomerId);
                model.SupplierCode= supplierEntity?.Code;
                model.SupplierName = supplierEntity?.Name;

                var oqcOrderOperateEntity = oqcOrderOperateEntities.FirstOrDefault(a => a.OQCOrderId == item.Id);
                model.OperateBy= oqcOrderOperateEntity?.OperateBy;
                model.OperateOn = oqcOrderOperateEntity?.OperateOn;

                var oqcOrderUnqualifiedHandleEntity = oqcOrderUnqualifiedHandleEntities.FirstOrDefault(a => a.OQCOrderId == item.Id);
                model.HandMethod= oqcOrderUnqualifiedHandleEntity?.HandMethod;
                model.ProcessedBy= oqcOrderUnqualifiedHandleEntity?.ProcessedBy;
                model.ProcessedOn= oqcOrderUnqualifiedHandleEntity?.ProcessedOn;
                model.UnqualifiedHandRemark = oqcOrderUnqualifiedHandleEntity?.Remark;

                dtos.Add(model);
            }

            // 实体到DTO转换 装载数据
            //var dtos = pagedInfo.Data.Select(s => s.ToModel<QualOqcOrderDto>());
            return new PagedInfo<QualOqcOrderDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }


        /// <summary>
        /// 获取OQC检验单检验类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualOqcOrderTypeOutDto>> GetOqcOrderTypeAsync(long id)
        {
            var result =new List<QualOqcOrderTypeOutDto>();
            var qualOqcOrderTypeEntities = await _qualOqcOrderTypeRepository.GetEntitiesAsync(new QualOqcOrderTypeQuery { OQCOrderId= id ,SiteId=_currentSite.SiteId??0});
            if (qualOqcOrderTypeEntities == null || !qualOqcOrderTypeEntities.Any()) 
            { 
                return result;
            }

            foreach (var item in qualOqcOrderTypeEntities)
            {
                var model = new QualOqcOrderTypeOutDto();
                model.Id = item.Id;
                model.InspectionType= item.InspectionType;
                model.SampleQty= item.SampleQty;
                model.CheckedQty= item.CheckedQty;

                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// 校验样品条码
        /// </summary>
        /// <param name="checkBarCodeQuqryDto"></param>
        /// <returns></returns>
        public async Task<CheckBarCodeOutDto> CheckBarCodeAsync(CheckBarCodeQuqryDto checkBarCodeQuqryDto)
        {
            if (checkBarCodeQuqryDto.BarCode == null|| checkBarCodeQuqryDto.ShipmentId==null|| checkBarCodeQuqryDto.InspectionOrderId==null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10111));
            }

            //获取出货单物料信息
            var whShipmentMaterialEntities = await _whShipmentMaterialRepository.GetEntitiesAsync(new WhShipmentMaterialQuery { ShipmentId = checkBarCodeQuqryDto.ShipmentId.GetValueOrDefault(), SiteId = _currentSite.SiteId ?? 0 });
            if (whShipmentMaterialEntities == null || !whShipmentMaterialEntities.Any()) {
                throw new CustomerValidationException(nameof(ErrorCode.MES17802));
            }
            var shipmentMaterialIds = whShipmentMaterialEntities.Select(a => a.Id);

            //获取出货单条码信息
            var whShipmentBarcodeEntities = await _whShipmentBarcodeRepository.GetEntitiesAsync(new WhShipmentBarcodeQuery {ShipmentDetailIds= shipmentMaterialIds,BarCode= checkBarCodeQuqryDto.BarCode,SiteId=_currentSite.SiteId??0 });
            if (whShipmentBarcodeEntities == null || !whShipmentBarcodeEntities.Any()) {
                throw new CustomerValidationException(nameof(ErrorCode.MES17803)).WithData("barcode", checkBarCodeQuqryDto.BarCode);
            }

            //获取OQC检验单
            var qualOqcOrderEntity = await _qualOqcOrderRepository.GetByIdAsync(checkBarCodeQuqryDto.InspectionOrderId.GetValueOrDefault());
            if (qualOqcOrderEntity == null) {
                throw new CustomerValidationException(nameof(ErrorCode.MES17804));
            }

            //获取OQC检验参数组快照
            var qualOqcParameterGroupEntity = await _qualOqcParameterGroupDetailSnapshootRepository.GetByIdAsync(qualOqcOrderEntity.GroupSnapshootId);
            if (qualOqcParameterGroupEntity == null) {
                throw new CustomerValidationException(nameof(ErrorCode.MES17805));
            }
            var result = qualOqcParameterGroupEntity.ToModel<CheckBarCodeOutDto>();
            result.BarCode= checkBarCodeQuqryDto.BarCode;
            return result;
        }

        /// <summary>
        /// 保存样品数据
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task SaveOrderAsync(QualOqcOrderExecSaveDto requestDto) 
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) 
            { 
                throw new CustomerValidationException(nameof(ErrorCode.MES10101)); 
            }

            // 获取OQC检验单
            var qualOqcOrderEntity = await _qualOqcOrderRepository.GetByIdAsync(requestDto.OQCOrderId);
            if (qualOqcOrderEntity == null) {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            // 获取OQC检验类型
            var oqcOrderTypeEntity = await _qualOqcOrderTypeRepository.GetByIdAsync(requestDto.OQCOrderTypeId);
            if (oqcOrderTypeEntity == null) {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            // 检查该类型是否已经录入
            var sampleQuery = requestDto.ToQuery<QualOqcOrderSampleQuery>();
            sampleQuery.SiteId = qualOqcOrderEntity.SiteId;
            var sampleEntities = await _qualOqcOrderSampleRepository.GetEntitiesAsync(sampleQuery);
            if (sampleEntities != null && sampleEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19905))
                    .WithData("Code", requestDto.Barcode)
                    .WithData("Type", oqcOrderTypeEntity.InspectionType.GetDescription());
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 样本
            var sampleId = IdGenProvider.Instance.CreateId();
            var sampleEntity = new QualOqcOrderSampleEntity
            {
                Id = sampleId,
                SiteId = qualOqcOrderEntity.SiteId,
                OQCOrderId = qualOqcOrderEntity.Id,
                OQCOrderTypeId = oqcOrderTypeEntity.Id,
                Barcode = requestDto.Barcode,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn
            };

            // 遍历样本参数
            //样本明细
            var sampleDetailEntities = new List<QualOqcOrderSampleDetailEntity>();
            //样本附件
            var attachmentEntities = new List<InteAttachmentEntity>();
            //样本明细附件
            var sampleDetailAttachmentEntities = new List<QualOqcOrderSampleDetailAnnexEntity>();
            foreach (var item in requestDto.Details)
            {
                var sampleDetailId = IdGenProvider.Instance.CreateId();
                sampleDetailEntities.Add(new QualOqcOrderSampleDetailEntity
                {
                    Id = sampleDetailId,
                    SiteId = qualOqcOrderEntity.SiteId,
                    OQCOrderId = qualOqcOrderEntity.Id,
                    OQCOrderSampleId = sampleId,
                    GroupDetailSnapshootId = item.Id,
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
                            SiteId = qualOqcOrderEntity.SiteId,
                        });

                        // 样本附件
                        sampleDetailAttachmentEntities.Add(new QualOqcOrderSampleDetailAnnexEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = qualOqcOrderEntity.SiteId,
                            OQCOrderId = requestDto.OQCOrderId,
                            AnnexId = attachmentId,
                            CreatedBy = updatedBy,
                            CreatedOn = updatedOn,
                            UpdatedBy = updatedBy,
                            UpdatedOn = updatedOn
                        });
                    }
                }
            }

            //保存
            using (var trans = TransactionHelper.GetTransactionScope()) {
                var insertOqcOrderSampleRes = await _qualOqcOrderSampleRepository.InsertAsync(sampleEntity);
                if (insertOqcOrderSampleRes == 0) {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17806));
                }
                var insertOqcOrderSampleDetailRes = await _qualOqcOrderSampleDetailRepository.InsertRangeAsync(sampleDetailEntities);
                if (insertOqcOrderSampleDetailRes == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17806));
                }
                if (attachmentEntities.Any())
                {
                    var inteAttachmentRes=await _inteAttachmentRepository.InsertRangeAsync(attachmentEntities);
                    if (inteAttachmentRes == 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES17806));
                    }
                    var oqcOrderSampleDetailRes= await _qualOqcOrderSampleDetailAnnexRepository.InsertRangeAsync(sampleDetailAttachmentEntities);
                    if (oqcOrderSampleDetailRes == 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES17806));
                    }
                }
                trans.Complete();
            } 
             
            
        }


        /// <summary>
        /// 完成检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task CompleteOrderAsync(QualIqcOrderCompleteDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 获取OQC检验单
            var oqcOrderEntity = await _qualOqcOrderRepository.GetByIdAsync(requestDto.IQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // 只有"检验中"的状态才允许点击"完成"
            if (oqcOrderEntity.Status != InspectionStatusEnum.Inspecting)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19909))
                    .WithData("Before", InspectionStatusEnum.Inspecting.GetDescription())
                    .WithData("After", InspectionStatusEnum.Completed.GetDescription());
            }

            // 检查每种类型是否已经录入足够
            var orderTypeEntities = await _qualOqcOrderTypeRepository.GetEntitiesAsync(new QualOqcOrderTypeQuery { OQCOrderId= oqcOrderEntity.Id,SiteId=_currentSite.SiteId??0});

            // 读取一个未录入完整的类型
            var orderTypeEntity = orderTypeEntities.FirstOrDefault(f => f.SampleQty > f.CheckedQty);
            if (orderTypeEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19908))
                    .WithData("Type", orderTypeEntity.InspectionType.GetDescription())
                    .WithData("CheckedQty", orderTypeEntity.CheckedQty)
                    .WithData("SampleQty", orderTypeEntity.SampleQty);
            }

            // 检查类型是否已经存在
            var operationType = OrderOperateTypeEnum.Complete;
            var orderOperationEntities = await _qualOqcOrderOperateRepository.GetEntitiesAsync(new QualOqcOrderOperateQuery
            {
                SiteId = oqcOrderEntity.SiteId,
                OQCOrderId = oqcOrderEntity.Id,
                OperationType = operationType
            });
            if (orderOperationEntities != null && orderOperationEntities.Any()) {
                throw new CustomerValidationException(nameof(ErrorCode.MES17807)).WithData("code", oqcOrderEntity.InspectionOrder);
            } 

            // TODO 判断是否不合格
            //entity.AcceptanceLevel

            //// 插入检验单状态操作记录
            //return await _qualIqcOrderOperateRepository.InsertAsync(new QualIqcOrderOperateEntity
            //{
            //    Id = IdGenProvider.Instance.CreateId(),
            //    SiteId = entity.SiteId,
            //    IQCOrderId = entity.Id,
            //    OperationType = operationType,
            //    OperateBy = updatedBy,
            //    OperateOn = updatedOn,
            //    CreatedBy = updatedBy,
            //    CreatedOn = updatedOn,
            //    UpdatedBy = updatedBy,
            //    UpdatedOn = updatedOn
            //});
        }
    }
}
