using System;
using System.Linq;
using CalculatorCompetition.Backend.Runtime;
using System.Collections.Generic;
using System.Text;

namespace CalculatorCompetition.Backend.TypeSystem
{
    /// <summary>
    /// Represents the matrix in mathematical sense.
    /// In short, a matrix. That's it. That's as simple as it is and it doesn't imply
    /// any meanings beyond that.
    /// </summary>
    public struct Matrix<T>
    {
        int rows, cols;
        T[] elems;

        /// <summary>
        /// Creates a new matrix with all elements set to default(T).
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="cols">The number of cols.</param>
        public Matrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            elems = new T[rows * cols];
            for(int i = 0; i < rows * cols; ++i)
                elems[i] = default(T);
        }

        /// <summary>
        /// Creates a new matrix with specified initial elements.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="cols">The number of cols.</param>
        /// <param name="elements">Elements.</param>
        public Matrix(int rows, int cols, IEnumerable<T> elements)
        {
            this.rows = rows;
            this.cols = cols;
            elems = elements.ToArray();
        }

        /// <summary>
        /// Creates a new matrix with specified initial elements.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="cols">The number of cols.</param>
        /// <param name="elements">Elements.</param>
        public Matrix(int rows, int cols, params T[] elements)
        {
            this.rows = rows;
            this.cols = cols;
            elems = elements;
        }

        /// <summary>
        /// Creates a n by n identity matrix.
        /// </summary>
        /// <returns>The identity.</returns>
        /// <param name="rank">Rank.</param>
        public static Matrix<T> CreateIdentity(int rank)
        {
            T[] elems = new T[rank * rank];
            for(int i = 0; i < rank * rank; i += (rank + 1))
                elems[i] = GenericOperatorHelper<T, double>.DoAddition(default(T), 1.0);

            return new Matrix<T>(rank, rank, elems);
        }

        #region Operator overloads
        public static Matrix<T> operator+(Matrix<T> lhs, Matrix<T> rhs)
        {
            return lhs.Add(rhs);
        }

        public static Matrix<T> operator-(Matrix<T> x)
        {
            for(int i = 0; i < x.cols * x.rows; ++i)
                x.elems[i] = GenericOperatorHelper<T>.DoNegation(x.elems[i]);

            return x;
        }

        public static Matrix<T> operator-(Matrix<T> lhs, Matrix<T> rhs)
        {
            return lhs.Subtract(rhs);
        }

        /// Multiply matrices `lhs` and `rhs` element by element.
        /// So this is not a mathematical operation.
        /// <param name="lhs">Lhs.</param>
        /// <param name="rhs">Rhs.</param>
        public static Matrix<T> operator*(Matrix<T> lhs, Matrix<T> rhs)
        {
            if(lhs.rows != rhs.rows && lhs.cols != rhs.cols){
                throw new InvalidOperationException(
                    string.Format("The size of {0} must match that of {1}.",
                        lhs, rhs
                    )
                );
            }

            T[] results = new T[lhs.rows * lhs.cols];
            for(int i = 0; i < lhs.cols * lhs.rows; ++i)
                results[i] = GenericOperatorHelper<T>.DoMultiplication(lhs.elems[i], rhs.elems[i]);

            return new Matrix<T>(lhs.rows, lhs.cols, results);
        }

        public static Matrix<T> operator*(int t, Matrix<T> m)
        {
            T[] result = new T[m.cols * m.rows];
            for(int i = 0; i < m.cols * m.rows; ++i)
                result[i] = GenericOperatorHelper<T, int>.DoMultiplication(m.elems[i], t);

            return new Matrix<T>(m.rows, m.cols, result);
        }

        public static Matrix<T> operator*(double t, Matrix<T> m)
        {
            T[] result = new T[m.cols * m.rows];
            for(int i = 0; i < m.cols * m.rows; ++i)
                result[i] = GenericOperatorHelper<T, double>.DoMultiplication(m.elems[i], t);

            return new Matrix<T>(m.rows, m.cols, result);
        }

        public static Matrix<T> operator*(Matrix<T> m, int t)
        {
            T[] result = new T[m.cols * m.rows];
            for(int i = 0; i < m.cols * m.rows; ++i)
                result[i] = GenericOperatorHelper<T, int>.DoMultiplication(m.elems[i], t);

            return new Matrix<T>(m.rows, m.cols, result);
        }

        public static Matrix<T> operator*(Matrix<T> m, double t)
        {
            T[] result = new T[m.cols * m.rows];
            for(int i = 0; i < m.cols * m.rows; ++i)
                result[i] = GenericOperatorHelper<T, double>.DoMultiplication(m.elems[i], t);

            return new Matrix<T>(m.rows, m.cols, result);
        }

