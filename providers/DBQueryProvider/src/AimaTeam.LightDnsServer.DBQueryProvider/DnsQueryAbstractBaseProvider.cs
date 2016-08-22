using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ARSoft.Tools.Net.Dns;

namespace AimaTeam.LightDnsServer.DBQueryProvider.DBQueryProvider
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

        public void Add(string domain, RecordType rType, params IPAddress[] ipAddr)
        {
            throw new NotImplementedException();
        }

        public void Destory()
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

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~DnsQueryAbstractBaseProvider() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
