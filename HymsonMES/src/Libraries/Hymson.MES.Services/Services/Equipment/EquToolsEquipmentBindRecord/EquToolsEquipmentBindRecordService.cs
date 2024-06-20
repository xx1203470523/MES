using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquEquipmentRecord;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.EquSparepartRecord;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.EquEquipmentRecord;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.EquSparepartRecord;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Net.NetworkInformation;
using System.Transactions;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（工具绑定设备操作记录表） 
    /// </summary>
    public class EquToolsEquipmentBindRecordService : IEquToolsEquipmentBindRecordService
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
        /// 仓储接口（工具绑定设备操作记录表）
        /// </summary>
        private readonly IEquToolsEquipmentBindRecordRepository _equToolsEquipmentBindRecordRepository;

        private readonly IEquToolsRecordRepository _toolsRecordRepository;
        private readonly IEquToolsRepository _toolsRepository;
        private readonly IEquEquipmentRecordRepository _equipmentRecordRepository;
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquToolsEquipmentBindRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
            IEquToolsEquipmentBindRecordRepository equToolsEquipmentBindRecordRepository,
            IEquToolsRecordRepository toolsRecordRepository,
            IEquToolsRepository toolsRepository,
            IEquEquipmentRecordRepository equipmentRecordRepository,
            IEquEquipmentRepository equEquipmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equToolsEquipmentBindRecordRepository = equToolsEquipmentBindRecordRepository;
            _toolsRecordRepository = toolsRecordRepository;
            _toolsRepository = toolsRepository;
            _equipmentRecordRepository = equipmentRecordRepository;
            _equEquipmentRepository = equEquipmentRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> InstallAsync(EquToolsEquipmentBindRecordCreateDto saveDto)
        {
            #region 验证

            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            var siteId = _currentSite.SiteId ?? 0;
            //查询工具信息
            var toolsEntity = await _toolsRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = siteId,
                Code = saveDto.ToolCode.Trim()
            });
            if (toolsEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17701));
            }
            //工具未启用
            if (toolsEntity.Status != Core.Enums.DisableOrEnableEnum.Enable)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17702)).WithData("code", toolsEntity.Code);
            }
            if (toolsEntity.CurrentUsedLife<=0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17705));
            }

            //查询设备信息
            var equEquipmentEntity = await _equEquipmentRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = siteId,
                Code = saveDto.EquipmentCode.Trim()
            });
            if (equEquipmentEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16338));
            }
            //是否已经绑定设备
            var recordEntities = await _equToolsEquipmentBindRecordRepository.GetEntitiesAsync(new EquToolsEquipmentBindRecordQuery
            {
                SiteId = siteId,
                ToolId = toolsEntity.Id,
                EquipmentId = equEquipmentEntity.Id,
                OperationType = BindOperationTypeEnum.Install
            });
            if (recordEntities != null && recordEntities.Any())
            {
                var position = recordEntities.FirstOrDefault()?.Position ?? "";
                throw new CustomerValidationException(nameof(ErrorCode.MES17703)).WithData("code", toolsEntity.Code).WithData("position", position);
            }

            //校验工具所属工具类型是否允许在设备使用（工具类型在所有设备组均可使用或工具类型分配的设备组包含该设备所属的设备组），若不允许，则报错：工具XXX不允许在设备XXX使用；

            //校验工具所属工具类型是否允许对设备生产的产品使用（工具类型对所有产品均可使用或工具类型分配的产品料号包含该设备所在线体当前已激活工单的产品料号），若不允许，则报错：工具XXX不允许对产品XXX使用；

            //验证某个位置是否已经安装了其他工具
            var toolsEquipmentBindRecordEntity = await _equToolsEquipmentBindRecordRepository.GetIsPostionBindAsync(new EquToolsEquipmentBindRecordQuery
            {
                SiteId = siteId,
                Position = saveDto.Position.Trim(),
                EquipmentId = equEquipmentEntity.Id,
                OperationType = BindOperationTypeEnum.Install
            });
            if (toolsEquipmentBindRecordEntity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17704)).WithData("code", equEquipmentEntity.EquipmentCode).WithData("position", saveDto.Position);
            }

            #endregion

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            #region 组装数据
            var toolsRecordEntity = new EquToolsRecordEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                ToolId = toolsEntity.Id,
                Code = toolsEntity.Code,
                Name = toolsEntity.Name,
                ToolsId = toolsEntity.ToolsId,
                RatedLife = toolsEntity.RatedLife,
                RatedLifeUnit = toolsEntity.RatedLifeUnit,
                CumulativeUsedLife = toolsEntity.CumulativeUsedLife,
                CurrentUsedLife = toolsEntity.CurrentUsedLife,
                LastVerificationTime = toolsEntity.LastVerificationTime,
                IsCalibrated = toolsEntity.IsCalibrated,
                CalibrationCycle = toolsEntity.CalibrationCycle,
                CalibrationCycleUnit = toolsEntity.CalibrationCycleUnit,
                Status = toolsEntity.Status,
                Remark = toolsEntity.Remark,
                EquipmentId = equEquipmentEntity.Id,
                OperationType = ToolRecordOperationTypeEnum.Binding,
                OperationRemark = "",
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn,
                SiteId = siteId
            };

            var equRecordEntity = GetEquRecord(equEquipmentEntity, updatedBy, updatedOn, EquipmentRecordOperationTypeEnum.ToolBind);

            //安装记录
            var bindRecordEntity = new EquToolsEquipmentBindRecordEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                ToolId = toolsEntity.Id,
                ToolsRecordId = 0,
                EquipmentId = equEquipmentEntity.Id,
                EquipmentRecordId = equRecordEntity.Id,
                Position = saveDto.Position,
                OperationType = BindOperationTypeEnum.Install,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn,
                SiteId = siteId
            };
            #endregion

            var rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                // 保存
                await _toolsRecordRepository.InsertAsync(toolsRecordEntity);

                await _equipmentRecordRepository.InsertAsync(equRecordEntity);
                rows += await _equToolsEquipmentBindRecordRepository.InsertAsync(bindRecordEntity);
                ts.Complete();
            }
            return rows;
        }

        private EquEquipmentRecordEntity GetEquRecord(EquEquipmentEntity equEquipmentEntity, string updatedBy, DateTime updatedOn, EquipmentRecordOperationTypeEnum operationType)
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
                OperationType = EquEquipmentRecordOperationTypeEnum.SparePartsBinding,
                Ip = equEquipmentEntity.Ip,
                TakeTime = equEquipmentEntity.TakeTime,
                Remark = equEquipmentEntity.Remark,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn,
                SiteId = _currentSite.SiteId ?? 0
            };
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> UninstallAsync(EquToolsEquipmentBindRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            var recordEntity = await _equToolsEquipmentBindRecordRepository.GetByIdAsync(saveDto.Id);
            if (recordEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            if (saveDto.CurrentUsedLife <= 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17706));
            }

            #region 组装数据

            //查询设备信息
            var equEquipmentEntity = await _equEquipmentRepository.GetByIdAsync(recordEntity.EquipmentId);
            if (equEquipmentEntity == null)
            {
                equEquipmentEntity = new EquEquipmentEntity();
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            // DTO转换实体
            recordEntity.OperationType = BindOperationTypeEnum.Uninstall;
            recordEntity.UninstallReason = saveDto.UninstallReason;
            recordEntity.Remark = saveDto.Remark;
            recordEntity.UpdatedBy = updatedBy;
            recordEntity.UpdatedOn = updatedOn;
            recordEntity.UninstallBy = updatedBy;
            recordEntity.UninstallOn = updatedOn;
            recordEntity.CurrentUsedLife = saveDto.CurrentUsedLife;

            var siteId = _currentSite.SiteId ?? 0;
            //查询工具信息
            var toolsEntity = await _toolsRepository.GetByIdAsync(recordEntity.ToolId);
            var toolsRecordEntity = new EquToolsRecordEntity();
            if (toolsEntity != null)
            {
                if ((toolsEntity.CurrentUsedLife - saveDto.CurrentUsedLife) <= 0)
                {
                    toolsEntity.Status = DisableOrEnableEnum.Disable;
                }
                toolsEntity.UpdatedBy = updatedBy;
                toolsEntity.UpdatedOn = updatedOn;
                toolsEntity.CumulativeUsedLife = toolsEntity.CumulativeUsedLife + saveDto.CurrentUsedLife;
                toolsEntity.CurrentUsedLife = toolsEntity.CurrentUsedLife - saveDto.CurrentUsedLife;

                toolsRecordEntity = new EquToolsRecordEntity()
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    ToolId = toolsEntity.Id,
                    Code = toolsEntity.Code,
                    Name = toolsEntity.Name,
                    ToolsId = toolsEntity.ToolsId,
                    RatedLife = toolsEntity.RatedLife,
                    RatedLifeUnit = toolsEntity.RatedLifeUnit,
                    CumulativeUsedLife = toolsEntity.CumulativeUsedLife,
                    CurrentUsedLife = toolsEntity.CumulativeUsedLife,
                    LastVerificationTime = toolsEntity.LastVerificationTime,
                    IsCalibrated = toolsEntity.IsCalibrated,
                    CalibrationCycle = toolsEntity.CalibrationCycle,
                    CalibrationCycleUnit = toolsEntity.CalibrationCycleUnit,
                    Status = toolsEntity.Status,
                    Remark = toolsEntity.Remark,
                    EquipmentId = equEquipmentEntity.Id,
                    OperationType = ToolRecordOperationTypeEnum.UnBind,
                    OperationRemark = "",
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn,
                    SiteId = siteId
                };
            }

            var equRecordEntity = GetEquRecord(equEquipmentEntity, updatedBy, updatedOn, EquipmentRecordOperationTypeEnum.ToolUnbind);

            #endregion

            // 保存
            var rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                if (toolsEntity != null)
                {
                    await _toolsRepository.UpdateAsync(toolsEntity);
                    await _toolsRecordRepository.InsertAsync(toolsRecordEntity);
                }

                await _equipmentRecordRepository.InsertAsync(equRecordEntity);
                rows += await _equToolsEquipmentBindRecordRepository.UpdateAsync(recordEntity);
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equToolsEquipmentBindRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _equToolsEquipmentBindRecordRepository.DeletesAsync(new DeleteCommand
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
        public async Task<EquToolsEquipmentBindRecordDto?> QueryByIdAsync(long id)
        {
            var bindRecordEntity = await _equToolsEquipmentBindRecordRepository.GetByIdAsync(id);
            if (bindRecordEntity == null) return null;

            var bindRecordDto = bindRecordEntity.ToModel<EquToolsEquipmentBindRecordDto>();
            //查询备件信息
            var toolsEntity = await _toolsRepository.GetByIdAsync(bindRecordEntity.ToolId);

            //查询设备信息
            var equEquipmentEntity = await _equEquipmentRepository.GetByIdAsync(bindRecordEntity.EquipmentId);

            bindRecordDto.ToolCode = toolsEntity?.Code ?? "";
            bindRecordDto.ToolName = toolsEntity?.Name ?? "";
            bindRecordDto.EquipmentCode = equEquipmentEntity?.EquipmentCode ?? "";
            bindRecordDto.EquipmentName = equEquipmentEntity?.EquipmentName ?? "";
            bindRecordDto.RatedLife = toolsEntity?.RatedLife ?? 0;
            bindRecordDto.RemainingUsedLife = toolsEntity?.CurrentUsedLife ?? 0;
            return bindRecordDto;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquToolsEquipmentBindRecordViewDto>> GetPagedListAsync(EquToolsEquipmentBindRecordPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquToolsEquipmentBindRecordPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equToolsEquipmentBindRecordRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquToolsEquipmentBindRecordViewDto>());
            return new PagedInfo<EquToolsEquipmentBindRecordViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
