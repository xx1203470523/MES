using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.ManuJzBind;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.Qkny.ManuJzBind.Command;
using Hymson.MES.Data.Repositories.ManuJzBind;
using Hymson.MES.Data.Repositories.ManuJzBind.Query;
using Hymson.MES.Services.Dtos.ManuJzBind;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.ManuJzBind
{
    /// <summary>
    /// 服务（极组绑定） 
    /// </summary>
    public class ManuJzBindService : IManuJzBindService
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
        private readonly AbstractValidator<ManuJzBindSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（极组绑定）
        /// </summary>
        private readonly IManuJzBindRepository _manuJzBindRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="manuJzBindRepository"></param>
        public ManuJzBindService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ManuJzBindSaveDto> validationSaveRules, 
            IManuJzBindRepository manuJzBindRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _manuJzBindRepository = manuJzBindRepository;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(ManuJzBindSaveDto saveDto)
        {
            // DTO转换实体
            var entity = saveDto.ToEntity<ManuJzBindEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();

            // 保存
            return await _manuJzBindRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 根据极组条码查询
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<ManuJzBindEntity> GetByJzSfcAsync(ManuJzBindQuery query)
        {
            var dbModel = await _manuJzBindRepository.GetByJzSfcAsync(query);
            if(dbModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45270));
            }

            return dbModel;
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeletePhysicsAsync(long id)
        {
            return await _manuJzBindRepository.DeletePhysicsAsync(id);
        }

        /// <summary>
        /// 根据id更新电芯码
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<int> UpdateSfcById(UpdateSfcByIdCommand command)
        {
            int result = await _manuJzBindRepository.UpdateSfcById(command);

            return result;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ManuJzBindSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuJzBindEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = updatedBy;
            entity.CreatedOn = updatedOn;
            entity.UpdatedBy = updatedBy;
            entity.UpdatedOn = updatedOn;
            entity.SiteId = _currentSite.SiteId ?? 0;

            // 保存
            return await _manuJzBindRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(ManuJzBindSaveDto saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

             // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            // DTO转换实体
            var entity = saveDto.ToEntity<ManuJzBindEntity>();
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            return await _manuJzBindRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await _manuJzBindRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] ids)
        {
            return await _manuJzBindRepository.DeletesAsync(new DeleteCommand
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
        public async Task<ManuJzBindDto?> QueryByIdAsync(long id) 
        {
           var manuJzBindEntity = await _manuJzBindRepository.GetByIdAsync(id);
           if (manuJzBindEntity == null) return null;
           
           return manuJzBindEntity.ToModel<ManuJzBindDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuJzBindDto>> GetPagedListAsync(ManuJzBindPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuJzBindPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuJzBindRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuJzBindDto>());
            return new PagedInfo<ManuJzBindDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
