/*
 *creator: Karl
 *
 *describe: 条码生产信息（物理删除）    服务 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-18 05:37:27
 */
using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 条码生产信息（物理删除） 服务
    /// </summary>
    public class ManuSfcProduceService : IManuSfcProduceService
    {
        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly AbstractValidator<ManuSfcProduceCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuSfcProduceModifyDto> _validationModifyRules;

        public ManuSfcProduceService(IManuSfcProduceRepository manuSfcProduceRepository, AbstractValidator<ManuSfcProduceCreateDto> validationCreateRules, AbstractValidator<ManuSfcProduceModifyDto> validationModifyRules)
        {
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuSfcProduceDto"></param>
        /// <returns></returns>
        public async Task CreateManuSfcProduceAsync(ManuSfcProduceCreateDto manuSfcProduceCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuSfcProduceCreateDto);

            //DTO转换实体
            var manuSfcProduceEntity = manuSfcProduceCreateDto.ToEntity<ManuSfcProduceEntity>();
            manuSfcProduceEntity.Id= IdGenProvider.Instance.CreateId();
            manuSfcProduceEntity.CreatedBy = "TODO";
            manuSfcProduceEntity.UpdatedBy = "TODO";
            manuSfcProduceEntity.CreatedOn = HymsonClock.Now();
            manuSfcProduceEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _manuSfcProduceRepository.InsertAsync(manuSfcProduceEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuSfcProduceAsync(long id)
        {
            await _manuSfcProduceRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuSfcProduceAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            return await _manuSfcProduceRepository.DeleteRangeAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuSfcProducePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcProduceDto>> GetPageListAsync(ManuSfcProducePagedQueryDto manuSfcProducePagedQueryDto)
        {
            var manuSfcProducePagedQuery = manuSfcProducePagedQueryDto.ToQuery<ManuSfcProducePagedQuery>();
            var pagedInfo = await _manuSfcProduceRepository.GetPagedInfoAsync(manuSfcProducePagedQuery);

            //实体到DTO转换 装载数据
            List<ManuSfcProduceDto> manuSfcProduceDtos = PrepareManuSfcProduceDtos(pagedInfo);
            return new PagedInfo<ManuSfcProduceDto>(manuSfcProduceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuSfcProduceDto> PrepareManuSfcProduceDtos(PagedInfo<ManuSfcProduceEntity>   pagedInfo)
        {
            var manuSfcProduceDtos = new List<ManuSfcProduceDto>();
            foreach (var manuSfcProduceEntity in pagedInfo.Data)
            {
                var manuSfcProduceDto = manuSfcProduceEntity.ToModel<ManuSfcProduceDto>();
                manuSfcProduceDtos.Add(manuSfcProduceDto);
            }

            return manuSfcProduceDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuSfcProduceDto"></param>
        /// <returns></returns>
        public async Task ModifyManuSfcProduceAsync(ManuSfcProduceModifyDto manuSfcProduceModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuSfcProduceModifyDto);

            //DTO转换实体
            var manuSfcProduceEntity = manuSfcProduceModifyDto.ToEntity<ManuSfcProduceEntity>();
            manuSfcProduceEntity.UpdatedBy = "TODO";
            manuSfcProduceEntity.UpdatedOn = HymsonClock.Now();

            await _manuSfcProduceRepository.UpdateAsync(manuSfcProduceEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcProduceDto> QueryManuSfcProduceByIdAsync(long id) 
        {
           var manuSfcProduceEntity = await _manuSfcProduceRepository.GetByIdAsync(id);
           if (manuSfcProduceEntity != null) 
           {
               return manuSfcProduceEntity.ToModel<ManuSfcProduceDto>();
           }
            return null;
        }
    }
}
