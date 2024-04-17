using Hymson.Infrastructure.Exceptions;
using Hymson.MES.BackgroundServices.Dtos.Manufacture.LabelTemplate;
using Hymson.MES.BackgroundServices.Manufacture.PrintTemplate.ProductionBarcode;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Process;
using Hymson.Print.Abstractions;
using Hymson.Print.DataService;
using Microsoft.Extensions.DependencyInjection;

namespace Hymson.MES.BackgroundServices.Manufacture.PrintTemplate.Utility.Execute
{
    /// <summary>
    /// 获取打印数据源
    /// </summary>
    public class ExecuteLabelTemplateDataSourceService : IExecuteLabelTemplateDataSourceService
    {
        /// <summary>
        /// 注入反射获取依赖对象
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 打印仓储
        /// </summary>
        private readonly IPrintService _printService;

        /// <summary>
        /// 资源关联打印机仓储
        /// </summary>
        private readonly IProcResourceConfigPrintRepository _resourceConfigPrintRepository;

        /// <summary>
        /// 工序配置打印表仓储
        /// </summary>
        private readonly IProcProcedurePrintRelationRepository _procedurePrintRelationRepository;

        /// <summary>
        /// 标签表仓储
        /// </summary>
        private readonly IProcLabelTemplateRepository _procLabelTemplateRepository;

        /// <summary>
        /// 打印配置仓储
        /// </summary>
        private readonly IProcPrintConfigRepository _printConfigRepository;

        /// <summary>
        /// 
        /// </summary>
        public ExecuteLabelTemplateDataSourceService(IServiceProvider serviceProvider,
            IPrintService printService,
            IProcResourceConfigPrintRepository procResourceConfigPrintRepository,
            IProcProcedurePrintRelationRepository procProcedurePrintRelationRepository,
            IProcLabelTemplateRepository procLabelTemplateRepository,
            IProcPrintConfigRepository procPrintConfigRepository)
        {
            _printService = printService;
            _serviceProvider = serviceProvider;
            _resourceConfigPrintRepository = procResourceConfigPrintRepository;
            _procedurePrintRelationRepository = procProcedurePrintRelationRepository;
            _procLabelTemplateRepository = procLabelTemplateRepository;
            _printConfigRepository = procPrintConfigRepository;
        }
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task Execute(LabelTemplateSourceDto param)
        {
            var procResourceConfigPrintEnties = await _resourceConfigPrintRepository.GetByResourceIdAsync(param.ResourceId);
            var materiaIds = param.BarCodes.Select(x => x.MateriaId).Distinct();
            var procProcedurePrintReleationEnties = await _procedurePrintRelationRepository.GetProcProcedurePrintReleationEntitiesAsync(new ProcProcedurePrintReleationQuery
            {
                SiteId = param.SiteId,
                ProcedureId = param.ProcedureId,
                MaterialIds = materiaIds
            });
            var printConfigEnties = await _printConfigRepository.GetByIdsAsync(procResourceConfigPrintEnties.Select(x => x.PrintId));
            var procLabelTemplateEnties = await _procLabelTemplateRepository.GetByIdsAsync(procProcedurePrintReleationEnties.Select(x => x.TemplateId).Distinct());

            var printExecuteTaskEnties = new List<PrintExecuteTaskEntity>();

            var groups = param.BarCodes.GroupBy(x => x.MateriaId);
            foreach (var groupItem in groups)
            {
                var procProcedurePrintReleationByMaterialIdEnties = procProcedurePrintReleationEnties.Where(x => x.MaterialId == groupItem.Key);
                foreach (var procResourceConfigPrint in procResourceConfigPrintEnties)
                {
                    var printConfigEntity = printConfigEnties.FirstOrDefault(x => x.Id == procResourceConfigPrint.PrintId);
                    foreach (var procProcedurePrintReleation in procProcedurePrintReleationByMaterialIdEnties)
                    {
                        var rocLabelTemplateEntity = procLabelTemplateEnties.FirstOrDefault(x => x.Id == procProcedurePrintReleation.TemplateId);
                        string className = "Hymson.MES.Services.Dtos.Process.LabelTemplate.DataSource.ProductionBarcodeDto";
                        Type? dtoType = Type.GetType(className);
                        Type serviceType = typeof(IBarcodeDataSourceService<>).MakeGenericType(dtoType!);
                        var services = _serviceProvider.GetServices(serviceType);
                        var service = services.FirstOrDefault();
                        if (service == null)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES10378)).WithData("DataSourceName", "");
                        }

                        var baseLabelTemplateDataDto = new BaseLabelTemplateDataDto
                        {
                            BarCodes = groupItem.Select(x => x.BarCode).Distinct(),
                            TemplateName = rocLabelTemplateEntity!.Name,
                            PrintName = printConfigEntity!.PrintName,
                            PrintCount = Convert.ToInt16(procProcedurePrintReleation.Copy),
                            SiteId = param.SiteId
                        };

                        // 获取 GetLabelTemplateData 方法的 MethodInfo
                        var getLabelTemplateDataMethod = service.GetType().GetMethod("GetLabelTemplateDataAsync");
                        object[] args = new object[] { baseLabelTemplateDataDto };

                        // 调用 GetLabelTemplateData 方法
                        var result = getLabelTemplateDataMethod?.Invoke(service, args);

                        var getAwaiterMethod = result!.GetType()!.GetMethod("GetAwaiter", Type.EmptyTypes);
                        var getAwaiterResult = getAwaiterMethod!.Invoke(result, null);

                        var getResultMethod = getAwaiterResult!.GetType()!.GetMethod("GetResult", Type.EmptyTypes);
                        var getResultResult = getResultMethod!.Invoke(getAwaiterResult, null);

                        //TODO 本次调用不太严谨 
                        var addTaskMethod = _printService.GetType().GetMethods().
                            FirstOrDefault(x => x.Name == "AddTaskAsync" && x.IsGenericMethod && x.GetParameters().Length == 2);

                        var specificAddTaskMethod = addTaskMethod!.MakeGenericMethod(new Type[] { dtoType! });

                        object[] addTaskArgs = new object[] { getResultResult!, param.UserName };
        
                        var rowTask = (Task<int>)specificAddTaskMethod!.Invoke(_printService!, addTaskArgs)!;

                        var row = await rowTask!;
                    }
                }
            }
        }
    }
}
