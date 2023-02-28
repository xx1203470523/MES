/*
 *creator: Karl
 *
 *describe: BOM明细表    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 10:38:06
 */
using FluentValidation;
using Hymson.Authentication;
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
using Hymson.Utils;
using Org.BouncyCastle.Crypto;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// BOM明细表 服务
    /// </summary>
    public class ProcBomDetailService : IProcBomDetailService
    {
        /// <summary>
        /// BOM明细表 仓储
        /// </summary>
        private readonly IProcBomDetailRepository _procBomDetailRepository;
        private readonly AbstractValidator<ProcBomDetailCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcBomDetailModifyDto> _validationModifyRules;
        private readonly ICurrentUser _currentUser;

        public ProcBomDetailService(ICurrentUser currentUser, IProcBomDetailRepository procBomDetailRepository, AbstractValidator<ProcBomDetailCreateDto> validationCreateRules, AbstractValidator<ProcBomDetailModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _procBomDetailRepository = procBomDetailRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procBomDetailDto"></param>
        /// <returns></returns>
        public async Task CreateProcBomDetailAsync(ProcBomDetailCreateDto procBomDetailCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procBomDetailCreateDto);

            //DTO转换实体
            var procBomDetailEntity = procBomDetailCreateDto.ToEntity<ProcBomDetailEntity>();
            procBomDetailEntity.Id= IdGenProvider.Instance.CreateId();
            procBomDetailEntity.CreatedBy = _currentUser.UserName;
            procBomDetailEntity.UpdatedBy = _currentUser.UserName;
            procBomDetailEntity.CreatedOn = HymsonClock.Now();
            procBomDetailEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _procBomDetailRepository.InsertAsync(procBomDetailEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcBomDetailAsync(long id)
        {
            await _procBomDetailRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcBomDetailAsync(long[] ids)
        {
            return await _procBomDetailRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procBomDetailPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcBomDetailDto>> GetPageListAsync(ProcBomDetailPagedQueryDto procBomDetailPagedQueryDto)
        {
            var procBomDetailPagedQuery = procBomDetailPagedQueryDto.ToQuery<ProcBomDetailPagedQuery>();
            var pagedInfo = await _procBomDetailRepository.GetPagedInfoAsync(procBomDetailPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcBomDetailDto> procBomDetailDtos = PrepareProcBomDetailDtos(pagedInfo);
            return new PagedInfo<ProcBomDetailDto>(procBomDetailDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcBomDetailDto> PrepareProcBomDetailDtos(PagedInfo<ProcBomDetailEntity>   pagedInfo)
        {
            var procBomDetailDtos = new List<ProcBomDetailDto>();
            foreach (var procBomDetailEntity in pagedInfo.Data)
            {
                var procBomDetailDto = procBomDetailEntity.ToModel<ProcBomDetailDto>();
                procBomDetailDtos.Add(procBomDetailDto);
            }

            return procBomDetailDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procBomDetailDto"></param>
        /// <returns></returns>
        public async Task ModifyProcBomDetailAsync(ProcBomDetailModifyDto procBomDetailModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procBomDetailModifyDto);

            //DTO转换实体
            var procBomDetailEntity = procBomDetailModifyDto.ToEntity<ProcBomDetailEntity>();
            procBomDetailEntity.UpdatedBy = _currentUser.UserName;
            procBomDetailEntity.UpdatedOn = HymsonClock.Now();

            await _procBomDetailRepository.UpdateAsync(procBomDetailEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcBomDetailDto> QueryProcBomDetailByIdAsync(long id) 
        {
           var procBomDetailEntity = await _procBomDetailRepository.GetByIdAsync(id);
           if (procBomDetailEntity != null) 
           {
               return procBomDetailEntity.ToModel<ProcBomDetailDto>();
           }
            return null;
        }
    }
}
