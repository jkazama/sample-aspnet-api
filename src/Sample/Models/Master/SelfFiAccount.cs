using Sample.Context.Orm;

namespace Sample.Models.Master
{
    //<summary>
    // サービス事業者の決済金融機関を表現します。
    // low: サンプルなので支店や名称、名義といったなど本来必須な情報をかなり省略しています。(通常は全銀仕様を踏襲します)
    //</summary>
    public class SelfFiAccount : OrmActiveRecord<SelfFiAccount>
    {
        /** ID */
        public long Id { get; set; }
        /** 利用用途カテゴリ */
        public string Category { get; set; }
        /** 通貨 */
        public string Currency { get; set; }
        /** 金融機関コード */
        public string FiCode { get; set; }
        /** 金融機関口座ID */
        public string FiAccountId { get; set; }

        public static SelfFiAccount Load(Repository rep, string category, string currency)
        {
            return rep.Load<SelfFiAccount>(m => m.Category == category && m.Currency == currency);
        }
    }
}