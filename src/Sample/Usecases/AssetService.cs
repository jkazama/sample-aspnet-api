
using Sample.Context;
using Sample.Models;
using Sample.Models.Asset;
using System.Collections.Generic;

namespace Sample.Usecases
{
    //<summary>
    // 資産ドメインに対する顧客ユースケース処理
    //</summary>
    public class AssetService
    {
        private Repository _rep;
        public AssetService(Repository rep)
        {
            _rep = rep;
        }

        //<summary>匿名を除く利用者を返します</summary>
        private Actor Actor()
        {
            return ServiceUtils.ActorUser(_rep.Helper.Actor());
        }

        //<summary>
        // 未処理の振込依頼情報を検索します。
        // low: 参照系は口座ロックが必要無いケースであれば @Transactionalでも十分
        // low: CashInOutは情報過多ですがアプリケーション層では公開対象を特定しにくい事もあり、
        // UI層に最終判断を委ねています。
        //</summary>
        public List<CashInOut> FindUnprocessedCashOut()
        {
            return _rep.Tx(() => CashInOut.FindUnprocessed(_rep, Actor().Id));
        }

        //<summary>
        // 振込出金依頼をします。
        // low: 公開リスクがあるためUI層には必要以上の情報を返さない事を意識します。
        // low: 監査ログの記録は状態を変えうる更新系ユースケースでのみ行います。
        // low: ロールバック発生時にメールが飛ばないようにトランザクション境界線を明確に分離します。
        //</summary>
        public long Withdraw(RegCashOut p)
        {
            p.AccountId = Actor().Id; // 顧客側はログイン利用者で強制上書き
            var cio = _rep.Tx(() => CashInOut.Withdraw(_rep, p));
            //TODO: メール送信処理
            return cio.Id;
        }
    }
}