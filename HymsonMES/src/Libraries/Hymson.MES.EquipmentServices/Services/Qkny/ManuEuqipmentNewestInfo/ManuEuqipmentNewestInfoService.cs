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

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuEuqipmentNewestInfoSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuEuqipmentNewestInfoEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _manuEuqipmentNewestInfoRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ManuEuqipmentNewestInfoSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuEuqipmentNewestInfoEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _manuEuqipmentNewestInfoRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _manuEuqipmentNewestInfoRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _manuEuqipmentNewestInfoRepository.DeletesAsync(new DeleteCommand
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
        public async Task<ManuEuqipmentNewestInfoDto?> QueryByIdAsync(long id) 
        {
           var manuEuqipmentNewestInfoEntity = await _manuEuqipmentNewestInfoRepository.GetByIdAsync(id);
           if (manuEuqipmentNewestInfoEntity == null) return null;
           
           return manuEuqipmentNewestInfoEntity.ToModel<ManuEuqipmentNewestInfoDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuEuqipmentNewestInfoDto>> GetPagedListAsync(ManuEuqipmentNewestInfoPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuEuqipmentNewestInfoPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuEuqipmentNewestInfoRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuEuqipmentNewestInfoDto>());
            return new PagedInfo<ManuEuqipmentNewestInfoDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
