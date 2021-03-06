﻿using System;
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

        protected int _base;

        public virtual int Base
        {
            get { return _base; }
            set { _base = value; }
        }

        public abstract bool EqZero();
        public abstract Number Sqr();
        public virtual Number Sqrt()
        {
            return this;
        }

        public abstract bool IsNaN();
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
        public Real(double n, int b)
        {
            Base = b;
            num = n;
        }
        public Real(string n, int b)
        {
            Base = b;
            if (Base == 10) double.TryParse(n, out num);
            else num = ConverterP10.DoTrasfer(n, b);
        }

        public override string ToString()
        {
            return Base == 10 ? num.ToString() : Converter10p.DoTrasfer(num, Base);
        }

        public override bool EqZero()
        {
            return num == 0;
        }

        public override Number Sqr()
        {
            return new Real(num * num, Base);
        }

        public override Number Sqrt()
        {
            return new Real(Math.Sqrt(num), Base);
        }

        public override bool IsNaN()
        {
            return double.IsNaN(num);
        }

        public override Number Rev()
        {
            return new Real(1 / num, Base);
        }

        protected override Number Plus(object obj)
        {
            var other = obj as Real;

            if (other == null)
                throw new Exception("Plus error");

            return new Real(num + other.num, Base);
        }

        protected override Number Minus(object obj)
        {
            var other = obj as Real;

            if (other == null)
                throw new Exception("Minus error");

            return new Real(num - other.num, Base);
        }

        protected override Number Mult(object obj)
        {
            var other = obj as Real;

            if (other == null)
                throw new Exception("Multiply error");

            return new Real(num * other.num, Base);
        }

        protected override Number Div(object obj)
        {
            var other = obj as Real;

            if (other == null)
                throw new Exception("Multiply error");

            return new Real(num / other.num, Base);
        }

        protected override Number Deny()
        {
            return new Real(-num, Base);
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

        public Frac(string n, int b)
        {
            if (string.IsNullOrEmpty(n))
            {
                num = 0;
                den = 1;
                return;
            }
            var splitedNumber = n.Split('/').ToArray();
            if (b == 10)
            {
                BigInteger.TryParse(splitedNumber[0], out num);
                if (splitedNumber.Length == 1 ||
                    string.IsNullOrEmpty(splitedNumber[1])) den = 1;
                else BigInteger.TryParse(splitedNumber[1], out den);
            }
            else
            {
                num = (BigInteger)ConverterP10.DoTrasfer(splitedNumber[0], b);
                if (splitedNumber.Length == 1 ||
                    string.IsNullOrEmpty(splitedNumber[1])) den = 1;
                else den = (BigInteger)ConverterP10.DoTrasfer(splitedNumber[1], b);
            }
        }
        public override string ToString()
        {
            if (den == 0) return "∞";
            return Base == 0 ? num + "/" + den : Converter10p.DoTrasfer((double)num, Base) + "/" + Converter10p.DoTrasfer((double)den, Base);
        }

        public override bool EqZero()
        {
            return num == 0;
        }

        public override Number Sqr()
        {
            return new Frac(num * num, den * den).Reduce();
        }


        public override bool IsNaN()
        {
            return false;
        }


        public override Number Rev()
        {
            if (num < 0)
            {
                num = -num;
                den = -den;
            }
            return new Frac(den, num).Reduce();
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
            var gcd = BigInteger.GreatestCommonDivisor(num, den);
            return new Frac(num / gcd, den / gcd);
        }
    }

    public class Complex : Number
    {
        Real Re;
        Real Im;

        public override int Base
        {
            get { return _base; }
            set
            {
                _base = value;
                if (Re == null || Im == null) return;
                Re.Base = value;
                Im.Base = value;
            }
        }

        public Complex(double re, double im, int b)
        {
            Base = b;
            Im = new Real(im, Base);
            Re = new Real(re, Base);
        }

        private Complex(Real re, Real im, int b)
        {
            Base = b;
            Im = im;
            Re = re;
        }

        public Complex(string n, int b)
        {
            if (n.Contains("+"))
            {
                var splitedNumber = n.Split('+').ToArray();
                Re = new Real(splitedNumber[0], b);
                Im = new Real(splitedNumber[1].Substring(0, splitedNumber[1].Length - 1), b);
            }
            else
            {
                Re = new Real(n, b);
                Im = new Real(0, b);
            }
            Base = b;
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
            return new Complex((Real)(Re.Sqr() - Im.Sqr()), (Real)((new Real(2, Base)) * Re * Im), Base);
        }

        public override Number Sqrt()
        {
            throw new NotImplementedException();
        }

        public override bool IsNaN()
        {
            return Re.IsNaN() || Re.IsNaN();
        }

        public override Number Rev()
        {
            var ab = Re.Sqr() + Im.Sqr();
            return new Complex((Real)(Re / ab), (Real)(-Im / ab), Base);
        }

        protected override Number Plus(object obj)
        {
            var other = obj as Complex;

            if (other == null)
                throw new Exception("Plus error");

            return new Complex((Real)(Re + other.Re), (Real)(Im + other.Im), Base);
        }

        protected override Number Minus(object obj)
        {
            var other = obj as Complex;

            if (other == null)
                throw new Exception("Minus error");

            return new Complex((Real)(Re - other.Re), (Real)(Im - other.Im), Base);
        }

        protected override Number Mult(object obj)
        {
            var other = obj as Complex;

            if (other == null)
                throw new Exception("Multiply error");

            return new Complex((Real)(Re * other.Re - Im * other.Im),
                                (Real)(Im * other.Re + Re * other.Im), Base);
        }

        protected override Number Div(object obj)
        {
            var other = obj as Complex;

            if (other == null)
                throw new Exception("Div error");
            var cd = other.Re.Sqr() + other.Im.Sqr();
            return new Complex((Real)((Re * other.Re + Im * other.Im) / cd),
                                (Real)((Im * other.Re - Re * other.Im) / cd), Base);
        }

        protected override Number Deny()
        {
            return new Complex((Real)(-Re), (Real)(-Im), Base);
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
