using Hymson.MES.BackgroundServices.Rotor.Dtos.Manu;
using Hymson.MES.BackgroundServices.Rotor.Dtos.Stator;
using Hymson.MES.Core.Domain.Mavel.Stator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.BackgroundServices.Rotor.Repositories
{
    /// <summary>
    /// 装箱仓储
    /// </summary>
    public interface IPackListRepository
    {
        /// <summary>
        /// 获取过站数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<PackListDto>> GetList(string sql);

        /// <summary>
        /// 获取定子线组装好的数据
        /// </summary>
        /// <param name="ProductionCode"></param>
        /// <returns></returns>
        Task<List<ManuStatorBarcodeEntity>> GetStatorListAsync(string ProductionCode);

        /// <summary>
        /// 获取定子线组装好的数据
        /// </summary>
        /// <param name="innerId"></param>
        /// <returns></returns>
        Task<StatorOp340> GetStatorOp340Async(string innerId);


        //OP340
    }
}
