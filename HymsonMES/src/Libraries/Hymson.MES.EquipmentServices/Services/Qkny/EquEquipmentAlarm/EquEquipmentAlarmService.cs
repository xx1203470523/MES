using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquEquipmentAlarm;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquEquipmentAlarm;
using Hymson.MES.Data.Repositories.EquEquipmentAlarm.Query;
using Hymson.MES.Services.Dtos.EquEquipmentAlarm;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.EquEquipmentAlarm
{
    /// <summary>
    /// 服务（设备报警记录） 
    /// </summary>
    public class EquEquipmentAlarmService : IEquEquipmentAlarmService
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
        private readonly AbstractValidator<EquEquipmentAlarmSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（设备报警记录）
        /// </summary>
        private readonly IEquEquipmentAlarmRepository _equEquipmentAlarmRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="equEquipmentAlarmRepository"></param>
        public EquEquipmentAlarmService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<EquEquipmentAlarmSaveDto> validationSaveRules, 
            IEquEquipmentAlarmRepository equEquipmentAlarmRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _equEquipmentAlarmRepository = equEquipmentAlarmRepository;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(EquEquipmentAlarmSaveDto saveDto)
        {
            var entity = saveDto.ToEntity<EquEquipmentAlarmEntity>();
            return await _equEquipmentAlarmRepository.InsertAsync(entity);
        }
    }
}
