using Hymson.Infrastructure;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart;
using Hymson.MES.Data.Repositories.Equipment.EquSparePart.Query;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.Snowflake;
using Hymson.Utils;
using static Dapper.SqlMapper;

namespace Hymson.MES.Services.Services.Equipment.EquSparePart
{
    /// <summary>
    /// 业务处理层（备件注册） 
    /// </summary>
    public class EquSparePartService : IEquSparePartService
    {
        /// <summary>
        /// 仓储（备件注册） 
        /// </summary>
        private readonly IEquSparePartRepository _equSparePartRepository;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="equSparePartRepository"></param>
        public EquSparePartService(IEquSparePartRepository equSparePartRepository)
        {
            _equSparePartRepository = equSparePartRepository;
        }

        /// <summary>
        /// 添加（备件注册）
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task<int> CreateEquSparePartAsync(EquSparePartCreateDto createDto)
        {
            //验证DTO


            //DTO转换实体
            var entity = createDto.ToEntity<EquSparePartEntity>();
            entity.Id = IdGenProvider.Instance.CreateId();
            entity.CreatedBy = "TODO";
            entity.UpdatedBy = "TODO";

            //入库
            return await _equSparePartRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改（备件注册）
        /// </summary>
        /// <param name="modifyDto"></param>
        /// <returns></returns>
        public async Task<int> ModifyEquSparePartAsync(EquSparePartModifyDto modifyDto)
        {
            //验证DTO


            //DTO转换实体
            var entity = modifyDto.ToEntity<EquSparePartEntity>();
            entity.UpdatedBy = "TODO";

            return await _equSparePartRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 删除（备件注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteEquSparePartAsync(long id)
        {
            return await _equSparePartRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除（备件注册）
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesEquSparePartAsync(long[] idsArr)
        {
            return await _equSparePartRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 分页查询列表（备件注册）
        /// </summary>
        /// <param name="equSparePartPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<EquSparePartDto>> GetPageListAsync(EquSparePartPagedQueryDto equSparePartPagedQueryDto)
        {
            var equSparePartPagedQuery = equSparePartPagedQueryDto.ToQuery<EquSparePartPagedQuery>();
            var pagedInfo = await _equSparePartRepository.GetPagedInfoAsync(equSparePartPagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<EquSparePartDto>());
            return new PagedInfo<EquSparePartDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 查询详情（备件注册）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EquSparePartDto> QueryEquSparePartByIdAsync(long id)
        {
            return (await _equSparePartRepository.GetByIdAsync(id)).ToModel<EquSparePartDto>();
        }

    }
}
