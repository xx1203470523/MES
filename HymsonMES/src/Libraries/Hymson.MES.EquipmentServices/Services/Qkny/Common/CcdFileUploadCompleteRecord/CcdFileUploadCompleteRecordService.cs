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
    }
}
