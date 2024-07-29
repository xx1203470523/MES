/*
 *creator: Karl
 *
 *describe: 设备台账信息    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:53:50
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
using Hymson.MES.Core.Enums.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquEquipmentRecord;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.EquSparepartRecord;
using Hymson.MES.Services.Dtos.EquEquipmentRecord;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.EquEquipmentRecord
{
    /// <summary>
    /// 设备台账信息 服务
    /// </summary>
    public class EquEquipmentRecordService : IEquEquipmentRecordService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备台账信息 仓储
        /// </summary>
        private readonly IEquEquipmentRecordRepository _equEquipmentRecordRepository;
        /// <summary>
        /// 设备 
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;
        public EquEquipmentRecordService(ICurrentUser currentUser, ICurrentSite currentSite, IEquEquipmentRecordRepository equEquipmentRecordRepository, IEquEquipmentRepository equEquipmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equEquipmentRecordRepository = equEquipmentRecordRepository;
            _equEquipmentRepository = equEquipmentRepository;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="equEquipmentRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentRecordPagedViewDto>> GetPagedListAsync(EquEquipmentRecordPagedQueryDto equEquipmentRecordPagedQueryDto)
        {
            var equEquipmentRecordPagedQuery = equEquipmentRecordPagedQueryDto.ToQuery<EquEquipmentRecordPagedQuery>();
            equEquipmentRecordPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equEquipmentRecordRepository.GetPagedInfoAsync(equEquipmentRecordPagedQuery);

            //实体到DTO转换 装载数据
            List<EquEquipmentRecordPagedViewDto> equEquipmentRecordDtos = PrepareEquEquipmentRecordDtos(pagedInfo);
            return new PagedInfo<EquEquipmentRecordPagedViewDto>(equEquipmentRecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquEquipmentRecordPagedViewDto> PrepareEquEquipmentRecordDtos(PagedInfo<EquEquipmentRecordPagedView> pagedInfo)
        {
            var equEquipmentRecordDtos = new List<EquEquipmentRecordPagedViewDto>();
            foreach (var equEquipmentRecordEntity in pagedInfo.Data)
            {
                var equEquipmentRecordDto = equEquipmentRecordEntity.ToModel<EquEquipmentRecordPagedViewDto>();
                equEquipmentRecordDtos.Add(equEquipmentRecordDto);
            }

            return equEquipmentRecordDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentRecordDto> QueryEquEquipmentRecordByIdAsync(long id)
        {
            var equEquipmentRecordEntity = await _equEquipmentRecordRepository.GetByIdAsync(id);
            if (equEquipmentRecordEntity != null)
            {
                return equEquipmentRecordEntity.ToModel<EquEquipmentRecordDto>();
            }
            return null;
        }


        /// <summary>
        /// 获取添加设备记录（根据设备）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>  
        public async Task<EquEquipmentRecordEntity?> GetAddEquRecordByEquEquipmentAsync(GetAddEquRecordByEquEquipmentDto param)
        {
            if (param.EquEquipmentEntity == null && param.EquipmentId != 0)
            {
                var equEquipmentEntity = await _equEquipmentRepository.GetByIdAsync(param.EquipmentId);
                param.EquEquipmentEntity = equEquipmentEntity;
            }

            if (param.EquEquipmentEntity == null) return null;

            return new EquEquipmentRecordEntity()
            {
                Id = IdGenProvider.Instance.CreateId(),
                EquipmentId = param.EquEquipmentEntity.Id,
                EquipmentCode = param.EquEquipmentEntity.EquipmentCode,
                EquipmentName = param.EquEquipmentEntity.EquipmentName,
                EquipmentGroupId = param.EquEquipmentEntity.EquipmentGroupId,
                EquipmentDesc = param.EquEquipmentEntity.EquipmentDesc,
                WorkCenterFactoryId = param.EquEquipmentEntity.WorkCenterFactoryId,
                WorkCenterShopId = param.EquEquipmentEntity.WorkCenterShopId,
                WorkCenterLineId = param.EquEquipmentEntity.WorkCenterLineId,
                Location = param.EquEquipmentEntity.Location,
                EquipmentType = param.EquEquipmentEntity.EquipmentType,
                UseDepartment = param.EquEquipmentEntity.UseDepartment,
                UseStatus = param.EquEquipmentEntity.UseStatus,
                EntryDate = param.EquEquipmentEntity.EntryDate,
                QualTime = param.EquEquipmentEntity.QualTime,
                ExpireDate = param.EquEquipmentEntity.ExpireDate,
                Manufacturer = param.EquEquipmentEntity.Manufacturer,
                Supplier = param.EquEquipmentEntity.Supplier,
                Power = param.EquEquipmentEntity.Power,
                EnergyLevel = param.EquEquipmentEntity.EnergyLevel,
                OperationType = param.operationType,
                Ip = param.EquEquipmentEntity.Ip,
                TakeTime = param.EquEquipmentEntity.TakeTime,
                Remark = param.EquEquipmentEntity.Remark,
                CreatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                SiteId = _currentSite.SiteId ?? 0
            };
        }

    }
}
