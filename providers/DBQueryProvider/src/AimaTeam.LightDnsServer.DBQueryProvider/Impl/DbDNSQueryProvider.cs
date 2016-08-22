
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using ARSoft.Tools.Net.Dns;
using AimaTeam.LightDnsServer.Core;

namespace AimaTeam.LightDnsServer.DBQueryProvider.Impl
{
    internal class DbDNSQueryProvider : IDNSQueryProvider
    {
        public void Add(string domain, RecordType rType, params IPAddress[] ipAddr)
        {
            throw new NotImplementedException();
        }

        public void Destory()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void Remove(string domain, RecordType rType)
        {
            throw new NotImplementedException();
        }

        public DnsRecordBase Select(string domain, RecordType rType)
        {
            throw new NotImplementedException();
        }

        public void Update(string domain, RecordType rType, params IPAddress[] ipAddr)
        {
            throw new NotImplementedException();
        }
    }
}
