using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Models.Constraints
{
    //<summary>
    // 絶対値の金額(必須)を表現する制約注釈。
    //</summary>
    public class AbsAmount : ValidationAttribute
    {
        public AbsAmount()
        {
            // error_domain_absAmount
            this.ErrorMessage = "マイナスを含めない数字を入力してください。";
        }

        public override bool IsValid(object value)
        {
            if (value == null || String.IsNullOrWhiteSpace(value.ToString())) return true;
            decimal result;
            if (Decimal.TryParse(value.ToString(), out result))
            {
                if (0 < result) return true;
            }
            return false;
        }
    }
}
