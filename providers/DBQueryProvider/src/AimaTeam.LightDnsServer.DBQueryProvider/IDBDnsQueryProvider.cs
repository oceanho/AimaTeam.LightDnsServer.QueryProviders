using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AimaTeam.LightDnsServer.DBQueryProvider
{
    public interface IDBDnsQueryProvider
    {
        string DbType
        {
            get;
        }
    }
}
