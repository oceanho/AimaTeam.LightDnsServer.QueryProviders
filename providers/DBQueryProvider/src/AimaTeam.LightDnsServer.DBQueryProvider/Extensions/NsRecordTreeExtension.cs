

using ARSoft.Tools.Net.Dns;
using System;
using System.Net;

using AimaTeam.LightDnsServer.DBQueryProvider.Entity;

namespace AimaTeam.LightDnsServer.DBQueryProvider.Extensions
{
    internal static class NsRecordTreeExtension
    {
        internal static NsRecordTree MakeAllTreeNode(this NsRecordTree nsRecordTree, int nsLabelIndex, string[] nsLabels, RecordType rType, params IPAddress[] ipAddr)
        {
            var _nsMaxLabelCount = nsLabels.Length;
            var _nsRT = new NsRecordTree(nsRecordTree.Level + 1);
            if (nsLabelIndex >= nsLabels.Length)
                return null;

            for (int dn_index = nsLabelIndex; dn_index >= 0; dn_index--)
            {
                //isFined = NsRecordTreeHerlper.FindDataFromNsRootRecordTree(nsRecordTree,
                //    dn.Labels[dn_index], rType, 1, defaultMaxFindLevel, new Action<NsRecordTree>((nsRecordTree) =>
                //{
                //    var _nsRT = new NsRecordTree(nsRecordTree.Level + 1);
                //    _nsRT.RecordType = rType;
                //    _nsRT.RecordClass = RecordClass.INet;
                //    _nsRT.Record = nsLabels[(dn_index > 0) ? dn_index - 1 : dn_index];

                //            //
                //            //  Build all domain name record tree node
                //            //
                //            var _nsChildRT = _nsRT.MakeAllTreeNode(dn_index, dn.Labels, rType, ipAddr);
                //    if (_nsChildRT != null)
                //        _nsRT.ChildNsRecordTreeList.Add(_nsChildRT);
                //    nsRecordTree.ChildNsRecordTreeList.Add(_nsRT);
                //}), isMatchCurrentNode: true);
                //if (isFined) break;
            }
            return _nsRT;
        }

        /// <summary>
        /// 通过递归的方式构建指定NameServer的RecordTree对象
        /// </summary>
        /// <param name="nsRecordTree">NsRecordTree对象,构建的NsRecord将会构建到此参数的ChildNsRecordList属性上</param>
        /// <param name="nsLabels"></param>
        /// <param name="nsLabelNextIndexValue"></param>
        /// <param name="rType"></param>
        /// <param name="ipAddr"></param>
        internal static void MakeChildNodeNsTree(this NsRecordTree nsRecordTree, string[] nsLabels, ref int nsLabelNextIndexValue, RecordType rType, params IPAddress[] ipAddr)
        {
            string nsCurLabel = string.Empty;
            NsRecordTree returnNsRecordTree = null;
            if (nsLabelNextIndexValue >= nsLabels.Length)
                return;

            nsCurLabel = nsLabels[nsLabelNextIndexValue];
            returnNsRecordTree = new NsRecordTree(nsRecordTree.Level + 1, nsCurLabel);
            if (nsLabelNextIndexValue < nsLabels.Length - 1)
            {
                nsLabelNextIndexValue++;
                returnNsRecordTree.MakeChildNodeNsTree(nsLabels, ref nsLabelNextIndexValue, rType, ipAddr);
            }

            NsRecordTree labelChildNsTree = null;
            if (CheckLabelIsExistCurrentNsRecordChildTree(nsRecordTree, new Action<NsRecordTree>((tree) =>
            {
                tree.Record = nsCurLabel;
                tree.RecordType = rType;
                tree.DnsRecordBase = null;
            }), nsCurLabel, rType))
                return;

            nsRecordTree.ChildNsRecordTreeList.Add(returnNsRecordTree);
        }

        internal static bool CheckLabelIsExistCurrentNsRecordChildTree(this NsRecordTree nsRecordTree, Action<NsRecordTree> findedNsRecordTreeAction, string currentLabel, RecordType rType)
        {
            if (nsRecordTree == null)
                return false;
            if (nsRecordTree.ChildNsRecordTreeList == null || nsRecordTree.ChildNsRecordTreeList.Count == 0)
                return false;
            for (int i = 0; i < nsRecordTree.ChildNsRecordTreeList.Count; i++)
            {
                if (nsRecordTree.ChildNsRecordTreeList[i].RecordType == rType &&
                    nsRecordTree.ChildNsRecordTreeList[i].Record.Equals(currentLabel, StringComparison.InvariantCultureIgnoreCase))
                {
                    findedNsRecordTreeAction(nsRecordTree.ChildNsRecordTreeList[i]);
                    return true;
                }
            }
            return false;
        }
    }
}
