using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.ManuEquipmentStatusTime;
using Hymson.MES.Core.Domain.ManuEuqipmentNewestInfoEntity;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.ManuEquipmentStatusTime;
using Hymson.MES.Data.Repositories.ManuEquipmentStatusTime.Query;
using Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo;
using Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo.Query;
using Hymson.MES.Services.Dtos.ManuEquipmentStatusTime;
using Hymson.Snowflake;
using Hymson.Utils;
using static Dapper.SqlMapper;

namespace Hymson.MES.Services.Services.ManuEquipmentStatusTime
{
    /// <summary>
    /// 服务（设备状态时间） 
    /// </summary>
    public class ManuEquipmentStatusTimeService : IManuEquipmentStatusTimeService
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
        private readonly AbstractValidator<ManuEquipmentStatusTimeSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（设备状态时间）
        /// </summary>
        private readonly IManuEquipmentStatusTimeRepository _manuEquipmentStatusTimeRepository;

        /// <summary>
        /// 设备最新状态
        /// </summary>
        private readonly IManuEuqipmentNewestInfoRepository _manuEuqipmentNewestInfoRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="manuEquipmentStatusTimeRepository"></param>
        /// <param name="manuEuqipmentNewestInfoRepository"></param>
        public ManuEquipmentStatusTimeService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ManuEquipmentStatusTimeSaveDto> validationSaveRules, 
            IManuEquipmentStatusTimeRepository manuEquipmentStatusTimeRepository,
            IManuEuqipmentNewestInfoRepository manuEuqipmentNewestInfoRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _manuEquipmentStatusTimeRepository = manuEquipmentStatusTimeRepository;
            _manuEuqipmentNewestInfoRepository = manuEuqipmentNewestInfoRepository;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(ManuEquipmentStatusTimeSaveDto saveDto)
        {
            //判断是否已经上传过状态信息(从最新记录表)
            ManuEuqipmentNewestInfoQuery query = new ManuEuqipmentNewestInfoQuery() { EquipmentId = saveDto.EquipmentId };
            var newList = await _manuEuqipmentNewestInfoRepository.GetEntitiesAsync(query);
            if(newList == null || newList.Any() == false)
            {
                return 0;
            }
            var newestModel = newList.Where(m => string.IsNullOrEmpty(m.Status) == false).FirstOrDefault();
            if(newestModel == null)
            {
                return 0;
            }
            //没有上传过则不处理，已经上传过则添加一条记录
            var dateNow = HymsonClock.Now();
            ManuEquipmentStatusTimeEntity dbModel = new ManuEquipmentStatusTimeEntity();
            dbModel.Id = IdGenProvider.Instance.CreateId();
            dbModel.EquipmentId = saveDto.EquipmentId;
            dbModel.CurrentStatus = (ManuEquipmentStatusEnum)Enum.Parse(typeof(ManuEquipmentStatusEnum), newestModel.Status); //newestModel.Status;
            dbModel.BeginTime = (DateTime)newestModel.StatusUpdatedOn;
            dbModel.NextStatus = (ManuEquipmentStatusEnum)Enum.Parse(typeof(ManuEquipmentStatusEnum), saveDto.NextStatus);//saveDto.NextStatus;
            dbModel.EndTime = dateNow;
            TimeSpan span = dbModel.EndTime - dbModel.BeginTime;
            dbModel.StatusDuration = (int)span.TotalSeconds;
            dbModel.CreatedBy = saveDto.CreatedBy;
            dbModel.CreatedOn = dateNow;
            dbModel.UpdatedBy = dbModel.CreatedBy;
            dbModel.UpdatedOn = dateNow;
            dbModel.SiteId = saveDto.SiteId;
            dbModel.EquipmentDownReason = newestModel.DownReason;

            return await _manuEquipmentStatusTimeRepository.InsertAsync(dbModel);
        }
    }
}
