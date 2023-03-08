using Hymson.Infrastructure;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Services.Process.IProcessService
{
    /// <summary>
    /// 打印配置接口service
    /// </summary>
    public interface IProcPrintConfigService
    {
        /// <summary>
        /// 查询资源类型维护表详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcPrinterDto> GetByIdAsync(long id);
        Task<ProcPrinterDto> GetByPrintNameAsync(string printName);
        /// <summary>
        /// 查询资源维护表列表(关联资源类型，展示资源类型名称)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcPrinterDto>> GetPageListAsync(ProcPrinterPagedQueryDto query);

        /// <summary>
        /// 获取资源分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedInfo<ProcPrinterDto>> GetListAsync(ProcPrinterPagedQueryDto query);

       
        /// <summary>
        /// 添加资源数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddProcPrintConfigAsync(ProcPrinterDto param);

        /// <summary>
        /// 修改资源数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateProcPrintConfigAsync(ProcPrinterUpdateDto param);

        /// <summary>
        /// 批量删除资源类型数据
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        Task<int> DeleteProcPrintConfigAsync(long[] idsAr);
    }
}
