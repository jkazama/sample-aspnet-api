using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Context
{
    //<summary>
    // 審査例外情報です。
    //</summary>
    public class Warns : IEnumerable<Warn>
    {
        private List<Warn> _list = new List<Warn>();

        private Warns() {}

        public IEnumerator<Warn> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Warns Add(string message)
        {
            _list.Add(new Warn { Message = message });
            return this;
        }
        public Warns Add(string field, string message)
        {
            _list.Add(new Warn { Field = field, Message = message });
            return this;
        }
        public Warns Add(string field, string message, string[] messageArgs)
        {
            _list.Add(new Warn { Field = field, Message = message, MessageArgs = messageArgs });
            return this;
        }

        public Warns Clear()
        {
            _list.Clear();
            return this;
        }
        public bool NonEmpty()
        {
            return 0 < this.Count();
        }

        public static Warns Init() {
            return new Warns();
        }
        public static Warns Of(string message) {
            return Init().Add(message);
        }
        public static Warns Of(string field, string message) {
            return Init().Add(field, message);
        }
        public static Warns Of(string field, string message, string[] messageArgs) {
            return Init().Add(field, message, messageArgs);
        }
    }

    //<summary>
    // フィールドスコープの審査例外トークンを表現します。
    //</summary>
    public class Warn
    {
        /** 審査例外フィールドキー */
        public string Field { get; set; }
        /** 審査例外メッセージ */
        public string Message { get; set; }
        /** 審査例外メッセージ */
        public string[] MessageArgs { get; set; }

        /** フィールドに従属しないグローバル例外時はtrue */
        public bool Global()
        {
            return Field == null || Field.Trim() == "";
        }
    }
}