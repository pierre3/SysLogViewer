using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SysLogViewer.Models;

namespace SysLogViewer.Repositories
{
    public class PleasanterDbClient : IDisposable
    {
        private DbConnection Connection { get; set; }
        private int CommandTimeout { get; set; }

        public PleasanterDbClient(string connectionString, int commandTimeout)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
            CommandTimeout = commandTimeout;
        }

        public async ValueTask<IEnumerable<SysLogModel>> GetSysLogs(DateTime? startDate,DateTime? endDate, SysLogTypes sysLogType, string url)
        {
            var whereList = new List<string>();
            var param = new DynamicParameters();
            if (startDate is not null) 
            {
                whereList.Add($"[CreatedTime] > @startDate");
                param.Add("startDate", startDate);
            } 
            if(endDate is not null)
            {
                whereList.Add($"[CreatedTime] < @endDate");
                param.Add("endDate", endDate);
            }
            if(sysLogType != SysLogTypes.None)
            {
                whereList.Add($"[SysLogType] = @sysLogType");
                param.Add("sysLogType", (int)sysLogType);
            }
            if (!string.IsNullOrEmpty(url))
            {
                whereList.Add($"[Url] like @url");
                param.Add("url", "%" + url + "%");
            }
            var where = (whereList.Count > 0) ? "where " + string.Join(" and ", whereList) : string.Empty;

            return await Connection.QueryAsync<SysLogModel>(
                $"select top (1000) * from [SysLogs] {where}", 
                param, 
                commandTimeout: CommandTimeout);
        }

        public void Dispose()
        {
            Connection.Close();
            Connection.Dispose();
        }
    }
}
