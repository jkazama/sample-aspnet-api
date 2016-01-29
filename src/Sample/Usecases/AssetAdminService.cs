using Sample.Models;
using Sample.Models.Asset;
using System;
using System.Collections.Generic;

namespace Sample.Usecases
{
    //<summary>
    // 資産ドメインに対する社内ユースケース処理。
    //</summary>
    public class AssetAdminService
    {
        private Repository _rep;
        public AssetAdminService(Repository rep)
        {
            _rep = rep;
        }

        //<summary>
        // 振込入出金依頼を検索します。
        // low: 口座横断的なので割り切りでREADロックはかけません。
        //</summary>
        public List<CashInOut> FindCashInOut(FindCashInOut p)
        {
            return _rep.Tx(() => CashInOut.Find(_rep, p));
        }

        //<summary>
        // 振込出金依頼の締め処理をします。
        //</summary>
        public void ClosingCashOut()
        {
            _rep.Tx(() =>
            {
                CashInOut.FindUnprocessed(_rep).ForEach(cio =>
                {
                    try
                    {
                        cio.Process(_rep);
                    }
                    catch (Exception e)
                    {
                        //TODO: Logger への書き換え
                        Console.Write("[" + cio.Id + "] 振込出金依頼の締め処理に失敗しました。");
                        Console.Write(e.Message);
                        try
                        {
                            cio.Error(_rep);
                        }
                        catch (Exception ex)
                        {
                            //low: 2重障害(恐らくDB起因)なのでloggerのみの記載に留める
                            Console.Write(ex.Message);
                        }
                    }
                });
            });
        }

        //<summary>
        // キャッシュフローを実現します。
        // <p>受渡日を迎えたキャッシュフローを残高に反映します。
        //</summary>
        public void RealizeCashflow()
        {
            var day = _rep.Helper.Time.Day();
            _rep.Tx(() =>
            {
                Cashflow.FindDoRealize(_rep, day).ForEach(cf =>
                {
                    try
                    {
                        cf.Realize(_rep);
                    }
                    catch (Exception e)
                    {
                        //TODO: Logger への書き換え
                        Console.Write("[" + cf.Id + "] キャッシュフローの実現に失敗しました。");
                        Console.Write(e.Message);
                        try
                        {
                            cf.Error(_rep);
                        }
                        catch (Exception ex)
                        {
                            //low: 2重障害(恐らくDB起因)なのでloggerのみの記載に留める
                            Console.Write(ex.Message);
                        }
                    }
                });
            });
        }
    }
}
