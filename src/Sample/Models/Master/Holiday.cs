using System;
using System.Collections.Generic;
using System.Linq;
using Sample.Context;
using Sample.Context.Orm;

namespace Sample.Models.Master
{
    //<summary>
    // サービス事業者の決済金融機関を表現します。
    // low: サンプルなので支店や名称、名義といったなど本来必須な情報をかなり省略しています。(通常は全銀仕様を踏襲します)
    //</summary>
    public class Holiday : OrmActiveRecord<Holiday>
    {
        public const string CategoryDefault = "default";

        /** ID */
        public long Id { get; set; }
        /** 休日区分 */
        public string Category { get; set; }
        /** 休日 */
        public DateTime Day { get; set; }
        /** 休日名称 */
        public string Name { get; set; }

        //<summary>休日マスタを取得します</summary>
        public static Holiday Get(Repository rep, DateTime day)
        {
            return Get(rep, day, CategoryDefault);
        }
        public static Holiday Get(Repository rep, DateTime day, string category)
        {
            return rep.Get<Holiday>(m => m.Category == category && m.Day == day);
        }
        //<summary>休日マスタを取得します(例外付)</summary>
        public static Holiday Load(Repository rep, DateTime day)
        {
            return Load(rep, day, CategoryDefault);
        }
        public static Holiday Load(Repository rep, DateTime day, string category)
        {
            return rep.Load<Holiday>(m => m.Category == category && m.Day == day);
        }

        //<summary>休日情報を検索します low: きちんとしたSQLで発行したいなら年度をfrom-to指定で抽出する</summary>
        public static List<Holiday> Find(Repository rep, int year)
        {
            return Find(rep, year, CategoryDefault);
        }
        public static List<Holiday> Find(Repository rep, int year, string category)
        {
            return rep.Template<Holiday>().Find(m => m.Category == category && m.Day.Year == year);
        }

        //<summary>休日マスタを登録します</summary>
        public static void Register(Repository rep, RegHoliday p)
        {
            rep.Holidays.RemoveRange(rep.Holidays.Where(m => m.Category == p.Category && m.Day.Year == p.Year));
            rep.Holidays.AddRange(p.List.Select(m => m.Create(p)).ToList());
            rep.SaveChanges();
        }
    }

    //<summary>登録パラメタ</summary>
    public class RegHoliday : IDto
    {
        public string Category { get; set; } = Holiday.CategoryDefault;
        public int Year { get; set; }
        public List<RegHolidayItem> List { get; set; }
    }

    //<summary>登録パラメタ(要素)</summary>
    public class RegHolidayItem : IDto
    {
        public DateTime Day { get; set; }
        public string Name { get; set; }

        public Holiday Create(RegHoliday p)
        {
            return new Holiday { Category = p.Category, Day = Day, Name = Name };
        }
    }

}