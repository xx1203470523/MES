/*
 *creator: Karl
 *
 *describe: 设备维修记录    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:56:10
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquEquipmentRecord;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.EquRepairOrder;
using Hymson.MES.Core.Domain.EquRepairOrderAttachment;
using Hymson.MES.Core.Domain.EquRepairOrderFault;
using Hymson.MES.Core.Domain.EquRepairResult;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.CoreServices.Bos.Manufacture.ManuGenerateBarcode;
using Hymson.MES.CoreServices.Services.Manufacture.ManuGenerateBarcode;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.EquEquipmentRecord;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.EquRepairOrder;
using Hymson.MES.Data.Repositories.EquRepairOrderAttachment;
using Hymson.MES.Data.Repositories.EquRepairOrderFault;
using Hymson.MES.Data.Repositories.EquRepairResult;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Services.Dtos.Equipment.EquMaintenance;
using Hymson.MES.Services.Dtos.EquRepairOrder;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Org.BouncyCastle.Pqc.Crypto.Frodo;
using System.Transactions;

namespace Hymson.MES.Services.Services.EquRepairOrder
{
    /// <summary>
    /// 设备维修记录 服务
    /// </summary>
    public class EquRepairOrderService : IEquRepairOrderService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备维修记录 仓储
        /// </summary>
        private readonly IEquRepairOrderRepository _equRepairOrderRepository;
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        private readonly IEquEquipmentRecordRepository _equipmentRecordRepository;
        private readonly IEquRepairOrderFaultRepository _equRepairOrderFaultRepository;
        private readonly IEquRepairResultRepository _equRepairResultRepository;
        private readonly IInteCodeRulesRepository _inteCodeRulesRepository;

        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        private readonly IEquFaultPhenomenonRepository _equFaultPhenomenonRepository;
        private readonly IEquRepairOrderAttachmentRepository _equRepairOrderAttachmentRepository;
        private readonly IInteAttachmentRepository _inteAttachmentRepository;
        private readonly AbstractValidator<EquRepairOrderCreateDto> _validationCreateRules;
        private readonly AbstractValidator<EquRepairOrderModifyDto> _validationModifyRules;

        public EquRepairOrderService(ICurrentUser currentUser, ICurrentSite currentSite, IEquRepairOrderRepository equRepairOrderRepository, AbstractValidator<EquRepairOrderCreateDto> validationCreateRules, AbstractValidator<EquRepairOrderModifyDto> validationModifyRules, IEquEquipmentRepository equEquipmentRepository, IEquEquipmentRecordRepository equipmentRecordRepository, IEquRepairOrderFaultRepository equRepairOrderFaultRepository, IEquRepairResultRepository equRepairResultRepository, IInteCodeRulesRepository inteCodeRulesRepository, IManuGenerateBarcodeService manuGenerateBarcodeService, IEquFaultPhenomenonRepository equFaultPhenomenonRepository, IEquRepairOrderAttachmentRepository equRepairOrderAttachmentRepository, IInteAttachmentRepository inteAttachmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equRepairOrderRepository = equRepairOrderRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _equEquipmentRepository = equEquipmentRepository;
            _equipmentRecordRepository = equipmentRecordRepository;
            _equRepairOrderFaultRepository = equRepairOrderFaultRepository;
            _equRepairResultRepository = equRepairResultRepository;
            _inteCodeRulesRepository = inteCodeRulesRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _equFaultPhenomenonRepository = equFaultPhenomenonRepository;
            _equRepairOrderAttachmentRepository = equRepairOrderAttachmentRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
        }
        #region
        /// <summary>
        /// 报修
        /// </summary>
        /// <param name="equReportRepairDto"></param>
        /// <returns></returns>
        public async Task ReportRepairAsync(EquReportRepairDto equReportRepairDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }
            var siteId = _currentSite.SiteId ?? 0;
            var equEquipment = await _equEquipmentRepository.GetByCodeAsync(new EntityByCodeQuery { Site = siteId, Code = equReportRepairDto.EquipmentCode })
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17950)).WithData("Code", equReportRepairDto.EquipmentCode);

            //var equRecord = GetEquRecord(equEquipment);

            EquRepairOrderEntity equRepairOrderEntity = new()
            {
                //TODO 这里编号要改的

                RepairOrder = await GenerateOrderCodeAsync(siteId, _currentUser.UserName), // 维修信息
                EquipmentId = equEquipment.Id, // 替换为实际的设备ID
                EquipmentRecordId = 0,// equRecord.Id, // 替换为实际的设备记录ID
                FaultTime = equReportRepairDto.FaultTime, // 设置为当前时间或实际的故障时间
                IsShutdown = equReportRepairDto.IsShutdown, // 是否停机
                Status = EquRepairOrderStatusEnum.PendingRepair, // 设置状态，比如待维修
                Remark = equReportRepairDto.Remark ?? "", // 备注信息

                Id = equReportRepairDto.Id > 0 ? equReportRepairDto.Id : IdGenProvider.Instance.CreateId(),
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedOn = HymsonClock.Now(),
                SiteId = _currentSite.SiteId ?? 0
            };

            List<EquRepairOrderFaultEntity> equRepairOrderFaultEntitys = new();

            foreach (var item in equReportRepairDto.FaultPhenomenonDto)
            {

                EquRepairOrderFaultEntity equRepairOrderFaultEntity = new()
                {
                    RepairOrderId = equRepairOrderEntity.Id,
                    FaultPhenomenonId = item.Id,
                    FaultPhenomenon = item.Code,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = _currentSite.SiteId ?? 0

                    //FaultReasonId = 789,
                    //FaultReason = "Reason description",
                    //Remark = "Additional remarks"
                };
                equRepairOrderFaultEntitys.Add(equRepairOrderFaultEntity);

            }

            using var trans = TransactionHelper.GetTransactionScope();

            //await _equipmentRecordRepository.InsertAsync(equRecord);
            await _equRepairOrderRepository.InsertAsync(equRepairOrderEntity);
            await _equRepairOrderFaultRepository.InsertsAsync(equRepairOrderFaultEntitys);

            trans.Complete();
        }

        /// <summary>
        /// 维修
        /// </summary>
        /// <param name="equMaintenanceDto"></param>
        /// <returns></returns> 
        public async Task MaintenanceAsync(EquMaintenanceDto equMaintenanceDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }
            if (equMaintenanceDto.RepairStartTime > equMaintenanceDto.RepairEndTime)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17957));
            }
            var siteId = _currentSite.SiteId ?? 0;

            var equRepairOrder = await _equRepairOrderRepository.GetByIdAsync(equMaintenanceDto.Id)
           ?? throw new CustomerValidationException(nameof(ErrorCode.MES17951));
            if (equRepairOrder.Status != EquRepairOrderStatusEnum.PendingRepair)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17953)).WithData("Code", equRepairOrder.RepairOrder);
            }

            var equEquipment = await _equEquipmentRepository.GetByIdAsync(equRepairOrder.EquipmentId)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES17950)).WithData("Code", "");

            #region 组装
            var equRecord = GetEquRecord(equEquipment);

            equRepairOrder.EquipmentRecordId = equRecord.Id;
            equRepairOrder.Status = EquRepairOrderStatusEnum.Repaired;
            equRepairOrder.UpdatedBy = _currentUser.UserName;
            equRepairOrder.UpdatedOn = HymsonClock.Now();

            List<UpdateFaultReasonsQuery> updateFaultReasonsQuerys = new();
            foreach (var item in equMaintenanceDto.FaultPhenomenonDto)
            {
                if (item.FaultReasonId == 0 || string.IsNullOrWhiteSpace(item.FaultReason))
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES17958));
                }
                UpdateFaultReasonsQuery updateFaultReasonsQuery = new()
                {
                    Id = item.Id,
                    FaultReasonId = item.FaultReasonId,
                    FaultReason = item.FaultReason,
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                };
                updateFaultReasonsQuerys.Add(updateFaultReasonsQuery);
            }
            EquRepairResultEntity equRepairResultEntity = new();
            var equRepairResult = await _equRepairResultRepository.GetByRepairOrderIdAsync(equRepairOrder.Id);
            if (equRepairResult != null)
            {
                equRepairResult.RepairOrderId = equRepairOrder.Id;
                equRepairResult.RepairStartTime = equMaintenanceDto.RepairStartTime;
                equRepairResult.RepairEndTime = equMaintenanceDto.RepairEndTime;
                equRepairResult.LongTermHandlingMeasures = equMaintenanceDto.LongTermHandlingMeasures;
                equRepairResult.TemporaryTermHandlingMeasures = equMaintenanceDto.TemporaryTermHandlingMeasures;
                equRepairResult.RepairPerson = _currentUser.UserName;
                equRepairResult.UpdatedBy = _currentUser.UserName;
                equRepairResult.UpdatedOn = HymsonClock.Now();
            }
            else
            {
                equRepairResultEntity = new()
                {
                    RepairOrderId = equRepairOrder.Id,
                    RepairStartTime = equMaintenanceDto.RepairStartTime,
                    RepairEndTime = equMaintenanceDto.RepairEndTime,
                    LongTermHandlingMeasures = equMaintenanceDto.LongTermHandlingMeasures,
                    TemporaryTermHandlingMeasures = equMaintenanceDto.TemporaryTermHandlingMeasures,
                    RepairPerson = _currentUser.UserName,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = siteId
                    // ConfirmResult = _currentUser.UserName,
                    //ConfirmOn = DateTime.Now,
                    //ConfirmBy = "Supervisor",
                    //Remark = "Additional remarks"
                };
            }


            #endregion


            using var trans = TransactionHelper.GetTransactionScope();

            await _equipmentRecordRepository.InsertAsync(equRecord);
            await _equRepairOrderRepository.UpdateAsync(equRepairOrder);
            await _equRepairOrderFaultRepository.UpdateFaultReasonsAsync(updateFaultReasonsQuerys);
            if (equRepairResult != null)
            {
                await _equRepairResultRepository.UpdateAsync(equRepairResult);
            }
            else
            {
                await _equRepairResultRepository.InsertAsync(equRepairResultEntity);
            }

            trans.Complete();
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="confirmDto"></param>
        /// <returns></returns> 
        public async Task ConfirmAsync(ConfirmDto confirmDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }
            var equRepairOrder = await _equRepairOrderRepository.GetByIdAsync(confirmDto.Id)
              ?? throw new CustomerValidationException(nameof(ErrorCode.MES17951));
            if (equRepairOrder.Status != EquRepairOrderStatusEnum.Repaired)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17954)).WithData("Code", equRepairOrder.RepairOrder);
            }
            var equRepairResult = await _equRepairResultRepository.GetByRepairOrderIdAsync(equRepairOrder.Id)
             ?? throw new CustomerValidationException(nameof(ErrorCode.MES17951));
            #region 组装

            equRepairOrder.Status = confirmDto.ConfirmResult == EquConfirmResultEnum.RepairCompleted ? EquRepairOrderStatusEnum.Confirmed : EquRepairOrderStatusEnum.PendingRepair;
            equRepairOrder.UpdatedBy = _currentUser.UserName;
            equRepairOrder.UpdatedOn = HymsonClock.Now();

            equRepairResult.ConfirmBy = _currentUser.UserName;
            equRepairResult.ConfirmOn = HymsonClock.Now();
            equRepairResult.ConfirmResult = confirmDto.ConfirmResult;

            #endregion

            using var trans = TransactionHelper.GetTransactionScope();

            await _equRepairOrderRepository.UpdateAsync(equRepairOrder);
            await _equRepairResultRepository.UpdateAsync(equRepairResult);

            trans.Complete();
        }


        /// <summary>
        /// 根据OrderId查询FROM数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquRepairOrderFromDto> QueryEquRepairOrderByIdAsync(long id)
        {
            var equRepairOrderEntity = await _equRepairOrderRepository.GetByIdAsync(id);
            if (equRepairOrderEntity != null)
            {
                var repairOrderDto = new EquRepairOrderFromDto
                {
                    Id = equRepairOrderEntity.Id,
                    RepairOrder = equRepairOrderEntity.RepairOrder,
                    Status = equRepairOrderEntity.Status,
                    CreatedBy = equRepairOrderEntity.CreatedBy,
                    FaultTime = equRepairOrderEntity.FaultTime,
                    CreatedOn = equRepairOrderEntity.CreatedOn,
                    IsShutdown = equRepairOrderEntity.IsShutdown,

                };

                var equEquipment = await _equEquipmentRepository.GetByIdAsync(equRepairOrderEntity.EquipmentId);
                if (equEquipment != null)
                {
                    repairOrderDto.EquipmentCode = equEquipment.EquipmentCode;
                    repairOrderDto.EquipmentName = equEquipment.EquipmentName;
                }
                var equRepairResult = await _equRepairResultRepository.GetByRepairOrderIdAsync(equRepairOrderEntity.Id);
                if (equRepairResult != null)
                {
                    repairOrderDto.RepairStartTime = equRepairResult.RepairStartTime;
                    repairOrderDto.RepairEndTime = equRepairResult.RepairEndTime;
                }

                return repairOrderDto;
            }
            return new EquRepairOrderFromDto();
        }

        /// <summary>
        /// 根据OrderId查询FROM数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquRepairOrderFromDetailDto> QueryEquRepairOrderDetailByIdAsync(long id)
        {
            var equRepairOrderEntity = await _equRepairOrderRepository.GetByIdAsync(id);
            if (equRepairOrderEntity != null)
            {
                var repairOrderDto = new EquRepairOrderFromDetailDto()
                {
                    Remark = equRepairOrderEntity.Remark,
                };
                var equRepairResult = await _equRepairResultRepository.GetByRepairOrderIdAsync(equRepairOrderEntity.Id);
                if (equRepairResult != null)
                {
                    repairOrderDto.LongTermHandlingMeasures = equRepairResult.LongTermHandlingMeasures;
                    repairOrderDto.TemporaryTermHandlingMeasures = equRepairResult.TemporaryTermHandlingMeasures;
                    repairOrderDto.RepairPerson = equRepairResult.RepairPerson;
                    repairOrderDto.ConfirmBy = equRepairResult.ConfirmBy;
                    repairOrderDto.ConfirmResult = equRepairResult.ConfirmResult;
                    repairOrderDto.ConfirmOn = equRepairResult.ConfirmOn;
                }

                return repairOrderDto;
            }
            return new EquRepairOrderFromDetailDto();
        }

        /// <summary>
        /// 根据OrderId查询故障详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquReportRepairFaultDto>> QueryEquRepairOrderFaultByOrderIdAsync(long id)
        {
            var equRepairOrderFault = await _equRepairOrderFaultRepository.GetByRepairOrderIdAsync(id);
            List<EquReportRepairFaultDto> equReportRepairFaultDtoList = new();
            if (equRepairOrderFault != null && equRepairOrderFault.Any())
            {
                var equFaultPhenomenons = await _equFaultPhenomenonRepository.GetByIdsAsync(equRepairOrderFault.Select(it => it.FaultPhenomenonId ?? 0).ToArray());
                foreach (var item in equRepairOrderFault)
                {
                    var equFaultPhenomenon = equFaultPhenomenons.Where(it => it.Id == item.FaultPhenomenonId).FirstOrDefault();
                    EquReportRepairFaultDto equReportRepairFaultDto = new()
                    {
                        Id = item.Id,
                        Code = item.FaultPhenomenon,
                        Name = equFaultPhenomenon?.Name ?? "",
                        FaultReasonId = item.FaultReasonId,
                        FaultReason = item.FaultReason
                    };
                    equReportRepairFaultDtoList.Add(equReportRepairFaultDto);
                }
            }
            return equReportRepairFaultDtoList;
        }





        /// <summary>
        /// 保存检验单附件
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SaveAttachmentAsync(EquRepairOrderSaveAttachmentDto requestDto)
        {
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            //单据 
            //var entity = await _equRepairOrderRepository.GetByIdAsync(requestDto.OrderId)
            //    ?? throw new CustomerValidationException(nameof(ErrorCode.MES17961));

            if (!requestDto.Attachments.Any()) return 0;


            List<InteAttachmentEntity> inteAttachmentEntities = new();
            List<EquRepairOrderAttachmentEntity> orderAttachmentEntities = new();
            foreach (var attachment in requestDto.Attachments)
            {
                var attachmentId = IdGenProvider.Instance.CreateId();
                inteAttachmentEntities.Add(new InteAttachmentEntity
                {
                    Id = attachmentId,
                    Name = attachment.Name,
                    Path = attachment.Path,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn,
                    SiteId = _currentSite.SiteId ?? 0,
                });

                orderAttachmentEntities.Add(new EquRepairOrderAttachmentEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    AttachmentType = requestDto.Type,
                    RepairOrderId = requestDto.OrderId,
                    AttachmentId = attachmentId,
                    Remark = "",
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });
            }

            var rows = 0;
            try
            {
                using var trans = TransactionHelper.GetTransactionScope();
                rows += await _inteAttachmentRepository.InsertRangeAsync(inteAttachmentEntities);
                rows += await _equRepairOrderAttachmentRepository.InsertsAsync(orderAttachmentEntities);
                trans.Complete();
            }
            catch (Exception ex) { }
            return rows;
        }

        /// <summary>
        /// 删除检验单附件
        /// </summary>
        /// <param name="orderAttachmentId"></param>
        /// <returns></returns>
        public async Task<int> DeleteAttachmentByIdAsync(long orderAttachmentId)
        {
            var attachmentEntity = await _equRepairOrderAttachmentRepository.GetByIdAsync(orderAttachmentId);
            if (attachmentEntity == null) return default;

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _inteAttachmentRepository.DeleteAsync(attachmentEntity.AttachmentId);
            rows += await _equRepairOrderAttachmentRepository.DeleteAsync(attachmentEntity.Id);
            trans.Complete();
            return rows;
        }


        /// <summary>
        /// 查询单据附件
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryOrderAttachmentListByIdAsync(EquRepairOrderAttachmentDto dto)
        {
            var orderAttachments = await _equRepairOrderAttachmentRepository.GetByOrderIdAsync(new EquRepairOrderAttachmentQuery
            {
                OrderId = dto.OrderId,
                AttachmentType = dto.Type
            });
            if (orderAttachments == null) return Array.Empty<InteAttachmentBaseDto>();

            var attachmentEntities = await _inteAttachmentRepository.GetByIdsAsync(orderAttachments.Select(s => s.AttachmentId));
            if (attachmentEntities == null) return Array.Empty<InteAttachmentBaseDto>();

            return PrepareAttachmentBaseDtos(orderAttachments, attachmentEntities);
        }
        /// <summary>
        /// 转换成附近DTO
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

        #region 帮助
        /// <summary>
        /// 设备记录
        /// </summary>
        /// <param name="equEquipmentEntity"></param>
        /// <returns></returns>
        private EquEquipmentRecordEntity GetEquRecord(EquEquipmentEntity equEquipmentEntity)
        {
            return new EquEquipmentRecordEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                EquipmentId = equEquipmentEntity.Id,
                EquipmentCode = equEquipmentEntity.EquipmentCode,
                EquipmentName = equEquipmentEntity.EquipmentName,
                EquipmentGroupId = equEquipmentEntity.EquipmentGroupId,
                EquipmentDesc = equEquipmentEntity.EquipmentDesc,
                WorkCenterFactoryId = equEquipmentEntity.WorkCenterFactoryId,
                WorkCenterShopId = equEquipmentEntity.WorkCenterShopId,
                WorkCenterLineId = equEquipmentEntity.WorkCenterLineId,
                Location = equEquipmentEntity.Location,
                EquipmentType = equEquipmentEntity.EquipmentType,
                UseDepartment = equEquipmentEntity.UseDepartment,
                UseStatus = equEquipmentEntity.UseStatus,
                EntryDate = equEquipmentEntity.EntryDate,
                QualTime = equEquipmentEntity.QualTime,
                ExpireDate = equEquipmentEntity.ExpireDate,
                Manufacturer = equEquipmentEntity.Manufacturer,
                Supplier = equEquipmentEntity.Supplier,
                Power = equEquipmentEntity.Power,
                EnergyLevel = equEquipmentEntity.EnergyLevel,
                OperationType = EquEquipmentRecordOperationTypeEnum.Repair,
                Ip = equEquipmentEntity.Ip,
                TakeTime = equEquipmentEntity.TakeTime,
                Remark = equEquipmentEntity.Remark,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                SiteId = _currentSite.SiteId ?? 0
            };
        }

        /// <summary>
        /// 点检单号生成
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="CustomerValidationException"></exception>
        private async Task<string> GenerateOrderCodeAsync(long siteId, string userName)
        {
            var codeRules = await _inteCodeRulesRepository.GetListAsync(new InteCodeRulesReQuery
            {
                SiteId = siteId,
                CodeType = Core.Enums.Integrated.CodeRuleCodeTypeEnum.EquRepairOrder
            });
            if (codeRules == null || !codeRules.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17956));
            }
            if (codeRules.Count() > 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17955));
            }

            var orderCodes = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeBo
            {
                SiteId = siteId,
                UserName = userName,
                CodeRuleId = codeRules.First().Id,
                Count = 1
            });

            return orderCodes.First();
        }
        #endregion


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="equRepairOrderCreateDto"></param>
        /// <returns></returns>
        public async Task CreateEquRepairOrderAsync(EquRepairOrderCreateDto equRepairOrderCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(equRepairOrderCreateDto);

            //DTO转换实体
            var equRepairOrderEntity = equRepairOrderCreateDto.ToEntity<EquRepairOrderEntity>();
            equRepairOrderEntity.Id = IdGenProvider.Instance.CreateId();
            equRepairOrderEntity.CreatedBy = _currentUser.UserName;
            equRepairOrderEntity.UpdatedBy = _currentUser.UserName;
            equRepairOrderEntity.CreatedOn = HymsonClock.Now();
            equRepairOrderEntity.UpdatedOn = HymsonClock.Now();
            equRepairOrderEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _equRepairOrderRepository.InsertAsync(equRepairOrderEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEquRepairOrderAsync(long id)
        {
            await _equRepairOrderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param> 
        /// <returns></returns>
        public async Task<int> DeletesEquRepairOrderAsync(EquRepairOrderDeletesDto param)
        {
            var equRepairOrders = await _equRepairOrderRepository.GetByIdsAsync(param.Ids.ToArray());
            if (equRepairOrders != null && equRepairOrders.Any())
            {
                var equRepairOrderStatuss = equRepairOrders.Where(it => it.Status != EquRepairOrderStatusEnum.PendingRepair);
                if (equRepairOrderStatuss != null && equRepairOrderStatuss.Any())
                {
                    var codes = string.Join(",", equRepairOrderStatuss.Select(it => it.RepairOrder));
                    throw new CustomerValidationException(nameof(ErrorCode.MES17959)).WithData("Code", codes);
                }
                var equRepairOrderOn = equRepairOrders.Where(it => it.CreatedOn != it.UpdatedOn);
                if (equRepairOrderOn != null && equRepairOrderOn.Any())
                {
                    var codes = string.Join(",", equRepairOrderOn.Select(it => it.RepairOrder));
                    throw new CustomerValidationException(nameof(ErrorCode.MES17960)).WithData("Code", codes);
                }
            }
            return await _equRepairOrderRepository.DeletesAsync(new DeleteCommand { Ids = param.Ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="equRepairOrderPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquRepairOrderDto>> GetPagedListAsync(EquRepairOrderPagedQueryDto equRepairOrderPagedQueryDto)
        {
            var equRepairOrderPagedQuery = equRepairOrderPagedQueryDto.ToQuery<EquRepairOrderPagedQuery>();
            equRepairOrderPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equRepairOrderRepository.GetPagedInfoAsync(equRepairOrderPagedQuery);

            //实体到DTO转换 装载数据
            List<EquRepairOrderDto> equRepairOrderDtos = PrepareEquRepairOrderDtos(pagedInfo);
            return new PagedInfo<EquRepairOrderDto>(equRepairOrderDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquRepairOrderDto> PrepareEquRepairOrderDtos(PagedInfo<EquRepairOrderPageView> pagedInfo)
        {
            var equRepairOrderDtos = new List<EquRepairOrderDto>();
            foreach (var equRepairOrderEntity in pagedInfo.Data)
            {
                var equRepairOrderDto = equRepairOrderEntity.ToModel<EquRepairOrderDto>();
                equRepairOrderDtos.Add(equRepairOrderDto);
            }

            return equRepairOrderDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="equRepairOrderDto"></param>
        /// <returns></returns>
        public async Task ModifyEquRepairOrderAsync(EquRepairOrderModifyDto equRepairOrderModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(equRepairOrderModifyDto);

            //DTO转换实体
            var equRepairOrderEntity = equRepairOrderModifyDto.ToEntity<EquRepairOrderEntity>();
            equRepairOrderEntity.UpdatedBy = _currentUser.UserName;
            equRepairOrderEntity.UpdatedOn = HymsonClock.Now();

            await _equRepairOrderRepository.UpdateAsync(equRepairOrderEntity);
        }
    }
}
