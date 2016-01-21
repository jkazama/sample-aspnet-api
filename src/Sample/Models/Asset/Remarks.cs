using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Models.Asset
{
    //<summary>
    // 摘要定数
    //</summary>
    public static class Remarks
    {
        /** 振込入金 */
        public const string CashInOut = "cashIn";
        /** 振込入金(調整) */
        public const string CashInAdjust = "cashInAdjust";
        /** 振込入金(取消) */
        public const string CashInCancel = "cashInCancel";
        /** 振込出金 */
        public const string CashOut = "cashOut";
        /** 振込出金(調整) */
        public const string CashOutAdjust = "cashOutAdjust";
        /** 振込出金(取消) */
        public const string CashOutCancel = "cashOutCancel";
    }
}
