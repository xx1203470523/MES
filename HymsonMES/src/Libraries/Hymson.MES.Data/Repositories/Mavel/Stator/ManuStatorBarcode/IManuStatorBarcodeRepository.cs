using Hymson.MES.Core.Domain.Mavel.Rotor;
using Hymson.MES.Core.Domain.Mavel.Stator;
using Hymson.MES.Data.Repositories.Mavel.Rotor.ManuRotorSfc.Query;
using Hymson.MES.Data.Repositories.Mavel.Stator.ManuStatorBarcode.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Mavel.Stator.ManuStatorBarcode
{
    /// <summary>
    /// 定子条码查询
    /// </summary>
    public interface IManuStatorBarcodeRepository
    {
        /// <summary>
        /// 查询总成码信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<ManuStatorBarcodeEntity>> GetListBySfcsAsync(StatorSfcQuery query);
    }
}
