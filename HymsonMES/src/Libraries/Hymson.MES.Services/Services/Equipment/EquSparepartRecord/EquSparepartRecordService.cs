/*
 *creator: Karl
 *
 *describe: 设备备件记录表    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:29:55
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquSparepartRecord;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquEquipmentRecord;
using Hymson.MES.Data.Repositories.EquSparepartRecord;
using Hymson.MES.Services.Dtos.EquSparepartRecord;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.EquSparepartRecord
{
    /// <summary>
    /// 设备备件记录表 服务
    /// </summary>
    public class EquSparepartRecordService : IEquSparepartRecordService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备备件记录表 仓储
        /// </summary>
        private readonly IEquSparepartRecordRepository _equSparepartRecordRepository;
        public EquSparepartRecordService(ICurrentUser currentUser, ICurrentSite currentSite, IEquSparepartRecordRepository equSparepartRecordRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equSparepartRecordRepository = equSparepartRecordRepository;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="equSparepartRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparepartRecordPagedViewDto>> GetPagedListAsync(EquSparepartRecordPagedQueryDto equSparepartRecordPagedQueryDto)
        {
            var equSparepartRecordPagedQuery = equSparepartRecordPagedQueryDto.ToQuery<EquSparepartRecordPagedQuery>();
            equSparepartRecordPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _equSparepartRecordRepository.GetPagedInfoAsync(equSparepartRecordPagedQuery);

            //实体到DTO转换 装载数据
            List<EquSparepartRecordPagedViewDto> equSparepartRecordDtos = PrepareEquSparepartRecordDtos(pagedInfo);
            return new PagedInfo<EquSparepartRecordPagedViewDto>(equSparepartRecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquSparepartRecordPagedViewDto> PrepareEquSparepartRecordDtos(PagedInfo<EquSparepartRecordPagedView> pagedInfo)
        {
            var equSparepartRecordDtos = new List<EquSparepartRecordPagedViewDto>();

            foreach (var equSparepartRecordEntity in pagedInfo.Data)
            {
                var equSparepartRecordDto = equSparepartRecordEntity.ToModel<EquSparepartRecordPagedViewDto>();
                if (equSparepartRecordDto.OperationType == EquOperationTypeEnum.Outbound)
                {
                    equSparepartRecordDto.WorkCenterCode = equSparepartRecordDto.RecordWorkCenterCode;
                }
                equSparepartRecordDtos.Add(equSparepartRecordDto);
            }

            return equSparepartRecordDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSparepartRecordDto> QueryEquSparepartRecordByIdAsync(long id)
        {
            var equSparepartRecordEntity = await _equSparepartRecordRepository.GetByIdAsync(id);
            if (equSparepartRecordEntity != null)
            {
                return equSparepartRecordEntity.ToModel<EquSparepartRecordDto>();
            }
            return null;
        }
    }
}
