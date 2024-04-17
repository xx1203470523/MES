using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquProcessParamRecord;
using Hymson.MES.Core.Domain.ManuFeedingNoProductionRecord;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.ManuFeedingNoProductionRecord;
using Hymson.MES.Data.Repositories.ManuFeedingNoProductionRecord.Query;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.ManuFeedingNoProductionRecord;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.ManuFeedingNoProductionRecord
{
    /// <summary>
    /// 服务（设备投料非生产投料(洗罐子)） 
    /// </summary>
    public class ManuFeedingNoProductionRecordService : IManuFeedingNoProductionRecordService
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
        private readonly AbstractValidator<ManuFeedingNoProductionRecordSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（设备投料非生产投料(洗罐子)）
        /// </summary>
        private readonly IManuFeedingNoProductionRecordRepository _manuFeedingNoProductionRecordRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="manuFeedingNoProductionRecordRepository"></param>
        public ManuFeedingNoProductionRecordService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ManuFeedingNoProductionRecordSaveDto> validationSaveRules, 
            IManuFeedingNoProductionRecordRepository manuFeedingNoProductionRecordRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _manuFeedingNoProductionRecordRepository = manuFeedingNoProductionRecordRepository;
        }

        /// <summary>
        /// 添加多个
        /// </summary>
        /// <param name="saveDtoList"></param>
        /// <returns></returns>
        public async Task<int> AddMultAsync(List<ManuFeedingNoProductionRecordSaveDto> saveDtoList)
        {
            List<ManuFeedingNoProductionRecordEntity> list = saveDtoList
                .Select(m => new ManuFeedingNoProductionRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    EquipmentId = m.EquipmentId,
                    ConsumeEquipmentCode = m.ConsumeEquipmentCode,
                    ConsumeResourceCodeCode = m.ConsumeResourceCodeCode,
                    Sfc = m.Sfc,
                    Qty = m.Qty,
                    Category = m.Category,
                    CreatedBy = m.CreatedBy,
                    CreatedOn = m.CreatedOn,
                    UpdatedBy = m.UpdatedBy,
                    UpdatedOn = m.UpdatedOn,
                    IsDeleted = m.IsDeleted,
                    Remark = m.Remark,
                    SiteId = m.SiteId,
                }).ToList();
            return await _manuFeedingNoProductionRecordRepository.InsertRangeAsync(list);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuFeedingNoProductionRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuFeedingNoProductionRecordEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _manuFeedingNoProductionRecordRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ManuFeedingNoProductionRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuFeedingNoProductionRecordEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _manuFeedingNoProductionRecordRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _manuFeedingNoProductionRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _manuFeedingNoProductionRecordRepository.DeletesAsync(new DeleteCommand
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
        public async Task<ManuFeedingNoProductionRecordDto?> QueryByIdAsync(long id) 
        {
           var manuFeedingNoProductionRecordEntity = await _manuFeedingNoProductionRecordRepository.GetByIdAsync(id);
           if (manuFeedingNoProductionRecordEntity == null) return null;
           
           return manuFeedingNoProductionRecordEntity.ToModel<ManuFeedingNoProductionRecordDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFeedingNoProductionRecordDto>> GetPagedListAsync(ManuFeedingNoProductionRecordPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuFeedingNoProductionRecordPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuFeedingNoProductionRecordRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuFeedingNoProductionRecordDto>());
            return new PagedInfo<ManuFeedingNoProductionRecordDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
