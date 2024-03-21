using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquToolLifeRecord;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquToolLifeRecord;
using Hymson.MES.Data.Repositories.EquToolLifeRecord.Query;
using Hymson.MES.Services.Dtos.EquToolLifeRecord;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.EquToolLifeRecord
{
    /// <summary>
    /// 服务（设备夹具寿命） 
    /// </summary>
    public class EquToolLifeRecordService : IEquToolLifeRecordService
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
        private readonly AbstractValidator<EquToolLifeRecordSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（设备夹具寿命）
        /// </summary>
        private readonly IEquToolLifeRecordRepository _equToolLifeRecordRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equToolLifeRecordRepository"></param>
        public EquToolLifeRecordService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquToolLifeRecordSaveDto> validationSaveRules, 
            IEquToolLifeRecordRepository equToolLifeRecordRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equToolLifeRecordRepository = equToolLifeRecordRepository;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(EquToolLifeRecordSaveDto saveDto)
        {
            // DTO转换实体
            var entity = saveDto.ToEntity<EquToolLifeRecordEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();

            // 保存
            return await _equToolLifeRecordRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquToolLifeRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<EquToolLifeRecordEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _equToolLifeRecordRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquToolLifeRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<EquToolLifeRecordEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _equToolLifeRecordRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _equToolLifeRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _equToolLifeRecordRepository.DeletesAsync(new DeleteCommand
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
        public async Task<EquToolLifeRecordDto?> QueryByIdAsync(long id) 
        {
           var equToolLifeRecordEntity = await _equToolLifeRecordRepository.GetByIdAsync(id);
           if (equToolLifeRecordEntity == null) return null;
           
           return equToolLifeRecordEntity.ToModel<EquToolLifeRecordDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquToolLifeRecordDto>> GetPagedListAsync(EquToolLifeRecordPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquToolLifeRecordPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equToolLifeRecordRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquToolLifeRecordDto>());
            return new PagedInfo<EquToolLifeRecordDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
