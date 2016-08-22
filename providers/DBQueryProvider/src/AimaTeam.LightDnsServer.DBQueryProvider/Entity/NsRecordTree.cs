using ARSoft.Tools.Net.Dns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AimaTeam.LightDnsServer.DBQueryProvider.Entity
{
    /// <summary>
    /// Dns Server 的域名解析数据存储的树形结构类
    /// </summary>
    public class NsRecordTree
    {
        #region ctor       
        public NsRecordTree(int deepLevel) :
            this(deepLevel, null)
        {
            this.level = deepLevel;
            this.childNsRecordTreeList = new List<NsRecordTree>(0);
        }
        public NsRecordTree(int level, string record)
        {
            this.level = level;
            this.Record = record;
            this.childNsRecordTreeList = new List<NsRecordTree>(0);
        }
        #endregion

        #region fileds

        private int level;
        private string record;
        private RecordType recordType;
        private RecordClass recordClass;
        private DnsRecordBase dnsRecordBase;
        private List<NsRecordTree> childNsRecordTreeList;
        #endregion

        #region properties

        /// <summary>
        /// 获取节点的深度等级
        /// </summary>
        public int Level
        {
            get
            {
                return level;
            }
        }

        /// <summary>
        /// 获取或者设置节点保存的记录值
        /// </summary>
        public string Record
        {
            get
            {
                return record;
            }
            set
            {
                if (level > 0 && (value == null || string.IsNullOrEmpty(value)))
                    throw new ArgumentNullException("Invalid set value,because of the record value is null or empty");
                record = value;
            }
        }

        /// <summary>
        /// 获取节点的是否为根节点
        /// </summary>
        public bool IsRoot
        {
            get
            {
                return level == 1;
            }
        }

        /// <summary>
        /// 获取或者设置Ns节点的RecordType
        /// </summary>
        public RecordType RecordType
        {
            get
            {
                return recordType;
            }
            set
            {
                recordType = value;
            }
        }

        /// <summary>
        /// 获取或者设置Ns节点的RecordClass
        /// </summary>
        public RecordClass RecordClass
        {
            get
            {
                return recordClass;
            }
            set
            {
                recordClass = value;
            }
        }

        /// <summary>
        /// 获取所有NsTree的子节点列表
        /// </summary>
        public List<NsRecordTree> ChildNsRecordTreeList
        {
            get
            {
                return childNsRecordTreeList;
            }
        }

        /// <summary>
        /// 获取所有NsTree的节点的DnsRecordBase对象
        /// </summary>
        public DnsRecordBase DnsRecordBase
        {
            get
            {
                return dnsRecordBase;
            }
            set
            {
                dnsRecordBase = value;
            }
        }

        #endregion
    }
}
