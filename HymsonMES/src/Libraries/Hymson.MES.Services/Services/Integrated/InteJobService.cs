/*
 *creator: Karl
 *
 *describe: 作业表    服务 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 04:32:34
 */
using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 作业表 服务
    /// </summary>
    public class InteJobService : IInteJobService
    {
        /// <summary>
        /// 作业表 仓储
        /// </summary>
        private readonly IInteJobRepository _inteJobRepository;
        private readonly AbstractValidator<InteJobCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteJobModifyDto> _validationModifyRules;

        public InteJobService(IInteJobRepository inteJobRepository, AbstractValidator<InteJobCreateDto> validationCreateRules, AbstractValidator<InteJobModifyDto> validationModifyRules)
        {
            _inteJobRepository = inteJobRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="inteJobDto"></param>
        /// <returns></returns>
        public async Task CreateInteJobAsync(InteJobCreateDto inteJobCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(inteJobCreateDto);

            //DTO转换实体
            var inteJobEntity = inteJobCreateDto.ToEntity<InteJobEntity>();
            inteJobEntity.Id= IdGenProvider.Instance.CreateId();
            inteJobEntity.CreatedBy = "TODO";
            inteJobEntity.UpdatedBy = "TODO";
            inteJobEntity.CreatedOn = HymsonClock.Now();
            inteJobEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _inteJobRepository.InsertAsync(inteJobEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteInteJobAsync(long id)
        {
            await _inteJobRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteJobAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            return await _inteJobRepository.DeleteRangeAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="inteJobPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteJobDto>> GetPageListAsync(InteJobPagedQueryDto inteJobPagedQueryDto)
        {
            var inteJobPagedQuery = inteJobPagedQueryDto.ToQuery<InteJobPagedQuery>();
            var pagedInfo = await _inteJobRepository.GetPagedInfoAsync(inteJobPagedQuery);

            //实体到DTO转换 装载数据
            List<InteJobDto> inteJobDtos = PrepareInteJobDtos(pagedInfo);
            return new PagedInfo<InteJobDto>(inteJobDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteJobDto> PrepareInteJobDtos(PagedInfo<InteJobEntity>   pagedInfo)
        {
            var inteJobDtos = new List<InteJobDto>();
            foreach (var inteJobEntity in pagedInfo.Data)
            {
                var inteJobDto = inteJobEntity.ToModel<InteJobDto>();
                inteJobDtos.Add(inteJobDto);
            }

            return inteJobDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteJobDto"></param>
        /// <returns></returns>
        public async Task ModifyInteJobAsync(InteJobModifyDto inteJobModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(inteJobModifyDto);

            //DTO转换实体
            var inteJobEntity = inteJobModifyDto.ToEntity<InteJobEntity>();
            inteJobEntity.UpdatedBy = "TODO";
            inteJobEntity.UpdatedOn = HymsonClock.Now();

            await _inteJobRepository.UpdateAsync(inteJobEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteJobDto> QueryInteJobByIdAsync(long id) 
        {
           var inteJobEntity = await _inteJobRepository.GetByIdAsync(id);
           if (inteJobEntity != null) 
           {
               return inteJobEntity.ToModel<InteJobDto>();
           }
            return null;
        }
    }
}
