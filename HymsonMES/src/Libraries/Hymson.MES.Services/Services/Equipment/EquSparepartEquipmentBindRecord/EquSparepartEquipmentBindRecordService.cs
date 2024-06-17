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
using Hymson.MES.Data.Repositories.EquEquipmentRecord;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Data.Repositories.EquSparepartRecord;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Equipment
{
    /// <summary>
    /// 服务（工具绑定设备操作记录表） 
    /// </summary>
    public class EquSparepartEquipmentBindRecordService : IEquSparepartEquipmentBindRecordService
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
        private readonly IEquSparepartEquipmentBindRecordRepository _equSparepartEquipmentBindRecordRepository;

        private readonly IEquSparepartRecordRepository _sparepartRecordRepository;
        private readonly IEquEquipmentRecordRepository _equipmentRecordRepository;
        private readonly IEquSparePartsRepository _sparePartsRepository;
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public EquSparepartEquipmentBindRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
            IEquSparepartEquipmentBindRecordRepository equSparepartEquipmentBindRecordRepository,
            IEquSparepartRecordRepository sparepartRecordRepository,
            IEquEquipmentRecordRepository equipmentRecordRepository,
            IEquSparePartsRepository sparePartsRepository,
            IEquEquipmentRepository equEquipmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equSparepartEquipmentBindRecordRepository = equSparepartEquipmentBindRecordRepository;
            _sparepartRecordRepository = sparepartRecordRepository;
            _equipmentRecordRepository = equipmentRecordRepository;
            _sparePartsRepository = sparePartsRepository;
            _equEquipmentRepository = equEquipmentRepository;
        }

        /// <summary>
        /// 备件安装
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> InstallAsync(EquSparepartEquipmentBindRecordCreateDto saveDto)
        {
            #region 验证

            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            //查询备件信息
            var sparePartsEntity = await _sparePartsRepository.GetByIdAsync(saveDto.SparepartId);
            if (sparePartsEntity == null)
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
            var recordEntities = await _equSparepartEquipmentBindRecordRepository.GetEntitiesAsync(new EquSparepartEquipmentBindRecordQuery
            {
                SiteId = siteId,
                SparepartId = saveDto.SparepartId,
                //EquipmentId = saveDto.EquipmentId,
                OperationType = BindOperationTypeEnum.Install
            });
            if (recordEntities != null && recordEntities.Any())
            {
                var position = recordEntities.FirstOrDefault()?.Position??"";
                throw new CustomerValidationException(nameof(ErrorCode.MES17602)).WithData("position", position);

                //if (recordEntities.Any(x => x.EquipmentId == saveDto.EquipmentId && x.Position.ToLowerInvariant() == saveDto.Position.ToLowerInvariant()))
                //{
                //    throw new CustomerValidationException(nameof(ErrorCode.MES17602)).WithData("position", saveDto.Position);
                //}
            }
            #endregion

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            #region 组装数据
            var spareRecordEntity = new EquSparepartRecordEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SparepartId = sparePartsEntity.Id,
                Code=sparePartsEntity.Code,
                Name=sparePartsEntity.Name,
                SparePartTypeId=sparePartsEntity.SparePartTypeId,
                ProcMaterialId=0,
                Type= EquipmentPartTypeEnum.SparePart,
                UnitId=0,
                IsKey= sparePartsEntity.IsCritical??YesOrNoEnum.No,
                IsStandard= sparePartsEntity.IsStandard,
                Status =sparePartsEntity.Status,
                BluePrintNo=sparePartsEntity.DrawCode??"",
                Brand="",
                Qty=sparePartsEntity.Qty,
                Position = saveDto.Position,
                CreatedBy = updatedBy,
                CreatedOn = updatedOn,
                UpdatedBy = updatedBy,
                UpdatedOn = updatedOn,
                SiteId = siteId
            };

            var equRecordEntity = GetEquRecord(equEquipmentEntity, updatedBy, updatedOn);

            //安装记录
            var bindRecordEntity = new EquSparepartEquipmentBindRecordEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                SparepartId = saveDto.SparepartId,
                SparepartRecordId = spareRecordEntity.Id,
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
                await _sparepartRecordRepository.InsertAsync(spareRecordEntity);

                await _equipmentRecordRepository.InsertAsync(equRecordEntity);
                rows += await _equSparepartEquipmentBindRecordRepository.InsertAsync(bindRecordEntity);
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
                OperationType= 5,
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
        /// 卸载
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> UninstallAsync(EquSparepartEquipmentBindRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            var recordEntity=await _equSparepartEquipmentBindRecordRepository.GetByIdAsync(saveDto.Id);
            if (recordEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10104));
            }

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            // DTO转换实体
            recordEntity.OperationType=BindOperationTypeEnum.Uninstall;
            recordEntity.UninstallReason = saveDto.UninstallReason;
            recordEntity.Remark = saveDto.Remark;
            recordEntity.UpdatedBy = updatedBy;
            recordEntity.UpdatedOn = updatedOn;
            recordEntity.UninstallBy = updatedBy;
            recordEntity.UninstallOn = updatedOn;

            return await _equSparepartEquipmentBindRecordRepository.UpdateAsync(recordEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equSparepartEquipmentBindRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _equSparepartEquipmentBindRecordRepository.DeletesAsync(new DeleteCommand
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
        public async Task<EquSparepartEquipmentBindRecordDto?> QueryByIdAsync(long id)
        {
            var bindRecordEntity = await _equSparepartEquipmentBindRecordRepository.GetByIdAsync(id);
            if (bindRecordEntity == null) return null;

            var bindRecordDto=bindRecordEntity.ToModel<EquSparepartEquipmentBindRecordDto>();
            //查询备件信息
            var sparePartsEntity = await _sparePartsRepository.GetByIdAsync(bindRecordEntity.SparepartId);
 
            //查询设备信息
            var equEquipmentEntity = await _equEquipmentRepository.GetByIdAsync(bindRecordEntity.EquipmentId);

            bindRecordDto.SparepartCode = sparePartsEntity?.Code??"";
            bindRecordDto.SparepartName= sparePartsEntity?.Name??"";
            bindRecordDto.EquipmentCode = equEquipmentEntity?.EquipmentCode ?? "";
            bindRecordDto.EquipmentName= equEquipmentEntity?.EquipmentName ?? "";
            return bindRecordDto;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparepartEquipmentBindRecordViewDto>> GetPagedListAsync(EquSparepartEquipmentBindRecordPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquSparepartEquipmentBindRecordPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equSparepartEquipmentBindRecordRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquSparepartEquipmentBindRecordViewDto>());
            return new PagedInfo<EquSparepartEquipmentBindRecordViewDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
