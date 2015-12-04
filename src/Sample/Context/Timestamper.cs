using System;

namespace Sample.Context
{
    //<summary>
    // 日時ユーティリティコンポーネント。
    //</summary>
    public class Timestamper
    {
        public const string KeyDay = "system.businessDay.day";

        private DateTime? _mockDate;
        public Timestamper()
        {
            _mockDate = null;
        }
        public Timestamper(DateTime mockDate)
        {
            _mockDate = mockDate;
        }

        //<summary>営業日を返します</summary>
        public DateTime day()
        {
            return _mockDate.HasValue ? _mockDate.Value.Date : DateTime.Today;
        }
        //<summary>日時を返します</summary>
        public DateTime date()
        {
            return _mockDate.HasValue ? _mockDate.Value : DateTime.Now;
        }
    }
}