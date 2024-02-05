/*
 *creator: Karl
 *
 *describe: 降级品录入记录    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:49
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 降级品录入记录 服务
    /// </summary>
    public class ManuDowngradingRecordService : IManuDowngradingRecordService
    {
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 降级品录入记录 仓储
        /// </summary>
        private readonly IManuDowngradingRecordRepository _manuDowngradingRecordRepository;

        public ManuDowngradingRecordService(ICurrentSite currentSite, IManuDowngradingRecordRepository manuDowngradingRecordRepository)
        {
            _currentSite = currentSite;
            _manuDowngradingRecordRepository = manuDowngradingRecordRepository;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuDowngradingRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuDowngradingRecordDto>> GetPagedListAsync(ManuDowngradingRecordPagedQueryDto manuDowngradingRecordPagedQueryDto)
        {
            var manuDowngradingRecordPagedQuery = manuDowngradingRecordPagedQueryDto.ToQuery<ManuDowngradingRecordPagedQuery>();
            manuDowngradingRecordPagedQuery.SiteId = _currentSite.SiteId??0;
            var pagedInfo = await _manuDowngradingRecordRepository.GetPagedInfoAsync(manuDowngradingRecordPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuDowngradingRecordDto> manuDowngradingRecordDtos = PrepareManuDowngradingRecordDtos(pagedInfo);
            return new PagedInfo<ManuDowngradingRecordDto>(manuDowngradingRecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuDowngradingRecordDto> PrepareManuDowngradingRecordDtos(PagedInfo<ManuDowngradingRecordEntity>   pagedInfo)
        {
            var manuDowngradingRecordDtos = new List<ManuDowngradingRecordDto>();
            foreach (var manuDowngradingRecordEntity in pagedInfo.Data)
            {
                var manuDowngradingRecordDto = manuDowngradingRecordEntity.ToModel<ManuDowngradingRecordDto>();
                manuDowngradingRecordDtos.Add(manuDowngradingRecordDto);
            }

            return manuDowngradingRecordDtos;
        }
    }
}
