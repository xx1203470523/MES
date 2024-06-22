using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（生产退料单明细） 
    /// </summary>
    public class ManuReturnOrderDetailService : IManuReturnOrderDetailService
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
        private readonly AbstractValidator<ManuReturnOrderDetailSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（生产退料单明细）
        /// </summary>
        private readonly IManuReturnOrderDetailRepository _manuReturnOrderDetailRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="manuReturnOrderDetailRepository"></param>
        public ManuReturnOrderDetailService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ManuReturnOrderDetailSaveDto> validationSaveRules, 
            IManuReturnOrderDetailRepository manuReturnOrderDetailRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _manuReturnOrderDetailRepository = manuReturnOrderDetailRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuReturnOrderDetailSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuReturnOrderDetailEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _manuReturnOrderDetailRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ManuReturnOrderDetailSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuReturnOrderDetailEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _manuReturnOrderDetailRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _manuReturnOrderDetailRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _manuReturnOrderDetailRepository.DeletesAsync(new DeleteCommand
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
        public async Task<ManuReturnOrderDetailDto?> QueryByIdAsync(long id) 
        {
           var manuReturnOrderDetailEntity = await _manuReturnOrderDetailRepository.GetByIdAsync(id);
           if (manuReturnOrderDetailEntity == null) return null;
           
           return manuReturnOrderDetailEntity.ToModel<ManuReturnOrderDetailDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuReturnOrderDetailDto>> GetPagedListAsync(ManuReturnOrderDetailPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuReturnOrderDetailPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuReturnOrderDetailRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuReturnOrderDetailDto>());
            return new PagedInfo<ManuReturnOrderDetailDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
