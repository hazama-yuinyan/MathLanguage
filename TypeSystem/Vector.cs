using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculatorCompetition.Backend.Runtime;

namespace CalculatorCompetition.Backend.TypeSystem
{
    /// <summary>
    /// Represents the mathematical vector type.
    /// </summary>
    public struct Vector<T>
    {
        T[] elems;

        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        public int Dimensions{
            get{return elems.Length;}
        }

        public Vector(params T[] elements)
        {
            elems = elements;
        }

        public Vector(IEnumerable<T> elements)
        {
            elems = elements.ToArray();
        }

        public Vector(int dimensions)
        {
            elems = new T[dimensions];
            for(int i = 0; i < dimensions; ++i)
                elems[i] = default(T);
        }

        #region Operator overloads
        public static Vector<T> operator+(Vector<T> u, Vector<T> v)
        {
            return u.Add(v);
        }

        public static Vector<T> operator-(Vector<T> a)
        {
            T[] negated = new T[a.Dimensions];
            for(int i = 0; i < a.Dimensions; ++i)
                negated[i] = GenericOperatorHelper<T>.DoNegation(a.elems[i]);

            return new Vector<T>(negated);
        }

        public static Vector<T> operator-(Vector<T> u, Vector<T> v)
        {
            return u.Subtract(v);
        }

        public static Vector<T> operator*(Vector<T> u, Vector<T> v)
        {
            return u.CrossProduct(v);
        }

        public static Vector<T> operator*(int t, Vector<T> v)
        {
            T[] elems = new T[v.Dimensions];
            for(int i = 0; i < v.Dimensions; ++i)
                elems[i] = GenericOperatorHelper<T, int>.DoMultiplication(v.elems[i], t);

            return new Vector<T>(elems);
        }

        public static Vector<T> operator*(double t, Vector<T> v)
        {
            T[] elems = new T[v.Dimensions];
            for(int i = 0; i < v.Dimensions; ++i)
                elems[i] = GenericOperatorHelper<T, double>.DoMultiplication(v.elems[i], t);

            return new Vector<T>(elems);
        }

        public static Vector<T> operator*(Vector<T> v, int t)
        {
            T[] elems = new T[v.Dimensions];
            for(int i = 0; i < v.Dimensions; ++i)
                elems[i] = GenericOperatorHelper<T, int>.DoMultiplication(v.elems[i], t);

            return new Vector<T>(elems);
        }

        public static Vector<T> operator*(Vector<T> v, double t)
        {
            T[] elems = new T[v.Dimensions];
            for(int i = 0; i < v.Dimensions; ++i)
                elems[i] = GenericOperatorHelper<T, double>.DoMultiplication(v.elems[i], t);

            return new Vector<T>(elems);
        }

        public static Vector<T> operator/(Vector<T> v, int t)
        {
            T[] elems = new T[v.Dimensions];
            for(int i = 0; i < v.Dimensions; ++i)
                elems[i] = GenericOperatorHelper<T, int>.DoDivision(v.elems[i], t);

            return new Vector<T>(elems);
        }

        public static Vector<T> operator/(Vector<T> v, double t)
        {
            T[] elems = new T[v.Dimensions];
            for(int i = 0; i < v.Dimensions; ++i)
                elems[i] = GenericOperatorHelper<T, double>.DoDivision(v.elems[i], t);

            return new Vector<T>(elems);
        }
        #endregion

        #region Operator implementations
        public Vector<T> Add<U>(Vector<U> v)
        {
            if(Dimensions != v.Dimensions){
                throw new InvalidOperationException(
                    string.Format("Can not add {0}-ary vector {1} to {2}-ary vector {3}",
                        Dimensions, this, v.Dimensions, v
                    )
                );
            }

            T[] result = new T[Dimensions];
            for(int i = 0; i < Dimensions; ++i)
                result[i] = GenericOperatorHelper<T, U>.DoAddition(elems[i], v.elems[i]);

            return new Vector<T>(result);
        }

        public Vector<T> Subtract<U>(Vector<U> v)
        {
            if(Dimensions != v.Dimensions){
                throw new InvalidOperationException(
                    string.Format("Can not subtract {2}-ary vector {3} from {0}-ary vector {1}",
                        Dimensions, this, v.Dimensions, v
                    )
                );
            }

            T[] result = new T[Dimensions];
            for(int i = 0; i < Dimensions; ++i)
                result[i] = GenericOperatorHelper<T, U>.DoSubtraction(elems[i], v.elems[i]);

            return new Vector<T>(result);
        }

        public T DotProduct<U>(Vector<U> a)
        {
            if(Dimensions != a.Dimensions){
                throw new InvalidOperationException(
                    string.Format("Can not calculate the dot product of {0} and {1}",
                        this, a
                    )
                );
            }

            T result = default(T);
            for(int i = 0; i < Dimensions; ++i){
                T tmp = GenericOperatorHelper<T, U>.DoMultiplication(elems[i], a.elems[i]);
                result = GenericOperatorHelper<T>.DoAddition(result, tmp);
            }

            return result;
        }

        public Vector<T> CrossProduct<U>(Vector<U> b)
        {
            if(Dimensions != 3 || b.Dimensions != 3){
                throw new InvalidOperationException(
                    string.Format("Can not calculate the cross product of {0} and {1}",
                        this, b
                    )
                );
            }

            T[] results = new T[]{
                GenericOperatorHelper<T, T>.DoSubtraction(
                    GenericOperatorHelper<T, U>.DoMultiplication(elems[1], b.elems[2]),
                    GenericOperatorHelper<T, U>.DoMultiplication(elems[2], b.elems[1])
                ),
                GenericOperatorHelper<T, T>.DoSubtraction(
                    GenericOperatorHelper<T, U>.DoMultiplication(elems[2], b.elems[0]),
                    GenericOperatorHelper<T, U>.DoMultiplication(elems[0], b.elems[2])
                ),
                GenericOperatorHelper<T, T>.DoSubtraction(
                    GenericOperatorHelper<T, U>.DoMultiplication(elems[0], b.elems[1]),
                    GenericOperatorHelper<T, U>.DoMultiplication(elems[1], b.elems[0])
                )
            };

            return new Vector<T>(results);
        }
        #endregion

        public override bool Equals(object obj)
        {
            var o = (Vector<T>)obj;

            if(Dimensions != o.Dimensions)
                return false;

            for(int i = 0; i < Dimensions; ++i){
                if(!elems[i].Equals(o.elems[i]))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("(");
            bool first = true;
            foreach(T elem in elems){
                if(first)
                    first = false;
                else
                    builder.Append(" ");

                builder.Append(elem);
            }
            builder.Append(")");

            return builder.ToString();
        }
    }
}

