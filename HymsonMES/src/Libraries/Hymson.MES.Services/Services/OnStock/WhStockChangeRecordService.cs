using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.OnStock;
using Hymson.MES.Data.Repositories.OnStock;
using Hymson.MES.Services.Dtos.OnStock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.OnStock
{
    public class WhStockChangeRecordService: IWhStockChangeRecordService
    {
        private readonly IWhStockChangeRecordRepository _whStockChangeRecordRepository;
        private readonly AbstractValidator<WhStockChangeRecordDto> _validationRules;

        public WhStockChangeRecordService(IWhStockChangeRecordRepository whStockChangeRecordRepository, AbstractValidator<WhStockChangeRecordDto> validationRules)
        {
            _whStockChangeRecordRepository = whStockChangeRecordRepository;
            _validationRules = validationRules;
        }

        public async Task CreateWhStockChangeRecordAsync(WhStockChangeRecordDto whStockChangeRecordDto)
        {
            //验证DTO
            await _validationRules.ValidateAndThrowAsync(whStockChangeRecordDto);
            //DTO转换实体
            var whStockChangeRecordEntity = whStockChangeRecordDto.ToEntity<WhStockChangeRecordEntity>();
            whStockChangeRecordEntity.CreatedBy = "jinyi";
            whStockChangeRecordEntity.UpdatedBy = "jinyi";
            //入库
            await _whStockChangeRecordRepository.InsertAsync(whStockChangeRecordEntity);

        }

        public Task DeleteWhStockChangeRecordAsync(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="whStockChangeRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<WhStockChangeRecordDto>> GetListAsync(WhStockChangeRecordPagedQueryDto whStockChangeRecordPagedQueryDto)
        {
            var whStockChangeRecordPagedQuery = whStockChangeRecordPagedQueryDto.ToQuery<WhStockChangeRecordPagedQuery>();
            var pagedInfo = await _whStockChangeRecordRepository.GetPagedInfoAsync(whStockChangeRecordPagedQuery);

            //实体到DTO转换 装载数据
            List<WhStockChangeRecordDto> whStockChangeRecordDtos = PrepareWhStockChangeRecordDtos(pagedInfo);
            return new PagedInfo<WhStockChangeRecordDto>(whStockChangeRecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        private static List<WhStockChangeRecordDto> PrepareWhStockChangeRecordDtos(PagedInfo<WhStockChangeRecordEntity> pagedInfo)
        {
            var whStockChangeRecordDtos = new List<WhStockChangeRecordDto>();
            foreach (var whStockChangeRecordEntity in pagedInfo.Data)
            {
                var whStockChangeRecordDto = whStockChangeRecordEntity.ToModel<WhStockChangeRecordDto>();
                whStockChangeRecordDtos.Add(whStockChangeRecordDto);
            }

            return whStockChangeRecordDtos;
        }

        public Task ModifyWhStockChangeRecordAsync(WhStockChangeRecordDto whStockChangeRecordDto)
        {
            throw new NotImplementedException();
        }
    }
}
