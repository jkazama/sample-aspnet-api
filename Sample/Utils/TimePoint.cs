using System;

namespace Sample.Utils
{
    //<summary>
    // 日付と日時のペアを表現します。
    //<p>0:00に営業日切り替えが行われないケースなどでの利用を想定しています。
    //</summary>
    public class TimePoint
    {
        /** 日付(営業日) */
        public DateTime Day { get; set; }
        /** 日付におけるシステム日時 */
        public DateTime Date { get; set; }

        //<summary>指定日付と同じか(day == targetDay)</summary>
        public bool EqualsDay(DateTime targetDay)
        {
            return Day == targetDay.Date;
        }
        //<summary>指定日付よりも前か(day &lt; targetDay)</summary>
        public bool BeforeDay(DateTime targetDay)
        {
            return Day < targetDay.Date;
        }
        //<summary>指定日付以前か(day &lt;= targetDay) </summary>
        public bool BeforeEqualsDay(DateTime targetDay)
        {
            return Day <= targetDay.Date;
        }
        //<summary>指定日付よりも後か(targetDay &lt; day)</summary>
        public bool AfterDay(DateTime targetDay)
        {
            return Day > targetDay.Date;
        }
        //<summary>指定日付以降か(targetDay &lt;= day)</summary>
        public bool AfterEqualsDay(DateTime targetDay)
        {
            return Day >= targetDay.Date;
        }

        //<summary>指定日付/日時を元にTimePointを生成します</summary>
        public static TimePoint Of(DateTime day, DateTime date)
        {
            return new TimePoint { Day = day, Date = date };
        }
        //<summary>指定日付を元にTimePointを生成します</summary>
        public static TimePoint Of(DateTime day)
        {
            return Of(day.Date, day.Date);
        }
        //<summary>TimePointを生成します</summary>
        public static TimePoint Now()
        {
            return Of(DateTime.Now);
        }
        //<summary>指定時間を元にTimePointを生成します</summary>
        public static TimePoint Now(DateTime date)
        {
            return Of(date.Date, date);
        }
    }
}