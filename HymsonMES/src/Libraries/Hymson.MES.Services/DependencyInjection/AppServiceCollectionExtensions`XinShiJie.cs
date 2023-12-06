using FluentValidation;
using Hymson.MES.Services.Dtos.Equipment;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Plan;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.MES.Services.Dtos.Warehouse;
using Hymson.MES.Services.Services.EquEquipmentGroup;
using Hymson.MES.Services.Services.Equipment;
using Hymson.MES.Services.Services.Equipment.EquEquipment;
using Hymson.MES.Services.Services.Equipment.EquEquipmentUnit;
using Hymson.MES.Services.Services.Equipment.EquFaultPhenomenon;
using Hymson.MES.Services.Services.Equipment.EquSparePart;
using Hymson.MES.Services.Services.Equipment.EquSparePartType;
using Hymson.MES.Services.Services.Integrated;
using Hymson.MES.Services.Services.Integrated.IIntegratedService;
using Hymson.MES.Services.Services.Integrated.InteCalendar;
using Hymson.MES.Services.Services.Integrated.InteClass;
using Hymson.MES.Services.Services.Integrated.InteContainer;
using Hymson.MES.Services.Services.Manufacture;
using Hymson.MES.Services.Services.Manufacture.ManuFeeding;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuOutStation;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuPackage;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.OutStation;
using Hymson.MES.Services.Services.Manufacture.ManuSfc;
using Hymson.MES.Services.Services.Manufacture.ManuSfcProduce;
using Hymson.MES.Services.Services.Plan;
using Hymson.MES.Services.Services.Process;
using Hymson.MES.Services.Services.Process.LabelTemplate;
using Hymson.MES.Services.Services.Process.MaskCode;
using Hymson.MES.Services.Services.Process.PrintConfig;
using Hymson.MES.Services.Services.Process.Procedure;
using Hymson.MES.Services.Services.Process.ProcessRoute;
using Hymson.MES.Services.Services.Process.Resource;
using Hymson.MES.Services.Services.Process.ResourceType;
using Hymson.MES.Services.Services.Quality;
using Hymson.MES.Services.Services.Quality.QualUnqualifiedCode;
using Hymson.MES.Services.Services.Quality.QualUnqualifiedGroup;
using Hymson.MES.Services.Services.Report;
using Hymson.MES.Services.Services.Warehouse;
using Hymson.MES.Services.Validators.Equipment;
using Hymson.MES.Services.Validators.Integrated;
using Hymson.MES.Services.Validators.Manufacture;
using Hymson.MES.Services.Validators.Plan;
using Hymson.MES.Services.Validators.Process;
using Hymson.MES.Services.Validators.Quality;
using Hymson.MES.Services.Validators.Warehouse;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class AppServiceCollectionExtensions
    {
        /// <summary>
        /// 添加服务依赖
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddServicesForXinShiJie(this IServiceCollection services)
        {
            return services;
        }

        /// <summary>
        /// 添加验证器相关服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddValidatorsForXinShiJie(IServiceCollection services)
        {
            return services;
        }

    }
}
