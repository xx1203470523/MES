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
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
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

            //查询工具信息
            var toolsEntity = await _toolsRepository.GetByIdAsync(saveDto.ToolId);
            if (toolsEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17601));
            }
            //查询设备信息
            var equEquipmentEntity = await _equEquipmentRepository.GetByIdAsync(saveDto.EquipmentId);
            if (equEquipmentEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16338));
            }

            //备件能安装的数量
            var siteId = _currentSite.SiteId ?? 0;

            //指定位置是否已经绑定设备
            var recordEntities = await _equToolsEquipmentBindRecordRepository.GetEntitiesAsync(new EquToolsEquipmentBindRecordQuery
            {
                SiteId = siteId,
                ToolId = saveDto.ToolId,
                //EquipmentId = saveDto.EquipmentId,
                OperationType = BindOperationTypeEnum.Install
            });
            if (recordEntities != null && recordEntities.Any())
            {
                var position = recordEntities.FirstOrDefault()?.Position ?? "";
                throw new CustomerValidationException(nameof(ErrorCode.MES17602)).WithData("position", position);
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
                RatedLifeUnit=toolsEntity.RatedLifeUnit,
                CumulativeUsedLife=toolsEntity.CumulativeUsedLife,
                CurrentUsedLife=toolsEntity.CurrentUsedLife,
                LastVerificationTime= toolsEntity.LastVerificationTime,
                IsCalibrated=toolsEntity.IsCalibrated,
                CalibrationCycle=toolsEntity.CalibrationCycle,
                CalibrationCycleUnit=toolsEntity.CalibrationCycleUnit,
                Status=toolsEntity.Status,
                Remark=toolsEntity.Remark,
                EquipmentId=saveDto.EquipmentId,
                OperationType="2",
                OperationRemark="",
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn,
                SiteId = siteId
            };

            var equRecordEntity = GetEquRecord(equEquipmentEntity, updatedBy, updatedOn);

            //安装记录
            var bindRecordEntity = new EquToolsEquipmentBindRecordEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                ToolId = saveDto.ToolId,
                ToolsRecordId = 0,
                EquipmentId = saveDto.EquipmentId,
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

        private EquEquipmentRecordEntity GetEquRecord(EquEquipmentEntity equEquipmentEntity, string updatedBy, DateTime updatedOn)
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
                OperationType = EquipmentRecordOperationTypeEnum.SparepartsBind,
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

            return await _equToolsEquipmentBindRecordRepository.UpdateAsync(recordEntity);
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
