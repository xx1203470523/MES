using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.ManuEquipmentStatusTime;
using Hymson.MES.Core.Domain.ManuEuqipmentNewestInfoEntity;
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
            dbModel.CurrentStatus = newestModel.Status;
            dbModel.BeginTime = (DateTime)newestModel.StatusUpdatedOn;
            dbModel.NextStatus = saveDto.NextStatus;
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

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuEquipmentStatusTimeSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuEquipmentStatusTimeEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _manuEquipmentStatusTimeRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ManuEquipmentStatusTimeSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuEquipmentStatusTimeEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _manuEquipmentStatusTimeRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _manuEquipmentStatusTimeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _manuEquipmentStatusTimeRepository.DeletesAsync(new DeleteCommand
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
        public async Task<ManuEquipmentStatusTimeDto?> QueryByIdAsync(long id) 
        {
           var manuEquipmentStatusTimeEntity = await _manuEquipmentStatusTimeRepository.GetByIdAsync(id);
           if (manuEquipmentStatusTimeEntity == null) return null;
           
           return manuEquipmentStatusTimeEntity.ToModel<ManuEquipmentStatusTimeDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuEquipmentStatusTimeDto>> GetPagedListAsync(ManuEquipmentStatusTimePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuEquipmentStatusTimePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuEquipmentStatusTimeRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuEquipmentStatusTimeDto>());
            return new PagedInfo<ManuEquipmentStatusTimeDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
