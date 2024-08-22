using Dapper;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Hymson.MES.Data.Repositories.Report;



public partial class PackTraceSFCParameterRepository : BaseRepository, IPackTraceSFCParameterRepository
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="connectionOptions"></param>
    public PackTraceSFCParameterRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
    {
    }

    /// <summary>
    /// PACK码追溯电芯码查询设备采集参数
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public async Task<IEnumerable<PackTraceSFCParameterView>> GetListAsync(PackTraceSFCParameterQuery query)
    {
        SqlBuilder sqlBuilder = new SqlBuilder();

        var getSql = sqlBuilder.AddTemplate(GetListSQL);

        string sfcstr = string.Join("','", query.SFC);

        sqlBuilder.Where($" SFC IN ('{sfcstr}') ");

        //永泰数据有问题，电芯码记录的可能是Pack码
        sqlBuilder.AddParameters(query);

        using var conn = GetMESDbConnection();
        return await conn.QueryAsync<PackTraceSFCParameterView>(getSql.RawSql, getSql.Parameters);
    }
}



/// <summary>
/// SQL语句
/// </summary>
public partial class PackTraceSFCParameterRepository
{
    private readonly string GetListSQL = @"WITH RECURSIVE T1 AS (
SELECT msc.SFC PACK, msc.CirculationBarCode MODULE, msc.SFC,1 LV FROM manu_sfc_circulation msc /**where**/
UNION ALL
SELECT T1.PACK PACK, msc2.CirculationBarCode, msc2.SFC MODULE, T1.LV + 1 LV FROM T1
LEFT JOIN manu_sfc_circulation msc2 ON T1.SFC = msc2.CirculationBarCode
WHERE msc2.SFC<> '' AND msc2.CirculationBarCode<>'' AND msc2.IsDeleted = 0 AND msc2.IsDisassemble = 0 AND t1.sfc != msc2.SFC
)
SELECT T1.PACK, T1.SFC, ee.EquipmentName , mpp.JudgmentResult,DATE_FORMAT(mpp.LocalTime,'%Y-%m-%d %H:%i:%s') `LocalTime`,pp.ParameterCode , pp.ParameterName , mpp.ParamValue ParameterValue, pp2.Code procedureCode, pp2.Name procedureName,
mpp.StandardLowerLimit, mpp.StandardUpperLimit FROM T1
LEFT JOIN manu_product_parameter mpp ON T1.SFC = mpp.SFC
left join equ_equipment ee on ee.Id  = mpp.EquipmentId and ee.IsDeleted = 0
left join proc_parameter pp on pp.id = mpp.ParameterId and pp.IsDeleted = 0
left join proc_procedure pp2 on pp2.Id = mpp.ProcedureId and pp2.IsDeleted = 0
WHERE lv = 3 and mpp.Id<> '' 
ORDER BY T1.PACK,mpp.localTime DESC
";
}