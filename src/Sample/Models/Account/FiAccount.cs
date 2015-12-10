using Sample.Context.Orm;

namespace Sample.Models.Account
{
    //<summary>
    // 口座に紐づく金融機関口座を表現します。
    // <p>口座を相手方とする入出金で利用します。
    // low: サンプルなので支店や名称、名義といった本来必須な情報をかなり省略しています。(通常は全銀仕様を踏襲します)
    //</summary>
    public class FiAccount : OrmActiveRecord<FiAccount>
    {
        /** ID */
        public long Id { get; set; }
        /** 口座ID */
        public string AccountId { get; set; }
        /** 利用用途カテゴリ */
        public string Category { get; set; }
        /** 通貨 */
        public string Currency { get; set; }
        /** 金融機関コード */
        public string FiCode { get; set; }
        /** 金融機関口座ID */
        public string FiAccountCode { get; set; }

        public static FiAccount Load(Repository rep, string accountId, string category, string currency)
        {
            return rep.Load<FiAccount>(m => m.AccountId == accountId && m.Category == category && m.Currency == currency);
        }
    }
}