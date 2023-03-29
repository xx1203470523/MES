using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Equipment.EquFaultPhenomenon;
using Hymson.MES.Data.Repositories.Equipment.EquFaultPhenomenon.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Equipment.EquFaultPhenomenon
{
    /// <summary>
    /// 服务（设备故障现象）
    /// </summary>
    public class EquFaultPhenomenonService : IEquFaultPhenomenonService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 仓储（设备故障现象）
        /// </summary>
        private readonly IEquFaultPhenomenonRepository _equFaultPhenomenonRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="currentUser"></param>
        /// <param name="equFaultPhenomenonRepository"></param>
        public EquFaultPhenomenonService(ICurrentUser currentUser, ICurrentSite currentSite,
            IEquFaultPhenomenonRepository equFaultPhenomenonRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _equFaultPhenomenonRepository = equFaultPhenomenonRepository;
        }


        /// <summary>
        /// 添加（设备故障现象）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(EquFaultPhenomenonSaveDto createDto)
        {
            // 验证DTO


            // DTO转换实体
            var entity = createDto.ToEntity<EquFaultPhenomenonEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = _currentUser.UserName;
            entity.UpdatedBy = _currentUser.UserName;
            entity.SiteId = _currentSite.SiteId;

            // 判断编号是否已存在
            var isExists = await _equFaultPhenomenonRepository.IsExistsAsync(entity.FaultPhenomenonCode);
            if (isExists == true)
            {
                // TODO 返回值
                return -1;
                //responseDto.Msg = $"此编码{model.FaultPhenomenonCode}在系统已经存在！";
                //return responseDto;
            }

            // 保存实体
            try
            {
                return await _equFaultPhenomenonRepository.InsertAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
            return 0;
        }

        /// <summary>
        /// 修改（设备故障现象）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyAsync(EquFaultPhenomenonSaveDto modifyDto)
        {
            // 验证DTO


            // DTO转换实体
            var entity = modifyDto.ToEntity<EquFaultPhenomenonEntity>();
            entity.UpdatedBy = _currentUser.UserName;

            return await _equFaultPhenomenonRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除（设备故障现象）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(long[] idsArr)
        {
            return await _equFaultPhenomenonRepository.DeletesAsync(new DeleteCommand
            {
                Ids = idsArr,
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now()
            });
        }

        /// <summary>
        /// 查询列表（设备故障现象）
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquFaultPhenomenonDto>> GetPagedListAsync(EquFaultPhenomenonPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<EquFaultPhenomenonPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _equFaultPhenomenonRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquFaultPhenomenonDto>());
            return new PagedInfo<EquFaultPhenomenonDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（设备故障现象）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquFaultPhenomenonDto> GetDetailAsync(long id)
        {
            return (await _equFaultPhenomenonRepository.GetViewByIdAsync(id)).ToModel<EquFaultPhenomenonDto>();
        }


    }
}
