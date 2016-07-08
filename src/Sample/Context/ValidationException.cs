using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Context
{
    //<summary>
    // 審査例外を表現します。
    // <p>ValidationExceptionは入力例外や状態遷移例外等の復旧可能な審査例外です。
    // その性質上ログ等での出力はWARNレベル(ERRORでなく)で行われます。
    // <p>審査例外はグローバル/フィールドスコープで複数保有する事が可能です。複数件の例外を取り扱う際は
    // Warnsを利用して初期化してください。
    //</summary>
    public class ValidationException : Exception
    {
        private Warns _warns;

        //<summary>フィールドに従属しないグローバルな審査例外を通知するケースで利用してください。</summary>
        public ValidationException(string message) : base(message)
        {
            _warns = Warns.Of(message);
        }
        //<summary>フィールドに従属する審査例外を通知するケースで利用してください。</summary>
        public ValidationException(string field, string message) : base(message)
        {
            _warns = Warns.Of(field, message);
        }
        //<summary>フィールドに従属する審査例外を通知するケースで利用してください。</summary>
        public ValidationException(string field, string message, string[] messageArgs) : base(message)
        {
            _warns = Warns.Of(field, message, messageArgs);
        }
        //<summary>複数件の審査例外を通知するケースで利用してください。</summary>
        public ValidationException(Warns warns) : base(warns.Select(v => v.Message).FirstOrDefault())
        {
            this._warns = warns;
        }

        //<summary>発生した審査例外一覧を返します。
        public List<Warn> List()
        {
            return _warns.ToList();
        }
    }
}