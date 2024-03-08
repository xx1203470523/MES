using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.CcdFileUploadCompleteRecord;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.CcdFileUploadCompleteRecord;
using Hymson.MES.Data.Repositories.CcdFileUploadCompleteRecord.Query;
using Hymson.MES.Services.Dtos.CcdFileUploadCompleteRecord;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.CcdFileUploadCompleteRecord
{
    /// <summary>
    /// 服务（CCD文件上传完成） 
    /// </summary>
    public class CcdFileUploadCompleteRecordService : ICcdFileUploadCompleteRecordService
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
        private readonly AbstractValidator<CcdFileUploadCompleteRecordSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（CCD文件上传完成）
        /// </summary>
        private readonly ICcdFileUploadCompleteRecordRepository _ccdFileUploadCompleteRecordRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="ccdFileUploadCompleteRecordRepository"></param>
        public CcdFileUploadCompleteRecordService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<CcdFileUploadCompleteRecordSaveDto> validationSaveRules, 
            ICcdFileUploadCompleteRecordRepository ccdFileUploadCompleteRecordRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _ccdFileUploadCompleteRecordRepository = ccdFileUploadCompleteRecordRepository;
        }

        /// <summary>
        /// 添加多个
        /// </summary>
        /// <param name="saveDtoList"></param>
        /// <returns></returns>
        public async Task<int> AddMultAsync(List<CcdFileUploadCompleteRecordSaveDto> saveDtoList)
        {
            List<CcdFileUploadCompleteRecordEntity> list = saveDtoList
                .Select(m => new CcdFileUploadCompleteRecordEntity
                {
                    Id = m.Id,
                    EquipmentId = m.EquipmentId,
                    Sfc = m.Sfc,
                    SfcIsPassed = m.SfcIsPassed,
                    Uri = m.Uri,
                    UriIsPassed = m.UriIsPassed,
                    CreatedBy = m.CreatedBy,
                    CreatedOn = m.CreatedOn,
                    UpdatedBy = m.UpdatedBy,
                    UpdatedOn = m.UpdatedOn,
                    IsDeleted = m.IsDeleted,
                    Remark = m.Remark,
                    SiteId = m.SiteId,
                }).ToList();
            return await _ccdFileUploadCompleteRecordRepository.InsertRangeAsync(list);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(CcdFileUploadCompleteRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<CcdFileUploadCompleteRecordEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _ccdFileUploadCompleteRecordRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(CcdFileUploadCompleteRecordSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<CcdFileUploadCompleteRecordEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _ccdFileUploadCompleteRecordRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _ccdFileUploadCompleteRecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _ccdFileUploadCompleteRecordRepository.DeletesAsync(new DeleteCommand
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
        public async Task<CcdFileUploadCompleteRecordDto?> QueryByIdAsync(long id) 
        {
           var ccdFileUploadCompleteRecordEntity = await _ccdFileUploadCompleteRecordRepository.GetByIdAsync(id);
           if (ccdFileUploadCompleteRecordEntity == null) return null;
           
           return ccdFileUploadCompleteRecordEntity.ToModel<CcdFileUploadCompleteRecordDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<CcdFileUploadCompleteRecordDto>> GetPagedListAsync(CcdFileUploadCompleteRecordPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<CcdFileUploadCompleteRecordPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _ccdFileUploadCompleteRecordRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<CcdFileUploadCompleteRecordDto>());
            return new PagedInfo<CcdFileUploadCompleteRecordDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
