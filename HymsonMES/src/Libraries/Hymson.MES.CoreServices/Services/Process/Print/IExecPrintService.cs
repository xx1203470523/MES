using Hymson.MES.CoreServices.Events.ProcessEvents.PrintEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Services.Process.Print
{
    /// <summary>
    /// 执行打印
    /// </summary>
    public interface IExecPrintService
    {
        Task PrintAsync(PrintIntegrationEvent @event);
    }
}
