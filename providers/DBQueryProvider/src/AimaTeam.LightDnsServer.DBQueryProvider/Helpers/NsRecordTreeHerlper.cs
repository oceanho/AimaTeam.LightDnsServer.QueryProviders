
using ARSoft.Tools.Net;
using ARSoft.Tools.Net.Dns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using AimaTeam.LightDnsServer.Core;
using AimaTeam.LightDnsServer.DBQueryProvider.Entity;
using AimaTeam.LightDnsServer.DBQueryProvider.Helpers;
using AimaTeam.LightDnsServer.DBQueryProvider.Extensions;

namespace AimaTeam.LightDnsServer.DBQueryProvider.Helpers
{
    internal static class NsRecordTreeHerlper
    {
        private static int defaultMaxFindLevel = 8;
        private static object safeAccessLockObject = new object();

        /// <summary>
        /// 构建一个新的根NS树形
        /// </summary>
        /// <returns></returns>
        internal static NsRecordTree CreateRootNsRecordTree()
        {
            return new NsRecordTree(0);
        }

        /// <summary>
        /// 新加或者更新一个域名解析记录
        /// </summary>
        /// <param name="rootNsRecordTree">域名存储Root树</param>
        /// <param name="domain">域名（支持泛解析）,例如：www.aimateam.org  aimateam.org  *.blog.aimateam.org</param>
        /// <param name="rType">域名解析类型</param>
        /// <param name="ipAddr">域名对应IP地址列表</param>
        internal static void AddOrUpdateNSRecord(NsRecordTree rootNsRecordTree, string domain, RecordType rType, params IPAddress[] ipAddr)
        {
            CheckRootNsRecordTree(rootNsRecordTree);
            var isFined = false;
            DomainName dn = DomainName.Parse(domain);
            int label_max_index = dn.LabelCount - 1;
            lock (safeAccessLockObject)
            {
                for (int dn_index = label_max_index; dn_index >= 0; dn_index--)
                {
                    isFined = FindDataFromNsRootRecordTree(rootNsRecordTree,
                        dn.Labels[dn_index], rType, 1, defaultMaxFindLevel, new Action<NsRecordTree>((nsRecordTree) =>
                    {
                        // It's will be excute here if find match node
                    }));
                    if (isFined) break;
                }

                // add new operation if not find any match result
                if (!isFined)
                {
                    for (int dn_index = label_max_index; dn_index >= 0; dn_index--)
                    {
                        isFined = FindDataFromNsRootRecordTree(rootNsRecordTree,
                            dn.Labels[dn_index], rType, 1, defaultMaxFindLevel, new Action<NsRecordTree>((nsRecordTree) =>
                        {
                            var _nsRT = new NsRecordTree(nsRecordTree.Level + 1);
                            _nsRT.RecordType = rType;
                            _nsRT.RecordClass = RecordClass.INet;
                            _nsRT.Record = dn.Labels[(dn_index > 0) ? dn_index - 1 : dn_index];

                            //
                            //  Build all domain name record tree node
                            //
                            var _nsChildRT = _nsRT.MakeAllTreeNode(dn_index, dn.Labels, rType, ipAddr);
                            if (_nsChildRT != null)
                                _nsRT.ChildNsRecordTreeList.Add(_nsChildRT);
                            nsRecordTree.ChildNsRecordTreeList.Add(_nsRT);
                        }), isMatchCurrentNode: true);
                        if (isFined) break;
                    }
                }
            }
        }

        /// <summary>
        /// 移除一个解析记录
        /// </summary>
        /// <param name="rootNsRecordTree">域名存储Root树</param>
        /// <param name="domain">域名（支持泛解析）,例如：www.aimateam.org  aimateam.org  *.blog.aimateam.org</param>
        /// <param name="rType">域名解析类型</param>
        internal static void RemoveNSRecord(NsRecordTree rootNsRecordTree, string domain, RecordType rType)
        {
            CheckRootNsRecordTree(rootNsRecordTree);
            //lock (safeAccessLockObject)
            //{
            //}
        }

