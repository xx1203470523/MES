using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquEquipmentHeartRecord;
using Hymson.MES.Core.Domain.EquEquipmentLoginRecord;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquEquipmentHeartRecord;
using Hymson.MES.Data.Repositories.EquEquipmentHeartRecord.Query;
using Hymson.MES.Services.Dtos.EquEquipmentHeartRecord;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.EquEquipmentHeartRecord
{
    /// <summary>
    /// 服务（设备心跳登录记录） 
    /// </summary>
    public class EquEquipmentHeartRecordService : IEquEquipmentHeartRecordService
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
        private readonly AbstractValidator<EquEquipmentHeartRecordSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（设备心跳登录记录）
        /// </summary>
        private readonly IEquEquipmentHeartRecordRepository _equEquipmentHeartRecordRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equEquipmentHeartRecordRepository"></param>
        public EquEquipmentHeartRecordService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquEquipmentHeartRecordSaveDto> validationSaveRules, 
            IEquEquipmentHeartRecordRepository equEquipmentHeartRecordRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equEquipmentHeartRecordRepository = equEquipmentHeartRecordRepository;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(EquEquipmentHeartRecordSaveDto saveDto)
        {
            // DTO转换实体
            var entity = saveDto.ToEntity<EquEquipmentHeartRecordEntity>();
            // 保存
            return await _equEquipmentHeartRecordRepository.InsertAsync(entity);
        }
    }
}
