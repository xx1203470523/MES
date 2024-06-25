using AutoMapper;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Domain.WHMaterialReceipt;
using Hymson.MES.Core.Domain.WHMaterialReceiptDetail;
using Hymson.MES.SystemServices.Dtos;

namespace Hymson.MES.SystemServices.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    public class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        /// <summary>
        /// 映射
        /// </summary>
        public MapperConfiguration()
        {
            CreateProcessMaps();
            CreateQualityMaps();
            CreateWarehouseMaps();
            CreatePlanMaps();
            CreateManufactureMaps();
        }


        /// <summary>
        /// 工艺模块
        /// </summary>
        protected virtual void CreateProcessMaps()
        {
            // TODO: Add mappings for Process module
        }

        /// <summary>
        /// 库存
        /// </summary>
        protected virtual void CreateWarehouseMaps()
        {
            // TODO: Add mappings for Warehouse module
        }

        /// <summary>
        /// 质量模块
        /// </summary>
        protected virtual void CreateQualityMaps()
        {
            CreateMap<WhMaterialReceiptDto, WhMaterialReceiptEntity>();
            CreateMap<WhMaterialReceiptMaterialDto, WHMaterialReceiptDetailEntity>();
        }

        /// <summary>
        /// 生产模块
        /// </summary>
        protected virtual void CreateManufactureMaps()
        {
            // TODO: Add mappings for Manufacture module
        }

        /// <summary>
        /// 计划模块
        /// </summary>
        protected virtual void CreatePlanMaps()
        {
            // TODO: Add mappings for Plan module
        }

        /// <summary>
        /// 排序，决定了加载的顺序
        /// </summary>
        public int Order => 0;

    }
}
