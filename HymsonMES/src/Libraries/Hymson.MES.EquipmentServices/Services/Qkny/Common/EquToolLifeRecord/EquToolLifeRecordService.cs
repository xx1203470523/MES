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
    }
}
