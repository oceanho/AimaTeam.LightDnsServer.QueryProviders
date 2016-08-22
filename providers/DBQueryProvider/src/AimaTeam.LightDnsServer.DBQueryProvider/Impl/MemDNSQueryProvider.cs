
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ARSoft.Tools.Net.Dns;

using AimaTeam.LightDnsServer.Core;
using AimaTeam.LightDnsServer.DBQueryProvider.Entity;
using AimaTeam.LightDnsServer.DBQueryProvider.Helpers;

namespace AimaTeam.LightDnsServer.DBQueryProvider.DNSServer.Impl
{
    internal class MemDNSQueryProvider : IDNSQueryProvider
    {
        private NsRecordTree rootnsRecordTree;
        internal MemDNSQueryProvider()
        {
            rootnsRecordTree = NsRecordTreeHerlper.CreateRootNsRecordTree();
        }

        public void Init()
        {
            // throw new NotImplementedException();
        }

        public void Destory()
        {
            Dispose();
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public void Add(string domain, RecordType rType, params IPAddress[] ipAddr)
        {
            NsRecordTreeHerlper.AddOrUpdateNSRecord(rootnsRecordTree, domain, rType, ipAddr);
        }
        public void Update(string domain, RecordType rType, params IPAddress[] ipAddr)
        {
            NsRecordTreeHerlper.AddOrUpdateNSRecord(rootnsRecordTree, domain, rType, ipAddr);
        }
        public void Remove(string domain, RecordType rType)
        {
            NsRecordTreeHerlper.RemoveNSRecord(rootnsRecordTree, domain, rType);
        }

        public DnsRecordBase Select(string domain, RecordType rType)
        {
            return NsRecordTreeHerlper.SelectNSRecord(rootnsRecordTree, domain, rType);
        }
    }
}