        /// <summary>
        /// 查询一个解析记录
        /// </summary>
        /// <param name="rootNsRecordTree">域名存储Root树</param>
        /// <param name="domain">域名（支持泛解析）,例如：www.aimateam.org  aimateam.org  *.blog.aimateam.org</param>
        /// <param name="rType">域名解析类型</param>
        internal static DnsRecordBase SelectNSRecord(NsRecordTree rootNsRecordTree, string domain, RecordType rType)
        {
            var nsExcuteRecordTree = rootNsRecordTree;
            CheckRootNsRecordTree(rootNsRecordTree);
            var isFined = false;
            var isFinedCount = 0;
            DomainName dn = DomainName.Parse(domain);
            int label_max_index = dn.LabelCount - 1;

            for (int dn_index = label_max_index; dn_index >= 0; dn_index--)
            {
                isFined = FindDataFromNsRootRecordTree(nsExcuteRecordTree,
                    dn.Labels[dn_index], rType, 1, defaultMaxFindLevel, new Action<NsRecordTree>((nsRecordTree) =>
                     {
                         // It's will be excute here if find match node
                         // isFined = FindDataFromNsRootRecordTree
                         nsExcuteRecordTree = nsRecordTree;
                     }));
                if (!isFined) break;
                isFinedCount++;
            }
            if (isFinedCount == label_max_index)
                return nsExcuteRecordTree.DnsRecordBase;
            return null;
            // return new DnsRecordBase
        }

        private static void CheckRootNsRecordTree(NsRecordTree nsRecordTree)
        {
            if (nsRecordTree == null)
                throw new ArgumentNullException("Invalid NsRecordTree ,because of the nsRecordTree is null");
            if (!nsRecordTree.IsRoot)
                throw new ArgumentException("Invalid NsRecordTree ,because of the nsRecordTree is not Root Node");
        }

        /// <summary>
        /// 递归遍历 NsRecordTree 节点,查找合适的NsRecordTree
        /// </summary>
        /// <param name="nsRecordTree">节点入口</param>
        /// <param name="label">record比较字符串</param>
        /// <param name="rType">记录类型</param>
        /// <param name="curFindLevel">当前递归深度</param>
        /// <param name="maxFindLevel">最大递归深度（避免死循环）</param>
        /// <param name="findTreeExcuteAction">找到nsRecordTree后执行的调用Action</param>
        /// <returns></returns>
        internal static bool FindDataFromNsRootRecordTree(NsRecordTree nsRecordTree, string label, RecordType rType, int curFindLevel, int maxFindLevel, Action<NsRecordTree> findTreeExcuteAction, bool isMatchCurrentNode = false)
        {
            if (nsRecordTree == null)
                throw new ArgumentNullException("nsRecordTree is null");

            if (curFindLevel++ > maxFindLevel)
                throw new TimeoutException("Excute the method FindDataFromNsRootRecordTree is timeout,because of curFindLevel[" + curFindLevel + "] larger than maxFindLevel[" + maxFindLevel + "]");

            if (isMatchCurrentNode)
            {
                if (nsRecordTree.Record.Equals(label, StringComparison.OrdinalIgnoreCase) && rType == nsRecordTree.RecordType)
                {
                    findTreeExcuteAction(nsRecordTree);
                    return true;
                }
            }

            for (int i = 0; i < nsRecordTree.ChildNsRecordTreeList.Count; i++)
            {
                if (nsRecordTree.ChildNsRecordTreeList[i].Record.Equals(label, StringComparison.OrdinalIgnoreCase) && rType == nsRecordTree.ChildNsRecordTreeList[i].RecordType)
                {
                    findTreeExcuteAction(nsRecordTree.ChildNsRecordTreeList[i]);
                    return true;
                }
            }

            var isFined = false;
            for (int i = 0; i < nsRecordTree.ChildNsRecordTreeList.Count; i++)
            {
                isFined = false;
                isFined = FindDataFromNsRootRecordTree(nsRecordTree.ChildNsRecordTreeList[i], label, rType, curFindLevel, maxFindLevel, findTreeExcuteAction);
                if (isFined) break;
            }
            return isFined;
        }
    }
}
