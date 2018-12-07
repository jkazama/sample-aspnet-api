using System;

namespace Sample.Context
{
    //<summary>
    // 処理時の実行例外を表現します。
    // <p>復旧不可能なシステム例外をラップする目的で利用してください。
    //</summary>
    public class InvocationException : Exception
    {
        public InvocationException() { }
        public InvocationException(string message) : base(message) { }
        public InvocationException(string message, Exception inner) : base(message, inner) { }
    }
}