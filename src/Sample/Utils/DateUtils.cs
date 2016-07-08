using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Utils
{
    public class DateUtils
    {
        //<summary>指定した日付の翌日から1msec引いた日時を返します。</summary>
        public static DateTime DateTo(DateTime day)
        {
            return day.Date.AddDays(1).AddMilliseconds(-1);
        }
    }
}
