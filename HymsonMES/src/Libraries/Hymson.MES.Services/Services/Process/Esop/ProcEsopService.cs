/*
 *creator: Karl
 *
 *describe: ESOP    服务 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:39:53
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// ESOP 服务
    /// </summary>
    public class ProcEsopService : IProcEsopService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// ESOP 仓储
        /// </summary>
        private readonly IProcEsopRepository _procEsopRepository;
        private readonly AbstractValidator<ProcEsopCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcEsopModifyDto> _validationModifyRules;

        public ProcEsopService(ICurrentUser currentUser, ICurrentSite currentSite, IProcEsopRepository procEsopRepository, AbstractValidator<ProcEsopCreateDto> validationCreateRules, AbstractValidator<ProcEsopModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procEsopRepository = procEsopRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procEsopCreateDto"></param>
        /// <returns></returns>
        public async Task CreateProcEsopAsync(ProcEsopCreateDto procEsopCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procEsopCreateDto);

            //DTO转换实体
            var procEsopEntity = procEsopCreateDto.ToEntity<ProcEsopEntity>();
            procEsopEntity.Id= IdGenProvider.Instance.CreateId();
            procEsopEntity.CreatedBy = _currentUser.UserName;
            procEsopEntity.UpdatedBy = _currentUser.UserName;
            procEsopEntity.CreatedOn = HymsonClock.Now();
            procEsopEntity.UpdatedOn = HymsonClock.Now();
            procEsopEntity.SiteId = _currentSite.SiteId ?? 0;

            //入库
            await _procEsopRepository.InsertAsync(procEsopEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcEsopAsync(long id)
        {
            await _procEsopRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcEsopAsync(long[] ids)
        {
            return await _procEsopRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procEsopPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcEsopDto>> GetPagedListAsync(ProcEsopPagedQueryDto procEsopPagedQueryDto)
        {
            var procEsopPagedQuery = procEsopPagedQueryDto.ToQuery<ProcEsopPagedQuery>();
            var pagedInfo = await _procEsopRepository.GetPagedInfoAsync(procEsopPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcEsopDto> procEsopDtos = PrepareProcEsopDtos(pagedInfo);
            return new PagedInfo<ProcEsopDto>(procEsopDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcEsopDto> PrepareProcEsopDtos(PagedInfo<ProcEsopEntity>   pagedInfo)
        {
            var procEsopDtos = new List<ProcEsopDto>();
            foreach (var procEsopEntity in pagedInfo.Data)
            {
                var procEsopDto = procEsopEntity.ToModel<ProcEsopDto>();
                procEsopDtos.Add(procEsopDto);
            }

            return procEsopDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procEsopDto"></param>
        /// <returns></returns>
        public async Task ModifyProcEsopAsync(ProcEsopModifyDto procEsopModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procEsopModifyDto);

            //DTO转换实体
            var procEsopEntity = procEsopModifyDto.ToEntity<ProcEsopEntity>();
            procEsopEntity.UpdatedBy = _currentUser.UserName;
            procEsopEntity.UpdatedOn = HymsonClock.Now();

            await _procEsopRepository.UpdateAsync(procEsopEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcEsopDto> QueryProcEsopByIdAsync(long id) 
        {
           var procEsopEntity = await _procEsopRepository.GetByIdAsync(id);
           if (procEsopEntity != null) 
           {
               return procEsopEntity.ToModel<ProcEsopDto>();
           }
            return null;
        }
    }
}
