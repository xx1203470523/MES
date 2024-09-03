using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.NIO;
using Hymson.MES.CoreServices.Helper;
using Hymson.MES.Data.NIO;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.NIO;
using Hymson.MES.Data.Repositories.NIO.Query;
using Hymson.MES.Services.Dtos.NIO;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.NIO
{
    /// <summary>
    /// 服务（物料发货信息表） 
    /// </summary>
    public class NioPushActualDeliveryService : INioPushActualDeliveryService
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
        /// 仓储接口（物料发货信息表）
        /// </summary>
        private readonly INioPushActualDeliveryRepository _nioPushActualDeliveryRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly INioPushRepository _nioPushRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NioPushActualDeliveryService(ICurrentUser currentUser, ICurrentSite currentSite,
            INioPushActualDeliveryRepository nioPushActualDeliveryRepository,
            INioPushRepository nioPushRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _nioPushActualDeliveryRepository = nioPushActualDeliveryRepository;
            _nioPushRepository = nioPushRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(NioPushActualDeliverySaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<NioPushActualDeliveryEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            //entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _nioPushActualDeliveryRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(NioPushActualDeliverySaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            var dbModel = await _nioPushRepository.GetByIdAsync(saveDto.NioPushId);
            if(dbModel.Status == Core.Enums.Plan.PushStatusEnum.Success)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17773));
            }

            DateTime date = Convert.ToDateTime(saveDto.Date);
            saveDto.ActualDeliveryTime = NioHelper.GetTimestamp(date);

            // DTO转换实体
            var entity = saveDto.ToEntity<NioPushActualDeliveryEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _nioPushActualDeliveryRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _nioPushActualDeliveryRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _nioPushActualDeliveryRepository.DeletesAsync(new DeleteCommand
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
        public async Task<NioPushActualDeliveryDto?> QueryByIdAsync(long id) 
        {
           var nioPushActualDeliveryEntity = await _nioPushActualDeliveryRepository.GetByIdAsync(id);
           if (nioPushActualDeliveryEntity == null) return null;
           
           return nioPushActualDeliveryEntity.ToModel<NioPushActualDeliveryDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<NioPushActualDeliveryDto>> GetPagedListAsync(NioPushActualDeliveryPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<NioPushActualDeliveryPagedQuery>();
            //pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _nioPushActualDeliveryRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<NioPushActualDeliveryDto>());
            return new PagedInfo<NioPushActualDeliveryDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
