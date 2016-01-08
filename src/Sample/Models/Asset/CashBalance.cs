using System;
using System.ComponentModel.DataAnnotations.Schema;
using Sample.Context.Orm;
using Sample.Utils;

namespace Sample.Models.Asset
{
    //<summary>
    // 口座残高を表現します。
    //</summary>
    public class CashBalance : OrmActiveRecord<CashBalance>
    {
        /** ID */
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        /** 口座ID */
        public string AccountId { get; set; }
        /** 基準日 */
        public string BaseDay { get; set; }
        /** 通貨 */
        public string Currency { get; set; }
        /** 金額 */
        public decimal Amount { get; set; }
        /** 更新日 */
        public DateTime UpdateDate { get; set; }

        //<summary>
        // 残高へ指定した金額を反映します。
        // low: ここではCurrencyを使っていますが、実際の通貨桁数や端数処理定義はDBや設定ファイル等で管理されます。
        //</summary>
        public CashBalance Add(Repository rep, decimal addAmount)
        {
            this.Amount =
                Calculator.Of(Amount).Scale(0, RoundingMode.Down)
                    .Add(addAmount).DecimalValue();
            return Update(rep);
        }
    }
}