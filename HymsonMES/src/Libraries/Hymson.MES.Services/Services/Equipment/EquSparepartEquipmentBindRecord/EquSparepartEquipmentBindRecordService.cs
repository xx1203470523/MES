using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;

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

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="equSparepartEquipmentBindRecordRepository"></param>
        public EquSparepartEquipmentBindRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
            IEquSparepartEquipmentBindRecordRepository equSparepartEquipmentBindRecordRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equSparepartEquipmentBindRecordRepository = equSparepartEquipmentBindRecordRepository;
        }

        /// <summary>
        /// 安装
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquSparepartEquipmentBindRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSparepartEquipmentBindRecordEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _equSparepartEquipmentBindRecordRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 卸载
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquSparepartEquipmentBindRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // DTO转换实体
            var entity = saveDto.ToEntity<EquSparepartEquipmentBindRecordEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _equSparepartEquipmentBindRecordRepository.UpdateAsync(entity);
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
           var equSparepartEquipmentBindRecordEntity = await _equSparepartEquipmentBindRecordRepository.GetByIdAsync(id);
           if (equSparepartEquipmentBindRecordEntity == null) return null;
           
           return equSparepartEquipmentBindRecordEntity.ToModel<EquSparepartEquipmentBindRecordDto>();
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
