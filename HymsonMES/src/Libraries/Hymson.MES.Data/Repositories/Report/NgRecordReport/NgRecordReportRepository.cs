using Hymson.MES.Data.Options;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Report;

public class NgRecordReportRepository : BaseRepository, INgRecordReportRepository
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    private readonly ConnectionOptions _connectionOptions;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="connectionOptions"></param>
    /// <param name="memoryCache"></param>
    public NgRecordReportRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
    {
        _connectionOptions = connectionOptions.Value;
    }

}
