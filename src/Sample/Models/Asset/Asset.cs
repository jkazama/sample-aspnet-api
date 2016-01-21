using Sample.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Models.Asset
{
    //<summary>
    // 口座の資産概念を表現します。
    // asset配下のEntityを横断的に取り扱います。
    // low: 実際の開発では多通貨や執行中/拘束中のキャッシュフローアクションに対する考慮で、サービスによってはかなり複雑になります。
    //</summary>
    public class Asset
    {
        public string Id { get; set; }
        private Asset(string id)
        {
            this.Id = id;
        }

        //<summary>
        // 振込出金可能か判定します。
        // <p>0 &lt;= 口座残高 + 未実現キャッシュフロー - (出金依頼拘束額 + 出金依頼額) 
        // low: 判定のみなのでscale指定は省略。余力金額を返す時はきちんと指定する
        //</summary>
        public bool CanWithdraw(Repository rep, string currency, decimal absAmount, DateTime valueDay)
        {
            var calc = Calculator.Of(CashBalance.GetOrNew(rep, Id, currency).Amount);
            Cashflow.FindUnrealize(rep, Id, currency, valueDay).ForEach(cf => calc.Add(cf.Amount));
            CashInOut.FindUnprocessed(rep, Id, currency, true).ForEach(cf => calc.Add(-cf.AbsAmount));
            calc.Add(-absAmount);
            return 0 <= calc.DecimalValue();
        }

        //<summary>口座IDに紐付く資産概念を返します。</summary>
        public static Asset Of(string accountId)
        {
            return new Asset(accountId);
        }
    }
}
