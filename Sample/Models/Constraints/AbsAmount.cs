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
        public AbsAmount() : base(Resources.Exception.DomainAbsAmount) {}

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || String.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }
            decimal result;
            if (Decimal.TryParse(value.ToString(), out result))
            {
                if (0 < result)return ValidationResult.Success;
            }
            return new ValidationResult(Resources.Exception.DomainAbsAmount);
        }
    }
}