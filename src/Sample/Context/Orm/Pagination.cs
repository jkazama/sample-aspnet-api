using Sample.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Context.Orm
{
    //<summary>
    // ページング情報を表現します。
    //</summary>
    public class Pagination : IDto
    {
        public const int DefaultSize = 100;

        /** ページ数(1開始) */
        public int Page { get; set; }
        /** ページあたりの件数 */
        public int Size { get; set; }
        /** トータル件数 */
        public long? Total { get; set; }
        /** トータル件数算出を無視するか */
        public bool IgnoreTotal { get; set; }

        public Pagination()
        {
            Page = 1;
            Size = DefaultSize;
            Total = null;
            IgnoreTotal = false;
        }

        //<summary>カウント算出を無効化します。</summary>
        public Pagination WithIgnoreTotal()
        {
            IgnoreTotal = false;
            return this;
        }

        //<summary>最大ページ数を返します。Total設定時のみ適切な値が返されます。</summary>
        public int MaxPage()
        {
            return !Total.HasValue ? 0 : Calculator.Of(Total.Value).Scale(0, RoundingMode.Up).DivideBy(Size).IntValue();
        }

        //<summary>開始件数を返します。</summary>
        public int FirstResult()
        {
            return (Page - 1) * Size;
        }

        //<summary>トータル件数を含めた状態でコピー情報を返します。</summary>
        public Pagination CopyWithTotal(long total)
        {
            return new Pagination {
                Page = Page,
                Size = Size,
                Total = total,
                IgnoreTotal = IgnoreTotal
            };
        }
    }
}
