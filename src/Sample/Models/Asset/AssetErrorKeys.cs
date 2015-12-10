namespace Sample.Models.Asset
{
    //<summary>
    //審査例外で用いるメッセージキー定数
    //</summary>
    public static class ErrorKeys
    {
        /** 受渡日を迎えていないため実現できません */
        public const string CashflowRealizeDay = "error.Cashflow.realizeDay";
        /** 既に受渡日を迎えています */
        public const string CashflowBeforeEqualsDay = "error.Cashflow.beforeEqualsDay";

        /** 未到来の受渡日です */
        public const string CashInOutAfterEqualsDay = "error.CashInOut.afterEqualsDay";
        /** 既に発生日を迎えています */
        public const string CashInOutBeforeEqualsDay = "error.CashInOut.beforeEqualsDay";
        /** 出金可能額を超えています */
        public const string CashInOutWithdrawAmount = "error.CashInOut.withdrawAmount";
    }

}