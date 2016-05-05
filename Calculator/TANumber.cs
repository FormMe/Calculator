using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public abstract class Number
    {
        public override string ToString()
        {
            return base.ToString();
        }

        public abstract bool EqZero();
        public abstract Number Sqr();
        public abstract Number Rev();

        public static Number operator +(Number l, Number r) => l.Plus(r);
        protected abstract Number Plus(object n);

        public static Number operator -(Number l, Number r) => l.Minus(r);
        protected abstract Number Minus(object n);

        public static Number operator *(Number l, Number r) => l.Mult(r);
        protected abstract Number Mult(object n);

        public static Number operator /(Number l, Number r) => l.Div(r);
        protected abstract Number Div(object n);
        public static Number operator -(Number obj) => obj.Deny();
        protected abstract Number Deny();
        public static bool operator ==(Number l, Number r)
        {
            if (ReferenceEquals(l, r))
                return true;

            if (ReferenceEquals(l, null) || ReferenceEquals(r, null))
                return false;

            return l.Equals(r);
        }
        public static bool operator !=(Number l, Number r) => !(l == r);
    }

    public class Real : Number
    {
        double num;
        public Real(double n)
        {
            num = n;
        }

        public Real(string n)
        {
            double.TryParse(n, out num);
        }

        public override string ToString()
        {
            return num.ToString();
        }

        public override bool EqZero()
        {
            return num == 0;
        }

        public override Number Sqr()
        {
            return new Real(num * num);
        }

        public override Number Rev()
        {
            return new Real(1 / num);
        }

        protected override Number Plus(object obj)
        {
            var other = obj as Real;

            if (other == null)
                throw new Exception("Plus error");

            return new Real(num + other.num);
        }

        protected override Number Minus(object obj)
        {
            var other = obj as Real;

            if (other == null)
                throw new Exception("Minus error");

            return new Real(num - other.num);
        }

        protected override Number Mult(object obj)
        {
            var other = obj as Real;

            if (other == null)
                throw new Exception("Multiply error");

            return new Real(num * other.num);
        }

        protected override Number Div(object obj)
        {
            var other = obj as Real;

            if (other == null)
                throw new Exception("Multiply error");

            return new Real(num / other.num);
        }

        protected override Number Deny()
        {
            return new Real(-num);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Real;

            if (other == null)
                throw new Exception("Multiply error");

            return num == other.num;
        }
    }

    public class Frac : Number
    {
        BigInteger num, den;

        public Frac(BigInteger n, BigInteger d)
        {
            num = n;
            den = d;
        }

        public Frac(string n)
        {
            var nn = n.Split('/').ToArray();
            BigInteger.TryParse(nn[0], out num);
            BigInteger.TryParse(nn[1], out den);
        }

        public override string ToString()
        {
            if (den == 1) return num.ToString();
            if (EqZero()) return "0";
            return num + "/" + den;
        }

        public override bool EqZero()
        {
            return num == 0;
        }

        public override Number Sqr()
        {
            return new Frac(num * num, den * den);
        }

        public override Number Rev()
        {
            return new Frac(den, num);
        }

        protected override Number Plus(object obj)
        {
            var other = obj as Frac;

            if (other == null)
                throw new Exception("Plus error");
            Reduce();
            other.Reduce();

            if (den == other.den)
                return new Frac(num + other.num, den).Reduce();
            if (den > other.den)
            {
                if (den / other.den == 0)
                    return new Frac(num + other.num * (den % other.den), den).Reduce();
            }
            else
            {
                if (other.den / den == 0)
                    return new Frac(other.num + num * (other.den % den), other.den).Reduce();
            }
            return new Frac(num * other.den + other.num * den, den * other.den).Reduce();

        }

        protected override Number Minus(object obj)
        {
            var other = obj as Frac;

            if (other == null)
                throw new Exception("Minus error");
            Reduce();
            other.Reduce();

            if (den == other.den)
                return new Frac(num - other.num, den).Reduce();
            if (den > other.den)
            {
                if (den / other.den == 0)
                    return new Frac(num - other.num * (den % other.den), den).Reduce();
            }
            else
            {
                if (other.den / den == 0)
                    return new Frac(other.num - num * (other.den % den), other.den).Reduce();
            }
            return new Frac(num * other.den - other.num * den, den * other.den).Reduce();
        }

        protected override Number Mult(object obj)
        {
            var other = obj as Frac;

            if (other == null)
                throw new Exception("Multiply error");

            Reduce();
            other.Reduce();

            return new Frac(num * other.num, den * other.den).Reduce();
        }

        protected override Number Div(object obj)
        {
            var other = obj as Frac;

            if (other == null)
                throw new Exception("Div error");

            Reduce();
            other.Reduce();

            return new Frac(num * other.den, den * other.num).Reduce();
        }

        protected override Number Deny()
        {
            return new Frac(-num, den);
        }

        private Number Reduce()
        {
            BigInteger gcd;
            do
            {
                gcd = BigInteger.GreatestCommonDivisor(num, den);
                num /= gcd;
                den /= gcd;
            } while (gcd != 1);
            return new Frac(num, den);
        }
    }

    public class Complex : Number
    {
        Real Re;
        Real Im;

        public Complex(double re, double im)
        {
            Im = new Real(im);
            Re = new Real(re);
        }

        public Complex(Real re, Real im)
        {
            Im = im;
            Re = re;
        }

        public Complex(string n)
        {

        }
        public override string ToString()
        {
            if (EqZero()) return "0";
            return Re + " + " + Im + "i";
        }


        public override bool EqZero()
        {
            return Re.EqZero() && Im.EqZero();
        }

        public override Number Sqr()
        {
            return new Complex((Real)(Re.Sqr() - Im.Sqr()), (Real)((new Real(2)) * Re * Im));
        }

        public override Number Rev()
        {
            var ab = Re.Sqr() + Im.Sqr();
            return new Complex((Real)(Re / ab), (Real)(-Im / ab));
        }

        protected override Number Plus(object obj)
        {
            var other = obj as Complex;

            if (other == null)
                throw new Exception("Plus error");

            return new Complex((Real)(Re + other.Re), (Real)(Im + other.Im));
        }

        protected override Number Minus(object obj)
        {
            var other = obj as Complex;

            if (other == null)
                throw new Exception("Minus error");

            return new Complex((Real)(Re - other.Re), (Real)(Im - other.Im));
        }

        protected override Number Mult(object obj)
        {
            var other = obj as Complex;

            if (other == null)
                throw new Exception("Multiply error");

            return new Complex((Real)(Re * other.Re - Im * other.Im),
                                (Real)(Im * other.Re + Re * other.Im));
        }

        protected override Number Div(object obj)
        {
            var other = obj as Complex;

            if (other == null)
                throw new Exception("Div error");
            var cd = other.Re.Sqr() + other.Im.Sqr();
            return new Complex((Real)((Re * other.Re + Im * other.Im) / cd),
                                (Real)((Im * other.Re - Re * other.Im) / cd));
        }

        protected override Number Deny()
        {
            return new Complex((Real)(-Re), (Real)(-Im));
        }

        public override bool Equals(object obj)
        {
            var other = obj as Complex;

            if (other == null)
                throw new Exception("Multiply error");

            return Re == other.Re && Im == other.Im;
        }
    }

}
