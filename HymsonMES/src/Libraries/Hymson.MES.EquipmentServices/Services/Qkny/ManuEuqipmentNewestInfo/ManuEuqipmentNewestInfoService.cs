using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquEquipmentLoginRecord;
using Hymson.MES.Core.Domain.ManuEuqipmentNewestInfoEntity;
using Hymson.MES.Core.Enums.Qkny;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo;
using Hymson.MES.Data.Repositories.ManuEuqipmentNewestInfo.Query;
using Hymson.MES.Services.Dtos.ManuEuqipmentNewestInfo;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.ManuEuqipmentNewestInfo
{
    /// <summary>
    /// 服务（设备最新信息） 
    /// </summary>
    public class ManuEuqipmentNewestInfoService : IManuEuqipmentNewestInfoService
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
        private readonly AbstractValidator<ManuEuqipmentNewestInfoSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（设备最新信息）
        /// </summary>
        private readonly IManuEuqipmentNewestInfoRepository _manuEuqipmentNewestInfoRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="manuEuqipmentNewestInfoRepository"></param>
        public ManuEuqipmentNewestInfoService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ManuEuqipmentNewestInfoSaveDto> validationSaveRules, 
            IManuEuqipmentNewestInfoRepository manuEuqipmentNewestInfoRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _manuEuqipmentNewestInfoRepository = manuEuqipmentNewestInfoRepository;
        }

        /// <summary>
        /// 添加或者更新
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateAsync(ManuEuqipmentNewestInfoSaveDto saveDto)
        {
            int result = 0;
            var model = await QueryByEquIdAsync(saveDto.EquipmentId);
            if(model == null)
            {
                ManuEuqipmentNewestInfoEntity dbModel = saveDto.ToEntity<ManuEuqipmentNewestInfoEntity>();
                dbModel.Id = IdGenProvider.Instance.CreateId();
                if(saveDto.Type == NewestInfoEnum.Heart)
                {
                    dbModel.Heart = saveDto.Heart;
                    dbModel.HeartUpdatedOn = saveDto.CreatedOn;
                    dbModel.LoginResultUpdatedOn = null;
                    dbModel.StatusUpdatedOn = null;
                }
                else if(saveDto.Type == NewestInfoEnum.Status)
                {
                    dbModel.Status = saveDto.Status;
                    dbModel.DownReason = saveDto.DownReason;
                    dbModel.StatusUpdatedOn = saveDto.CreatedOn;
                    dbModel.LoginResultUpdatedOn = null;
                    dbModel.HeartUpdatedOn = null;
                }
                else if(saveDto.Type == NewestInfoEnum.Login)
                {
                    dbModel.LoginResult = saveDto.LoginResult;
                    dbModel.LoginResultUpdatedOn = saveDto.CreatedOn;
                    dbModel.StatusUpdatedOn = null;
                    dbModel.HeartUpdatedOn = null;
                }
                result = await _manuEuqipmentNewestInfoRepository.InsertAsync(dbModel);
            }
            else
            {
                model.UpdatedBy = saveDto.UpdatedBy;
                model.UpdatedOn = saveDto.UpdatedOn;
                if (saveDto.Type == NewestInfoEnum.Heart)
                {
                    model.Heart = saveDto.Heart;
                    model.Status = string.Empty;
                    model.LoginResult = string.Empty;
                    model.HeartUpdatedOn = model.UpdatedOn;
                }
                else if (saveDto.Type == NewestInfoEnum.Status)
                {
                    model.Status = saveDto.Status;
                    model.Heart = string.Empty;
                    model.LoginResult = string.Empty;
                    model.StatusUpdatedOn = model.UpdatedOn;
                    model.DownReason = saveDto.DownReason;
                }
                else if (saveDto.Type == NewestInfoEnum.Login)
                {
                    model.LoginResult = saveDto.LoginResult;
                    model.Status = string.Empty;
                    model.Heart = string.Empty;
                    model.LoginResultUpdatedOn = model.UpdatedOn;
                }
                result = await _manuEuqipmentNewestInfoRepository.UpdateAsync(model);
            }

            return result;
        }

        /// <summary>
        /// 根据设备ID查询
        /// </summary>
        /// <param name="equId"></param>
        /// <returns></returns>
        public async Task<ManuEuqipmentNewestInfoEntity?> QueryByEquIdAsync(long equId)
        {
            ManuEuqipmentNewestInfoQuery query = new ManuEuqipmentNewestInfoQuery();
            query.EquipmentId = equId;
            var list = await _manuEuqipmentNewestInfoRepository.GetEntitiesAsync(query);
            if (list == null || list.Any() == false)
            {
                return null;
            }

            return list.First();
        }
    }
}
