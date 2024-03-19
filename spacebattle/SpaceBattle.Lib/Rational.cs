namespace SpaceBattle.Lib
{
    public class Rational
    {
        private int _numerator;
        private int _denominator;

        public Rational(int numerator, int denominator = 360)
        {
            if (denominator == 0)
            {
                throw new Exception();
            }

            _numerator = numerator;
            _denominator = denominator;
            CorrectFormRational();

        }
        private void CorrectFormRational()
        {

            if (_denominator * _numerator < 0)
            {
                _numerator = Math.Abs(_numerator) * (-1);
                _denominator = Math.Abs(_denominator);
            }
            else
            {
                _numerator = Math.Abs(_numerator);
                _denominator = Math.Abs(_denominator);
            }

            var gcd = GreatestCommonDivisor(_numerator, _denominator);

            _numerator /= gcd;
            _denominator /= gcd;

            _numerator = _numerator % _denominator;
        }
        public static Rational operator +(Rational first, Rational second)
        {
            var numerator = first._numerator * second._denominator + first._denominator * second._numerator;
            var denominator = first._denominator * second._denominator;
            return new Rational(numerator, denominator);
        }
        private static int GreatestCommonDivisor(int numerator, int denominator)
        {
            numerator = Math.Abs(numerator);
            denominator = Math.Abs(denominator);
            while (denominator != 0)
            {
                var temp = denominator;
                denominator = numerator % denominator;
                numerator = temp;
            }

            return numerator;
        }
        public override bool Equals(object? obj)
        {

            if (obj == null || typeof(Rational) != obj.GetType())
            {
                return false;
            }

            return ((Rational)obj)._numerator == _numerator && ((Rational)obj)._denominator == _denominator;
        }

        public override int GetHashCode()
        {

            return HashCode.Combine(_numerator, _denominator);
        }
    }
}