        public static Matrix<T> operator/(Matrix<T> lhs, Matrix<T> rhs)
        {
            if(lhs.rows != rhs.rows || lhs.cols != rhs.cols){
                throw new InvalidOperationException(
                    string.Format("The size of {0} must match that of {1}",
                        lhs, rhs
                    )
                );
            }

            T[] results = new T[lhs.rows * lhs.cols];
            for(int i = 0; i < lhs.rows * lhs.cols; ++i)
                results[i] = GenericOperatorHelper<T>.DoDivision(lhs.elems[i], rhs.elems[i]);

            return new Matrix<T>(lhs.rows, lhs.cols, results);
        }
        #endregion

        /// <summary>
        /// Creates and returns a new matrix with all the elements casted to the specified type.
        /// </summary>
        /// <typeparam name="U">The type to which all the elements will be casted.</typeparam>
        public Matrix<U> Convert<U>()
        {
            var converted = elems.Cast<U>();
            return new Matrix<U>(rows, cols, converted);
        }

        /// <summary>
        /// Transposes the matrix and returns a new one.
        /// </summary>
        public Matrix<T> Transpose()
        {
            T[] transposed = new T[rows * cols];
            for(int i = 0; i < rows * cols; ++i)
                transposed[i] = elems[i % cols + i / rows];

            return new Matrix<T>(rows, cols, transposed);
        }

        /// <summary>
        /// Computes the inverse of the matrix.
        /// </summary>
        public Matrix<T> Invert()
        {
            if(rows != cols){
                throw new InvalidOperationException(
                    string.Format("Can not invert a {0} x {1} matrix",
                        rows, cols
                    )
                );
            }

            Matrix<T> inverted = CreateIdentity(rows);
            // Calculate the inverse of `this` using the gaussian elimination
            for(int i = 0; i < rows; ++i){
                double pivot = System.Convert.ToDouble(elems[i * cols + i]);
                inverted.elems[i * cols + i] = GenericOperatorHelper<T, double>.DoDivision(inverted.elems[i * cols + i], pivot);
                for(int j = 0; j < rows; ++j){
                    if(j != i){
                        double d = System.Convert.ToDouble(elems[j * cols + j]);
                        for(int k = 0; k < cols; ++k){
                            inverted.elems[j * cols + k] = 
                                GenericOperatorHelper<T, double>.DoSubtraction(
                                    inverted.elems[j * cols + k],
                                    GenericOperatorHelper<double, T>.DoMultiplication(d, inverted.elems[i * cols + i])
                                );
                        }
                    }
                }
            }

            return inverted;
        }

        #region Operator implementations
        /*public Matrix<T> CrossProduct(Matrix<T> a)
        {

        }*/

        public Matrix<T> Add<U>(Matrix<U> matrix)
        {
            if(rows != matrix.rows || cols != matrix.cols)
                throw new InvalidOperationException("Operands must have the same size!");

            T[] result = new T[rows * cols];
            for(int i = 0; i < rows * cols; ++i)
                result[i] = GenericOperatorHelper<T, U>.DoAddition(elems[i], matrix.elems[i]);

            return new Matrix<T>(rows, cols, result);
        }

        public Matrix<T> Subtract<U>(Matrix<U> matrix)
        {
            if(rows != matrix.rows || cols != matrix.cols)
                throw new InvalidOperationException("Operands must have the same size!");

            T[] result = new T[rows * cols];
            for(int i = 0; i < rows * cols; ++i)
                result[i] = GenericOperatorHelper<T, U>.DoSubtraction(elems[i], matrix.elems[i]);

            return new Matrix<T>(rows, cols, result);
        }

        /// <summary>
        /// Computes the dot product of 2 matrices.
        /// </summary>
        /// <returns>The product.</returns>
        /// <param name="a">The alpha component.</param>
        public Matrix<T> DotProduct(Matrix<T> b)
        {
            if(cols != b.rows){
                throw new InvalidOperationException(
                    string.Format("The number of columns on left-hand-side and the number of rows on right-hand-side must match to compute the product")
                );
            }

            T[] results = new T[cols * b.rows];
            for(int i = 0; i < cols * b.rows; ++i){
                T tmp = default(T);
                for(int j = 0; j < cols; ++j){
                    T lhs = elems[i / cols * cols + j];
                    T rhs = b.elems[i % cols + cols * j];
                    tmp = GenericOperatorHelper<T>.DoAddition(tmp,
                        GenericOperatorHelper<T>.DoMultiplication(lhs, rhs)
                    );
                }

                results[i] = tmp;
            }

            return new Matrix<T>(rows, b.rows, results);
        }
        #endregion

        public override bool Equals(object obj)
        {
            var o = (Matrix<T>)obj;

            if(rows != o.rows || cols != o.cols)
                return false;

            for(int i = 0; i < rows * cols; ++i){
                if(!elems[i].Equals(o.elems[i]))
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[");
            bool first = true;
            for(int i = 0; i < rows * cols; ++i){
                if(first)
                    first = false;
                else
                    sb.AppendFormat("{0} ", (i % cols == 0) ? "," : "");

                sb.Append(elems[i]);
            }
            sb.Append("]");

            return sb.ToString();
        }
    }
}

