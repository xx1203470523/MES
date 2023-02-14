/*
 *creator: Karl
 *
 *describe: BOM表    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 10:04:25
 */
using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Extensions;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// BOM表 服务
    /// </summary>
    public class ProcBomService : IProcBomService
    {
        /// <summary>
        /// BOM表 仓储
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;
        private readonly AbstractValidator<ProcBomCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcBomModifyDto> _validationModifyRules;

        public ProcBomService(IProcBomRepository procBomRepository, AbstractValidator<ProcBomCreateDto> validationCreateRules, AbstractValidator<ProcBomModifyDto> validationModifyRules)
        {
            _procBomRepository = procBomRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procBomDto"></param>
        /// <returns></returns>
        public async Task CreateProcBomAsync(ProcBomCreateDto procBomCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procBomCreateDto);

            //DTO转换实体
            var procBomEntity = procBomCreateDto.ToEntity<ProcBomEntity>();
            procBomEntity.Id= IdGenProvider.Instance.CreateId();
            procBomEntity.CreatedBy = "TODO";
            procBomEntity.UpdatedBy = "TODO";
            procBomEntity.CreatedOn = HymsonClock.Now();
            procBomEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _procBomRepository.InsertAsync(procBomEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcBomAsync(long id)
        {
            await _procBomRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcBomAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            return await _procBomRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procBomPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcBomDto>> GetPageListAsync(ProcBomPagedQueryDto procBomPagedQueryDto)
        {
            procBomPagedQueryDto.SiteCode = "";//TODO

            var procBomPagedQuery = procBomPagedQueryDto.ToQuery<ProcBomPagedQuery>();
            var pagedInfo = await _procBomRepository.GetPagedInfoAsync(procBomPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcBomDto> procBomDtos = PrepareProcBomDtos(pagedInfo);
            return new PagedInfo<ProcBomDto>(procBomDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcBomDto> PrepareProcBomDtos(PagedInfo<ProcBomEntity>   pagedInfo)
        {
            var procBomDtos = new List<ProcBomDto>();
            foreach (var procBomEntity in pagedInfo.Data)
            {
                var procBomDto = procBomEntity.ToModel<ProcBomDto>();
                procBomDtos.Add(procBomDto);
            }

            return procBomDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procBomDto"></param>
        /// <returns></returns>
        public async Task ModifyProcBomAsync(ProcBomModifyDto procBomModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procBomModifyDto);

            //DTO转换实体
            var procBomEntity = procBomModifyDto.ToEntity<ProcBomEntity>();
            procBomEntity.UpdatedBy = "TODO";
            procBomEntity.UpdatedOn = HymsonClock.Now();

            await _procBomRepository.UpdateAsync(procBomEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcBomDto> QueryProcBomByIdAsync(long id) 
        {
           var procBomEntity = await _procBomRepository.GetByIdAsync(id);
           if (procBomEntity != null) 
           {
               return procBomEntity.ToModel<ProcBomDto>();
           }
            return null;
        }
    }
}
