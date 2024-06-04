using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.CcdFileUploadCompleteRecord;
using Hymson.MES.Core.Domain.ManuFeedingTransferRecord;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.ManuFeedingTransferRecord;
using Hymson.MES.Data.Repositories.ManuFeedingTransferRecord.Query;
using Hymson.MES.Services.Dtos.ManuFeedingTransferRecord;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.ManuFeedingTransferRecord
{
    /// <summary>
    /// 服务（上料信息转移记录） 
    /// </summary>
    public class ManuFeedingTransferRecordService : IManuFeedingTransferRecordService
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
        private readonly AbstractValidator<ManuFeedingTransferRecordSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（上料信息转移记录）
        /// </summary>
        private readonly IManuFeedingTransferRecordRepository _manuFeedingTransferRecordRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="manuFeedingTransferRecordRepository"></param>
        public ManuFeedingTransferRecordService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ManuFeedingTransferRecordSaveDto> validationSaveRules, 
            IManuFeedingTransferRecordRepository manuFeedingTransferRecordRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _manuFeedingTransferRecordRepository = manuFeedingTransferRecordRepository;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(ManuFeedingTransferRecordSaveDto saveDto)
        {
            // DTO转换实体
            var entity = saveDto.ToEntity<ManuFeedingTransferRecordEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();

            // 保存
            return await _manuFeedingTransferRecordRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDtoList"></param>
        /// <returns></returns>
        public async Task<int> AddMultAsync(List<ManuFeedingTransferRecordSaveDto> saveDtoList)
        {
            List<ManuFeedingTransferRecordEntity> list = saveDtoList
                .Select(m => new ManuFeedingTransferRecordEntity
                {
                    Id = m.Id,
                    EquipmentId = m.EquipmentId,
                    Sfc = m.Sfc,
                    Qty = m.Qty,
                    EquipmentCodeIn = m.EquipmentCodeIn,
                    EquipmentCodeOut = m.EquipmentCodeOut,
                    CreatedBy = m.CreatedBy,
                    CreatedOn = m.CreatedOn,
                    UpdatedBy = m.UpdatedBy,
                    UpdatedOn = m.UpdatedOn,
                    IsDeleted = m.IsDeleted,
                    Remark = m.Remark,
                    SiteId = m.SiteId,
                }).ToList();
            return await _manuFeedingTransferRecordRepository.InsertRangeAsync(list);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuFeedingTransferRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuFeedingTransferRecordEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _manuFeedingTransferRecordRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ManuFeedingTransferRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuFeedingTransferRecordEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _manuFeedingTransferRecordRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _manuFeedingTransferRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _manuFeedingTransferRecordRepository.DeletesAsync(new DeleteCommand
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
        public async Task<ManuFeedingTransferRecordDto?> QueryByIdAsync(long id) 
        {
           var manuFeedingTransferRecordEntity = await _manuFeedingTransferRecordRepository.GetByIdAsync(id);
           if (manuFeedingTransferRecordEntity == null) return null;
           
           return manuFeedingTransferRecordEntity.ToModel<ManuFeedingTransferRecordDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuFeedingTransferRecordDto>> GetPagedListAsync(ManuFeedingTransferRecordPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuFeedingTransferRecordPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuFeedingTransferRecordRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuFeedingTransferRecordDto>());
            return new PagedInfo<ManuFeedingTransferRecordDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
