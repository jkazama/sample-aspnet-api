using System;
using System.Numerics;

namespace Sample.Utils
{
    //<summary>
    // 計算ユーティリティ
    //</summary>
    public class Calculator
    {
        private decimal _value = 0m;
        /** 小数点以下桁数 */
        private int _scale = 0;
        /** 端数処理。標準では切り捨て */
        private RoundingMode _mode = RoundingMode.Down;
        /** 計算の都度端数処理をする時はtrue */
        private bool _roundingAlways = false;
        /** scala未設定時の除算scale */
        private const int defaultScale = 18;

        private Calculator(decimal value)
        {
            _value = value;
        }

        public Calculator Scale(int scale)
        {
            return this.Scale(scale, RoundingMode.Down);
        }
        public Calculator Scale(int scale, RoundingMode mode)
        {
            _scale = scale;
            _mode = mode;
            return this;
        }
        public Calculator RoundingAlways(bool roundingAlways)
        {
            this._roundingAlways = roundingAlways;
            return this;
        }

        public Calculator Add(decimal v)
        {
            _value = Rounding(_value + v);
            return this;
        }
        private decimal Rounding(decimal v, bool force = false)
        {
            if (!force && !_roundingAlways) return v;
            decimal power = (decimal)Math.Pow(10, _scale);
            switch (_mode)
            {
                case RoundingMode.Down:
                    return Math.Floor(_value * power) / power;
                case RoundingMode.Up:
                    return Math.Ceiling(_value * power) / power;
                case RoundingMode.HalfUp:
                    return Math.Round(_value, _scale, MidpointRounding.AwayFromZero);
                case RoundingMode.HalfEven:
                    return Math.Round(_value, _scale, MidpointRounding.ToEven);
                default:
                    return v;
            }
        }
        public Calculator Subtract(decimal v)
        {
            _value = Rounding(_value - (decimal)v);
            return this;
        }
        public Calculator Multiply(decimal v)
        {
            _value = Rounding(_value * (decimal)v);
            return this;
        }
        public Calculator DivideBy(decimal v)
        {
            _value = Rounding(_value / (decimal)v);
            return this;
        }

        public decimal DecimalValue()
        {
            return Rounding(_value, true);
        }

        public static Calculator Init()
        {
            return new Calculator(0m);
        }
        public static Calculator Of(decimal value)
        {
            return new Calculator(value);
        }

    }

    //<summary>端数種別</summary>
    public enum RoundingMode
    {
        /** 切り捨て*/
        Down,
        /** 切り上げ */
        Up,
        /** 四捨五入 */
        HalfUp,
        /** 銀行丸め */
        HalfEven
    }
}