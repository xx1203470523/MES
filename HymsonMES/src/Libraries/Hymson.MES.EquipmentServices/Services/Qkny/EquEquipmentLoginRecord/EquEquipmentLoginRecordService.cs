using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquEquipmentLoginRecord;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquEquipmentLoginRecord;
using Hymson.MES.Data.Repositories.EquEquipmentLoginRecord.Query;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.EquEquipmentLoginRecord
{
    /// <summary>
    /// 服务（操作员登录记录） 
    /// </summary>
    public class EquEquipmentLoginRecordService : IEquEquipmentLoginRecordService
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
        private readonly AbstractValidator<EquEquipmentLoginRecordSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（操作员登录记录）
        /// </summary>
        private readonly IEquEquipmentLoginRecordRepository _equEquipmentLoginRecordRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equEquipmentLoginRecordRepository"></param>
        public EquEquipmentLoginRecordService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquEquipmentLoginRecordSaveDto> validationSaveRules, 
            IEquEquipmentLoginRecordRepository equEquipmentLoginRecordRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equEquipmentLoginRecordRepository = equEquipmentLoginRecordRepository;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(EquEquipmentLoginRecordSaveDto saveDto)
        {
            // DTO转换实体
            var entity = saveDto.ToEntity<EquEquipmentLoginRecordEntity>();
            // 保存
            return await _equEquipmentLoginRecordRepository.InsertAsync(entity);
        }
    }
}
