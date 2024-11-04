/*
 *creator: Karl
 *
 *describe: 出站绑定的物料批次条码    服务 | 代码由框架生成
 *builder:  chenjianxiong
 *build datetime: 2023-05-25 08:58:04
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 出站绑定的物料批次条码 服务
    /// </summary>
    public class ManuSfcStepMaterialService : IManuSfcStepMaterialService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 出站绑定的物料批次条码 仓储
        /// </summary>
        private readonly IManuSfcStepMaterialRepository _manuSfcStepMaterialRepository;
        private readonly AbstractValidator<ManuSfcStepMaterialCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuSfcStepMaterialModifyDto> _validationModifyRules;

        public ManuSfcStepMaterialService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcStepMaterialRepository manuSfcStepMaterialRepository, AbstractValidator<ManuSfcStepMaterialCreateDto> validationCreateRules, AbstractValidator<ManuSfcStepMaterialModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcStepMaterialRepository = manuSfcStepMaterialRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuSfcStepMaterialCreateDto"></param>
        /// <returns></returns>
        public async Task CreateManuSfcStepMaterialAsync(ManuSfcStepMaterialCreateDto manuSfcStepMaterialCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuSfcStepMaterialCreateDto);

            //DTO转换实体
            var manuSfcStepMaterialEntity = manuSfcStepMaterialCreateDto.ToEntity<ManuSfcStepMaterialEntity>();
            manuSfcStepMaterialEntity.Id= IdGenProvider.Instance.CreateId();
            manuSfcStepMaterialEntity.CreatedBy = _currentUser.UserName;
            manuSfcStepMaterialEntity.UpdatedBy = _currentUser.UserName;
            manuSfcStepMaterialEntity.CreatedOn = HymsonClock.Now();
            manuSfcStepMaterialEntity.UpdatedOn = HymsonClock.Now();
            manuSfcStepMaterialEntity.SiteId = _currentSite.SiteId ?? 123456;

            //入库
            await _manuSfcStepMaterialRepository.InsertAsync(manuSfcStepMaterialEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuSfcStepMaterialAsync(long id)
        {
            await _manuSfcStepMaterialRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuSfcStepMaterialAsync(long[] ids)
        {
            return await _manuSfcStepMaterialRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuSfcStepMaterialPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcStepMaterialDto>> GetPagedListAsync(ManuSfcStepMaterialPagedQueryDto manuSfcStepMaterialPagedQueryDto)
        {
            var manuSfcStepMaterialPagedQuery = manuSfcStepMaterialPagedQueryDto.ToQuery<ManuSfcStepMaterialPagedQuery>();
            var pagedInfo = await _manuSfcStepMaterialRepository.GetPagedInfoAsync(manuSfcStepMaterialPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuSfcStepMaterialDto> manuSfcStepMaterialDtos = PrepareManuSfcStepMaterialDtos(pagedInfo);
            return new PagedInfo<ManuSfcStepMaterialDto>(manuSfcStepMaterialDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuSfcStepMaterialDto> PrepareManuSfcStepMaterialDtos(PagedInfo<ManuSfcStepMaterialEntity>   pagedInfo)
        {
            var manuSfcStepMaterialDtos = new List<ManuSfcStepMaterialDto>();
            foreach (var manuSfcStepMaterialEntity in pagedInfo.Data)
            {
                var manuSfcStepMaterialDto = manuSfcStepMaterialEntity.ToModel<ManuSfcStepMaterialDto>();
                manuSfcStepMaterialDtos.Add(manuSfcStepMaterialDto);
            }

            return manuSfcStepMaterialDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcStepMaterialDto"></param>
        /// <returns></returns>
        public async Task ModifyManuSfcStepMaterialAsync(ManuSfcStepMaterialModifyDto manuSfcStepMaterialModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuSfcStepMaterialModifyDto);

            //DTO转换实体
            var manuSfcStepMaterialEntity = manuSfcStepMaterialModifyDto.ToEntity<ManuSfcStepMaterialEntity>();
            manuSfcStepMaterialEntity.UpdatedBy = _currentUser.UserName;
            manuSfcStepMaterialEntity.UpdatedOn = HymsonClock.Now();

            await _manuSfcStepMaterialRepository.UpdateAsync(manuSfcStepMaterialEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcStepMaterialDto> QueryManuSfcStepMaterialByIdAsync(long id) 
        {
           var manuSfcStepMaterialEntity = await _manuSfcStepMaterialRepository.GetByIdAsync(id);
           if (manuSfcStepMaterialEntity != null) 
           {
               return manuSfcStepMaterialEntity.ToModel<ManuSfcStepMaterialDto>();
           }
            return null;
        }
    }
}
