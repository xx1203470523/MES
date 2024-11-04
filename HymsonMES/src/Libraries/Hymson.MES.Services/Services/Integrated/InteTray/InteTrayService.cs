using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Integrated.InteTray.Query;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 托盘信息 服务
    /// </summary>
    public class InteTrayService : IInteTrayService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 托盘信息 仓储
        /// </summary>
        private readonly IInteTrayRepository _inteTrayRepository;
        private readonly AbstractValidator<InteTraySaveDto> _validationSaveRules;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="inteTrayRepository"></param>
        public InteTrayService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<InteTraySaveDto> validationSaveRules,
            IInteTrayRepository inteTrayRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _inteTrayRepository = inteTrayRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(InteTraySaveDto parm)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(parm);

            // DTO转换实体
            var entity = parm.ToEntity<InteTrayEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId ?? 123456;

            // 编码唯一性验证
            var checkEntity = await _inteTrayRepository.GetByCodeAsync(new EntityByCodeQuery { Site = entity.SiteId, Code = entity.Code });
            if (checkEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES10900)).WithData("Code", entity.Code);

            // 入库
            return await _inteTrayRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(InteTraySaveDto parm)
        {
            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(parm);

            // DTO转换实体
            var entity = parm.ToEntity<InteTrayEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            return await _inteTrayRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _inteTrayRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteTrayDto>> GetPagedListAsync(InteTrayPagedQueryDto parm)
        {
            var pagedQuery = parm.ToQuery<InteTrayPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 123456;
            var pagedInfo = await _inteTrayRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<InteTrayDto>());
            return new PagedInfo<InteTrayDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteTrayDto> QueryByIdAsync(long id)
        {
            var entity = await _inteTrayRepository.GetByIdAsync(id);
            if (entity == null) return null;

            return entity.ToModel<InteTrayDto>();
        }

    }
}
