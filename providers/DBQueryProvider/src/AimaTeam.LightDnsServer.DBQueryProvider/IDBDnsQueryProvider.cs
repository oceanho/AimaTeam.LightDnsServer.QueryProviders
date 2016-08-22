using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AimaTeam.LightDnsServer.Core;
namespace AimaTeam.LightDnsServer.DBQueryProvider.DBQueryProvider
{
    public interface IDBDnsQueryProvider: IDNSQueryProvider
    {
        string DbType
        {
            get;
        }
    }
}
