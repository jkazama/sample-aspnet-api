using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
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
        public DateTime BaseDay { get; set; }
        /** 通貨 */
        public string Currency { get; set; }
        /** 金額 */
        public decimal Amount { get; set; }
        /** 更新日 */
        public DateTime UpdateDate { get; set; }

        //<summary>
        // 残高へ指定した金額を反映します。
        // low: ここでは決めうちで精度0を利用していますが、実際の通貨桁数や端数処理定義はDBや設定ファイル等で管理されます。
        //</summary>
        public CashBalance Add(Repository rep, decimal addAmount)
        {
            this.Amount =
                Calculator.Of(Amount).Scale(0, RoundingMode.Down)
                    .Add(addAmount).DecimalValue();
            return Update(rep);
        }

        //<summary>
        // 指定口座の残高を取得します。(存在しない時は繰越保存後に取得します)
        // low: 複数通貨の適切な考慮や細かい審査は本筋でないので割愛。
        //</summary>
        public static CashBalance GetOrNew(Repository rep, string accountId, string currency)
        {
            var baseDay = rep.Helper.Time.Day();
            var cb = rep.Get<CashBalance>(m => m.BaseDay == baseDay && m.AccountId == accountId && m.Currency == currency);
            return cb == null ? Create(rep, accountId, currency) : cb;
        }
        private static CashBalance Create(Repository rep, string accountId, string currency)
        {
            var now = rep.Helper.Time.Tp();
            var prev = rep.EntitySet<CashBalance>()
                .Where(m => m.AccountId == accountId && m.Currency == currency)
                .OrderBy(m => m.BaseDay)
                .FirstOrDefault();
            var amount = prev != null ? prev.Amount : decimal.Zero; // 残高繰越考慮
            return new CashBalance { AccountId = accountId, BaseDay = now.Day, Currency = currency, Amount = amount, UpdateDate = now.Date };
        }
    }
}