using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AimaTeam.LightDnsServer.DBQueryProvider
{
    public abstract class DnsQueryAbstractBaseProvider : IDBDnsQueryProvider
    {
        private const string _defaultDbType = "SQLServer";

        private string _dbType;
        public DnsQueryAbstractBaseProvider() : this(_defaultDbType)
        {            
        }

        public DnsQueryAbstractBaseProvider(string dbType)
        {
            _dbType = dbType;
        }

        public virtual string DbType
        {
            get
            {
                return _dbType;
            }
        }
    }
}
