using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Models.Constraints
    {
        //<summary>
        // 入力必須を表現する制約注釈。
        // low: 標準の Required のローカライズがうまくいかなかったため暫定的に作成
        //</summary>
        public class NotNull : ValidationAttribute
        {
            public NotNull() : base(Resources.Exception.PropertyValueRequired) {}

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                return value != null ? ValidationResult.Success : new ValidationResult(Resources.Exception.PropertyValueRequired);
            }
        }
    }