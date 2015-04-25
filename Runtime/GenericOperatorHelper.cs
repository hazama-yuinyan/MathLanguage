using System;
using System.Linq.Expressions;

namespace CalculatorCompetition.Backend.Runtime
{
    using UnaryOperator = Func<ParameterExpression, UnaryExpression>;
    using BinaryOperator = Func<ParameterExpression, ParameterExpression, BinaryExpression>;

    /// <summary>
    /// Holds the basic operator operations for a generic type T.
    /// </summary>
    public static class GenericOperatorHelper<T>
    {
        public static ParameterExpression x = Expression.Parameter(typeof(T), "x");
        public static ParameterExpression y = Expression.Parameter(typeof(T), "y");

        public static Func<T, T, T> Add = Lambda(Expression.Add);
        public static Func<T, T, T> Subtract = Lambda(Expression.Subtract);
        public static Func<T, T, T> Multiply = Lambda(Expression.Multiply);
        public static Func<T, T, T> Divide = Lambda(Expression.Divide);
        public static Func<T, T> Negate = LambdaUnary(Expression.Negate);

        static Func<T, T> LambdaUnary(UnaryOperator op)
        {
            return Expression.Lambda<Func<T, T>>(
                op(x),
                x).Compile();
        }

        static Func<T, T, T> Lambda(BinaryOperator op)
        {
            return Expression.Lambda<Func<T, T, T>>(
                op(x, y),
                x, y).Compile();
        }

        public static T DoNegation(T a)
        {
            return GenericOperatorHelper<T>.Negate(a);
        }

        public static T DoAddition(T x, T y)
        {
            return GenericOperatorHelper<T>.Add(x, y);
        }

        public static T DoSubtraction(T x, T y)
        {
            return GenericOperatorHelper<T>.Subtract(x, y);
        }

        public static T DoMultiplication(T x, T y)
        {
            return GenericOperatorHelper<T>.Multiply(x, y);
        }

        public static T DoDivision(T x, T y)
        {
            return GenericOperatorHelper<T>.Divide(x, y);
        }
    }

    /// <summary>
    /// Holds the basic operator operations for generic types T and U.
    /// Assumes that the return type is always T.
    /// So that means that you should provide more general type on the first parameter
    /// to avoid loss of information and retain accuracy.
    /// </summary>
    public static class GenericOperatorHelper<T, U>
    {
        public static ParameterExpression x = Expression.Parameter(typeof(T), "x");
        public static ParameterExpression y = Expression.Parameter(typeof(U), "y");

        public static Func<T, U, T> Add = Lambda(Expression.Add);
        public static Func<T, U, T> Subtract = Lambda(Expression.Subtract);
        public static Func<T, U, T> Multiply = Lambda(Expression.Multiply);
        public static Func<T, U, T> Divide = Lambda(Expression.Divide);

        static Func<T, U, T> Lambda(BinaryOperator op)
        {
            return Expression.Lambda<Func<T, U, T>>(
                op(x, y),
                x, y).Compile();
        }

        public static T DoAddition(T x, U y)
        {
            return GenericOperatorHelper<T, U>.Add(x, y);
        }

        public static T DoSubtraction(T x, U y)
        {
            return GenericOperatorHelper<T, U>.Subtract(x, y);
        }

        /*public static U DoSubtraction(U x, T y)
        {
            return GenericOperatorHelper<T, U>.Subtract(x, y);
        }*/

        public static T DoMultiplication(T x, U y)
        {
            return GenericOperatorHelper<T, U>.Multiply(x, y);
        }

        public static T DoDivision(T x, U y)
        {
            return GenericOperatorHelper<T, U>.Divide(x, y);
        }

        /*public static U DoDivision(U x, T y)
        {
            return GenericOperatorHelper<T, U>.Divide(x, y);
        }*/
    }
}

