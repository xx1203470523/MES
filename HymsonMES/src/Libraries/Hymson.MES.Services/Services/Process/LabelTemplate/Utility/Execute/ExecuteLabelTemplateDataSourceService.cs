using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.CoreServices.Services.Job;
using Hymson.MES.Services.Dtos.Process.LabelTemplate.DataSource;
using Hymson.MES.Services.Dtos.Process.LabelTemplate.Utility;
using Hymson.MES.Services.Services.Process.LabelTemplate.DataSource;
using Hymson.Print.DataService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process.LabelTemplate.Utility.Execute
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

        private readonly IPrintExecuteTaskRepository _printExecuteTaskRepository;

        /// <summary>
        /// 
        /// </summary>
        public ExecuteLabelTemplateDataSourceService(IServiceProvider serviceProvider, IPrintExecuteTaskRepository printExecuteTaskRepository)
        {
            _printExecuteTaskRepository = printExecuteTaskRepository;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<string> Execute(LabelTemplateSourceDto param)
        {
            var services = _serviceProvider.GetServices<IBarcodeDataSourceService>();

            var service = services.FirstOrDefault(x => x.GetType().Name == param.DataSourceName);

            if (service == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10378)).WithData("DataSourceName", param.DataSourceName);
            }

            var baseLabelTemplateDataDto = new BaseLabelTemplateDataDto
            {
                BarCodes = param.BarCodes,
                SiteId = param.SiteId
            };

            return await service.GetLabelTemplateData(baseLabelTemplateDataDto);
        }
    }
}
