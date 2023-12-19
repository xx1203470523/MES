using Hymson.Print.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process.LabelTemplate.Utility.Execute
{
    /// <summary>
    /// 获取打印数据库原
    /// </summary>
    public class ExecuteLabelTemplateDataSourceService : IExecuteLabelTemplateDataSourceService
    {
        private readonly IPrintExecuteTaskRepository _printExecuteTaskRepository;

        /// <summary>
        /// 
        /// </summary>
        public ExecuteLabelTemplateDataSourceService(IPrintExecuteTaskRepository printExecuteTaskRepository)
        {
            _printExecuteTaskRepository= printExecuteTaskRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Execute()
        {

        }
    }
}
