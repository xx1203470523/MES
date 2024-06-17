/*
 *creator: Karl
 *
 *describe: 设备台账信息    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-06-12 10:53:50
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.EquEquipmentRecord;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.EquEquipmentRecord;
using Hymson.MES.Services.Dtos.EquEquipmentRecord;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.EquEquipmentRecord
{
    /// <summary>
    /// 设备台账信息 服务
    /// </summary>
    public class EquEquipmentRecordService : IEquEquipmentRecordService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备台账信息 仓储
        /// </summary>
        private readonly IEquEquipmentRecordRepository _equEquipmentRecordRepository;
        public EquEquipmentRecordService(ICurrentUser currentUser, ICurrentSite currentSite, IEquEquipmentRecordRepository equEquipmentRecordRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equEquipmentRecordRepository = equEquipmentRecordRepository;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="equEquipmentRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquEquipmentRecordDto>> GetPagedListAsync(EquEquipmentRecordPagedQueryDto equEquipmentRecordPagedQueryDto)
        {
            var equEquipmentRecordPagedQuery = equEquipmentRecordPagedQueryDto.ToQuery<EquEquipmentRecordPagedQuery>();
            var pagedInfo = await _equEquipmentRecordRepository.GetPagedInfoAsync(equEquipmentRecordPagedQuery);

            //实体到DTO转换 装载数据
            List<EquEquipmentRecordDto> equEquipmentRecordDtos = PrepareEquEquipmentRecordDtos(pagedInfo);
            return new PagedInfo<EquEquipmentRecordDto>(equEquipmentRecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<EquEquipmentRecordDto> PrepareEquEquipmentRecordDtos(PagedInfo<EquEquipmentRecordEntity> pagedInfo)
        {
            var equEquipmentRecordDtos = new List<EquEquipmentRecordDto>();
            foreach (var equEquipmentRecordEntity in pagedInfo.Data)
            {
                var equEquipmentRecordDto = equEquipmentRecordEntity.ToModel<EquEquipmentRecordDto>();
                equEquipmentRecordDtos.Add(equEquipmentRecordDto);
            }

            return equEquipmentRecordDtos;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquEquipmentRecordDto> QueryEquEquipmentRecordByIdAsync(long id)
        {
            var equEquipmentRecordEntity = await _equEquipmentRecordRepository.GetByIdAsync(id);
            if (equEquipmentRecordEntity != null)
            {
                return equEquipmentRecordEntity.ToModel<EquEquipmentRecordDto>();
            }
            return null;
        }
    }
}
