using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process.MaskCode;
using Hymson.MES.Data.Repositories.Process.MaskCode.Query;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Process.MaskCode
{
    /// <summary>
    /// 服务（掩码维护）
    /// </summary>
    public class ProcMaskCodeService : IProcMaskCodeService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 
        /// </summary>
        private readonly IProcMaskCodeRepository _procMaskCodeRepository;
        //private readonly AbstractValidator<ProcMaskCodeSaveDto> _validationCreateRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="procMaskCodeRepository"></param>
        /// <param name="validationRules"></param>
        public ProcMaskCodeService(ICurrentUser currentUser, ICurrentSite currentSite,
            IProcMaskCodeRepository procMaskCodeRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procMaskCodeRepository = procMaskCodeRepository;
        }

        /// <summary>
        /// 添加（掩码维护）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ProcMaskCodeSaveDto createDto)
        {
            // 验证DTO
            //await _validationCreateRules.ValidateAndThrowAsync(createDto);

            // DTO转换实体
            var entity = createDto.ToEntity<ProcMaskCodeEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId ?? 0;
            entity.Code = entity.Code.ToUpper();

            // 保存实体
            return await _procMaskCodeRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 更新（掩码维护）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ProcMaskCodeSaveDto modifyDto)
        {
            // DTO转换实体
            var entity = modifyDto.ToEntity<ProcMaskCodeEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            // 保存实体
            return await _procMaskCodeRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除（掩码维护）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            return await _procMaskCodeRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 获取分页数据（掩码维护）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaskCodeDto>> GetPagedListAsync(ProcMaskCodePagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ProcMaskCodePagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _procMaskCodeRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ProcMaskCodeDto>());
            return new PagedInfo<ProcMaskCodeDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（掩码维护）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaskCodeDto> GetDetailAsync(long id)
        {
            return (await _procMaskCodeRepository.GetByIdAsync(id)).ToModel<ProcMaskCodeDto>();
        }
    }
}
