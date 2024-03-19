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
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Data.Repositories.Warehouse;
using Hymson.MES.Data.Repositories.Warehouse.Query;
using Hymson.MES.Data.Repositories.WhShipment;
using Hymson.MES.Data.Repositories.WhShipment.Query;
using Hymson.MES.Services.Dtos.Integrated;
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
        private readonly IQualOqcOrderAnnexRepository _qualOqcOrderAnnexRepository;
        private readonly IInteCustomRepository _inteCustomRepository;

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
        /// <param name="qualOqcOrderAnnexRepository"></param>
        /// <param name="inteCustomRepository"></param>
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
            IQualOqcOrderSampleDetailAnnexRepository qualOqcOrderSampleDetailAnnexRepository,
            IQualOqcOrderAnnexRepository qualOqcOrderAnnexRepository,
            IInteCustomRepository inteCustomRepository)
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
            _qualOqcOrderAnnexRepository = qualOqcOrderAnnexRepository;
            _inteCustomRepository = inteCustomRepository;
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
            //校验是否已生成过检验单
            var orderList = await _qualOqcOrderRepository.GetEntitiesAsync(new QualOqcOrderQuery
            {
                ShipmentMaterialIds = saveDto.ShipmentDetailIds
            });
            if (orderList != null && orderList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11809)).WithData("ShipmentNum", shipmentEntity.ShipmentNum).WithData("ShipmentMaterialIds", string.Join(',', orderList.Select(x => x.ShipmentMaterialId).Distinct()));
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
        /// 修改检验单状态（执行检验）
        /// </summary>
        /// <param name="updateStatusDto"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(UpdateStatusDto updateStatusDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // OQC检验单
            var oqcOrderEntity = await _qualOqcOrderRepository.GetByIdAsync(updateStatusDto.OQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            // 只有待检验和检验中可以执行检验
            if (oqcOrderEntity.Status == InspectionStatusEnum.WaitInspect || oqcOrderEntity.Status == InspectionStatusEnum.Inspecting)
            {
                // 更新时间
                var updatedBy = _currentUser.UserName;
                var updatedOn = HymsonClock.Now();

                var qualOqcOrderOperateEntity = new QualOqcOrderOperateEntity();
                qualOqcOrderOperateEntity.Id = IdGenProvider.Instance.CreateId();
                qualOqcOrderOperateEntity.SiteId = oqcOrderEntity.SiteId;
                qualOqcOrderOperateEntity.OQCOrderId = oqcOrderEntity.Id;
                qualOqcOrderOperateEntity.OperateType = OrderOperateTypeEnum.Start;
                qualOqcOrderOperateEntity.OperateBy = updatedBy;
                qualOqcOrderOperateEntity.OperateOn = updatedOn;
                qualOqcOrderOperateEntity.CreatedBy = updatedBy;
                qualOqcOrderOperateEntity.CreatedOn = updatedOn;
                qualOqcOrderOperateEntity.UpdatedBy = updatedBy;
                qualOqcOrderOperateEntity.UpdatedOn = updatedOn;

                using (var trans = TransactionHelper.GetTransactionScope())
                {
                    //修改检验单状态
                    var updateOQCOrderRes = await _qualOqcOrderRepository.UpdateStatusAsync(new QualOqcOrderEntity { Id = updateStatusDto.OQCOrderId, Status = InspectionStatusEnum.Inspecting, UpdatedBy = updatedBy, UpdatedOn = updatedOn });
                    if (updateOQCOrderRes == 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES17809));
                    }

                    //新增检验操作记录
                    var insertOperateRes = await _qualOqcOrderOperateRepository.InsertAsync(qualOqcOrderOperateEntity);
                    if (insertOperateRes == 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES17809));
                    }

                    trans.Complete();
                }
            }
            else {
                throw new CustomerValidationException(nameof(ErrorCode.MES17813));
            }
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
            var entity = await _qualOqcOrderRepository.GetByIdAsync(id);
            if (entity == null) return null;

            // 实体到DTO转换
            var dto = entity.ToModel<QualOqcOrderDto>();
            dto.StatusStr = entity.Status.GetDescription();

            // 读取出货单明细
            var whShipmentMaterialEntity = await _whShipmentMaterialRepository.GetByIdAsync(entity.ShipmentMaterialId);
            if (whShipmentMaterialEntity == null) return dto;

            //获取出货单
            var whShipmentEntity = await _whShipmentRepository.GetByIdAsync(whShipmentMaterialEntity.ShipmentId);
            if (whShipmentEntity == null) {
                return dto;
            }
            dto.ShipmentNum = whShipmentEntity.ShipmentNum;
            dto.ShipmentOrderId = whShipmentEntity.Id;

            // TODO 规格型号
            //dto.Specifications = "-";

            // 读取产品
            var materialEntity = await _procMaterialRepository.GetByIdAsync(entity.MaterialId);
            if (materialEntity != null)
            {
                dto.MaterialCode = materialEntity.MaterialCode;
                dto.MaterialName = materialEntity.MaterialName;
                dto.Version = materialEntity.Version ?? "";
                dto.Unit= materialEntity.Unit;
            }

            // 读取客户
            var supplierEntity = await _inteCustomRepository.GetByIdAsync(whShipmentEntity.CustomerId);
            if (supplierEntity != null)
            {
                dto.SupplierCode = supplierEntity.Code;
                dto.SupplierName = supplierEntity.Name;
                //TODO
                dto.SupplierBatch = "";
            }

            //获取检验单附件
            var qualOqcOrderAnnexEntities = await _qualOqcOrderAnnexRepository.GetEntitiesAsync(new QualOqcOrderAnnexQuery { OQCOrderId= id });
            if (qualOqcOrderAnnexEntities != null && qualOqcOrderAnnexEntities.Any()) {
                var inteAnnexIds = qualOqcOrderAnnexEntities.Select(a => a.AnnexId).Distinct();
                var inteAnnexEntities = await _inteAttachmentRepository.GetByIdsAsync(inteAnnexIds);
                dto.Attachments = inteAnnexEntities.Select((a) => {
                    return a.ToModel<InteAttachmentBaseDto>();
                });
            }

            return dto;
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
            pagedQuery.InspectionOrderLike = pagedQueryDto.InspectionOrder;

            #region 组装查询条件

            //物料信息
            var materialEntities = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery { MaterialName = pagedQueryDto.MaterialName, MaterialCode = pagedQueryDto.MaterialCode, Version = pagedQueryDto.Version, SiteId = _currentSite.SiteId ?? 0 });
            if (materialEntities == null || !materialEntities.Any())
            {
                return resultData;
            }
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.MaterialCode) || !string.IsNullOrWhiteSpace(pagedQueryDto.Version) || !string.IsNullOrWhiteSpace(pagedQueryDto.MaterialName))
            {
                pagedQuery.MaterialIds = materialEntities.Select(a => a.Id).Distinct();
            }

            //客户信息
            var supplierEntities = await _inteCustomRepository.GetInteCustomEntitiesAsync(new InteCustomQuery { Code = pagedQueryDto.SupplierCode??"",SiteId=_currentSite.SiteId??0 });
            if (supplierEntities == null || !supplierEntities.Any())
            {
                return resultData;
            }
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.SupplierCode))
            {
                pagedQuery.CustomerIds = supplierEntities.Select(a => a.Id).Distinct();
            }

            var whShipmentMaterialQuery = new WhShipmentMaterialQuery();
            whShipmentMaterialQuery.SiteId = _currentSite.SiteId ?? 0;
            var shipmentIds=new List<long>();

            //出货单信息
            if (!string.IsNullOrEmpty(pagedQueryDto.ShipmentNum))
            {
                var shipmentByShipmentNumEntity = await _whShipmentRepository.GetEntityAsync(new WhShipmentQuery { ShipmentNum = pagedQueryDto.ShipmentNum ?? "" });
                if (shipmentByShipmentNumEntity == null)
                {
                    return resultData;
                }
                whShipmentMaterialQuery.ShipmentId = shipmentByShipmentNumEntity.Id;
                shipmentIds.Add(shipmentByShipmentNumEntity.Id);
            }

            //获取出货单明细
            var shipmentMaterialEntities = await _whShipmentMaterialRepository.GetEntitiesAsync(whShipmentMaterialQuery);
            if (shipmentMaterialEntities == null || !shipmentMaterialEntities.Any())
            {
                return resultData;
            }
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.ShipmentNum))
            {
                pagedQuery.ShipmentMaterialIds = shipmentMaterialEntities.Select(a => a.Id).Distinct();
            }
            else {
                shipmentIds.AddRange(shipmentMaterialEntities.Select(a => a.ShipmentId).Distinct());
            }
            

            var shipmentEntities = await _whShipmentRepository.GetByIdsAsync(shipmentIds.Distinct());
            if (shipmentEntities == null || !shipmentEntities.Any()) {
                return resultData;
            }

            #endregion

            var pagedInfo = await _qualOqcOrderRepository.GetPagedListAsync(pagedQuery);
            if (pagedInfo.Data == null || !pagedInfo.Data.Any())
            {
                return resultData;
            }

            //获取OQC检验记录
            var oqcOrderIds = pagedInfo.Data.Select(a => a.Id);
            var oqcOrderOperateEntities = await _qualOqcOrderOperateRepository.GetEntitiesAsync(new QualOqcOrderOperateQuery { OQCOrderIds = oqcOrderIds, SiteId = _currentSite.SiteId ?? 0 });

            //获取OQC不合格处理结果
            var oqcOrderUnqualifiedHandleEntities = await _qualOqcOrderUnqualifiedHandleRepository.GetEntitiesAsync(new QualOqcOrderUnqualifiedHandleQuery {OQCOrderIds= oqcOrderIds, SiteId = _currentSite.SiteId ?? 0,HandMethod= pagedQueryDto.HandMethod });
            //if (oqcOrderUnqualifiedHandleEntities == null || !oqcOrderUnqualifiedHandleEntities.Any()) {
            //    return resultData;
            //}

            //TODO 型号规则暂不确定是那个字段

            var dtos = new List<QualOqcOrderDto>();
            foreach (var item in pagedInfo.Data) {
                var model = item.ToModel<QualOqcOrderDto>();

                var shipmentMaterialEntity = shipmentMaterialEntities.FirstOrDefault(a => a.Id == item.ShipmentMaterialId);
                if (shipmentMaterialEntity == null) {
                    return resultData;
                }
                var shipmentEntity = shipmentEntities.FirstOrDefault(a => a.Id == shipmentMaterialEntity.ShipmentId);
                if(shipmentEntity==null) { return resultData; }

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

                var operateType = OrderOperateTypeEnum.Start;
                switch (item.Status)
                {
                    case InspectionStatusEnum.Completed:
                        operateType = OrderOperateTypeEnum.Complete;
                        break;
                    case InspectionStatusEnum.Closed:
                        operateType = OrderOperateTypeEnum.Close;
                        break;
                    default:
                        break;
                }
                var oqcOrderOperateEntity = oqcOrderOperateEntities.FirstOrDefault(a => a.OQCOrderId == item.Id&&a.OperateType== operateType);
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
            var qualOqcOrderTypeEntities = await _qualOqcOrderTypeRepository.GetEntitiesAsync(new QualOqcOrderTypeQuery { OQCOrderId= id});
            if (qualOqcOrderTypeEntities == null || !qualOqcOrderTypeEntities.Any()) 
            { 
                return result;
            }

            foreach (var item in qualOqcOrderTypeEntities)
            {
                var model = new QualOqcOrderTypeOutDto();
                model.Id = item.Id;
                model.InspectionType= item.InspectionType;

                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// 校验样品条码
        /// </summary>
        /// <param name="checkBarCodeQuqryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CheckBarCodeOutDto>> CheckBarCodeAsync(CheckBarCodeQuqryDto checkBarCodeQuqryDto)
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
            //var qualOqcParameterGroupEntity = await _qualOqcParameterGroupSnapshootRepository.GetByIdAsync(qualOqcOrderEntity.GroupSnapshootId);
            //if (qualOqcParameterGroupEntity == null) {
            //    throw new CustomerValidationException(nameof(ErrorCode.MES17805));
            //}

            var qualOqcParameterDetailGroupEntities = await _qualOqcParameterGroupDetailSnapshootRepository.GetEntitiesAsync(new QualOqcParameterGroupDetailSnapshootQuery {ParameterGroupId= qualOqcOrderEntity.GroupSnapshootId, SiteId=_currentSite.SiteId??0 }); 
            if (qualOqcParameterDetailGroupEntities == null|| !qualOqcParameterDetailGroupEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17805));
            }
            var result = qualOqcParameterDetailGroupEntities.Select((a) =>
            {
                var model=a.ToModel<CheckBarCodeOutDto>();
                model.BarCode = checkBarCodeQuqryDto.BarCode;
                return model;
            });
            //var result = qualOqcParameterGroupEntity.ToModel<CheckBarCodeOutDto>();
            //result.BarCode= checkBarCodeQuqryDto.BarCode;
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
            var oqcOrderTypeEntity = await _qualOqcOrderTypeRepository.GetEntityAsync(new QualOqcOrderTypeQuery {OQCOrderId= requestDto.OQCOrderId,InspectionType= requestDto.InspectionType });
            if (oqcOrderTypeEntity == null) {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            // 检查该样品，类型是否已经录入
            var sampleQuery = requestDto.ToQuery<QualOqcOrderSampleQuery>();
            sampleQuery.SiteId = qualOqcOrderEntity.SiteId;
            sampleQuery.OQCOrderTypeId = oqcOrderTypeEntity.Id;
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
            
            //样本明细
            var sampleDetailEntities = new List<QualOqcOrderSampleDetailEntity>();
            //样本附件
            var attachmentEntities = new List<InteAttachmentEntity>();
            //样本明细附件
            var sampleDetailAttachmentEntities = new List<QualOqcOrderSampleDetailAnnexEntity>();

            var checkedQty = 0;
            foreach (var item in requestDto.Details)
            {
                checkedQty += 1;
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

                if (item.Attachment != null && item.Attachment.Any())
                {
                    foreach (var attachment in item.Attachment)
                    {
                        // 附件
                        var attachmentId = IdGenProvider.Instance.CreateId();
                        attachmentEntities.Add(new InteAttachmentEntity
                        {
                            Id = attachmentId,
                            Name = attachment.OriginalName,
                            Path = attachment.FileUrl,
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
                            SampleDetailId= sampleDetailId,
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
                //新增样品
                var insertOqcOrderSampleRes = await _qualOqcOrderSampleRepository.InsertAsync(sampleEntity);
                if (insertOqcOrderSampleRes == 0) {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17806));
                }

                //新增样品明细
                var insertOqcOrderSampleDetailRes = await _qualOqcOrderSampleDetailRepository.InsertRangeAsync(sampleDetailEntities);
                if (insertOqcOrderSampleDetailRes == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17806));
                }

                //新增样品附件
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

                //更新已检验数量
                var updateQtyRes = await _qualOqcOrderTypeRepository.UpdateCheckedQtyAsync(new QualOqcOrderTypeEntity {Id= oqcOrderTypeEntity.Id,UpdatedBy= updatedBy,UpdatedOn=updatedOn,CheckedQty=checkedQty });
                if (updateQtyRes == 0) {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17806));
                }

                trans.Complete();
            } 
             
            
        }

        /// <summary>
        /// 完成检验单
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task CompleteOrderAsync(QualOqcOrderCompleteDto requestDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 获取OQC检验单
            var oqcOrderEntity = await _qualOqcOrderRepository.GetByIdAsync(requestDto.OQCOrderId)
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
            var orderTypeEntities = await _qualOqcOrderTypeRepository.GetEntitiesAsync(new QualOqcOrderTypeQuery { OQCOrderId= oqcOrderEntity.Id});

            // 判断已检数是否小于应检数
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
            //查询OQC检验单操作记录
            var orderOperationEntities = await _qualOqcOrderOperateRepository.GetEntitiesAsync(new QualOqcOrderOperateQuery
            {
                SiteId = oqcOrderEntity.SiteId,
                OQCOrderId = oqcOrderEntity.Id,
                OperationTypes = new List<OrderOperateTypeEnum> { OrderOperateTypeEnum.Complete, OrderOperateTypeEnum.Close}
            });
            if (orderOperationEntities != null && orderOperationEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17807)).WithData("code", oqcOrderEntity.InspectionOrder);
            }

            // 读取所有明细参数
            var sampleDetailEntities = await _qualOqcOrderSampleDetailRepository.GetEntitiesAsync(new QualOqcOrderSampleDetailQuery
            {
                SiteId = oqcOrderEntity.SiteId,
                OQCOrderId = oqcOrderEntity.Id
            });
            
            // 如果不合格数超过接收水准，单据为不合格，状态为待检验
            if (sampleDetailEntities.Count(c => c.IsQualified == TrueOrFalseEnum.No) > oqcOrderEntity.AcceptanceLevel)
            {
                oqcOrderEntity.Status = InspectionStatusEnum.Completed;
                operationType = OrderOperateTypeEnum.Complete;
                oqcOrderEntity.IsQualified = TrueOrFalseEnum.No;
            }
            else
            {
                // 默认是关闭
                oqcOrderEntity.Status = InspectionStatusEnum.Closed;
                oqcOrderEntity.IsQualified = TrueOrFalseEnum.Yes;
                operationType = OrderOperateTypeEnum.Close;
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //修改单据状态
                var updatequalOqcOrderRes=await _qualOqcOrderRepository.UpdateStatusAndIsQualifiedAsync(oqcOrderEntity);
                if (updatequalOqcOrderRes == 0) {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17808));
                }

                // 插入检验单状态操作记录
                var insertRes = await _qualOqcOrderOperateRepository.InsertAsync(new QualOqcOrderOperateEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = oqcOrderEntity.SiteId,
                    OQCOrderId = oqcOrderEntity.Id,
                    OperateType = operationType,
                    OperateBy = updatedBy,
                    OperateOn = updatedOn,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });
                if (insertRes == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17808));
                }
                trans.Complete();
            }
        }

        #region 单据附件

        /// <summary>
        /// 保存检验单附件
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OQCAnnexOutDto>> SaveAttachmentAsync(QualOqcOrderSaveAttachmentDto requestDto)
        {
            var result=new List<OQCAnnexOutDto>();

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // OQC检验单
            var entity = await _qualOqcOrderRepository.GetByIdAsync(requestDto.OQCOrderId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            if (!requestDto.Attachments.Any()) {
                return result;
            };

            //附件文件信息
            var attachmentEntities = new List<InteAttachmentEntity>();
            //检验单附件信息
            var orderAnnexEntities = new List<QualOqcOrderAnnexEntity>();
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

                orderAnnexEntities.Add(new QualOqcOrderAnnexEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = entity.SiteId,
                    OQCOrderId = requestDto.OQCOrderId,
                    AnnexId = attachmentId,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });

                var model = new OQCAnnexOutDto();
                model.Id = attachmentId;
                model.Name = attachment.Name;
                model.Path = attachment.Path;
                result.Add(model);
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                var insertAttachmentRes = await _inteAttachmentRepository.InsertRangeAsync(attachmentEntities);
                if (insertAttachmentRes == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17810));
                }
                var insertOqcOrderAnnexRes = await _qualOqcOrderAnnexRepository.InsertRangeAsync(orderAnnexEntities);
                if (insertAttachmentRes == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17810));
                }
                trans.Complete();
            }

            return result;
        }

        /// <summary>
        /// 删除检验单附件
        /// </summary>
        /// <param name="orderAnnexId"></param>
        /// <returns></returns>
        public async Task DeleteAttachmentByIdAsync(long orderAnnexId)
        {
            //var attachmentEntity = await _qualOqcOrderAnnexRepository.GetByIdAsync(orderAnnexId);
            //if (attachmentEntity == null) {
            //    throw new CustomerValidationException(nameof(ErrorCode.MES17810));
            //} 
            
            using (var trans = TransactionHelper.GetTransactionScope()) { 
                var delAttachmentRes = await _inteAttachmentRepository.DeleteAsync(orderAnnexId);
                if (delAttachmentRes == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17810)); 
                }
                var delIqcOrderAnnexRes=await _qualOqcOrderAnnexRepository.DeleteAnnexAsync(orderAnnexId);
                if (delIqcOrderAnnexRes == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17810));
                }
                trans.Complete();
            } 
        }

        #endregion

        /// <summary>
        /// 查询检验单样本数据（分页）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<OqcOrderParameterDetailDto>> OqcOrderQueryDetailSamplePagedListAsync(OqcOrderParameterDetailPagedQueryDto pagedQueryDto)
       {
            // 初始化集合
            var defaultResult = new PagedInfo<OqcOrderParameterDetailDto>(Array.Empty<OqcOrderParameterDetailDto>(), pagedQueryDto.PageIndex, pagedQueryDto.PageSize, 0);

            //获取OQC检验单
            var oqcOrderEntity = await _qualOqcOrderRepository.GetByIdAsync(pagedQueryDto.OQCOrderId.GetValueOrDefault());
            if (oqcOrderEntity == null) return defaultResult;
            
            var pagedQuery = pagedQueryDto.ToQuery<QualOqcOrderSampleDetailPagedQuery>();
            pagedQuery.SiteId = oqcOrderEntity.SiteId;

            // 查询检验单下面的所有样本
            var sampleEntities = await _qualOqcOrderSampleRepository.GetEntitiesAsync(new QualOqcOrderSampleQuery
            {
                SiteId = oqcOrderEntity.SiteId,
                OQCOrderId = oqcOrderEntity.Id,
                Barcode = pagedQueryDto.BarCode
            });
            if (sampleEntities == null || !sampleEntities.Any()) { 
                return defaultResult;
            }
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.BarCode))
            {
                pagedQuery.OQCOrderSampleIds = sampleEntities.Select(s => s.Id);
            }

            // 根据参数编码获取快照明细ID
            var snapshotDetailEntities = await _qualOqcParameterGroupDetailSnapshootRepository.GetEntitiesAsync(new QualOqcParameterGroupDetailSnapshootQuery
            {
                SiteId = oqcOrderEntity.SiteId,
                ParameterCode = pagedQueryDto.ParameterCode
            });
            if (snapshotDetailEntities == null || !snapshotDetailEntities.Any()) {
                return defaultResult;
            }
            if (!string.IsNullOrWhiteSpace(pagedQueryDto.ParameterCode))
            {
                pagedQuery.GroupDetailSnapshootIds = snapshotDetailEntities.Select(s => s.Id);
            }

            // 查询数据
            var pagedInfo = await _qualOqcOrderSampleDetailRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = await PrepareSampleDetailDtos(oqcOrderEntity, pagedInfo.Data);
            return new PagedInfo<OqcOrderParameterDetailDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 修改样品检验数据
        /// </summary>
        /// <param name="updateSampleDetailDto"></param>
        /// <returns></returns>
        public async Task UpdateSampleDetailAsync(UpdateSampleDetailDto updateSampleDetailDto) 
        {
            //获取样品明细
            var qualOqcOrderSampleDetailEntity = await _qualOqcOrderSampleDetailRepository.GetByIdAsync(updateSampleDetailDto.Id.GetValueOrDefault());
            if (qualOqcOrderSampleDetailEntity == null) {
                throw new CustomerValidationException(nameof(ErrorCode.MES17811));
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            
            var model =  new QualOqcOrderSampleDetailEntity();
            model.Id= updateSampleDetailDto.Id.GetValueOrDefault();
            model.InspectionValue = updateSampleDetailDto.InspectionValue??"";
            model.Remark = updateSampleDetailDto.Remark??"";
            model.IsQualified = updateSampleDetailDto.IsQualified;
            model.UpdatedBy = updatedBy;
            model.UpdatedOn = updatedOn;

            //附件文件信息
            var attachmentEntities = new List<InteAttachmentEntity>();
            //检验单样品附件信息
            var orderSampleDetailAnnexEntities = new List<QualOqcOrderSampleDetailAnnexEntity>();
            if (updateSampleDetailDto.Attachment != null && !updateSampleDetailDto.Attachment.Any()) {
                foreach (var item in updateSampleDetailDto.Attachment)
                {
                    var attachmentId = IdGenProvider.Instance.CreateId();
                    attachmentEntities.Add(new InteAttachmentEntity
                    {
                        Id = attachmentId,
                        Name = item.Name,
                        Path = item.Path,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        SiteId = _currentSite.SiteId??0,
                    });

                    orderSampleDetailAnnexEntities.Add(new QualOqcOrderSampleDetailAnnexEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        OQCOrderId = qualOqcOrderSampleDetailEntity.OQCOrderId,
                        AnnexId = attachmentId,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn
                    });
                }
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //更新样品明细
                var updatequalOqcOrderSampleDetail = await _qualOqcOrderSampleDetailRepository.UpdateSampleDetailAsync(model);
                if (updatequalOqcOrderSampleDetail == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17811));
                }

                //新增附件
                if (attachmentEntities.Any())
                {
                    var insertAttachmentRes = await _inteAttachmentRepository.InsertRangeAsync(attachmentEntities);
                    if (insertAttachmentRes == 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES17811));
                    }
                }

                //新增样品附件
                if (orderSampleDetailAnnexEntities.Any())
                {
                    //删除样品附件
                    var delSampleDateilAnnexRes = await _qualOqcOrderSampleDetailAnnexRepository.DeleteAnnexBySampleDetailIdAsync(new DeleteCommand { Id = qualOqcOrderSampleDetailEntity.Id, UserId = updatedBy, DeleteOn = updatedOn });
                    if (delSampleDateilAnnexRes == 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES17811));
                    }

                    var insertSampleDateilAnnexRes = await _qualOqcOrderSampleDetailAnnexRepository.InsertRangeAsync(orderSampleDetailAnnexEntities);
                    if (insertSampleDateilAnnexRes == 0)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES17811));
                    }
                }

                trans.Complete();
            }
        }

        /// <summary>
        /// 不合格处理
        /// </summary>
        /// <param name="oQCOrderUnqualifiedHandleDto"></param>
        /// <returns></returns>
        public async Task UnqualifiedHandleAnync(OQCOrderUnqualifiedHandleDto oQCOrderUnqualifiedHandleDto)
        {
            if (oQCOrderUnqualifiedHandleDto.HandMethod == null || oQCOrderUnqualifiedHandleDto.OQCOrderId == null) {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            // OQC检验单
            var oqcOrderEntity = await _qualOqcOrderRepository.GetByIdAsync(oQCOrderUnqualifiedHandleDto.OQCOrderId.GetValueOrDefault())
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10104));

            //只有"已检验"的状态才允许"关闭"
            if (oqcOrderEntity.Status != InspectionStatusEnum.Completed)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11912))
                    .WithData("Before", InspectionStatusEnum.Completed.GetDescription())
                    .WithData("After", InspectionStatusEnum.Closed.GetDescription());
            }
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            var qualOqcOrderOperateEntity = new QualOqcOrderOperateEntity();
            qualOqcOrderOperateEntity.Id = IdGenProvider.Instance.CreateId();
            qualOqcOrderOperateEntity.SiteId = oqcOrderEntity.SiteId;
            qualOqcOrderOperateEntity.OQCOrderId = oqcOrderEntity.Id;
            qualOqcOrderOperateEntity.OperateType = OrderOperateTypeEnum.Close;
            qualOqcOrderOperateEntity.OperateBy = updatedBy;
            qualOqcOrderOperateEntity.OperateOn = updatedOn;
            qualOqcOrderOperateEntity.CreatedBy = updatedBy;
            qualOqcOrderOperateEntity.CreatedOn = updatedOn;
            qualOqcOrderOperateEntity.UpdatedBy = updatedBy;
            qualOqcOrderOperateEntity.UpdatedOn = updatedOn;

            var qualOqcOrderUnqualifiedHandleEntity=new QualOqcOrderUnqualifiedHandleEntity();
            qualOqcOrderUnqualifiedHandleEntity.Id= IdGenProvider.Instance.CreateId();
            qualOqcOrderUnqualifiedHandleEntity.HandMethod= oQCOrderUnqualifiedHandleDto.HandMethod;
            qualOqcOrderUnqualifiedHandleEntity.SiteId = oqcOrderEntity.SiteId;
            qualOqcOrderUnqualifiedHandleEntity.OQCOrderId = oqcOrderEntity.Id;
            qualOqcOrderUnqualifiedHandleEntity.SourceSystem = SourceSystemEnum.MES;
            qualOqcOrderUnqualifiedHandleEntity.ProcessedBy = updatedBy;
            qualOqcOrderUnqualifiedHandleEntity.ProcessedOn = updatedOn;
            qualOqcOrderUnqualifiedHandleEntity.CreatedBy = updatedBy;
            qualOqcOrderUnqualifiedHandleEntity.CreatedOn = updatedOn;
            qualOqcOrderUnqualifiedHandleEntity.CreatedBy = updatedBy;
            qualOqcOrderUnqualifiedHandleEntity.CreatedOn = updatedOn;
            qualOqcOrderUnqualifiedHandleEntity.Remark = oQCOrderUnqualifiedHandleDto.Remark??"";

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //修改检验单状态
                var updateOQCOrderRes = await _qualOqcOrderRepository.UpdateStatusAsync(new QualOqcOrderEntity { Id = oQCOrderUnqualifiedHandleDto.OQCOrderId.GetValueOrDefault(), Status = InspectionStatusEnum.Closed,UpdatedBy= _currentUser.UserName ,UpdatedOn= HymsonClock.Now() });
                if (updateOQCOrderRes == 0) {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17812));
                }

                //新增不合格处理
                var insertOqcOrderUnqualifiedHandleRes=await _qualOqcOrderUnqualifiedHandleRepository.InsertAsync(qualOqcOrderUnqualifiedHandleEntity);
                if (insertOqcOrderUnqualifiedHandleRes == 0) {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17812));
                }

                //新增检验操作记录
                var insertOperateRes =await _qualOqcOrderOperateRepository.InsertAsync(qualOqcOrderOperateEntity);
                if (insertOperateRes == 0) {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17812));
                }

                trans.Complete();
            }
        }

        /// <summary>
        /// 查询不合格样品数据（分页）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<OqcOrderParameterDetailDto>> OqcOrderQueryUnqualifiedPagedListAsync(OqcOrderParameterDetailPagedQueryDto pagedQueryDto)
        {
            // 初始化集合
            var defaultResult = new PagedInfo<OqcOrderParameterDetailDto>(Array.Empty<OqcOrderParameterDetailDto>(), pagedQueryDto.PageIndex, pagedQueryDto.PageSize, 0);

            var entity = await _qualOqcOrderRepository.GetByIdAsync(pagedQueryDto.OQCOrderId.GetValueOrDefault());
            if (entity == null) return defaultResult;

            // 查询检验单下面的所有样本
            var pagedQuery = pagedQueryDto.ToQuery<QualOqcOrderSampleDetailPagedQuery>();
            pagedQuery.SiteId = entity.SiteId;
            pagedQuery.IsQualified = TrueOrFalseEnum.No;

            // 查询数据
            var pagedInfo = await _qualOqcOrderSampleDetailRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = await PrepareSampleDetailDtos(entity, pagedInfo.Data);
            return new PagedInfo<OqcOrderParameterDetailDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        ///// <summary>
        ///// 根据ID查询类型
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <returns></returns>
        //public async Task<IEnumerable<QualOqcOrderTypeDto>> QueryOrderTypeListByIdAsync(long orderId)
        //{
        //    var entities = await _qualOqcOrderTypeRepository.GetByOQCOrderIdAsync(orderId);
        //    return entities.Select(s => s.ToModel<QualOqcOrderTypeDto>());
        //}

        /// <summary>
        /// 获取已检数据和样本数量
        /// </summary>
        /// <param name="sampleQtyAndCheckedQtyQueryDto"></param>
        /// <returns></returns>
        public async Task<SampleQtyAndCheckedQtyQueryOutDto> GetSampleQtyAndCheckedQtyAsync(SampleQtyAndCheckedQtyQueryDto sampleQtyAndCheckedQtyQueryDto)
        {
            var query=new QualOqcOrderTypeQuery();
            query.OQCOrderId = sampleQtyAndCheckedQtyQueryDto.OQCOrderId;
            query.InspectionType = sampleQtyAndCheckedQtyQueryDto.InspectionType;
            var entity = await _qualOqcOrderTypeRepository.GetEntityAsync(query);

            return entity.ToModel<SampleQtyAndCheckedQtyQueryOutDto>();
        }

        #region 私有方法

        /// <summary>
        /// 转换为Dto对象
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sampleDetailEntities">检验单样品明细</param>
        /// <returns></returns>
        private async Task<IEnumerable<OqcOrderParameterDetailDto>> PrepareSampleDetailDtos(QualOqcOrderEntity entity, IEnumerable<QualOqcOrderSampleDetailEntity> sampleDetailEntities)
        {
            // 查询样品明细对应的快照明细
            var snapshotDetailEntities = await _qualOqcParameterGroupDetailSnapshootRepository.GetByIdsAsync(sampleDetailEntities.Select(s => s.GroupDetailSnapshootId).ToArray());

            // 查询检验单下面的所有样本附件
            var sampleAttachmentEntities = await _qualOqcOrderSampleDetailAnnexRepository.GetEntitiesAsync(new QualOqcOrderSampleDetailAnnexQuery
            {
                SiteId = entity.SiteId,
                OQCOrderId = entity.Id
            });

            // 所有样品明细对应的样品集合
            var sampleEntities = await _qualOqcOrderSampleRepository.GetByIdsAsync(sampleDetailEntities.Select(s => s.OQCOrderSampleId).ToArray());

            // 附件集合
            Dictionary<long, IGrouping<long, QualOqcOrderSampleDetailAnnexEntity>> sampleAttachmentDic = new();
            IEnumerable<InteAttachmentEntity> attachmentEntities = Array.Empty<InteAttachmentEntity>();
            if (sampleAttachmentEntities.Any())
            {
                sampleAttachmentDic = sampleAttachmentEntities.ToLookup(w => w.SampleDetailId).ToDictionary(d => d.Key, d => d);
                attachmentEntities = await _inteAttachmentRepository.GetByIdsAsync(sampleAttachmentEntities.Select(s => s.AnnexId));
            }

            List<OqcOrderParameterDetailDto> dtos = new();
            foreach (var sampleDetailEntity in sampleDetailEntities)
            {
                // 快照数据
                var snapshotDetailEntity = snapshotDetailEntities.FirstOrDefault(f => f.Id == sampleDetailEntity.GroupDetailSnapshootId);
                if (snapshotDetailEntity == null) continue;

                var dto = snapshotDetailEntity.ToModel<OqcOrderParameterDetailDto>();
                dto.Id = sampleDetailEntity.Id;
                dto.Remark= sampleDetailEntity.Remark;
                dto.InspectionValue = sampleDetailEntity.InspectionValue;
                dto.IsQualified = sampleDetailEntity.IsQualified;

                // 填充条码
                var sampleEntity = sampleEntities.FirstOrDefault(f => f.Id == sampleDetailEntity.OQCOrderSampleId);
                if (sampleEntity == null) continue;
                dto.Barcode = sampleEntity.Barcode;

                // 填充附件
                if (attachmentEntities != null && sampleAttachmentDic.TryGetValue(sampleDetailEntity.Id, out var detailAttachmentEntities))
                {
                    dto.Attachment = PrepareAttachmentBaseDtos(detailAttachmentEntities, attachmentEntities);
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
