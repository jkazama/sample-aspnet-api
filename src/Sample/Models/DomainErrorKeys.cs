using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Models
{
    //<summary>
    // 汎用ドメインで用いるメッセージキー定数。
    //</summary>
    public class DomainErrorKeys
    {
        /** マイナスを含めない数字を入力してください */
        public const string AbsAmountZero = "error.domain.AbsAmount.zero";
    }
}
