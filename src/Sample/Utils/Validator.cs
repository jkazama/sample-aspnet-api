using System;
using Sample.Context;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Sample.Utils
{
    //<summary>審査例外の構築概念を表現します</summary>
    public class Validator
    {
        private Warns _warns = Warns.Init();

        //<summary>審査を行います。validがfalseの時に例外をスタックします。</summary>
        public Validator Check(bool valid, string message)
        {
            if (!valid) _warns.Add(message);
            return this;
        }

        //<summary>個別属性の審査を行います。validがfalseの時に例外をスタックします。</summary>
        public Validator CheckField(bool valid, string field, string message)
        {
            if (!valid) _warns.Add(field, message);
            return this;
        }

        //<summary>審査を行います。失敗した時は即座に例外を発生させます。</summary>
        public Validator Verify(bool valid, string message)
        {
            return Check(valid, message).Verify();
        }

        //<summary>個別属性の審査を行います。失敗した時は即座に例外を発生させます。</summary>
        public Validator VerifyField(bool valid, string field, string message)
        {
            return CheckField(valid, field, message).Verify();
        }

        //<summary>検証します。事前に行ったCheckで例外が存在していた時は例外を発生させます。</summary>
        public Validator Verify() {
            if (HasWarn()) throw new Context.ValidationException(_warns);
            return Clear();
        }

        //<summary>審査例外を保有している時はtrueを返します。</summary>
        public bool HasWarn() {
            return _warns.NonEmpty();
        }

        //<summary>内部に保有する審査例外を初期化します。</summary>
        public Validator Clear() {
            _warns.Clear();
            return this;
        }

        //<summary>審査処理を行います</summary>
        public static void Validate(Action<Validator> act)
        {
            Validator validator = new Validator();
            act(validator);
            validator.Verify();
        }

        //<summary>DataAnnotaionを付与したオブジェクトに対する審査処理を行います</summary>
        public static void ValidateObject(object target)
        {
            Warns warns = Warns.Init();
            List<ValidationResult> r = new List<ValidationResult>();
            if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(target, new ValidationContext(target, null, null), r, true))
            {
                r.ForEach(v => v.MemberNames.ToList().ForEach(field => warns.Add(field, v.ErrorMessage)));
                throw new Context.ValidationException(warns);
            }

        }
    }

}